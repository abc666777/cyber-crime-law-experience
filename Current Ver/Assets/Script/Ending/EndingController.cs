using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndingController : MonoBehaviour
{
    // Start is called before the first frame update    public TextMeshProUGUI description;
    // DECOR
    public TextMeshProUGUI title;
    public GameObject mainPanel;
    public GameObject sentencesPanel;
    public GameObject choicesPanelandPlayerSentencesPanel;
    //
    private TextAsset checkpointData;
    private string[] data;
    private List<string> sentences = new List<string>();
    private List<string> choices = new List<string>();
    private List<string> playerSentences = new List<string>();
    // -- ----------------------------------------------------------------------------------
    public TextMeshProUGUI sentencesText;
    public TextMeshProUGUI choicesTextandPlayerSentencesText;

    public GameObject nextButton;
    // Start is called before the first frame update
    void Awake()
    {
        title.color = new Color(title.color.r, title.color.g, title.color.b, 0);
        LeanTween.scale(sentencesPanel.GetComponent<RectTransform>(), new Vector3(0.001f, 1, 0), 0f);
        sentencesPanel.SetActive(false);
        LeanTween.scale(choicesPanelandPlayerSentencesPanel.GetComponent<RectTransform>(), new Vector3(0.0001f, 1, 0), 0f);
        choicesPanelandPlayerSentencesPanel.SetActive(false);
        LeanTween.scale(mainPanel.GetComponent<RectTransform>(), new Vector3(0, 0, 0), 0f).setOnComplete(TweenMainPanelSide);
        //LeanTween.alpha(fader.GetComponent<RectTransform>(), 0f, 0.3f);
        checkpointData = AssetsLoader.instance.checkpointData;
        data = checkpointData.ToString().Replace("[blank]", " ").Split('\n');
        nextButton.SetActive(false);
    }
    private void TweenMainPanelSide()
    {
        LeanTween.scale(mainPanel.GetComponent<RectTransform>(), new Vector3(1, 0.001f, 1), 0.2f).setOnComplete(TweenMainPanelUp);
    }

    private void TweenMainPanelUp()
    {
        LeanTween.scale(mainPanel.GetComponent<RectTransform>(), new Vector3(1, 1, 1), 0.3f);
        LeanTween.delayedCall(1.5f, ChangeMainTextColor);
    }

    private void ChangeMainTextColor()
    {
        LeanTween.value(title.gameObject, a => title.alpha = a, 0, 1f, 3f).setOnComplete(TweenFirstSubPanelSide);
    }
    private void TweenFirstSubPanelSide()
    {
        sentencesPanel.SetActive(true);
        LeanTween.scale(sentencesPanel.GetComponent<RectTransform>(), new Vector3(1, 1, 1), 0.2f).setOnComplete(CallFirstCoroutine);
    }

    private void TweenSecondSubPanelSide()
    {
        choicesPanelandPlayerSentencesPanel.SetActive(true);
        LeanTween.scale(choicesPanelandPlayerSentencesPanel.GetComponent<RectTransform>(), new Vector3(1, 1, 1), 0.2f).setOnComplete(CallSecondCoroutine);
    }

    private void CallFirstCoroutine()
    {
        settingDscTxt = StartCoroutine(SetDscTxt(sentencesText));
    }

    private void CallSecondCoroutine()
    {
        settingDscTxt = StartCoroutine(SetDscTxt(choicesTextandPlayerSentencesText));
    }
    string targetText = "";
    Coroutine settingDscTxt = null;
    IEnumerator SetDscTxt(TextMeshProUGUI txt)
    {
        if (txt.name == "SentenceText")
        {
            foreach (string _dt in data) //Loop in data
            {
                foreach (string dt in GameManager.instance.checkpoints) //Loop in Checkpoint
                {
                    if (_dt.StartsWith(dt))
                    {
                        if (_dt.Contains("<<sentence>>"))
                        {
                            sentences.Add(_dt.Replace("<<sentence>>", "").Split(':')[1]);
                        }
                    }
                }
            }
            foreach (string st in sentences)
            {
                targetText += (st + "<br>");
            }
        }
        else
        {
            foreach (string _dt in data) //Loop in data
            {
                foreach (string dt in GameManager.instance.checkpoints) //Loop in Checkpoint
                {
                    if (_dt.StartsWith(dt))
                    {
                        if (_dt.Contains("<<choice>>"))
                        {
                            choices.Add(_dt.Replace("<<choice>>", "").Split(':')[1]);
                        }
                        else if (_dt.Contains("<<player>>"))
                        {
                            playerSentences.Add(_dt.Replace("<<player>>", "").Split(':')[1]);
                        }
                    }
                }
            }
            if (playerSentences.Count == 0)
                playerSentences.Add("<size=200%><b><color=#6FCBBF>คุณไม่ได้กระทำความผิดใด ๆ ในบทนี้</color></b></size>");

            foreach (string st in choices)
            {
                targetText += (st + "<br>");
            }
            foreach (string st in playerSentences)
            {
                targetText += (st + "<br>");
            }
        }
        txt.text = "";
        txt.text += targetText;
        txt.ForceMeshUpdate();
        int runsThisFrame = 0;

        TMP_TextInfo inf = txt.textInfo;
        int vis = 0;

        inf = txt.textInfo;
        int max = inf.characterCount;

        txt.maxVisibleCharacters = vis;

        int cpf = 1;

        while (vis < max)
        {
            //reveal a certain number of characters per frame.
            while (runsThisFrame < cpf)
            {
                vis++;
                txt.maxVisibleCharacters = vis;
                runsThisFrame++;
            }

            //wait for the next available revelation time.
            runsThisFrame = 0;
            yield return new WaitForSeconds(0.03f);
        }
        targetText = "";
        if (txt.name == "SentenceText") TweenSecondSubPanelSide();
        else nextButton.SetActive(true);
        settingDscTxt = null;
    }

    public void ToMenu()
    {
        AudioManager.instance.PlayBGM(null);
        SceneManager.instance.LoadScene(GlobalReferences.Scene.StartScene);
    }
}
