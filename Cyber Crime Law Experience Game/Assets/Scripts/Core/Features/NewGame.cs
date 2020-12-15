using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class NewGame : MonoBehaviour
{
    private AudioMixer _MasterMixer;
    void Awake() {
        _MasterMixer = Resources.Load<AudioMixer>("Audio/Mixer/Mixer");
    } 
    void Start() {
        //AudioManager.instance.PlayBGM(Resources.Load<AudioClip>("Audio/BGM/test2") as AudioClip); Change to something new.
        if(PlayerPrefs.HasKey("Master Volume")){
            _MasterMixer.SetFloat ("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("Master Volume")) * 20);
        }
        else{
            PlayerPrefs.SetFloat("Master Volume", 1f);
        }
        if(PlayerPrefs.HasKey("Background Music Volume")){
           _MasterMixer.SetFloat ("BGMVolume", Mathf.Log10(PlayerPrefs.GetFloat("Background Music Volume")) * 20);
        }
        else{
            PlayerPrefs.SetFloat("Background Music Volume", 1f);
        }
        if(PlayerPrefs.HasKey("Sound Effect Volume")){
            _MasterMixer.SetFloat ("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("Sound Effect Volume")) * 20);
        }
        else{
            PlayerPrefs.SetFloat("Sound Effect Volume", 1f);
        }
    }
    // Start is called before the first frame update
 
    public void NewGameFunc(){
        PlayerPrefs.SetString("Load Mode", "new");
        SceneManager.instance.LoadScene("Gameplay");
    }
}
