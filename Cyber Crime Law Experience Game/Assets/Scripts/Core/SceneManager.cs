using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;
    // Start is called before the first frame update
    void Start()
    {
        if(instance != null && instance != this){
            Destroy(this.gameObject);
            return;
        }
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void LoadScene(string sceneName){
        AsyncOperation opt = UnitySceneManager.LoadSceneAsync(sceneName);
    }

    public void NewGame(){
        PlayerPrefs.SetString("Load Mode", "new");
        AsyncOperation opt = UnitySceneManager.LoadSceneAsync("Gameplay");
    }
}
