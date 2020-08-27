using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3 : MonoBehaviour
{
    BackgroundManager controller;
    public Texture tex;
    public MovieTexture mov;

    BackgroundManager.Layer layer;
    // Start is called before the first frame update
    void Start()
    {
        controller = BackgroundManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKey(KeyCode.Q)){
            layer = controller.background;
            Debug.Log("Q");
        } 
        if(Input.GetKey(KeyCode.W)){
            layer = controller.cinematic;
        } 
        if(Input.GetKey(KeyCode.E)){
            layer = controller.foreground;
        } 

        else{
            if(Input.GetKeyDown(KeyCode.A)){
                layer.TransitionToTexture(tex, 2f, false);
            }
            else if(Input.GetKeyDown(KeyCode.S)){
                layer.TransitionToTexture(mov, 2f, false);
            }
        }
        
    }
}
