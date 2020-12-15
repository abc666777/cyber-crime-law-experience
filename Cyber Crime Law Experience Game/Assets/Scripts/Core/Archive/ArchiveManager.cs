using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchiveManager : MonoBehaviour
{
    [HideInInspector] public List<string> data = new List<string>();
    [HideInInspector] public GAMEDATA milestone;
    [HideInInspector] public static ArchiveManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        var textFile = Resources.Load<TextAsset>("Data/article") as TextAsset;
        data = FileManager.ArrayToList(textFile.text.Split('\n'));
        //data = FileManager.LoadFile("Assets/Resources/Data/article.txt");
        string milestoneFilePath = FileManager.dataPath + "milestone" + FileManager.fileExtension;
        if(System.IO.File.Exists(milestoneFilePath)){
            milestone = FileManager.LoadJSON<GAMEDATA>(milestoneFilePath);
        }
        else{
            milestone = new GAMEDATA();
            FileManager.SaveJSON(milestoneFilePath, milestone);
        }
        
    }
}
