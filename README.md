# 프로젝트 소개

- 개발엔진 : Unity5(2020.3.30)
- 개발기간 : 2022.04 ~2023.09
- 출      시 : 2023.10
- 게임설명 : 프랑켄슈타인의 원작소설을 바탕으로하며, 갈바니즘 이론(전기능력)을 주된 기믹으로 스테이지를 클리어하는 잠입, 퍼즐, 액션 게임

<a href="https://store.steampowered.com/app/2516530?snr=5000_5100__">
    <img src="https://github.com/user-attachments/assets/89ff3bfc-3643-4dbb-a75f-a680b7273785" alt="img_steam" width="705" height="157">
</a>

# 프로젝트 이력

- 공모전
    - 2022.08. ➡️ BIGS 출품 : 방구석 인디게임쇼 2022 온라인 전시
    - 2023.08. ➡️ BIC 전시 : 부산인디게임페스티벌 2023 오프라인 전시
- 크라우드 펀딩
    - 2022.12. ➡️ 텀블벅 크라우드 펀딩 : 크라우드펀딩을 통해 목표금액의 665% 달성

# 담당 파트

- 게임 플레이 메인 프로그래머
    - 플레이어 조작, 스킬, 성장
    - 게임 기믹 장치
    - 기반 시스템 : Game Manager, ObjectPool System, Database, Steam API

- 쉐이더 프로그래머
    - 스텐실 쉐이더

# 기술설명

<blockquote>
구현 기술에 대한 설명이 필요한 주된 메커니즘들을 설명합니다

