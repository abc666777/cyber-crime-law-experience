using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test6 : MonoBehaviour
{
    public Texture tex1;
    public Texture tex2;
    public Texture tex3;

    public Texture tran1;
    public Texture tran2;
    public Texture tran3;

    public int progress = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)){
            if(Input.GetKeyDown(KeyCode.DownArrow)){
                progress = Mathf.Clamp(progress - 1, 0, 10);
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow)){
                progress = Mathf.Clamp(progress + 1, 0, 10);
            }

            switch(progress)
            {
                case 0:
                    TransitionManager.ShowScene(false);
                    break;
                case 1:
                    TransitionManager.ShowScene(true);
                    break;
                case 2:
                    TransitionManager.transitionLayer(BackgroundManager.instance.background, tex1, tran1);
                    break;
                case 3:
                    TransitionManager.transitionLayer(BackgroundManager.instance.background, tex2, tran2);
                    break;
                case 4:
                    TransitionManager.transitionLayer(BackgroundManager.instance.background, tex3, tran3);
                    break;
                case 5:
                    BackgroundManager.instance.background.TransitionToTexture(tex1);
                    break;
                case 6:
                    TransitionManager.transitionLayer(BackgroundManager.instance.background, tex3, tran3);
                    break;
                case 7:
                    BackgroundManager.instance.background.TransitionToTexture(tex2);
                    break;
                case 8:
                    TransitionManager.transitionLayer(BackgroundManager.instance.background, null, tran1);
                    break;
                case 9:
                    BackgroundManager.instance.background.TransitionToTexture(tex3);
                    TransitionManager.ShowScene(true);
                    break;
                case 10:
                    TransitionManager.ShowScene(false);
                    break;
            }
        }
    }
}
