using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using UnityEngine.UI;


public class ShinraTenseiSkill : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] int Uses;
    [SerializeField] float cooldown;
    [SerializeField] Text usesText, coolDownText;

    int maxUses;
    float cooldownTimer;

    public KeyCode castKeybind;
    public float range;
    public GameObject shinraTenseiPreview;
    public LayerMask layerMask;
    private bool casting;

    PhotonView PV;

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
            if (!casting) { shinraTenseiPreview.SetActive(false); }
        }


        if (casting)
        {
            ShinraTensei();
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

    void ShinraTensei()
    {
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ward Of Dawn"), cam.position + cam.forward, cam.rotation);
        if (Uses > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.forward, out hit, range, layerMask))
            {
                if (!shinraTenseiPreview.activeSelf) { shinraTenseiPreview.SetActive(true); }

                Quaternion rotation = Quaternion.Euler(0, 0, 0);
                rotation.y = 0;

                shinraTenseiPreview.transform.localRotation = rotation;
                shinraTenseiPreview.transform.position = hit.point;

                if (Input.GetMouseButtonDown(1))
                {
                    Uses -= 1;
                    usesText.text = Uses.ToString();
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ward Of Dawn"), hit.point, shinraTenseiPreview.transform.rotation);
                    casting = false;
                    shinraTenseiPreview.SetActive(false);
                }
            }
            else
            {
                shinraTenseiPreview.SetActive(false);
            }
        }
    }
}
