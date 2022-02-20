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
    public override void OnConnectedToMaster() // ���� Ŭ���忡 ������ �� �Ǹ� ȣ��Ǵ� �ݹ��Լ�
    {
        base.OnConnectedToMaster();
        Debug.Log("Entered Lobby");
        userId.text = GetUserId();
        //PhotonNetwork.JoinRandomRoom();
    }
    string GetUserId()
    {
        string userId = PlayerPrefs.GetString("USER_ID"); //���ÿ� ����� �÷��̾� �̸��� ��ȯ�ϰų� �����ϴ� �Լ�
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
        PhotonNetwork.CreateRoom("MyRoom", new RoomOptions { MaxPlayers = 20 }); //�ִ� 20�� �� �������
    }
    public void OnClickJoinRandomRoom()
    {
        PhotonNetwork.NickName = userId.text; //���� �÷��̾� �̸� ����
        PlayerPrefs.SetString("USER_ID", userId.text); //�÷��̾� �̸� ����
        PhotonNetwork.JoinRandomRoom(); //�������� ����� ������ ����
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
