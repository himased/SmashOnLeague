using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraCollider : MonoBehaviour
{
    public Transform cam;
    Vector3 cameraDirection;

    float camDistance;
    Vector2 cameraDistanceMinMax = new Vector2(0.5f, 0.5f);

    // Start is called before the first frame update
    void Start()
    {
        cameraDirection = cam.localPosition.normalized;
        camDistance = cameraDistanceMinMax.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CheckCameraOcclusionAndCollision(cam);
    }

    public void CheckCameraOcclusionAndCollision(Transform cam)
    {
        Vector3 desiredCamPosition = transform.TransformPoint(cameraDirection * cameraDistanceMinMax.y);
        RaycastHit hit;
       // Debug.DrawLine(transform.position, desiredCamPosition, Color.red);
        if(Physics.Linecast(transform.position, desiredCamPosition, out hit))
        {
            camDistance = Mathf.Clamp(hit.distance, cameraDistanceMinMax.x, cameraDistanceMinMax.y);
        }
        else
        {
            camDistance = cameraDistanceMinMax.y;
        }

        cam.localPosition = cameraDirection * camDistance;
    }
}
