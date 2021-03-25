using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterLineManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static Line Interpret(string rawLine)
    {
        return new Line(rawLine);
    }
    public class Line
    {
        public string speaker = "";
        public List<Segment> segments = new List<Segment>();
        public List<string> actions = new List<string>();

        public string LastSegmentsWholeDialogue = "";
        public Line(string rawLine)
        {
            string[] dialogueAndActions = rawLine.Split('"');
            char actionSplitter = ' ';
            string[] actionsArr = dialogueAndActions.Length == 3 ? dialogueAndActions[2].Split(actionSplitter) : dialogueAndActions[0].Split(actionSplitter);

            if (dialogueAndActions.Length == 3)
            {
                speaker = dialogueAndActions[0] == "" ? NovelController.instance.cachedLastSpeaker : dialogueAndActions[0];
                if (speaker[speaker.Length - 1] == ' ')
                    speaker = speaker.Remove(speaker.Length - 1);
                speaker = speaker.Replace("_", " ");
                NovelController.instance.cachedLastSpeaker = speaker;

                SegmentDialogue(dialogueAndActions[1]);
            }
            foreach (string action in actionsArr)
            {
                actions.Add(action);
            }
        }
        void SegmentDialogue(string dialogue)
        {
            segments.Clear();
            string[] parts = dialogue.Split('{', '}');
            for (int i = 0; i < parts.Length; i++)
            {
                Segment segment = new Segment();
                bool isOdd = i % 2 != 0;

                if (isOdd)
                {
                    string[] commandData = parts[i].Split(' ');
                    switch (commandData[0])
                    {
                        case "c":
                            segment.trigger = Segment.Trigger.waitClick;
                            break;
                        case "w":
                            segment.trigger = Segment.Trigger.autoDelay;
                            segment.autoDelay = float.Parse(commandData[1]);
                            break;
                        default:
                            break;
                    }
                    i++;

                }

                segment.dialogue = parts[i];
                segment.line = this;

                segments.Add(segment);
            }
        }
        public class Segment
        {
            public Line line;
            public string dialogue = "";
            public string preText = "";
            public enum Trigger { waitClick, autoDelay }
            public Trigger trigger = Trigger.waitClick;
            public float autoDelay = 0;

            public void Run()
            {
                if (running != null)
                    NovelController.instance.StopCoroutine(running);
                running = NovelController.instance.StartCoroutine(Running());
            }
            public bool isRunning { get { return running != null; } }
            Coroutine running = null;
            public TextArchitect architect = null;
            List<string> allCurrentlyExecutedEvents = new List<string>();
            IEnumerator Running()
            {
                allCurrentlyExecutedEvents.Clear();
                TagManager.Inject(ref dialogue);
                //Debug.Log(dialogue);
                string[] parts = dialogue.Split('[', ']');
                for (int i = 0; i < parts.Length; i++)
                {
                    bool isOdd = i % 2 != 0;
                    if (isOdd)
                    {
                        DialogueEventSystem.HandleEvent(parts[i], this);
                        allCurrentlyExecutedEvents.Add(parts[i]);
                        i++;
                    }
                    string targDialogue = parts[i];
                    if (targDialogue.Length < 1)
                        continue;

                    if (!line.speaker.Contains("narrator"))
                    {
                        string[] trueSpeaker = line.speaker.Split('-');
                        if (trueSpeaker.Length > 1)
                        {
                            Character c = CharacterManager.instance.GetCharacter(trueSpeaker[0]);
                            c.Say(targDialogue);
                            DialogueSystem.instance.speakerNameText.text = trueSpeaker[1];
                        }
                        else
                        {
                            Character c = CharacterManager.instance.GetCharacter(trueSpeaker[0]);
                            c.Say(targDialogue);
                        }
                    }
                    else
                    {
                        string[] trueSpeaker = line.speaker.Split('-');
                        if (trueSpeaker.Length > 1)
                        {
                            DialogueSystem.instance.Say(targDialogue, preText != "", trueSpeaker[0]);
                            DialogueSystem.instance.speakerNameText.text = trueSpeaker[1];
                        }
                        else
                        {
                            DialogueSystem.instance.Say(targDialogue, preText != "", trueSpeaker[0]);
                        }
                    }
                    //Debug.Log(DialogueSystem.instance.textArchitect == null);
                    architect = DialogueSystem.instance.textArchitect;

                    while (architect.isConstructing)
                        yield return new WaitForEndOfFrame();
                }
                running = null;

            }

            public void ForceFinish()
            {
                if (running != null)
                    NovelController.instance.StopCoroutine(running);
                running = null;

                if (architect != null)
                {
                    architect.ForceFinish();
                    if (preText == "")
                        line.LastSegmentsWholeDialogue = "";
                    string[] parts = dialogue.Split('[', ']');
                    for (int i = 0; i < parts.Length; i++)
                    {
                        bool isOdd = i % 2 != 0;
                        if (isOdd)
                        {
                            string e = parts[i];
                            if (allCurrentlyExecutedEvents.Contains(e))
                                allCurrentlyExecutedEvents.Remove(e);
                            else
                                DialogueEventSystem.HandleEvent(e, this);
                            i++;
                        }
                        line.LastSegmentsWholeDialogue += parts[i];
                    }
                    architect.ShowText(line.LastSegmentsWholeDialogue);
                }
            }
        }
    }
}
