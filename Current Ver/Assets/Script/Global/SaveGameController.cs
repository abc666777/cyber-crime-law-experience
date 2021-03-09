using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameController : MonoBehaviour
{
    public GameObject fader;
    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenuFunction();
        }
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
}
