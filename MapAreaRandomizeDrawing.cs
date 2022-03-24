using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAreaRandomizeDrawing : MonoBehaviour
{
    public static MapAreaRandomizeDrawing Instance;
    public GameObject mapAreaDr1, mapAreaDr2, mapAreaDr3, mapAreaDr4;
    int randNumDr1, randNumDr2;

    public bool isFloorDr1, isFloorDr2, isFloorDr3, isFloorDr4;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        randNumDr1 = Random.Range(0, 3);
        randNumDr2 = Random.Range(0, 3);
        //randNumDr2 = Random.Range(0, 3);

        isFloorDr1 = false;
        isFloorDr2 = false;
        isFloorDr3 = false;
        isFloorDr4 = false;

        mapAreaDr1.GetComponent<BoxCollider>().enabled = false;
        mapAreaDr2.GetComponent<BoxCollider>().enabled = false;
        mapAreaDr3.GetComponent<BoxCollider>().enabled = false;
        mapAreaDr4.GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (randNumDr1 == randNumDr2)
        {
            randNumDr1 = Random.Range(0, 3);
        }
        /*if (randNumDr1 == randNumDr3 || randNumDr2 == randNumDr3)
        {
            randNumDr3 = Random.Range(0, 3);
        }*/


        //if (randNumDr1 != randNumDr2 && randNumDr1 != randNumDr3 && randNumDr2 != randNumDr3)
        if (randNumDr1 != randNumDr2)
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
        if (randNumDr1 == 0)
        {
            mapAreaDr1.GetComponent<BoxCollider>().enabled = true;
            isFloorDr1 = true;
        }
        if (randNumDr1 == 1)
        {
            //mapArea2.SetActive(true);
            mapAreaDr2.GetComponent<BoxCollider>().enabled = true;
            isFloorDr2 = true;
        }
        if (randNumDr1 == 2)
        {
            //mapArea3.SetActive(true);
            mapAreaDr3.GetComponent<BoxCollider>().enabled = true;
            isFloorDr3 = true;
        }
        if (randNumDr1 == 3)
        {
            //mapArea4.SetActive(true);
            mapAreaDr4.GetComponent<BoxCollider>().enabled = true;
            isFloorDr4 = true;
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
        if (randNumDr2 == 0)
        {
            mapAreaDr1.GetComponent<BoxCollider>().enabled = true;
            isFloorDr1 = true;
        }
        if (randNumDr2 == 1)
        {
            mapAreaDr2.GetComponent<BoxCollider>().enabled = true;
            isFloorDr2 = true;
        }
        if (randNumDr2 == 2)
        {
            mapAreaDr3.GetComponent<BoxCollider>().enabled = true;
            isFloorDr3 = true;
        }
        if (randNumDr2 == 3)
        {
            mapAreaDr4.GetComponent<BoxCollider>().enabled = true;
            isFloorDr4 = true;
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
        if (randNumDr3 == 0)
        {
            mapAreaDr1.GetComponent<BoxCollider>().enabled = true;
            isFloorDr1 = true;
        }
        if (randNumDr3 == 1)
        {
            mapAreaDr2.GetComponent<BoxCollider>().enabled = true;
            isFloorDr2 = true;
        }
        if (randNumDr3 == 2)
        {
            mapAreaDr3.GetComponent<BoxCollider>().enabled = true;
            isFloorDr3 = true;
        }
        if (randNumDr3 == 3)
        {
            mapAreaDr4.GetComponent<BoxCollider>().enabled = true;
            isFloorDr4 = true;
        }
    }*/

    
}
