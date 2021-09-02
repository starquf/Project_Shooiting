# Project_Shooting

## 1. 게임 설명
>동방 프로젝트의 시스템을 참고하여 유니티로 만든 탄막 슈팅게임입니다.

>총 2스테이지로 구성되어있고 게임시작전에 2개의 캐릭터중 하나를 선택해서 플레이할 수 있습니다.

## 2. 코드 설명
### 
다양한 탄막을 구현해보려고 노력하였고 대부분의 탄막들은 삼각함수와 코루틴, 이동속도 변경등을 활용하여 구현하였습니다.

또한 다양한 적의 패턴을 구현하기 위하여 코드를 재활용할 수 있는 FSM을 구현하여 인터페이스를 활용하여 필요한 행동만 만들어 사용하고 곂치는 코드(예를 들어 원을 그리며 쏘는 패턴)는 재활용 할 수 있습니다.
