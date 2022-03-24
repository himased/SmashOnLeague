using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class UsernameDisplay : MonoBehaviour
{
    [SerializeField] PhotonView playerPV;
    [SerializeField] TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        if (playerPV.IsMine)
        {
            gameObject.SetActive(false);
        }
        text.text = playerPV.Owner.NickName;

        /*if(PlayerManager.Instance.myTeam == 1)
        {
            text.color = Color.red;
        }
        else
        {
            text.color = Color.blue;
        }*/
    }
}
