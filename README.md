# 3D 배틀탱크 멀티플레이 대전 게임 제작
![imge1](https://user-images.githubusercontent.com/90877724/156958802-70af56f0-e1ad-450e-b209-bba35791896e.png)
![imge2](https://user-images.githubusercontent.com/90877724/156959594-748381bf-e4da-4eac-9ddb-9b1774f23374.png)

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
### 3.1 탱크 조작 기능 구현
- 카메라 세팅
	- Rotation Damping값을 조절해 이동 및 회전 시 동적인 느낌 구현
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
	- 포탄이 발사되면 Ray가 도달한 지점 발사 속도, 중력에 영향을 받아 Trail Renderer가 적용되고 Terrain과 맞닥드리는 지점에서 Explosion 이펙트가 발생
### 3.2 멀티 플레이 환경 세팅 및 플레이어 동기화
- Photon PUN2 네트워크 환경 세팅
- IPunObservable 인터페이스를 상속하고, OnPhotonSerializeView 메서드를 사용해 로컬/원격 플레이어 위치/회전/포신 관련 정보 송수신
### 3.3 탱크 Health bar 상태 구현
- 적 포탄에 피격 시 체력이 깎이고 결국 Destroy 되도록 Head Up Display 방식의 체력바 UI 제작
- 최초 hpBar 색상을 Green으로 설정, 피격시 currHP -= 20 씩 깎고, hpBar.fillAmount(백분율)가 0.6f 미만으로 떨어지면 Yellow, 0.4f 미만으로 떨어질 시 Red 색상으로 전환
- LookAt 함수를 사용하여 위치 및 회전과 상관 없게 Player의 체력 바를 메인 카메라가 바라볼 수 있도록 설정, User ID가 함께 활성화
- 탱크 체력이 0일 경우, ExplosionTank() 코루틴 함수 실행
- ExplosionTank() 함수는 탱크 파괴 시, 폭발 효과 실행, HUD 비활성화 및 탱크 MeshRenderer의 isVisible을 false로 해 투명 처리 -> 3초 후 최초 체력 복귀와 함께 HUD 및 탱크를 활성화하여 Respawn
### 3.4 로비 Scene 구현 및 메인 Scene 입장
- User ID 입력 후 Join Random Room(MaxPlayers = 20) 입장
- 방 입장 시 LoadBattleField() 코루틴 함수 실행, 씬을 이동하는 동안 포톤 클라우드 서버로부터 네트워크 메시지 수신 중단 및 백그라운드로 비동기 씬 로딩
- 메인 씬에서 GameManager 클래스 구현, Random.Range(-100.0f, 100.0f)에서 최초 탱크 생성
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
