using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using UnityEngine.UI;

public class GravStickSkill : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] int Uses;
    [SerializeField] float cooldown;
    [SerializeField] Text usesText, coolDownText;
    [SerializeField] Image SmashCDImage;

    int maxUses;
    float cooldownTimer;

    PhotonView PV;

    public GameObject playerController;
    Animator anim;
    bool canSwing = false;

    public SoundManager SM;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        maxUses = Uses;
        cooldownTimer = cooldown;
        usesText.text = Uses.ToString();
        coolDownText.text = cooldown.ToString("0");

        anim = playerController.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
            return;

        SmashCDImage.fillAmount = cooldownTimer / cooldown;

        if (Input.GetKeyDown(KeyCode.E))
        {
            //GravStick();
        }

        if (Uses > 0)
        {
            canSwing = true;
        }
        if(Uses <= 0)
        {
            canSwing = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && canSwing == true)
        {
            anim.SetBool("useSkill1", true);
            anim.SetBool("inAir", false);
        }

        if (PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.jumpState)
        {
            if (Input.GetKeyDown(KeyCode.E) && canSwing == true)
            {
                anim.SetBool("useSkill1", true);
                anim.SetBool("Jump", false);
            }
        }

        if (PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.idleJumpState)
        {
            if (Input.GetKeyDown(KeyCode.E) && canSwing == true)
            {
                anim.SetBool("useSkill1", true);
                anim.SetBool("Jump", false);
            }
        }

        if (PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.jumpBackState)
        {
            if (Input.GetKeyDown(KeyCode.E) && canSwing == true)
            {
                anim.SetBool("useSkill1", true);
                anim.SetBool("Jump", false);
            }
        }

        if (PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.locoState)
        {
            if (Input.GetKeyDown(KeyCode.E) && canSwing == true)
            {
                anim.SetBool("useSkill1", true);
                //PlayerController.Instance.anim.SetBool("InAir", false);
            }
        }

        if (PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.walkBackState)
        {
            if (Input.GetKeyDown(KeyCode.E) && canSwing == true)
            {
                anim.SetBool("useSkill1", true);
                //PlayerController.Instance.anim.SetBool("InAir", false);
            }
        }

        if (PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.locoNoBallState)
        {
            if (Input.GetKeyDown(KeyCode.E) && canSwing == true)
            {
                anim.SetBool("useSkill1", true);
                anim.SetBool("InAir", false);
            }
        }

        if (PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.airTimeState || PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.airTimeBackState)
        {
            if (Input.GetKeyDown(KeyCode.E) && canSwing == true)
            {
                anim.SetBool("useSkill1", true);
            }
        }

        if (PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.rollBackState || PlayerController.Instance.currentBaseState.fullPathHash == PlayerController.rollState)
        {
            if (Input.GetKeyDown(KeyCode.E) && canSwing == true)
            {
                anim.SetBool("useSkill1", true);
            }
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
        usesText.text = Uses.ToString();
    }

    void GravStick()
    {
        if (Uses > 0)
        {
            Uses -= 1;
            //usesText.text = Uses.ToString();
            SM.PlaySound("skill_2");
            SM.PlaySound("SFX");
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Spear"), cam.position + cam.forward, cam.rotation);
        }
    }

    void GravStickSecond()
    {
        SM.PlaySound("SFX");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Spear"), cam.position + cam.forward, cam.rotation);
    }

    void HitFloorGravStick()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Smash"), transform.position , transform.rotation);
    }
}
