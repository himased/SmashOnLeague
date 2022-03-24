using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviourPunCallbacks
{
    public static GameSetup GS;

    public static bool isEndRound = false, enableFirstMapAreaTeamOne = false, enableFirstMapAreaTeamTwo = false;
    public static bool enableSecondMapAreaTeamOne = false, enableSecondMapAreaTeamTwo = false;

    public int nextPlayersTeam;
    public Transform[] spawnPointsTeamOne;
    public Transform[] spawnPointsTeamTwo;

    public static int teamOneScore = 0;
    public static int teamTwoScore = 0;
    public Text teamOneScoreText;
    public Text teamTwoScoreText;
    public Text killWinScoreText;

    public GameObject youWin;
    public GameObject youLose;
    public GameObject pauseMenu;
    public GameObject firstAreaAppealUI, secondAreaAppealUI;

    bool closeAlert_1, closeAlert_2;

    public int playersInRoom, gameSet;

    bool closePause;

    private void Awake()
    {
        GS = this;

        SetScoreText();
        youWin.SetActive(false);
        youLose.SetActive(false);
        pauseMenu.SetActive(false);

        teamOneScore = 0;
        teamTwoScore = 0;

        isEndRound = false; 
        enableFirstMapAreaTeamOne = false; 
        enableFirstMapAreaTeamTwo = false;
        enableSecondMapAreaTeamOne = false; 
        enableSecondMapAreaTeamTwo = false;

        playersInRoom = (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayersInRoom"];
    }

    /*private void OnEnable()
    {
        if (GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }*/

    private void Update()
    {
        SetGameKilled();
        SetScoreText();
        GameDeathMatch();
        AreaAlert();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            closePause = !closePause;
        }

        if(closePause == true)
        {
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.SetActive(true);
        }
        if(closePause == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.SetActive(false);
        }

        killWinScoreText.text = gameSet.ToString();

        if(closeAlert_1 == true)
        {
            firstAreaAppealUI.SetActive(false);
        }
        if (closeAlert_2 == true)
        {
            secondAreaAppealUI.SetActive(false);
        }

        if (TimeController.isEnd == true)
        {
            if(teamOneScore > teamTwoScore && PlayerManager.Instance.myTeam == 1)
            {
                youWin.SetActive(true);
            }
            if (teamOneScore > teamTwoScore && PlayerManager.Instance.myTeam == 2)
            {
                youLose.SetActive(true);
            }
            if (teamOneScore < teamTwoScore && PlayerManager.Instance.myTeam == 1)
            {
                youLose.SetActive(true);
            }
            if (teamOneScore < teamTwoScore && PlayerManager.Instance.myTeam == 2)
            {
                youWin.SetActive(true);
            }
            if (teamOneScore == teamTwoScore)
            {
                youWin.SetActive(true);
            }
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.SetActive(true);
            //StartCoroutine("WaitAndDisconnect");
        }
    }

    IEnumerator WaitAndDisconnect()
    {
        yield return new WaitForSeconds(10f);

        DisconnectPlayer();
    }

    public void DisconnectPlayer()
    {
        Destroy(RoomManager.Instance.gameObject);
        //StartCoroutine(DisconnectAndLoad());
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
        base.OnLeftRoom();
    }

    IEnumerator DisconnectAndLoad()
    {
        //PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        //while (PhotonNetwork.IsConnected)
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene("MainMenu");
    }

    void SetScoreText()
    {
        teamOneScoreText.text = teamOneScore.ToString();
        teamTwoScoreText.text = teamTwoScore.ToString();
    }

    void SetGameKilled()
    {
        if (playersInRoom >= 1 && playersInRoom <= 3)
        {
            gameSet = 16;
        }
        if (playersInRoom >= 4 && playersInRoom <= 5)
        {
            gameSet = 24;
        }
        if (playersInRoom >= 6)
        {
            gameSet = 36;
        }
    }

    void GameDeathMatch()
    {
        if (teamOneScore >= gameSet / 4 || teamTwoScore - teamOneScore >= gameSet / 2)
        {
            enableFirstMapAreaTeamOne = true;
        }
        if (teamTwoScore >= gameSet / 4 || teamOneScore - teamTwoScore >= gameSet / 2)
        {
            enableFirstMapAreaTeamTwo = true;
        }

        if (teamOneScore >= gameSet / 2 && MapCaptureController.Instance.allCaptured == 1)
        {
            enableSecondMapAreaTeamOne = true;
        }
        if (teamTwoScore >= gameSet / 2 && MapCaptureController.Instance.allCaptured == 1)
        {
            enableSecondMapAreaTeamTwo = true;
        }

        if (teamTwoScore >= gameSet)
        {
            MapCaptureController.isEnd = true;
            if (PlayerManager.Instance.myTeam == 2 && MapCaptureController.isEnd == true)
            {
                youWin.SetActive(true);
            }
            else
            {
                youLose.SetActive(true);
            }
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.SetActive(true);
        }
        if (teamOneScore >= gameSet)
        {
            MapCaptureController.isEnd = true;
            if (PlayerManager.Instance.myTeam == 1 && MapCaptureController.isEnd == true)
            {
                youWin.SetActive(true);
            }
            else
            {
                youLose.SetActive(true);
            }
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.SetActive(true);
        }
    }

    void AreaAlert()
    {
        if (teamOneScore >= gameSet / 4 && PlayerManager.Instance.myTeam == 1)
        {
            firstAreaAppealUI.SetActive(true);
            StartCoroutine("FirstAreaCloseAlert");
        }
        if (teamTwoScore >= gameSet / 4 && PlayerManager.Instance.myTeam == 2)
        {
            firstAreaAppealUI.SetActive(true);
            StartCoroutine("FirstAreaCloseAlert");
        }

        if (teamOneScore >= gameSet / 2 && MapCaptureController.Instance.allCaptured == 1 && PlayerManager.Instance.myTeam == 1)
        {
            secondAreaAppealUI.SetActive(true);
            StartCoroutine("SecondAreaCloseAlert");
        }
        if (teamTwoScore >= gameSet / 2 && MapCaptureController.Instance.allCaptured == 1 && PlayerManager.Instance.myTeam == 2)
        {
            secondAreaAppealUI.SetActive(true);
            StartCoroutine("SecondAreaCloseAlert");
        }
    }

    IEnumerator FirstAreaCloseAlert()
    {
        yield return new WaitForSeconds(5f);
        firstAreaAppealUI.SetActive(false);
        closeAlert_1 = true;
    }

    IEnumerator SecondAreaCloseAlert()
    {
        yield return new WaitForSeconds(5f);
        secondAreaAppealUI.SetActive(false);
        closeAlert_2 = true;
    }

    /*public void UpdateTeam()
    {
        if (nextPlayersTeam == 1)
        {
            nextPlayersTeam = 2;
        }
        else
        {
            nextPlayersTeam = 1;
        }
    }*/
}
