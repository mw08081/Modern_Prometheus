# Modern_Prometheus
GameMakers 6기 It's Alive 팀 그룹 작품 진행상황 보고서

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
    - [ ] 테이즈 스킬 조건 추가
    
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
    - [ ] isTransitedClear Method 버그 수정
    - [ ] 물 객체 전기전이 새로운 메카니즘 개발

2. 물 객체와의 상호작용 테스트  
세부내용 : 다양한 예시를 통한 물 객체 전기전이 테스트  
    - [x] 물 객체 모델 수령 
    - [ ] 물 객체 제작
    - [ ] 테스트  
    
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
    -

4. 상호작용 업그레이드  
세부내용 : Water/Metal의 각각의 포인트에서 다른 Interactable Object를 만났을때, 전기 전이가 이뤄지는 메카니즘을 생성  
    - [ ] 오브젝트 포인터 별 다른 오브젝트 체크  
    - [ ] 새로운 오브젝트 발견 시, 전이(mainBody/Dervied 상관없이 전이 발생)  
    
5. 전기 흡수 메카니즘  
세부내용 : 발생시킨 전이 전기와 그 주변 인근 전기 흡수 메카니즘   
    - [x] 흡수된 전이전기의 주변 전이전기 계산
    - [x] 전기 흡수 메카니즘 개선
    
6. ObtainSkill Sava System 구축
세부내용 : 기본 스킬과 습득 스킬에 따른 ObtainSKill DB 구축  
    - [ ] 기본 스킬과 습득 스킬 정보 수집
    - [ ] ObtainSkill Data 저장 메카니즘 구현
    - [ ] ObtainSKill에 기본 스킬 초기화 메소드 구현
    - [ ] 부활/게임로드의 스킬 데이터 초기화 메소드 구현

7. 픽락   
세부내용 : 상호작용 PickLock 스킬 구현  
ㄴ사전작업 : IOpen  
    - [ ] IOpen interface 구현  
    - [ ] Player Skill CoolDown Dic 구현
    - [ ] PickLock Debug 연출 구현  
    - [ ] ~~열쇠시스템~~  
    - [ ] ~~문 열림 애니메이션~~  

8. 썬더  
세부내용 : RMB 스킬 썬더 스킬 구현  
ㄴ사전작업 : DATA_SKILLRANGE_TABLE DataBase 연동, ScriptableObject 생성  
    - [ ] EXCEL 연동
    - [ ] Scriptable Object 생성
    - [ ] DataBase 연동

### 5주차
1. 플레이어 이동 관련 버그 수정
    - [ ] 실행 pc에 따른 이동속도 버그 수정


### 개발방향을 위한 가이드
- [ ] Water 구간제외 개발 후 확장성과 공통점에 따른 상속 구현  
- [ ] Water 구간제외 개발 후 물에서의 전기 흡수 메카니즘  
- [ ] 대각전기 전이 버그 수정  
- [ ] ~~IMove Interface 구현~~  
- [ ] ~~Interact F 구현~~  
- [ ] ~~오브젝트 이동 시, 오브젝트 포인트 재계산 메카니즘 구현~~  
