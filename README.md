# Modern_Prometheus
GameMakers 6기 It's Alive 팀 그룹 작품 진행상황 보고서

### 8주차(5.4. ~ 5.10.)
1. 게임 데이터 로드  
세부내용 : 게임 데이터 로드 구현 진행상황  
    - [x] 플레이어 데이터 : 체력, 마나 (#1. 리스폰)
    - [x] 플레이어 데이터 : 체력, 마나 (#2. 재시작)
    - [x] 보유스킬
    - [x] 수집목록 
    - [x] 스테이지 오브젝트 - 세이브포인트(Only ReStart)
    - [x] 스테이지 오브젝트 - 열쇠
    - [x] 스테이지 오브젝트 - 문
    - [ ] 스테이지 오브젝트 - 에너미 (#1. 리스폰)
    - [ ] 스테이지 오브젝트 - 에너미 (#2. 재시작)
    - [ ] ~~스테이지 해금~~
    - [ ] ~~정제전력~~
    - [ ] ~~도전과제~~
        
2. 스테이지 클리어 포인트
세부내용 : 스테이지 클리어 포인트 생성과 구현
    - [ ] 스테이지 클리어 포인트 구현
    - [ ] 스테이지 해금 업데이트
    - [ ] 스테이지 해금 데이터베이스 연동

3. 카메라 트랙킹     
세부내용 : 씨네머신
    - [ ] 씨네머신 활용해보기
    - [ ] 카메라 워크 컷씬, 스크린 셰이크 이란...
    - [ ] 씨네머신 응용하기
    
 4. 비동기 씬 로드  
 세부내용 : 타이틀씬 - 인게임씬 작업  
    - [x] 타이틀 구현
    - [x] 로딩씬 구현
    - [x] 비동기 씬 로드
    - [x] 새 게임 생성 시, 게임데이터 삭제하기
    
 5. 전기 데미지  
 세부내용 : 초당 전기 데미지 - enemy  
    - [ ] 전기 데미지 
    
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
    - [ ] 플레이어 데이터 : 체력, 마나 (#2. 재시작)
    - [x] 보유스킬
    - [ ] 수집목록 
    - [ ] 스테이지 오브젝트 - 세이브포인트(Only ReStart)
    - [x] 스테이지 오브젝트 - 열쇠
    - [x] 스테이지 오브젝트 - 문
    - [x] 스테이지 오브젝트 - 에너미 (#1. 리스폰)
    - [ ] 스테이지 오브젝트 - 에너미 (#2. 재시작)
    - [ ] ~~스테이지 해금~~
    - [ ] ~~정제전력~~
    - [ ] ~~도전과제~~

### 다음 주차 가이드라인

### 개발방향 가이드라인
- Water 구간제외 개발 후 확장성과 공통점에 따른 상속 구현  
- Water 구간제외 개발 후 물 lifeTime 메카니즘 구현
- Water 구간제외 개발 후 물에서의 전기 흡수 메카니즘 개선
- 대각전기 전이 버그 수정   

- 밝기조정  
- 해상도조정  

- 천둥   
세부내용 : 기능 구현을 통한 초안 구현  
    - [ ] 초안
    - [ ] 소음 시스템

- 썬더  
세부내용 : RMB 스킬 썬더 스킬 추가 구현  
ㄴ사전작업 : DATA_SKILLRANGE_TABLE DataBase 연동, ScriptableObject 생성  
    - [ ] EXCEL 연동
    - [ ] Scriptable Object 생성
    - [ ] DataBase 연동

- 스킬 이펙트 결정   
세부내용 : 기획자회의를 통한 이펙트 선정  
    - [ ] 천둥 이펙트 확정

- 스킬 이팩트 적용    
세부내용 : 회의를 통해 결정된 이펙트 적용
    - [ ] 천둥 이펙트 적용
