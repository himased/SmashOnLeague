using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosMeteor : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] Vector3 spawnOffset = new Vector3(0f, 1f, 0f);
    [SerializeField] float flyDuration, travelDistance, flySpeed, groundSpeed, meteorSize, spinSpeed, pulseTime, pulseRadius, impactFXDestroyDelay;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] bool visualizeRadius;

    GameObject impactFX;
    Vector3 moveDirection, landLocation;
    float pulseTimer;
    bool flying = true;

    // Start is called before the first frame update
    void Start()
    {
        impactFX = transform.Find("ImpactFX").gameObject;
        moveDirection = transform.position - (transform.position + spawnOffset);
        //Position
        transform.position = transform.position + spawnOffset;
        //Rotation
        Vector3 relativePos = moveDirection - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }

    // Update is called once per frame
    void Update()
    {
        spawnOffset = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z);
        RaycastHit hit;
        if (flying)
        {
            if (flyDuration > 0)
            {
                flyDuration -= Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
            transform.position = transform.position + moveDirection * flySpeed * Time.deltaTime;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, meteorSize, groundLayer))
            {
                flying = false;
                moveDirection.y = 0;
                landLocation = hit.point;
                impactFX.SetActive(true);
                impactFX.transform.SetParent(null);
                Destroy(impactFX.gameObject, impactFXDestroyDelay);
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, meteorSize, groundLayer))
            {
                Vector3 nextPosition = new Vector3(transform.position.x, hit.point.y + meteorSize, transform.position.z);
                transform.position = nextPosition + moveDirection * groundSpeed * Time.deltaTime;
            }

            float dist = Vector3.Distance(landLocation, transform.position);
            if(dist > travelDistance)
            {
                Destroy(gameObject);
            }

            float spin = spinSpeed * Time.deltaTime;
            transform.Rotate(spin, 0f, 0f);
            DamagePulses();
        }
    }

    void DamagePulses()
    {
        if(pulseTimer > 0)
        {
            pulseTimer -= Time.deltaTime;
        }
        else
        {
            Collider[] gameObjectsInRange = Physics.OverlapSphere(transform.position, pulseRadius);

            foreach(Collider col in gameObjectsInRange)
            {
                Rigidbody enemy = col.GetComponent<Rigidbody>();

                if(enemy != null)
                {
                    col.gameObject.GetComponent<IDamageable>()?.TakeDamage(15);
                }
            }

            pulseTimer = pulseTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (visualizeRadius)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, pulseRadius);
        }
    }
}
