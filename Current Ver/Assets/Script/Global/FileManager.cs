using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;


public class FileManager
{
    public static string[] LoadChapterFile(string fileName)
    {
        TextAsset rawData = Resources.Load<TextAsset>(GlobalReferences.Path.StoryPath + fileName) as TextAsset;
        return rawData.ToString().Replace("[blank]", " ").Split('\n');
    }
    public static void SaveJSON(string filePath, object classToSave)
    {
        string jsonString = JsonUtility.ToJson(classToSave);
        SaveFile(filePath, jsonString);
    }

    public static T LoadJSON<T>(string filePath)
    {
        string jsonString = LoadFile(filePath)[0];
        return JsonUtility.FromJson<T>(jsonString);
    }

    public static void SaveFile(string filePath, List<string> lines)
    {
        StreamWriter sw = new StreamWriter(filePath);
        int i = 0;
        for (i = 0; i < lines.Count; i++)
        {
            sw.WriteLine(lines[i]);
        }
        sw.Close();
    }

    public static List<string> LoadFile(string filePath, bool removeBlankLines = true)
    {
        List<string> lines = File.ReadAllLines(filePath).ToList();
        return lines;
    }

    public static void SaveFile(string filePath, string line)
    {
        Debug.Log(filePath);
        SaveFile(filePath, new List<string>() { line });
    }
}
