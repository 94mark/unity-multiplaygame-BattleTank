using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;

public class TankMove : MonoBehaviour
{
    public float moveSpeed = 20.0f;
    public float rotSpeed = 50.0f;
    private Rigidbody rbody;
    private Transform tr;
    private float h, v;

    private PhotonView pv = null;
    public Transform camPivot;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        rbody.centerOfMass = new Vector3(0.0f, 0.5f, 0.0f);
        pv = GetComponent<PhotonView>();

        if(pv.IsMine) //로컬이라면
        {
            Camera.main.GetComponent<SmoothFollow>().target = camPivot;
            rbody.centerOfMass = new Vector3(0.0f, 0.5f, 0.0f);
        }
        else //원격 플레이어의 탱크는 물리력을 이용하지 않음
        {
            rbody.isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine) return; //로컬이 아니라면 Update 함수 실행 x

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        tr.Rotate(Vector3.up * rotSpeed * h * Time.deltaTime);
        tr.Translate(Vector3.forward * v * moveSpeed * Time.deltaTime);
    }
}
