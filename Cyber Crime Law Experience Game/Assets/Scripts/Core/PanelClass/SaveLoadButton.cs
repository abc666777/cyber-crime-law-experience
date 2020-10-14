using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveLoadButton : MonoBehaviour
{
    [SerializeField] private int saveIndex;
    [SerializeField] private TextMeshProUGUI fileName;
    [SerializeField] private TextMeshProUGUI dateTime;
    private GAMEFILE activeGameFile;

    private enum MODE {save,load};
    private MODE mode;
    // Start is called before the first frame update
    void Start()
    {
        switch (PlayerPrefs.GetString("Scene Title"))
        {
            case "โหลดเกม":
                mode = MODE.load;
                break;
            case "บันทึกเกม":
                mode = MODE.save;
                break;
            default:
                break;
        }

        Innitialize();
    }

    // Update is called once per frame
    public void Delete()
    {
        string filePath = FileManager.savPath + "Resources/gameFiles/" + saveIndex.ToString() + ".txt";
        if(System.IO.File.Exists(filePath)){
            System.IO.File.Delete(filePath);
            Innitialize();
        }
    }
    
    public void OnClick(){
        switch (mode)
        {
            case MODE.load:
                string filePath = FileManager.savPath + "Resources/gameFiles/" + saveIndex.ToString() + ".txt";
                if(System.IO.File.Exists(filePath)){
                    PlayerPrefs.SetInt("Load File", saveIndex);
                    PlayerPrefs.SetString("Load Mode", "load");
                    SceneManager.instance.LoadScene("Gameplay");
                }
                else{
                    Debug.LogError("File at " + saveIndex.ToString() + " does not exist");
                }
                break;
            case MODE.save:
                NovelController.instance.SaveGameFile(saveIndex);
                Innitialize();
                break;
            default:
                break;
        }
    }

    void Innitialize(){
        string filePath = FileManager.savPath + "Resources/gameFiles/" + saveIndex.ToString() + ".txt";
        if(System.IO.File.Exists(filePath)){
            activeGameFile = FileManager.LoadJSON<GAMEFILE>(filePath);
            fileName.text = activeGameFile.chapterName;
            dateTime.text = "เวลาที่บันทึกไฟล์: " + activeGameFile.currentDate;
        }
        else{
            fileName.text = "ไฟล์ว่างเปล่า";
            dateTime.text = "";
        }
    }
}
