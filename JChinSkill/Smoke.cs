using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    [SerializeField] float duration, health;
    [SerializeField] string bulletToCheck;
    [SerializeField] LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            transform.position = hit.point;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 || duration <= 0)
        {
            Destroy(gameObject);
        }
        if (duration > 0)
        {
            duration -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bulletToCheck == other.tag)
        {
            WardCollision wcol = other.GetComponent<WardCollision>();

            if (wcol)
            {
                wcol.particleImpactFX.SetActive(true);
                wcol.particleImpactFX.transform.SetParent(null);

                //Destroy(wcol.particleImpactFX, wcol.particleDestroyDelay);
                //Destroy(wcol.transform.parent.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (bulletToCheck == other.tag)
        {
            WardCollision wcol = other.GetComponent<WardCollision>();

            if (wcol)
            {
                wcol.particleImpactFX.SetActive(false);
            }
        }
    }
}
