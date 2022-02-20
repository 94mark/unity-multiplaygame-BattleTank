using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Awake()
    {
        CreateTank();
        PhotonNetwork.IsMessageQueueRunning = true; //���� Ŭ���� ��Ʈ��ũ �޽��� ���� �翬��
    }
    void CreateTank() //��ũ�� ���� ��Ʈ��ũ�� ����
    {
        float pos = Random.Range(-100.0f, 100.0f);
        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 20.0f, pos), Quaternion.identity, 0); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
