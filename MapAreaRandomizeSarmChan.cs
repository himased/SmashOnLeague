using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAreaRandomizeSarmChan : MonoBehaviour
{
    public static MapAreaRandomizeSarmChan Instance;
    public GameObject mapArea1, mapArea2, mapArea3, mapArea4, mapArea5, mapArea6;
    int randNum1, randNum2;

    public bool isFloor1R, isFloor1L, isFloor2R, isFloor2L, isFloor3R, isFloor3L;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        randNum1 = Random.Range(0, 5);
        randNum2 = Random.Range(0, 5);
        //randNum2 = Random.Range(0, 5);

        isFloor1R = false;
        isFloor1L = false;
        isFloor2R = false;
        isFloor2L = false;
        isFloor3R = false;
        isFloor3L = false;

        mapArea1.GetComponent<BoxCollider>().enabled = false;
        mapArea2.GetComponent<BoxCollider>().enabled = false;
        mapArea3.GetComponent<BoxCollider>().enabled = false;
        mapArea4.GetComponent<BoxCollider>().enabled = false;
        mapArea5.GetComponent<BoxCollider>().enabled = false;
        mapArea6.GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (randNum1 == randNum2)
        {
            randNum1 = Random.Range(0, 5);
        }
        /*if (randNum1 == randNum3 || randNum2 == randNum3)
        {
            randNum3 = Random.Range(0, 5);
        }*/

        //if (randNum1 != randNum2 && randNum1 != randNum3 && randNum2 != randNum3)
        if (randNum1 != randNum2)
            {
            if (GameSetup.enableFirstMapAreaTeamOne == true && PlayerManager.Instance.myTeam == 1)
            {
                RandomFirstMapArea();
            }
            if (GameSetup.enableFirstMapAreaTeamTwo == true && PlayerManager.Instance.myTeam == 2)
            {
                RandomFirstMapArea();
            }
        }

        /*if(PlayerController.Instance.currentHealth <= 0)
        {
            mapArea1.GetComponent<BoxCollider>().enabled = false;
            mapArea2.GetComponent<BoxCollider>().enabled = false;
            mapArea3.GetComponent<BoxCollider>().enabled = false;
            mapArea4.GetComponent<BoxCollider>().enabled = false;
            mapArea5.GetComponent<BoxCollider>().enabled = false;
            mapArea6.GetComponent<BoxCollider>().enabled = false;
        }*/
    }

    void RandomFirstMapArea()
    {
        if (PlayerController.Instance.currentHealth > 0)
        {
            if (randNum1 == 0)
            {
                mapArea1.GetComponent<BoxCollider>().enabled = true;
                isFloor1R = true;
            }
            if (randNum1 == 1)
            {
                mapArea2.GetComponent<BoxCollider>().enabled = true;
                isFloor1L = true;
            }
            if (randNum1 == 2)
            {
                mapArea3.GetComponent<BoxCollider>().enabled = true;
                isFloor2R = true;
            }
            if (randNum1 == 3)
            {
                mapArea4.GetComponent<BoxCollider>().enabled = true;
                isFloor2L = true;
            }
            if (randNum1 == 4)
            {
                mapArea5.GetComponent<BoxCollider>().enabled = true;
                isFloor3R = true;
            }
            if (randNum1 == 5)
            {
                mapArea6.GetComponent<BoxCollider>().enabled = true;
                isFloor3L = true;
            }
        }
        if (GameSetup.enableSecondMapAreaTeamOne == true && PlayerManager.Instance.myTeam == 1)
        {
            RandomSecondMapArea();
        }
        if (GameSetup.enableSecondMapAreaTeamTwo == true && PlayerManager.Instance.myTeam == 2)
        {
            RandomSecondMapArea();
        }
    }

    void RandomSecondMapArea()
    {
        if (PlayerController.Instance.currentHealth > 0)
        {
            if (randNum2 == 0)
            {
                mapArea1.GetComponent<BoxCollider>().enabled = true;
                isFloor1R = true;
            }
            if (randNum2 == 1)
            {
                mapArea2.GetComponent<BoxCollider>().enabled = true;
                isFloor1L = true;
            }
            if (randNum2 == 2)
            {
                mapArea3.GetComponent<BoxCollider>().enabled = true;
                isFloor2R = true;
            }
            if (randNum2 == 3)
            {
                mapArea4.GetComponent<BoxCollider>().enabled = true;
                isFloor2L = true;
            }
            if (randNum2 == 4)
            {
                mapArea5.GetComponent<BoxCollider>().enabled = true;
                isFloor3R = true;
            }
            if (randNum2 == 5)
            {
                mapArea6.GetComponent<BoxCollider>().enabled = true;
                isFloor3L = true;
            }
        }
        /*if (MapCaptureController.teamOneCompleted == true && PlayerManager.Instance.myTeam == 1)
        {
            EnableLastArea();
        }
        if (MapCaptureController.teamTwoCompleted == true && PlayerManager.Instance.myTeam == 2)
        {
            EnableLastArea();
        }*/
    }

    /*void EnableLastArea()
    {
        if (randNum3 == 0)
        {
            mapArea1.GetComponent<BoxCollider>().enabled = true;
            isFloor1R = true;
        }
        if (randNum3 == 1)
        {
            mapArea2.GetComponent<BoxCollider>().enabled = true;
            isFloor1L = true;
        }
        if (randNum3 == 2)
        {
            mapArea3.GetComponent<BoxCollider>().enabled = true;
            isFloor2R = true;
        }
        if (randNum3 == 3)
        {
            mapArea4.GetComponent<BoxCollider>().enabled = true;
            isFloor2L = true;
        }
        if (randNum3 == 4)
        {
            mapArea5.GetComponent<BoxCollider>().enabled = true;
            isFloor3R = true;
        }
        if (randNum3 == 5)
        {
            mapArea6.GetComponent<BoxCollider>().enabled = true;
            isFloor3L = true;
        }
    }*/
}
