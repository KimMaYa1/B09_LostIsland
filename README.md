# SpartaDungeonInUnity

### 목차

1. 게임 설명
2. 구현 목록
3. 플레이 화면

---

# 1. 게임 설명

- **게임명: `로스트 아일랜드`**
- **설명:** [내일 배움 캠프 8기 Unity] Unity 게임개발 숙련 팀 프로젝트.
- **개요:**
    - 3D 서바이벌 게임을 구현.

---

# 2. 구현 목록


# 🔽 조범준
### 플레이어
- [플레이어 이동 / 플레이어 점프 / 물체별 상호작용 구현](https://github.com/KimMaYa1/B09_LostIsland/blob/main/Assets/Scripts/Player/PlayerClickMove.cs)
    - 기존에 배웠던 방식과 달리 3인칭으로 진행하여 이동을 마우스로 받았습니다.
    - 마우스로 이동을 받다보니 다른 모든 행동들을 마우스로 받으면 좋겟다해서 전부 마우스로 처리가능하도록 만들었습니다.
    - 하면서 어려웠던 점은 이동 방법이 계속 바뀌다보니 코드를 계속 수정하여 어려웠습니다
    - 아쉬웠던 점은 기능별로 스크립트를 나눴으면 좀더 수월하게 작업할수 있었을거같습니다.
    - 앞으로는 하루를 다쓰더라도 초기 기획단계를 많이 사용하여야 할거같다고 느꼇습니다.
- [플레이어 컨디션](https://github.com/KimMaYa1/B09_LostIsland/blob/main/Assets/Scripts/Player/PlayerConditins.cs)
    - 강의중에 진행했던 컨디션 부분을 받아와서 약간의 수정을 거쳣습니다.
### 카메라
- [카메라 이동](https://github.com/KimMaYa1/B09_LostIsland/blob/main/Assets/Scripts/Player/CamMove.cs)
    - 3인칭 뷰의 다른 게임들을 바탕으로 카메라의 이동이나 고정을 변경하였습니다.
    - 다른데에 시간을 쓰다보니 좀더 디테일하게 만들지 못하여 조금 아쉽습니다.
### UI
- [일시정지](https://github.com/KimMaYa1/B09_LostIsland/blob/main/Assets/Scripts/Interface/StopGame.cs)
    - 만들지 않았다는것을 까먹고 마지막날 부랴부랴 만들었습니다.
    - 전 프로젝트에 만들어본 경험이 있어서 어렵지 않게 구현하였습니다.
- [상호작용](https://github.com/KimMaYa1/B09_LostIsland/blob/main/Assets/Scripts/Manager/InteractionManager.cs)
    - 어떤 오브젝트에 올렸도 바뀌지 않는 마우스가 밋밋해서 커서를 변경하도록 하였습니다.
    - 하단의 SetPromptText()부분은 장성림님께서 작업하셨습니다.

# 🔽 고영현
- 몬스터
    - 애니메이션/ 공격/ 리스폰/ 상태 변경 



# 🔽 우민규
- 아이템
    - 장비 / 설치류 / 재료 / 소비
    - 인벤토리



# 🔽 박민혁
- 맵
  -    게임시작씬 / 게임씬 / 로딩씬 / 맵 / 아이템 리스폰 / 프롤로그씬 / 날씨
  
- 아이템
  -    리소스 아이템



# 🔽 장성림
- UI
    - 플레이어 컨디션 바
    - 인벤토리
    - 제작
    - 마우스 인터렉트

애니메이션
     - 플레이어

- 아이템
    - 제작 아이템
    - 제작 아이템 레시피

- 건축
    - 건축 아이템
    - 건축 기능

- 사운드
    - 3가지 효과음
    - 배경음악

---

# 3. 플레이 화면
- 시작 화면
  
  ![image](https://github.com/KimMaYa1/B09_LostIsland/assets/86953615/e7413f02-0455-4dbe-be3e-a6d883b1414d)
- 로딩 화면
  
  ![image](https://github.com/KimMaYa1/B09_LostIsland/assets/86953615/f4b4f29a-7bb6-4271-b13b-022d7bc94ba9)
- 프롤로그 화면
  
  ![image](https://github.com/KimMaYa1/B09_LostIsland/assets/86953615/c8ab1292-58e5-41e6-b098-35d3b930260a)
- 게임 화면
  
  ![image](https://github.com/KimMaYa1/B09_LostIsland/assets/86953615/037baa60-cf43-40b1-805f-c662c2fea126)




---
