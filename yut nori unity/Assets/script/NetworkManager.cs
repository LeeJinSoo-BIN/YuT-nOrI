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
    


    [Header("�α���")]
    public GameObject LoginPanel;
    public TMP_InputField NickNameInput;    
    public TMP_Text ConnectButtonText;
    public bool Connecting = false;
    public bool Disconnecting = false;


    [Header("�ɼ�, �޴���")]    
    public GameObject OptionPanel;
    public TMP_Dropdown Resolution;
    
    public GameObject ManualPanel;
    public GameObject EspList;    
    public GameObject ErrorPop;
    public AudioMixer audioMixer;
    public Slider BgmSlider;
    public Slider SfxSlider;    
    private string[] EspTooltip = new string[] {
                                                "������ ��� �������� ���ư��� ��ź�� ��ġ�Ѵ�. ��ź�� �ǾƱ����� ����. �����İ��� �ߵ��Ѵ�.", //0. �Ɽ!
                                                "������ �� �ϳ��� ��� �������� ����������. ���� ���� �ִ��� �ϳ��� ����������.", //1. �� ��. ���ư�.
                                                "���� �ƴ� �ֻ����� 2�� ������. �� �ֻ����� ���� �˸´� ������ ����Ѵ�.(6�� �޵�)\n������ ������ �� �� �� ���� �� �ְ�, �� �� ���� ������ �� �ִ�.", //2. ���繮��
                                                "��� ���ʸ� 1�� �ǳʶڴ�.",//3. ���ε�
                                                "�̹� ���ʿ� ������ ���� ��� ���� ���ʿ� ����Ѵ�. ŵ�� ���� �����Ͽ� ���� ������ ����� �� �ִ�.", //4. ŵ�̿�.
                                                "���� �ϳ� �� ����Ѵ�. ��� ������ �� 2�� �̻��� ���� �־�� Ȱ��ȭ �����ϸ�, Ȱ��ȭ �� ���� ������ ���۸��� �����̸� ���ȴ�. �̹� ������ �ִ� ���� �����̸� ����� ��ҵȴ�.", //5. �������
                                                "����� �� �ϳ��� �� ���ϳ��� ��ġ�� �ٲ۴�. ���� ���� ������ ��� �ٲ��.",//6. �ʵ�������ġ��ȯ��
                                                "�̹� ���ʿ� ó�� ���� ���� �����Ѵ�.",//7. ��Ÿ��
                                                "�̹� ���ʿ� ������ ���� ��� �Ųٷ� �����δ�.", //8. ����ũ
                                                "������ ����ĭ���� ���� ��Ż�� ��ġ�Ѵ�. ��Ż�� �ǾƱ����� ����. �����İ��� �ߵ��Ѵ�.", //9. ������
                                                "������ �ɷ��� �����Ѵ�. ������ �ɷ��� ���� ������ ����� �� �ִ�.", //10. ����ť
                                                "���� �� �ϳ��� ��� �ڽ��� �� ���� 1ĭ�� �̵���Ų��. �ݴ�ε� �����ϴ�. ���� ������ ���� �ι�° ������ ���� ������ �̵���Ų��. �̵� ��Ű���� ���� ���� �ִٸ�, �ڽ��� �� ������ �̵� �����ϴ�.",// 11. �а� ����
                                                "�̹� ���ʿ� ������ ���� ������ ������ ���´�.\n�� �� �� �� �� �޵� ��� ���� �� �ϳ��� ��������."
                                                };

    [Header("�κ�")]
    public GameObject LobbyPanel;
    public TMP_Text DisconnectButtonText;
    public GameObject LobbyButtonList;


    [Header("�� ����")]
    public GameObject CreateRoomPanel;
    public TMP_InputField RoomNameToCreat;


    [Header("�� ��")]
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

    [Header("�� ����")]
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

        //print("In room player list : " + PhotonNetwork.PlayerList.Length); // �뿡 �ִ� �÷��̾� ����Ʈ
        //print("In lobby count of players : " + PhotonNetwork.CountOfPlayers);// ��ü �÷��̾� ��
        
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

    #region ���� ȭ��
    #region �α���
    public void ConnectButtonClick()
    {
        if(NickNameInput.text == "")
        {
            StartCoroutine(PopErrorMsg("���� ����", "�г����� �Է����ּ���."));
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
        DisconnectButtonText.text = "���� ����";
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
        ConnectButtonText.text = "���� ����";
        LoginPanel.SetActive(true);
        LobbyPanel.SetActive(false);
    }
    #endregion

    #region �ɼ�
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

    #region ���Ӽ���
    public void ManualButtonClick()
    {
        ManualPanel.SetActive(true);
        EspList.SetActive(false);
        ManualPanel.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "�ʴɷ� ���� ����";
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
            ManualPanel.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "�ڷ�";
            EspList.transform.GetChild(EspList.transform.childCount - 1).gameObject.SetActive(false);
        }
        else
        {
            EspList.SetActive(false);
            ManualPanel.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "�ʴɷ� ���� ����";
            EspList.transform.GetChild(EspList.transform.childCount - 1).gameObject.SetActive(false);
        }
    }
    #endregion

    #endregion


    #region �κ�
    #region �� ����
    public void CreateRoomButtonClickInLobby()
    {
        CreateRoomPanel.SetActive(true);
    }
    public void CreateRoomButtonClickInPanel()
    {
        if (RoomNameToCreat.text == "")
        {
            StartCoroutine(PopErrorMsg("�� ���� ����", "�� �̸��� �Է����ּ���."));
        }
        else
        {
            PhotonNetwork.CreateRoom(RoomNameToCreat.text, new RoomOptions { MaxPlayers = 2 });
            CreateRoomPanel.SetActive(false);
        }
        //PhotonNetwork.JoinRoom(RoomNameToCreat.text);
    }       
    
    
#endregion

#region �� ����
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
            StartCoroutine(PopErrorMsg("�� ���� ����", "�� �̸��� �Է����ּ���."));
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
        StartCoroutine(PopErrorMsg("�� ���� ����", message));
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        StartCoroutine(PopErrorMsg("�� ���� ����", message));
    }

    

#endregion

#endregion
#region ��

#region ���
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
        PV.RPC("send_message", RpcTarget.All, "<color=yellow>" + newPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        update_room_member();
        PV.RPC("send_message", RpcTarget.All, "<color=yellow>" + otherPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
        PV.RPC("ready_in_room", RpcTarget.AllBuffered, false);
        if (InGame.activeSelf)
        {
            GameEndPop.transform.GetChild(0).GetComponent<TMP_Text>().text = "�¸�!!";
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



