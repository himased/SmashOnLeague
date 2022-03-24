using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public enum ShieldState
{
    InHands,
    Flying,
    Returning,
    Ragdoll,
    Stuck
}

public class ShieldController : MonoBehaviour
{
    public ShieldState state = ShieldState.InHands;
    public Transform player, handHolder;
    public Text uiText;
    public Vector3 curveRotation, throwRotation;
    public float flyingSpeed = 30, chargeSpeed = 2, maxBounces = 10, constantYSpin = 3000, flyingXSpin = 250, pickUpRange = 3, destroyDistance = 50, fxDestroyTime = 0.5f, ragdollSpin = 1000;
    public bool curvedStart, curvedFlying, curvedReturn;

    MeshCollider col;
    Vector3 lastPos, returnPos;
    Rigidbody rb;
    Transform shieldModel;
    public Transform cam;
    GameObject impactFX, trailFX;
    bool mustReturn = false, firstBounce;
    float tempBounces = 1;
    int bounces = 1;

    PhotonView PV;

    private void Start()
    {
        col = GetComponent<MeshCollider>();
        //cam = Camera.main.transform;
        shieldModel = transform.Find("ShieldModel");
        rb = GetComponent<Rigidbody>();
        lastPos = transform.position;
        impactFX = transform.Find("ImpactFX").gameObject;
        trailFX = transform.Find("TrailFX").gameObject;

        PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!PV.IsMine)
            return;

        if (state.Equals(ShieldState.InHands))
        {
            if (Input.GetKey(KeyCode.Q))
            {
                tempBounces += chargeSpeed * Time.deltaTime;
                bounces = Mathf.RoundToInt(tempBounces);
                uiText.text = bounces.ToString();
                if (bounces >= maxBounces) { Throw(); }
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                Throw();
            }

            rb.velocity = Vector3.zero;
        }
        else if (state.Equals(ShieldState.Flying) || state.Equals(ShieldState.Returning))
        {
            if (!player) {Returned(); }

            //Movement
            transform.Translate(transform.forward * flyingSpeed * Time.deltaTime, Space.World);

            Rotation();
            shieldModel.RotateAround(shieldModel.position, shieldModel.up, constantYSpin * Time.deltaTime);

            if (Input.GetKeyUp(KeyCode.Q)) { mustReturn = true; }
        }
        else if (state.Equals(ShieldState.Ragdoll) || state.Equals(ShieldState.Stuck))
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist < pickUpRange) { Returned();}
        }

        //Check if too far or does not find player then return instantly or destroy shield
        if (player)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist > destroyDistance)
            {
                Returned();
            }
        }
        else 
        { 
            Returned(); 
        }
    }

    void Rotation()
    {
        if (curvedStart && !firstBounce)
        {
            transform.eulerAngles = transform.eulerAngles + (curveRotation * Time.deltaTime);
        }
        else if (curvedFlying && state.Equals(ShieldState.Flying))
        {
            transform.eulerAngles = transform.eulerAngles + (curveRotation * Time.deltaTime);
        }
        else if (curvedReturn && state.Equals(ShieldState.Returning))
        {
            transform.eulerAngles = transform.eulerAngles + (curveRotation * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(transform.position, transform.forward, flyingXSpin * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!state.Equals(ShieldState.InHands))
        {
            if (collision.transform.tag == "Sticky") { Stick(); }
            if (collision.transform.tag == "Player" && firstBounce) { Returned();}
        }

        if (state.Equals(ShieldState.Returning)) { Ragdoll(); }

        if (state.Equals(ShieldState.Flying) && bounces > 0) { Bounce(collision.GetContact(0).normal); }
    }

    void Bounce(Vector3 contactNormal)
    {
        firstBounce = true;
        bounces -= 1;
        uiText.text = bounces.ToString();

        Vector3 direction = contactNormal - lastPos.normalized;
        direction = direction.normalized;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);
        lastPos = transform.position;

        var fx = Instantiate(impactFX, transform.position, Quaternion.identity);
        fx.SetActive(true);
        Destroy(fx, fxDestroyTime);

        if (mustReturn)
        {
            state = ShieldState.Returning;

            bounces = 1;
            uiText.text = "Returning";
            returnPos = player.position;
            transform.LookAt(returnPos);
        }
        else if (bounces <= 0) { Ragdoll(); }
    }

    void Throw()
    {
        firstBounce = false;
        PV.RPC("ShieldNotInHand", RpcTarget.All);
        //transform.SetParent(null);
        transform.position = cam.position + cam.forward;
        transform.eulerAngles = cam.eulerAngles + throwRotation;
        trailFX.SetActive(true);
        col.enabled = true;
        rb.isKinematic = false;
        state = ShieldState.Flying;
    }

    void Stick()
    {
        state = ShieldState.Stuck;

        //Rigidbody
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;
        rb.isKinematic = true;

        mustReturn = false;
        trailFX.SetActive(false);
        col.enabled = false;

        uiText.text = "Stuck";
    }

    void Ragdoll()
    {
        if (state.Equals(ShieldState.InHands)) return;
        state = ShieldState.Ragdoll;

        //Rigidbody
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        Vector3 torque = new Vector3(Random.Range(-ragdollSpin, ragdollSpin), Random.Range(-ragdollSpin, ragdollSpin), Random.Range(-ragdollSpin, ragdollSpin));
        rb.AddTorque(torque);

        var fx = Instantiate(impactFX, transform.position, Quaternion.identity);
        fx.SetActive(true);
        Destroy(fx, fxDestroyTime);

        uiText.text = "Ragdoll";
        trailFX.SetActive(false);
    }

    void Returned()
    {
        if (!player) return;
        state = ShieldState.InHands;

        //Rigidbody
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.isKinematic = true;

        mustReturn = false;
        trailFX.SetActive(false);
        col.enabled = false;
        PV.RPC("ShieldInHand", RpcTarget.All);
        //transform.SetParent(handHolder);
        //transform.localPosition = Vector3.zero;
        //transform.localRotation = Quaternion.Euler(0, 0, 0);
        //PV.RPC("ShieldInHandPos", RpcTarget.All);
        transform.localScale = new Vector3(2f, 2f, 0.55f);
        bounces = 1;
        tempBounces = 1;
        uiText.text = "Ready";
    }

    [PunRPC]
    void ShieldInHand()
    {
        transform.SetParent(handHolder);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    [PunRPC]
    void ShieldNotInHand()
    {
        transform.SetParent(null);
    }
}
