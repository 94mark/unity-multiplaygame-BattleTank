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
        PhotonNetwork.IsMessageQueueRunning = true; //포톤 클라우드 네트워크 메시지 수신 재연결
    }
    void CreateTank() //탱크를 포톤 네트워크로 실행
    {
        float pos = Random.Range(-100.0f, 100.0f);
        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 20.0f, pos), Quaternion.identity, 0); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
