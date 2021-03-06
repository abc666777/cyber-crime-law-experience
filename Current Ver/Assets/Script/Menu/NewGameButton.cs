using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class NewGameButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject description;
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI descText;
    public int chapterIndex;

    private int[] id = new int[6];
    public void OnPointerEnter(PointerEventData eventData)
    {
        Color tempColor = description.GetComponent<Image>().color;
        tempColor.a = 0f;
        description.GetComponent<Image>().color = tempColor;
        headerText.alpha = 0f;
        descText.alpha = 0f;
        LeanTween.cancel(id[3]);
        LeanTween.cancel(id[4]);
        LeanTween.cancel(id[5]);
        id[0] = LeanTween.alpha(description.GetComponent<RectTransform>(), 0.8f, 0.3f).id;
        id[1] = LeanTween.value(headerText.gameObject, a => headerText.alpha = a, 0f, 1f, 0.3f).setDelay(0.3f).id;
        id[2] = LeanTween.value(descText.gameObject, a => descText.alpha = a, 0f, 1f, 0.3f).setDelay(0.6f).id;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color tempColor = description.GetComponent<Image>().color;
        tempColor.a = 0.8f;
        description.GetComponent<Image>().color = tempColor;
        headerText.alpha = 0.8f;
        descText.alpha = 0.8f;
        LeanTween.cancel(id[0]);
        LeanTween.cancel(id[1]);
        LeanTween.cancel(id[2]);
        id[3] = LeanTween.value(headerText.gameObject, a => headerText.alpha = a, 1f, 0f, 0.3f).id;
        id[4] = LeanTween.value(descText.gameObject, a => descText.alpha = a, 1f, 0f, 0.3f).id;
        id[5] = LeanTween.alpha(description.GetComponent<RectTransform>(), 0f, 0.3f).setDelay(0.3f).id;
    }

    public void OnClick()
    {
        GameManager.instance.currentChapterIndex = chapterIndex;
        SceneManager.instance.LoadScene(GlobalReferences.Scene.GameplayScene);
    }
}
