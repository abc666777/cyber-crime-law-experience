using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingController : MonoBehaviour
{
    public GameObject fader;
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    private AudioMixer masterMixer;

    public TextMeshProUGUI masterText;
    public TextMeshProUGUI bgmText;
    public TextMeshProUGUI sfxText;
    // Start is called before the first frame update
    void Awake()
    {
        Init();
        masterMixer = AssetsLoader.instance.masterMixer;
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
            masterText.text = "[" + (masterSlider.value * 100).ToString("000.00") + "]";
        }
        else
        {
            masterSlider.value = 1;
            masterText.text = "[" + "100.00" + "]";
        }

        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume");
            bgmText.text = "[" + (bgmSlider.value * 100).ToString("000.00") + "]";
        }
        else
        {
            bgmSlider.value = 1;
            bgmText.text = "[" + "100.00" + "]";
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            sfxText.text = "[" + (sfxSlider.value * 100).ToString("000.00") + "]";
        }
        else
        {
            sfxSlider.value = 1;
            sfxText.text = "[" + "100.00" + "]";
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenuFunction();
        }
        if (masterText.text == "[001.00]") masterText.text = "[000.00]";
        if (bgmText.text == "[001.00]") bgmText.text = "[000.00]";
        if (sfxText.text == "[001.00]") sfxText.text = "[000.00]";
    }

    void Init()
    {
        LeanTween.alpha(fader.GetComponent<RectTransform>(), 0f, 0.3f);
    }

    public void BackToMenuFunction()
    {
        DestroyThisPanel();
    }

    private void DestroyThisPanel()
    {
        Destroy(gameObject);
    }

    public void SetMasterVolume(Slider volume)
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(volume.value) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume.value);
        masterText.text = "[" + (volume.value * 100).ToString("000.00") + "]";
    }

    public void SetBGMVolume(Slider volume)
    {
        masterMixer.SetFloat("BGMVolume", Mathf.Log10(volume.value) * 20);
        PlayerPrefs.SetFloat("BGMVolume", volume.value);
        bgmText.text = "[" + (volume.value * 100).ToString("000.00") + "]";
    }

    public void SetSFXVolume(Slider volume)
    {
        masterMixer.SetFloat("SFXVolume", Mathf.Log10(volume.value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume.value);
        sfxText.text = "[" + (volume.value * 100).ToString("000.00") + "]";
    }
}
