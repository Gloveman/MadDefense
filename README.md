# MapCraft:Animal Rush
## Build your own map&play with others!
---

![image](https://github.com/Gloveman/MadDefense/assets/20718582/0c0d1df3-78a8-4830-893b-0c836b523adc)

Week2 4분반 윤현우, 이창우

- 2D 기반 1대1 멀티플레이 플랫포머 게임
- 여러 몹들과 점수 아이템, 소모 아이템 존재
- 맵 에디터를 통해 직접 맵을 만들어 서버에 업로드 가능

---

### a. 개발 팀원

- 이창우 - POSTECH 컴퓨터공학과 20학번
- 김경민 - 한양대학교 컴퓨터소프트웨어학부 22학번

---

### b. 개발환경

- Language: Unity(C# script)
- Database: JSON
- IDE: Unity editor, Visual Studio

---

### c. Game 소개

### 1.Tutorial

![image](https://github.com/Gloveman/MadDefense/assets/20718582/c15f3898-49ba-43b1-b9af-eafbc810eb34)
![image](https://github.com/Gloveman/MadDefense/assets/20718582/554a0082-4487-4603-b524-87a2133a6b92)

***Major features***
- 게임의 조작법과 아이템, 클리어 조건을 확인할 수 있는 튜토리얼 맵

---

***기술설명***
- 사용자가 입력한 직군 정보를 통해 Designer와 Developer를 구분하여 서로 다른 form을 보여줍니다. 
- 입력을 완료한 경우 onsubmit을 호출해 서버로 data를 넘기고, 서버에서 mongodb에 insert하는 방식입니다.
- 사용자가 입력한 정보는 모두 useState를 통해 내용이 변화할때마다 실시간으로 업데이트 됩니다.

---

### 2.Map editor

![image](https://github.com/Gloveman/MadDefense/assets/20718582/d1f35346-7930-4806-bff9-27bb7704a5f7)

***Major features***
- 원하는 크기로 캔버스를 생성하여 맵을 자유롭게 구성할 수 있습니다.
- 시작 위치와 클리어 위치도 정할 수 있습니다.
- 맵의 이름과 제한 시간을 설정하여 서버에 업로드할 수 있습니다.
---

***기술설명***
- 입력한 크기에 맞추어 empty tile을 instiate하며, 카메라 크기도 자동 조정됩니다. 
- TileType enum을 통해 tile, mob, item 종류를 관리합니다.


---

### 3.Multiplay

***Major features***
- Map select 화면에서 원하는 맵을 선택하여 대기실에 join할 수 있습니다.
- 두 명이 들어올시 자동으로 게임이 시작됩니다.
- 제한 시간 안에 클리어 지점에 먼저 도착한 플레이어가 승리하게 됩니다.

---

***기술설명***
- 멀티플레이 서버 및 관련 동작은 모두 Photon network를 사용했습니다. 
- RPC 등을 통해 두 클라이언트 화면의 동작, 아이템 사용 등을 동기화하게 됩니다.


