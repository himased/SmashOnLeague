using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
    }

    /*
    void Update()
    {
        var p = target.transform.position;
        transform.position = p + Vector3.forward
            * distance + Vector3.up;
    }
    */
    void Update()
    {
        var p = target.transform.position;
        transform.position = p + target.transform.forward
            * distance * -1 + Vector3.up;
        p.y = 1f;
        transform.LookAt(p);
    }
}
