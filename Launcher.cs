using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;
using System.IO;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    PhotonView PV;

    private int playersInRoom;
    private int maxPlayer;
    private float timeToStart;
    private float lessThanMaxPlayers;
    public float startingTime;
    public int countPlayerTeamOne, countPlayerTeamTwo, playerTeamOneReady, playerTeamTwoReady;

    int myTeam;

    //delay start
    private bool readyToCount;

    public string myCharacter;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] TMP_Text charRoomNameText;
    [SerializeField] TMP_Text startingTimeText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] Transform charPlayerListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject goInGameButton;
    [SerializeField] GameObject teamOneButton, teamTwoButton;
    [SerializeField] GameObject readyButton;
    [SerializeField] Text teamOneCount, teamTwoCount;
    [SerializeField] GameObject characterLists;
    [SerializeField] TMP_Text playerTeamOneReadyCount, playerTeamTwoReadyCount, mapNameText;
    [SerializeField] GameObject matchSettingButton;

    string mapName;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        maxPlayer = 6;
        readyToCount = false;
        timeToStart = startingTime;
        lessThanMaxPlayers = startingTime;

        PV = GetComponent<PhotonView>();

        Debug.Log("Connected to Master");
        PhotonNetwork.ConnectUsingSettings();

        mapName = "Drawing";
    }

    private void Update()
    {
        SelectCharacter();
        // PV.RPC("SelectTeam", RpcTarget.AllBuffered, myTeam);

        startingTimeText.text = timeToStart.ToString("0");
        if (readyToCount == true)
        {
            lessThanMaxPlayers -= Time.deltaTime;
            timeToStart = lessThanMaxPlayers;
        }
        if (timeToStart <= 0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                goInGameButton.SetActive(true);
            }
            startingTimeText.text = "";
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (playerTeamOneReady + playerTeamTwoReady >= playersInRoom)
            {
                goInGameButton.SetActive(true);
            }
            else
            {
                goInGameButton.SetActive(false);
            }
        }

        if (countPlayerTeamOne >= 3)
        {
            teamOneButton.SetActive(false);
        }
        if (countPlayerTeamTwo >= 3)
        {
            teamTwoButton.SetActive(false);
        }

        teamOneCount.text = countPlayerTeamOne.ToString();
        teamTwoCount.text = countPlayerTeamTwo.ToString();
        playerTeamOneReadyCount.text = "team one ready : " + playerTeamOneReady.ToString();
        playerTeamTwoReadyCount.text = "team two ready : " + playerTeamTwoReady.ToString();
        mapNameText.text = mapName;

        countPlayerForCapture();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");

    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)maxPlayer };
        MenuManager.Instance.OpenMenu("loading");
    }

    public void JoinTeam(int team)
    {
        //do we already have a team?
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            //we already have a team- so switch teams
            PhotonNetwork.LocalPlayer.CustomProperties["Team"] = team;
        }
        else
        {
            //we dont have a team yet- create the custom property and set it
            //0 for blue, 1 for red
            //set the player properties of this client to the team they clicked
            ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable
        {
            { "Team", team }
        };
            //set the property of Team to the value the user wants
            PhotonNetwork.SetPlayerCustomProperties(playerProps);
        }

        characterLists.SetActive(true);
        teamOneButton.SetActive(false);
        teamTwoButton.SetActive(false);
    }

    void countPlayerForCapture()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("PlayersInRoom"))
        {
            //we already have a team- so switch teams
            PhotonNetwork.LocalPlayer.CustomProperties["PlayersInRoom"] = playersInRoom;
        }
        else
        {
            //we dont have a team yet- create the custom property and set it
            //0 for blue, 1 for red
            //set the player properties of this client to the team they clicked
            ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable
        {
            { "PlayersInRoom", playersInRoom }
        };
            //set the property of Team to the value the user wants
            PhotonNetwork.SetPlayerCustomProperties(playerProps);
        }
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;
        playersInRoom = players.Length;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        if (playersInRoom == maxPlayer)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        matchSettingButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed :<" + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Join Failed :<" + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void OpenCharSelect()
    {
        if (PV.IsMine)
        {
            PV.RPC("StartGame", RpcTarget.AllBuffered);
        }
    }

    void SelectCharacter()
    {
        if((int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == 1)
        {
            if (PlayerInfo.PI.mySelectedCharacter == 0)
            {
                myCharacter = "JE/JEV1";
            }
            if (PlayerInfo.PI.mySelectedCharacter == 1)
            {
                myCharacter = "JChin/JChinV1";
            }
            if (PlayerInfo.PI.mySelectedCharacter == 2)
            {
                myCharacter = "JKarn/JkarnV1";
            }
            if (PlayerInfo.PI.mySelectedCharacter == 3)
            {
                myCharacter = "JMae/JMaeV1";
            }
            if (PlayerInfo.PI.mySelectedCharacter == 4)
            {
                myCharacter = "JFaii/JFaiiV1";
            }
        }
        if ((int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == 2)
        {
            if (PlayerInfo.PI.mySelectedCharacter == 0)
            {
                myCharacter = "JE/JEV2";
            }
            if (PlayerInfo.PI.mySelectedCharacter == 1)
            {
                myCharacter = "JChin/JChinV2";
            }
            if (PlayerInfo.PI.mySelectedCharacter == 2)
            {
                myCharacter = "JKarn/JkarnV2";
            }
            if (PlayerInfo.PI.mySelectedCharacter == 3)
            {
                myCharacter = "JMae/JMaeV2";
            }
            if (PlayerInfo.PI.mySelectedCharacter == 4)
            {
                myCharacter = "JFaii/JFaiiV2";
            }
        }
        /*if (PlayerInfo.PI.mySelectedCharacter == 0)
        {
            myCharacter = "JEController";
        }
        if (PlayerInfo.PI.mySelectedCharacter == 1)
        {
            myCharacter = "JChinController";
        }
        if (PlayerInfo.PI.mySelectedCharacter == 2)
        {
            myCharacter = "JKarnController";
        }
        if (PlayerInfo.PI.mySelectedCharacter == 3)
        {
            myCharacter = "JMaeController";
        }
        if (PlayerInfo.PI.mySelectedCharacter == 4)
        {
            myCharacter = "JFaiiController";
        }*/
        if (PlayerInfo.PI.mySelectedCharacter == 5)
        {
            myCharacter = "TestChar";
        }
    }

    [PunRPC]
    void StartGame()
    {
        readyToCount = true;

        MenuManager.Instance.OpenMenu("char select");
        PhotonNetwork.CurrentRoom.IsOpen = false;
        charRoomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;
        playersInRoom = players.Length;

        foreach (Transform child in charPlayerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab, charPlayerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
    }

    public void GoInGame()
    {
        /*if (PV.IsMine)
        {
            Debug.Log("Instantiated Player Controller");
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", myCharacter), Vector3.zero, Quaternion.identity);
        }*/

        if(mapName == "Drawing")
        {
            PhotonNetwork.LoadLevel(2);
        }
        
        if(mapName == "SarmChan")
        {
            PhotonNetwork.LoadLevel(3);
        }

        if (mapName == "Cafeteria")
        {
            PhotonNetwork.LoadLevel(4);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    [PunRPC]
    void CountPlayerTeamOne()
    {
        countPlayerTeamOne++;
    }

    [PunRPC]
    void CountPlayerTeamTwo()
    {
        countPlayerTeamTwo++;
    }

    [PunRPC]
    void CountPlayerTeamOneReady()
    {
        playerTeamOneReady++;
    }

    [PunRPC]
    void CountPlayerTeamOneUnready()
    {
        playerTeamOneReady--;
    }

    [PunRPC]
    void CountPlayerTeamTwoReady()
    {
        playerTeamTwoReady++;
    }

    [PunRPC]
    void CountPlayerTeamTwoUnready()
    {
        playerTeamTwoReady--;
    }

    public void SelectDrawingMap()
    {
        mapName = "Drawing";
    }

    public void SelectSarmChanMap()
    {
        mapName = "SarmChan";
    }

    public void SelectCafeteriaMap()
    {
        mapName = "Cafeteria";
    }

    public void TeamOneCountUp()
    {
        PV.RPC("CountPlayerTeamOne", RpcTarget.All);
        myTeam = 1;
    }

    public void TeamTwoCountUp()
    {
        PV.RPC("CountPlayerTeamTwo", RpcTarget.All);
        myTeam = 2;
    }

    public void ReadyCountUp()
    {
        if(myTeam == 1)
        {
            PV.RPC("CountPlayerTeamOneReady", RpcTarget.All);
        }
        if (myTeam == 2)
        {
            PV.RPC("CountPlayerTeamTwoReady", RpcTarget.All);
        }
        readyButton.SetActive(false);
    }

    public void UnreadyCountUp()
    {
        if (myTeam == 1)
        {
            PV.RPC("CountPlayerTeamOneUnready", RpcTarget.All);
        }
        if (myTeam == 2)
        {
            PV.RPC("CountPlayerTeamTwoUnready", RpcTarget.All);
        }
        readyButton.SetActive(true);
    }

    public void OnClickSound()
    {
        MainMenuSoundManager.Instance.Audio.PlayOneShot(MainMenuSoundManager.Instance.Click);
    }

    public void OnSelectCharSound()
    {
        MainMenuSoundManager.Instance.Audio.PlayOneShot(MainMenuSoundManager.Instance.SelectChar);
    }
}
