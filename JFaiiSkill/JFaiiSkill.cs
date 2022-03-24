using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class JFaiiSkill : MonoBehaviour
{
    [SerializeField] Transform cam;

    PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        //cam = Camera.main.transform;
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
            return;
        if (Input.GetKeyDown(KeyCode.X))
        {
            ChaosMeteor();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Spear();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //ShinraTensei();
        }
    }

    //Hook Skill
    void ChaosMeteor()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Meteor"), transform.position, transform.rotation);
    }

    /*void Spear()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Spear"), cam.position + cam.forward, cam.rotation);
    }*/

    /*void ShinraTensei()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Ward Of Dawn"), cam.position + cam.forward, cam.rotation);
    }*/
}
