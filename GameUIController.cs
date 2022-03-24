using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameUIController : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    private float coolDown;
    [SerializeField] TMP_Text coolDownText;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        coolDown = BallController.Instance.getCoolDown;
        Debug.Log(coolDown);
        coolDownText.text = coolDown.ToString("0");
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;
    }
}
