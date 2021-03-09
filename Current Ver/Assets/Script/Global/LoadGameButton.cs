using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class LoadGameButton : MonoBehaviour
{
    private GAMEFILE gameFile;
    public int SaveIndex;
    public TextMeshProUGUI episodeText;
    public TextMeshProUGUI dateText;
    // Start is called before the first frame update
    private void Awake()
    {
        SetInterface();
    }

    public void OnClick()
    {
        if (File.Exists(GlobalReferences.Path.SavePath + SaveIndex))
        {
            GameManager.instance.currentMode = GameManager.LoadMode.loadGame;
            GameManager.instance.SaveIndex = this.SaveIndex;
            SceneManager.instance.LoadScene(GlobalReferences.Scene.GameplayScene);
            //Destroy(gameObject);
        }
    }

    private void SetInterface()
    {
        try
        {
            gameFile = FileManager.LoadJSON<GAMEFILE>(GlobalReferences.Path.SavePath + SaveIndex);
            episodeText.text = "EPISODE: " + gameFile.episodeNumber;
            dateText.text = gameFile.date;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            episodeText.text = "EMPTY SAVE DATA";
            dateText.text = "";
        }
    }
}
