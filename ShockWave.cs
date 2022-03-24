using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShockWave : MonoBehaviour
{
    /*[SerializeField] float movementSpeed;
    [SerializeField] string[] enemyTagsToCheck;

    float duration, distanceDuration;

    GameObject enemy;
    bool isStun;
    GameObject hitFX, impactFX;

    Vector3 scaleUp;

    // Start is called before the first frame update
    void Start()
    {
        //hitFX = transform.Find("HitFX").gameObject;
        //impactFX = transform.Find("ImpactFX").gameObject;
        duration = 3f;
        //distanceDuration = 3f;
        //transform.Rotate(90f, 0f, 0f);

        //scaleUp = new Vector3(0.1f, 0f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);

        //transform.localScale += scaleUp;

        //Debug.Log("Duration: " + duration);

        //distanceDuration -= Time.deltaTime;

        //if (distanceDuration <= 0)
        //{
        //Destroy(gameObject);
        //}

        if (isStun == true)
        {
            enemy.GetComponent<PlayerController>().enabled = false;
            duration -= Time.deltaTime;
            //distanceDuration = 1.5f;
           
            if (duration <= 0)
            {
                Destroy(gameObject);
                enemy.GetComponent<PlayerController>().enabled = true;
                duration = 3;
                isStun = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enemyTagsToCheck.Contains(other.gameObject.tag))
        {
            isStun = true;
            enemy = other.gameObject;
            //hitFX.SetActive(true);
            //impactFX.SetActive(true);
        }
    }*/

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    public float health, duration;

    //private SkinnedMeshRenderer rend;
    //private MeshCollider col;
    bool isStun = false;

    Transform enemy;

    // Start is called before the first frame update
    void Start()
    {
        //rend = GetComponent<SkinnedMeshRenderer>();
        //col = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isRaised)
        //{
        //blendAmount += raiseSpeed * Time.deltaTime;
        //rend.SetBlendShapeWeight(0, blendAmount);
        //Mesh bakeMesh = new Mesh();
        //rend.BakeMesh(bakeMesh);
        //col.sharedMesh = bakeMesh;
        //if (blendAmount >= 100) { isRaised = true; }
        //}

        if (health <= 0 && enemy != null)
        {
            Destroy(gameObject);
            enemy.GetComponent<PlayerController>().enabled = true;
            isStun = false;
            StuntController.Instance.isStunt = false;
            /*Component[] fractures = GetComponentsInChildren(typeof(Rigidbody), true);
            foreach (Rigidbody child in fractures)
            {
                child.transform.SetParent(null);
                child.gameObject.SetActive(true);
                Destroy(child.gameObject, destroyDelay);
                //PhotonNetwork.Destroy(child.gameObject);

                var forceDir = child.position - transform.position;
                if (child != transform)
                {
                    Vector3 randomTorque;
                    randomTorque.x = Random.Range(-destroyRotForce, destroyRotForce);
                    randomTorque.y = Random.Range(-destroyRotForce, destroyRotForce);
                    randomTorque.z = Random.Range(-destroyRotForce, destroyRotForce);

                    child.AddTorque(randomTorque);
                    child.AddForce(forceDir.normalized * destroyPushForce, ForceMode.VelocityChange);
                }
                if (child == fractures.Last()) { Destroy(gameObject); }
            }*/
        }

        if (duration <= 0) { health = 0; } else { duration -= Time.deltaTime; }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemy = other.transform;
            enemy.gameObject.GetComponent<PlayerController>().enabled = false;
            isStun = true;
            StuntController.Instance.isStunt = true;
        }
    }
}
