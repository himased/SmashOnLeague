using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAreaHintCafe : MonoBehaviour
{
    public GameObject mapAreaHint;

    [SerializeField] GameObject CafeFloor1, CafeFloor2F, CafeFloor2R, CafeFloor2L;

    bool closeHint;
    // Start is called before the first frame update
    void Start()
    {
        CafeFloor1.SetActive(false);
        CafeFloor2F.SetActive(false);
        CafeFloor2R.SetActive(false);
        CafeFloor2L.SetActive(false);
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

        if (MapAreaRandomizeCafeteria.Instance.isFloor1R == true)
        {
            CafeFloor1.SetActive(true);
        }
        if (MapAreaRandomizeCafeteria.Instance.isFloor2F == true)
        {
            CafeFloor2F.SetActive(true);
        }
        if (MapAreaRandomizeCafeteria.Instance.isFloor2R == true)
        {
            CafeFloor2R.SetActive(true);
        }
        if (MapAreaRandomizeCafeteria.Instance.isFloor2L == true)
        {
            CafeFloor2L.SetActive(true);
        }
    }
}
