using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;
    public Elements elements;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame

    public void Say(string speech, bool additive = false, string speaker = "")
    {
        StopSpeaking();
        if (additive)
            speechText.text = targetSpeech;
        speaking = StartCoroutine(Speaking(speech, additive, speaker));

    }

    public void StopSpeaking()
    {
        if (isSpeaking)
        {
            StopCoroutine(speaking);
        }
        if (textArchitect != null && textArchitect.isConstructing)
        {
            textArchitect.Stop();
        }
        speaking = null;
    }

    public bool isSpeaking { get { return speaking != null; } }
    [HideInInspector] public bool isWaitingForUserInput = false;
    Coroutine speaking = null;
    public string targetSpeech = "";
    public TextArchitect textArchitect = null;
    IEnumerator Speaking(string speech, bool additive, string speaker = "")
    {
        //Debug.Log(speech);
        speechPanel.SetActive(true);
        string additiveSpeech = additive ? speechText.text : "";
        targetSpeech = additiveSpeech + speech;
        if (textArchitect == null)
            textArchitect = new TextArchitect(speechText, speech, additiveSpeech);
        else
            textArchitect.Renew(speech, additiveSpeech);
        //speechText.text = "";
        speakerNameText.text = DetermineSpeaker(speaker);
        isWaitingForUserInput = false;

        while (textArchitect.isConstructing)
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //textArchitect.ForceFinish();

            //speechText.text = speech;
            //speechText.maxVisibleCharacters = speechText.textInfo.characterCount;
            yield return new WaitForEndOfFrame();
        }
        isWaitingForUserInput = true;
        while (isWaitingForUserInput)
            yield return new WaitForEndOfFrame();

        StopSpeaking();

    }

    string DetermineSpeaker(string s)
    {
        string retVal = speakerNameText.text;
        if (s != speakerNameText.text && s != "")
        {
            retVal = (s.ToLower().Contains("narrator")) ? "" : s;
        }
        return retVal;
    }

    public void Close()
    {
        StopSpeaking();
        speechPanel.SetActive(false);
    }

    [System.Serializable]
    public class Elements
    {
        public GameObject speechPanel;
        public TextMeshProUGUI speakerNameText;
        public TextMeshProUGUI speechText;
    }
    public GameObject speechPanel { get { return elements.speechPanel; } }
    public TextMeshProUGUI speakerNameText { get { return elements.speakerNameText; } }
    public TextMeshProUGUI speechText { get { return elements.speechText; } }
}
