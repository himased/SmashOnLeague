using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAreaRandomize : MonoBehaviour
{
    public static MapAreaRandomize Instance;
    public GameObject mapArea1, mapArea2, mapArea3, mapArea4, mapArea5, mapArea6;
    int randNum1, randNum2, randNum3;

    public bool isSarmChan, isDrawing, isLarnGuang;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        randNum1 = Random.Range(1, 4);
        randNum2 = Random.Range(1, 4);
        randNum2 = Random.Range(1, 4);

        isSarmChan = false;
        isLarnGuang = false;
        isDrawing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (randNum1 == randNum2)
        {
            randNum1 = Random.Range(1, 4);
        }
        if (randNum1 == randNum3 || randNum2 == randNum3)
        {
            randNum3 = Random.Range(1, 4);
        }

        Debug.Log("RandNum1: " + randNum1);
        Debug.Log("RandNum2: " + randNum2);
        Debug.Log("RandNum3: " + randNum3);


        if (randNum1 != randNum2 && randNum1 != randNum3 && randNum2 != randNum3)
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
    }

    void RandomFirstMapArea()
    {
        if (randNum1 == 1)
        {
            mapArea1.GetComponent<BoxCollider>().enabled = true;
            isSarmChan = true;
        }
        if (randNum1 == 2)
        {
            //mapArea2.SetActive(true);
            mapArea2.GetComponent<BoxCollider>().enabled = true;
            isSarmChan = true;
        }
        if (randNum1 == 3)
        {
            //mapArea3.SetActive(true);
            mapArea3.GetComponent<BoxCollider>().enabled = true;
            isLarnGuang = true;
        }
        if (randNum1 == 4)
        {
            //mapArea4.SetActive(true);
            mapArea4.GetComponent<BoxCollider>().enabled = true;
            isDrawing = true;
        }
        if (randNum1 == 5)
        {
            //mapArea5.SetActive(true);
            mapArea5.GetComponent<BoxCollider>().enabled = true;
            isDrawing = true;
        }
        if (randNum1 == 0)
        {
            //mapArea6.SetActive(true);
            mapArea6.GetComponent<BoxCollider>().enabled = true;
            isDrawing = true;
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
        if (randNum2 == 1)
        {
            mapArea1.GetComponent<BoxCollider>().enabled = true;
            isSarmChan = true;
        }
        if (randNum2 == 2)
        {
            //mapArea2.SetActive(true);
            mapArea2.GetComponent<BoxCollider>().enabled = true;
            isSarmChan = true;
        }
        if (randNum2 == 3)
        {
            //mapArea3.SetActive(true);
            mapArea3.GetComponent<BoxCollider>().enabled = true;
            isLarnGuang = true;
        }
        if (randNum2 == 4)
        {
            //mapArea4.SetActive(true);
            mapArea4.GetComponent<BoxCollider>().enabled = true;
            isDrawing = true;
        }
        if (randNum2 == 5)
        {
            //mapArea5.SetActive(true);
            mapArea5.GetComponent<BoxCollider>().enabled = true;
            isDrawing = true;
        }
        if (randNum2 == 0)
        {
            //mapArea6.SetActive(true);
            mapArea6.GetComponent<BoxCollider>().enabled = true;
            isDrawing = true;
        }
        if (MapCaptureController.teamOneCompleted == true && PlayerManager.Instance.myTeam == 1)
        {
            EnableLastArea();
        }
        if (MapCaptureController.teamTwoCompleted == true && PlayerManager.Instance.myTeam == 2)
        {
            EnableLastArea();
        }
    }

    void EnableLastArea()
    {
        if (randNum3 == 1)
        {
            mapArea1.GetComponent<BoxCollider>().enabled = true;
            isSarmChan = true;
        }
        if (randNum3 == 2)
        {
            //mapArea2.SetActive(true);
            mapArea2.GetComponent<BoxCollider>().enabled = true;
            isSarmChan = true;
        }
        if (randNum3 == 3)
        {
            //mapArea3.SetActive(true);
            mapArea3.GetComponent<BoxCollider>().enabled = true;
            isLarnGuang = true;
        }
        if (randNum3 == 4)
        {
            //mapArea4.SetActive(true);
            mapArea4.GetComponent<BoxCollider>().enabled = true;
            isDrawing = true;
        }
        if (randNum3 == 5)
        {
            //mapArea5.SetActive(true);
            mapArea5.GetComponent<BoxCollider>().enabled = true;
            isDrawing = true;
        }
        if (randNum3 == 0)
        {
            //mapArea6.SetActive(true);
            mapArea6.GetComponent<BoxCollider>().enabled = true;
            isDrawing = true;
        }
    }
}
