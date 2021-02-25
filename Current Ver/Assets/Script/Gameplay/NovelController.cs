using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NovelController : MonoBehaviour
{
    List<string> data = new List<string>();
    int progress = 0;
    private void Start()
    {
        LoadChapterFile("0");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

    void LoadChapterFile(string fileName)
    {
        //data = Resources.Load
    }
}