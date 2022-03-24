using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class BallManager : MonoBehaviour
{
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        Debug.Log("Instantiated Ball Controller"); // Instantiate our ball controller
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "BallController"), Vector3.zero, Quaternion.identity);
    }
}
