using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;
    public ELEMENTS elements;


    void Awake()
    {
        instance = this;
    }
    
    void Update() {
    }

    public void Say(string speech, string speaker = "", bool additive = false){
        StopSpeaking();
        if(additive) speechText.text = targetSpeech;
        speaking = StartCoroutine(Speaking(speech, additive, speaker));
    }

    public void StopSpeaking(){
        if(isSpeaking){
            StopCoroutine(speaking);
        }
        if(textArchitech != null && textArchitech.isConstructing){
            textArchitech.Stop();
        }
        speaking = null;
    }

    public bool isSpeaking {get{return speaking != null;}}
    [HideInInspector] public bool isWaitingForUserInput = false;

    [HideInInspector] public string targetSpeech = "";
    public Coroutine speaking = null;
    private TextArchitect textArchitech = null;
    public TextArchitect currentArchitect{get{return textArchitech;}}
    IEnumerator Speaking(string speech, bool additive, string speaker = ""){
        speechPanel.SetActive(true);
        string additiveSpeech = additive ? speechText.text : "";
        targetSpeech = additiveSpeech + speech;
        if(textArchitech == null){
            textArchitech = new TextArchitect(speechText, targetSpeech, additiveSpeech);
        }
        else{
            textArchitech.Renew(speech, additiveSpeech);
        }
        speakerNameText.text = DetermineSpeaker(speaker);
        speakerNamePanel.SetActive(speakerNameText.text != "");
        isWaitingForUserInput = false;

        while(textArchitech.isConstructing){
            /*if(Input.GetKey(KeyCode.Space)){
                textArchitech.skip = true;
            }*/

            yield return new WaitForEndOfFrame();
        }

        isWaitingForUserInput = true;
        while (isWaitingForUserInput) 
        {
            yield return new WaitForEndOfFrame();
        }
        StopSpeaking();
    }

    string DetermineSpeaker(string name)
    {
        string tempName = speakerNameText.text;
        if(name != speakerNameText.text && name != ""){
            tempName = (name.ToLower().Contains("narrator")) ? "" : name;
        }
        return tempName;
    }

    public void Close(){
        StopSpeaking();
        speechPanel.SetActive(false);
    }

    public void Open(){
        StopSpeaking();
        speechPanel.SetActive(true);
    }

    [System.Serializable]
    public class ELEMENTS{
        public GameObject speechPanel;
        public GameObject speakerNamePanel;
        public TextMeshProUGUI speakerNameText;
        public TextMeshProUGUI speechText;
    }
    public GameObject speechPanel {get{return elements.speechPanel;}}
    public GameObject speakerNamePanel {get{return elements.speakerNamePanel;}}
    public TextMeshProUGUI speakerNameText {get{return elements.speakerNameText;}}
    public TextMeshProUGUI speechText {get{return elements.speechText;}}
}
