using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAreaHintDrawing : MonoBehaviour
{
    public GameObject mapAreaHint;
    public GameObject drawingFloor1, drawingFloor2, drawingFloor3, drawingFloor4;
    bool closeHint;

    // Start is called before the first frame update
    void Start()
    {
        drawingFloor1.SetActive(false);
        drawingFloor2.SetActive(false);
        drawingFloor3.SetActive(false);
        drawingFloor4.SetActive(false);
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

        if (MapAreaRandomizeDrawing.Instance.isFloorDr1 == true)
        {
            drawingFloor1.SetActive(true);
        }
        if (MapAreaRandomizeDrawing.Instance.isFloorDr2 == true)
        {
            drawingFloor2.SetActive(true);
        }
        if (MapAreaRandomizeDrawing.Instance.isFloorDr3 == true)
        {
            drawingFloor3.SetActive(true);
        }
        if (MapAreaRandomizeDrawing.Instance.isFloorDr4 == true)
        {
            drawingFloor4.SetActive(true);
        }
    }
}
