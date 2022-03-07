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
- 포탄 발사
	- Trail Renderer를 사용하여 포탄 프리팹 제작
	- ExplosionCannon 코루틴 함수를 사용해 포 발사 기능 구현
	- 포탄이 발사되면 Ray가 도달한 지점 발사 속도, 중력에 영향을 받아 Trail Rederer가 적용되고 Terrain과 맞닥드리는 지점에서 Explosion 이펙트가 발생
### 3.3 플레이어 동기화 및 추가 작업
- IPunObservalbe 인터페이스를 상속하고, OnPhotonSerializeView 메서드를 사용해 로컬/원격 플레이어 위치/회전/포신 관련 정보 송수신 
- 
## 4. 문제 해결 내용
### 4.1 원격 플레이어의 움직임이 조금씩 끊기는 듯한 문제 발생
-   로컬 클라이언트의 가변적인 프레임 속도와 네트워크 상의 데이터 송수신 빈도와의 차이 때문에 발생하는 현상
-   데이터 송수신 빈도를 높이면 데이터 이용량이 많아지고, 사용자의 네트워크 환경에 의해 지연 시간 (Latency)이 발생하여 좋은 해결책이 아님
-   이동값과 회전값을 서버에서 전달받은 값으로 선형 보간(Lerp)해 동기화하는 식으로 해결
### 4.2 포 발사 관련 동기화 문제 발생
- 포탄 발사 관련하여 Transform View 컴포넌트 속성을 PhotonView.IsMine 조건문에서 사용 시 에러가 발생
- Transform View로 적용되는 속성이 Position, Scale, Rotation이기 때문에 Photon View로 접근할 수 있는 목록이 아닌 이상 함수로 접근해야 함
- 포 발사 관련 동기화 함수를 원격에서도 적용되게 하기 위해 IPunObservable 인터페이스 상속 대신, [PunRPC] 어트리뷰트 사용
- PhotonView.RPC 메서드를 사용해 원격 네트워크 플레이어의 탱크에 RPC 원격으로 Fire 함수를 호출하여 해결
