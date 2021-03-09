using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    [HideInInspector] public int currentChapterIndex = -1;
    public List<string> checkpoints = new List<string>();
    public enum LoadMode
    {
        newGame,
        loadGame
    }
    public LoadMode currentMode = LoadMode.newGame;

    public int SaveIndex;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
}
