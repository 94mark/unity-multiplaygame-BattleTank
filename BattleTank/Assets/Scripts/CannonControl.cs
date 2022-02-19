using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CannonControl : MonoBehaviourPun, IPunObservable
{
    private Transform tr;
    public float rotSpeed = 100.0f;
    private PhotonView pv = null;
    private Quaternion currRot = Quaternion.identity; //원격 플레이어 탱크 포신 회전 값 변수

    // Start is called before the first frame update
    void Awake()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;
        pv.Synchronization = ViewSynchronization.UnreliableOnChange;
        currRot = tr.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(pv.IsMine)
        {
            float angle = -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * rotSpeed;
            tr.Rotate(angle, 0, 0);
        }
        else
        {
            tr.localRotation = Quaternion.Slerp(tr.localRotation, currRot, Time.deltaTime * 3.0f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(tr.localRotation);
        }
        else
        {
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
