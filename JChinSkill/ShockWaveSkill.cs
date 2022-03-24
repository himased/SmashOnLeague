using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using UnityEngine.UI;

public class ShockWaveSkill : MonoBehaviour
{
    [SerializeField] int Uses;
    [SerializeField] float cooldown;
    [SerializeField] Text usesText; //, coolDownText
    [SerializeField] Transform cam;

    [SerializeField] Image shockWaveCDImage;

    public Transform shockWaveTrans;

    int maxUses;
    float cooldownTimer;

    public SoundManager SM;

    PhotonView PV;
    // Start is called before the first frame update

    void Start()
    {
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

        shockWaveCDImage.fillAmount = cooldownTimer / cooldown;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Impale();
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

        if (PlayerController.Instance.currentHealth <= 0)
        {
            Uses = 0;
        }

        //coolDownText.text = cooldownTimer.ToString("0");
    }

    void Impale()
    {
        if (Uses > 0)
        {
            Uses -= 1;
            usesText.text = Uses.ToString();
            SM.PlaySound("skill_1");
            //First and third person
            //We calculate our current forward direction
            var spawnCalculation = shockWaveTrans.transform.position + (shockWaveTrans.transform.forward * 1f);
            //Create the first impale object that wil be the parent and will duplicate itself
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ShockWaveV2"), spawnCalculation, transform.rotation);
            //StartCoroutine(shockWaveTimes(3.5f));

        }
    }

    /*IEnumerator shockWaveTimes(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        var spawnCalculation = shockWaveTrans.transform.position + (shockWaveTrans.transform.forward * 1.5f);
        //Create the first impale object that wil be the parent and will duplicate itself
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ShockWaveV2"), spawnCalculation, transform.rotation);
    }*/
}
