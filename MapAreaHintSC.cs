using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAreaHintSC : MonoBehaviour
{
    public GameObject mapAreaHint;

    [SerializeField] GameObject SCFloor1R, SCFloor1L, SCFloor2R, SCFloor2L, SCFloor3R, SCFloor3L;

    bool closeHint;
    // Start is called before the first frame update
    void Start()
    {
        SCFloor1R.SetActive(false);
        SCFloor1L.SetActive(false);
        SCFloor2R.SetActive(false);
        SCFloor2L.SetActive(false);
        SCFloor3R.SetActive(false);
        SCFloor3L.SetActive(false);
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

        if (MapAreaRandomizeSarmChan.Instance.isFloor1R == true)
        {
            SCFloor1R.SetActive(true);
        }
        if (MapAreaRandomizeSarmChan.Instance.isFloor1L == true)
        {
            SCFloor1L.SetActive(true);
        }
        if (MapAreaRandomizeSarmChan.Instance.isFloor2R == true)
        {
            SCFloor2R.SetActive(true);
        }
        if (MapAreaRandomizeSarmChan.Instance.isFloor2L == true)
        {
            SCFloor2L.SetActive(true);
        }
        if (MapAreaRandomizeSarmChan.Instance.isFloor3R == true)
        {
            SCFloor3R.SetActive(true);
        }
        if (MapAreaRandomizeSarmChan.Instance.isFloor3L == true)
        {
            SCFloor3L.SetActive(true);
        }
    }
}
