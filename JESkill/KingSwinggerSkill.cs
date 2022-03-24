using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class KingSwinggerSkill : MonoBehaviour
{
    private const float NORMAL_FOV = 60F;
    private const float GRAPSHOT_FOV = 90F;

    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    public Camera fpsCam;
    private CameraFov cameraFov;
    private float maxDistance = 30f;
    private SpringJoint joint;
    float swingDuration = 0, swingMaxDuration = 2f;

    [SerializeField] int Uses;
    [SerializeField] float cooldown;
    [SerializeField] Text usesText, coolDownText;
    [SerializeField] Image ksCDImage;

    int maxUses;
    float cooldownTimer;

    bool canSwing = false;

    PhotonView PV;

    public GameObject playerController;
    Animator anim;

    public SoundManager SM;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        lr = GetComponent<LineRenderer>();
        cameraFov = fpsCam.GetComponent<CameraFov>();
    }

    private void Start()
    {
        maxUses = Uses;
        cooldownTimer = cooldown;
        usesText.text = Uses.ToString();
        coolDownText.text = cooldown.ToString("0");
        swingDuration = swingMaxDuration;

        anim = playerController.GetComponent<Animator>();
    }

    void Update()
    {
        if (!PV.IsMine)
            return;

        //PV.RPC("DrawRope", RpcTarget.All);
        ksCDImage.fillAmount = cooldownTimer / cooldown;

        if (Uses > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable))
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    canSwing = true;
                }
            }
            if(Input.GetKeyUp(KeyCode.Q))
            {
                canSwing = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && canSwing == true)
        {
            anim.SetBool("IsSwinging", true);
            
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            anim.SetBool("IsSwinging", false);
        }

        if(PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.jumpState)
        {
            if (Input.GetKeyDown(KeyCode.Q) && canSwing == true)
            {
                anim.SetBool("IsSwinging", true);
                anim.SetBool("Jump", false);
            }
        }

        if (PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.idleJumpState)
        {
            if (Input.GetKeyDown(KeyCode.Q) && canSwing == true)
            {
                anim.SetBool("IsSwinging", true);
                anim.SetBool("Jump", false);
            }
        }

        if (PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.jumpBackState)
        {
            if (Input.GetKeyDown(KeyCode.Q) && canSwing == true)
            {
                anim.SetBool("IsSwinging", true);
                anim.SetBool("Jump", false);
            }
        }

        if (PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.locoState)
        {
            if (Input.GetKeyDown(KeyCode.Q) && canSwing == true)
            {
                anim.SetBool("IsSwinging", true);
                //PlayerController.Instance.anim.SetBool("InAir", false);
            }
        }

        if (PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.walkBackState)
        {
            if (Input.GetKeyDown(KeyCode.Q) && canSwing == true)
            {
                anim.SetBool("IsSwinging", true);
                //PlayerController.Instance.anim.SetBool("InAir", false);
            }
        }

        if (PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.locoNoBallState)
        {
            if (Input.GetKeyDown(KeyCode.Q) && canSwing == true)
            {
                anim.SetBool("IsSwinging", true);
                anim.SetBool("InAir", false);
            }
        }

        if (PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.airTimeState || PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.airTimeBackState)
        {
            if (Input.GetKeyDown(KeyCode.Q) && canSwing == true)
            {
                anim.SetBool("IsSwinging", true);
            }
        }

        if (PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.rollBackState || PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.rollState)
        {
            if (Input.GetKeyDown(KeyCode.Q) && canSwing == true)
            {
                anim.SetBool("IsSwinging", true);
            }
        }

        Debug.Log("SwingDuration: " + swingDuration);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartGrapple();
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            StopGrapple();
        }
        else if(swingDuration <= 0)
        {
            StopGrapple();
            anim.SetBool("IsSwinging", false);
            swingDuration = swingMaxDuration;
        }

        if(canSwing == true)
        {
            swingDuration -= Time.deltaTime;
        }

        if (Uses < maxUses)
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }
            else
            {
                Uses += 1;
                cooldownTimer = cooldown;
                usesText.text = Uses.ToString();
            }
        }

        if (PlayerController.Instance.currentHealth <= 0)
        {
            Uses = 0;
        }

        coolDownText.text = cooldownTimer.ToString("0");
    }

    void LateUpdate()
    {
        //PV.RPC("DrawRope", RpcTarget.All);
        DrawRope();
    }


    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple()
    {
        if (Uses > 0)
        {
            swingDuration = swingMaxDuration;

            RaycastHit hit;
            if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable))
            {
                cameraFov.SetCameraFov(GRAPSHOT_FOV);
                grapplePoint = hit.point;
                joint = player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = grapplePoint;

                float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

                //The distance grapple will try to keep from grapple point. 
                joint.maxDistance = distanceFromPoint * 0.7f;
                joint.minDistance = distanceFromPoint * 0.25f;

                //Adjust these values to fit your game.
                joint.spring = 7f;
                joint.damper = 2f;
                joint.massScale = 7f;

                lr.positionCount = 2;
                currentGrapplePosition = gunTip.position;

                SM.PlaySound("skill_1");
                SM.PlaySound("skill_2");
            }
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple()
    {
        canSwing = false;
        cameraFov.SetCameraFov(NORMAL_FOV);
        lr.positionCount = 0;
        Destroy(joint);
        if(Uses > 0)
        {
            Uses -= 1;
            usesText.text = Uses.ToString();
        }
    }

    private Vector3 currentGrapplePosition;

    [PunRPC]
    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
        PV.RPC("DrawLine", RpcTarget.All);
    }

    [PunRPC]
    void DrawLine()
    {
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
