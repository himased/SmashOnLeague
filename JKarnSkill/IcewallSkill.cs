using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;

public class IcewallSkill : MonoBehaviour
{
    public KeyCode castKeybind, directionKeybind;
    public float range;
    public GameObject iceWallPreview;
    public LayerMask layerMask;
    private bool direction, casting;

    [SerializeField] int Uses;
    [SerializeField] float cooldown;
    [SerializeField] Text usesText, coolDownText;
    [SerializeField] Image iceWallCDImage;

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

        if (Input.GetKeyDown(castKeybind))
        {
            casting = !casting;
            if (!casting) { iceWallPreview.SetActive(false); }
        }

        iceWallCDImage.fillAmount = cooldownTimer / cooldown;
        
        if (casting) { CastingIceWall(); }

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

    void CastingIceWall()
    {
        if (Uses > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.forward, out hit, range, layerMask))
            {
                if (!iceWallPreview.activeSelf) { iceWallPreview.SetActive(true); }

                Quaternion rotation = Quaternion.Euler(0, 0, 0);
                if (direction) { rotation.y = 1; }
                else { rotation.y = 0; }

                iceWallPreview.transform.localRotation = rotation;
                iceWallPreview.transform.position = hit.point;

                if (Input.GetMouseButtonDown(1))
                {
                    Uses -= 1;
                    usesText.text = Uses.ToString();
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "IceWall(Clone)"), hit.point, iceWallPreview.transform.rotation);
                    casting = false;
                    iceWallPreview.SetActive(false);
                    SM.PlaySound("skill_1");
                    SM.PlaySound("SFX");
                }
            }
            else
            {
                iceWallPreview.SetActive(false);
            }

            if (Input.GetKeyDown(directionKeybind)) { direction = !direction; }
        }
    }
}
