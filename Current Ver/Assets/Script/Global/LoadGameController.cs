﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameController : MonoBehaviour
{
    public GameObject fader;
    public static LoadGameController instance;
    // Start is called before the first frame update
    void Awake()
    {
        Init();
        instance = this;
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
