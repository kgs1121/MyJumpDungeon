# MyJumpDungeon

## 주요기능
- 플레이어 컨트롤, 아이템 사용, 버프 아이템, 움직이는 플랫폼, 낙하 데미지, 점프대, 벽 오르기

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


![image](https://github.com/user-attachments/assets/f1419b93-f041-4d41-99ed-21c5f6f7e235)
- 닿으면 오르거나 내려갈 수 있음


## 코드 구성
- PlayerController.cs 에서 플레이어 점프, 이동, 낙하 데미지, 보는 방향, 스피드 버프 적용 구현, 벽 타기 관련 로직 구현
- TramPoline.cs 에서 점프대 로직 구현
- MovePlatform.cs 에서 움직이는 플랫폼 로직 구현
- Condition, UICondition, PlayerCondition.cs 에서 플레이어의 체력, 스테미나, 스피드 관리
- Ladder.cs 에서 벽 타기 관련 로직 구현

## 트러블 슈팅
- 낙하데미지를 체공시간에 비례하여 주려 했으나 벽에 오를때 체공시간이 계속 흘러가서 플레이어의 y좌표와 땅의 y좌표 값의 차이에 비례해 데미지를 주기로 하였습니다.

- 점프대 구현 과정에서 플레이어가 점프대 옆면에 부딪혀도 튀어오르는 문제가 발생하였습니다. 이를 해결하기 위해 검색 해본 결과 ContactPoint에서 충돌체끼리의 법선 벡터를 통해 부딪히는 방향을 구할수 있다는 것을 알게 되어 다음과 같이 해결하였습니다. ![image](https://github.com/user-attachments/assets/8fd281bc-a26a-4bbf-8158-948583bdc18f)

