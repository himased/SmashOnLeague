using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAreaHint : MonoBehaviour
{
    public GameObject mapAreaHint;
    public GameObject sarmChan, drawingBuilding, larnGuang;

    bool closeHint;
    // Start is called before the first frame update
    void Start()
    {
        mapAreaHint.SetActive(false);
        sarmChan.SetActive(false);
        drawingBuilding.SetActive(false);
        larnGuang.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            closeHint = true;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            closeHint = false;
        }

        if (closeHint == true)
        {
            mapAreaHint.SetActive(true);
        }
        if (closeHint == false)
        {
            mapAreaHint.SetActive(false);
        }

        if(MapAreaRandomize.Instance.isDrawing == true)
        {
            drawingBuilding.SetActive(true);
        }
        if (MapAreaRandomize.Instance.isLarnGuang == true)
        {
            larnGuang.SetActive(true);
        }
        if (MapAreaRandomize.Instance.isSarmChan == true)
        {
            sarmChan.SetActive(true);
        }
    }
}
