using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class BallController : MonoBehaviour, IPunObservable
{
    public static BallController Instance;
    //[SerializeField] TMP_Text coolDownText;
    [SerializeField] Image forcebarImage, ballRecallImage;
    [SerializeField] Text ballDamageText;

    private const float NORMAL_FOV = 60F;
    private const float THROWSHOT_FOV = 45F;
    //[SerializeField] BallController ballScript;
    Rigidbody rb;
    public PhotonView PV;

    SphereCollider coll;
    [SerializeField] Transform player, gunContainer, fpsCam, fpsCamAim;
    private CameraFov cameraFov;
    

    [SerializeField] float pickUpRange;
    [SerializeField] float dropForwardForce, dropUpwardForce;

    [SerializeField] bool equipped;
    public static bool slotFull;

    private float holdDownStartTime, maxForce, _callBackCoolDown;
    [SerializeField] float callBackCoolDown;
    public float getCoolDown;

    bool isShooting;

    float force;
    int damageTimer, damage;

    bool isAlive = true;
    Vector3 position;
    Quaternion rotation;
    float lerpSmoothing = 5f;

    public PhotonView playerPV;
    public string ballOwnerName, dyingMan;

    public SoundManager SM;

    string myName, killerName = "";

    private void Awake()
    {
        Instance = this;

        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            myName = PV.Owner.NickName;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<SphereCollider>();
        
        _callBackCoolDown = 3f;

        cameraFov = fpsCam.GetComponent<CameraFov>();

        maxForce = 100f;
        damage = 35;

        if (!PV.IsMine)
        {
            Destroy(rb);
        }
        //Setup
        if (!equipped)
        {
            //ballScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped)
        {
            //ballScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }        
    }

    private void Update()
    {
        RaycastHit hit;

        if (!PV.IsMine)
            return;

        ballDamageText.text = damage.ToString();

        ballOwnerName = playerPV.Owner.NickName;
        //PV.RPC("GetOwnerName", RpcTarget.All);

        //Check if player is in range and "E" is pressed
        //Vector3 distanceToPlayer = player.position - transform.position;
        //if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.F) && !slotFull) 
        if (fpsCam == null)
        {
            PhotonNetwork.Destroy(gameObject);
        }

        /*if (!equipped && Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, pickUpRange) && Input.GetKeyDown(KeyCode.F) && !slotFull)
        {
            PickUp();
        }

        //Drop if equipped and "Q" is pressed
        if (equipped && Input.GetKeyDown(KeyCode.G)) 
        {
            Drop();
        }*/

        if (!equipped && PlayerController.Instance.currentHealth <= 0)
        {
            PickUp();
        }

        if (!equipped)
        {
            ballRecallImage.fillAmount = callBackCoolDown / 3;
        }

        /*if (!equipped && Input.GetKeyDown(KeyCode.C) && callBackCoolDown <=0)
        {
            PickUp();
            callBackCoolDown = _callBackCoolDown;
        }*/

        if (!equipped && callBackCoolDown <= 0)
        {
            PickUp();
           // callBackCoolDown = _callBackCoolDown;
        }

        if (!equipped && Input.GetMouseButtonDown(0))
        {
            maxForce = 0;
        }

        if((!equipped && PlayerManager.Instance.isDead == true) || (!equipped && PlayerController.Instance.currentHealth <= 0))
        {
            PhotonNetwork.Destroy(gameObject);
        }

        if (equipped)
        {
            //PickUp();
            ballRecallImage.fillAmount = 1;
            callBackCoolDown = _callBackCoolDown;
            PV.RPC("BallInHand", RpcTarget.All);
        }

        getCoolDown = callBackCoolDown;
        callBackCoolDown -= Time.deltaTime;

        Shoot();

        //coolDownText.text = callBackCoolDown.ToString("0");
        
        //ballRecallImage.fillAmount = callBackCoolDown / 3;

        if (callBackCoolDown <= 0)
        {
            callBackCoolDown = 0;
            //coolDownText.text = "Ready";
        }

        if(isShooting == true)
        {
            forcebarImage.fillAmount += 1 * Time.deltaTime / 2;
        }

        CalculateDamage();
        Debug.Log("Damage per times: " + damage);
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        //Make weapon a child of the camera and move it to default position
        PV.RPC("RPC_PickUp", RpcTarget.All);
        //transform.SetParent(gunContainer);
        //transform.localPosition = Vector3.zero;
        //transform.localRotation = Quaternion.Euler(Vector3.zero);
        PV.RPC("BallInHand", RpcTarget.All);
        transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

        //Make Rigidbody kinematic and BoxCollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        SM.PlaySound("ballBack");

        //Enable script
        //ballScript.enabled = true;
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        //Set parent to null
        //transform.SetParent(null);
        PV.RPC("RPC_Drop", RpcTarget.All);

        //Make Rigidbody not kinematic and BoxCollider normal
        rb.isKinematic = false;
        coll.isTrigger = false;

        //Gun carries momentum of player
        //rb.velocity = player.GetComponent<Rigidbody>().velocity;

        //AddForce
        rb.AddForce(fpsCamAim.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCamAim.up * dropUpwardForce, ForceMode.Impulse);
        //Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        //Disable script
        //ballScript.enabled = false;
    }

    void AddForce(float _force)
    {
        Drop();
        //Vector3 force = new Vector3(0f, 0f, _force);
        rb.AddForce(fpsCamAim.forward * _force, ForceMode.Impulse);
        rb.AddForce(fpsCamAim.up * _force/100, ForceMode.Impulse);
    }

    void Shoot()
    {
        if (equipped && Input.GetMouseButtonDown(0))
        {
            maxForce = 60f;
            holdDownStartTime = Time.time;
            isShooting = true;
            cameraFov.SetCameraFov(THROWSHOT_FOV);
        }
        if (maxForce != 0)
        {
            if (equipped && Input.GetMouseButtonUp(0))
            {
                isShooting = false;
                forcebarImage.fillAmount = 0;
                float holdDownTime = Time.time - holdDownStartTime;
                AddForce(CalculateHoldDownForce(holdDownTime));
                cameraFov.SetCameraFov(NORMAL_FOV);
                SM.PlaySound("throw");
            }
        }
    }

    private float CalculateHoldDownForce(float holdTime)
    {
        float maxForceHoldDown = 2f;
        float holdTimeNormalized = Mathf.Clamp01(holdTime / maxForceHoldDown);
        force = holdTimeNormalized * maxForce;
        return force;
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine)
        {
            return;
        }
            
    }

    void CalculateDamage()
    {
        if (MapCaptureController.Instance.allCaptured == 1)
        {
            damage = 65;
        }
        if (MapCaptureController.Instance.teamOneCaptured >= MapCaptureController.Instance.captureObject && PlayerManager.Instance.myTeam == 1)
        {
            damage = 100;
        }
        if (MapCaptureController.Instance.teamTwoCaptured >= MapCaptureController.Instance.captureObject && PlayerManager.Instance.myTeam == 2)
        {
            damage = 100;
        }
        /*if (MapCaptureController.Instance.allCaptured == 3)
        {
            damage = 115;
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage);
        if(collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<PhotonView>().IsMine == false)
        {
            //dyingMan = collision.gameObject.GetComponent<PhotonView>().Owner.NickName;
            killerName = collision.gameObject.GetComponent<PhotonView>().Owner.NickName;
            /*if (myName != killerName)
            {
                Debug.Log("Hello world!");
                SM.PlaySound("hitEnemy");
            }*/
            SM.PlaySound("hitEnemy");
            //SM.PlaySound("hit");
        }
        
        Debug.Log("Who: " + dyingMan);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameObject.transform.position);
            stream.SendNext(gameObject.transform.rotation);
        }
        else
        {
            position = (Vector3)stream.ReceiveNext();
            rotation = (Quaternion)stream.ReceiveNext();
        }
    }

    [PunRPC]
    void RPC_Drop()
    {
        transform.SetParent(null);
    }

    [PunRPC]
    void RPC_PickUp()
    {
        transform.SetParent(gunContainer);
    }

    [PunRPC]
    void BallInHand()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    [PunRPC]
    void GetOwnerName()
    {
        ballOwnerName = playerPV.Owner.NickName;
    }
}
