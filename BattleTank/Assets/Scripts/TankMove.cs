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
        pv.Synchronization = ViewSynchronization.UnreliableOnChange; //������ ���� Ÿ�� ����
        pv.ObservedComponents[0] = this; //Photon Observed Components �Ӽ��� TankMove ��ũ��Ʈ ����

        if(pv.IsMine) //�����̶��
        {
            Camera.main.GetComponent<SmoothFollow>().target = camPivot;
            rbody.centerOfMass = new Vector3(0.0f, -0.5f, 0.0f);
        }
        else //���� �÷��̾��� ��ũ�� �������� �̿����� ����
        {
            rbody.isKinematic = true;
        }
        //���� ��ũ�� ��ġ, ȸ�� ���� ó���� ������ �ʱⰪ ����
        currPos = tr.position; 
        currRot = tr.rotation;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting) //���� �÷��̾��� ��ġ ���� �۽�
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else //���� �÷��̾��� ��ġ ���� ����
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine) //������ ���
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            tr.Rotate(Vector3.up * rotSpeed * h * Time.deltaTime);
            tr.Translate(Vector3.forward * v * moveSpeed * Time.deltaTime);
        }
        else //���� �÷��̾��� ���
        {
            //���� �÷��̾��� ��ũ�� ���Ź��� ��ġ/�������� �ε巴�� �̵�
            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 3.0f);
            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 3.0f);
        }
    }

}
