using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;


public class InGame : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    public Sprite[] YutGarak;
    public Sprite[] YutAnimal;
    public Sprite[] Ganzi;
    private string[] YutHanguel = new string[] { "µÞµµ!", "µµ!", "°³!", "°É!", "Àµ!", "¸ð!", "³«!" };


    public GameObject Caan;
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
    public bool MyTurn = false;

    public PhotonView PV;
    public GameObject WaitCanvas;
    public GameObject GameCanvas;


    public GameObject Baton;
    public GameObject Master;
    public GameObject Slave;
    public GameObject Wait;



    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (IsRolling)
        {
            RollButton.interactable = false;
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
                back_cnt = 6; // ³«
                break;
            }
        }
        if (back_cnt == 1 && back_do == 1)
        {
            CurrentYut = -1; // µÞµµ
        }
        else if (back_cnt == 0) 
        {
            CurrentYut = 5; // ¸ð
        }
        else
        {
            CurrentYut = back_cnt; //µµ °³ °É À·
        }
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
        
    }
    
    void rolled_yut_result()
    {
        int rolled_yut = CurrentYut;
        RollingYut.transform.GetChild(2).gameObject.SetActive(true);
        RollingYut.transform.GetChild(3).gameObject.SetActive(true);

        if (rolled_yut == -1)
        {
            RollingYut.transform.GetChild(1).GetComponent<Image>().sprite = YutGarak[0];
            RollingYut.transform.GetChild(2).GetComponent<Image>().sprite = YutAnimal[0];
            RollingYut.transform.GetChild(3).GetComponent<TMP_Text>().text = YutHanguel[0];

        }
        else
        {
            RollingYut.transform.GetChild(1).GetComponent<Image>().sprite = YutGarak[rolled_yut];
            RollingYut.transform.GetChild(2).GetComponent<Image>().sprite = YutAnimal[rolled_yut];
            RollingYut.transform.GetChild(3).GetComponent<TMP_Text>().text = YutHanguel[rolled_yut];
        }


        if (rolled_yut != 6)
        {
            if (rolled_yut == -1)
                rolled_yut = 6;
            rolled_yut--;
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


        Invoke("rolled_yut_end", 1.5f);
    }
    void rolled_yut_end()
    {
        int rolled_yut = CurrentYut;
        if (rolled_yut != 4 && rolled_yut != 5)
        {
            if (Master.transform.childCount == 0)
                Baton.transform.SetParent(Master.transform);
            else
                Baton.transform.SetParent(Slave.transform);
        }
        IsRolling = false;
        RollingYut.SetActive(false);
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
        print("gamse start");
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
            for(int k = 0; k< 4; k++)
            {
                MyMalList.transform.GetChild(k).GetComponent<Image>().sprite = Ganzi[master_mal];
                MyMalList.transform.GetChild(k).GetChild(0).gameObject.SetActive(false);
                OpMalList.transform.GetChild(k).GetComponent<Image>().sprite = Ganzi[slave_mal];
                OpMalList.transform.GetChild(k).GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            for (int k = 0; k < 4; k++)
            {
                MyMalList.transform.GetChild(k).GetComponent<Image>().sprite = Ganzi[slave_mal];
                MyMalList.transform.GetChild(k).GetChild(0).gameObject.SetActive(false);
                OpMalList.transform.GetChild(k).GetComponent<Image>().sprite = Ganzi[master_mal];
                OpMalList.transform.GetChild(k).GetChild(0).gameObject.SetActive(false);
            }
        }
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
    }
    [PunRPC]
    void end_game()
    {
        WaitCanvas.SetActive(true);
        GameCanvas.SetActive(false);
    }

}
