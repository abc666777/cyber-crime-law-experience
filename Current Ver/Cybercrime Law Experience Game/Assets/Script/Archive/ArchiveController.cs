using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArchiveController : MonoBehaviour
{
    public TextMeshProUGUI description;
    private TextAsset archiveData;
    private string[] data;
    public GameObject fader;
    // Start is called before the first frame update
    void Awake() {
        LeanTween.alpha(fader.GetComponent<RectTransform>(), 0f, 0.3f);
        archiveData = AssetsLoader.instance.archiveData;
        data = archiveData.ToString().Replace("[blank]", " ").Split('\n');
        SetText(0);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            BackToMenuFunction();
        }
    }
    public void BackToMenuFunction(){
        SceneManager.instance.LoadScene(GlobalReferences.Scene.StartScene);
    }

    public void SetText(int index){
        string[] data = this.data[index].Split(':');
        description.text = data[0] + "\n\n" + data[1] + "\n\n" + data[2];
    }
}
