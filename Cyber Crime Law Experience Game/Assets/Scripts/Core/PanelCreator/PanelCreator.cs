using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelCreator : MonoBehaviour
{
    GameObject[] panels;
    // Start is called before the first frame update
    void Start()
    {
        panels = Resources.LoadAll<GameObject>("Prefabs/Panel");
    }

    public void InstantiatePanel(int index){
        GameObject.Instantiate(panels[index]);
    }

    public void SetTitle(string title){
        PlayerPrefs.SetString("Scene Title", title);
    }
}
