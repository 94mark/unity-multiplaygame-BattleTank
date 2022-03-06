# 3D 배틀탱크 멀티플레이 대전 게임 제작
## 1. 프로젝트 개요
### 1.1 개발 인원/기간 및 포지션
- 1인, 총 3일 소요
- 프로그래밍
### 1.2 개발 환경
- unity 2020.3.16f
- 멀티 환경 : Photon Unity Network 2
- 언어 : C#
- OS : Window 10			
## 2. 개발 단계
### 2.1 벤치마킹
- 전차 전략 액션 게임 <월드 오브 탱크> 모티브
### 2.2 개발 과정
 - 프로토타입 버전 : 탱크 이동/포 사격 등 기본 기능 구현
 - 알파 타입 버전 : 멀티플레이 환경 구현 
## 3. 핵심 구현 내용 
### 3.1 멀티플레이 환경 구축
- Photon PUN2 네트워크 환경 세팅
### 3.2 탱크 조작 기능 구현
- 카메라 세팅
	+ Rotation Damping값을 조절해 이동 및 회전 시 동적인 느낌 구현
- 탱크 이동
	- Rotate(Vector3.up)과 Translate(Vector3.forward) 함수를 사용하여 탱크 이동 및 회전 기능 구현
	- Rigidbody의 centerofMass y값을 -0.5f, 질량값을 5000을 주어 무게중심이 낮은 묵직한 움직임 연출
	- Material.SetTextureOffset의 노말 텍스처 y-offset 값을 변경시켜 탱크 이동 시 무한궤도가 도는 애니메이션 구현
	- Photon View 컴포넌트를 NetworkView.isMine의 bool 타입으로 비교해 로컬 클라이언트 동기화
- 포신 조작
	- ScreenPointToRay를 사용해 메인 카메라에서 마우스 커서 위치로 캐스팅되는 Ray를 생성
	- Transform.InverseTransformPoint 함수를 사용해 Ray에 맞은 위치를 월드 좌표에서 로컬 좌표로 변환
	- Mathf.Atan2로 로컬 좌표 x, y 두 점 사이의 각도를 계산하고 Rotate 함수에 적용하여 포신 회전 기능 구현
	- Mouse ScrollWheel을 조작해 포신의 높낮이를 움직일 수 있는 기능 구현
## 4. 문제 해결 내용
### 4.1 
- 
