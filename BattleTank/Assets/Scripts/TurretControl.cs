using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurretControl : MonoBehaviourPun, IPunObservable
{
    private Transform tr;
    private RaycastHit hit;
    public float rotSpeed = 5.0f;

    private PhotonView pv = null;
    private Quaternion currRot = Quaternion.identity;

    // Start is called before the first frame update
    void Awake()
    {
        tr = GetComponent<Transform>();

        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this; //PhotonView�� Observed �Ӽ��� �� ��ũ��Ʈ�� ����
        pv.Synchronization = ViewSynchronization.UnreliableOnChange; //Phton View ����ȭ �Ӽ� ����
        currRot = tr.localRotation; //�ʱ� ȸ�� �� ����
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine) //�ڽ��� ��ũ�� ���� ����
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
            {
                Vector3 relative = tr.InverseTransformPoint(hit.point);
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                tr.Rotate(0, angle * Time.deltaTime * rotSpeed, 0);
            }
        }
        else //���� ��ũ�� ���
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
