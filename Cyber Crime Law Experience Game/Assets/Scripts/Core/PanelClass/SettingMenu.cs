using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    private AudioMixer _MasterMixer;
    [SerializeField] private Slider master; 
    [SerializeField] private Slider bgm; 
    [SerializeField] private Slider sfx;
    void Awake() {
        _MasterMixer = Resources.Load<AudioMixer>("Audio/Mixer/Mixer");
    } 
    void Start() {
        if(PlayerPrefs.HasKey("Master Volume")){
            _MasterMixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("Master Volume")) * 20);
        }
        else{
            PlayerPrefs.SetFloat("Master Volume", 1f);
        }
        if(PlayerPrefs.HasKey("Background Music Volume")){
            _MasterMixer.SetFloat("BGMVolume", Mathf.Log10(PlayerPrefs.GetFloat("Background Music Volume")) * 20);
        }
        else{
            PlayerPrefs.SetFloat("Background Music Volume", 1f);
        }
        if(PlayerPrefs.HasKey("Sound Effect Volume")){
            _MasterMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("Sound Effect Volume")) * 20);
        }
        else{
            PlayerPrefs.SetFloat("Sound Effect Volume", 1f);
        }
        master.value = PlayerPrefs.GetFloat("Master Volume");
        bgm.value = PlayerPrefs.GetFloat("Background Music Volume");
        sfx.value = PlayerPrefs.GetFloat("Sound Effect Volume");
    }
            
    //[SerializeField] private GameObject menu;
    //public void SetActive(){
        //menu.SetActive(!menu.active);
    //}

    public void SetMasterVolume(Slider volume){
        _MasterMixer.SetFloat ("MasterVolume", Mathf.Log10(volume.value) * 20);
        PlayerPrefs.SetFloat("Master Volume", volume.value);
    }

    public void SetBGMVolume(Slider volume){
        _MasterMixer.SetFloat ("BGMVolume", Mathf.Log10(volume.value) * 20);
        PlayerPrefs.SetFloat("Background Music Volume", volume.value);
    }

    public void SetSFXVolume(Slider volume){
        _MasterMixer.SetFloat ("SFXVolume", Mathf.Log10(volume.value) * 20);
       PlayerPrefs.SetFloat("Sound Effect Volume", volume.value);
    }

}
