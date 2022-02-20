using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    public string version = "v1.0";
    public InputField userId;

    // Start is called before the first frame update
    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }
    public override void OnConnectedToMaster() // 포톤 클라우드에 접속이 잘 되면 호출되는 콜백함수
    {
        base.OnConnectedToMaster();
        Debug.Log("Entered Lobby");
        userId.text = GetUserId();
        //PhotonNetwork.JoinRandomRoom();
    }
    string GetUserId()
    {
        string userId = PlayerPrefs.GetString("USER_ID"); //로컬에 저장된 플레이어 이름을 반환하거나 생성하는 함수
        if(string.IsNullOrEmpty(userId))
        {
            userId = "USER_" + Random.Range(0, 999).ToString("000");
        }
        return userId;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("No Room!");
        PhotonNetwork.CreateRoom("MyRoom", new RoomOptions { MaxPlayers = 20 }); //최대 20명 방 만들어줌
    }
    public void OnClickJoinRandomRoom()
    {
        PhotonNetwork.NickName = userId.text; //로컬 플레이어 이름 설정
        PlayerPrefs.SetString("USER_ID", userId.text); //플레이어 이름 저장
        PhotonNetwork.JoinRandomRoom(); //무작위로 추출된 룸으로 입장
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Enter Room");
        //CreateTank();
    }
    /*
    void CreateTank()
    {
        float pos = Random.Range(-100.0f, 100.0f);
        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 20.0f, pos), Quaternion.identity, 0);
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
