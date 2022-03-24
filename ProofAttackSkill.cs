using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using UnityEngine.UI;

public class ProofAttackSkill : MonoBehaviour
{
    public GameObject iceBlast;

    [SerializeField] Transform cam;
    [SerializeField] int Uses;
    [SerializeField] float cooldown;
    [SerializeField] Text usesText, coolDownText;

    int maxUses;
    float cooldownTimer;

    PhotonView PV;

    GameObject _iceBlast;

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
            ProofAttack();
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

    void ProofAttack()
    {
        if (Uses > 0)
        {
            Uses -= 1;
            usesText.text = Uses.ToString();
            var tempIceBlast = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ProofATK"), cam.position + cam.forward, cam.rotation);
            tempIceBlast.GetComponent<IceBlastScript>().caster = transform;
        }
    }
}
