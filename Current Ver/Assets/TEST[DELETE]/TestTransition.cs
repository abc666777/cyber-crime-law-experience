using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTransition : MonoBehaviour
{
    public Texture2D tex1;
    public Texture2D tex2;
    public Texture2D tex3;
    public Texture2D trans1;
    public Texture2D trans2;
    public Texture2D trans3;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TransitionManager.TransitionLayer(BackgroundManager.instance.background, tex1, trans1);
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TransitionManager.TransitionLayer(BackgroundManager.instance.background, null, trans2);
        }
    }
}
