using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    [HideInInspector] public int currentChapterIndex = -1;
    [HideInInspector] public List<string> checkpoints = new List<string>();
    public AudioMixer masterMixer;
    public enum LoadMode
    {
        newGame,
        loadGame
    }
    [HideInInspector] public LoadMode currentMode = LoadMode.newGame;

    [HideInInspector] public int SaveIndex;
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
    private void Start()
    {
        LoadVolume();
    }

    public void LoadVolume()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
            masterMixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20);
        if (PlayerPrefs.HasKey("BGMVolume"))
            masterMixer.SetFloat("BGMVolume", Mathf.Log10(PlayerPrefs.GetFloat("BGMVolume")) * 20);
        if (PlayerPrefs.HasKey("SFXVolume"))
            masterMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
    }
}
