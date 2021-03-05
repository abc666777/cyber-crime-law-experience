using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventSystem
{
    public static void HandleEvent(string _event, ChapterLineManager.Line.Segment segment)
    {
        if (_event.Contains("("))
        {
            string[] actions = _event.Split(' ');
            foreach (string action in actions)
            {
                if (action.Length > 1)
                    NovelController.instance.HandleAction(action);
            }
            return;
        }
        string[] eventData = _event.Split(' ');
        switch (eventData[0])
        {
            case "txtSpd":
                Event_TxtSpeed(eventData[1], segment);
                break;
            case "/txtSpd":
                segment.architect.speed = 1;
                segment.architect.charactersPerFrame = 1;
                break;
        }
    }

    static void Event_TxtSpeed(string data, ChapterLineManager.Line.Segment seg)
    {
        string[] parts = data.Split(',');
        float delay = float.Parse(parts[0]);
        int charactersPerFrame = int.Parse(parts[1]);

        seg.architect.speed = delay;
        seg.architect.charactersPerFrame = charactersPerFrame;
    }
}
