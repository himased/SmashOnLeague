using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SoundManager : MonoBehaviour
{
    PhotonView PV;
    public AudioClip playerHitSound, throwSound;
    public AudioClip[] ajentSound;
    AudioClip _ajentSound;
    AudioSource audioSrc;

    public static SoundManager Instance;

    private void Awake()
    {
        Instance = this;

        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerHitSound = Resources.Load<AudioClip>("SoundEffect/Throw");
        throwSound = Resources.Load<AudioClip>("SoundEffect/BallHitting");

        audioSrc = GetComponent<AudioSource>();

        if (!PV.IsMine)
        {
            Destroy(audioSrc);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }
    }

    public void PlaySound(string clip)
    {
        if (!PV.IsMine)
            return;

        switch (clip)
        {
            case "throw":
                audioSrc.PlayOneShot(throwSound);
                _ajentSound = ajentSound[0];
                audioSrc.PlayOneShot(_ajentSound);
                break;
            case "hit":
                //audioSrc.PlayOneShot(playerHitSound);
                _ajentSound = ajentSound[1];
                audioSrc.PlayOneShot(_ajentSound);
                break;
            case "hitEnemy":
                audioSrc.PlayOneShot(playerHitSound);
                _ajentSound = ajentSound[2];
                audioSrc.PlayOneShot(_ajentSound);
                break;
            case "ballBack":
                _ajentSound = ajentSound[3];
                audioSrc.PlayOneShot(_ajentSound);
                break;
            case "respawn":
                _ajentSound = ajentSound[4];
                audioSrc.PlayOneShot(_ajentSound);
                break;
            case "die":
                _ajentSound = ajentSound[5];
                audioSrc.PlayOneShot(_ajentSound);
                break;
            case "jump":
                _ajentSound = ajentSound[6];
                audioSrc.PlayOneShot(_ajentSound);
                break;
            case "SFX":
                _ajentSound = ajentSound[7];
                audioSrc.PlayOneShot(_ajentSound);
                break;
            case "inCaptureArea":
                _ajentSound = ajentSound[8];
                audioSrc.PlayOneShot(_ajentSound);
                break;
            case "capturedArea":
                _ajentSound = ajentSound[9];
                audioSrc.PlayOneShot(_ajentSound);
                break;
            case "emote":
                _ajentSound = ajentSound[10];
                audioSrc.PlayOneShot(_ajentSound);
                break;
            case "skill_1":
                _ajentSound = ajentSound[11];
                audioSrc.PlayOneShot(_ajentSound);
                break;
            case "skill_2":
                _ajentSound = ajentSound[12];
                audioSrc.PlayOneShot(_ajentSound);
                break;
            case "SFX_2":
                _ajentSound = ajentSound[13];
                audioSrc.PlayOneShot(_ajentSound);
                break;
        }
    }
}
