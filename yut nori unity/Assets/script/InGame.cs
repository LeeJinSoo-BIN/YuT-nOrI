using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InGame : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    public Sprite[] YutGarak;
    public Sprite[] YutAnimal;
    public Sprite[] Ganzi;
    private string[] YutHanguel = new string[] { "µµ!", "°³!", "°É!", "Àµ!", "¸ð!", "µÞµµ!", "³«!", "µµÂø!" };
    private Color[] YutColor = new Color[] { new Color(255 / 255f, 223 / 255f, 206 / 255f, 1f),
                                            new Color(255 / 255f, 170 / 255f, 90 / 255f, 1f),
                                            new Color(222 / 255f, 219 / 255f, 236 / 255f, 1f),                                            
                                            new Color(55 / 255f, 94 / 255f, 126 / 255f, 1f),
                                            new Color(228 / 255f, 147 / 255f, 107 / 255f, 1f),
                                            new Color(206 / 255f, 255 / 255f, 223 / 255f, 1f),
                                            new Color(200 / 255f, 200 / 255f, 200 / 255f, 0.5f),
                                            new Color(255 / 255f, 128 / 255f, 128 / 255f, 1f)};
    public GameObject ReadyInRoomPanel;

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

    public Button RollButton;
    public GameObject RollingYut;
    public float BackProbability = 42f;
    //private float FrontProbability = 58f;
    public int CurrentYut;
    public bool MyTurn = false;

    public bool IsRollable = false;    
    public bool IsMalMovable = false;
    public bool IsRolling = false;
    public bool IsMoving = false;
    public float MoveSpeed = 3.5f;
    

    private bool IsMyStartMalClicked = false;
    private bool IsMyMovedMalClicked = false;
    public GameObject Caan;
    private GameObject MyClickedMovableMal;
    private int[] MyMalPosition = new int[] { -1, -1, -1, -1 };
    private Sprite MyMalImage;
    private Sprite OpMalImage;
    public GameObject MyMovingMal;
    public GameObject OpMovingMal;
    public GameObject MalBox;
    public GameObject MyTurnSign;
    public GameObject OpTurnSign;

    public PhotonView PV;
    public GameObject WaitCanvas;
    public GameObject GameCanvas;


    public GameObject Baton;
    public GameObject Master;
    public GameObject Slave;
    public GameObject Wait;
    public GameObject GameEnd;

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (MyTurn)
        {
            IsMalMovable = Check_Mal_Movable(MyYutStackList, MyStartMalList);
            if (!IsRollable && !IsMalMovable && !IsRolling && !IsMoving)
            {
                PV.RPC("change_turn", RpcTarget.All);
            }
            if (IsMalMovable && !IsMoving && !IsRolling && !IsMyStartMalClicked)
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
        }
        else
        {
            IsRollable = false;
            IsMalMovable = false;
        }

        RollButton.interactable = (IsRollable && !IsMoving && !IsRolling);


        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (Baton.transform.parent.name == "Master")
                MyTurn = true;
            else
                MyTurn = false;
        }
        else if (!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (Baton.transform.parent.name == "Slave")
                MyTurn = true;
            else
                MyTurn = false;
        }
        else
        {
            MyTurn = false;
        }

      

    }

    public void RollButtonClick()
    {
        IsRollable = false;
        int back_cnt = 0;
        int back_do = 0;
        for(int k = 0; k< 4; k++)        {
            int yut = Random.Range(0, 100);
            if (yut < BackProbability)
            {
                back_cnt++;
                if (k == 0)
                    back_do++;
            }
            else if(yut == BackProbability)
            {
                back_cnt = 7; // ³«
                break;
            }
        }
        if (back_cnt == 1 && back_do == 1)
        {
            CurrentYut = 6; // µÞµµ
        }
        else if (back_cnt == 0) 
        {
            CurrentYut = 5; // ¸ð
        }
        else
        {
            CurrentYut = back_cnt;
        }
        CurrentYut--;//µµ0 °³1 °É2 À·3 ¸ð4 µÞµµ5 ³«6
        PV.RPC("roll_yut", RpcTarget.All, CurrentYut);
    }

    [PunRPC]
    void roll_yut(int rolled_yut)
    {
        RollingYut.SetActive(true);
        IsRolling = true;
        IsRollable = false;
        RollingYut.transform.GetChild(1).GetComponent<Image>().sprite = YutGarak[7];
        RollingYut.transform.GetChild(2).gameObject.SetActive(false);
        RollingYut.transform.GetChild(3).gameObject.SetActive(false);        
        CurrentYut = rolled_yut;
        

        Invoke("show_rolled_yut_and_stack_up", 1f);
        Invoke("action_rolling_result", 2f);
    }
    
    void show_rolled_yut_and_stack_up()
    {
        int rolled_yut = CurrentYut;
        RollingYut.transform.GetChild(2).gameObject.SetActive(true);
        RollingYut.transform.GetChild(3).gameObject.SetActive(true);

        RollingYut.transform.GetChild(1).GetComponent<Image>().sprite = YutGarak[rolled_yut];
        RollingYut.transform.GetChild(2).GetComponent<Image>().sprite = YutAnimal[rolled_yut];
        RollingYut.transform.GetChild(3).GetComponent<TMP_Text>().text = YutHanguel[rolled_yut];

        if (rolled_yut != 6) // ³«
        { 
            if (MyTurn)
            {
                int now_count = int.Parse(MyYutStackList.transform.GetChild(rolled_yut).GetChild(2).GetComponent<TMP_Text>().text) + 1;
                MyYutStackList.transform.GetChild(rolled_yut).GetChild(2).GetComponent<TMP_Text>().text = (now_count).ToString();
                if (now_count > 0)
                    MyYutStackList.transform.GetChild(rolled_yut).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);               
            }
            else
            {
                int now_count = int.Parse(OpYutStackList.transform.GetChild(rolled_yut).GetChild(2).GetComponent<TMP_Text>().text) + 1;
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
            if (IsMalMovable)
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
            if (rolled_yut == 3 || rolled_yut == 4) //À· ¸ð
            {                
                IsRollable = true;
            }
        }
        else
        {
            if(!Check_Mal_Movable(OpYutStackList, OpStartMalList))
            {
                for (int k = 0; k < 6; k++)
                {
                    OpYutStackList.transform.GetChild(k).GetComponent<Image>().color = new Color(100 / 255f, 100 / 255f, 100 / 255f, 128 / 255f);
                    OpYutStackList.transform.GetChild(k).GetChild(2).GetComponent<TMP_Text>().text = "0";
                }
            }
        }
        IsRolling = false;
    }



    public void MyStartMalClick()
    {
        
        MyClickedMovableMal = EventSystem.current.currentSelectedGameObject;
        IsMyStartMalClicked = !IsMyStartMalClicked;
        IsMyMovedMalClicked = false;
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
        IsMyStartMalClicked = false;
        MyClickedMovableMal = clicked_moved_mal;

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
                            if(clicked_mal_pos + k + 1 + adj == 22)
                            {
                                adj = 19;
                            }
                        }
                        else if (clicked_mal_pos + k + 1 + adj >= 25 && (20<=clicked_mal_pos && clicked_mal_pos <= 24) && clicked_mal_pos != 22)
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
        int moving_yut = int.Parse(current_clicked_caan.transform.GetChild(0).GetChild(0).name);
        if (IsMyStartMalClicked)
        {
            turn_on_off_all_caan(false);
            int my_clicked_start_mal_num = MyClickedMovableMal.name[4] - '1';
            MyMalPosition[my_clicked_start_mal_num] = clicked_caan_num;
            

            PV.RPC("moving_start_mal", RpcTarget.All, my_clicked_start_mal_num, clicked_caan_num, moving_yut);            
            IsMyStartMalClicked = false;
        }
        if (IsMyMovedMalClicked)
        {
            turn_on_off_all_caan(false);

            int clicked_mal_pos = int.Parse(MyClickedMovableMal.transform.GetChild(2).name);            
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
        moving_mal.transform.GetChild(2).GetComponent<TMP_Text>().text = clicked_caan_num.ToString();
        moving_mal.transform.GetChild(2).name = clicked_caan_num.ToString();
        moving_mal.name += clicked_start_mal_num.ToString();
        moving_mal.SetActive(true);

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
            for(int k = 2; k < MalBox.transform.childCount; k++)
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


        moving_mal.transform.GetChild(2).GetComponent<TMP_Text>().text = (int.Parse(current_clicked_caan.name)).ToString();
        moving_mal.transform.GetChild(2).name = moving_mal.transform.GetChild(2).GetComponent<TMP_Text>().text;

    
        StartCoroutine(MoveMotion(moving_mal, current_clicked_caan, moving_yut));


    }
    Vector3 way_over(int mal_pos, int caan_pos)
    {
        if ((1 <= mal_pos && mal_pos <= 4) && 5 < caan_pos)
            return Caan.transform.GetChild(5).position + new Vector3(0f, 0.15f);
        else if ((6 <= mal_pos && mal_pos <= 9) && 10 < caan_pos)
            return Caan.transform.GetChild(10).position + new Vector3(0f, 0.15f);
        else if ((11 <= mal_pos && mal_pos <= 14) && 15 < caan_pos)
            return Caan.transform.GetChild(15).position + new Vector3(0f, 0.15f);
        else if ((20 <= mal_pos && mal_pos <= 24) && 16 <= caan_pos && caan_pos<=19)
            return Caan.transform.GetChild(15).position + new Vector3(0f, 0.15f);
        return new Vector3(0f, 0f);
    }
    IEnumerator MoveMotion(GameObject moving_mal, GameObject des_caan, int moving_cnt)
    {
        int moving_mal_pos = int.Parse(moving_mal.transform.GetChild(2).name) - (moving_cnt + 1);
        int caan_pos = int.Parse(des_caan.name);
        Vector3 layover_pos = way_over(moving_mal_pos, caan_pos) ;
        if (layover_pos.magnitude != 0)
        {
            while (true)
            {
                moving_mal.transform.position = Vector3.MoveTowards(moving_mal.transform.position, layover_pos, MoveSpeed * Time.deltaTime);
                if (Vector3.Magnitude(moving_mal.transform.position - layover_pos) < 0.0001f)
                {
                    moving_mal.transform.position = layover_pos;
                    break;
                }
                yield return null;
            }
        }

        Vector3 des_pos = des_caan.transform.position + new Vector3(0f, 0.15f);
        while (true)
        {
            moving_mal.transform.position = Vector3.MoveTowards(moving_mal.transform.position, des_pos, MoveSpeed * Time.deltaTime);
            if (Vector3.Magnitude(moving_mal.transform.position - des_pos) < 0.0001f)
            {
                moving_mal.transform.position = des_pos;
                action_moving_result(moving_mal, moving_cnt);
                break;
            }
            yield return null;
        }
    }
    void action_moving_result(GameObject moved_mal, int moving_yut) { 
        List<GameObject> destroy_list = new List<GameObject>();
        string moved_mal_pos = moved_mal.transform.GetChild(2).GetComponent<TMP_Text>().text;
        int moved_mal_cnt = int.Parse(moved_mal.transform.GetChild(1).GetComponent<TMP_Text>().text);

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
                    if(goal_cnt == 4)
                    {
                        for(int t = 2; t < MalBox.transform.childCount; t++)
                        {
                            Destroy(MalBox.transform.GetChild(t).gameObject);
                        }
                        GameEnd.transform.GetChild(0).GetComponent<TMP_Text>().text = "½Â¸®!!";
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
                        GameEnd.transform.GetChild(0).GetComponent<TMP_Text>().text = "ÆÐ¹è ¤Ì¤Ì";
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
                if (current_mal_name[0] == moved_mal.name[0]) { //°°Àº »ç¶÷ ¸»
                    moved_mal.transform.GetChild(1).GetComponent<TMP_Text>().text = (moved_mal_cnt + current_mal_cnt).ToString();
                    destroy_list.Add(MalBox.transform.GetChild(k).gameObject);
                }
                else // ´Ù¸¥ »ç¶÷ ¸»À» ÀâÀ½
                { 
                    destroy_list.Add(MalBox.transform.GetChild(k).gameObject);
                    int catched_mal_cnt = int.Parse(MalBox.transform.GetChild(k).GetChild(1).GetComponent<TMP_Text>().text);
                    int return_cnt = 0;                    
                    for (int t = 0; t < 4; t++)
                    {
                        if (MyTurn) // ³»°¡ »ó´ë¸» ÀâÀ½
                        {
                            if (!OpStartMalList.transform.GetChild(t).gameObject.activeSelf && !OpStartMalList.transform.GetChild(t + 4).gameObject.activeSelf)
                            {
                                OpStartMalList.transform.GetChild(t).gameObject.SetActive(true);
                                return_cnt++;
                            }
                            if(moving_yut != 3 && moving_yut != 4)//À·ÀÌ³ª ¸ð·Î ÀâÀº°Å ¾Æ´Ï¸é
                                IsRollable = true; // ÇÑ¹ø ´õ ±¼¸²
                        }
                        else // »ó´ë°¡ ³»¸» ÀâÀ½
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
        for(int k = 0; k< destroy_list.Count; k++)
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
        for(int k = 0; k< 5; k++)
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
        MyName = MyInfoList.transform.GetChild(2).GetComponent<TMP_Text>();
        MyStartMalList = MyInfoList.transform.GetChild(3).gameObject;
        MyEspList = MyInfoList.transform.GetChild(4).gameObject;
        MyYutStackList = MyInfoList.transform.GetChild(5).gameObject;

        OpName = OpInfoList.transform.GetChild(2).GetComponent<TMP_Text>();
        OpStartMalList = OpInfoList.transform.GetChild(3).gameObject;
        OpEspList = OpInfoList.transform.GetChild(4).gameObject;
        OpYutStackList = OpInfoList.transform.GetChild(5).gameObject;


        MyName.text = PhotonNetwork.LocalPlayer.NickName;
        OpName.text = PhotonNetwork.PlayerListOthers[0].NickName;

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            int turn = Random.Range(0, 2);
            int master_mal = Random.Range(0, 7);
            int slave__mal = Random.Range(0, 7);
            while (master_mal == slave__mal)
                master_mal = Random.Range(0, 7);
            PV.RPC("set_turn_and_character", RpcTarget.All, turn, master_mal, slave__mal);
            PV.RPC("change_turn", RpcTarget.All);
        }
        Clear();
    }


    [PunRPC]
    void change_turn()
    {
        print("!!change turn!!");
        if (Master.transform.childCount == 1)
            Baton.transform.SetParent(Slave.transform);
        else if (Slave.transform.childCount == 1)
            Baton.transform.SetParent(Master.transform);
        IsRollable = true;
        if (MyTurn)
        {
            MyTurnSign.SetActive(false);
            OpTurnSign.SetActive(true);
        }
        else
        {
            MyTurnSign.SetActive(true);
            OpTurnSign.SetActive(false);
        }
        MyTurn = !MyTurn;
    }
    [PunRPC]
    void set_turn_and_character(int turn, int master_mal, int slave_mal)
    {
        print("setting");
        if (turn == 0)
        {
            Baton.transform.SetParent(Master.transform);
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
            Baton.transform.SetParent(Slave.transform);
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
        }
        else
        {
            MyMalImage = Ganzi[slave_mal];
            OpMalImage = Ganzi[master_mal];            
        }
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
    }

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
        GameEnd.SetActive(false);
        IsRolling = false;
        IsMoving = false;
        IsRollable = false;
        IsMalMovable = false;

        for (int k = 2; k < MalBox.transform.childCount; k++)
        {
            Destroy(MalBox.transform.GetChild(k).gameObject);
        }

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
        
        Clear();
    }    
}
