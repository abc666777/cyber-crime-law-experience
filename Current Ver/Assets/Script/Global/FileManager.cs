using UnityEngine;

public class FileManager
{
    public static string[] LoadChapterFile(string fileName)
    {
        TextAsset rawData = Resources.Load<TextAsset>(GlobalReferences.Path.StoryPath + fileName) as TextAsset;
        return rawData.ToString().Replace("[blank]", " ").Split('\n');
    }
}
