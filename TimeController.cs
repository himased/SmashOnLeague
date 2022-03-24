using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public float countTime;
    float warmUpTime;

    //[SerializeField] float gameTime, phaseTime;
    [SerializeField] TMP_Text timeText;

    public Image timerCountUI;

    PhotonView PV;

    public static TimeController Instance;

    //PlayerManager playerManager;

    //public static bool isStart, inRound;

    public static bool isEnd;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        //playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        if(GameSetup.GS.playersInRoom >= 1 && GameSetup.GS.playersInRoom <= 3)
        {
            warmUpTime = 720;
        }
        if (GameSetup.GS.playersInRoom >= 4 && GameSetup.GS.playersInRoom <= 5)
        {
            warmUpTime = 900;
        }
        if (GameSetup.GS.playersInRoom >= 6)
        {
            warmUpTime = 1000;
        }

        countTime = warmUpTime;

        isEnd = false;
        //isStart = false;
        //inRound = false;
        //phaseTime = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        OnGameCount();
        timerCountUI.fillAmount = countTime / warmUpTime;
    }
    
    void OnGameCount()
    {
        countTime -= Time.deltaTime;
        //PV.RPC("RPC_SentTime", RpcTarget.All, countTime);
        if (countTime <= 0)
        {
            isEnd = true;
            //GameSetup.teamOneScore = 0;
            //GameSetup.teamTwoScore = 0;
            //countTime = gameTime;
            //isStart = true;
            /*if (GameSetup.isEndRound == true )
            {
                countTime += phaseTime;
                GameSetup.isEndRound = false;
            }*/
            countTime -= Time.deltaTime;
        }
        timeText.text = countTime.ToString("0");
    }

    [PunRPC]
    void RPC_SentTime(float _countTime)
    {
        countTime = _countTime;
    }
}
