using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using UnityEngine.UI;

public class SmokeSkill : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] int Uses;
    [SerializeField] float cooldown;
    [SerializeField] Text usesText; //, coolDownText

    [SerializeField] Image smokeCDImage;

    int maxUses;
    float cooldownTimer;

    public KeyCode castKeybind;
    public float range;
    public GameObject smokePreview;
    public LayerMask layerMask;
    private bool casting;

    public SoundManager SM;

    PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        //cam = Camera.main.transform;
        PV = GetComponent<PhotonView>();

        maxUses = Uses;
        cooldownTimer = cooldown;
        usesText.text = Uses.ToString();
        //coolDownText.text = cooldown.ToString("0");
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
            return;

        if (Input.GetKeyDown(castKeybind))
        {
            casting = !casting;
            if (!casting) { smokePreview.SetActive(false); }
        }

        smokeCDImage.fillAmount = cooldownTimer / cooldown;

        if (casting) { 
            Smoke();
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

        if(PlayerController.Instance.currentHealth <= 0)
        {
            Uses = 0;
        }

        //coolDownText.text = cooldownTimer.ToString("0");
    }

    void Smoke()
    {
        if (Uses > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.forward, out hit, range, layerMask))
            {
                if (!smokePreview.activeSelf) { smokePreview.SetActive(true); }

                Quaternion rotation = Quaternion.Euler(0, 0, 0);
                rotation.y = 0;

                smokePreview.transform.localRotation = rotation;
                smokePreview.transform.position = hit.point;

                if (Input.GetMouseButtonDown(1))
                {
                    Uses -= 1;
                    usesText.text = Uses.ToString();
                    SM.PlaySound("skill_2");
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Smoke"), hit.point, smokePreview.transform.rotation);
                    casting = false;
                    smokePreview.SetActive(false);
                }
            }
            else
            {
                smokePreview.SetActive(false);
            }
        }
    }
}
