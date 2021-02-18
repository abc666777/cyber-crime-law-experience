using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject fader;
    [SerializeField] private GameObject btn1;
    [SerializeField] private GameObject btn2;
    [SerializeField] private GameObject btn3;
    [SerializeField] private GameObject btn4;
    [SerializeField] private GameObject btn5;
    [SerializeField] private GameObject btn6;

    void Awake() {
        Init();
    }
    
    public void NewGameFunction(){
        Instantiate(AssetsLoader.instance.PanelLoader(GlobalReferences.Panel.NewGamePanel), GameObject.Find("Canvas").transform);
    }

    public void LoadGameFunction(){
        Instantiate(AssetsLoader.instance.PanelLoader(GlobalReferences.Panel.LoadGamePanel), GameObject.Find("Canvas").transform);
    }

    public void SettingFunction(){
        Instantiate(AssetsLoader.instance.PanelLoader(GlobalReferences.Panel.SettingPanel), GameObject.Find("Canvas").transform);
    }

    public void ArchiveFunction(){
        SceneManager.instance.LoadScene(GlobalReferences.Scene.ArchiveScene);
    }

    private void DestroyFader(){
        Destroy(fader);
    }

    void Init(){
        LeanTween.alpha(fader.GetComponent<RectTransform>(), 0f, 6f).setOnComplete(DestroyFader);
        /*LeanTween.moveX(btn1.GetComponent<RectTransform>(), -330f, 1f);
        LeanTween.moveX(btn2.GetComponent<RectTransform>(), -330f, 1f).setDelay(1f);
        LeanTween.moveX(btn3.GetComponent<RectTransform>(), -330f, 1f).setDelay(2f);
        LeanTween.moveX(btn4.GetComponent<RectTransform>(), -330f, 1f).setDelay(3f);
        LeanTween.moveX(btn5.GetComponent<RectTransform>(), -330f, 1f).setDelay(4f);
        LeanTween.moveX(btn6.GetComponent<RectTransform>(), -330f, 1f).setDelay(5f);*/
    }
}
