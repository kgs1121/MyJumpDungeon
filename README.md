# MyJumpDungeon

## 주요기능
- 플레이어 컨트롤, 아이템 사용, 버프 아이템, 움직이는 플랫폼, 낙하 데미지, 점프대

## 게임 설명
- 점프액션 형식을 기반으로 만들었습니다.
- 이동: WASD, 보는 방향: 마우스, 점프: 스페이스바, 아이템 줍기: E, 아이템 사용: F

## 아이템 및 오브젝트 기능 설명
![image](https://github.com/user-attachments/assets/e6f568c7-63e9-4e2b-8579-0f461a8d36e7)
- 닿으면 일시적으로 이동속도 증가
  
![image](https://github.com/user-attachments/assets/ff2666a9-8d85-4f72-b630-298f993cc9c3)
- 사용 시 체력 회복

![image](https://github.com/user-attachments/assets/f6c9e38c-e7cf-461f-a417-fee31d08f8ea)
- 밟을 시 튀어오름

## 코드 구성
- PlayerController.cs 에서 플레이어 점프, 이동, 낙하 데미지, 보는 방향, 스피드 버프 적용 구현
- TramPoline.cs 에서 점프대 로직 구현
- MovePlatform.cs 에서 움직이는 플랫폼 로직 구현
- Condition, UICondition, PlayerCondition.cs 에서 플레이어의 체력, 스테미나, 스피드 관리


