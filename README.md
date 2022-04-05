# Modern_Prometheus
GameMakers 6기 It's Alive 팀 그룹 작품 진행상황 보고서

### 1주차(3.16. ~ 3.22.)
1. 플레이어 에셋 찾기  
2. 플레이어 이동 구현(8방향)  
3. 플레이어 애니메이션 최적화 및 커스터마이즈
4. 타일맵 제작 / PlayerTestScene  
5. GameManager 싱글톤 구축

### 2주차(3.23. ~ 3.29.)
1. IBIio 테이즈 구현  
2. Iwater , IMetal 전기 전이 메카니즘  
3. IElectricity - elecPoint 전기 흡수 메카니즘
4. IElectricity 흡수 fadeOut Effect
5. 테이즈 시스템 조건 추가

### 3주차(3.30. ~ 4.5.)
1. 물 객체에 전기 전이 메카니즘 구현
2. 오브젝트 풀 시스템 구현
3. 파티클 시스템 라이프 타임 관련 스크립트 생성
4. 플레이어 인터렉터블 영역 변경 (circle -> Arc -> cube)
5. 데이터베이스 싱글톤 구축  
6. 엑셀 데이터 로더와 데이터베이스 연동

### 4주차(4.6. ~ 4.12.)
1. 물객체와의 상호작용 추가작업  
세부내용 : 물 객체와의 상호작용 추가 작업  
    - [x] 전이 전력 소멸 후 전력전이 포인트 초기화
    - [x] 물 객체 위에서의 물 객체와의 상호작용 불가 메카니즘 구현
    - [ ] 중첩전기의 lifeTime 증가 프로세스  

2. 물 객체와의 상호작용 테스트  
세부내용 : 다양한 예시를 통한 물 객체 전기전이 테스트  
    - [ ] 물 객체 모델 수령 
    - [ ] 물 객체 제작
    - [ ] 테스트  
    
3. 금속 객체와의 상호작용 구현  
세부내용 : Metal 전기 전이 구현  
ㄴ사전작업 : IMove, Interact F
    - [ ] IMove Interface 구현
    - [ ] Interact F 구현
    - [ ] Metal 전기전이 메카니즘 구현
    - [ ] 오브젝트 이동 시, 오브젝트 포인트 재계산 메카니즘 구현

4. 상호작용 업그레이드  
세부내용 : Water/Metal의 각각의 포인트에서 다른 Interactable Object를 만났을때, 전기 전이가 이뤄지는 메카니즘을 생성  
    - [ ] 오브젝트 포인터 별 다른 오브젝트 체크  
    - [ ] 새로운 오브젝트 발견 시, 전이(mainBody/Dervied 상관없이 전이 발생)  
    
5. 전기 흡수 메카니즘  
세부내용 : 발생시킨 전이 전기와 그 주변 인근 전기 흡수 메카니즘  
    - [ ] 흡수된 전이전기의 주변 전이전기 계산
    
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
    ~~- 열쇠시스템~~  
    ~~- 문 열림 애니메이션~~  

8. 썬더  
세부내용 : RMB 스킬 썬더 스킬 구현
ㄴ사전작업 : DATA_SKILLRANGE_TABLE DataBase 연동, ScriptableObject 생성  
    - [ ] EXCEL 연동
    - [ ] Scriptable Object 생성
    - [ ] DataBase 연동

### 5주차
