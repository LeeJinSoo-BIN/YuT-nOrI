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
    


    [Header("∑Œ±◊¿Œ")]
    public GameObject LoginPanel;
    public TMP_InputField NickNameInput;    
    public TMP_Text ConnectButtonText;
    public bool Connecting = false;
    public bool Disconnecting = false;


    [Header("ø…º«, ∏ﬁ¥∫æÛ")]    
    public GameObject OptionPanel;
    public TMP_Dropdown Resolution;
    
    public GameObject ManualPanel;
    public GameObject ErrorPop;

    [Header("∑Œ∫Ò")]
    public GameObject LobbyPanel;
    public TMP_Text DisconnectButtonText;
    public GameObject LobbyButtonList;


    [Header("πÊ ª˝º∫")]
    public GameObject CreateRoomPanel;
    public TMP_InputField RoomNameToCreat;


    [Header("πÊ æ»")]
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

    [Header("πÊ ¬¸∞°")]
    public GameObject JoinRoomPanel;
    public GameObject RoomList;
    public GameObject RoomInfo;
    public GameObject DirectRoomName;
    private float ShowErrorTime = 1.5f;
    
    public PhotonView PV;
    public GameObject WaitCanvas;
    public GameObject GameCanvas;

    

    private void Awake()
    {
#if UNITY_ANDROID
        Screen.SetResolution(1920, 1080, false);
#else
        Screen.SetResolution(960, 540, false);
#endif
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
        ManualPanel.SetActive(false);
        ErrorPop.SetActive(false);
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

        //print("In lobby player list : " + PhotonNetwork.PlayerList.Length); // ∑Îø° ¿÷¥¬ «√∑π¿ÃæÓ ∏ÆΩ∫∆Æ
        //print("In lobby count of players : " + PhotonNetwork.CountOfPlayers);// ¿¸√º «√∑π¿ÃæÓ ºˆ
        
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

#region ∑Œ±◊¿Œ
    public void ConnectButtonClick()
    {
        Connecting = true;
        Disconnecting = false;
        PhotonNetwork.ConnectUsingSettings();        
    }

    /*bool IsKorean(char ch)
    {
        if ((0xAC00 <= ch && ch <= 0xD7A3) || (0x3131 <= ch && ch <= 0x318E))
            return true;
        else
            return false;
    }
    bool IsEnglish(char ch)
    {
        if ((0x61 <= ch && ch <= 0x7A) || (0x41 <= ch && ch <= 0x5A))
            return true;
        else
            return false;
    }
    bool IsNumeric(char ch)
    {
        if (0x30 <= ch && ch <= 0x39)
            return true;
        else
            return false;
    }
    //«„øÎ«œ¥¬ πÆ¿⁄
    bool IsAllowedCharacter(char ch, string allowedCharacters)
    {
        return allowedCharacters.Contains(ch);        
    }

    void CkeckString()
    {
        string s = NickNameInput.text;
        //"«—±€§°§§§ø§√§¢§º∆RabcDEF~!@#$%^&*()_+|-=\\{}[]'\";:,.<>/? ";
        string allowCharacters = "-_[]()";
        for (int i = 0; i < s.Length; i++)
        {
            if (IsKorean(s[i]) == true)
            {
                print(s[i].ToString() + "  kor");
            }
            else if (IsEnglish(s[i]) == true)
            {
                print(s[i].ToString() + "   eng");
            }
            else if (IsNumeric(s[i]) == true)
            {
                print(s[i].ToString() + "   num");
            }
            else if (IsAllowedCharacter(s[i], allowCharacters) == true)
            {
                print(s[i].ToString() + "   allow");
            }
            else            {
                print(s[i].ToString() + "   unknown-----------");
            }
        }
    }*/

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        DisconnectButtonText.text = "¡¢º” ≤˜±‚";
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
        ConnectButtonText.text = "º≠πˆ ¡¢º”";
        LoginPanel.SetActive(true);
        LobbyPanel.SetActive(false);
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

    public void ManualButtonClick()
    {
        ManualPanel.SetActive(true);
    }

#endregion


#region ∑Œ∫Ò
#region πÊ ª˝º∫
    public void CreateRoomButtonClickInLobby()
    {
        CreateRoomPanel.SetActive(true);
    }
    public void CreateRoomButtonClickInPanel()
    {
        if (RoomNameToCreat.text == "")
        {
            StartCoroutine(PopErrorMsg("πÊ ª˝º∫ Ω«∆–", "πÊ ¿Ã∏ß¿ª ¿‘∑¬«ÿ¡÷ººø‰."));
        }
        else
        {
            PhotonNetwork.CreateRoom(RoomNameToCreat.text, new RoomOptions { MaxPlayers = 2 });
            CreateRoomPanel.SetActive(false);
        }
        //PhotonNetwork.JoinRoom(RoomNameToCreat.text);
    }
       
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        update_room_member();
        PV.RPC("send_message", RpcTarget.All, "<color=yellow>" + newPlayer.NickName + "¥‘¿Ã ¿‘¿Â«œºÃΩ¿¥œ¥Ÿ</color>");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        update_room_member();
        PV.RPC("send_message", RpcTarget.All, "<color=yellow>" + otherPlayer.NickName + "¥‘¿Ã ≈¿Â«œºÃΩ¿¥œ¥Ÿ</color>");
        PV.RPC("ready_in_room", RpcTarget.AllBuffered, false);        
    }
#endregion

#region πÊ ¬¸∞°
    public void JoinRoomButtonClickInLobby()
    {
        JoinRoomPanel.SetActive(true);
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
            member_info.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
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

    public void JoinDirectRoomButtonClickInJoinPanel()
    {
        string room_name = DirectRoomName.GetComponent<TMP_InputField>().text;
        if (room_name == "")
        {
            StartCoroutine(PopErrorMsg("πÊ ¬¸∞° Ω«∆–", "πÊ ¿Ã∏ß¿ª ¿‘∑¬«ÿ¡÷ººø‰."));
        }
        else
            PhotonNetwork.JoinRoom(room_name);
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

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        StartCoroutine(PopErrorMsg("πÊ ª˝º∫ Ω«∆–", message));
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        StartCoroutine(PopErrorMsg("πÊ ¬¸∞° Ω«∆–", message));
    }

    

#endregion

#endregion
#region πÊ

#region πÊæ»
    public void LeaveRoomButtonClick()
    {
        PhotonNetwork.LeaveRoom();        
        InRoomPanel.SetActive(false);
        LobbyPanel.SetActive(true);
        ChatInputField.text = "";
        ready_in_room(false);
    }

    void delete_chat_log()
    {
        for (int k = 0; k < ChatLogList.transform.childCount; k++)
        {
            Destroy(ChatLogList.transform.GetChild(k).gameObject);
        }
        ChatInputField.text = "";
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
#endregion
    
    void turn_on_buttons(bool onoff)
    {
        for (int k = 0; k < LobbyButtonList.transform.childCount; k++)
        {
            if (k == 2) continue;
            LobbyButtonList.transform.GetChild(k).transform.GetComponent<Button>().interactable = onoff;
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
#endregion

    IEnumerator PopErrorMsg(string error_type, string error_msg)
    {
        float timer = 0f;
        ErrorPop.SetActive(true);
        ErrorPop.transform.GetChild(2).GetComponent<TMP_Text>().text = error_type;
        ErrorPop.transform.GetChild(3).GetComponent<TMP_Text>().text = error_msg;
        while (true)
        {
            if (timer > ShowErrorTime)
            {
                break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        ErrorPop.SetActive(false);
    }
}



