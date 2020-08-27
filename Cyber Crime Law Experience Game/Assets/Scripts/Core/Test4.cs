using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test4 : MonoBehaviour
{
    public float volume, pitch;
    public AudioClip[] fxs;
    public AudioClip[] bgms;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            AudioManager.instance.PlaySFX(fxs[Random.Range(0, fxs.Length)], volume, pitch);
        }

        if(Input.GetKeyDown(KeyCode.M)){
            AudioManager.instance.PlayBGM(bgms[Random.Range(0, bgms.Length)]);
        }

        if(Input.GetKeyDown(KeyCode.S)){
            AudioManager.instance.PlayBGM(bgms[0]);
        }

         if(Input.GetKeyDown(KeyCode.D)){
            AudioManager.instance.PlayBGM(bgms[4]);
        }
    }
}
