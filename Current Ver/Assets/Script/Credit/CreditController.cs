using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditController : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayBGM(AssetsLoader.instance.AudioLoader(GlobalReferences.Audio.CreditTheme, "BGM"), loop: false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            AudioManager.instance.PlayBGM(null);
            SceneManager.instance.LoadScene(GlobalReferences.Scene.StartScene);
        }
    }
}
