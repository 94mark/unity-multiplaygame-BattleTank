using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireCannon : MonoBehaviourPun
{
    public GameObject cannon = null;
    public Transform firePos;
    private AudioClip fireSfx = null;
    private AudioSource sfx = null;

    private PhotonView pv = null;


    // Start is called before the first frame update
    void Awake()
    {
        cannon = (GameObject)Resources.Load("Cannon");
        fireSfx = Resources.Load<AudioClip>("CannonFire");
        sfx = GetComponent<AudioSource>();
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {        
        if(pv.IsMine && Input.GetMouseButtonDown(0))
        {
            Fire();
            //���� ��Ʈ��ũ �÷��̾��� ��ũ�� RPC �������� Fire �Լ� ȣ��
            pv.RPC("Fire", RpcTarget.Others, null);
        }
    }
    [PunRPC]
    void Fire()
    {
        sfx.PlayOneShot(fireSfx, 1.0f);
        Instantiate(cannon, firePos.position, firePos.rotation);
    }
}
