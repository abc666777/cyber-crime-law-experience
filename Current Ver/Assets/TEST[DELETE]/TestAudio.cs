using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudio : MonoBehaviour
{
    public AudioClip sfx;
    public AudioClip bgm;
    public AudioClip bgm1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance.PlayBGM(bgm);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AudioManager.instance.PlayBGM(bgm1);
        }
    }
}
