using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TextArchitect
{
    public string currentText { get { return _currentText; } }
    private string _currentText = "";

    public string preText;
    private string targetText;
    private int charactersPerFrame = 1;
    [Range(1f, 60f)]
    private float speed = 1f;
    private bool useEncapsulation = true;
    public bool skip = false;
    public bool isConstructing { get { return buildProcess != null; } }

    Coroutine buildProcess = null;
    public TextArchitect(string targetText, int charactersPerFrame = 1, float speed = 1f, bool useEncapsulation = true)
    {
        this.targetText = targetText;
        this.charactersPerFrame = charactersPerFrame;
        this.speed = speed;
        this.useEncapsulation = useEncapsulation;

        buildProcess = DialogueSystem.instance.StartCoroutine(Construction());
    }

    public void Stop()
    {
        if (isConstructing)
        {
            DialogueSystem.instance.StopCoroutine(buildProcess);
        }
        buildProcess = null;
    }

    IEnumerator Construction()
    {
        int runThisFrame = 0;
        string[] speechAndTags = useEncapsulation ? TagManager.SplitByTags(targetText) : new string[1] { targetText };
        _currentText = preText;
        string curText = "";
        for (int i = 0; i < speechAndTags.Length; i++)
        {
            string section = speechAndTags[i];
            bool isTag = (i & 1) != 0;
            if (isTag && useEncapsulation)
            {
                curText = _currentText;
                Encapsulated_Text encapsulation = new Encapsulated_Text(string.Format("<{0}>", section), speechAndTags, i);
                while (!encapsulation.isDone)
                {
                    bool stepped = encapsulation.Step();

                    _currentText = curText + encapsulation.displayText;

                    if (stepped)
                    {
                        runThisFrame++;
                        int maxRunsPerFrame = charactersPerFrame;
                        if (runThisFrame == maxRunsPerFrame)
                        {
                            runThisFrame = 0;
                            yield return new WaitForSeconds(0.01f * speed);
                        }
                    }
                }
                i = encapsulation.speechAndTagsArrayProgress + 1;
            }
            else
            {
                for (int j = 0; j < section.Length; j++)
                {
                    _currentText += section[j];
                    runThisFrame++;
                    int maxRunsPerFrame = charactersPerFrame;
                    if (runThisFrame == maxRunsPerFrame)
                    {
                        runThisFrame = 0;
                        yield return new WaitForSeconds(0.01f * speed);
                    }
                }
            }
        }
        buildProcess = null;
    }

    private class Encapsulated_Text
    {
        private string tag = "";
        private string endingTag = "";
        private string currentText = "";
        private string targetText = "";
        public string displayText { get { return _displayText; } }
        private string _displayText = "";
        private string[] allSpeechAndTagsArray;
        public int speechAndTagsArrayProgress { get { return arrayProgress; } }
        private int arrayProgress = 0;
        public bool isDone { get { return _isDone; } }
        private bool _isDone = false;

        public Encapsulated_Text encapsulator = null;
        public Encapsulated_Text subEncapsulator = null;
        public Encapsulated_Text(string tag, string[] allSpeechAndTagsArray, int arrayProgress)
        {
            this.tag = tag;
            GenerateEndingTag();
            this.allSpeechAndTagsArray = allSpeechAndTagsArray;
            this.arrayProgress = arrayProgress;
            if (allSpeechAndTagsArray.Length - 1 > arrayProgress)
            {
                string nextPart = allSpeechAndTagsArray[arrayProgress + 1];
                targetText = nextPart;
                /*bool isTag = ((arrayProgress + 1) & 1) != 0;
                if (!isTag)
                    targetText = nextPart;
                else
                {
                    subEncapsulator = new Encapsulated_Text(string.Format("<{0}>", nextPart), allSpeechAndTagsArray, arrayProgress + 1);
                }*/

                this.arrayProgress++;
            }
        }
        void GenerateEndingTag()
        {
            endingTag = tag.Replace("<", "").Replace(">", "");
            if (endingTag.Contains("="))
            {
                endingTag = string.Format("</{0}>", endingTag.Split('=')[0]);
            }
            else
            {
                endingTag = string.Format("</{0}>", endingTag);
            }
        }
        public bool Step()
        {
            if (isDone)
                return true;
            if (subEncapsulator != null && !subEncapsulator.isDone)
            {
                return subEncapsulator.Step();
            }
            else
            {
                if (currentText == targetText)
                {
                    if (allSpeechAndTagsArray.Length > arrayProgress + 1)
                    {
                        string nextPart = allSpeechAndTagsArray[arrayProgress + 1];
                        bool isTag = ((arrayProgress + 1) & 1) != 0;

                        if (isTag)
                        {
                            if (string.Format("<{0}>", nextPart) == endingTag)
                            {
                                _isDone = true;
                                if (encapsulator != null)
                                {
                                    string taggedText = tag + currentText + endingTag;
                                    encapsulator.currentText += taggedText;
                                    encapsulator.targetText += taggedText;

                                    UpdateArrayProgress(2);
                                }
                            }
                            else
                            {
                                subEncapsulator = new Encapsulated_Text(string.Format("<{0}>", nextPart), allSpeechAndTagsArray, arrayProgress + 1);
                                subEncapsulator.encapsulator = this;
                                UpdateArrayProgress();
                            }
                        }
                        else
                        {
                            targetText += nextPart;
                            UpdateArrayProgress();
                        }
                    }
                    else
                    {
                        _isDone = true;

                    }
                }
                else
                {
                    currentText += targetText[currentText.Length];
                    UpdateDisplay("");
                    return true;
                }
            }
            return false;
        }

        void UpdateArrayProgress(int val = 1)
        {
            arrayProgress += val;
            if (encapsulator != null)
                encapsulator.UpdateArrayProgress(val);
        }
        void UpdateDisplay(string subValue)
        {
            _displayText = string.Format("{0}{1}{2}{3}", tag, currentText, subValue, endingTag);
            if (encapsulator != null)
                encapsulator.UpdateDisplay(displayText);
        }
    }
}
