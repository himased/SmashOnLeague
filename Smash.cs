using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash : MonoBehaviour
{
    public float health, duration;

    bool isStun = false;

    Transform enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isStun == true)
        {
            enemy.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 30f);
            isStun = false;
            StuntController.Instance.isStunt = false;
            Destroy(gameObject, 5);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemy = other.transform;
            //enemy.gameObject.GetComponent<PlayerController>().enabled = false;
            isStun = true;
            StuntController.Instance.isStunt = true;
        }
    }
}
