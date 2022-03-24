using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource Audio;

    public AudioClip Click, SelectChar;

    public static MainMenuSoundManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
