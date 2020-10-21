using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMain : MonoBehaviour
{
    // Start is called before the first frame update
    public void BackToMainFunc(){
        SceneManager.instance.LoadScene("Start");
    }
}
