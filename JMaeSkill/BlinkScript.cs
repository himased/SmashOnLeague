using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;

public class BlinkScript : MonoBehaviour
{
    [SerializeField] int Uses;
    [SerializeField] float cooldown, distance, speed, destinationMultiplier, cameraHeight;
    [SerializeField] Text usesText, coolDownText;
    [SerializeField] Transform cam;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Image blinqCDImage;

    private const float NORMAL_FOV = 60F;
    private const float BLINK_FOV = 80F;

    [SerializeField] Camera fpsCam;
    private CameraFov cameraFov;

    int maxUses;
    float cooldownTimer;
    bool blinking = false;
    Vector3 destination;
    ParticleSystem trail;

    PhotonView PV;

    public SoundManager SM;


    // Start is called before the first frame update
    void Start()
    {
        trail = transform.Find("Trail").GetComponent<ParticleSystem>();
        maxUses = Uses;
        cooldownTimer = cooldown;
        usesText.text = Uses.ToString();
        coolDownText.text = cooldown.ToString("0");
        cameraFov = fpsCam.GetComponent<CameraFov>();
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Blink();
        }

        blinqCDImage.fillAmount = cooldownTimer / cooldown;

        if(Uses < maxUses)
        {
            if(cooldownTimer > 0)
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

        if (blinking)
        {
            var dist = Vector3.Distance(transform.position, destination);
            if(dist > 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
            }
            else
            {
                cameraFov.SetCameraFov(NORMAL_FOV);
                blinking = false;
            }
        }

        //cameraFov.SetCameraFov(NORMAL_FOV);
    }

    void Blink()
    {
        if(Uses > 0)
        {
            cameraFov.SetCameraFov(BLINK_FOV);

            Uses -= 1;
            usesText.text = Uses.ToString();

            SM.PlaySound("skill_1");
            SM.PlaySound("SFX_2");

            //trail.Play();
            PV.RPC("PlayTrail", RpcTarget.All);

            var spawnCalculation = transform.position;
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Teleport3"), spawnCalculation, transform.rotation);

            RaycastHit hit;
            if(Physics.Raycast(cam.position, cam.forward, out hit, distance, layerMask))
            {
                destination = hit.point * destinationMultiplier;
            }
            else
            {
                destination = (cam.position + cam.forward.normalized * distance) * destinationMultiplier;
            }

            destination.y += cameraHeight;
            blinking = true;
        }
    }

    [PunRPC]
    void PlayTrail()
    {
        trail.Play();
    }
}
