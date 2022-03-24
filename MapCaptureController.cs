using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MapCaptureController : MonoBehaviour
{
    public static MapCaptureController Instance;

    public float allCaptured = 0;

    public int captureObject, playersInRoom;

    public float teamOneCaptured, teamTwoCaptured;

    public static bool isEnd, teamOneCompleted, teamTwoCompleted;

    public GameObject diamondTeam1_1, diamondTeam1_2, diamondTeam1_3, diamondTeam1_4, diamondTeam1_5, diamondTeam1_6;
    public GameObject diamondTeam2_1, diamondTeam2_2, diamondTeam2_3, diamondTeam2_4, diamondTeam2_5, diamondTeam2_6;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playersInRoom = (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayersInRoom"];
        isEnd = false;

        if (playersInRoom >= 1 && playersInRoom <= 3)
        {
            captureObject = 2;
        }
        if (playersInRoom >= 4 && playersInRoom <= 5)
        {
            captureObject = 4;
        }
        if (playersInRoom >= 6)
        {
            captureObject = 6;
        }

        teamOneCaptured = 0;
        teamTwoCaptured = 0;
    }

    private void Update()
    {
        /*if(playersInRoom >= 1 && playersInRoom <= 3)
        {
            captureObject = 2;
        }
        if (playersInRoom >= 4 && playersInRoom <= 5)
        {
            captureObject = 4;
        }
        if (playersInRoom >= 6)
        {
            captureObject = 6;
        }*/

        TeamOneGetCaptured();
        TeamTwoGetCaptured();
        
        if (teamOneCaptured >= captureObject)
        {
            //isEnd = true;
            teamOneCompleted = true;
        }
        if (teamTwoCaptured >= captureObject)
        {
            //isEnd = true;
            teamTwoCompleted = true;
        }
    }


    public void TeamOneGetCaptured()
    {
        if (teamOneCaptured >= 1)
        {
            diamondTeam1_1.SetActive(true);
        }
        if (teamOneCaptured >= 2)
        {
            diamondTeam1_2.SetActive(true);
        }
        if (teamOneCaptured >= 3)
        {
            diamondTeam1_3.SetActive(true);
        }
        if (teamOneCaptured >= 4)
        {
            diamondTeam1_4.SetActive(true);
        }
        if (teamOneCaptured >= 5)
        {
            diamondTeam1_5.SetActive(true);
        }
        if (teamOneCaptured >= 6)
        {
            diamondTeam1_6.SetActive(true);
        }
    }

    public void TeamTwoGetCaptured()
    {
        if (teamTwoCaptured >= 1)
        {
            diamondTeam2_1.SetActive(true);
        }
        if (teamTwoCaptured >= 2)
        {
            diamondTeam2_2.SetActive(true);
        }
        if (teamTwoCaptured >= 3)
        {
            diamondTeam2_3.SetActive(true);
        }
        if (teamTwoCaptured >= 4)
        {
            diamondTeam2_4.SetActive(true);
        }
        if (teamTwoCaptured >= 5)
        {
            diamondTeam2_5.SetActive(true);
        }
        if (teamTwoCaptured >= 6)
        {
            diamondTeam2_6.SetActive(true);
        }
    }
}
