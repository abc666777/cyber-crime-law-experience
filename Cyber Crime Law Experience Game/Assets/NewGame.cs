using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGame : MonoBehaviour
{
    // Start is called before the first frame update
    public void NewGameFunc(){
        PlayerPrefs.SetString("Load Mode", "new");
        SceneManager.instance.LoadScene("Gameplay");
    }
}
