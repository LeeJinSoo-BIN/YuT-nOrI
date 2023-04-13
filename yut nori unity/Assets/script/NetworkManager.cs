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
    public GameObject LobbyButtonList;

    public GameObject CreateRoomPanel;
    public TMP_InputField RoomNameToCreat;

    public GameObject InRoomPanel;
    public TMP_Text RoomNameInRoomPanel;
    public TMP_Text ChatLog;
    public TMP_InputField ChatInputField;    
    public GameObject ChatLogList;
    public int MaxChatLog;
    public GameObject RoomMemberList;
    public GameObject RoomMemberInfo;
    public Button StartButton;
    public Button ReadyButton;
    public GameObject ReadyInRoomPanel;


    public GameObject JoinRoomPanel;
    public GameObject RoomList;
    public GameObject RoomInfo;

    public PhotonView PV;
    public GameObject WaitCanvas;
    public GameObject GameCanvas;

    public GameObject OptionPanel;
    public TMP_Dropdown Resolution;

    private void Awake()
    {
        Screen.SetResolution(960, 540, false);
    }
    void Start()
    {
        WaitCanvas.SetActive(true);
        GameCanvas.SetActive(false);

        LoginPanel.SetActive(true);
        LobbyPanel.SetActive(false);
        CreateRoomPanel.SetActive(false);
        InRoomPanel.SetActive(false);
        JoinRoomPanel.SetActive(false);
        OptionPanel.SetActive(false);
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
        
        if(CreateRoomPanel.activeSelf || InRoomPanel.activeSelf || JoinRoomPanel.activeSelf)
        {
            turn_on_buttons(false);
        }
        else
        {
            turn_on_buttons(true);
        }
        if (ChatInputField.text.Length > 0 && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {            
            PV.RPC("send_message", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName + " : " + ChatInputField.text);
            ChatInputField.text = "";            
            ChatInputField.ActivateInputField();
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
        CreateRoomPanel.SetActive(false);
        //PhotonNetwork.JoinRoom(RoomNameToCreat.text);
    }

    public override void OnJoinedRoom()
    {
        InRoomPanel.SetActive(true);
        LobbyPanel.SetActive(false);
        JoinRoomPanel.SetActive(false);
        RoomNameInRoomPanel.text = PhotonNetwork.CurrentRoom.Name;
        update_room_member();
        delete_chat_log();        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        update_room_member();
        PV.RPC("send_message", RpcTarget.All, "<color=yellow>" + newPlayer.NickName + "님이 입장하셨습니다</color>");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        update_room_member();
        PV.RPC("send_message", RpcTarget.All, "<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
        PV.RPC("ready_in_room", RpcTarget.AllBuffered, false);        
    }
    
    void update_room_member()
    {
        for (int k = 0; k < RoomMemberList.transform.childCount; k++)
        {
            Destroy(RoomMemberList.transform.GetChild(k).gameObject);
        }
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
        StartButton.interactable = PhotonNetwork.LocalPlayer.IsMasterClient;
        ReadyButton.interactable = !PhotonNetwork.LocalPlayer.IsMasterClient;
    }

    void delete_chat_log()
    {
        for (int k = 0; k < ChatLogList.transform.childCount; k++)
        {
            Destroy(ChatLogList.transform.GetChild(k).gameObject);
        }
        ChatInputField.text = "";
    }

    public void JoinRoomButtonClickInLobby()
    {        
        JoinRoomPanel.SetActive(true);        
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //print("room list updated");
        UpdateRoomList(roomList);        
    }
    

    void UpdateRoomList(List<RoomInfo> roomList)
    {
        for (int k = 0; k < RoomList.transform.childCount; k++)
        {
            Destroy(RoomList.transform.GetChild(k).gameObject);
        }
        //print(roomList.Count);
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
        ChatInputField.text = "";
        ready_in_room(false);
    }  

    void turn_on_buttons(bool onoff)
    {
        for (int k = 0; k < LobbyButtonList.transform.childCount; k++)
        {
            if (k == 2) continue;
            LobbyButtonList.transform.GetChild(k).transform.GetComponent<Button>().interactable = onoff;
        }
    }

    public void ReadyButtonClick()
    {        
        PV.RPC("ready_in_room", RpcTarget.All, !ReadyInRoomPanel.activeSelf);
    }
    public void StartButtonClick()
    {
        if (ReadyInRoomPanel.activeSelf)
        {
            PV.RPC("ready_in_room", RpcTarget.All, !ReadyInRoomPanel.activeSelf);
            PV.RPC("start_game_in_room", RpcTarget.All);
        }
    }
    
    

    [PunRPC]
    void send_message(string msg)
    {        
        TMP_Text chat = Instantiate(ChatLog);
        chat.text = msg;
        if(ChatLogList.transform.childCount == MaxChatLog)
        {
            Destroy(ChatLogList.transform.GetChild(0).gameObject);
        }
        chat.transform.SetParent(ChatLogList.transform);
        chat.gameObject.SetActive(true);
        chat.transform.localScale = new Vector3(1, 1, 1);
    }
    
    [PunRPC]
    void ready_in_room(bool isReady)
    {
        ReadyInRoomPanel.SetActive(isReady);
        string NickName = "";
        for(int k = 0; k< PhotonNetwork.PlayerList.Length; k++)
        {
            if (!PhotonNetwork.PlayerList[k].IsMasterClient)
                NickName = PhotonNetwork.PlayerList[k].NickName;

        }
        for (int k = 0; k < RoomMemberList.transform.childCount; k++)
        {
            if (RoomMemberList.transform.GetChild(k).GetChild(0).GetComponent<TMP_Text>().text == NickName)
            {
                RoomMemberList.transform.GetChild(k).GetChild(1).gameObject.SetActive(isReady);
            }

        }
    }

    [PunRPC]
    void start_game_in_room()
    {
        WaitCanvas.SetActive(false);
        GameCanvas.SetActive(true);  
    }


    public void CloseButtonClick()
    {
        GameObject current_clicked_button = EventSystem.current.currentSelectedGameObject;
        current_clicked_button.transform.parent.gameObject.SetActive(false);
    }


    public void OptionButtonClick()
    {
        OptionPanel.SetActive(!OptionPanel.activeSelf);
    }

    public void SelectResolution()
    {
        
        string selected_resolution_string = Resolution.options[Resolution.value].text;
        print(selected_resolution_string);
        string[] selected_resolution = selected_resolution_string.Split(" x ");
        Screen.SetResolution(int.Parse(selected_resolution[0]), int.Parse(selected_resolution[1]), false);
        
    }
}



