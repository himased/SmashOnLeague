using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class KingSwinggerSkill1 : MonoBehaviour
{
    private const float NORMAL_FOV = 60F;
    private const float GRAPSHOT_FOV = 80F;

    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    public Camera fpsCam;
    private CameraFov cameraFov;
    private float maxDistance = 30f;
    private SpringJoint joint;

    [SerializeField] int Uses;
    [SerializeField] float cooldown;
    [SerializeField] Text usesText, coolDownText;

    int maxUses;
    float cooldownTimer;

    PhotonView PV;

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
    }

    void Update()
    {
        if (!PV.IsMine)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartGrapple();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            StopGrapple();
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
        coolDownText.text = cooldownTimer.ToString("0");
    }

    //Called after Update
    void LateUpdate()
    {
        PV.RPC("DrawRope", RpcTarget.All);
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple()
    {
        if (Uses > 0)
        {
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
                joint.massScale = 5f;

                lr.positionCount = 2;
                currentGrapplePosition = gunTip.position;
            }
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple()
    {
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
