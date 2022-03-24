using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class StuntController : MonoBehaviour
{
    
    public bool isStunt, isHit;

    PostProcessVolume v;
    ChromaticAberration ca;
    Bloom b;

    public static StuntController Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isStunt = false;
        isHit = false;
        v = GetComponent<PostProcessVolume>();
        v.profile.TryGetSettings(out ca);
        v.profile.TryGetSettings(out b);
    }

    // Update is called once per frame
    void Update()
    {
        if(isStunt == true)
        {
            ca.intensity.value = 1f; 
            b.intensity.value = 10f;
        }
        if(isStunt == false)
        {
            ca.intensity.value = 0f;
            b.intensity.value = 0.1f;
        }
        if(isHit == true)
        {
            b.intensity.value = 10f;
        }
        if(isHit == false)
        {
            b.intensity.value = 0.1f;
        }
    }
}
