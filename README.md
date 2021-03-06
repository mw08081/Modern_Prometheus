# Modern_Prometheus 
GameMakers 6기 It's Alive 팀 그룹 작품 진행상황 보고서

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
    - [ ] 파워소스 리소스 업데이트
    
4. 물 
세부내용 : 물 매카니즘 리워크
    - [x] 매카니즘과 프리팹 변경
    - [x] 단순 전이
    - [x] 목표전기 전이
    - [x] pipe 2 water 전이
    - [x] water 2 pipe 전이
    - [ ] 복합전도체 전이
    - [ ] 복합 전도체 멀티 장치 우선 전이
    
5. 목표전기 전이  
세부내용 : 목표전기 전이 업그레이드
    - [ ] 전력원과 장치간의 목표전기 전이
    - [ ] 복합 전도체 위의 장치 목표 전이
    - [ ] 멀티 장치 최단거리 메카니즘 변경(복합 전도체 위의 멀티 장치)
    
6. UI  
세부내용 : 에셋을 활용한 UI작업
    - [ ] UI 자동화
    
7. 기타  
세부내용 : 기타 작업 및 버그 슈팅
    - [x] 전력원과 장치 연결 노션 작성
    - [x] 장치, 전력원 배치 방법 노션 작성
    - [ ] ~~목표 트리거 이후 전기 고착 메카니즘 구현 - 파이프~~(20주차 예정 : 개발자 회의 이후)
    
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


### 다음 주차 가이드라인

### 개발방향 가이드라인
- Water 구간제외 개발 후 확장성과 공통점에 따른 상속 구현  
- Water 구간제외 개발 후 물 lifeTime 메카니즘 구현
- Water 구간제외 개발 후 물에서의 전기 흡수 메카니즘 개선

- 대각전기 전이 버그 수정   
- fadeIn Effect 중에 옵션 페널이 실행되면 클릭이 불가한 점

- 밝기조정  
- 해상도조정  
- 천둥 : 천둥

### 기억에 남는 작업
- 프로파일링 - 리지드바디, 콜라이더. 바디타입  
- coroutine 동기로드 / async - await 비동기 
- Optimazing
- Player FOV Expansion
- 파이프라인 따라 전기 생성  
- Line Renederer

### 웃픈..
- cos()안에 계속 deg 값을 적용..  * Mathf.Deg2Rad

### 새로운 개념
- System.Enum.GetValue(typeof(EnumName)).Length  
- gameObject.trasnform.childCount  
- scripting API - https://docs.unity3d.com/ScriptReference
    - SortingOrder : using UnityEngine.Rendering
    - Tilemap : using UnityEngine.Tilemap
    - Light2D : using UnityEngine.Experimental.Rendering.LWRP
- RayCast unsing Speific Layer Filltering 
    - https://dallcom-forever2620.tistory.com/18
- Get Vector using angle(need to adjust Quaternion's(x,y,z) & StdVector)
    - https://wiseraintown.tistory.com/entry/Transform-%EA%B0%81%EB%8F%84%EC%97%90-%EB%94%B0%EB%A5%B8-Object-%EB%B0%A9%ED%96%A5-%EA%B5%AC%ED%95%98%EA%B8%B0
- Create Unity Editor custom Button to execute some Function
    - https://wergia.tistory.com/165
- Event on Editor & Get WorldPosition using Event.mousePosition
    - https://answers.unity.com/questions/381630/listen-for-a-key-in-edit-mode.html
    - https://answers.unity.com/questions/877467/how-to-get-mouse-position-in-world-space-when-in-e.html
- LineRenederer
    - https://beatchoi.github.io/unity3d/basics/2020/12/14/LineRenderer2/
- How to save variable in custom editor in unity
    - https://postpiglet.netlify.app/posts/unity-prefabscenemark/
    - https://forum.unity.com/threads/custom-editor-variables-not-saving.513406/
    - https://stackoverflow.com/questions/61238628/cannot-save-a-variable-of-a-editor-script
    - https://assetstore.unity.com/packages/tools/utilities/play-mode-save-177452
 - Particle System Playing Stop immediately
    - pc.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
