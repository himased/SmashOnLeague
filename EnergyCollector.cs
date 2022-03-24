using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnergyCollector : MonoBehaviour
{
    /*public Transform equipPosition;
    public float distance = 10f;
    GameObject currentEnergy;
    GameObject en;

    public Camera fpsCam;

    int countEnergy;

    bool canGet;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckEnergy();

        if (canGet)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (currentEnergy != null)
                    Drop();

                PickUp();
            }
        }

        if(currentEnergy != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                Drop();
        }

        if(PlayerController.Instance.currentHealth <= 0)
        {
            Drop();
        }
    }

    void CheckEnergy()
    {
        RaycastHit hit;

        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, distance))
        {
            if(hit.transform.tag == "RedEnergy" && PlayerManager.Instance.myTeam == 1)
            {
                canGet = true;
                en = hit.transform.gameObject;
            }
        }
        else
        {
            canGet = false;
        }
    }

    void PickUp()
    {
        currentEnergy = en;
        currentEnergy.transform.position = equipPosition.position;
        currentEnergy.transform.parent = equipPosition;
        currentEnergy.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        currentEnergy.GetComponent<Rigidbody>().isKinematic = true;
        currentEnergy.GetComponent<MeshCollider>().enabled = false;
        currentEnergy.GetComponent<MeshRenderer>().enabled = false;
    }
    
    void Drop()
    {
        currentEnergy.transform.parent = null;
        currentEnergy.GetComponent<Rigidbody>().isKinematic = false;
        currentEnergy.GetComponent<MeshCollider>().enabled = true;
        currentEnergy.GetComponent<MeshRenderer>().enabled = true;
        currentEnergy = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "RedEnergy" && PlayerManager.Instance.myTeam == 1)
        {
            en = collision.transform.gameObject;
            en.transform.position = equipPosition.position + new Vector3(0f, 0f, countEnergy);
            en.transform.parent = equipPosition;
            countEnergy += 1;
            en.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            en.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (collision.gameObject.tag == "BlueEnergy" && PlayerManager.Instance.myTeam == 2)
        {
            en = collision.transform.gameObject;
            en.transform.position = equipPosition.position + new Vector3(0f, 0f, countEnergy);
            en.transform.parent = equipPosition;
            countEnergy += 1;
            en.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            en.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            canGet = false;
        }
    }*/

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    Rigidbody rb;
    PhotonView PV;

    bool isPicked;

    MeshCollider coll;
    [SerializeField] Transform player, fpsCam, energyContainer;


    [SerializeField] float pickUpRange;
    [SerializeField] float dropForwardForce, dropUpwardForce;

    [SerializeField] bool equipped;
    public static bool slotFull;

    GameObject eng;

    private void Start()
    {
        isPicked = true;
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<MeshCollider>();
        PV = gameObject.GetComponent<PhotonView>();

        if (!PV.IsMine)
        {
            Destroy(rb);
        }
        //Setup
        /*if (!equipped)
        {
            //ballScript.enabled = false;
            eng.GetComponent<Rigidbody>().isKinematic = false;
            eng.GetComponent<MeshCollider>().isTrigger = false;
        }
        if (equipped)
        {
            //ballScript.enabled = true;
            eng.GetComponent<Rigidbody>().isKinematic = true;
            eng.GetComponent<MeshCollider>().isTrigger = true;
            slotFull = true;
        }*/
    }

    private void Update()
    {
        RaycastHit hit;

        if (!PV.IsMine)
            return;

        //Check if player is in range and "E" is pressed
        //Vector3 distanceToPlayer = player.position - transform.position;
        //if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.F) && !slotFull) 
        if (fpsCam == null)
        {
            Drop();
        }

        if (!equipped && Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, pickUpRange) && Input.GetKeyDown(KeyCode.F) && !slotFull )
        {
            eng = hit.transform.gameObject;
            if(hit.transform.gameObject.tag == "RedEnergy" && PlayerManager.Instance.myTeam == 1)
            {
                PickUp();
            }
            if (hit.transform.gameObject.tag == "BlueEnergy" && PlayerManager.Instance.myTeam == 2)
            {
                PickUp();
            }

        }

        //Drop if equipped and "Q" is pressed
        if (equipped && Input.GetKeyDown(KeyCode.Alpha1))
        {
            Drop();
        }

        if (equipped && PlayerController.Instance.currentHealth <= 0)
        {
            Drop();
        }
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        //Make weapon a child of the camera and move it to default position
        eng.transform.SetParent(energyContainer);
        eng.transform.localPosition = Vector3.zero;
        eng.transform.localRotation = Quaternion.Euler(Vector3.zero);
        eng.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

        //Make Rigidbody kinematic and BoxCollider a trigger
        eng.GetComponent<Rigidbody>().isKinematic = true;
        eng.GetComponent<MeshCollider>().isTrigger = true;

        //test
        eng.GetComponent<MeshRenderer>().enabled = false;

        //Enable script
        //ballScript.enabled = true;
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        //Set parent to null
        eng.transform.SetParent(null);

        //Make Rigidbody not kinematic and BoxCollider normal
        eng.GetComponent<Rigidbody>().isKinematic = false;
        eng.GetComponent<MeshCollider>().isTrigger = false;

        eng.GetComponent<MeshRenderer>().enabled = true;

        //Gun carries momentum of player
        eng.GetComponent<Rigidbody>().velocity = player.GetComponent<Rigidbody>().velocity;

        //AddForce
        eng.GetComponent<Rigidbody>().AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        eng.GetComponent<Rigidbody>().AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        //Add random rotation
        float random = Random.Range(-1f, 1f);
        eng.GetComponent<Rigidbody>().AddTorque(new Vector3(random, random, random) * 10);

        //Disable script
        //ballScript.enabled = false;
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;
    }
}
