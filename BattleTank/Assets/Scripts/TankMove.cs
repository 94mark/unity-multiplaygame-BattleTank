using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;

public class TankMove : MonoBehaviourPun, IPunObservable
{
    public float moveSpeed = 20.0f;
    public float rotSpeed = 50.0f;
    private Rigidbody rbody;
    private Transform tr;
    private float h, v;

    private PhotonView pv = null;
    public Transform camPivot;

    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

    // Start is called before the first frame update
    void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

        pv = GetComponent<PhotonView>();
        pv.Synchronization = ViewSynchronization.UnreliableOnChange; //데이터 전송 타입 설정
        pv.ObservedComponents[0] = this; //Photon Observed Components 속성에 TankMove 스크립트 연결

        if(pv.IsMine) //로컬이라면
        {
            Camera.main.GetComponent<SmoothFollow>().target = camPivot;
            rbody.centerOfMass = new Vector3(0.0f, -0.5f, 0.0f);
        }
        else //원격 플레이어의 탱크는 물리력을 이용하지 않음
        {
            rbody.isKinematic = true;
        }
        //원격 탱크의 위치, 회전 값을 처리할 변수의 초기값 설정
        currPos = tr.position; 
        currRot = tr.rotation;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting) //로컬 플레이어의 위치 정보 송신
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else //원격 플레이어의 위치 정보 수신
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine) //로컬인 경우
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            tr.Rotate(Vector3.up * rotSpeed * h * Time.deltaTime);
            tr.Translate(Vector3.forward * v * moveSpeed * Time.deltaTime);
        }
        else //원격 플레이어인 경우
        {
            //원격 플레이어의 탱크를 수신받은 위치/각도까지 부드럽게 이동
            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 3.0f);
            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 3.0f);
        }
    }

}