[스킬](https://github.com/mw08081/Modern_Prometheus#%EC%8A%A4%ED%82%AC) : 뇌운, 블루프린트

[장치](https://github.com/mw08081/Modern_Prometheus#%EC%9E%A5%EC%B9%98) : 램프, 로렌치니, 파워뱅크

[기반시스템](https://github.com/mw08081/Modern_Prometheus#%EA%B8%B0%EB%B0%98%EC%8B%9C%EC%8A%A4%ED%85%9C) : 스텐실 쉐이더, 오브젝트 풀링
  
</blockquote>
<br/>
<br/>
<br/>
  

## 스킬

### I. 뇌운 - 전기구름 투사체를 포물선으로 던지는 스킬

<blockquote>
	
구현목표 

- 2D 탑다운 뷰의 게임에서 포물선 궤적과 투사체 투척을 표현

<img width="666" height="345" alt="thunderStorm" src="https://github.com/user-attachments/assets/4ad89098-e8ac-4da9-8c64-98ff3ad359b7" />

구현방법 

- 라인렌더러를 이용하여 플레이어와 마우스사이 거리를 반주기로 하는 Sin 그래프를 통해 구현
- x 좌표 : 임의의 값(30)으로 구간을 설정 (좌표 개수 : 30개)
- y 좌표 : Sin그래프의 x 좌표로부터 얻어지는 값

<img width="853" height="404" alt="sin" src="https://github.com/user-attachments/assets/fa830659-44e8-4eab-a16a-ab84258bc559" />


코드

```csharp
 void DrawParabolicSkillTrajectory()
 {
     dist = Vector3.Distance(transform.position, GameManager.Instance.GetCurrentSceneT<InGameScene>().SkillSystem.worldMPos);
     for (float i = 0; i < lr.positionCount ; i++)
     {
		     float b = GameManager.Instance.GetCurrentSceneT<InGameScene>().
																										     SkillSystem.worldMPos;
         cachedPos = 
			         Vector3.Lerp(transform.position, b, (i + 2) / (pointCnt + 2));         
         // +2를 한 이유 : 발에서부터가 아닌 손에서부터이니까
         cachedPos.y += Mathf.Min(dist, sinMaxScale) *
									          Mathf.Sin(Mathf.PI / (pointCnt + 2) * (i + 2));       
         //sinScale * Mathf.Sin(Mathf.PI / dist * dist / pointCnt * i); 
         //dist : Vector3.Distance(transform.position, Camera.main.ScreenToWorldPoint(screnMPos));
         lr.SetPosition((int)i, cachedPos);

     }
 }
```

구현결과

<img width="1126" height="416" alt="Image" src="https://github.com/user-attachments/assets/81b524c8-2341-45f2-a3aa-65bac714c12e" />

<img width="1126" height="416" alt="Image" src="https://github.com/user-attachments/assets/091a491f-3c14-4779-98d3-9a5cdce3ae34" />

</blockquote>

### II. 갈바니즘 - **플레이어가 기록한 경로를 따라 개구리를 이동시키는 스킬**


<blockquote>

구현목표 

- update()함수에 의해 매 프레임 기록되는 포인트 사이에서 방향 벡터 계산이 어려움
→ 특정 거리마다 포인트를 기록하도록 변경
- 플레이어가 기록한 경로를 가시적으로 표현하는데 사용되는 포인트를 빈번하게 생성/파괴할 경우 부하가 생김
→ 포인트를 오브젝트풀로 관리함
- 스킬 시전간에 플레이어가 반드시 마우스 포인트를 개구리에서부터 시작할 보장이 없었다
→ 개구리와 최초 마우스 클릭 지점 사이를 보간하여 중간 경로 삽입

<img width="820" height="289" alt="image" src="https://github.com/user-attachments/assets/fdab0a1e-2888-482e-b267-901468204286" />

구현방법 

- 라인 렌더러의 좌표도 너무 많을 경우 부하가 생기기때문에 특정 거리(0.5f)씩 마다 좌표를 찍는 방식으로 변경
    
    ```csharp
    void DrawMovingTrajectory()
    {
        if ( /* ...(조건A) */  && Vector3.Distance(lastPos, mousePos) >= TRAJECTORYINTERVAL && /* ...(조건C) */ )    //TRAJECTORYINTERVAL = 0.5f;
        {
            //최대 사용량을 초과할 경우 더 이상 그려지지 않는다
            if (currentDT >= allowanceDT) 
            {
                return; 
            }
    
            //특정 거리(0.5f)마다 점 추가 시퀀스    
            AddTrajectoryPoint(Vector3.zero);
        }
    }
    ```
    

- 화살표 오브젝트를 오브젝트 풀링하여 관리하면 빈번한 생성과 파괴를 피할 수 있다.
    
    ```csharp
    Queue<GameObject> pointPool = new Queue<GameObject>();          //포인트 풀
    
    //화살표 오브젝트 풀 생성
    void GeneratePointPool()
    {
        for (int i = 0; i < poolCount; i++)
        {
            pointPool.Enqueue(GeneratePoint());
        }
    }
    
    //포인트 생성 함수
    GameObject GeneratePoint()
    {
        GameObject go = Instantiate(point, transform);
        go.SetActive(false);
        
        return go;
    }
    
    //포인트 요청
    GameObject GetPointFromQueue()
    {
        //예외처리를 통해 포인트가 없는 경우를 대비
        try
        {
            return pointPool.Dequeue();
        }
        catch
        {
            pointPool.Enqueue(GeneratePoint());             //추가 생산하여 인큐,
            return pointPool.Dequeue();                     //그리고 반환
        }
    }
    
    ```
    

- 개구리와 최초 마우스 입력 위치가 0.5f 이상일 경우 그 사이를 0.5f단위로 보간하여 라인렌더러에 좌표를 삽입한다
    
    ```csharp
    bool InterpolateTrajectory()
    {
        // ...
        float interpolDist = Vector3.Distance(mousePos, critterPos);                               //개구리와 최초 마우스 입력 위치의 거리 계산
        if (interpolDist < 0.6f) return true;                                                      //0.6미만이면 보간할 필요 없음(취소조건)
    
        //이후 거리에 따른 경로 보간
        Vector3 interpolPos;
        
        int interpolCnt = (int)(interpolDist / TRAJECTORYINTERVAL);             //보간 좌표 개수
        for (float i = 1; i < interpolCnt; i++)
        {
            interpolPos = Vector3.Lerp(critterPos, mousePos, i / interpolCnt);              //Lerp()를 이용한 보간
            AddTrajectoryPoint(interpolPos);
            //if( 보간 취소 조건 ) return false;                              //더이상 경로를 그릴 수 없다
        }
        return true;
    }
    ```
    

구현결과  
![gv2](https://github.com/mw08081/Modern_Prometheus/assets/58582985/870a7f5f-48dd-4015-a8e4-fd3fdd55e69d)  << 시전
![nointerpol](https://github.com/mw08081/Modern_Prometheus/assets/58582985/74505b0d-7608-4c9e-aef5-4c09bb6febdd)  << 개구리 이동: 보간 전  
![interpol](https://github.com/mw08081/Modern_Prometheus/assets/58582985/fbeb97f2-af2a-4424-8c23-c942400556e6)    << 개구리 이동: 보간 후  


</blockquote>

### III. 블루프린트 - 맵의 구조를 스캔하는 스킬


<blockquote>

구현목표 

- 특정한 오브젝트를 오브젝트 타입 색상으로 하이라이트하여 표시
- 그 외의 구조물은 더 어두운 검은색으로 표시

<img width="538" height="286" alt="000002447967919491120240121014413536" src="https://github.com/user-attachments/assets/be39239e-a9a4-4caf-b670-926604e51d86" />

구현방법 

- 특정 색상으로 표시하고자 하는 오브젝트에 스텐실 버퍼의 REF값 수정후 머테리얼이 적용
- 스킬 사용시 스텐실 버퍼 값과 비교하여 동일한 버퍼 값일 경우 필터의 색상을 채우는 필터를 활성화
- 배경과 이 외의 오브젝트는 스텐실 버퍼값이 그대로(REF 0) ➡️ REF 0에 검은색 적용

코드

```csharp
Pass {
		Stencil
		{
		    Ref [_StencilValue]
		    Comp Greater
		    Pass Replace
		    //Fail IncrSat
		}
		//...
}
```

```csharp
Pass
{
    Stencil
    {
        Ref[_StencilValue]
        Comp Equal
    }
    /...
}
```

구현결과

<img width="855" height="467" alt="Image" src="https://github.com/user-attachments/assets/e6f44427-4e98-4088-98be-bded68455f87" />


</blockquote>
  
<br/>
<br/>
<br/>
  

## 장치

### I. 램프 - 적에게 플레이어를 노출시키는 광원


<blockquote>

구현목표 

- 원형 콜라이더를 사용하기때문에, 벽 뒤에 있는 플레이어는 광원 밖 판정 구현

구현방법 

- 벡터의 외적을 이용하여 플레이어와 램프의 위치관계 구분 후 광원 적용여부 판정

<img width="803" height="519" alt="000002447967320251220231107044823052" src="https://github.com/user-attachments/assets/789f0c19-ad73-45ac-9133-83e9f7e5ad3e" />

코드

```csharp
Lamp lamp = collision.transform.parent.GetComponent<Lamp>();
if (lamp is Lamp)
{
    Vector3 vectorPivot = collision.transform.parent.GetComponentInChildren<ParticleSystem>().transform.position;

    if (lamp.isWallLamp == false ||                 //wallLamp가 아니면 바로 활성화
        (lamp.isWallLamp == true && 
        Vector3.Cross((playerCenterPos - vectorPivot).normalized, Vector3.right).z > 0))    //wallLamp면 벡터 조건 검사가 필요함
    {
        IsVisible = true;
        ps.Updating_IsActiveReady();
    }
}
```

구현결과

<img width="701" height="276" alt="Image" src="https://github.com/user-attachments/assets/da73dd4b-63f6-4e8b-8f79-8573525282c2" />


</blockquote>

### II. 로렌치니 - 플레이어를 검출하여 시설을 방어하는 레이더 장치


<blockquote>

구현목표 

- 플레이어 검출 레이더를 시각적으로 표현
- 최대 검출거리 안에 구조물이 있는 경우, 레이더 길이 조정

<img width="968" height="576" alt="000002447967320174820231107044338938" src="https://github.com/user-attachments/assets/ba8aea22-47ff-4d21-9ff2-12179ede7ecb" />

구현방법 

- 라인렌더러의 끝 좌표를 삼각함수 좌표(Cos **θ**, Sin **θ**)로 적용하여 회전 표현
- 구조물에 레이더가 부딪히는 경우 반지름의 길이를 구조물까지의 거리로 변경

<img width="496" height="296" alt="image 1" src="https://github.com/user-attachments/assets/92d072c6-77b7-4983-877e-8dfe6545ba97" />

코드

```csharp
// 라인렌더러를 삼각함수로 회전
void RotatingDetectRay()
{
    float angle = curAngleTime / DEFAULT_SONARSPEED * 360;

    curRayPos.x = transform.position.x + 
										    Mathf.Cos(-angle * Mathf.Deg2Rad) * curSonarRadius;
    curRayPos.y = transform.position.y + 
										    Mathf.Sin(-angle * Mathf.Deg2Rad) * curSonarRadius;

    line.SetPosition(1, curRayPos);
}

// 레이케스팅으로 플레이어 검출
void DetectOnRay()
{
    //레이케스팅을 정중앙(transform.position)에서부터 시작하지 않기위한 시작점 배치
    RaycastHit2D hit = 
				    Physics2D.Raycast(transform.position + (curRayPos - transform.position).normalized * 0.8f, 
													    (curRayPos - transform.position).normalized,
													     DEFAULT_SONARRADIUS,
													     detectLayer);
    try
    {
        //충돌할 경우 레이 감소
        if (hit.collider != null)
        {
            //레이 감소
            curSonarRadius = Vector3.Distance(hit.point, transform.position);

            //레이 감소상태에서 플레이어일 경우 이벤트 발생
            if (hit.collider.CompareTag("Player") && canDetect == true)
            
            /....
```

구현결과

<img width="482" height="397" alt="Image" src="https://github.com/user-attachments/assets/da7f3493-91a5-4b6c-b084-26038299ae34" />


</blockquote>

### III. 파워뱅크 - 여러 장치에 전력을 공급하는 장치


<blockquote>

구현목표 

- 커스텀 에디터를 통해 씬에 배치된 장치들을 파워뱅크에 연결하는 작업 소요시간 단축

구현방법 

- 커스텀 에디터를 이용하여 레벨 작업 소요시간 단축

코드

```csharp
// 에디터에 점 찍는 함수
private void OnSceneGUI()
{
    if (powerLineConnector.isFinWriting == true) return;

    if (Event.current.type != EventType.MouseDown || powerLineConnector.isWriting == false) return;

    var mousePosition = Event.current.mousePosition * EditorGUIUtility.pixelsPerPoint;
    mousePosition.y = Camera.current.pixelHeight - mousePosition.y;

    var worldPosTmp = Camera.current.ScreenPointToRay(mousePosition).origin;
    var worldPos = new Vector3(Mathf.RoundToInt(worldPosTmp.x), Mathf.RoundToInt(worldPosTmp.y), 0);

    powerLineConnector.AddPoint(worldPos);
}

// 찍힌 점을 기반으로 라인렌더러 연결
public void Create_Line()
{
    isFinWriting = true;
    int lineCnt = 0;

    try
    {
        powerSource = GetComponent<PowerSourceDevice>().powerSource;

        if (powerSource == null) throw new System.Exception("Power Source Binding First");
    }
    catch(System.Exception e)
    {
#if DEV
        Debug.LogWarning(e.Message);
#endif
        return;
    }

    try
    {
        lineCnt = disConnectPoints.Count;
    }
    finally
    {
        for (int i = 0; i <= lineCnt; i++)
        {
            lineIdx = i;
            GameObject powerLine = new GameObject("PowerLine_" + i);

            Init_LineRenedere(powerLine);
            StartCoroutine(Connect_PowerLine(powerLine));
        }
    }
}

void Init_LineRenedere(GameObject go)
{
    go.tag = "PowerLine";
    LineRenderer lr = go.AddComponent<LineRenderer>();

    lr.gameObject.transform.SetParent(transform);

    Color lineColor;
    ColorUtility.TryParseHtmlString("#7F8C8C", out lineColor);
    lr.startColor = lineColor;
    lr.endColor = lineColor;

    lr.numCornerVertices = 5;
    lr.numCapVertices = 5;

    lr.startWidth = 0.1f;
    lr.endWidth = 0.1f;

    lr.sortingLayerID = SortingLayer.NameToID("Platform");
    lr.sortingOrder = 2;

    lr.material = Resources.Load<Material>("PublicResources/Materials/Stencil_PowerLine");
}

IEnumerator Connect_PowerLine(GameObject powerLine)
{
    LineRenderer lr = powerLine.GetComponent<LineRenderer>();
    Vector3[] points = connectPoints.ToArray();

    int posIdx = 0;
    int disConnectPointIdx = lineIdx;
    disConnectPointIdx = disConnectPointIdx >= disConnectPoints.Count ? disConnectPoints.Count - 1 : disConnectPointIdx;

    //Point Setting
    stPos = lineIdx == 0 ? transform.position : disConnectPoints[lineIdx - 1].disConnectEnd;

    //Start Point
    lr.positionCount = 1;
    lr.SetPosition(posIdx++, stPos);

    //Middle Point
    int i = connectPoints.FindIndex(e => e == stPos);
    i = i < 0 ? 0 : i;
    for (; i < points.Length; i++)
    {
        lr.positionCount++;
        lr.SetPosition(posIdx++, points[i]);

        if (disConnectPoints.Count != 0 &&                                               //끊김포인트가 있고
            points[i] == disConnectPoints[disConnectPointIdx].disConnectStart)        //마지막 포인트가 끊김 시작포인트일 경우
        {
            yield break;
        }
    }

    //End Point
    lr.positionCount++;
    lr.SetPosition(posIdx++, powerSource.transform.position);

    yield return null;

#if UNITY_EDITOR
    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
#endif
}
```

구현결과


<img width="950" height="346" alt="Image" src="https://github.com/user-attachments/assets/7f3cc8ad-05f2-4ac3-8889-2218e7867e7b" />

<img width="765" height="346" alt="Image" src="https://github.com/user-attachments/assets/17cd193d-15ce-458a-886f-2a27daf8f097" />


</blockquote>
  
<br/>
<br/>
<br/>
  

## 기반시스템

### I. 스텐실 쉐이더


<blockquote>

구현목표 

- 벽 뒤의 플레이어를 시각적으로 표시

구현방법 

- 벽보다 먼저 그려진 스텐실 버퍼값이 클 경우, 벽 버퍼값으로 수정
- 벽 버퍼값과 벽 필터 버퍼값이 같으면 필터적용

코드

```csharp
// 플레이어 스텐실
Stencil 
{
	Ref[_StencilRef]   // Ref 11
	Comp[_StencilComp]  //Always
	Pass Replace
}

// 벽 스텐실
Stencil
{
    Ref [_StencilValue] // Ref 10
	  Comp Less           // 플레이어 버퍼를 만나서 10으로 변경
    Pass Replace
}

// 필터 스텐실
Stencil
{
    Ref[_StencilValue]  // Ref 10
    Comp Equal          // Ref 10인 경우 필터 색상 적용
}
```

구현결과

<img width="1815" height="599" alt="000002447967319877020231107042358064" src="https://github.com/user-attachments/assets/2a9ddf1f-0cce-4738-bb07-2a1855585bee" />


</blockquote>

### II. 오브젝트풀링 - 주로 사용하는 사용하는 오브젝트를 미리 생성


<blockquote>

구현목표 

- 주로 사용하는 오브젝트들을 미리 생성하여, 잦은 생성/파괴로 인한 가비지 컬렉터 호출을 방지
- 오브젝트 풀이 비어있는 경우, 추가 생성 로직 구현

구현방법 

- Queue<T>를 형식의 풀을 이용함으로써 사용가능한 오브젝트 탐색 시간을 제거

코드

```csharp
// Enum 으로 이펙트의 종류를 구분하여, List< Pool > 생성
// Pool은 Queue<T>로 생성하여, 사용가능한 오브젝트를 탐색하는 과정 제거
void GenerateSkillEffectPool()
{
    skillEffectPool = new List<Queue<GameObject>>();
    for (int i = 0; i < skillEffectPrefabs.Length; i++)
    {
        skillEffectPool.Add(new Queue<GameObject>());
        for (int j = 0; j < skillEffectPrefabs[i].cnt; j++)
        {
            GameObject go = 
		            Instantiate(skillEffectPrefabs[i].skillEffectPrefab, 
							            skillEffectParent
		            );           //Instantiate Prefab 
		            
            go.GetComponent<SkillEffect>().Initialize_SkillEffect();
            skillEffectPool[i].Enqueue(go);
        }
    }
}

// 요청하는 이팩트의 타입을 Enum으로 구분
public void ServeSkillEffect(SkillEffectCode skillEffectCode, Vector2 spawnPos)
{
		// 오브젝트가 없는 경우, 추가요청
    if (skillEffectPool[(int)skillEffectCode].Count == 0)
    {
        skillEffectPool[(int)skillEffectCode].Enqueue(
				        Additionally_GenerateSkillEffect(skillEffectCode)
        );
    }

    GameObject go = skillEffectPool[(int)skillEffectCode].Dequeue();
    go.GetComponent<SkillEffect>().Dequeue_SkillEffect(spawnPos);
}

// 오브젝트 추가 요청시, 생성하여 반환
GameObject Additionally_GenerateSkillEffect(SkillEffectCode skillEffectCode)
{
    GameObject go = Instantiate(skillEffectPrefabs[(int)skillEffectCode].skillEffectPrefab, skillEffectParent);           //Instantiate Prefab
    go.GetComponent<SkillEffect>().Initialize_SkillEffect();                                                          //Hide Prefab

    return go;
}

// --------------------------------

// 이펙트에서 스스로 초기화 후, 멤버변수로 갖고있던 이펙트 타입을 매개변수로 넘김
Initialize_SkillEffect();
GameManager.Instance.GetCurrentSceneT<InGameScene>().
					PoolSystem.ReturnSkillEffect(
									gameObject, skillEffectCode
					);

// 오브젝트 회수 시, 오브젝트 타입 Enum과 함께 반환
public void ReturnSkillEffect(GameObject gameObject, SkillEffectCode skillEffectCode)
{
    skillEffectPool[(int)skillEffectCode].Enqueue(gameObject);
}

```

구현결과

<img width="869" height="346" alt="Image" src="https://github.com/user-attachments/assets/608bcd44-317e-488c-86ca-abea32250300" />


</blockquote>
  
<br/>
<br/>
<br/>
  
0. TeamWork
   - Git Manager : 프로젝트의 버전관리를 깃 브랜치를 이용하여 작업 주도
1. Optimazing  
   - object pooling  
        - skill effect prefab 오브젝트 풀링
        - circle Size cached : 빈번한 스킬관련 원형 오브젝트를 생성할 경우, 오브젝트 생성/파괴
        - movingTrajectory : 개구리 이동경로를 가시적으로 표시하기 위한 화살표 오브젝트를 풀링
   - 프로파일링 : 리지드바디, 콜라이더 바디타입  별로 프로젝트 부하를 프로파일링 함  
2. Math  
   - LineRenderer : Draw Parabolic skill trajectory(Math - Mathf.sin) 
   - Get Deg from V3 (Mathf, Quaternion) : Quaternion 사원수를 이용하여 각도를 벡터로 계산
   - Get Position relation between two object : 벡터의 외적을 이용하여 두 오브젝트간의 위치관계를 확인
3. Tech  
   - UI 에셋 구매하여 남의 코드를 읽고 맛에 따라 적용 : 구매한 에셋에 대해서 게임에 맞게 적용
4. New Tech  
   - Shdaer : Stencil Buffer를 이용하여 StencilShader 를 제작함으로써, 보이지 않는 플레이어를 효과적으료 표현
   - LineRenderer : ExecuteInEditMode on Script 를 이용하여 유니티 에디터 상에서 전선 연결 작업을 용이하게 함
   - coroutine 동기로드 / async - await 비동기 를 이용하여 드라이브의 엑셀 데이터 로딩을 용이하게 함
5. Creativity  
   - Player FOV Expansion : 마우스를 이용하여 플레이어의 시야 확장을 하는 방식을 유니티에서 제공하는 시네머신을 이용하여 간편하게 구현함
  
## 에피소드
1. 스텐실을 위한 3주간 고민
2. 변수 변경은 통일된 장소에서,, 함부로, 별도로 변경을 하면 에러발생시 파악이 어렵다
3. BIC , 버닝비버, BIGS 등 전시
4. 펀딩
5. 기획팀과의 소통을 통해 이상한점, 어색한점 등 의견을 피력
6. A가 작업한것을 B가 업그레이드를 위해 그냥 가져다가 쓰니까 오류가 발생. (책상의 playerAttacked Layer, playerAttacked Layer를 감자하는 타임라인트리거)

7. 스텐실 노력
에셋을 사서 입맛에 맞게 작업하기
최xx과의 충돌
미흡한 의사소통으로 인한 이미지 리소스 오류
최적화에 대한 학습
수학의 활용(외적,내적 삼각함수: 뇌운 궤적,로렌치니 원형회전,개구리점프, 역삼: 백터로 각도계산-스킬이펙트배치시 시작점과 끝점의 각도계산, 갈바니즘 포인트 배치시-이전 점과의 벡터를 구하여 각도계산)
LINE_RENDERER, json, excel, RayCasting, OverLap, 

## 참고자료
- RayCast unsing Speific Layer Filltering   ←
    - https://dallcom-forever2620.tistory.com/18
- Get Vector using angle(need to adjust Quaternion's(x,y,z) & StdVector)    ←
    - https://wiseraintown.tistory.com/entry/Transform-%EA%B0%81%EB%8F%84%EC%97%90-%EB%94%B0%EB%A5%B8-Object-%EB%B0%A9%ED%96%A5-%EA%B5%AC%ED%95%98%EA%B8%B0
    - `Quaternion.Euler(0, 0, x) * Vector3.right;`
- Event on Editor & Get WorldPosition using Event.mousePosition
    - https://answers.unity.com/questions/381630/listen-for-a-key-in-edit-mode.html
    - https://answers.unity.com/questions/877467/how-to-get-mouse-position-in-world-space-when-in-e.html
- LineRenederer
    - https://beatchoi.github.io/unity3d/basics/2020/12/14/LineRenderer2/
- Create Unity Editor custom Button to execute some Function
    - https://wergia.tistory.com/165
- How to save variable in custom editor in unity
    - https://postpiglet.netlify.app/posts/unity-prefabscenemark/
    - https://forum.unity.com/threads/custom-editor-variables-not-saving.513406/
    - https://stackoverflow.com/questions/61238628/cannot-save-a-variable-of-a-editor-script
    - https://assetstore.unity.com/packages/tools/utilities/play-mode-save-177452
- Particle System Playing Stop immediately
    - pc.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
- Draw Bezier Curves Line
    - https://stackoverflow.com/questions/43547886/is-it-really-so-difficult-to-draw-smooth-lines-in-unity
- System.Enum.GetValue(typeof(EnumName)).Length  
- gameObject.trasnform.childCount  
- scripting API - https://docs.unity3d.com/ScriptReference
    - SortingOrder : using UnityEngine.Rendering
    - Tilemap : using UnityEngine.Tilemap
    - Light2D : using UnityEngine.Experimental.Rendering.LWRP

  
## 작업 일지
### 55 ~ 62주차(03.28. ~ 05.22.)
1. 스킬 구현
   - [x] 블랙아웃 스킬 구현  
   - [x] 과부화 스킬 구현
   - [x] 번개 스킬 구현
   - [x] 천둥 스킬 구현

2. 스킬 업그레이드
   - [x] 업그레이드 분석
   - [x] 플레시 레벨 0, 1 분리
   - [ ] ~픽락 레벨 1~
   - [x] 테이저 스킬업 구현
   - [x] 천둥 스킬업 구현
   - [x] 갈바니즘 스킬업 구현
   - [x] 뇌운 스킬업 part1(duration) 구현
   - [x] 뇌운 스킬업 part2(부여) 구현  

3. 스킬 안정화
    - [x] 스킬 UI 안정화
    - [x] 퀵힐 안정화
    
### 63 ~ 66주차(5.23. ~ 6.19.)
1. 스킬 안정화
    - [ ] ~스킬 테이블 수정~
    - [ ] ~스킬 스트링 테이블 수정~
    - [x] 과부화 이미지 적용
    - [x] 천둥 적용(협업)
    
2. 장치 구현
    - [x] 증기발생기
    - [x] 번개장벽
    - [x] 두줄 번개장벽
    
3. 로렌치니 구현
    - [x] 상태 관리
    - [x] 회전 관리
    - [x] 공격 관리
  
### 67 ~ 68주차(6.20. ~ 7.3.)
1. 장치구현
    - [x] 지뢰
    - [x] 무한동력 장치

### 69, 70주차(7.4. ~ 7.17.)
1. 버그 수정
2. INGAME_Canvas
    - [x] 패널 이동 관련 로직 수정
3. Skill Tree UI
    - [x] UI 만들기
    - [x] 스크립팅

### 71주차(7.18. ~ 7.24.)
1. 버그수정
2. Skill Tree UI
    - [x] 스킬버튼 선택 이펙트
    - [x] 스킬상태 및 스킬 습득 버튼
    - [x] 스킬 습득 모달 윈도우

### 72주차(7.25. ~ 7.31.)
1. 버그 수정
   - [ ] 스테이지 obtain → 글로벌 obtain
   - [x] 책상버그
   - [ ] 개구리 알림 적용
   - [x] 슬라이딩 도어 적용
2. Skill panel
   - [x] 경험치 슬라이더
   - [x] 데이터 베이스 연동
   - [x] 환경 전력 흡수 페널티 제거
   - [x] 스킬포인트 : player.ic → skillPointSys
   - [x] 스킬 습득에 따른 저장
3. Skill Panel buff
   - [x] 버프 획득 메커니즘
   - [x] 버프 에니메이션
4. 협업
   - [x] 대화시스템 로딩
   
### 73주차(8.1. ~ 8.7.)
1. 버그 수정
   - [x] 스테이지 obtain → 글로벌 obtain
   - [x] 영구 환경 전력
   - [x] 개구리 알림 적용
2. 전기 방벽
   - [x] 플랙서블 전기방벽
3. 협업
   - [x] 대화시스템 로딩

### 73주차(8.8. ~ 8.14.)
1. 버그 수정
   - [x] 램프 스텐실 소팅
2. 브라이드 스파인
   - [x] 스파인 및 에니메이션 
3. 리소스 업데이트
   - [ ] resurrection Device
   - [ ] skill panel text

### 74 ~ 76주차(8.15. ~ 9.4.)
1. 빌드
2. BIC
3. 무한 버그... 기억도 안나

### 77 ~ 79주차(9.5. ~ 9.25.)
1. 지도 UI

### 80 ~ 81주차(9.26. ~ 10.09.)
1. clear Window
2. QA
  
### 1주차(3.16. ~ 3.22.)
1. 플레이어 8방향 이동 구현  
세부내용 : 플레이어의 이동 구현
    - [x] 플레이어 에셋 탐색
    - [x] 플레이어 스크립트 제작
    - [x] 플레이어 컨트롤러 제작
    - [x] 플레이어 인터렉트컨트롤 제작
    - [x] 플레이어 애니메이션 최적화 및 리터치
    
2. 플레이어 테스트 환경 구축  
세부내용 : 플레이어 테스트를 위한 씬과 타일맵 제작
    - [x] PlayerTestScene 제작
    - [x] PlayerTestScene위에 타일맵 배치
    
3. GameManager 제작  
세부내용 : 최상위 매니저 GameManager를 싱글톤으로 제작하여 매니저 환경 구축
    - [x] 싱글톤 GameManager 제작

### 2주차(3.23. ~ 3.29.)
1. Iter , IMetal 전기 전이 메카니즘  
세부내용 : IWater, IMetal 인터페이스 구현하여 Qinteract를 통해 전기 전이 메카니즘 구현
    - [x] IWater interface
    - [x] IMetal interface
    - [x] interactQ 전기 부여 메카니즘 구현

2. IBio 전기 전이 메카니즘  
세부내용 : 전기전이 인터렉트 Q를 이용하여 IBio(Enemy) 테이즈 스킬 구현
    - [x] IBio interface
    - [x] interactQ 테이즈 스킬 구현

3. Interact Q 상호작용 추가 구현  
세부내용 : Interact Q의 상호작용 우선순위 부여, 테이즈 스킬 조건 추가
    - [x] 우선순위 메카니즘 구현
    - [x] 테이즈 스킬 조건 추가
    
4. IElectricity - elecPoint 전기 흡수 메카니즘  
세부내용 : IElectricity interface 제작과 그에 따른 InteractE 흡수 메카니즘
    - [x] IElectricity interface 제작
    - [x] Interact E  흡수 메카니즘 구현

5. IElectricity - ElecPoint GameObject 흡수 Effect  
세부내용 : ElecPoint GameObject 흡수 Effect구현
    - [x] ElecPoint GameObject 제작
    - [x] Interact E를 통한 흡수 Effect 구현(FadeOut)
    
6. 데이터베이스 구축    
세부내용 : 싱글톤을 이용하여 데이터베이스 구축
    - [x] 싱글톤 데이터 베이스 구축
    
7. 엑셀 데이터 로더와 데이터베이스 연동  
세부내용 : 엑셀 데이터 로더를 통해 SkillDataTable 데이터를 스크립터블 오브젝트로 크롤링하여 데이터베이스와 연동
    - [x] 데이터베이스와 크롤링 스크립터블 오브젝트 연동

### 3주차(3.30. ~ 4.5.)
1. 플레이어 상호작용 범위 개선  
세부내용 : 플레이어의 상호작용 범위를 몇가지 테스트용으로 구현  
    - [x] 플레이어의 마지막 입력 벡터 산출
    - [x] 마지막 입력벡터에 따른 반호 상호작용 범위 구현
    - [x] 마지막 입력벡터에 따른 사각형 상호작용 범위 구현
    - [x] 사각형 범위 디버그 조정 변수 설정

2. 오브젝트 풀 시스템 구현  
세부내용 : 전기이펙트를 위한 오브젝트 풀 구현(추후 다른 오브젝트 풀도 추가 예정)  
    - [x] 오브젝트 풀 구현
    - [x] 풀에 전기 이펙트 풀 생성
    
3. 전기이펙트 라이프 타임 관련 스크립트 생성  
세부내용 : 라이프타임에 따른 오브젝트풀 삽입 메카니즘 구현  
    - [x] 라이프타임에 따른 오브젝트풀 반환(삽입) 메카니즘 구현  

4. 물 객체와 전기 전이 상호작용 메카니즘 구현  
세부내용 : 전기 전이 상호작용을 통해 물 위에 전기를 발생  
    - [x] 물 객체 모델 3개 제작
    - [x] 물 객체 전기 전이 메카니즘 구현

### 4주차(4.6. ~ 4.12.)
1. 물 객체와 상호작용 추가작업   
세부내용 : 물 객체와의 상호작용 추가 작업  
    - [x] 전이 전력 소멸 후 전력전이 포인트 초기화
    - [x] 물 객체 위에서의 물 객체와의 상호작용 불가 메카니즘 구현
    - [ ] 중첩전기의 lifeTime 증가 구현  
    - [x] 플레이어 직접전이는 isTransited를 검사하지 않는 버그 수정
    - [x] isTransitedClear Method 버그 수정

2. 물 객체와의 상호작용 테스트  
세부내용 : 다양한 예시를 통한 물 객체 전기전이 테스트  
    - [x] 물 객체 모델 수령 
    - [x] 물 객체 제작
    - [x] 테스트  
    
3. 금속 객체와의 상호작용 구현  
세부내용 : Metal 전기 전이 구현  
ㄴ사전작업 : IMove, Interact F
    - [x] Metal 전기전이 메카니즘 구현
    - [x] 전이가능포인트 초기화 구현
    - [x] 중첩전기 lifeTime 증가 구현
    - [x] 종류별 전기 프리팹 제작
    - [x] 방향별 전기 프리팹 연동
    - [x] Y프레임 파이프 전기 전이 버그 수정
    - [x] 플레이어 직접전이는 isTransited를 검사하지 않는 버그 수정
    - [x] isTransitedClear Method 버그 수정
    - [ ] ~~IMove Interface 구현~~    
    - [ ] ~~Interact F 구현~~  
    - [ ] ~~오브젝트 이동 시, 오브젝트 포인트 재계산 메카니즘 구현~~ 

4. 상호작용 업그레이드   
세부내용 : Water/Metal의 각각의 포인트에서 다른 Interactable Object를 만났을때, 전기 전이가 이뤄지는 메카니즘을 생성  
    - [x] 오브젝트 포인터 별 다른 오브젝트 체크  
    - [x] 물 객체에서의 새로운 오브젝트 발견 시, 전이 메카니즘 구현(mainBody/Dervied 상관없이 전이 발생) 
    - [x] 금속 객체에서의 새로운 오브젝트 발견 시, 전이 메카니즘 구현
    
5. 전기 흡수 메카니즘  
세부내용 : 발생시킨 전이 전기와 그 주변 인근 전기 흡수 메카니즘   
    - [x] 흡수된 전이전기의 주변 전이전기 계산
    - [x] 전기 흡수 메카니즘 개선
    
### 5주차(4.13. ~ 4.19.)
1. 플레이어 이동 관련 버그 수정  
세부내용 : 실행 pc에 따라 이동속도가 상이한 현상을 수정
    - [x] 실행 pc에 따른 이동속도 버그 수정

2. 전기전이 상호작용 개선  
세부내용 : 파이프에서 물로의 전기전이가 비정상적인 경우에 대해 해결  
    - [x] 코드 정리
    - [x] 버그 수정
    - [x] 전이 가능 포인트 초기화 메카니즘 개선
    - [x] 동일 속성 전도체간 전이 메카니즘 구현(금속우선) 
    
3. IMetal Object 프리팹화  
세부내용 : 별다른 설정없이 파이프를 배치  
    - [x] 파이프 형태 프리팹화
    - [x] 자동 피벗 탐색 후 자동 좌표 설정
    - [x] 요청 파이프 프리팹 제작 
    - [ ] ~~Resizable 파이프 제작~~
    
4. 스킬 클래스 생성  
세부 내용 : 스킬 실행에 관여할 스킬 클래스 제작   
    - [x] 스킬 클래스 구현
    
5. 테이저  
세부내용 : 테이저 스킬 보완
    - [x] 테이저 벡터 컨디션 업데이트
    - [x] 스킬 클래스 쿨타임 매니저 연동

6. 이펙트 추가    
세부내용 : 테이저, 천둥, 흡수 ,픽락 시전 이펙트
    - [x] 부여 이펙트
    - [x] 스킬 사용실패 이펙트
    - [x] 테이저 이펙트  
    
7. 레벨 디자인 씬 셋팅  
세부내용 : 기획자를 위한 레벨 디자인씬 기반 시스템 구축
    - [x] 기반시스템 구축

### 6주차(4.20. ~ 4.26.)
1. 스킬 이펙트 결정  
세부내용 : 기획자회의를 통한 이펙트 선정  
    - [x] 실패 이펙트 확정
    - [x] 전이 이펙트 확정
    - [x] 테이저 이펙트 확정
    - [x] 픽락 이펙트 확정
    - [x] 마비 이펙트 확정

2. 스킬 이팩트 적용  
세부내용 : 회의를 통해 결정된 이펙트 적용
    - [x] 실패 이펙트 적용
    - [x] 전이 이펙트 적용
    - [x] 전이 2 water 이펙트 적용
    - [x] 마비 이펙트 적용
    - [x] 테이저 이펙트 적용
    - [x] 픽락온락 이펙트 적용
    - [x] 픽락 이펙트 적용
    - [x] 마비 이펙트 적용

3. 문 오브젝트  
세부내용 : IOpen Interface Door 생성
    - [x] IOpen 인터페이스 생성
    
4. 열쇠 오브젝트
세부내용 : IObtain Interface - key Item 구현
    - [x] IObtain Interface - key Item 구현
    
5. 문과 열쇠 상호작용
세부내용 : 열쇠를 통한 문 잠금 해제
    - [x] 잠금해제

6. 픽락  
세부내용 : 상호작용 PickLock 스킬 구현    
    - [x] PickLock Debug 연출 구현  

7. 전이 사이클 UI  
세부내용 : 인게임에 전이 사이클 수 표시  
    - [x] KeyHoldCycleDebuger 구현
    - [x] 개발 회의를 통한 기획진 설정
    - [x] UI 
  
### 7주차(4.27. ~ 5.3.)
1. 천둥   
세부내용 : 기능 구현을 통한 초안 구현  
    - [ ] ~~초안~~
    - [ ] ~~소음 시스템~~
    
2. PlayerCenterPos 변수 전격 변경  
세부 내용 : 함수의 매개변수로 전달되던 playerCenterPos를 구현된 GameManager.Instance.Player.PlayerCenterPos로 변경하기
    - [x] 변경

3. 스킬 이펙트 다듬기  
세부내용 : 어색한 이펙트 정리 및 디렉토리 정리
    - [x] 테이저
    - [x] 픽락1
    - [x] 픽락2

4. 게임 데이터 저장  
세부내용 : 체크포인트를 통한 게임 데이터 저장 요소와 진행상황  
    - [x] 플레이어 데이터 : 체력, 마나, 
    - [x] 보유스킬
    - [x] 수집목록
    - [x] 스테이지 오브젝트 - 세이브포인트
    - [x] 스테이지 오브젝트 - 열쇠
    - [x] 스테이지 오브젝트 - 문
    - [x] 스테이지 오브젝트 - 에너미
    - [ ] ~~스테이지 해금~~
    - [ ] ~~정제전력~~
    - [ ] ~~도전과제~~
    
5. 게임 데이터 로드  
세부내용 : 게임 데이터 로드 구현 진행상황  
    - [x] 플레이어 데이터 : 체력, 마나 (#1. 리스폰)
    - [x] 플레이어 데이터 : 체력, 마나 (#2. 재시작)
    - [x] 보유스킬
    - [x] 수집목록 
    - [x] 스테이지 오브젝트 - 세이브포인트(Only ReStart)
    - [x] 스테이지 오브젝트 - 열쇠
    - [x] 스테이지 오브젝트 - 문
    - [ ] ~~스테이지 해금~~
    - [ ] ~~정제전력~~
    - [ ] ~~도전과제~~
    
### 8주차(5.4. ~ 5.10.)
1. 게임 데이터 로드  
세부내용 : Json 게임 데이터 로드 구현 진행상황  
    - 플레이어
        - [x] 플레이어 데이터 : 위치
        - [x] 플레이어 데이터 : 체력, 마나 (#1. 리스폰)
        - [x] 플레이어 데이터 : 체력, 마나 (#2. 재시작)
    - 스킬
        - [x] 보유스킬
        - [x] 스킬쿨다운
    - 컬렉션
        - [x] 수집목록 
    - 스테이지 오브젝트
        - [x] 스테이지 오브젝트 - 세이브포인트(Only ReStart)
        - [x] 스테이지 오브젝트 - 열쇠
        - [x] 스테이지 오브젝트 - 문
    - 스테이지 정보
        - [x] 스테이지 해금 정보
    - [ ] ~~정제전력~~
    - [ ] ~~도전과제~~
        
2. 스테이지 클리어 포인트  
세부내용 : 스테이지 클리어 포인트 생성과 구현
    - [x] 스테이지 클리어 포인트 구현
    - [x] 스테이지 해금 업데이트
    - [x] 스테이지 해금 데이터베이스 연동

3. 카메라 트랙킹     
세부내용 : 씨네머신
    - [x] 씨네머신 적용하기
    - [ ] ~~카메라 워크 컷씬, 스크린 셰이크 이란...~~
    - [ ] ~~씨네머신 응용 구현~~
    
 4. 비동기 씬 로드  
 세부내용 : 타이틀씬 - 인게임씬 작업  
    - [x] 타이틀 구현
    - [x] 로딩씬 구현
    - [x] 인게임씬 구현
    - [x] 비동기 씬 로드
    - [x] 새 게임 생성 시, 게임데이터 삭제하기
    
 5. 전기 데미지  
 세부내용 : 초당 전기 데미지 - enemy  
    - [x] 전기 데미지 
    
 6. 소음발생장치
 세부내용 : 전기 프리팹을 통해 소음을 발생시키는 장치 제작
    - [x] 소음발생장치 디버그 구현
    - [x] 소음발생장치 SoundWave 구현
    
7. 브라이드 리소스 적용
세부내용 : 브라이드 리소스 적용하기
    - [x] 피벗 설정
    - [x] 컴포넌트 적용
    - [x] 걷기 애니메이션 연동
    - [ ] ~~공격 애니메이션 연동~~
    
### 9주차(5.11. ~ 5.17.)
1. INGAME 데이터 관리    
세부내용 : 인게임 데이터 관리
    - 스킬
        - [x] 스킬쿨다운
    - 에너미
        - [x] 리로드 리셋 적용
        
2. GAME 데이터 관리   
세부내용 : 게임 데이터 관리
    - 스킬
        - [x] 보유스킬
        - [ ] ~~스킬포인트~~
    - 스테이지 정보
        - [x] 스테이지 해금 정보 저장
        - [ ] ~~스테이지 overWrite 불가~~

3. 문, 열쇠 프리팹  
세부내용 : 새로운 프리팹 생성
    - [x] 문 프리팹 생성
    - [x] 열쇠 프리팹 생성

4. 소음발생 장치  
세부내용 : 소음발생 장치 사운드 웨이브 생성
    - [x] 사운드 웨이브 생성
    - [x] 소음발생장치 리소스 적용
    - [ ] ~~IMetal 상속~~
    
5. 씬 셋업  
세부내용 : 프로토타입 씬 세팅
    - 튜토리얼 씬
        - [x] 튜토리얼 씬 셋업
    - STAGE 1
        - [x] 씬 셋업

6. UI 작업  
세부내용 : player, Skill 정보 노출
    - 플레이어
        - [x] 체력
        - [x] 마나
    - skill
        - [x] Conduction
        - [x] PickLock
        - [x] Taze
        - [x] Drain
    - savePoint
        - [x] 저장 코멘트

7. 해상도 빌드  
세부내용 : 프로토타입 해상도 고정 빌드 방법 모색
    - [x] 해상도 고정 빌드 방법 모색
    
8. 애너미 상태 초기화  
세부내용 : 임시 방편으로? 데이터 저장없이 에너미 상태 초기화
    - [x] 씬 재로드에 따른 에너미 상태 초기화
    
### 10주차(5.18. ~ 5.24.)
1. Steal Key  
세부내용 : Make Stealable Key
    - [x] Steal Key 구현

2. 데이터 동기 로드  
세부내용 : 기반 / 엑셀 → 스킬 → FadeInEffect
    - [x] 동기 로드 구현

3. UI 개선  
세부내용 : 몇 가지 UI를 개선
    - [x] INGAME PANEL : HP, ELELC BAR UI 개선
    - [x] 그림자 적용
    - [x] TITLE SCENE의 전기 이펙트 판넬에 가려지게끔 적용
    - [x] 없는 대상에 대해서 UI INACTIVE 적용
    - [x] ST0, ST1 Scene Manage Button 구현
    - [x] F Interaction available UI

4. 전류 충전 배터리 제작  
세부내용 : 전류 흡수용 배터리 제작
    - [x] 배터리 제작
    - [x] 리소스 적용

5. 버그 관리  
세부내용 : 방역업체 컨텍
    - [x] 전력부여 UI Cancel 버그
    - [x] 픽락 deep condition에 따른 UI ACTIVE
    - [x] 테이저 deep condition에 따른 UI ACTIVE
    - [x] UnLock comment 가운데 정렬
    - [x] 개구리야 그만 울어라 나도 울고싶다
    - [x] 우리 문이 왜그럴까?
    
### 11주차(5.25. ~ 5.31.)
1. E스킬 병합  
세부내용 : E스킬(Drain) → Q스킬 계열로 로직, 키 병합
    - [x] 병합

2. UI 자동화  
세부내용 : UI 자동화에 대한 공부
    - [x] UI Object Auto Bind
    - [x] Function Auto Bind to Button UI Object
    
3. 체크포인트 로직 수정  
세부내용 : 순차 트리거 → Bigger 트리거
    - [x] 로직 수정
    
4. 최적화  
세부내용 : 최적화에 대한 내용 정리 + 빈 이벤트 함수 제거, InVoke() → Coroutine()
    - [x] 최적화 카드 생성
    - [x] Resources 폴더 최적화
    - [x] #define 최적화
    - [x] 하이어라키 최적화
    
5. 물 orig Point Auto Setting  
세부내용 : 자동 좌표설정과 프리팹화
    - [ ] ~~Orig Point Auto Setting~~
    - [ ] ~~프리팹 생성~~
    
6. 버그픽스  
세부내용 : 꾸준하게 버그를 리포트하고 수정
    - [x] 씬 로드 때, 문 애니메이션 소리 재생 수정
    - [x] 씬 로드 때, 트리거 코멘트 출력 수정
    - [x] 씬 로드 때, 플레이어 이동가능 수정
    - [x] 문 세이브 데이터 읽기 
  
### 12주차(6.1. ~ 6.7.)
1. 플레이어 시야 확장  
세부내용 : 마우스를 이용하 FOV확장
    - [x] 구현

2. 버그 수정   
    - [x] 체크포인트 인덱스 순차배치 강제성 제거
    - [x] 스킬 쿨다운 세이브/로드 오류 개선
    
3. etc
    - [x] 클리어 포인트 콜라이더 변경
    - [x] 문 콜라이더 변경
    - [x] 최적화
    - [x] 캔버스 생성 자동화
    - [x] Stage2 생성
  
### 13주차(6.8. ~ 6.14.)
1. 버그 해결
세부내용 : 빌드 전, 버그 수정
    - [x] 클리어 포인트 콜라이더 범위 변경
    - [x] 스킬 쿨다운 세이브 버그

2. 클리어포인트 인덱스의 자율성
세부내용 : 클리어포인트 인덱스 순차 트리거 매커니즘 해소
    - [x] 해소

3. 스테이지별 경과시간 매카니즘  
세부내용 : 스테이지별 경과시간 매카니즘 구현
    - [x] 스킬 쿨다운 세이브 버그로 인한 elapsed Time 구현
    
4. 스텐실 쉐이더(스텐실 버퍼) 구현  
세부내용 : 벽 위에 플레이어 실루엣 오버레이를 위한 스텐실 쉐이더 시스템 구현
    - [x] 2D & 3D Stencil Shader system
    - [ ] ~~URP Stencil Shader system~~
    
### 14주차(6.15. ~ 6.21.)
1. 스텐실 쉐이더(스텐실 버퍼) 구현  
세부내용 : 벽 위에 플레이어 실루엣 오버레이를 위한 스텐실 쉐이더 시스템 구현
    - [ ] URP Stencil Shader system
    
2. DataBase 데이터 로드 단순화
세부내용 : Scene 로드때마다의 불필요한 데이터 ReLoad 매카니즘 개선
    - [x] TitleScene 최초 실행 시에만 고정 데이터 로드 매카니즘 구현
    
3. 버그 해결  
세부내용 : 빌드 전, 버그 수정
    - [x] 벽 소팅 버그 수정

### 15주차(6.22. ~ 6.28.)
1. 스킬 테이블 연동  
세부내용 : 스킬테이블의 참조변수 필드 추가 연동
    - [x] Update skillValues Field

2. 텍스트 테이블 연동  
세부내용 : 언어설정에 따른 텍스트 변동목적의 텍스트 테이블 연동
    - [x] 텍스트 테이블 연동
    - [x] 텍스트 테이블 참조를 통한 UI설정

3. 다이나믹 텍스트 기능 구현  
세부내용 : 색상, 폰트 스타일 적용 방법 구현
    - [x] 고정색과 헥사코드 색상 적용 구현
    - [x] 볼드체와 이태릭채 적용 구현
    <img width="440" alt="StringReference N DynamicTextFont" src="https://user-images.githubusercontent.com/58582985/175847834-69d94fc4-0de4-48f9-a652-a3785034919a.png">
    
### 16주차(6.29. ~ 7.5.)
1. 구글 데이터 시트 연동  
세부내용 : 구글 데이터 시트 연동
    - [x] 연동
    
2. 버그 수정  
세부내용 : 버그
    - [x] 파이프 전력 전이 오류/파이프 콜라이더
    
3. UI  
세부내용 : UI 생성과 자동화
    - [ ] ~~작업보류~~
    
### 17, 18주차(7.6. ~ 7.19.)
1. 목표전기 전이(Elec TargetTransit)  
세부내용 : 목표전기 전이 구현
    - [x] 목표전기 전이 메카니즘 구현
    - [x] 전기 전이 중 버그(잔여 전기량과 관련없이 끊어짐)
    - [x] 목표 트리거 이후 전기 고착 메카니즘 구현 - 물
    
 2. 목표전기 전이 업그레이드  
 세부내용 : 목표전기 전이 업그레이드
    - [x] 멀티 장치 전기전이 메카니즘(최단 거리 장치 트리거) ← 18
    
3. 전력원과 장치  
세부내용 : 장치 구현
    - [x] F상호작용 토글 장치 구현
    - [x] 전력원에 따른 장치 구현
    - [x] 전력원 장치 인스펙터 자동화 ← 18
    - [x] 전력원 장치 회로 구현 메카니즘 ← 18
    - [x] 전기 컨테이너 구현 ← 18
    
4. 기타 작업  
세부내용 : 기타 작업 및 버그 수정  
    - [x] 에너미 피벗 포인트 변경에 따른 에너미 마비이펙트 생성 위치 ← 18
    - [x] elec Flow Sound Stop Mechanism ← 18
    - [x] Elec Trig Device Toggle off Mechanism ← 18
    - [x] soundGenerator device소속으로 편입
    
### 19주차(7.20. ~ 7.26.)
1. 전기변수 조정  
세부내용 : 전기 특성에 따른 변수 조정
    - [x] 전기 유지 시간 통일
    - [x] 전기 전이 속도 감소
    
2. 장치  
세부내용 : 장치관련 업데이트
    - [x] 램프의 장치 편입
    - [x] 램프 - 파워소스 연결
    - [x] IsUsable Device Data Save
  
3. 전력원  
세부내용 : 전력원 관련 업데이트
    - [x] powerSource containAmount Data Save
    - [x] 전력원 전력 부여 제한 메카니즘
    - [x] 장치 전력 부여 이펙트
    - [x] 파워소스 리소스 업데이트
    
4. 물 
세부내용 : 물 매카니즘 리워크
    - [x] 매카니즘과 프리팹 변경
    - [x] 단순 전이
    - [x] 목표전기 전이
    - [x] pipe 2 water 전이
    - [x] water 2 pipe 전이
    
5. 목표전기 전이  
세부내용 : 목표전기 전이 업그레이드
    - [ ] ~~트리거 전기 흡수 메카니즘(라이프타임 조정)~~ (후순위)
    - [ ] ~~전력원과 장치간의 목표전기 전이~~ (후순위)
    - [ ] ~~복합 전도체 위의 장치 목표 전이~~ (후순위)
    - [ ] ~~멀티 장치 최단거리 메카니즘 변경(복합 전도체 위의 멀티 장치)~~ (후순위)
    
6. UI  
세부내용 : 에셋을 활용한 UI작업
    - [x] Input System 변경
    
7. 기타  
세부내용 : 기타 작업 및 버그 슈팅
    - [x] 전력원과 장치 연결 노션 작성
    - [x] 장치, 전력원 배치 방법 노션 작성   
    
### 20주차(7.27. ~ 8.2.)
1. 프리팹  
세부내용 : 회의 내용 반영
    - [x] 파이프
    - [x] 물
    - [x] 상자
    - [x] 전력원
    
2. 전기전이
세부내용 : 버그 수정
    - [x] 타겟 전이 필요량 초과 부여시, 잔가지가 이상한 곳을 튀는 현상
    
3. UI
세부내용 : UI 적용
    - [x] UI 틀 갖추기
    - [x] Title에서 GameStart 연결하기
    - [x] stage Clear Panel
    - [x] Game Eneding Panel
    - [x] Option Panel
    
4. etc  
세부내용 : 등등,,
    - [x] 벽 뒤 light Can't trigger on Player
    - [x] Tazer Vector Checker Mechanism 

### 21주차(8.3. ~ 8.9.)
1. 목표전기 전이
세부내용 : 목표전기 전이 구체화  
    - [x] 트리거 전기 흡수 메카니즘(라이프타임 조정)
    - [x] 전력원과 장치간의 목표전기 전이

2. git Hub
세부내용 : 브랜치 관리  
    - [x] 브랜치 관리
    
### 22주차(8.10. ~ 8.16.)
1. 목표전기 전이
세부내용 : 목표전기 전이 구체화  
    - [x] 전력원과 장치간의 목표전기 전이 
    - [x] 복합 전도체 위의 장치 목표 전이(Water → Pipe)
    - [x] 멀티 장치 최단거리 메카니즘 변경(단순 거리 → 최소 이동 거리)  
    
2. 튜토리얼 최적화  
세부내용 : 스크립터블 오브젝트를 이용한 튜토리얼 메모리 최적화
    - [x] 스크립터블 오브젝트 이용
    
3. 라인렌더러
세부내용 : 라인렌더러 리소스 적용
    - [x] 방안 모색
    - [x] 임시 리소스 적용
    
4. 인게임 리소스
세부내용 : 인게임 리소스 적용
    - [x] 문
    - [x] 문 에니메이션
    - [x] 열쇠
    - [x] 자물쇠 FADE OUT
    
5. 상호작용 개선
세부내용 : 문 잠금 기능 제거
    - [x] 제거

### 23주차(8.17. ~ 8.23.)
1. 목표전기 전이  
세부내용 : 제발 마무리하자..
    - [x] 멀티 장치 & 복합 전도체 목표전기 전이

2. 체크포인트   
세부내용 : 체크포인트 상호작용 방식 수정  
    - [x] HeartEngine
    - [ ] Device
    - [ ] 수정
 
3. 스킬  
세부내용 : 스킬 구현
    - [ ] ~~스킬_구현~~
    
4. 문  
세부내용 : 문 기타 수정 사항
    - [x] Full Open sprite 수정
    - [x] 애니메이션 이벤트 함수 등록
    - [x] lock fade out 시간 조정

5. 기타  
세부내용 : 기타 수정 사항
    - [x] 몇몇 리소스 소팅 장애
    - [x] 파이프 규격화 데이터 수집

### 24주차(8.24. ~ 9.6.)
1. 체크포인트    
세부내용 : 체크포인트 상호작용 방식 수정  
    - [x] HeartEngine
    - [x] Device
    - [x] 수정
    
2. InGame ReSource  
세부내용 : 인게임 리소스 적용
    - [x] 파이프
    - [x] 파워라인
    
### 25주차(8.31. ~ 9.6.)
1. UI  
세부내용 : 플레이어 ui 변경
    - [x] 플레이어 상태
    - [x] 스킬

2. 세이브 포인트  
세부내용 : 추가 설정 변경
    - [x] PowerSourceDevice && IsUsable Device 특이점
    - [x] spawn Position 수정
    - [x] 리스폰 포인트 갱신 comment
    
3. 인게임 리소스 적용  
세부내용 : 리소스 적용
    - [x] 심장엔진
    - [x] 부활장치

4. 기타  
세부내용 : 기타사항
    - [x] 문 버그
    - [x] 전기 소팅 버그

### 26주차(9.7. ~ 9.13.)
1. UI  
세부내용 : skill Description  
    - [x] Hover Tip : Skill Description
    
 2. 스킬 이펙트 추가  
 세부내용 : 스킬 이펙트 추가
    - [x] 흡수
    - [x] 충전
    
3. 기타
세부내용 : 기타사항
    - [x] 개구리 IObtain → IContainer
    - [x] 플리커링 해결 방법 모색
    
### 27, 28주차(9.14. ~ 9.27.)
1. UI  
세부내용 : skill Description  
    - [x] Hover tip window Design   
    - [x] Hover Tip window Bug  
    - [x] Add Skill Name into hoverTip window   
    
2. Canvas  
세부내용 : 캔버스 병합 및 hotKey 적용
    - [x] 캔버스 병합(playerCanvas : tmp, inGameCanvas)
    - [x] hotKey : esc, j, mouseMidBtn
  
3. Skill Effect  
세부내용 : 스킬 이펙트 점검
    - [x] 은폐시 스킬 이펙트 표시 막기  
    - [x] 새로운 흡수 이펙트 
    - [x] 팔로잉 이펙트 : 충전, 실패
    - [ ] ~~이펙트 버그 - 충전~~
    - [x] 이펙트 버그 - 실패
     
4. 상호작용 메카니즘  
세부내용 : 상호작용 범위 변경
    - [x] 상하
    - [x] 좌우
 
5. 기타  
세부내용 : 기타사항
    - [x] 파이프 리소스 변경
    - [x] 파이프 - 전기 소팅 버그 
    - [x] 컨테이너 레이어 
    
### 29주차(9.28. ~ 10.4.)
1. 전기 흡수  
세부내용 : 전기 흡수 관련 버그수정
    - [x] 전기 흡수 메카니즘 변경
    - [ ] 컨테이너 전기 흡수 버그
    - [x] w2p targetting transit 버그
    
2. 스킬 시스템  
세부내용 : 스킬 시스템 유틸 함수 추가
    - [x] 스킬 획득 메카니즘
    - [x] 스킬 레벨업 메카니즘
    - [x] 스킬 시스템 최적화
    
3. 스킬 구현  
세부내용 : 신규 스킬 구현
    - [ ] 천둥
    - [ ] 플레시

4. RMB 스킬 UI  
세부내용 : RMB스킬 관련 UI
    - [x] 스킬 UI
    
5. 스킬 시스템  
세부내용 : 스킬 발동 메카니즘 변경
    - [x] 은폐중 스킬 사용 가능 조건 추가
    - [x] 은폐중 이동 방지 메카니즘 변경
    
6. 기타  
세부내용 : 기타사항
    - [x] 상호작용 관련 변수 코드 정리
    - [x] 상호작용 범위 변경


### 30주차(10.5. ~ 10.11.)
1. 스킬   
세부내용 : 신규 스킬 구현
    - [ ] 천둥
    - [ ] 플레시

2. 스킬 UI  
세부내용 : Quick Wheel Skill Panel
    - [x] UI 형성
    - [x] 버튼 애니메이션
    - [x] 버튼 메카니즘 
    
3. 컷신  
세부내용 : 컷신 어시스트
    - [x] signal script
    - [x] 컷신 어시스트
    
4. 기타  
세부내용 : 기타사항
    - [x] 전력 부여 이펙트 - glow effect 제거
    - [x] 위쪽 상호작용 범위 1.6f → 1.2f


### 31, 32주차(10.12. ~ 10.25.)
1. 스킬   
세부내용 : 신규 스킬 구현
    - [x] 천둥 
    - [x] 플레시
    - [x] 피뢰침
    
2. 컷신  
세부내용 : 컷신 어시스트
    - [x] 새로운 컷신 재생 메카니즘 - IToggle
    - [x] 새로운 컷신 재생 메카니즘 - IMetal
    
3. 스킬 이펙트  
세부내용 : 스킬 이펙트 협업
    - [x] 천둥
    - [x] 플래시
    - [x] 피뢰침
    
4. 스킬포인트  
세부내용 : 임시 스킬 습득 방식
    - [x] 스킬 포인트 프리팹 구현
    
5. 기타  
세부내용 : 기타사항 및 버그
    - [ ] Quick Wheel 버그
    - [ ] 흡수 이펙트 버그
    - [ ] 옵션 UI 버그
    - [ ] 흡수량 버그
    - [ ] 컨테이너 트리거 전기 흡수 버그

### 33주차(10.26. ~ 11.01.)  
휴식  


### 34주차(11.02. ~ 11.08.)  
1. 버그잡기
   - [x] 플래시 이펙트 버그
   - [x] Quick Wheel 버그
   - [x] 충전 이펙트 버그
   - [x] 옵션 UI 버그
   - [ ] 흡수량 버그
   - [x] 컨테이너 트리거 전기 흡수 버그

2. 스킬 정상화
   - [x] 플래시
   - [x] 천둥
   - [x] 피뢰침
   
### 35, 36, 37, 38주차(11.09. ~ 12.06.)
1. 쉐이더
   - [x] 스텐실 쉐이더
   - [x] 아웃라인 쉐이더

2. 장치
   - [x] 파괴가능 컨테이너
   - [x] 파과기능 저장
   
### 39, 40주차(12.7. ~ 12.20.)
1. 쉐이더
   - [x] 아웃라인
   - [x] 아웃라인 쉐이더 적용
   - [x] 스텐실 쉐이더 추가 작업

### 41주차(12.21. ~ 12.27.)
1. RMB 스킬   
   - [x] rmb 스킬 시퀀스 제작 

### 42 ~ 45주차(12.28. ~ 01.24.)
1. RMB 스킬 : 천둥
   - [x] 스킬 범위 UI
   - [x] 포물 궤적 UI
   - [x] 스킬 시전 불가 UI
   - [x] 스킬 사거리 데이터 시트 연동
   - [x] 원형 UI 케시 제작
   - [x] 스킬 범위 UI
   - [x] 레이케스팅 메카니즘
   - [x] 천둥 스킬 이펙트

2. E 스킬 : 블루프린트
   - [x] Q, F조작 불가
   - [x] F UI, OutLineMat OFF
   - [x] E UI
   - [x] 피격시 해제
   - [x] 호버 네임 텍스트
 
3. RMB 스킬 : 갈바니즘
   - [ ] 갈바니즘 스킬 메카니즘 및 UI  
   - [ ] 갈바니즘 스킬 이펙트 메카니즘  
   
### 46 ~ 49주차(01.25. ~ 02.21.)   
1. RMB 스킬 : 갈바니즘
   - [x] 갈바니즘 스킬 메카니즘 및 UI    
   - [ ] 갈바니즘 스킬 이펙트 메카니즘   

2. E 스킬 : Blue Print
   - [x] 전도체 빗금 해제
   - [x] 전도체 색상 확정
   
3. RMB Hold Mechanism
   - [x] 뇌운
   - [x] 갈바니즘

### 50~53주차(02.22. ~ 03.20.)  
1. 개구리 AI  
   - [x] 개구리 만들기  
   - [x] 개구리 소팅   
   - [x] 개구리 AI
   - [x] 개구리 이미지 점프 코드
   - [x] 개구리 방향 감지
   - [x] 개구리 애니메이션
   - [x] 개구리 충돌로 인한 순간이동 버그 해결  
    
2. Moving Trajectory 수정
   - [x] 선이 아닌 점으로 변경
   - [x] 방향에 따른 경로 표시  
   - [x] 글로우 이펙트 표시
   - [x] 장애물을 넘겨 그릴 수 없도록
  
3. 전력 부여 메카니즘 수정
   - [x] 최초 입력으로 즉시 전도 불가

### 54주차(03.21. ~ 03.27.)
1. 개구리 경로
   - [x] 첫번째 지점까지의 경로 보간
   - [x] obstruction 직전까지 경로보간


## 웃픈..
- cos()안에 계속 deg 값을 적용..  * Mathf.Deg2Rad
