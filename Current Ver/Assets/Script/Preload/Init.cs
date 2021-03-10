using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Init : MonoBehaviour
{
    public GameObject loadingImage;
    public TextMeshProUGUI loadingText;
    // Start is called before the first frame update
    void Start()
    {
        RotateImage();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Initialize());
    }

    void RotateImage()
    {
        LeanTween.rotate(loadingImage.GetComponent<RectTransform>(), 360f, 2f).setOnComplete(RotateImage);
    }

    IEnumerator Initialize()
    {
        yield return new WaitForSeconds(5f);
        loadingText.text = "LOADING COMPLETE";
        yield return new WaitForSeconds(1f);
        loadingText.gameObject.SetActive(false);
        loadingImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        SceneManager.instance.LoadScene(GlobalReferences.Scene.StartScene);

    }
}
