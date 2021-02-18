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
    void Awake() {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Say(string speech, string speaker = ""){
        StopSpeaking();
        speaking = StartCoroutine(Speaking(speech, speaker));
            
    }

    public void StopSpeaking(){
        if(isSpeaking){
            StopCoroutine(speaking);
        }
        speaking = null;
    }

    public bool isSpeaking {get{return speaking != null;}}
    [HideInInspector] public bool isWaitingForUserInput = false;
    Coroutine speaking = null;

    IEnumerator Speaking(string targetSpeech, string speaker = ""){
        speechPanel.SetActive(true);
        speechText.text = "";
        speakerNameText.text = DetermineSpeaker(speaker);
        isWaitingForUserInput = false;

        while(speechText.text != targetSpeech){
            speechText.text += targetSpeech[speechText.text.Length];
            yield return new WaitForEndOfFrame(); 
        }

        isWaitingForUserInput = true;
        while(isWaitingForUserInput)
            yield return new WaitForEndOfFrame();

        StopSpeaking();

    }

    string DetermineSpeaker(string s){
        string retVal = speakerNameText.text;
        if(s != speakerNameText.text && s != ""){
            retVal = (s.ToLower().Contains("narrator")) ? "" : s;
        }
        return retVal;
    }

    public void Close(){
        StopSpeaking();
        speechPanel.SetActive(false);
    }

    [System.Serializable]
    public class Elements{
        public GameObject speechPanel;
        public TextMeshProUGUI speakerNameText;
        public TextMeshProUGUI speechText;
    }
    public GameObject speechPanel {get{return elements.speechPanel;}}
    public TextMeshProUGUI speakerNameText {get{return elements.speakerNameText;}}
    public TextMeshProUGUI speechText {get{return elements.speechText;}}
}
