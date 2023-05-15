using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Audio;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    


    [Header("로그인")]
    public GameObject LoginPanel;
    public TMP_InputField NickNameInput;    
    public TMP_Text ConnectButtonText;
    public bool Connecting = false;
    public bool Disconnecting = false;


    [Header("옵션, 메뉴얼")]    
    public GameObject OptionPanel;
    public TMP_Dropdown Resolution;
    
    public GameObject ManualPanel;
    public GameObject EspList;    
    public GameObject ErrorPop;
    public AudioMixer audioMixer;
    public Slider BgmSlider;
    public Slider SfxSlider;    
    private string[] EspTooltip = new string[] {
                                                "밟으면 출발 이전으로 돌아가는 폭탄을 설치한다. 폭탄엔 피아구분이 없다. 지나쳐가도 발동한다.", //0. 콰광!
                                                "상대방의 말 하나를 출발 이전으로 돌려보낸다. 쌓인 말이 있더라도 하나만 돌려보낸다.", //1. 안 돼. 돌아가.
                                                "윷이 아닌 주사위를 2개 굴린다. 각 주사위의 눈에 알맞는 윷으로 취급한다.(6은 뒷도)\n더블이 나오면 한 번 더 굴릴 수 있고, 그 후 말을 움직일 수 있다.", //2. 서양문물
                                                "상대 차례를 1번 건너뛴다.",//3. 무인도
                                                "이번 차례에 던지는 윷을 모두 다음 차례에 사용한다. 킵한 윷은 다음턴에 윷을 굴려야 사용할 수 있다.", //4. 킵이요.
                                                "말을 하나 얹어서 출발한다. 출발 가능한 말 2개 이상이 남아 있어야 활성화 가능하며, 활성화 후 윷을 굴리고 시작말을 움직이면 사용된다. 이미 나와져 있는 말을 움직이면 사용이 취소된다.", //5. 부정출발
                                                "상대의 말 하나와 내 말하나의 위치를 바꾼다. 쌓인 말이 있으면 모두 바뀐다.",//6. 초동역학위치전환기
                                                "이번 차례에 처음 굴린 윷을 복사한다.",//7. 메타몽
                                                "이번 차례에 던지는 윷을 모두 거꾸로 움직인다.", //8. 문워크
                                                "밟으면 골인칸으로 들어가는 포탈을 설치한다. 포탈엔 피아구분이 없다. 지나쳐가도 발동한다.", //9. 집으로
                                                "상대방의 능력을 따라한다. 상대방이 능력을 쓰기 전에도 사용할 수 있다.", //10. 따라큐
                                                "상대방 말 하나를 골라 자신의 말 주위 1칸에 이동시킨다. 반대로도 가능하다. 먼저 선택한 말을 두번째 선택한 말의 주위로 이동시킨다. 이동 시키려는 곳에 말이 있다면, 자신의 말 위에만 이동 가능하다.",// 11. 밀고 당기기
                                                "이번 차례에 던지는 윷이 정해진 윷으로 나온다.\n도 개 걸 윷 모 뒷도 모든 윷들 중 하나로 정해진다."
                                                };

    [Header("로비")]
    public GameObject LobbyPanel;
    public TMP_Text DisconnectButtonText;
    public GameObject LobbyButtonList;


    [Header("방 생성")]
    public GameObject CreateRoomPanel;
    public TMP_InputField RoomNameToCreat;


    [Header("방 안")]
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
    public GameObject MalBox;

    [Header("방 참가")]
    public GameObject JoinRoomPanel;
    public GameObject RoomList;
    public GameObject RoomInfo;
    public GameObject DirectRoomName;
    private float ShowErrorTime = 1.5f;
    
    public PhotonView PV;
    public GameObject WaitCanvas;
    public GameObject GameCanvas;
    public GameObject InGame;
    public GameObject BackGround;
    public GameObject GameEndPop;

    

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
        InGame.SetActive(false);
        GameCanvas.SetActive(false);

        SetBgmVolume();
        SetSfxVolume();

        LoginPanel.SetActive(true);
        LobbyPanel.SetActive(false);
        CreateRoomPanel.SetActive(false);
        InRoomPanel.SetActive(false);
        JoinRoomPanel.SetActive(false);
        OptionPanel.SetActive(false);
        ManualPanel.SetActive(false);
        ErrorPop.SetActive(false);
        EspList.SetActive(false);
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

        //print("In room player list : " + PhotonNetwork.PlayerList.Length); // 룸에 있는 플레이어 리스트
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {           
            OptionButtonClick();
        }
        
    }

    #region 시작 화면
    #region 로그인
    public void ConnectButtonClick()
    {
        if(NickNameInput.text == "")
        {
            StartCoroutine(PopErrorMsg("접속 실패", "닉네임을 입력해주세요."));
            return;
        }
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
    #endregion

    #region 옵션
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

    public void SetBgmVolume()
    {
        audioMixer.SetFloat("BGM volume", Mathf.Log10(BgmSlider.value) * 20);
    }
    public void SetSfxVolume()
    {
        audioMixer.SetFloat("SFX volume", Mathf.Log10(SfxSlider.value) * 20);
    }
    #endregion

    #region 게임설명
    public void ManualButtonClick()
    {
        ManualPanel.SetActive(true);
        EspList.SetActive(false);
        ManualPanel.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "초능력 설명 보기";
    }

    public void ClickEspToolTip()
    {
        GameObject current_clicked_button = EventSystem.current.currentSelectedGameObject;
        if (current_clicked_button != null)
        {
            int esp_num = int.Parse(current_clicked_button.name);
            print(esp_num);
            EspList.transform.GetChild(EspList.transform.childCount - 1).GetChild(1).GetComponent<TMP_Text>().text = EspList.transform.GetChild(esp_num + 1).GetChild(2).GetComponent<TMP_Text>().text;
            EspList.transform.GetChild(EspList.transform.childCount - 1).GetChild(2).GetComponent<TMP_Text>().text = EspTooltip[esp_num];
        }
        EspList.transform.GetChild(EspList.transform.childCount - 1).gameObject.SetActive(true);
    }

    public void ClickShowEsp()
    {
        bool isOn = EspList.activeSelf;
        if (isOn == false)
        {
            EspList.SetActive(true);
            ManualPanel.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "뒤로";
            EspList.transform.GetChild(EspList.transform.childCount - 1).gameObject.SetActive(false);
        }
        else
        {
            EspList.SetActive(false);
            ManualPanel.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "초능력 설명 보기";
            EspList.transform.GetChild(EspList.transform.childCount - 1).gameObject.SetActive(false);
        }
    }
    #endregion

    #endregion


    #region 로비
    #region 방 생성
    public void CreateRoomButtonClickInLobby()
    {
        CreateRoomPanel.SetActive(true);
    }
    public void CreateRoomButtonClickInPanel()
    {
        if (RoomNameToCreat.text == "")
        {
            StartCoroutine(PopErrorMsg("방 생성 실패", "방 이름을 입력해주세요."));
        }
        else
        {
            PhotonNetwork.CreateRoom(RoomNameToCreat.text, new RoomOptions { MaxPlayers = 2 });
            CreateRoomPanel.SetActive(false);
        }
        //PhotonNetwork.JoinRoom(RoomNameToCreat.text);
    }       
    
    
#endregion

#region 방 참가
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
            Room.transform.localScale = new Vector3(1, 1, 1);
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
            StartCoroutine(PopErrorMsg("방 참가 실패", "방 이름을 입력해주세요."));
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
        StartCoroutine(PopErrorMsg("방 생성 실패", message));
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        StartCoroutine(PopErrorMsg("방 참가 실패", message));
    }

    

#endregion

#endregion
#region 방

#region 방안
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
        InRoomPanel.SetActive(false);
        BackGround.SetActive(false);
        GameCanvas.SetActive(true);
        InGame.SetActive(true);
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
        if (InGame.activeSelf)
        {
            GameEndPop.transform.GetChild(0).GetComponent<TMP_Text>().text = "승리!!";
            GameEndPop.transform.GetChild(3).gameObject.SetActive(true);
            for (int t = 2; t < MalBox.transform.childCount; t++)
            {
                Destroy(MalBox.transform.GetChild(t).gameObject);
            }
            GameEndPop.SetActive(true);
        }
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

    public void CloseButtonClick()
    {
        GameObject current_clicked_button = EventSystem.current.currentSelectedGameObject;
        current_clicked_button.transform.parent.gameObject.SetActive(false);
    }
}



