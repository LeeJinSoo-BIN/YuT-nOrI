using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGame : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    #region define
    public Sprite[] YutGarak;
    public Sprite[] YutAnimal;
    public Sprite[] Ganzi;
    public Sprite[] ESP_sprite;
    public Sprite[] Esp_Dice_sprite;
    private string[] YutHanguel = new string[] { "도!", "개!", "걸!", "윳!", "모!", "뒷도!", "낙!", "도착!" };
    private Color[] YutColor = new Color[] { new Color(255 / 255f, 223 / 255f, 206 / 255f, 1f),
                                            new Color(255 / 255f, 170 / 255f, 90 / 255f, 1f),
                                            new Color(222 / 255f, 219 / 255f, 236 / 255f, 1f),
                                            new Color(55 / 255f, 94 / 255f, 126 / 255f, 1f),
                                            new Color(228 / 255f, 147 / 255f, 107 / 255f, 1f),
                                            new Color(206 / 255f, 255 / 255f, 223 / 255f, 1f),
                                            new Color(200 / 255f, 200 / 255f, 200 / 255f, 0.5f),
                                            new Color(255 / 255f, 128 / 255f, 128 / 255f, 1f)};
    private string[] EspTooltip = new string[] {
                                                "밟으면 출발 이전으로 돌아가는 폭탄을 설치한다. 폭탄엔 피아구분이 없다. 지나쳐가도 발동한다.", //0. 콰광!
                                                "상대방의 말 하나를 출발 이전으로 돌려보낸다. 쌓인 말이 있더라도 하나만 돌려보낸다.", //1. 안 돼. 돌아가.
                                                "윷이 아닌 주사위를 2개 굴린다. 각 주사위의 눈에 알맞는 윷으로 취급한다.(6은 뒷도) 더블이 나오면 한 번 더 굴린다.", //2. 서양문물
                                                "상대 차례를 1번 건너뛴다.",//3. 무인도
                                                "이번 차례에 던지는 윷을 모두 다음 차례에 사용한다. 킵한 윷은 다음턴에 윷을 굴려야 사용할 수 있다.", //4. 킵이요.
                                                "말을 하나 얹어서 출발한다. 출발 가능한 말 2개 이상이 남아 있어야 활성화 가능하며, 활성화 후 윷을 굴리고 시작말을 움직이면 사용된다. 이미 나와져 있는 말을 움직이면 사용이 취소된다.", //5. 부정출발
                                                "상대의 말 하나와 내 말하나의 위치를 바꾼다. 쌓인 말이 있으면 모두 바뀐다.",//6. 초동역학위치전환기
                                                "이번 차례에 처음 굴린 윷을 복사한다.",//7. 메타몽
                                                "이번 차례에 던지는 윷을 모두 거꾸로 움직인다.", //8. 문워크
                                                "밟으면 골인칸으로 들어가는 포탈을 설치한다. 포탈엔 피아구분이 없다. 지나쳐가도 발동한다.", //9. 집으로
                                                "상대방의 능력을 따라한다. 상대방이 능력을 쓰기 전에도 사용할 수 있다.", //10. 따라큐
                                                "상대방 말 하나를 골라 자신의 말 주위 1칸에 이동시킨다. 반대로도 가능하다. 먼저 선택한 말을 두번째 선택한 말의 주위로 이동시킨다. 이동 시키려는 곳에 말이 있다면, 자신의 말 위에만 이동 가능하다.",// 11. 밀고 당기기
                                                "이번 차례에 던지는 윷이 확정적으로 도가 된다.", //12. 신의손 도
                                                "이번 차례에 던지는 윷이 확정적으로 개가 된다.", //13. 신의손 개
                                                "이번 차례에 던지는 윷이 확정적으로 걸이 된다.", //14. 신의손 걸
                                                "이번 차례에 던지는 윷이 확정적으로 윷이 된다.", //15. 신의손 윷
                                                "이번 차례에 던지는 윷이 확정적으로 모가 된다.", //16. 신의손 모
                                                "이번 차례에 던지는 윷이 확정적으로 뒷도가 된다." //17. 신의손 뒷도
                                                };

    [Header("setting")]
    public GameObject MyInfoList;
    public GameObject OpInfoList;
    private TMP_Text MyName;
    private TMP_Text OpName;
    private GameObject MyStartMalList;
    private GameObject OpStartMalList;
    private GameObject MyEspList;
    private GameObject OpEspList;
    private GameObject MyYutStackList;
    private GameObject OpYutStackList;
    private GameObject MyTurnSign;
    private GameObject OpTurnSign;
    private GameObject MyChatBubbleBox;
    private GameObject OpChatBubbleBox;
    private GameObject MyChatBubble;
    private GameObject OpChatBubble;

    [Header("Roll Yut")]
    public Button RollButton;
    public GameObject RollingYut;
    public float BackProbability = 42f;
    //private float FrontProbability = 58f;
    public int CurrentYut;
    public bool MyTurn = false;
    public float RollingTime = 1f;
    public float RollingResultTime = 1f;

    public bool IsRollable = false;
    public bool IsMalMovable = false;
    public bool IsRolling = false;
    public bool IsMoving = false;
    public float MoveSpeed = 3.5f;


    [Header("Mal Setting")]
    public GameObject Caan;
    public GameObject MalBox;
    private GameObject MyMovingMal;
    private GameObject OpMovingMal;
    private GameObject MyClickedMovableMal;
    private Sprite MyMalImage;
    private Sprite OpMalImage;
    private bool IsMyStartMalClicked = false;
    private bool IsMyMovedMalClicked = false;

    [Header("Chat")]
    public TMP_InputField InGameChatInputField;

    public float BubbleSpeed = 10f;
    public float BubbleTime = 2f;

    [Header("ESP")]
    public GameObject PopEsp;
    private GameObject ESPList;
    public GameObject PopEspUsing;
    private GameObject MyEspTooltipBox;
    private GameObject OpEspTooltipBox;
    public int MyEsp1;
    public int MyEsp2;
    public int OpEsp1;
    public int OpEsp2;
    private int LoopEspShowCnt = 2;
    public float EspPopTime = 0.02f;
    public float EspPopSlowDown = 1.2f;
    private bool EspDone = false;
    private bool IsEsp1Using = false;
    private bool IsEsp2Using = false;
    private bool IsEsp1Used = false;    
    private bool IsEsp2Used = false;
    private bool IsRolled = false;
    private int[] TrapType = new int[] { 1, 2 }; // 1 == 콰쾅, 2 == 집으로
    public bool IsEspIslandUsing = false;
    private bool IsEspKeepUsing = false;
    private bool IsEspGoBackUsing = false;
    private bool IsEspFalseStartUsing = false;
    private bool IsEspMetamongUsing = false;
    private bool IsEspChangePosUsing = false;
    private bool IsEspMagnetUsing = false;
    private int EspChangeIndex1 = -1;
    private int EspChangeIndex2 = -1;
    private int EspMagnetMovingMalIndex = -1;
    public bool EspMimikyuUsing = false;
    public bool EspMimikyuUsed = false;



    [Header("Turn")]
    public GameObject PopTurn;
    public float ShowPopTime = 3f;
    public float VenishPopSpeed = 3f;
    private bool WaitChanging = false;


    [Header("Game Start & End")]
    public GameObject GameEnd;
    public GameObject Ready;
    public GameObject GameStart;
    public bool GameStarted = false;
    public GameObject WaitCanvas;
    public GameObject GameCanvas;

    public PhotonView PV;

    #endregion

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameStart.transform.childCount == 2 && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            GameObject ready = Instantiate(Ready);
            ready.transform.SetParent(GameStart.transform);
            PV.RPC("change_turn", RpcTarget.All);
        }

        if (GameStarted)
        {
            if (MyTurn)
            {
                if (IsRolled)
                {
                    MyEspList.transform.GetChild(1).GetComponent<Button>().interactable = false;
                    MyEspList.transform.GetChild(2).GetComponent<Button>().interactable = false;
                }
                

                IsMalMovable = Check_Mal_Movable(MyYutStackList, MyStartMalList);
                if (!IsRollable && !IsMalMovable && !IsRolling && !IsMoving && MyTurn && EspDone && !WaitChanging)
                {
                    PV.RPC("change_turn", RpcTarget.All);
                }
                if (IsMalMovable && !IsMoving && !IsRolling)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                        if (hit.collider != null)
                        {
                            if (hit.transform.parent.gameObject == MalBox && hit.transform.name[0] == 'M')
                            {
                                MovedMalClick(hit.transform.gameObject);
                            }
                        }
                    }
                }
                if ((IsEspGoBackUsing  || IsEspChangePosUsing || IsEspMagnetUsing) && !IsEsp1Used)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                        if (hit.collider != null)
                        {
                            if (hit.transform.parent.gameObject == MalBox && hit.transform.name[0] == 'O')
                            {
                                OpMalClick(hit.transform.gameObject);                                
                            }
                            if (hit.transform.parent.gameObject == MalBox && hit.transform.name[0] == 'M')
                            {
                                MyMalClick(hit.transform.gameObject);
                            }
                        }
                    }
                }
            }
            else
            {
                IsRollable = false;
                IsMalMovable = false;
                MyEspList.transform.GetChild(1).GetComponent<Button>().interactable = false;
                MyEspList.transform.GetChild(2).GetComponent<Button>().interactable = false;
                for (int k = 0; k < 4; k++)
                {
                    MyStartMalList.transform.GetChild(k).GetComponent<Button>().interactable = false;
                }
            }
            RollButton.interactable = (IsRollable && !IsMoving && !IsRolling);

            if (InGameChatInputField.text.Length > 0 &&
              (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                PV.RPC("send_message_in_game", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName, InGameChatInputField.text);
                InGameChatInputField.text = "";
                InGameChatInputField.ActivateInputField();
            }
        }
    }

    #region Roll
    public void RollButtonClick()
    {
        IsRollable = false;
        int back_cnt = 0;
        int back_do = 0;
        for (int k = 0; k < 4; k++) {
            int yut = Random.Range(0, 100);
            if (yut < BackProbability)
            {
                back_cnt++;
                if (k == 0)
                    back_do++;
            }
            else if (yut == BackProbability)
            {
                back_cnt = 7; // 낙 7
                break;
            }
        }
        if (back_cnt == 1 && back_do == 1)
        {
            CurrentYut = 6; // 뒷도 6
        }
        else if (back_cnt == 0)
        {
            CurrentYut = 5; // 모 5
        }
        else
        {
            CurrentYut = back_cnt;
        }
        CurrentYut--;//도0 개1 걸2 윷3 모4 뒷도5 낙6
        if (IsEsp1Using && !IsEsp1Used)
        {
            if (IsEspIslandUsing || IsEspFalseStartUsing || IsEspMetamongUsing)
            {

            }
            else
            {
                Esp1ButtonClick();
            }
        }

        if (IsEsp2Using)
        {
            CurrentYut = MyEsp2;
            IsEsp2Used = true;
        }
        PV.RPC("roll_yut", RpcTarget.All, CurrentYut, IsEsp2Using);
    }

    [PunRPC]
    void roll_yut(int rolled_yut, bool esp2using)
    {
        RollingYut.SetActive(true);
        IsRolling = true;
        IsRollable = false;
        RollingYut.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = YutGarak[7];
        RollingYut.transform.GetChild(2).gameObject.SetActive(false);
        RollingYut.transform.GetChild(3).gameObject.SetActive(false);
        CurrentYut = rolled_yut;
        IsEsp2Using = esp2using;
        if (IsEsp2Using)
        {
            if (MyTurn)
            {
                MyEspList.transform.GetChild(2).GetComponent<Button>().interactable = false;
                MyEspList.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                MyEspList.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                OpEspList.transform.GetChild(2).GetComponent<Image>().sprite = ESP_sprite[OpEsp2 + ESPList.transform.childCount - 6];
                OpEspList.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
            }

        }
        IsRolled = true;
        StartCoroutine(show_rolling());
    }

    IEnumerator show_rolling()
    {
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer >= RollingTime)
            {
                break;
            }
            yield return null;
        }
        show_rolled_yut_and_stack_up();
        timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer >= RollingResultTime)
            {
                break;
            }
            yield return null;
        }
        action_rolling_result();
    }

    void show_rolled_yut_and_stack_up()
    {
        int rolled_yut = CurrentYut;
        RollingYut.transform.GetChild(2).gameObject.SetActive(true);
        RollingYut.transform.GetChild(3).gameObject.SetActive(true);

        if (IsEsp2Using)
        {
            RollingYut.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = ESP_sprite[rolled_yut + ESPList.transform.childCount - 6];
            RollingYut.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
            RollingYut.transform.GetChild(3).GetComponent<TMP_Text>().text = "신의 손!! ";
            RollingYut.transform.GetChild(3).GetComponent<TMP_Text>().text += YutHanguel[rolled_yut];
            if (!MyTurn)
            {
                OpEspList.transform.GetChild(2).GetComponent<Image>().sprite = ESP_sprite[rolled_yut + ESPList.transform.childCount - 6];
            }
            IsEsp2Using = false;
        }
        else
        {
            RollingYut.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = YutGarak[rolled_yut];
            RollingYut.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = YutAnimal[rolled_yut];
            RollingYut.transform.GetChild(3).GetComponent<TMP_Text>().text = YutHanguel[rolled_yut];
        }
        int stack_up = 1;
        if (IsEspMetamongUsing)
        {
            IsEspMetamongUsing = false;
            IsEsp1Using = false;
            if (rolled_yut == 6)
            {
                if (MyTurn)
                    MyEspList.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                stack_up = 2;
                if (MyTurn)
                    IsEsp1Used = true;
                int[] esp_stack = { 7, 7 };
                string[] ment = { "메타몽", "메타몽" };
                StartCoroutine(show_esp_used(esp_stack, ment, false));
                Esp1Used();
            }
        }
        if (rolled_yut != 6) // 낙
        {
            
            if (MyTurn)
            {
                int now_count = int.Parse(MyYutStackList.transform.GetChild(rolled_yut).GetChild(2).GetComponent<TMP_Text>().text) + stack_up;
                MyYutStackList.transform.GetChild(rolled_yut).GetChild(2).GetComponent<TMP_Text>().text = (now_count).ToString();
                if (now_count > 0)
                    MyYutStackList.transform.GetChild(rolled_yut).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                int now_count = int.Parse(OpYutStackList.transform.GetChild(rolled_yut).GetChild(2).GetComponent<TMP_Text>().text) + stack_up;
                OpYutStackList.transform.GetChild(rolled_yut).GetChild(2).GetComponent<TMP_Text>().text = (now_count).ToString();
                if (now_count > 0)
                    OpYutStackList.transform.GetChild(rolled_yut).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
    void action_rolling_result()
    {
        int rolled_yut = CurrentYut;
        RollingYut.SetActive(false);
        if (MyTurn)
        {
            if (IsEspKeepUsing)
            {
                PV.RPC("Esp1Used", RpcTarget.All);
                IsEsp1Used = true;
            }
            else if (Check_Mal_Movable(MyYutStackList, MyStartMalList))
            {
                for (int t = 0; t < 4; t++)
                {
                    MyStartMalList.transform.GetChild(t).GetComponent<Button>().interactable = true;
                }
            }
            else
            {
                for (int t = 0; t < 4; t++)
                {
                    MyStartMalList.transform.GetChild(t).GetComponent<Button>().interactable = false;
                }
                for (int k = 0; k < 6; k++)
                {
                    MyYutStackList.transform.GetChild(k).GetComponent<Image>().color = new Color(100 / 255f, 100 / 255f, 100 / 255f, 128 / 255f);
                    MyYutStackList.transform.GetChild(k).GetChild(2).GetComponent<TMP_Text>().text = "0";
                }
            }

            if (rolled_yut == 3 || rolled_yut == 4) //윷 모
            {
                IsRollable = true;
            }

        }
        else
        {
            if (!Check_Mal_Movable(OpYutStackList, OpStartMalList))
            {
                for (int k = 0; k < 6; k++)
                {
                    OpYutStackList.transform.GetChild(k).GetComponent<Image>().color = new Color(100 / 255f, 100 / 255f, 100 / 255f, 128 / 255f);
                    OpYutStackList.transform.GetChild(k).GetChild(2).GetComponent<TMP_Text>().text = "0";
                }
            }
        }

        if (IsEspFalseStartUsing && (rolled_yut == 5|| rolled_yut == 6))
        {
            IsEspFalseStartUsing = false;
            if (MyTurn)
            {
                IsEsp1Using = false;
                MyEspList.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            }
        }
        IsRolling = false;
    }

    #endregion

     #region Move
    public void MyStartMalClick()
    {
        if (IsMyMovedMalClicked)
        {
            IsMyMovedMalClicked = false;
            MyClickedMovableMal.transform.GetChild(3).gameObject.SetActive(false);
        }
        MyClickedMovableMal = EventSystem.current.currentSelectedGameObject;
        if (MyClickedMovableMal == null)
            return;
        IsMyStartMalClicked = !IsMyStartMalClicked;
        
        
        MyClickedMovableMal.transform.GetChild(0).gameObject.SetActive(IsMyStartMalClicked);
        if (IsMyStartMalClicked)
        {
            MyClickedMovableMal = EventSystem.current.currentSelectedGameObject;
            turn_on_off_all_caan(false);
            turn_on_off_all_moved_mal(false);
            for (int k = 0; k < 5; k++)
            {
                int yut = int.Parse(MyYutStackList.transform.GetChild(k).GetChild(2).GetComponent<TMP_Text>().text);
                if (yut > 0)
                {
                    Caan.transform.GetChild(k + 1).GetComponent<Button>().interactable = true;
                    Caan.transform.GetChild(k + 1).GetChild(0).GetComponent<Image>().color = YutColor[k];
                    Caan.transform.GetChild(k + 1).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = YutHanguel[k];
                    Caan.transform.GetChild(k + 1).GetChild(0).GetChild(0).name = (k).ToString();
                    Caan.transform.GetChild(k + 1).GetChild(0).gameObject.SetActive(true);
                }
            }
            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                if (MalBox.transform.GetChild(k).name[0] == 'M')
                    MalBox.transform.GetChild(k).GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else
        {
            turn_on_off_all_caan(false);
            turn_on_off_all_moved_mal(true);
        }
    }

    public void MovedMalClick(GameObject clicked_moved_mal)
    {
        IsMyMovedMalClicked = !IsMyMovedMalClicked;        
        if (IsMyStartMalClicked)
        {
            MyClickedMovableMal.transform.GetChild(0).gameObject.SetActive(false);
            IsMyStartMalClicked = false;
        }        
        MyClickedMovableMal = clicked_moved_mal;
        MyClickedMovableMal.transform.GetChild(3).gameObject.SetActive(IsMyMovedMalClicked);
        if (IsMyMovedMalClicked)
        {
            print("moved mal click" + clicked_moved_mal.name + " " + clicked_moved_mal.transform.GetChild(2).name);
            turn_on_off_all_caan(false);
            turn_on_off_all_moved_mal(false);
            clicked_moved_mal.GetComponent<BoxCollider2D>().enabled = true;

            int clicked_mal_pos = int.Parse(clicked_moved_mal.transform.GetChild(2).name);
            int adj = 0;
            for (int k = 0; k < 5; k++)
            {
                int yut = int.Parse(MyYutStackList.transform.GetChild(k).GetChild(2).GetComponent<TMP_Text>().text);
                if (yut > 0)
                {
                    if ((((clicked_mal_pos + k + 1 > 30) && (clicked_mal_pos > 25 || clicked_mal_pos == 22)) || clicked_mal_pos == 0) ||
                        ((clicked_mal_pos + k + 1 > 20) && (clicked_mal_pos) < 20))
                    {
                        Caan.transform.GetChild(30).GetComponent<Button>().interactable = true;
                        Caan.transform.GetChild(30).GetChild(0).GetComponent<Image>().color = YutColor[7];
                        Caan.transform.GetChild(30).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = YutHanguel[7];
                        Caan.transform.GetChild(30).GetChild(0).GetChild(0).name = (k).ToString();
                        Caan.transform.GetChild(30).GetChild(0).gameObject.SetActive(true);
                        Caan.transform.GetChild(0).gameObject.SetActive(false);
                        Caan.transform.GetChild(30).gameObject.SetActive(true);
                    }
                    else {
                        if ((clicked_mal_pos + k + 1 == 20) && clicked_mal_pos < 20 || (clicked_mal_pos + k + 1 == 30))
                        {
                            adj = -(clicked_mal_pos + k + 1);
                            Caan.transform.GetChild(0).gameObject.SetActive(true);
                            Caan.transform.GetChild(30).gameObject.SetActive(false);
                        }
                        else if (clicked_mal_pos + k + 1 == 22)
                            adj = 5;
                        else if (clicked_mal_pos == 5 || clicked_mal_pos == 10)
                        {
                            adj = 14;
                            if (clicked_mal_pos + k + 1 + adj == 22)
                            {
                                adj = 19;
                            }
                        }
                        else if (clicked_mal_pos + k + 1 + adj >= 25 && (20 <= clicked_mal_pos && clicked_mal_pos <= 24) && clicked_mal_pos != 22)
                        {
                            adj = -10;
                        }
                        Caan.transform.GetChild(clicked_mal_pos + k + 1 + adj).GetComponent<Button>().interactable = true;
                        Caan.transform.GetChild(clicked_mal_pos + k + 1 + adj).GetChild(0).GetComponent<Image>().color = YutColor[k];
                        Caan.transform.GetChild(clicked_mal_pos + k + 1 + adj).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = YutHanguel[k];
                        Caan.transform.GetChild(clicked_mal_pos + k + 1 + adj).GetChild(0).GetChild(0).name = (k).ToString();
                        Caan.transform.GetChild(clicked_mal_pos + k + 1 + adj).GetChild(0).gameObject.SetActive(true);
                    }
                }
            }
            if (int.Parse(MyYutStackList.transform.GetChild(5).GetChild(2).GetComponent<TMP_Text>().text) > 0)
            {
                if (clicked_mal_pos == 1)
                {
                    Caan.transform.GetChild(0).gameObject.SetActive(true);
                    Caan.transform.GetChild(30).gameObject.SetActive(false);
                    adj = 0;
                }
                if (clicked_mal_pos == 0)
                {
                    Caan.transform.GetChild(29).GetComponent<Button>().interactable = true;
                    Caan.transform.GetChild(29).GetChild(0).GetComponent<Image>().color = YutColor[5];
                    Caan.transform.GetChild(29).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = YutHanguel[5];
                    Caan.transform.GetChild(29).GetChild(0).GetChild(0).name = (5).ToString();
                    Caan.transform.GetChild(29).GetChild(0).gameObject.SetActive(true);
                    adj = 20;
                }
                else if (clicked_mal_pos == 15)
                {
                    Caan.transform.GetChild(24).GetComponent<Button>().interactable = true;
                    Caan.transform.GetChild(24).GetChild(0).GetComponent<Image>().color = YutColor[5];
                    Caan.transform.GetChild(24).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = YutHanguel[5];
                    Caan.transform.GetChild(24).GetChild(0).GetChild(0).name = (5).ToString();
                    Caan.transform.GetChild(24).GetChild(0).gameObject.SetActive(true);
                    adj = 0;
                }
                else if (clicked_mal_pos == 20 || clicked_mal_pos == 25)
                {
                    adj = -14;
                }
                else if (clicked_mal_pos == 23)
                {
                    adj = 5;
                }
                else if (clicked_mal_pos == 27)
                {
                    Caan.transform.GetChild(21).GetComponent<Button>().interactable = true;
                    Caan.transform.GetChild(21).GetChild(0).GetComponent<Image>().color = YutColor[5];
                    Caan.transform.GetChild(21).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = YutHanguel[5];
                    Caan.transform.GetChild(21).GetChild(0).GetChild(0).name = (5).ToString();
                    Caan.transform.GetChild(21).GetChild(0).gameObject.SetActive(true);
                    adj = 0;
                }
                else
                    adj = 0;
                Caan.transform.GetChild(clicked_mal_pos + -1 + adj).GetComponent<Button>().interactable = true;
                Caan.transform.GetChild(clicked_mal_pos + -1 + adj).GetChild(0).GetComponent<Image>().color = YutColor[5];
                Caan.transform.GetChild(clicked_mal_pos + -1 + adj).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = YutHanguel[5];
                Caan.transform.GetChild(clicked_mal_pos + -1 + adj).GetChild(0).GetChild(0).name = (5).ToString();
                Caan.transform.GetChild(clicked_mal_pos + -1 + adj).GetChild(0).gameObject.SetActive(true);
            }
        }
        else
        {
            turn_on_off_all_caan(false);
            turn_on_off_all_moved_mal(true);
        }
    }

    public void CaanClick()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            return;
        GameObject current_clicked_caan = EventSystem.current.currentSelectedGameObject;
        print(current_clicked_caan.name);
        int clicked_caan_num = int.Parse(current_clicked_caan.name);
        if (IsEspMagnetUsing)
        {
            if (EspMagnetMovingMalIndex != -1)
                PV.RPC("use_magnet", RpcTarget.All, EspMagnetMovingMalIndex, clicked_caan_num);            
            return;
        }        
        int moving_yut = int.Parse(current_clicked_caan.transform.GetChild(0).GetChild(0).name);
        if (IsMyStartMalClicked)
        {
            turn_on_off_all_caan(false);
            int my_clicked_start_mal_num = MyClickedMovableMal.name[4] - '1';
            MyClickedMovableMal.transform.GetChild(0).gameObject.SetActive(false);
            PV.RPC("moving_start_mal", RpcTarget.All, my_clicked_start_mal_num, clicked_caan_num, moving_yut);
            IsMyStartMalClicked = false;
        }
        if (IsMyMovedMalClicked)
        {
            turn_on_off_all_caan(false);
            int clicked_mal_pos = int.Parse(MyClickedMovableMal.transform.GetChild(2).name);
            MyClickedMovableMal.transform.GetChild(3).gameObject.SetActive(false);
            PV.RPC("moving_moved_mal", RpcTarget.All, clicked_mal_pos, clicked_caan_num, moving_yut);
            IsMyMovedMalClicked = false;
        }
    }

    [PunRPC]
    void moving_start_mal(int clicked_start_mal_num, int clicked_caan_num, int moving_yut)
    {
        IsMoving = true;
        GameObject moving_mal;
        GameObject current_clicked_caan = Caan.transform.GetChild(clicked_caan_num).gameObject;
        if (MyTurn)
        {
            GameObject current_clicked_mal = MyStartMalList.transform.GetChild(clicked_start_mal_num).gameObject;
            current_clicked_mal.SetActive(false);
            moving_mal = Instantiate(MyMovingMal);
            int yut_stack = int.Parse(MyYutStackList.transform.GetChild(clicked_caan_num - 1).GetChild(2).GetComponent<TMP_Text>().text);
            MyYutStackList.transform.GetChild(clicked_caan_num - 1).GetChild(2).GetComponent<TMP_Text>().text = (yut_stack - 1).ToString();
            if (yut_stack - 1 == 0)
            {
                MyYutStackList.transform.GetChild(clicked_caan_num - 1).GetComponent<Image>().color = new Color(100 / 255f, 100 / 255f, 100 / 255f, 128 / 255f);
            }
        }
        else
        {
            GameObject current_clicked_mal = OpStartMalList.transform.GetChild(clicked_start_mal_num).gameObject;
            current_clicked_mal.SetActive(false);
            moving_mal = Instantiate(OpMovingMal);
            int yut_stack = int.Parse(OpYutStackList.transform.GetChild(clicked_caan_num - 1).GetChild(2).GetComponent<TMP_Text>().text);
            OpYutStackList.transform.GetChild(clicked_caan_num - 1).GetChild(2).GetComponent<TMP_Text>().text = (yut_stack - 1).ToString();
            if (yut_stack - 1 == 0)
            {
                OpYutStackList.transform.GetChild(clicked_caan_num - 1).GetComponent<Image>().color = new Color(100 / 255f, 100 / 255f, 100 / 255f, 128 / 255f);
            }
        }
        moving_mal.transform.SetParent(MalBox.transform);
        moving_mal.transform.position = Caan.transform.GetChild(0).position;
        moving_mal.transform.GetChild(1).GetComponent<TMP_Text>().text = "1";
        moving_mal.transform.GetChild(2).GetComponent<TMP_Text>().text = "0";
        moving_mal.transform.GetChild(2).name = "0";
        moving_mal.name += clicked_start_mal_num.ToString();
        moving_mal.SetActive(true);

        if (IsEspFalseStartUsing)
        {
            if (MyTurn)
            {
                for (int k = 0; k < 4; k++)
                {
                    if (MyStartMalList.transform.GetChild(k).gameObject.activeSelf && !MyStartMalList.transform.GetChild(k + 4).gameObject.activeSelf)
                    {
                        MyStartMalList.transform.GetChild(k).gameObject.SetActive(false);
                        break;
                    }
                }
            }
            else
            {
                for (int k = 0; k < 4; k++)
                {
                    if (OpStartMalList.transform.GetChild(k).gameObject.activeSelf && !OpStartMalList.transform.GetChild(k + 4).gameObject.activeSelf)
                    {
                        OpStartMalList.transform.GetChild(k).gameObject.SetActive(false);
                        break;
                    }
                }
            }
            moving_mal.transform.GetChild(1).GetComponent<TMP_Text>().text = "2";
            moving_mal.transform.GetChild(0).gameObject.SetActive(true);
            moving_mal.transform.GetChild(1).gameObject.SetActive(true);
            IsEsp1Used = true;
            IsEspFalseStartUsing = false;
            int[] used_esp = { 5 };
            string[] ment = { "부정 출발!" };
            StartCoroutine(show_esp_used(used_esp, ment, false));
            Esp1Used();
        }
        StartCoroutine(MoveMotion(moving_mal, current_clicked_caan, moving_yut));
    }

    [PunRPC]
    void moving_moved_mal(int clicked_moved_mal_num, int clicked_caan_num, int moving_yut)
    {
        IsMoving = true;
        GameObject moving_mal = null;
        GameObject current_clicked_caan = Caan.transform.GetChild(clicked_caan_num).gameObject;
        if (MyTurn)
        {
            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                if (int.Parse(MalBox.transform.GetChild(k).GetChild(2).name) == clicked_moved_mal_num && MalBox.transform.GetChild(k).name[0] == 'M')
                    moving_mal = MalBox.transform.GetChild(k).gameObject;
            }
            int yut_stack = int.Parse(MyYutStackList.transform.GetChild(moving_yut).GetChild(2).GetComponent<TMP_Text>().text);
            MyYutStackList.transform.GetChild(moving_yut).GetChild(2).GetComponent<TMP_Text>().text = (yut_stack - 1).ToString();
            if (yut_stack - 1 == 0)
            {
                MyYutStackList.transform.GetChild(moving_yut).GetComponent<Image>().color = new Color(100 / 255f, 100 / 255f, 100 / 255f, 128 / 255f);
            }
        }
        else
        {
            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                if (int.Parse(MalBox.transform.GetChild(k).GetChild(2).name) == clicked_moved_mal_num && MalBox.transform.GetChild(k).name[0] == 'O')
                    moving_mal = MalBox.transform.GetChild(k).gameObject;
            }
            int yut_stack = int.Parse(OpYutStackList.transform.GetChild(moving_yut).GetChild(2).GetComponent<TMP_Text>().text);
            OpYutStackList.transform.GetChild(moving_yut).GetChild(2).GetComponent<TMP_Text>().text = (yut_stack - 1).ToString();
            if (yut_stack - 1 == 0)
            {
                OpYutStackList.transform.GetChild(moving_yut).GetComponent<Image>().color = new Color(100 / 255f, 100 / 255f, 100 / 255f, 128 / 255f);
            }
        }
        if (IsEspFalseStartUsing)
        {
            IsEspFalseStartUsing = false;
            if (MyTurn)
            {
                IsEsp1Using = false;
                MyEspList.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            }
        }
        StartCoroutine(MoveMotion(moving_mal, current_clicked_caan, moving_yut));


    }
    GameObject way_over(int mal_pos, int caan_pos)
    {
        if ((1 <= mal_pos && mal_pos <= 4) && 5 < caan_pos)
            return Caan.transform.GetChild(5).gameObject;
        else if ((6 <= mal_pos && mal_pos <= 9) && 10 < caan_pos)
            return Caan.transform.GetChild(10).gameObject;
        else if ((11 <= mal_pos && mal_pos <= 14) && 15 < caan_pos)
            return Caan.transform.GetChild(15).gameObject;
        else if ((20 <= mal_pos && mal_pos <= 24) && 16 <= caan_pos && caan_pos <= 19)
            return Caan.transform.GetChild(15).gameObject;
        return Caan.transform.GetChild(0).gameObject;
    }
    int check_trap(int start_pos, int des_pos, int[] trap_pos, int rolled_yut)
    {
        if (des_pos == -1 || trap_pos[0] == 0)
            return -1;
        if (start_pos == 0 && des_pos == 30)
            return -1;
        if (des_pos == 0)
            des_pos = 30;

        print("from " + start_pos + ", to " + des_pos + ", with " + rolled_yut);
        if (rolled_yut == 5)
        {
            for (int k = 0; k < TrapType.Length; k++)
            {
                if (trap_pos[TrapType[k]] == des_pos)
                {
                    return des_pos;
                }
            }
            return -1;
        }        
        
        if (start_pos == 5 && ((20 <= des_pos && des_pos <= 24) || des_pos == 27))
        {
            int caught_trap_pos = 31;
            for (int k = 0; k < TrapType.Length; k++)
            {
                if (20 <= trap_pos[TrapType[k]] && trap_pos[TrapType[k]] <= des_pos)
                {
                    if (caught_trap_pos > trap_pos[TrapType[k]])
                        caught_trap_pos = trap_pos[TrapType[k]];
                }
            }
            if (caught_trap_pos == 31)
                return -1;
            else
                return caught_trap_pos;
        }
        else if (start_pos == 10 && (25 <= des_pos && des_pos <= 29))
        {
            int caught_trap_pos = 31;
            for (int k = 0; k < TrapType.Length; k++)
            {
                if (25 <= trap_pos[TrapType[k]] && trap_pos[TrapType[k]] <= des_pos)
                {
                    if (caught_trap_pos > trap_pos[TrapType[k]])
                        caught_trap_pos = trap_pos[TrapType[k]];
                }
            }
            if (caught_trap_pos == 31)
                return -1;
            else
                return caught_trap_pos;
        }
        else if (des_pos == 15 && (21 <= start_pos && start_pos <= 24))
        {
            int caught_trap_pos = 31;
            for (int k = 0; k < TrapType.Length; k++)
            {
                if (trap_pos[TrapType[k]] == 27)
                {
                    if (caught_trap_pos < trap_pos[TrapType[k]] - 5)
                    {
                        caught_trap_pos = trap_pos[TrapType[k]] - 5;
                    }
                }
                if( trap_pos[TrapType[k]] == 15)
                {
                    if (caught_trap_pos < trap_pos[TrapType[k]] + 10)
                    {
                        caught_trap_pos = trap_pos[TrapType[k]] +10;
                    }
                }
                if (start_pos < trap_pos[TrapType[k]] && trap_pos[TrapType[k]] <= 24)
                {
                    if (caught_trap_pos > trap_pos[TrapType[k]])
                        caught_trap_pos = trap_pos[TrapType[k]];
                }
            }
            if (caught_trap_pos == 31)
                return -1;
            else if (caught_trap_pos == 22)
                return 27;
            else if (caught_trap_pos == 25)
                return 15;
            else
                return caught_trap_pos;
        }
        else
        {
            int caught_trap_pos = 31;
            for (int k = 0; k < TrapType.Length; k++)
            {
                if ((start_pos < trap_pos[TrapType[k]] && trap_pos[TrapType[k]] <= des_pos) || des_pos == trap_pos[TrapType[k]])
                {
                    if (caught_trap_pos > trap_pos[TrapType[k]])
                        caught_trap_pos = trap_pos[TrapType[k]];
                }
            }
            if (caught_trap_pos == 31)
                return -1;
            else
                return caught_trap_pos;
        }

    }
    int[] where_is_trap()
    {
        int[] trap_pos = new int[TrapType.Length + 1];
        for (int k = 0; k < trap_pos.Length; k++)
            trap_pos[k] = 0;
        for (int t = 0; t < TrapType.Length; t++)
        {
            for (int k = 1; k < Caan.transform.childCount; k++)
            {
                if (Caan.transform.GetChild(k).GetChild(TrapType[t]).gameObject.activeSelf)
                {
                    trap_pos[TrapType[t]] = k;
                    trap_pos[0] = 1;
                }
            }
        }
        for(int k = 0; k<TrapType.Length; k++)
        {
            print("trap " + TrapType[k] + " is at " + trap_pos[TrapType[k]]);
        }
        return trap_pos;
    }
    IEnumerator MoveMotion(GameObject moving_mal, GameObject des_caan, int moving_cnt)
    {
        if (Check_Mal_Movable(MyYutStackList, MyStartMalList))
        {
            for (int t = 0; t < 4; t++)
            {
                MyStartMalList.transform.GetChild(t).GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            for (int t = 0; t < 4; t++)
            {
                MyStartMalList.transform.GetChild(t).GetComponent<Button>().interactable = false;
            }
        }
        int moving_mal_pos = int.Parse(moving_mal.transform.GetChild(2).name);
        int des_caan_pos = int.Parse(des_caan.name);
        int[] trap_pos = where_is_trap();        
        GameObject layover_caan = way_over(moving_mal_pos, des_caan_pos);
        int layover_pos = -1;
        if (layover_caan.name != "0")
            layover_pos = int.Parse(layover_caan.name);

        int caught_trap_pos = check_trap(moving_mal_pos, layover_pos, trap_pos, moving_cnt);        



        Vector3 layover_des;
        if (layover_caan.name == "0" || caught_trap_pos != -1)
        {
            layover_des = new Vector3(0f, 0f);
            if(caught_trap_pos != -1)
                print("caught in trap before layover at " + caught_trap_pos);
        }
        else
        {
            layover_des = layover_caan.transform.position + new Vector3(0f, 0.15f);
            moving_mal.transform.GetChild(2).name = layover_caan.name;
        }
        if (layover_des.magnitude != 0 && moving_cnt != 5)
        {
            while (true)
            {
                moving_mal.transform.position = Vector2.MoveTowards(moving_mal.transform.position, layover_des, MoveSpeed * Time.deltaTime);
                if (Vector2.SqrMagnitude(moving_mal.transform.position - layover_des) < 0.0001f)
                {
                    moving_mal.transform.position = layover_des;
                    break;
                }
                yield return null;
            }
        }
        moving_mal_pos = int.Parse(moving_mal.transform.GetChild(2).name);
        caught_trap_pos = check_trap(moving_mal_pos, des_caan_pos, trap_pos, moving_cnt);

        Vector3 des_pos;
        if (caught_trap_pos == -1)
        {
            des_pos = des_caan.transform.position + new Vector3(0f, 0.15f);
            moving_mal.transform.GetChild(2).name = des_caan.name;
        }
        else
        {
            des_pos = Caan.transform.GetChild(caught_trap_pos).transform.position + new Vector3(0f, 0.15f);
            moving_mal.transform.GetChild(2).name = caught_trap_pos.ToString();
            print("caught in trap after layover at " + caught_trap_pos);
        }
        while (true)
        {
            moving_mal.transform.position = Vector2.MoveTowards(moving_mal.transform.position, des_pos, MoveSpeed * Time.deltaTime);
            if (Vector2.SqrMagnitude(moving_mal.transform.position - des_pos) < 0.0001f)
            {
                des_pos.z = 1f;
                moving_mal.transform.position = des_pos;
                action_moving_result(moving_mal, moving_cnt, caught_trap_pos);
                if (caught_trap_pos == -1)
                {
                    moving_mal.transform.GetChild(2).GetComponent<TMP_Text>().text = (des_caan_pos).ToString();
                    moving_mal.transform.GetChild(2).name = (des_caan_pos).ToString();
                }
                break;
            }
            yield return null;
        }
    }
    void action_moving_result(GameObject moved_mal, int moving_yut, int caught_in_trap) {
        List<GameObject> destroy_list = new List<GameObject>();
        string moved_mal_pos = moved_mal.transform.GetChild(2).name;
        int moved_mal_cnt = int.Parse(moved_mal.transform.GetChild(1).GetComponent<TMP_Text>().text);

        if (caught_in_trap != -1)
        {
            int trap_type = Caan.transform.GetChild(caught_in_trap).GetChild(1).gameObject.activeSelf ? 1 : 2;
            Caan.transform.GetChild(caught_in_trap).GetChild(trap_type).gameObject.SetActive(false);
            int to_do_cnt = moved_mal_cnt;

            if (trap_type == 1)
            {
                print("go to bomb");
                int back_mal_cnt = 0;
                if (moved_mal.name[0] == 'M')
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (!MyStartMalList.transform.GetChild(k).gameObject.activeSelf && !MyStartMalList.transform.GetChild(k + 4).gameObject.activeSelf)
                        {
                            MyStartMalList.transform.GetChild(k).gameObject.SetActive(true);
                            back_mal_cnt++;
                        }
                        if (back_mal_cnt == to_do_cnt)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (!OpStartMalList.transform.GetChild(k).gameObject.activeSelf && !OpStartMalList.transform.GetChild(k + 4).gameObject.activeSelf)
                        {
                            OpStartMalList.transform.GetChild(k).gameObject.SetActive(true);
                            back_mal_cnt++;
                        }
                        if (back_mal_cnt == to_do_cnt)
                        {
                            break;
                        }
                    }
                }
                if (caught_in_trap == 30)
                    Caan.transform.GetChild(0).GetChild(trap_type).gameObject.SetActive(false);
                Destroy(moved_mal);
                turn_on_off_all_moved_mal(true);
                int[] esp_stack = { 0 };
                string[] ment_stack = { "콰쾅!" };
                StartCoroutine(show_esp_used(esp_stack, ment_stack, true));
                
                return;
            }
            else if (trap_type == 2)
            {
                
                moved_mal.transform.position = Caan.transform.GetChild(0).transform.position + new Vector3(0f, 0.15f);
                moved_mal.transform.GetChild(2).name = "0";
                int[] trap_pos = where_is_trap();
                if (trap_pos[1] == 30)
                {
                    Caan.transform.GetChild(30).GetChild(1).gameObject.SetActive(false);
                    Caan.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                    Destroy(moved_mal);
                    turn_on_off_all_moved_mal(true);
                    int[] esp_stack = { 9, 0 };
                    string[] ment_stack = { "집으로!", "콰쾅!" };
                    StartCoroutine(show_esp_used(esp_stack, ment_stack, true));

                    int back_mal_cnt = 0;
                    if (moved_mal.name[0] == 'M')
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            if (!MyStartMalList.transform.GetChild(k).gameObject.activeSelf && !MyStartMalList.transform.GetChild(k + 4).gameObject.activeSelf)
                            {
                                MyStartMalList.transform.GetChild(k).gameObject.SetActive(true);
                                back_mal_cnt++;
                            }
                            if (back_mal_cnt == to_do_cnt)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            if (!OpStartMalList.transform.GetChild(k).gameObject.activeSelf && !OpStartMalList.transform.GetChild(k + 4).gameObject.activeSelf)
                            {
                                OpStartMalList.transform.GetChild(k).gameObject.SetActive(true);
                                back_mal_cnt++;
                            }
                            if (back_mal_cnt == to_do_cnt)
                            {
                                break;
                            }
                        }
                    }
                    
                    return;
                }
                else
                {
                    int[] esp_stack = { 9 };
                    string[] ment_stack = { "집으로!" };
                    StartCoroutine(show_esp_used(esp_stack, ment_stack, false));
                }
                    
            }
        }
        moved_mal_pos = moved_mal.transform.GetChild(2).name;

        if (int.Parse(moved_mal_pos) >= 30)
        {
            int goal_cnt = 0;
            int to_goal = moved_mal_cnt;
            if (moved_mal.name[0] == 'M')
            {
                for (int k = 0; k < 4; k++)
                {
                    if (!MyStartMalList.transform.GetChild(k).gameObject.activeSelf && !MyStartMalList.transform.GetChild(k + 4).gameObject.activeSelf)
                    {
                        MyStartMalList.transform.GetChild(k + 4).gameObject.SetActive(true);
                        goal_cnt++;
                    }
                    if (goal_cnt == to_goal)
                        break;
                }
                goal_cnt = 0;
                for (int k = 4; k < 8; k++)
                {
                    if (MyStartMalList.transform.GetChild(k).gameObject.activeSelf)
                        goal_cnt++;
                    if (goal_cnt == 4)
                    {
                        for (int t = 2; t < MalBox.transform.childCount; t++)
                        {
                            Destroy(MalBox.transform.GetChild(t).gameObject);
                        }
                        GameEnd.transform.GetChild(0).GetComponent<TMP_Text>().text = "승리!!";
                        GameEnd.SetActive(true);
                    }
                }
            }
            else
            {
                for (int k = 0; k < 4; k++)
                {
                    if (!OpStartMalList.transform.GetChild(k).gameObject.activeSelf && !OpStartMalList.transform.GetChild(k + 4).gameObject.activeSelf)
                    {
                        OpStartMalList.transform.GetChild(k + 4).gameObject.SetActive(true);
                        goal_cnt++;
                    }
                    if (goal_cnt == to_goal)
                        break;
                }
                goal_cnt = 0;
                for (int k = 4; k < 8; k++)
                {
                    if (OpStartMalList.transform.GetChild(k).gameObject.activeSelf)
                        goal_cnt++;
                    if (goal_cnt == 4)
                    {
                        for (int t = 2; t < MalBox.transform.childCount; t++)
                        {
                            Destroy(MalBox.transform.GetChild(t).gameObject);
                        }
                        GameEnd.transform.GetChild(0).GetComponent<TMP_Text>().text = "패배 ㅜㅜ";
                        GameEnd.SetActive(true);
                    }
                }
            }
            Destroy(moved_mal);
            turn_on_off_all_moved_mal(true);
            IsMoving = false;
            return;
        }


        for (int k = 2; k < MalBox.transform.childCount; k++)
        {
            string current_mal_pos = MalBox.transform.GetChild(k).GetChild(2).GetComponent<TMP_Text>().text;
            string current_mal_name = MalBox.transform.GetChild(k).name;
            int current_mal_cnt = int.Parse(MalBox.transform.GetChild(k).GetChild(1).GetComponent<TMP_Text>().text);
            if (current_mal_pos == moved_mal_pos && MalBox.transform.GetChild(k).gameObject != moved_mal)
            {
                if (current_mal_name[0] == moved_mal.name[0]) { //같은 사람 말
                    moved_mal.transform.GetChild(1).GetComponent<TMP_Text>().text = (moved_mal_cnt + current_mal_cnt).ToString();
                    destroy_list.Add(MalBox.transform.GetChild(k).gameObject);
                }
                else // 다른 사람 말을 잡음
                {
                    destroy_list.Add(MalBox.transform.GetChild(k).gameObject);
                    int catched_mal_cnt = int.Parse(MalBox.transform.GetChild(k).GetChild(1).GetComponent<TMP_Text>().text);
                    int return_cnt = 0;
                    for (int t = 0; t < 4; t++)
                    {
                        if (MyTurn) // 내가 상대말 잡음
                        {
                            if (!OpStartMalList.transform.GetChild(t).gameObject.activeSelf && !OpStartMalList.transform.GetChild(t + 4).gameObject.activeSelf)
                            {
                                OpStartMalList.transform.GetChild(t).gameObject.SetActive(true);
                                return_cnt++;
                            }
                            if (moving_yut != 3 && moving_yut != 4)//윷이나 모로 잡은거 아니면
                                IsRollable = true; // 한번 더 굴림
                        }
                        else // 상대가 내말 잡음
                        {
                            if (!MyStartMalList.transform.GetChild(t).gameObject.activeSelf && !MyStartMalList.transform.GetChild(t + 4).gameObject.activeSelf)
                            {
                                MyStartMalList.transform.GetChild(t).gameObject.SetActive(true);
                                return_cnt++;
                            }
                        }
                        if (return_cnt == catched_mal_cnt)
                            break;
                    }
                }
            }
        }
        for (int k = 0; k < destroy_list.Count; k++)
        {
            Destroy(destroy_list[k]);
        }
        if (int.Parse(moved_mal.transform.GetChild(1).GetComponent<TMP_Text>().text) > 1)
        {
            moved_mal.transform.GetChild(0).gameObject.SetActive(true);
            moved_mal.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            moved_mal.transform.GetChild(0).gameObject.SetActive(false);
            moved_mal.transform.GetChild(1).gameObject.SetActive(false);
        }        
        turn_on_off_all_moved_mal(true);
        IsMoving = false;
    }

    

    bool Check_Mal_Movable(GameObject YutStackList, GameObject StartMalList)
    {
        if (IsEspKeepUsing)
            return false;
        for (int k = 0; k < 5; k++)
        {
            if (int.Parse(YutStackList.transform.GetChild(k).GetChild(2).GetComponent<TMP_Text>().text) > 0)
                return true;
        }
        if (int.Parse(YutStackList.transform.GetChild(5).GetChild(2).GetComponent<TMP_Text>().text) > 0)
        {
            if (Check_Mal_Moved(StartMalList))
            {
                return true;
            }
        }
        return false;
    }
    bool Check_Mal_Moved(GameObject StartMalList)
    {
        for (int k = 0; k < 4; k++)
        {
            if (!StartMalList.transform.GetChild(k).gameObject.activeSelf && !StartMalList.transform.GetChild(k + 4).gameObject.activeSelf)
                return true;
        }
        return false;
    }

    #endregion

    #region ESP
    public void Esp2ButtonClick()
    {
        IsEsp2Using = !IsEsp2Using;
        MyEspList.transform.GetChild(2).GetChild(0).gameObject.SetActive(IsEsp2Using);
    }

    public void Esp1ButtonClick()
    {
        IsEsp1Using = !IsEsp1Using;
        MyEspList.transform.GetChild(1).GetChild(0).gameObject.SetActive(IsEsp1Using);
        switch (MyEsp1)
        {
            case 0: // 콰쾅!
                on_off_caan_trap(1, IsEsp1Using, -1);
                break;
            case 1: // 안돼 돌아가                
                click_go_back();
                MyEspList.transform.GetChild(1).GetChild(0).gameObject.SetActive(IsEsp1Using);
                break;
            case 2: // 서양 문물
                break;
            case 3: // 무인도
                PV.RPC("use_island", RpcTarget.All, IsEsp1Using);
                break;
            case 4: // 킵이요
                IsEspKeepUsing = IsEsp1Using;
                break;
            case 5: // 부정출발
                click_false_start();
                MyEspList.transform.GetChild(1).GetChild(0).gameObject.SetActive(IsEsp1Using);
                break;
            case 6: // 위치변환
                click_change_pos();
                MyEspList.transform.GetChild(1).GetChild(0).gameObject.SetActive(IsEsp1Using);
                break;
            case 7: // 메타몽
                PV.RPC("use_metamong", RpcTarget.All, IsEsp1Using);
                break;
            case 8: // 문워크
                break;
            case 9: // 집으로
                on_off_caan_trap(2, IsEsp1Using, -1);
                break;
            case 10: // 따라큐
                break;
            case 11: // 밀당
                click_magnet();
                MyEspList.transform.GetChild(1).GetChild(0).gameObject.SetActive(IsEsp1Using);
                break;
        }
    }


    public void TrapButtonClick()
    {
        GameObject current_clicked_button = EventSystem.current.currentSelectedGameObject;
        IsEsp1Used = true;
        int trap_type = current_clicked_button.name[0] == 'b' ? 1 : 2;
        int trap_pos = int.Parse(current_clicked_button.transform.parent.name);
        current_clicked_button.GetComponent<Button>().interactable = false;
        PV.RPC("Esp1Used", RpcTarget.All);
        PV.RPC("PlantingTrap", RpcTarget.All, trap_type, trap_pos);        
    }
    [PunRPC]
    void PlantingTrap(int trap_type, int trap_pos)
    {
        GameObject current_planting_trap = Caan.transform.GetChild(trap_pos).GetChild(trap_type).gameObject;
        if (trap_pos == 30)
            Caan.transform.GetChild(0).GetChild(trap_type).gameObject.SetActive(true);
        current_planting_trap.GetComponent<Button>().interactable = false;
        on_off_caan_trap(trap_type, false, trap_pos);
    }

    IEnumerator show_esp_used(int[] esp_stack, string[] ment, bool end)
    {

        for (int k = 0; k < esp_stack.Length; k++)
        {
            PopEspUsing.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ESP_sprite[esp_stack[k]];
            PopEspUsing.transform.GetChild(0).localPosition = new Vector3(0f, 0.55f);
            if (esp_stack[k] == 0)
            {
                PopEspUsing.transform.GetChild(0).localPosition += new Vector3(0.3f, 0f);
            }
            PopEspUsing.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            PopEspUsing.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(253 / 255f, 246 / 255f, 187 / 255f, 1f);
            PopEspUsing.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = ment[k];
            PopEspUsing.SetActive(true);
            float turn_show_time = 0;
            while (true)
            {
                turn_show_time += Time.deltaTime;
                if (turn_show_time >= ShowPopTime)
                {
                    break;
                }
                yield return null;
            }

            while (true)
            {
                if (PopEspUsing.transform.GetChild(0).GetComponent<SpriteRenderer>().color.a <= 0)
                {
                    PopEspUsing.SetActive(false);
                    break;
                }
                PopEspUsing.transform.GetChild(0).GetComponent<SpriteRenderer>().color =
                    new Color(1f, 1f, 1f,
                    PopEspUsing.transform.GetChild(0).GetComponent<SpriteRenderer>().color.a - VenishPopSpeed * Time.deltaTime);
                PopEspUsing.transform.GetChild(1).GetComponent<SpriteRenderer>().color =
                    new Color(253 / 255f, 246 / 255f, 187 / 255f,
                    PopEspUsing.transform.GetChild(0).GetComponent<SpriteRenderer>().color.a - VenishPopSpeed * Time.deltaTime);
                yield return null;
            }
        }
        if (end)
            IsMoving = false;
    }


    [PunRPC]
    void use_island(bool usingEsp1)
    {
        IsEspIslandUsing = usingEsp1;
    }

    void click_go_back()
    {
        int op_mal_cnt = 0;
        for (int k = 2; k < MalBox.transform.childCount; k++)
        {
            if (MalBox.transform.GetChild(k).name[0] == 'O')
                op_mal_cnt++;
        }
        if (op_mal_cnt >= 1)
        {
            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                if (MalBox.transform.GetChild(k).name[0] == 'O')
                {
                    MalBox.transform.GetChild(k).GetChild(3).gameObject.SetActive(IsEsp1Using);
                }
            }
        }
        else
            IsEsp1Using = false;
        IsEspGoBackUsing = IsEsp1Using;        
    }
    
    void click_change_pos()
    {
        if (IsEsp1Using)
        {
            int op_mal_cnt = 0;
            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                if (MalBox.transform.GetChild(k).name[0] == 'O')
                {
                    op_mal_cnt++;
                    break;
                }
            }
            int my_mal_cnt = 0;
            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                if (MalBox.transform.GetChild(k).name[0] == 'M')
                {
                    my_mal_cnt++;
                    break;
                }
            }
            if (op_mal_cnt != 0 && my_mal_cnt != 0)
            {
                for (int k = 2; k < MalBox.transform.childCount; k++)
                {
                    if (MalBox.transform.GetChild(k).name[0] == 'M')
                    {
                        MalBox.transform.GetChild(k).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 150 / 255f, 150 / 255f, 1f);
                        MalBox.transform.GetChild(k).GetChild(3).GetChild(1).GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 150 / 255f, 150 / 255f, 1f);
                        MalBox.transform.GetChild(k).GetChild(3).GetChild(2).GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 150 / 255f, 150 / 255f, 1f);                        
                        MalBox.transform.GetChild(k).GetChild(3).gameObject.SetActive(true);
                    }
                }
                IsEspChangePosUsing = true;
            }
            else
            {
                IsEsp1Using = false;
                IsEspChangePosUsing = false;
            }
        }
        else
        {
            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                MalBox.transform.GetChild(k).GetChild(3).gameObject.SetActive(false);
                MalBox.transform.GetChild(k).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
                MalBox.transform.GetChild(k).GetChild(3).GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
                MalBox.transform.GetChild(k).GetChild(3).GetChild(2).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
            }
            EspChangeIndex1 = -1;
            EspChangeIndex2 = -1;
            MyEspList.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            IsEspChangePosUsing = false;
        }
    }

    

    void click_magnet()
    {
        if (IsEsp1Using)
        {
            EspChangeIndex1 = -1;
            EspChangeIndex2 = -1;
            int op_mal_cnt = 0;
            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                if (MalBox.transform.GetChild(k).name[0] == 'O')
                {
                    op_mal_cnt++;
                    break;
                }
            }
            int my_mal_cnt = 0;
            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                if (MalBox.transform.GetChild(k).name[0] == 'M')
                {
                    my_mal_cnt++;
                    break;
                }
            }
            if (op_mal_cnt != 0 && my_mal_cnt != 0)
            {
                for (int k = 2; k < MalBox.transform.childCount; k++)
                {
                    MalBox.transform.GetChild(k).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 150 / 255f, 150 / 255f, 1f);
                    MalBox.transform.GetChild(k).GetChild(3).GetChild(1).GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 150 / 255f, 150 / 255f, 1f);
                    MalBox.transform.GetChild(k).GetChild(3).GetChild(2).GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 150 / 255f, 150 / 255f, 1f);
                    MalBox.transform.GetChild(k).GetChild(3).gameObject.SetActive(true);
                }
                IsEspMagnetUsing = true;
            }
            else
            {
                turn_on_off_all_caan(false);
                IsEsp1Using = false;
                IsEspMagnetUsing = false;
            }
        }
        else
        {
            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                MalBox.transform.GetChild(k).GetChild(3).gameObject.SetActive(false);
                MalBox.transform.GetChild(k).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
                MalBox.transform.GetChild(k).GetChild(3).GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
                MalBox.transform.GetChild(k).GetChild(3).GetChild(2).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
            }
            turn_on_off_all_caan(false);
            EspChangeIndex1 = -1;
            EspChangeIndex2 = -1;
            MyEspList.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            IsEspMagnetUsing = false;
        }
    }


    void OpMalClick(GameObject clicked_mal)
    {
        if (IsEspGoBackUsing)
        {
            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                if (MalBox.transform.GetChild(k).gameObject == clicked_mal)
                {
                    PV.RPC("use_go_back", RpcTarget.All, k);
                    break;
                }
            }
            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                if (MalBox.transform.GetChild(k).name[0] == 'O')
                {
                    MalBox.transform.GetChild(k).GetChild(3).gameObject.SetActive(false);
                }
            }
            IsEsp1Used = true;
            IsEsp1Using = false;
            IsEspGoBackUsing = false;
        }
        if (IsEspChangePosUsing && EspChangeIndex1 != -1)
        {
            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                if (MalBox.transform.GetChild(k).gameObject == clicked_mal)
                {
                    EspChangeIndex2 = k;
                    break;
                }
            }
            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                if (k == EspChangeIndex2 || k == EspChangeIndex1)
                {
                    MalBox.transform.GetChild(k).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
                    MalBox.transform.GetChild(k).GetChild(3).GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
                    MalBox.transform.GetChild(k).GetChild(3).GetChild(2).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
                }
                else
                {
                    MalBox.transform.GetChild(k).GetChild(3).gameObject.SetActive(false);
                }
            }
            if (EspChangeIndex1 != -1 && EspChangeIndex2 != -1)
            {
                PV.RPC("use_change_pos", RpcTarget.All, EspChangeIndex1, EspChangeIndex2);
                IsEsp1Used = true;
                IsEsp1Using = false;
                IsEspChangePosUsing = false;
            }
        }
        else if (IsEspMagnetUsing)
        {
            int clicked_mal_index = -1;
            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                if (MalBox.transform.GetChild(k).gameObject == clicked_mal)
                {
                    clicked_mal_index = k;
                    break;
                }
            }
            if (EspChangeIndex2 == clicked_mal_index && EspChangeIndex2 != -1)
            {
                turn_on_off_all_caan(false);
                EspChangeIndex2 = -1;
                if (EspChangeIndex1 == -1)
                {
                    IsEsp1Using = false;
                    click_magnet();
                    IsEsp1Using = true;
                    click_magnet();
                }
                else
                {
                    for (int k = 2; k < MalBox.transform.childCount; k++)
                    {
                        if (MalBox.transform.GetChild(k).name[0] == 'O')
                        {
                            MalBox.transform.GetChild(k).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 150 / 255f, 150 / 255f, 1f);
                            MalBox.transform.GetChild(k).GetChild(3).GetChild(1).GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 150 / 255f, 150 / 255f, 1f);
                            MalBox.transform.GetChild(k).GetChild(3).GetChild(2).GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 150 / 255f, 150 / 255f, 1f);
                            MalBox.transform.GetChild(k).GetChild(3).gameObject.SetActive(true);
                        }
                    }
                }
                
            }
            else if (EspChangeIndex2 == -1)
            {
                EspChangeIndex2 = clicked_mal_index;
                for (int k = 2; k < MalBox.transform.childCount; k++)
                {
                    if (k == EspChangeIndex2)
                    {
                        MalBox.transform.GetChild(k).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
                        MalBox.transform.GetChild(k).GetChild(3).GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
                        MalBox.transform.GetChild(k).GetChild(3).GetChild(2).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
                    }
                    else
                    {
                        if (MalBox.transform.GetChild(k).name[0] == 'O')
                            MalBox.transform.GetChild(k).GetChild(3).gameObject.SetActive(false);
                    }
                    if (EspChangeIndex1 != -1)
                    {
                        activate_magnet_caan(EspChangeIndex1 ,EspChangeIndex2);
                    }
                }
            }
        }
    }
    void MyMalClick(GameObject clicked_mal)
    {
        int clicked_mal_index = -1;
        for (int k = 2; k < MalBox.transform.childCount; k++)
        {
            if (MalBox.transform.GetChild(k).gameObject == clicked_mal)
            {
                clicked_mal_index = k;
                break;
            }
        }
        if (EspChangeIndex1 == clicked_mal_index && EspChangeIndex1 != -1)
        {
            
            if (IsEspMagnetUsing)
            {
                turn_on_off_all_caan(false);
                EspChangeIndex1 = -1;
                if (EspChangeIndex2 == -1)
                {
                    IsEsp1Using = false;
                    click_magnet();
                    IsEsp1Using = true;
                    click_magnet();
                }
                else
                {                    
                    for(int k = 2; k< MalBox.transform.childCount; k++)
                    {
                        if(MalBox.transform.GetChild(k).name[0] == 'M')
                        {
                            MalBox.transform.GetChild(k).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 150 / 255f, 150 / 255f, 1f);
                            MalBox.transform.GetChild(k).GetChild(3).GetChild(1).GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 150 / 255f, 150 / 255f, 1f);
                            MalBox.transform.GetChild(k).GetChild(3).GetChild(2).GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 150 / 255f, 150 / 255f, 1f);
                            MalBox.transform.GetChild(k).GetChild(3).gameObject.SetActive(true);
                        }
                    }
                }
                return;
            }
            else if (IsEspChangePosUsing)
            {
                IsEsp1Using = false;
                click_change_pos();
                IsEsp1Using = true;
                click_change_pos();
            }   
            return;
        }
        else if (IsEspChangePosUsing && EspChangeIndex1 == -1)
        {
            EspChangeIndex1 = clicked_mal_index;
            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                if (k == EspChangeIndex1)
                {
                    MalBox.transform.GetChild(k).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
                    MalBox.transform.GetChild(k).GetChild(3).GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
                    MalBox.transform.GetChild(k).GetChild(3).GetChild(2).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
                }
                else
                {
                    MalBox.transform.GetChild(k).GetChild(3).gameObject.SetActive(false);
                }
            }

            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                if (MalBox.transform.GetChild(k).name[0] == 'O')
                {
                    MalBox.transform.GetChild(k).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 150 / 255f, 150 / 255f, 1f);
                    MalBox.transform.GetChild(k).GetChild(3).GetChild(1).GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 150 / 255f, 150 / 255f, 1f);
                    MalBox.transform.GetChild(k).GetChild(3).GetChild(2).GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 150 / 255f, 150 / 255f, 1f);
                    MalBox.transform.GetChild(k).GetChild(3).gameObject.SetActive(true);
                }
            }
        }

        else if (IsEspMagnetUsing && EspChangeIndex1 == -1)
        {
            EspChangeIndex1 = clicked_mal_index;
            for (int k = 2; k < MalBox.transform.childCount; k++)
            {
                if (k == EspChangeIndex1)
                {
                    MalBox.transform.GetChild(k).GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
                    MalBox.transform.GetChild(k).GetChild(3).GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
                    MalBox.transform.GetChild(k).GetChild(3).GetChild(2).GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 117 / 255f, 138 / 255f, 1f);
                }
                else
                {
                    if(MalBox.transform.GetChild(k).name[0] == 'M')
                        MalBox.transform.GetChild(k).GetChild(3).gameObject.SetActive(false);
                }
                if(EspChangeIndex2 != -1)
                {
                    activate_magnet_caan(EspChangeIndex2, EspChangeIndex1);
                }
            }
            
        }

    }


    [PunRPC]
    void use_change_pos(int change_index1, int change_index2)
    {
        int[] esp = { 6 };
        string[] ment = { "초동역학 위치전환기!" };
        StartCoroutine(show_esp_used(esp, ment, false));
        GameObject moving_mal1 = MalBox.transform.GetChild(change_index1).gameObject;
        GameObject moving_mal2 = MalBox.transform.GetChild(change_index2).gameObject;

        string moving_mal1_pos = moving_mal1.transform.GetChild(2).GetComponent<TMP_Text>().text;
        string moving_mal2_pos = moving_mal2.transform.GetChild(2).GetComponent<TMP_Text>().text;
        moving_mal1.transform.GetChild(2).GetComponent<TMP_Text>().text = moving_mal2_pos;
        moving_mal1.transform.GetChild(2).name = moving_mal2_pos;
        moving_mal2.transform.GetChild(2).GetComponent<TMP_Text>().text = moving_mal1_pos;
        moving_mal2.transform.GetChild(2).name = moving_mal1_pos;
        StartCoroutine(esp_change_pos(moving_mal1, moving_mal2));
        EspChangeIndex1 = -1;
        EspChangeIndex2 = -1;
    }
    IEnumerator esp_change_pos(GameObject moving_mal1, GameObject moving_mal2)
    {
        Vector3 des_pos1 = moving_mal1.transform.position;
        Vector3 des_pos2 = moving_mal2.transform.position;
        float waiting = 0f;
        while (true)
        {
            waiting += Time.deltaTime;
            if (waiting > 0.5f)
                break;
            yield return null;
        }
        while (true)
        {
            moving_mal1.transform.position = Vector2.MoveTowards(moving_mal1.transform.position, des_pos2, MoveSpeed * Time.deltaTime);
            moving_mal2.transform.position = Vector2.MoveTowards(moving_mal2.transform.position, des_pos1, MoveSpeed * Time.deltaTime);
            if ((Vector2.SqrMagnitude(moving_mal1.transform.position - des_pos2) < 0.0001)
                && (Vector2.SqrMagnitude(moving_mal2.transform.position - des_pos1) < 0.0001f))
            {
                des_pos1.z = 1f;
                des_pos2.z = 1f;
                moving_mal1.transform.position = des_pos2;
                moving_mal2.transform.position = des_pos1;
                break;
            }
            yield return null;
        }
        if (MyTurn)
        {
            IsEsp1Using = false;
            click_change_pos();
        }
    }
    [PunRPC]
    void use_go_back(int mal_num)
    {
        GameObject clicked_mal = MalBox.transform.GetChild(mal_num).gameObject;
        print(clicked_mal.name);
        int mal_cnt = int.Parse(clicked_mal.transform.GetChild(1).GetComponent<TMP_Text>().text);
        if (MyTurn)
        {
            for (int k = 0; k < 4; k++)
            {
                if (!OpStartMalList.transform.GetChild(k).gameObject.activeSelf && !OpStartMalList.transform.GetChild(k + 4).gameObject.activeSelf)
                {
                    OpStartMalList.transform.GetChild(k).gameObject.SetActive(true);
                    break;
                }
            }
        }
        else
        {
            for (int k = 0; k < 4; k++)
            {
                if (!MyStartMalList.transform.GetChild(k).gameObject.activeSelf && !MyStartMalList.transform.GetChild(k + 4).gameObject.activeSelf)
                {
                    MyStartMalList.transform.GetChild(k).gameObject.SetActive(true);
                    break;
                }
            }
        }
        if (mal_cnt > 1)
        {
            clicked_mal.transform.GetChild(1).GetComponent<TMP_Text>().text = (mal_cnt - 1).ToString();
            if (mal_cnt - 1 == 1)
            {
                clicked_mal.transform.GetChild(0).gameObject.SetActive(false);
                clicked_mal.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else
        {
            Destroy(clicked_mal);
        }
        Esp1Used();
        int[] esp_stack = { 1 };
        string[] ment = { "안 돼. 돌아가." };
        StartCoroutine(show_esp_used(esp_stack, ment, false));
    }
    
    [PunRPC]
    void use_magnet(int moving_mal_index, int des_caan_index)
    {
        Esp1Used();
        int[] esp_stack = { 11 };
        string[] ment = { "밀고 당기기!" };
        show_esp_used(esp_stack, ment, false);        
        GameObject moving_mal = MalBox.transform.GetChild(moving_mal_index).gameObject;
        GameObject des_caan = Caan.transform.GetChild(des_caan_index).gameObject;
        StartCoroutine(esp_magnet(moving_mal, des_caan));
        EspChangeIndex1 = -1;
        EspChangeIndex2 = -1;
        IsEsp1Used = true;
        IsEsp1Using = false;
        IsEspMagnetUsing = false;
    }
    IEnumerator esp_magnet(GameObject moving_mal, GameObject des_caan)
    {
        Vector3 des_pos = des_caan.transform.position + new Vector3(0f, 0.15f);
        float waiting = 0f;
        while (true)
        {
            waiting += Time.deltaTime;
            if (waiting > 0.5f)
                break;
            yield return null;
        }
        while (true)
        {
            moving_mal.transform.position = Vector2.MoveTowards(moving_mal.transform.position, des_pos, MoveSpeed * Time.deltaTime);
            if ((Vector2.SqrMagnitude(moving_mal.transform.position - des_pos) < 0.0001))
                
            {
                des_pos.z = 1f;
                moving_mal.transform.position = des_pos;
                break;
            }
            yield return null;
        }
        string des_caan_num = des_caan.name;
        for(int k = 2; k < MalBox.transform.childCount; k++)
        {
            if (MalBox.transform.GetChild(k).GetChild(2).name == des_caan_num && MalBox.transform.GetChild(k).name[0] == moving_mal.name[0])
            {
                int cnt1 = int.Parse(moving_mal.transform.GetChild(1).GetComponent<TMP_Text>().text);
                int cnt2 = int.Parse(MalBox.transform.GetChild(k).GetChild(1).GetComponent<TMP_Text>().text);
                moving_mal.transform.GetChild(1).GetComponent<TMP_Text>().text = (cnt1 + cnt2).ToString();
                moving_mal.transform.GetChild(0).gameObject.SetActive(true);
                moving_mal.transform.GetChild(1).gameObject.SetActive(true);
                Destroy(MalBox.transform.GetChild(k).gameObject);
            }
        }
        if (MyTurn)
        {
            IsEsp1Using = false;
            click_magnet();
        }
    }

    void click_false_start()
    {
        int remained = 0;
        for(int k = 0; k< 4; k++)
        {
            if(MyStartMalList.transform.GetChild(k).gameObject.activeSelf && !MyStartMalList.transform.GetChild(k + 4).gameObject.activeSelf)
            {
                remained++;
                if (remained == 2)
                    break;
            }
        }
        if(remained != 2)
        {
            IsEsp1Using = false;
        }
        else
        {

        }
        PV.RPC("use_false_start", RpcTarget.All, IsEsp1Using);
    }

    [PunRPC]
    void use_false_start(bool esp_using)
    {
        IsEspFalseStartUsing = esp_using;
    }

    [PunRPC]
    void use_metamong(bool esp_using)
    {
        IsEspMetamongUsing = esp_using;
    }


    [PunRPC]
    void Esp1Used()
    {
        if (MyTurn)
        {
            MyEspList.transform.GetChild(1).GetComponent<Button>().interactable = false;
            MyEspList.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            MyEspList.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);            
        }
        else
        {
            OpEspList.transform.GetChild(1).GetComponent<Image>().sprite = ESP_sprite[OpEsp1];
            OpEspList.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        }
    }

    void activate_magnet_caan(int moving_mal_idx, int des_mal_idx)
    {
        turn_on_off_all_caan(false);
        int moving_pos = int.Parse(MalBox.transform.GetChild(moving_mal_idx).GetChild(2).name);
        int des_pos = int.Parse(MalBox.transform.GetChild(des_mal_idx).GetChild(2).name);        
        int front = des_pos + 1;
        int back = des_pos - 1;
        int side1 = -1;
        int side2 = -1;
        if (des_pos == 5)
            side1 = 20;
        else if (des_pos == 10)
            side1 = 25;
        else if (des_pos == 15)
            side1 = 24;
        else if (des_pos == 0 || des_pos == 30)
        {
            front = 1;
            back = 19;
            side1 = 29;
        }
        else if (des_pos == 20)
            back = 5;
        else if (des_pos == 24)
            front = 15;        
        else if (des_pos == 25)        
            back = 10;        
        else if (des_pos == 27)
        {            
            side1 = 21;
            side2 = 23;
        }
        print("from " + moving_pos + " to " + des_pos + " : " + front + " " + back + " " + side1 + " " + side2);
        for (int k = 2; k < MalBox.transform.childCount; k++)
        {
            int current_mal_pos = int.Parse(MalBox.transform.GetChild(k).GetChild(2).name);
            if (current_mal_pos == front && (MalBox.transform.GetChild(k).name[0] == 'O' || k == moving_mal_idx)) 
            {
                print("front " + k + " " + current_mal_pos + " " + MalBox.transform.GetChild(k).name);
                front = -1;
            }
            else if (current_mal_pos == back && (MalBox.transform.GetChild(k).name[0] == 'O' || k == moving_mal_idx))
            {
                print("back " + k + " " + current_mal_pos + " " + MalBox.transform.GetChild(k).name);
                back = -1;
            }
            else if (current_mal_pos == side1 && (MalBox.transform.GetChild(k).name[0] == 'O' || k == moving_mal_idx))
            {
                print("sdie1 " + k + " " + current_mal_pos + " " + MalBox.transform.GetChild(k).name);
                side1 = -1;
            }
            else if (current_mal_pos == side2 && (MalBox.transform.GetChild(k).name[0] == 'O' || k == moving_mal_idx))
            {
                print("sdie2 " + k + " " + current_mal_pos + " " + MalBox.transform.GetChild(k).name);
                side2 = -1;
            }
        }
        print("from " + moving_pos + " to " + des_pos + " : " + front + " " + back + " " + side1 + " " + side2);
        if (front != -1)
        {
            Caan.transform.GetChild(front).GetComponent<Button>().interactable = true;
            Caan.transform.GetChild(front).GetChild(0).gameObject.SetActive(true);
            Caan.transform.GetChild(front).GetChild(0).GetComponent<Image>().color = new Color(255 / 255f, 0 / 255f, 255 / 255f, 1f);
            Caan.transform.GetChild(front).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "밀당!";
        }
        if (back != -1)
        {
            Caan.transform.GetChild(back).GetComponent<Button>().interactable = true;
            Caan.transform.GetChild(back).GetChild(0).gameObject.SetActive(true);
            Caan.transform.GetChild(back).GetChild(0).GetComponent<Image>().color = new Color(255 / 255f, 0 / 255f, 255 / 255f, 1f);
            Caan.transform.GetChild(back).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "밀당!";
        }
        if (side1 != -1)
        {
            Caan.transform.GetChild(side1).GetComponent<Button>().interactable = true;
            Caan.transform.GetChild(side1).GetChild(0).gameObject.SetActive(true);
            Caan.transform.GetChild(side1).GetChild(0).GetComponent<Image>().color = new Color(255 / 255f, 0 / 255f, 255 / 255f, 1f);
            Caan.transform.GetChild(side1).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "밀당!";
        }
        if (side2 != -1)
        {
            Caan.transform.GetChild(side2).GetComponent<Button>().interactable = true;
            Caan.transform.GetChild(side2).GetChild(0).gameObject.SetActive(true);
            Caan.transform.GetChild(side2).GetChild(0).GetComponent<Image>().color = new Color(255 / 255f, 0 / 255f, 255 / 255f, 1f);
            Caan.transform.GetChild(side2).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "밀당!";
        }
        if(front == -1 && back == -1 && side1 == -1 && side2 == -1)
        {
            EspMagnetMovingMalIndex = -1;
            IsEspMagnetUsing = false;
            IsEsp1Using = false;
            click_magnet();
        }
        else
        {
            EspMagnetMovingMalIndex = moving_mal_idx;
        }
    }
    void on_off_caan_trap(int trap_type, bool on, int exept_num) // trap type 1 == bomb, 2==home
    {
        for (int k = 1; k < Caan.transform.childCount; k++)
        {
            if (k == 22)
                continue;
            if (on && (Caan.transform.GetChild(k).GetChild(1).gameObject.activeSelf || Caan.transform.GetChild(k).GetChild(2).gameObject.activeSelf))
                continue;
            if (on && k == 30 && trap_type == 2)
                continue;
            bool haved = false;
            for (int t = 2; t < MalBox.transform.childCount; t++)
            {
                if (int.Parse(MalBox.transform.GetChild(t).GetChild(2).name) == k)
                {
                    haved = true;
                    break;
                }
                if (int.Parse(MalBox.transform.GetChild(t).GetChild(2).name) == 0 && k == 30)
                {
                    haved = true;
                    break;
                }
            }
            if (haved)
                continue;
            Caan.transform.GetChild(k).GetChild(trap_type).gameObject.SetActive(on);
            if (k == exept_num)
                Caan.transform.GetChild(k).GetChild(trap_type).gameObject.SetActive(!on);
        }
    }


    #endregion

    #region Setting
    public void EndGameButtonClick()
    {
        end_game();        
    }

    public void StartButtonClick()
    {        
        if (GameCanvas.activeSelf)
        {
            PV.RPC("start_game", RpcTarget.All);
        }
    }
    [PunRPC]
    void start_game()
    {
        
        print("Prepare Game");
        MyTurnSign = MyInfoList.transform.GetChild(0).gameObject;
        MyName = MyInfoList.transform.GetChild(2).GetComponent<TMP_Text>();
        MyStartMalList = MyInfoList.transform.GetChild(3).gameObject;
        MyEspList = MyInfoList.transform.GetChild(4).gameObject;
        MyYutStackList = MyInfoList.transform.GetChild(5).gameObject;
        MyChatBubbleBox = MyInfoList.transform.GetChild(6).gameObject;
        MyEspTooltipBox = MyInfoList.transform.GetChild(7).gameObject;
        MyChatBubble = MyChatBubbleBox.transform.GetChild(0).gameObject;
        MyMovingMal = MalBox.transform.GetChild(0).gameObject;


        OpTurnSign = OpInfoList.transform.GetChild(0).gameObject;
        OpName = OpInfoList.transform.GetChild(2).GetComponent<TMP_Text>();
        OpStartMalList = OpInfoList.transform.GetChild(3).gameObject;
        OpEspList = OpInfoList.transform.GetChild(4).gameObject;
        OpYutStackList = OpInfoList.transform.GetChild(5).gameObject;
        OpChatBubbleBox = OpInfoList.transform.GetChild(6).gameObject;
        OpEspTooltipBox = OpInfoList.transform.GetChild(7).gameObject;
        OpChatBubble = OpChatBubbleBox.transform.GetChild(0).gameObject;
        OpMovingMal = MalBox.transform.GetChild(1).gameObject;


        ESPList = PopEsp.transform.GetChild(2).gameObject;

        MyName.text = PhotonNetwork.LocalPlayer.NickName;
        OpName.text = PhotonNetwork.PlayerListOthers[0].NickName;
        Clear();
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            int turn = Random.Range(0, 2);
            int master_mal = Random.Range(0, 7);
            int slave__mal = Random.Range(0, 7);
            while (master_mal == slave__mal)
                master_mal = Random.Range(0, 7);
            
            int master_esp1 = Random.Range(0, 9);
            int slave_esp1 = Random.Range(0, 9);
            while (master_esp1 == slave_esp1)
                master_esp1 = Random.Range(0, 9);

            int master_esp2 = Random.Range(0, 6);
            int slave_esp2 = Random.Range(0, 6);
            while (master_esp2 == slave_esp2)
                master_esp2 = Random.Range(0, 6);

            if (master_esp1 == 2)
                master_esp1 = 9;
            else if (master_esp1 == 8)
                master_esp1 = 11;
            if (slave_esp1 == 2)
                slave_esp1 = 9;
            else if (slave_esp1 == 8)
                slave_esp1 = 11;
            PV.RPC("set_turn_and_character_and_esp", RpcTarget.All, turn, master_mal, slave__mal, master_esp1, slave_esp1, master_esp2, slave_esp2);            
        }
    }



    [PunRPC]
    void set_turn_and_character_and_esp(int turn, int master_mal, int slave_mal, int master_esp1, int slave_esp1, int master_esp2, int slave_esp2)
    {
        print("setting");
        if (turn == 0)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                MyTurn = true;
                IsRollable = true;
            }
            else
            {
                MyTurn = false;
                IsRollable = false;
            }
        }
        else
        {
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                MyTurn = true;
                IsRollable = true;
            }
            else
            {
                MyTurn = false;
                IsRollable = false;
            }
        }

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            MyMalImage = Ganzi[master_mal];
            OpMalImage = Ganzi[slave_mal];
            MyEsp1 = master_esp1;
            MyEsp2 = master_esp2;
            OpEsp1 = slave_esp1;
            OpEsp2 = slave_esp2;
        }
        else
        {
            MyMalImage = Ganzi[slave_mal];
            OpMalImage = Ganzi[master_mal];
            MyEsp1 = slave_esp1;
            MyEsp2 = slave_esp2;
            OpEsp1 = master_esp1;
            OpEsp2 = master_esp2;
        }        
        StartCoroutine(Show_ESP_Choose(MyEsp1, MyEsp2));
        for (int k = 0; k < 4; k++)
        {
            MyStartMalList.transform.GetChild(k).GetComponent<Image>().sprite = MyMalImage;
            MyStartMalList.transform.GetChild(k).GetComponent<Button>().interactable = false;            
            OpStartMalList.transform.GetChild(k).GetComponent<Image>().sprite = OpMalImage;            
        }
        for (int k = 4; k < 8; k++)
        {
            MyStartMalList.transform.GetChild(k).GetComponent<Image>().sprite = MyMalImage;            
            OpStartMalList.transform.GetChild(k).GetComponent<Image>().sprite = OpMalImage;
        }
        MyMovingMal.GetComponent<SpriteRenderer>().sprite = MyMalImage;
        OpMovingMal.GetComponent<SpriteRenderer>().sprite = OpMalImage;
        PopTurn.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = MyMalImage;
        MyEspTooltipBox.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = ESPList.transform.GetChild(MyEsp1).GetChild(2).GetComponent<TMP_Text>().text;
        MyEspTooltipBox.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = EspTooltip[MyEsp1];
        MyEspTooltipBox.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 42 + (EspTooltip[MyEsp1].Length + 18) / 18 * 23);
        MyEspTooltipBox.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = ESPList.transform.GetChild(ESPList.transform.childCount - 6 + MyEsp2).GetChild(2).GetComponent<TMP_Text>().text +" ";
        MyEspTooltipBox.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text += YutHanguel[MyEsp2];
        MyEspTooltipBox.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = EspTooltip[ESPList.transform.childCount - 6 + MyEsp2];

        OpEspTooltipBox.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = ESPList.transform.GetChild(OpEsp1).GetChild(2).GetComponent<TMP_Text>().text;
        OpEspTooltipBox.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = EspTooltip[OpEsp1];
        OpEspTooltipBox.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 42 + (EspTooltip[OpEsp1].Length + 18) / 18 * 23);
        OpEspTooltipBox.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = ESPList.transform.GetChild(ESPList.transform.childCount - 6 + OpEsp2).GetChild(2).GetComponent<TMP_Text>().text + " ";
        OpEspTooltipBox.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text += YutHanguel[OpEsp2];
        OpEspTooltipBox.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = EspTooltip[ESPList.transform.childCount - 6 + OpEsp2];
    }

    IEnumerator Show_ESP_Choose(int esp1, int esp2)
    {
        print("select Esp");
        PopEsp.SetActive(true);
        int slow_down = 1;
        for (int t = 0; t < LoopEspShowCnt; t++)
        {
            for (int k = 0; k < ESPList.transform.childCount - 6; k++)
            {
                ESPList.transform.GetChild(k).GetChild(3).gameObject.SetActive(true);
                if (k == 0)
                    ESPList.transform.GetChild(ESPList.transform.childCount - 6 - 1).GetChild(3).gameObject.SetActive(false);
                else
                    ESPList.transform.GetChild(k - 1).GetChild(3).gameObject.SetActive(false);

                float timer = 0f;
                while (true)
                {
                    timer += Time.deltaTime;
                    if (timer > EspPopTime * slow_down * EspPopSlowDown)
                        break;
                    yield return null;
                }
                if (t == LoopEspShowCnt - 1 && k == esp1)
                    break;
                if (k % 3 == 0)
                    slow_down++;
            }            
        }

        slow_down = 1;
        for (int t = 0; t < LoopEspShowCnt; t++)
        {
            for (int k = ESPList.transform.childCount - 6; k < ESPList.transform.childCount; k++)
            {
                ESPList.transform.GetChild(k).GetChild(3).gameObject.SetActive(true);
                if (k == ESPList.transform.childCount - 6)
                    ESPList.transform.GetChild(ESPList.transform.childCount - 1).GetChild(3).gameObject.SetActive(false);
                else
                    ESPList.transform.GetChild(k - 1).GetChild(3).gameObject.SetActive(false);

                float timer = 0f;
                while (true)
                {
                    timer += Time.deltaTime;
                    if (timer > EspPopTime * slow_down * EspPopSlowDown)
                        break;
                    yield return null;
                }
                if (t == LoopEspShowCnt - 1 && k == esp2 + ESPList.transform.childCount - 6)
                    break;
                if (k % 3 == 0)
                    slow_down++;
            }            
        }
        float timer2 = 0;        
        while (true)
        {
            timer2 += Time.deltaTime;
            if (timer2 > 1.5)
                break;
            yield return null;
        }
        MyEspList.transform.GetChild(1).GetComponent<Image>().sprite = ESP_sprite[esp1];
        MyEspList.transform.GetChild(2).GetComponent<Image>().sprite = ESP_sprite[ESPList.transform.childCount - 6 + esp2];        
        PV.RPC("set_esp_done", RpcTarget.All);
        print("set_esp_done");
    }

    [PunRPC]
    void set_esp_done()
    {
        print(PhotonNetwork.LocalPlayer.NickName + "is Ready");
        GameObject ready = Instantiate(Ready);
        ready.transform.SetParent(GameStart.transform);
    }


    [PunRPC]
    void change_turn()
    {
        WaitChanging = true;
        if (IsEspIslandUsing)
        {
            if (!MyTurn)
            {
                PopTurn.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ESP_sprite[OpEsp1];
                PopTurn.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "한턴 쉬세요!";
                StartCoroutine(show_turn());
            }
            else
                IsEsp1Used = true;
            Esp1Used();
            MyTurn = !MyTurn;
            IsEspIslandUsing = false;
        }
        else if (IsEspKeepUsing)
        {
            IsEspKeepUsing = false;
        }
        StartCoroutine(wait_and_change());
    }
    IEnumerator wait_and_change()
    {
        float waiting = 0f;
        while (true)
        {
            waiting += Time.deltaTime;
            if (waiting > 0.1f)
                break;
            yield return null;
        }
        print(MyTurn + "!!change turn!!" + PhotonNetwork.LocalPlayer.NickName);
        if (!GameStarted)
        {
            EspDone = true;
            PopEsp.SetActive(false);
            GameStarted = true;
        }

        MyTurn = !MyTurn;
        MyTurnSign.SetActive(MyTurn);
        OpTurnSign.SetActive(!MyTurn);
        IsRollable = MyTurn;
        if (MyTurn)
        {            
            StartCoroutine(show_turn());
            IsRolled = false;
            if (!IsEsp1Used)
                MyEspList.transform.GetChild(1).GetComponent<Button>().interactable = true;
            if (!IsEsp2Used)
                MyEspList.transform.GetChild(2).GetComponent<Button>().interactable = true;

            

        }
        WaitChanging = false;
    }


    IEnumerator show_turn()
    {
        PopTurn.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        PopTurn.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(253/255f, 246/255f, 187/255f, 1f);
        PopTurn.SetActive(true);
        float turn_show_time = 0;
        while (true)
        {
            turn_show_time += Time.deltaTime;
            if (turn_show_time >= ShowPopTime)
            {
                break;
            }
            yield return null;
        }

        while (true)
        {            
            if (PopTurn.transform.GetChild(0).GetComponent<SpriteRenderer>().color.a <= 0)
            {                
                PopTurn.SetActive(false);
                break;
            }
            PopTurn.transform.GetChild(0).GetComponent<SpriteRenderer>().color =
                new Color(1f, 1f, 1f, 
                PopTurn.transform.GetChild(0).GetComponent<SpriteRenderer>().color.a - VenishPopSpeed * Time.deltaTime);
            PopTurn.transform.GetChild(1).GetComponent<SpriteRenderer>().color =
                new Color(253 / 255f, 246 / 255f, 187 / 255f, 
                PopTurn.transform.GetChild(0).GetComponent<SpriteRenderer>().color.a - VenishPopSpeed * Time.deltaTime);
            yield return null;
        }
        PopTurn.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = MyMalImage;
        PopTurn.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "내 차례!";
    }

    #endregion

    #region Chat
    [PunRPC]
    void send_message_in_game(string who, string msg)
    {
        GameObject chatBubble = null;
        if (who == PhotonNetwork.LocalPlayer.NickName)
        {
            if (MyChatBubbleBox.transform.childCount > 1)
                Destroy(MyChatBubbleBox.transform.GetChild(1).gameObject);
            chatBubble = Instantiate(MyChatBubble);
            chatBubble.transform.SetParent(MyChatBubbleBox.transform);
        }
        else
        {
            if (OpChatBubbleBox.transform.childCount > 1)
                Destroy(OpChatBubbleBox.transform.GetChild(1).gameObject);
            chatBubble = Instantiate(OpChatBubble);
            chatBubble.transform.SetParent(OpChatBubbleBox.transform);
        }
        chatBubble.transform.GetChild(1).GetComponent<TMP_Text>().text = msg;
        StartCoroutine(MakeBubble(chatBubble));
    }

    IEnumerator MakeBubble(GameObject bubble)
    {
        bubble.transform.localScale = new Vector3(0f, 1f);
        bubble.transform.localPosition = new Vector3(0f, 0f, 0f);
        bubble.SetActive(true);
        while (true)
        {            
            bubble.transform.localScale = new Vector3(bubble.transform.localScale.x + BubbleSpeed * Time.deltaTime, 1f);
            if (bubble.transform.localScale.x >= 1)           
            {
                bubble.transform.localScale = new Vector3(1f, 1f);
                break;
            }
            yield return null;
        }
        float bubble_show_time = 0;
        while (true)
        {
            bubble_show_time += Time.deltaTime;
            if (bubble_show_time >= BubbleTime)
            {                
                break;
            }
            yield return null;
        }
        Destroy(bubble);

    }
    #endregion

    #region Ending
    void Clear()
    {
        for (int k = 0; k < 6; k++)
        {
            MyYutStackList.transform.GetChild(k).GetChild(2).GetComponent<TMP_Text>().text = (0).ToString();
            MyYutStackList.transform.GetChild(k).GetComponent<Image>().color = new Color(100 / 255f, 100 / 255f, 100 / 255f, 128 / 255f);
            OpYutStackList.transform.GetChild(k).GetChild(2).GetComponent<TMP_Text>().text = (0).ToString();
            OpYutStackList.transform.GetChild(k).GetComponent<Image>().color = new Color(100 / 255f, 100 / 255f, 100 / 255f, 128 / 255f);
        }
        for (int k = 0; k < 4; k++)
        {
            MyStartMalList.transform.GetChild(k).gameObject.SetActive(true);
            MyStartMalList.transform.GetChild(k).GetComponent<Button>().interactable = false;
            OpStartMalList.transform.GetChild(k).gameObject.SetActive(true);
            OpStartMalList.transform.GetChild(k).GetComponent<Button>().interactable = false;
        }
        for (int k = 4; k < 8; k++)
        {
            MyStartMalList.transform.GetChild(k).gameObject.SetActive(false);
            OpStartMalList.transform.GetChild(k).gameObject.SetActive(false);
        }
        on_off_caan_trap(1, false, -1);
        on_off_caan_trap(2, false, -1);
        GameEnd.SetActive(false);
        RollingYut.SetActive(false);
        MyTurn = false;
        IsRolling = false;
        IsMoving = false;
        IsRollable = false;
        IsMalMovable = false;
        IsEsp1Used = false;
        IsEsp1Using = false;
        IsEsp2Used = false;
        IsEsp2Using = false;
        IsEspGoBackUsing = false;
        IsEspIslandUsing = false;
        IsEspKeepUsing = false;
        IsEspFalseStartUsing = false;
        IsEspMetamongUsing = false;
        IsEspChangePosUsing = false;
        IsEspMagnetUsing = false;
        PopEsp.SetActive(false);
        PopEspUsing.SetActive(false);
        PopTurn.SetActive(false);
        for (int k = 2; k < MalBox.transform.childCount; k++)
        {
            Destroy(MalBox.transform.GetChild(k).gameObject);
        }
        for (int k = 0; k < GameStart.transform.childCount; k++)
        {
            Destroy(GameStart.transform.GetChild(k).gameObject);
        }
        for (int k = 0; k < ESPList.transform.childCount; k++)
        {
            ESPList.transform.GetChild(k).GetChild(3).gameObject.SetActive(false);
        }
        MyEspList.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        MyEspList.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        MyEspList.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        MyEspList.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
        MyEspList.transform.GetChild(1).GetComponent<Image>().sprite = ESP_sprite[18];
        MyEspList.transform.GetChild(2).GetComponent<Image>().sprite = ESP_sprite[18];

        OpEspList.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        OpEspList.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);        
        OpEspList.transform.GetChild(1).GetComponent<Image>().sprite = ESP_sprite[18];
        OpEspList.transform.GetChild(2).GetComponent<Image>().sprite = ESP_sprite[18];
        
        turn_on_off_all_caan(false);
        turn_on_off_all_moved_mal(false);
        on_off_caan_trap(1, false, -1);
        on_off_caan_trap(2, false, -1);

    }


    void turn_on_off_all_caan(bool turn)
    {
        for (int k = 0; k <= 30; k++)
        {
            Caan.transform.GetChild(k).GetComponent<Button>().interactable = turn;
            Caan.transform.GetChild(k).GetChild(0).gameObject.SetActive(turn);
        }
    }

    void turn_on_off_all_moved_mal(bool turn)
    {
        for (int k = 2; k < MalBox.transform.childCount; k++)
        {
            if (MalBox.transform.GetChild(k).name[0] == 'M')
                MalBox.transform.GetChild(k).GetComponent<BoxCollider2D>().enabled = turn;
        }
    }

    void end_game()
    {
        WaitCanvas.SetActive(true);
        GameCanvas.SetActive(false);
        GameStarted = false;
        Clear();
    }

    #endregion
}
