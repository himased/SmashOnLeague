using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;

public class ZaQuantum : MonoBehaviour
{
    PhotonView PV;

    [SerializeField] Transform cam;
    [SerializeField] int Uses;
    [SerializeField] float cooldown;
    [SerializeField] Text usesText, coolDownText;
    [SerializeField] Image blackHoleCDImage;

    int maxUses;
    float cooldownTimer;

    public KeyCode castKeybind;
    public float range;
    public GameObject ZeQuantumPreview;
    public LayerMask layerMask;
    private bool casting;

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

        blackHoleCDImage.fillAmount = cooldownTimer / cooldown;

        if (Input.GetKeyDown(castKeybind))
        {
            casting = !casting;
            if (!casting) { ZeQuantumPreview.SetActive(false); }
        }


        if (casting)
        {
            ZeQuantumu();
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

    void ZeQuantumu()
    {
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Will-O-Wisp"), transform.position , transform.rotation);
        if (Uses > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.forward, out hit, range, layerMask))
            {
                if (!ZeQuantumPreview.activeSelf) { ZeQuantumPreview.SetActive(true); }

                Quaternion rotation = Quaternion.Euler(0, 0, 0);
                rotation.y = 0;

                ZeQuantumPreview.transform.localRotation = rotation;
                ZeQuantumPreview.transform.position = hit.point;

                if (Input.GetMouseButtonDown(1))
                {
                    Uses -= 1;
                    usesText.text = Uses.ToString();
                    SM.PlaySound("skill_1");
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "BlackHole"), hit.point + new Vector3(0, 1f, 0), ZeQuantumPreview.transform.rotation);
                    casting = false;
                    ZeQuantumPreview.SetActive(false);
                }
            }
            else
            {
                ZeQuantumPreview.SetActive(false);
            }
        }
    }
}
