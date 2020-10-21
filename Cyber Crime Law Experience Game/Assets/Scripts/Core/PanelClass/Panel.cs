using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Panel : MonoBehaviour
{
    void Start() {
        SetTitle(PlayerPrefs.GetString("Scene Title"));
    }
    [SerializeField] private TextMeshProUGUI panelTitle;
    public void Close(){
        Destroy(gameObject);
    }

    public void BackToMainMenu(){
        SceneManager.instance.LoadScene("Start");
        
    }

    public void SetTitle(string title){
        if(panelTitle != null)
            panelTitle.text = title;
    }
}
