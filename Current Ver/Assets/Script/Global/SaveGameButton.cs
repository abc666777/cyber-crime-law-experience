using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class SaveGameButton : MonoBehaviour
{
    // Start is called before the first frame update
    private GAMEFILE gameFile;
    public int SaveIndex;
    public TextMeshProUGUI episodeText;
    public TextMeshProUGUI dateText;
    private void Awake()
    {
        //gameFile = FileManager.LoadJSON<GAMEFILE>(GlobalReferences.Path.SavePath + SaveIndex);
        SetInterface();
    }

    public void OnClick()
    {
        NovelController.instance.SaveGameFile(SaveIndex);
        SetInterface();
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


    // Update is called once per frame
}
