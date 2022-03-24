using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControl : MonoBehaviour
{
   public Transform Target;

    public Transform Obstruction;
    float zoomSpeed = 2f;

    bool isZoom;
    
    void Start()
    {
        Obstruction = Target;
    }

    private void LateUpdate()
    {

    }
    

    void ViewObstructed()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Target.transform.position - transform.position, out hit, 4.5f))
        {
           //Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            if (hit.collider.gameObject.tag != "Player")
            {
                Obstruction = hit.transform;
                Debug.Log("Obstruct: " + Obstruction);
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

                isZoom = true;

                //if(Vector3.Distance(Obstruction.position, transform.position) >= 3f && Vector3.Distance(transform.position, Target.position) >= 1.5f)
                    //transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
            }
            else
            {
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                //if (Vector3.Distance(transform.position, Target.position) < 4.5f)
                //transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
                isZoom = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            Obstruction = other.transform;
            Debug.Log("Obstruct: " + Obstruction);
            Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }
}