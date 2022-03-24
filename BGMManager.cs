using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource _AudioSource; 

    public AudioClip _AudioClip1;
    public AudioClip _AudioClip2;
    public AudioClip _AudioClip3;

    void Start()
    {
        _AudioSource.clip = _AudioClip1;
        _AudioSource.Play();
    }


    void Update()
    {

        if (MapCaptureController.Instance.teamOneCaptured == MapCaptureController.Instance.captureObject / 2 && PlayerManager.Instance.myTeam == 1)
        {
            if (_AudioSource.clip == _AudioClip1)
            {

                _AudioSource.clip = _AudioClip2;

                _AudioSource.Play();

            }
        }
        if (MapCaptureController.Instance.teamTwoCaptured == MapCaptureController.Instance.captureObject / 2 && PlayerManager.Instance.myTeam == 2)
        {
            if (_AudioSource.clip == _AudioClip1)
            {

                _AudioSource.clip = _AudioClip2;

                _AudioSource.Play();

            }
        }

        if (MapCaptureController.Instance.teamOneCaptured == MapCaptureController.Instance.captureObject && PlayerManager.Instance.myTeam == 1)
        {
            if (_AudioSource.clip == _AudioClip2)
            {

                _AudioSource.clip = _AudioClip3;

                _AudioSource.Play();

            }
        }
        if (MapCaptureController.Instance.teamTwoCaptured == MapCaptureController.Instance.captureObject && PlayerManager.Instance.myTeam == 2)
        {
            if (_AudioSource.clip == _AudioClip2)
            {

                _AudioSource.clip = _AudioClip3;

                _AudioSource.Play();

            }
        }

    }
}
