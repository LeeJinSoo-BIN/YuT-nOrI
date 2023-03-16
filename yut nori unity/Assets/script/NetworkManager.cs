using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public bool Connecting = false;
    public bool Disconnecting = false;
    public TMP_Text ConnectButtonText;
    public TMP_Text DisconnectButtonText;
    public TMP_InputField NickNameInput;
    public GameObject BeforeConnectedLobby;
    public GameObject AfterConnectedLobby;
    void Start()
    {
        BeforeConnectedLobby.SetActive(true);
        AfterConnectedLobby.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Connecting)
        {
            ConnectButtonText.text = PhotonNetwork.NetworkClientState.ToString();
        }
        if (Disconnecting)
        {
            DisconnectButtonText.text = PhotonNetwork.NetworkClientState.ToString();
        }
    }

    public void ConnectButtonClick()
    {
        Connecting = true;
        Disconnecting = false;
        PhotonNetwork.ConnectUsingSettings();
        
    }

    public override void OnConnectedToMaster()
    {

        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        DisconnectButtonText.text = "연결 끊기";
        BeforeConnectedLobby.SetActive(false);
        AfterConnectedLobby.SetActive(true);
    }

    public void DisconnectButtonClick()
    {
        Connecting = false;
        Disconnecting = true;
        PhotonNetwork.Disconnect();
        
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        ConnectButtonText.text = "서버 접속";
        BeforeConnectedLobby.SetActive(true);
        AfterConnectedLobby.SetActive(false);
    }
    //public void CreateRoom() => PhotonNetwork.CreateRoom();
}

