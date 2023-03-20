using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public bool Connecting = false;
    public bool Disconnecting = false;



    public GameObject LoginPanel;
    public TMP_InputField NickNameInput;    
    public TMP_Text ConnectButtonText;

    public GameObject LobbyPanel;
    public TMP_Text DisconnectButtonText;


    public GameObject CreateRoomPanel;
    public TMP_InputField RoomNameToCreat;

    public GameObject InRoomPanel;
    public TMP_Text RoomNameInRoomPanel;
    public TMP_Text ChatLog;
    public GameObject ChatBox;
    public GameObject RoomMemberList;
    public GameObject RoomMemberInfo;
    
    public GameObject JoinRoomPanel;
    public GameObject RoomList;
    public GameObject RoomInfo;

    private void Awake()
    {
        Screen.SetResolution(960, 540, false);
    }
    void Start()
    {
        LoginPanel.SetActive(true);
        LobbyPanel.SetActive(false);
        CreateRoomPanel.SetActive(false);
        InRoomPanel.SetActive(false);
        JoinRoomPanel.SetActive(false);
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
       
        //print("In lobby player list : " + PhotonNetwork.PlayerList.Length); // 룸에 있는 플레이어 리스트
        //print("In lobby count of players : " + PhotonNetwork.CountOfPlayers);// 전체 플레이어 수
            
           
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
        PhotonNetwork.JoinLobby();        

    }

    public override void OnJoinedLobby()
    {
        DisconnectButtonText.text = "접속 끊기";
        LoginPanel.SetActive(false);
        LobbyPanel.SetActive(true);

        print(PhotonNetwork.PlayerList.ToString());
        print(PhotonNetwork.PlayerList);
        print(PhotonNetwork.PlayerList.Length);

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
        LoginPanel.SetActive(true);
        LobbyPanel.SetActive(false);
    }




    public void CreateRoomButtonClickInLobby()
    {
        CreateRoomPanel.SetActive(true);
    }
    public void CreateRoomButtonClickInPanel()
    {
        PhotonNetwork.CreateRoom(RoomNameToCreat.text, new RoomOptions { MaxPlayers = 2 });
        //PhotonNetwork.JoinRoom(RoomNameToCreat.text);
    }

    public override void OnJoinedRoom()
    {
        InRoomPanel.SetActive(true);
        LobbyPanel.SetActive(false);
        JoinRoomPanel.SetActive(false);
        RoomNameInRoomPanel.text = PhotonNetwork.CurrentRoom.Name;
        delete_room_member();
        add_room_member();

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        delete_room_member();
        add_room_member();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        delete_room_member();
        add_room_member();
    }
    void delete_room_member()
    {
        for (int k = 0; k < RoomMemberList.transform.childCount; k++)
        {
            Destroy(RoomMemberList.transform.GetChild(k).gameObject);
        }
    }
    void add_room_member()
    {
        for (int k = 0; k < PhotonNetwork.PlayerList.Length; k++)
        {
            GameObject member_info = Instantiate(RoomMemberInfo);
            member_info.transform.SetParent(RoomMemberList.transform);
            string member_nickname = PhotonNetwork.PlayerList[k].NickName;
            if (PhotonNetwork.PlayerList[k].IsMasterClient)
            {
                member_nickname += " (*)";
            }
            member_info.transform.GetChild(0).GetComponent<TMP_Text>().text = member_nickname;
            member_info.SetActive(true);
        }
    }

    public void JoinRoomButtonClickInLobby()
    {        
        JoinRoomPanel.SetActive(true);        
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print("room list updated");
        UpdateRoomList(roomList);
        
    }

    void UpdateRoomList(List<RoomInfo> roomList)
    {
        for (int k = 0; k < RoomList.transform.childCount; k++)
        {
            Destroy(RoomList.transform.GetChild(k).gameObject);
        }
        print(roomList.Count);
        for (int k = 0; k < roomList.Count; k++)
        {
            if (roomList[k].RemovedFromList) continue;
            GameObject Room = Instantiate(RoomInfo);
            Room.transform.SetParent(RoomList.transform);
            Room.SetActive(true);
            Room.transform.GetChild(0).GetComponent<TMP_Text>().text = roomList[k].Name;
            string count = roomList[k].PlayerCount + " / " + roomList[k].MaxPlayers;
            Room.transform.GetChild(1).GetComponent<TMP_Text>().text = count;
            if (roomList[k].PlayerCount == roomList[k].MaxPlayers)
                Room.GetComponent<Button>().interactable = false;
        }
    }

    public void JoinRoomButtonClickInJoinPanel()
    {
        string room_name = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TMP_Text>().text;
        PhotonNetwork.JoinRoom(room_name);
    }


    public void LeaveRoomButtonClick()
    {
        PhotonNetwork.LeaveRoom();
        InRoomPanel.SetActive(false);
        LobbyPanel.SetActive(true);
    }  

    
}



