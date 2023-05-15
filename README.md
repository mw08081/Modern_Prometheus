# It's Alive 팀 Galvanic Bride 작품 진행상황 보고서
갈바닉 브라이드는 전기 능력을 보유한 주인공이 스텔스를 유지한채 스테이지를 해결하는 게임이다

## 기록할만한 구현 목록
### 1. Skill - Galvanism : 플레이어가 기록한 경로를 따라 개구리를 이동시키는 스킬 (적 교란)  
변경사항  
- update()함수에 의해 매 프레임 기록되는 포인트 사이에서 방향 벡터 계산이 어려움  <br> →  특정 거리마다 포인트를 기록하도록 변경  
- 플레이어가 기록한 경로를 가시적으로 표현하는데 사용되는 포인트를 빈번하게 생성/파괴할 경우 부하가 생김  <br> →  포인트를 오브젝트풀로 관리함  
- 오브젝트 풀에서 Dequeue된 오브젝트들의 머테리얼을 순차적으로 관리하는데 어려움  <br> →  Dequeue된 오브젝트들을 List에 다시 담아 순차적으로 관리함  
- 순차적으로 관리되는 오브젝트들이더라도 빈번한 스프라이트 컴포넌트를 가져올 경우 부하가 생김  <br> →  Tuple<GameObject, SpriteRenderer>로 관리함    
- 스킬 시전간에 플레이어가 반드시 마우스 포인트를 개구리에서부터 시작할 보장이 없었다  <br> →  개구리와 최초 마우스 클릭 지점 사이를 보간하여 

　  
> Q. Update() 호출 시, 매 프레임 기록되는 라인 렌더러의 좌표가 너무 많아서 각 좌표 사이의 방향 벡터를 계산할 경우 소숫점 아래까지 많이 동일하여 방향 벡터가 (0, 0, 0)으로 표시됨  
![image](https://github.com/mw08081/Modern_Prometheus/assets/58582985/113d3b12-f321-4493-a984-481d81f87d42)   << 구현 목표  
![image](https://github.com/mw08081/Modern_Prometheus/assets/58582985/f2eaeee7-830c-4b34-a94f-570c43f20e0c)   << 각 좌표사이의 방향 벡터가 제대로 계산되지 못함  

A. 라인 렌더러의 좌표도 너무 많을 경우 부하가 생기기때문에 특정 거리(0.5f)씩 마다 좌표를 찍는 방식으로 변경
```C#
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
  　  
  　  
> Q. 플레이어에의해 그려지는 라인 렌더러를 쉽게 식별하기 위해 0.5f거리마다 노란색 점을 표시하는데, 지속적으로 화살표 오브젝트를 생성/파괴할 경우 무거운 Instantiate()/Destroy()를 호출하여 부하를 일으킨다

A. 화살표 오브젝트를 오브젝트 풀링하여 관리하면 빈번한 생성과 파괴를 피할 수 있다.
```C#
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
  　  
  　  
> Q. 월드에 배치된 포인트의 흐름을 확인시켜주기 위해 각 포인트 스프라이트의 머테리얼을 순차적으로 한 개씩 머테리얼을 변경해줘야 하는데, 단순히 오브젝트 풀에서 Dequeue()된 것만으로는 관리가 어려움
  
A. Dequeue()된 오브젝트를 List<GameObject>에 넣어서 순차적으로 관리
```C#
List<GameObject> pointList = new List<GameObject>();            //Enqueue 포인트 리스트
 
void PointOnTrajectory(Vector3 _position)
{
    GameObject go = GetPointFromQueue();
    pointList.Add(go);                          //큐에서 나온 포인트를 리스트로 삽입(순차적인 머테리얼 관리와 일괄 비활을 위한 삽입)
 
    // ...
}
 
IEnumerator IterateListForChangingMat()
{
    for (int i = 0; i < pointList.Count; i++)      //for 을 통해 순차적으로 머테리얼 변경
    {
        pointList[i].getComponent<SpriteReneder>().material = glowMat;             //머테리얼 변경
        pointList[i == 0 ? pointList.Count - 1 : i - 1].getComponent<SpriteRenderer>().material = defaultMat;     //머테리얼 원복
        
        yield return INTERATION_INTERVAL;
    }
}
```
　  
　    
> Q. 월드에 배치된 포인트 한 번에 한 개씩 Glow Effect를 부여하려고 할 때, 매 번 getComponent<SpriteRenederer>()를 하면 부하가 생김  
![gv2](https://github.com/mw08081/Modern_Prometheus/assets/58582985/870a7f5f-48dd-4015-a8e4-fd3fdd55e69d)  << 구현목표  

A. 포인트 오브젝트 풀을 `Queue<GameObject>`가 아닌 `Queue<Tuple<Gameobject, SpriteRenderer>>`로 관리한다  
```C#
Queue<Tuple<GameObject, SpriteRenderer>> pointPool = new Queue<Tuple<GameObject, SpriteRenderer>>();          //포인트 풀
List<Tuple<GameObject, SpriteRenderer>> pointList = new List<Tuple<GameObject, SpriteRenderer>>();            //Enqueue 포인트 리스트

//풀 생성과 풀 오브젝트 리퀘스트
Tuple<GameObject, SpriteRenderer> GeneratePoint()
{
    //Instantiate 과정

    Tuple<GameObject, SpriteRenderer> tuple                     //최초에 스프라이트렌더러를 받아옴
             = new Tuple<GameObject, SpriteRenderer>(go, go.GetComponentInChildren<SpriteRenderer>());    
    return tuple;
}
Tuple<GameObject, SpriteRenderer> GetPointFromQueue()
{
   // ...
}
 
//tuple.Item2 에 저장된 스프라이트 렌더러 머테리얼 
IEnumerator IterateListForChangingMat()
{
    for (int i = 0; i < pointList.Count; i++)
    {
        pointList[i].Item2.material = glowMat;
        pointList[i == 0 ? pointList.Count - 1 : i - 1].Item2.material = defaultMat;
        yield return INTERATION_INTERVAL;
    }
    isInIterating = false;
}
```
　  
　  
> Q. 개구리와 최초 마우스 클릭 지점사이를 보간하지 않을 경우 개구리가 한 번에 엄청난 거리를 이동하는 버그가 생긴다
  
![nointerpol](https://github.com/mw08081/Modern_Prometheus/assets/58582985/74505b0d-7608-4c9e-aef5-4c09bb6febdd)  << 보간 전  
![interpol](https://github.com/mw08081/Modern_Prometheus/assets/58582985/fbeb97f2-af2a-4424-8c23-c942400556e6)    << 보간 후  
A. 개구리와 최초 마우스 입력 위치가 0.5f 이상일 경우 그 사이를 0.5f단위로 보간하여 라인렌더러에 좌표를 삽입한다
```C#
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


　  
　  
### 2. Skill - bluePrint : 청사진과 같이 현재 플레이어 주변의 맵 상황을 확인하는 스킬
![image](https://github.com/mw08081/Modern_Prometheus/assets/58582985/81544dc2-c76c-443a-956b-968b6222e548)  
변경사항  
- 특정한 오브젝트를 해당하는 색상으로 표시하는데, 각 오브젝트들의 스텐실 스프라이트를 추가로 배치하여 두개씩 관리하기엔 양이 너무 많아 관리하기 어려움  <br> →  생삭으로 표시될 오브젝트의 스프라이트를 처음부터 스텐실 버퍼를 가지며, 온전하게 그려내는 머테리얼로 설정한 뒤 맵 전체에 스텐실 버퍼를 표시하는 필터를 배치
- 색상이 적용된 오브젝트 이외에는 좀 더 어두운 색상으로 맵을 가리듯 표시하는 방법 모색  <br> →  스텐실 버퍼가 없는 곳은 스텐실 버퍼값이 0이므로 스텐실 버퍼 값이 0인 부분을 검은색으로 설정

　  
> Q. 최초엔 스텐실 머테리얼이 적용된 스프라이트를 블루프린트 스킬 활성화 시 setActive(true) 설정하고, 스킬 비활성화 시엔 setActive(false)시키는 방식으로 고안되었다. 그러나 setActive()시킬 오브젝트가 한 개일 경우엔 문제가 없지만 이러한 오브젝트가 많을 경우에는 관리가 어렵고 불필요한 작업이 발생할 수 있을 것 같아서 더 좋은 방식을 고안해기로 했다  

![image](https://github.com/mw08081/Modern_Prometheus/assets/58582985/a28ab91c-963d-421d-a388-bd33b8c289a1)  
![image](https://github.com/mw08081/Modern_Prometheus/assets/58582985/8e4fc2c5-37bc-49b0-a942-3e9b7f0fa64d)  << 머테리얼이 stencil_Q인 스프라이트를 on/off 해야한다

A. 기본적으로 색상이 입혀질 오브젝트는 그대로 그리며, 스텐실 버퍼를 변경하기만 한다. 그리고 맵 전체를 덮는 스텐실 필터 오브젝트를 생성하여 스텐실 필터와 겹쳐지며 스텐실 버퍼값이 동일한 경우 해당 부분에 색상을 표시하는 방식으로 진행한다
```shader
// 오브젝트는 스텐실 버퍼값만 변경(Pass Replace)하면서 항상 그대로 그려낸다(Comp Always)
Pass
{
    Tags { "LightMode" = "Universal2D" }

    Stencil
    {
        Ref [_StencilValue]
        Comp Always
        Pass Replace
    }
    //...
}
    
//스텐실 버퍼 필터 오브젝트는 스텐실 버퍼값은 변경하지 않고, 스텐실 버퍼값이 동일한 경우에만 그린다(Comp Equal)
Pass
{
    Tags { "LightMode" = "Universal2D" }

    Stencil
    {
        Ref[_StencilValue]
        Comp Equal
    }
    //....
}
```
　  
　  
> Q. 색상이 표시되는 오브젝트 이외에 벽이나 길 같은 것은 좀 더 검은색으로 보이게 하고싶었다 게임자체를 어둡게 할 경우, 스텐실 필터의 색상도 어두워지므로 그렇게 구현할 수는 없었다.

![image](https://github.com/mw08081/Modern_Prometheus/assets/58582985/fdd884a9-c310-49dd-a1ce-a3140258c965) << 구현 전  
![image](https://github.com/mw08081/Modern_Prometheus/assets/58582985/083e96d9-5556-4457-8ea1-35a2d1253795) << 구현 후


A. 좀 더 검은색으로 표시된 부분들의 공통점을 생각해보니 스텐실 버퍼가 변경되지 않은( 스텐실 버퍼 == 0) 부분이었다. 그래서 스텐실 버퍼 필터와 동일한 방식으로 스텐실 버퍼가 0인 부분을 짙은 회색으로 색을 입히면 될 것같다고 생각했다.
```shader
// 사전에 스텐실 버퍼값을 변경할 오브젝트는 존재하지 않는다( 스텐실 버퍼가 0인 그대로 진행)

// 스텐실 버퍼가 0인 부분을 좀 더 검은색으로 색을 입힐 스텐실 버퍼 필터의 쉐이더 pass 내용
Pass
{
    Tags { "LightMode" = "Universal2D" }

    Stencil
    {
        Ref 0
        Comp Equal
    }
    //....
}
```

  　  
  　  
 

## 기억에 남는 작업
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
   - [ ] 과부화 스킬 구현
   - [x] 번개 스킬 구현
   - [ ] 천둥 스킬 구현

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
    - [ ] 스킬 테이블 수정
    - [ ] 스킬 스트링 테이블 수정
    - [ ] 스킬 UI 안정화
    - [ ] 퀵힐 안정화


  
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
