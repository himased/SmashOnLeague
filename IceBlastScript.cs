using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlastScript : MonoBehaviour
{
    public bool scaleUp = true;
    public float flySpeed, flyRotateSpeed, waitingRotateSpeed,scalingSpeed, maxScale, minScale, timeToReachTarget, impactDestroyDelay, maxDistance, fxScaleSpeed;
    public KeyCode releaseKey;
    public string enemyTag;
    [HideInInspector]
    public Transform caster;

    GameObject impactFX, flyingFX, blastFX;
    float t;
    bool hasBlasted = false;
    Vector3 startPosition, endPosition, endScale;

    private void Start()
    {
        impactFX = transform.Find("ImpactFX").gameObject;
        flyingFX = transform.Find("FlyingFX").gameObject;
        transform.localScale = new Vector3(minScale, minScale, minScale);
        endScale = transform.localScale;
        startPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(releaseKey) && !hasBlasted)
        {
            CreateBlast();
        }

        if (hasBlasted)
        {
            t += Time.deltaTime / timeToReachTarget;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            if (flyingFX) 
            {
                flyingFX.transform.RotateAround(flyingFX.transform.position, flyingFX.transform.right, waitingRotateSpeed * Time.deltaTime);
                flyingFX.transform.localScale = Vector3.Lerp(flyingFX.transform.localScale, endScale, fxScaleSpeed * Time.deltaTime);
            }

            var distance = Vector3.Distance(transform.position, endPosition);
            if (distance < 0.1f) { Impact(); }
        }
        else
        {
            flyingFX.transform.RotateAround(transform.position, transform.right, flyRotateSpeed * Time.deltaTime);
            transform.Translate(Vector3.forward * flySpeed * Time.deltaTime);
            endScale = Vector3.Lerp(endScale, new Vector3(maxScale, maxScale, maxScale), scalingSpeed * Time.deltaTime);
            if (scaleUp) transform.localScale = endScale;

            var distance = Vector3.Distance(startPosition, transform.position);
            if (distance > maxDistance) { CreateBlast(); }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == enemyTag && hasBlasted)
        {
            //Frostbitten effect to avoid enemy from healing
        }
    }

    void CreateBlast()
    {
        hasBlasted = true;

        flyingFX.transform.SetParent(null);
        Destroy(flyingFX, timeToReachTarget);

        blastFX = Instantiate(flyingFX, transform.position, flyingFX.transform.rotation);
        blastFX.transform.SetParent(transform);
        blastFX.transform.localScale = new Vector3(1, 1, 1);

        startPosition = caster.transform.position;
        endPosition = blastFX.transform.position;
        transform.position = startPosition;
        transform.localScale = new Vector3(minScale, minScale, minScale);
    }

    void Impact()
    {
        impactFX.transform.SetParent(null);
        impactFX.transform.localScale = endScale;
        impactFX.SetActive(true);

        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, endScale.x);
        foreach (Collider col in objectsInRange)
        {
            if (col.tag == enemyTag)
            {
                //EnemyScript enemy = col.GetComponent<EnemyScript>().ApplyDamage(1);
            }
        }

        Destroy(impactFX, impactDestroyDelay);
        Destroy(gameObject);
    }
}
