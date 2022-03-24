using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;

public class HookSkill : MonoBehaviour
{

    //[SerializeField] GameObject hookOBJ;

    PhotonView PV;

    [SerializeField] Transform cam, Player;
    [SerializeField] int Uses;
    [SerializeField] float cooldown;
    [SerializeField] Text usesText, coolDownText;

    int maxUses;
    float cooldownTimer;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        maxUses = Uses;
        cooldownTimer = cooldown;
        usesText.text = Uses.ToString();
        coolDownText.text = cooldown.ToString("0");
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
            return;

        if (Input.GetKeyDown(KeyCode.X))
        {
            Hook();
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

    //Hook Skill
    void Hook()
    {
        if (Uses > 0)
        {
            Uses -= 1;
            usesText.text = Uses.ToString();
            /*var hook = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "HookObject"), cam.position + cam.forward, cam.rotation);
            hook.GetComponent<HookScript>().caster = transform;*/


        }
    }

}
