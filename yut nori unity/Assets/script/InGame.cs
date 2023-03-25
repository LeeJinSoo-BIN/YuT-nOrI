using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InGame        : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    public Sprite[] YutGarak;
    public Sprite[] YutAnimal;
    public Sprite[] Ganzi;
    private string[] YutHanguel = new string[] { "µµ!", "°³!", "°É!", "Àµ!", "¸ð!", "µÞµµ!", "³«!" };
    private Color[] YutColor = new Color[] { new Color(255 / 255f, 223 / 255f, 206 / 255f, 1f),
                                            new Color(255 / 255f, 170 / 255f, 90 / 255f, 1f),
                                            new Color(222 / 255f, 219 / 255f, 236 / 255f, 1f),                                            
                                            new Color(55 / 255f, 94 / 255f, 126 / 255f, 1f),
                                            new Color(228 / 255f, 147 / 255f, 107 / 255f, 1f),
                                            new Color(206 / 255f, 255 / 255f, 223 / 255f, 1f)};
    public GameObject ReadyInRoomPanel;

    public GameObject MyInfoList;
    public GameObject OpInfoList;
    private TMP_Text MyName;
    private TMP_Text OpName;
    private GameObject MyMalList;
    private GameObject OpMalList;
    private GameObject MyEspList;
    private GameObject OpEspList;
    private GameObject MyYutStackList;
    private GameObject OpYutStackList;    

    public Button RollButton;
    public GameObject RollingYut;
    private float BackProbability = 42f;
    //private float FrontProbability = 58f;
    public int CurrentYut;
    public bool IsRolling = false;
    public bool IsRolled = false;
    public bool MyTurn = false;

    private bool IsMyMalClicked = false;
    public GameObject Caan;
    private GameObject MyClickedStartMal;
    private int[] MyMalPosition = new int[] { -1, -1, -1, -1 };
    private Sprite MyMalImage;
    private Sprite OpMalImage;
    public GameObject MyMovingMal;
    public GameObject OpMovingMal;
    public GameObject MalBox;
    public bool IsMoving = false;

    public PhotonView PV;
    public GameObject WaitCanvas;
    public GameObject GameCanvas;


    public GameObject Baton;
    public GameObject Master;
    public GameObject Slave;
    public GameObject Wait;
    public GameObject MyTurnFlag;
    public GameObject TurnEndFlag;

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (IsRolling)
        {
            if (IsRolled) {
                if (MyTurn)
                {
                    if (IsStackEmpty(MyYutStackList))
                    {
                        IsRolling = false;
                    }                    
                }
                else
                {

                }
            }
        }
        else
        {
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
            RollButton.interactable = MyTurn;
        }
        MyTurnFlag.SetActive(MyTurn);        
    }

    public void RollButtonClick()
    {
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
            CurrentYut = back_cnt; //µµ1 °³2 °É3 À·4 ¸ð5 µÞµµ6 ³«7
        }
        CurrentYut--;
        PV.RPC("roll_yut", RpcTarget.All, CurrentYut);
    }
    [PunRPC]
    void roll_yut(int rolled_yut)
    {        
        RollingYut.SetActive(true);
        RollButton.interactable = false;
        RollingYut.transform.GetChild(1).GetComponent<Image>().sprite = YutGarak[7];
        RollingYut.transform.GetChild(2).gameObject.SetActive(false);
        RollingYut.transform.GetChild(3).gameObject.SetActive(false);
        IsRolling = true;
        CurrentYut = rolled_yut;        


        Invoke("rolled_yut_result", 1f);
        Invoke("rolled_yut_end", 2f);
    }
    
    void rolled_yut_result()
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
    void rolled_yut_end()
    {
        int rolled_yut = CurrentYut;
        RollingYut.SetActive(false);
        if (MyTurn)
        {
            bool stack = false;
            for(int k = 0; k< 5; k++)
            {
                if (int.Parse(MyYutStackList.transform.GetChild(k).GetChild(2).GetComponent<TMP_Text>().text) > 0)
                {
                    stack = true;
                    break;
                }
            }
            if (stack)
            {
                for (int t = 0; t < 4; t++)
                {
                    MyMalList.transform.GetChild(t).GetComponent<Button>().interactable = true;
                }
            }
            else
            {
                for (int t = 0; t < 4; t++)
                {
                    MyMalList.transform.GetChild(t).GetComponent<Button>().interactable = false;
                }
            }
            if (rolled_yut == 3 || rolled_yut == 4)
                RollButton.interactable = true;            
        }
        IsRolled = true;
    }
    public void MyStartMalClick()
    {
        MyClickedStartMal = EventSystem.current.currentSelectedGameObject;
        IsMyMalClicked = !IsMyMalClicked;
        if (IsMyMalClicked)
        {
            for (int k = 0; k < 5; k++)
            {
                int yut = int.Parse(MyYutStackList.transform.GetChild(k).GetChild(2).GetComponent<TMP_Text>().text);
                if (yut > 0)
                {
                    Caan.transform.GetChild(k + 1).GetComponent<Button>().interactable = true;
                    Caan.transform.GetChild(k + 1).GetChild(0).GetComponent<Image>().color = YutColor[k];
                    Caan.transform.GetChild(k + 1).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = YutHanguel[k];
                    Caan.transform.GetChild(k + 1).GetChild(0).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            for (int k = 0; k < 5; k++)
            {
                Caan.transform.GetChild(k + 1).GetComponent<Button>().interactable = false;
                Caan.transform.GetChild(k + 1).GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void CaanClick()
    {
        GameObject current_clicked_caan = EventSystem.current.currentSelectedGameObject;
        int clicked_caan_num = int.Parse(current_clicked_caan.name);
        if (IsMyMalClicked)
        {
            int my_clicked_start_mal_num = MyClickedStartMal.name[4] - '1';
            MyMalPosition[my_clicked_start_mal_num] = clicked_caan_num;
            MyClickedStartMal.SetActive(false);
            PV.RPC("moving_mal", RpcTarget.All, my_clicked_start_mal_num, clicked_caan_num);

            for (int k = 0; k < 5; k++)
            {
                Caan.transform.GetChild(k + 1).GetComponent<Button>().interactable = false;
                Caan.transform.GetChild(k + 1).GetChild(0).gameObject.SetActive(false);
            }
            IsMyMalClicked = false;
        }
    }

    [PunRPC]
    void moving_mal(int clicked_start_mal_num, int clicked_caan_num)
    {
        print("rpc");
        IsMoving = true;
        GameObject moving_mal;        
        GameObject current_clicked_caan = Caan.transform.GetChild(clicked_caan_num).gameObject;
        if (MyTurn)
        {
            GameObject current_clicked_mal = MyMalList.transform.GetChild(clicked_start_mal_num).gameObject;
            current_clicked_mal.SetActive(false);
            moving_mal = Instantiate(MyMovingMal);
            int yut_stack = int.Parse(MyYutStackList.transform.GetChild(clicked_caan_num - 1).GetChild(2).GetComponent<TMP_Text>().text);
            MyYutStackList.transform.GetChild(clicked_caan_num - 1).GetChild(2).GetComponent<TMP_Text>().text = (yut_stack - 1).ToString();
            if(yut_stack-1 == 0)
            {
                MyYutStackList.transform.GetChild(clicked_caan_num - 1).GetComponent<Image>().color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 128 / 255f);
            }
        }
        else
        {
            GameObject current_clicked_mal = OpMalList.transform.GetChild(clicked_start_mal_num).gameObject;
            current_clicked_mal.SetActive(false);
            moving_mal = Instantiate(OpMovingMal);
            int yut_stack = int.Parse(OpYutStackList.transform.GetChild(clicked_caan_num - 1).GetChild(2).GetComponent<TMP_Text>().text);
            OpYutStackList.transform.GetChild(clicked_caan_num - 1).GetChild(2).GetComponent<TMP_Text>().text = (yut_stack - 1).ToString();
            if (yut_stack - 1 == 0)
            {
                OpYutStackList.transform.GetChild(clicked_caan_num - 1).GetComponent<Image>().color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 128 / 255f);
            }
        }
        moving_mal.transform.SetParent(MalBox.transform);
        moving_mal.transform.position = Caan.transform.GetChild(0).position;
        moving_mal.SetActive(true);

        StartCoroutine(MoveMotion(moving_mal, current_clicked_caan.transform.position));
    }

    /*    IEnumerator MoveMotion(GameObject moving_mal, Vector3 des)
        {
            while (true)
            {
                Vector3.MoveTowards(moving_mal.transform.position, des, 1);
                if (Vector3.Magnitude(moving_mal.transform.position - des) < 0.01f)
                    break;
                yield return null;
            }
            moving_mal.transform.position = des;
            IsMoving = false;        

        }
    */
    IEnumerator MoveMotion(GameObject moving_mal, Vector3 des)
    {
        float count = 0;
        Vector3 ori_pos = moving_mal.transform.position;
        while (true)
        {
            count += Time.deltaTime;
            moving_mal.transform.position = Vector3.Lerp(ori_pos, des, count);
            if (count >= 1)
            {
                moving_mal.transform.position = des;
                break;
            }
            yield return null;
        }
        IsMoving = false;
    }


    bool IsStackEmpty(GameObject StackList)
    {
        for(int k = 0; k< 5; k++)
        {
            if (int.Parse(StackList.transform.GetChild(k).GetChild(2).GetComponent<TMP_Text>().text) > 0)
                return false;
        }
        if (int.Parse(StackList.transform.GetChild(5).GetChild(2).GetComponent<TMP_Text>().text) > 0)
        {

        }
        return true;
    }

    bool IsMalMoved(GameObject StackList)
    {
        for (int k = 0; k < 4; k++)
        {
            if (!StackList.transform.GetChild(k).gameObject.activeSelf)
                return true;
        }
        return false;
    }

    public void EndGameButtonClick()
    {
        PV.RPC("end_game", RpcTarget.All);
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
        MyName = MyInfoList.transform.GetChild(1).GetComponent<TMP_Text>();
        MyMalList = MyInfoList.transform.GetChild(2).gameObject;
        MyEspList = MyInfoList.transform.GetChild(3).gameObject;
        MyYutStackList = MyInfoList.transform.GetChild(4).gameObject;

        OpName = OpInfoList.transform.GetChild(1).GetComponent<TMP_Text>();
        OpMalList = OpInfoList.transform.GetChild(2).gameObject;
        OpEspList = OpInfoList.transform.GetChild(3).gameObject;
        OpYutStackList = OpInfoList.transform.GetChild(4).gameObject;


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
        }


        
        Clear();
    }
    [PunRPC]
    void set_turn_and_character(int turn, int master_mal, int slave_mal)
    {
        if (turn == 0)
            Baton.transform.SetParent(Master.transform);
        else
            Baton.transform.SetParent(Slave.transform);

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
            MyMalList.transform.GetChild(k).GetComponent<Image>().sprite = MyMalImage;
            MyMalList.transform.GetChild(k).GetComponent<Button>().interactable = false;
            MyMalList.transform.GetChild(k).GetChild(0).gameObject.SetActive(false);
            OpMalList.transform.GetChild(k).GetComponent<Image>().sprite = OpMalImage;
            OpMalList.transform.GetChild(k).GetChild(0).gameObject.SetActive(false);
        }
        MyMovingMal.GetComponent<SpriteRenderer>().sprite = MyMalImage;
        OpMovingMal.GetComponent<SpriteRenderer>().sprite = OpMalImage;
    }

    void Clear()
    {
        for (int k = 0; k < 6; k++)
        {
            MyYutStackList.transform.GetChild(k).GetChild(2).GetComponent<TMP_Text>().text = (0).ToString();
            MyYutStackList.transform.GetChild(k).GetComponent<Image>().color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 128 / 255f);
            OpYutStackList.transform.GetChild(k).GetChild(2).GetComponent<TMP_Text>().text = (0).ToString();
            OpYutStackList.transform.GetChild(k).GetComponent<Image>().color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 128 / 255f);
        }
        for (int k = 0; k < 4; k++)
        {
            MyMalList.transform.GetChild(k).gameObject.SetActive(true);
            MyMalList.transform.GetChild(k).GetComponent<Button>().interactable = false;
            MyMalList.transform.GetChild(k).GetChild(0).gameObject.SetActive(false);
            OpMalList.transform.GetChild(k).gameObject.SetActive(true);
            OpMalList.transform.GetChild(k).GetComponent<Button>().interactable = false;
            OpMalList.transform.GetChild(k).GetChild(0).gameObject.SetActive(false);
        }
    }
    [PunRPC]
    void end_game()
    {
        WaitCanvas.SetActive(true);
        GameCanvas.SetActive(false);
        IsRolling = false;
        for(int k = 2; k < MalBox.transform.childCount; k++)
        {
            Destroy(MalBox.transform.GetChild(k).gameObject);
        }
    }

}
