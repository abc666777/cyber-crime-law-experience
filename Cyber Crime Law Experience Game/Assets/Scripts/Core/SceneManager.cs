using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;
    private Coroutine loadScene = null;
    private bool isLoadScene{get{return loadScene != null;}}
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
        if(isLoadScene){
            StopCoroutine(loadScene);
        }
        loadScene = StartCoroutine(_LoadScene(sceneName));
        
    }

    IEnumerator _LoadScene(string sceneName){
        Instantiate(Resources.LoadAll<GameObject>("Prefabs/Panel")[2]);
        yield return new WaitForSeconds(7f);
        AsyncOperation opt = UnitySceneManager.LoadSceneAsync(sceneName);
        while(!opt.isDone){
            yield return null;
        }
        StopCoroutine(loadScene);
        loadScene = null;
    }
}
