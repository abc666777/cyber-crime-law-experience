using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayBGM(Resources.Load("Audio/BGM/test1") as AudioClip);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
