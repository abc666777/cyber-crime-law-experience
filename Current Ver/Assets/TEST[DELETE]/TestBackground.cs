using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBackground : MonoBehaviour
{
    BackgroundManager controller;
    public Texture tex;
    public float speed;
    public bool smooth;
    BackgroundManager.Layer layer = null;

    // Start is called before the first frame update
    void Start()
    {
        controller = BackgroundManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            layer = controller.background;
        }
        if (Input.GetKey(KeyCode.W))
        {
            layer = controller.cinematic;
        }
        if (Input.GetKey(KeyCode.E))
        {
            layer = controller.foreground;
        }
        if (Input.GetKey(KeyCode.T))
        {
            layer.TransitionToTexture(tex, speed, smooth);
        }
    }
}
