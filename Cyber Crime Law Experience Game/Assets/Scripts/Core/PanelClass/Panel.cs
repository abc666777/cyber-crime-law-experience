using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public void Close(){
        Destroy(gameObject);
    }

    public void BackToMainMenu(){
        SceneManager.instance.LoadScene("Start");
    }
}
