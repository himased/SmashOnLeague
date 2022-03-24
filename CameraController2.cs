using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    public GameObject target;
    public float distance;
    Vector3 d;

    // Start is called before the first frame update
    void Start()
    {
        d = Vector3.forward * distance + Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        var p = target.transform.position;
        p.y = 0f;
        transform.position = p + d;
        transform.RotateAround(p, Vector3.up, 0.1f);
        d = transform.position - p;
    }
}
