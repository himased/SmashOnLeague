using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static PlayerManager Instance;

    public int myTeam;

    public PhotonView PV;

    GameObject controller;
    public GameObject auraTeamOne, auraTeamTwo;

    public bool isDead;
    public bool isTeamOne, isTeamTwo;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        /*if (PV.IsMine)
        {
            PV.RPC("RPC_GetTeam", RpcTarget.MasterClient);
        }*/

        if (controller == null)
        {
            myTeam = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
        }

        isDead = false;

            /*if (myTeam == 1)
            {
                isTeamOne = true;
            }
            if (myTeam == 2)
            {
                isTeamTwo = true;
            }*/
        }

    private void Update()
    {
        CreateController();

        if(myTeam == 1)
        {
            isTeamOne = true;
            isTeamTwo = false;
        }
        if(myTeam == 2)
        {
            isTeamTwo = true;
            isTeamOne = false;
        }
    }

    void CreateController()
    {
        Debug.Log("Instantiated Player Controller"); // Instantiate our player controller
        /*controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", Launcher.Instance.myCharacter), 
                                                            Vector3.zero, 
                                                            Quaternion.identity, 
                                                            0, 
                                                            new object[] { PV.ViewID });*/

        if (controller == null && myTeam != 0)
        {
            if (myTeam == 1 )
            {
                int spawnPicker = Random.Range(0, GameSetup.GS.spawnPointsTeamOne.Length);
                if (PV.IsMine)
                {
                    controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", Launcher.Instance.myCharacter),
                                                         GameSetup.GS.spawnPointsTeamOne[spawnPicker].position,
                                                         GameSetup.GS.spawnPointsTeamOne[spawnPicker].rotation,
                                                         0,
                                                         new object[] { PV.ViewID });
                    
                }
                //StartCoroutine(waitToSpawnTeamOne());
            }
            //if (myTeam == 2 )
            else
            {
                int spawnPicker = Random.Range(0, GameSetup.GS.spawnPointsTeamTwo.Length);
                if (PV.IsMine)
                {
                    controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", Launcher.Instance.myCharacter),
                                                         GameSetup.GS.spawnPointsTeamTwo[spawnPicker].position,
                                                         GameSetup.GS.spawnPointsTeamTwo[spawnPicker].rotation,
                                                         0,
                                                         new object[] { PV.ViewID });
                }
                //StartCoroutine(waitToSpawnTeamTwo());
            }
        }
    }

    /*IEnumerator waitToSpawnTeamOne()
    {
        yield return new WaitForSeconds(2);

        int spawnPicker = Random.Range(0, GameSetup.GS.spawnPointsTeamOne.Length);
        if (PV.IsMine)
        {
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", Launcher.Instance.myCharacter),
                                                 GameSetup.GS.spawnPointsTeamOne[spawnPicker].position,
                                                 GameSetup.GS.spawnPointsTeamOne[spawnPicker].rotation,
                                                 0,
                                                 new object[] { PV.ViewID });
        }
    }

    IEnumerator waitToSpawnTeamTwo()
    {
        yield return new WaitForSeconds(2.5f);

        int spawnPicker = Random.Range(0, GameSetup.GS.spawnPointsTeamTwo.Length);
        if (PV.IsMine)
        {
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", Launcher.Instance.myCharacter),
                                                 GameSetup.GS.spawnPointsTeamTwo[spawnPicker].position,
                                                 GameSetup.GS.spawnPointsTeamTwo[spawnPicker].rotation,
                                                 0,
                                                 new object[] { PV.ViewID });
        }
    }*/

    public void Die()
    {
        isDead = true;
        PhotonNetwork.Destroy(controller);
        CreateController();
    }

    /*[PunRPC]
    void RPC_GetTeam()
    {
        myTeam = GameSetup.GS.nextPlayersTeam;
        GameSetup.GS.UpdateTeam();
        PV.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);
    }

    [PunRPC]
    void RPC_SentTeam(int whichTeam)
    {
        myTeam = whichTeam;
    }*/
}
