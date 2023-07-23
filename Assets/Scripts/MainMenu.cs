using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    bool isConnecting;
    private byte maxPlayersPerRoom = 2;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Connect()
    {
        isConnecting = true;
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = "1";
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        Debug.Log(isConnecting);
        if (isConnecting)
            PhotonNetwork.JoinRandomRoom();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("disconnected because of {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("creating new room....");
        PhotonNetwork.CreateRoom("holahola", new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Room joined");
        PhotonNetwork.LoadLevel("SampleScene");

    }
}
