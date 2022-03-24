using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.IO;

public class RecallAbility : MonoBehaviour
{
    //How far back in time are we going
    public float maxDuration;
    //How often do we save information to be restored
    public float saveInterval;
    //How fast we go back in time
    public float recallSpeed;
    //UI effect
    public CanvasGroup cameraFX;

    //Stats to save in lists, the lists lenghts depend on our maxDuration and saveInterval
    [HideInInspector]
    public List<Vector3> positions;
    [HideInInspector]
    public List<int> healths;

    //This boolean controls our loops in Update
    private bool recalling;
    //This float is used to store our timer
    private float saveStatsTimer;
    //This float limits our lists to avoid saving too much
    private float maxStatsStored;
    //Player controller to disable during recall
    private PlayerController controller;
    //OPTIONAL
    private CapsuleCollider col;
    //OPTIONAL
    private Rigidbody rb;

    [SerializeField] int Uses;
    [SerializeField] float cooldown;
    [SerializeField] Text usesText, coolDownText;

    [SerializeField] Image requiemCDImage;

    int maxUses;
    float cooldownTimer;
    PhotonView PV;

    public SoundManager SM;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        //Get our controller
        controller = GetComponent<PlayerController>();
        //OPTIONAL
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        //Calculate the list's length
        maxStatsStored = maxDuration / saveInterval;

        maxUses = Uses;
        cooldownTimer = cooldown;
        usesText.text = Uses.ToString();
        coolDownText.text = cooldown.ToString("0");
    }

    void Update()
    {
        if (!PV.IsMine)
            return;

        requiemCDImage.fillAmount = cooldownTimer / cooldown;

        if(Input.GetKeyDown(KeyCode.Q) && Uses > 0)
        {
            SM.PlaySound("Skill_2");
            SM.PlaySound("SFX");
        }

        if (!recalling)
        {
            //Get the input to start recalling, you could add a cooldown check here
            if (Input.GetKeyDown(KeyCode.Q) && positions.Count > 0 && Uses > 0)
            {
                Uses -= 1;
                usesText.text = Uses.ToString();

                var spawnCalculation = transform.position;
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Teleport3"), spawnCalculation, transform.rotation);

                //Set our boolean to true

                recalling = true;

                //OPTIONAL
                controller.enabled = false;
                col.enabled = false;
                rb.isKinematic = true;
            }

            //Handling our saving timer
            if (saveStatsTimer > 0)
            {
                saveStatsTimer -= Time.deltaTime; //If our timer is bigger than 0 we lower it by deltaTime
            }
            else
            {
                StoreStats(); //If our timer is smaller or equals to zero we save our stats
            }

            //We set the canvas group alpha to 0 ovetime
            cameraFX.alpha = Mathf.Lerp(cameraFX.alpha, 0, recallSpeed * Time.deltaTime);
        }
        else
        {
            //If we have values stored in our position list
            if (positions.Count > 0)
            {
                //We move our transform to the first position in our list overtime
                transform.position = Vector3.Lerp(transform.position, positions[0], recallSpeed * Time.deltaTime);
                //Store the distance between our transform and the first position in our list
                float dist = Vector3.Distance(transform.position, positions[0]);
                if (dist < 0.25f) //If the distance variable is smaller than 0.25
                {
                    SetStats(); //We set the stats
                }
            }
            
            else //No values stored in our position list so we set our loop bool accordingly
            {
                //Set our boolean to false
                recalling = false;

                //OPTIONAL
                controller.enabled = true;
                col.enabled = true;
                rb.isKinematic = false;
            }

            //We set the canvas group alpha to 1 ovetime
            cameraFX.alpha = Mathf.Lerp(cameraFX.alpha, 1, recallSpeed * Time.deltaTime);
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

        coolDownText.text = cooldownTimer.ToString("0");

    }

    void SetStats()
    {
        //Set the health of our player
        controller.currentHealth = healths[0];
        //Set the location of this transform
        transform.position = positions[0];

        //Removing the first stored stats from our lists
        positions.RemoveAt(0);
        healths.RemoveAt(0);
    }

    void StoreStats()
    {
        //Set the saving timer
        saveStatsTimer = saveInterval;

        //Add the current position to our position list
        positions.Insert(0, transform.position);
        //Add our current health to our health list
        healths.Insert(0, ((int)controller.currentHealth));

        //Check if we have more than the maximum amount of stats we can store, if we do then we remove the last stored data
        if (positions.Count > maxStatsStored)
        {
            positions.RemoveAt(positions.Count - 1);
        }
        if (healths.Count > maxStatsStored)
        {
            healths.RemoveAt(healths.Count - 1);
        }
    }
}
