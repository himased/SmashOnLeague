using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;

public class ZeroPointBreakSkill : MonoBehaviour
{
    public KeyCode castKeybind;
    public float range;
    public GameObject ZeroPointBreakPreview;
    public LayerMask layerMask;
    private bool  casting;

    [SerializeField] int Uses;
    [SerializeField] float cooldown;
    [SerializeField] Text usesText, coolDownText;
    [SerializeField] Image zpbCDImage;

    int maxUses;
    float cooldownTimer;

    public Transform cam;

    PhotonView PV;

    public SoundManager SM;

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

        zpbCDImage.fillAmount = cooldownTimer / cooldown;

        if (Input.GetKeyDown(castKeybind))
        {
            casting = !casting;
            if (!casting) { ZeroPointBreakPreview.SetActive(false); }
        }


        if (casting) { CastingZeroPointBreak(); }

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

    void CastingZeroPointBreak()
    {
        if (Uses > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.forward, out hit, range, layerMask))
            {
                if (!ZeroPointBreakPreview.activeSelf) { ZeroPointBreakPreview.SetActive(true); }

                Quaternion rotation = Quaternion.Euler(0, 0, 0);

                ZeroPointBreakPreview.transform.localRotation = rotation;
                ZeroPointBreakPreview.transform.position = hit.point;

                if (Input.GetMouseButtonDown(1))
                {
                    Uses -= 1;
                    usesText.text = Uses.ToString();
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "FrozenMine"), hit.point, ZeroPointBreakPreview.transform.rotation);
                    casting = false;
                    ZeroPointBreakPreview.SetActive(false);
                    SM.PlaySound("skill_2");
                }
            }
            else
            {
                ZeroPointBreakPreview.SetActive(false);
            }
        }
    }
}
