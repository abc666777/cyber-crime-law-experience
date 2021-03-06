using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NovelController : MonoBehaviour
{
    public static NovelController instance;
    [HideInInspector]
    private bool isAuto = false;
    List<string> data = new List<string>();
    string[] startChptr = new string[]
    {
        "episode1_0"
    };

    //int progress = 0;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        GameManager.instance.checkpoints = new List<string>();
        LoadChapterFile(startChptr[GameManager.instance.currentChapterIndex]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Next();
        }
    }

    void LoadChapterFile(string fileName)
    {
        data = FileManager.LoadChapterFile(fileName).ToList();
        //progress = 0;
        cachedLastSpeaker = "";
        if (handlingChapterFile != null)
            StopCoroutine(handlingChapterFile);
        handlingChapterFile = StartCoroutine(HandlingChapterFile());

        Next();
    }

    bool _next = false;
    public void Next()
    {
        _next = true;
    }
    public bool isHandlingChapterFile { get { return handlingChapterFile != null; } }
    Coroutine handlingChapterFile = null;
    [HideInInspector] public int chapterProgress = 0;
    IEnumerator HandlingChapterFile()
    {
        chapterProgress = 0;
        while (chapterProgress < data.Count)
        {
            if (_next)
            {
                string line = data[chapterProgress];
                if (line.StartsWith("choice"))
                {
                    yield return HandlingChoiceLine(line);
                    chapterProgress++;
                }
                else
                {
                    HandleLine(data[chapterProgress]);
                    chapterProgress++;
                    while (isHandlingLine)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }
        handlingChapterFile = null;
    }

    IEnumerator HandlingChoiceLine(string line)
    {
        //string title = line.Split('"')[1];
        List<string> choices = new List<string>();
        List<string> actions = new List<string>();
        bool gatheringChoices = true;
        while (gatheringChoices)
        {
            chapterProgress++;
            line = data[chapterProgress];

            if (line.Contains("{"))
            {
                continue;
            }

            line = line.Replace("    ", "");

            if (line != "}")
            {
                Debug.Log(line);
                choices.Add(line.Split('"')[1]);
                actions.Add(data[chapterProgress + 1].Replace("    ", ""));
                chapterProgress++;
            }
            else if (line.Contains("}"))
            {
                gatheringChoices = false;
            }
        }

        if (choices.Count > 0)
        {
            ChoiceScreen.Show(choices.ToArray()); yield return new WaitForEndOfFrame();
            while (ChoiceScreen.isWaitingForChoiceToBeMade)
                yield return new WaitForEndOfFrame();
            string action = actions[ChoiceScreen.lastChoiceMade.index];
            HandleLine(action);

            while (isHandlingLine)
                yield return new WaitForEndOfFrame();
        }
        else Debug.LogError("ERROR: Something went wrong with your choice. (Script)");

    }

    void HandleLine(string rawLine)
    {
        ChapterLineManager.Line line = ChapterLineManager.Interpret(rawLine);
        StopHandlingLine();
        handlingLine = StartCoroutine(HandlingLine(line));
    }

    void StopHandlingLine()
    {
        if (isHandlingLine)
            StopCoroutine(handlingLine);
        handlingLine = null;
    }
    public bool isHandlingLine { get { return handlingLine != null; } }
    Coroutine handlingLine = null;
    IEnumerator HandlingLine(ChapterLineManager.Line line)
    {
        _next = false;
        int lineProgress = 0;
        while (lineProgress < line.segments.Count)
        {
            _next = false;
            ChapterLineManager.Line.Segment segment = line.segments[lineProgress];
            if (lineProgress > 0)
            {
                if (segment.trigger == ChapterLineManager.Line.Segment.Trigger.autoDelay)
                {
                    for (float timer = segment.autoDelay; timer >= 0; timer -= Time.deltaTime)
                    {
                        yield return new WaitForEndOfFrame();
                        if (_next)
                            break;
                    }
                }
                else
                {
                    while (!_next)
                        yield return new WaitForEndOfFrame();
                }
            }
            _next = false;
            segment.Run();
            while (segment.isRunning)
            {
                yield return new WaitForEndOfFrame();
                if (_next)
                {
                    segment.ForceFinish();
                }
                _next = false;
            }

            lineProgress++;

            yield return new WaitForEndOfFrame();
        }
        foreach (string action in line.actions)
        {
            if (action.Length > 1)
                HandleAction(action);
        }
        handlingLine = null;
    }
    [HideInInspector]
    public string cachedLastSpeaker = "";

    public void HandleAction(string action)
    {
        string[] data = action.Split('(', ')');
        switch (data[0])
        {
            case "setBackground":
                Command_SetLayerImage(data[1], BackgroundManager.instance.background);
                return;
            case "setCinematic":
                Command_SetLayerImage(data[1], BackgroundManager.instance.cinematic);
                return;
            case "setForeground":
                Command_SetLayerImage(data[1], BackgroundManager.instance.foreground);
                return;
            case "playSFX":
                Command_PlaySFX(data[1]);
                return;
            case "playBGM":
                Command_PlayBGM(data[1]);
                return;
            case "move":
                Command_MoveCharacter(data[1]);
                return;
            case "setPosition":
                Command_SetPosition(data[1]);
                return;
            case "setExpression":
                Command_SetExpression(data[1]);
                return;
            case "enter":
                Command_Enter(data[1]);
                return;
            case "exit":
                Command_Exit(data[1]);
                return;
            case "prepare":
                Command_PrepareCharacter(data[1]);
                return;
            case "transBackground":
                Command_TransLayer(BackgroundManager.instance.background, data[1]);
                return;
            case "transCinematic":
                Command_TransLayer(BackgroundManager.instance.cinematic, data[1]);
                return;
            case "transForeground":
                Command_TransLayer(BackgroundManager.instance.foreground, data[1]);
                return;
            case "showScene":
                Command_ShowScene(data[1]);
                return;
            case "load":
                Command_Load(data[1]);
                return;
            case "addCheckPoint":
                Command_AddCheckPoint(data[1]);
                return;
            case "ending":
                SceneManager.instance.LoadScene(GlobalReferences.Scene.EndingScene);
                return;
            default:
                Debug.LogError("ERROR: command " + data[0] + "does not exist. (Command not found)");
                return;
        }
    }

    #region command
    private void Command_SetLayerImage(string data, BackgroundManager.Layer layer)
    {
        string texName = data.Contains(",") ? data.Split(',')[0] : data;
        Texture2D tex = AssetsLoader.instance.GetBackground(texName);
        float spd = 2f;
        bool smth = false;
        if (data.Contains(","))
        {
            string[] parameters = data.Split(',');
            foreach (string p in parameters)
            {
                float fVal = 0;
                bool bVal = false;
                if (float.TryParse(p, out fVal))
                {
                    spd = fVal; continue;
                }
                if (bool.TryParse(p, out bVal))
                {
                    smth = bVal; continue;
                }
            }
        }
        layer.TransitionToTexture(tex, spd, smth);
    }

    private void Command_PlaySFX(string name)
    {
        AudioManager.instance.PlaySFX(AssetsLoader.instance.AudioLoader(name, "SFX"));
    }

    private void Command_PlayBGM(string name)
    {
        AudioManager.instance.PlayBGM(AssetsLoader.instance.AudioLoader(name, "BGM"));
    }
    private void Command_MoveCharacter(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        float locationX = float.Parse(parameters[1]);
        float locationY = parameters.Length >= 3 ? float.Parse(parameters[2]) : 0;
        float speed = parameters.Length >= 4 ? float.Parse(parameters[3]) : 1f;
        bool smooth = parameters.Length == 5 ? bool.Parse(parameters[4]) : false;

        Character c = CharacterManager.instance.GetCharacter(character);
        c.MoveTo(new Vector2(locationX, locationY), speed, smooth);
    }
    private void Command_SetPosition(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        float locationX = float.Parse(parameters[1]);
        float locationY = parameters.Length >= 3 ? float.Parse(parameters[2]) : 0;

        Character c = CharacterManager.instance.GetCharacter(character);
        c.SetPosition(new Vector2(locationX, locationY));
    }
    private void Command_SetExpression(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        string expression = parameters[1];
        float speed = parameters.Length >= 3 ? float.Parse(parameters[2]) : 2f;
        bool smooth = parameters.Length == 4 ? bool.Parse(parameters[3]) : false;

        Character c = CharacterManager.instance.GetCharacter(character);
        c.TransitionExpression(c.GetSprite(expression), speed, smooth);
    }

    private void Command_Exit(string data)
    {
        string[] parameters = data.Split(',');
        string[] characters = parameters[0].Split(';');
        float speed = 5f;
        bool smooth = false;
        for (int i = 1; i < parameters.Length; i++)
        {
            float fVal = 0; bool bVal = false;
            if (float.TryParse(parameters[i], out fVal))
            { speed = fVal; continue; }
            if (bool.TryParse(parameters[i], out bVal))
            { smooth = bVal; continue; }
        }

        foreach (string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s);
            c.FadeOut(speed, smooth);
        }
    }
    private void Command_Enter(string data)
    {
        string[] parameters = data.Split(',');
        string[] characters = parameters[0].Split(';');
        float speed = 5f;
        bool smooth = false;
        for (int i = 1; i < parameters.Length; i++)
        {
            float fVal = 0; bool bVal = false;
            if (float.TryParse(parameters[i], out fVal))
            { speed = fVal; continue; }
            if (bool.TryParse(parameters[i], out bVal))
            { smooth = bVal; continue; }
        }

        foreach (string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s, true, true);
            if (!c.enabled)
            {
                c.renderers.renderer.color = Color.white;
                c.enabled = true;
            }
            c.FadeIn(speed, smooth);
        }
    }

    private void Command_PrepareCharacter(string data)
    {
        string[] characters = data.Split(';');
        foreach (string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s, true, false);
        }

    }

    private void Command_TransLayer(BackgroundManager.Layer layer, string data)
    {
        string[] parameters = data.Split(',');
        string texName = parameters[0];
        string transTexName = parameters[1];
        Texture2D tex = AssetsLoader.instance.GetBackground(texName);
        Texture2D transTex = AssetsLoader.instance.GetTransitionEffects(transTexName);

        float spd = 1f;
        bool smth = false;

        for (int i = 2; i < parameters.Length; i++)
        {
            string p = parameters[i];
            float fVal = 0;
            bool bVal = false;
            if (float.TryParse(p, out fVal))
            { spd = fVal; continue; }
            if (bool.TryParse(p, out bVal))
            { smth = bVal; continue; }
        }
        TransitionManager.TransitionLayer(layer, tex, transTex, spd, smth);
    }
    private void Command_ShowScene(string data)
    {
        string[] parameters = data.Split(',');
        bool show = bool.Parse(parameters[0]);
        string texName = parameters[1];
        Texture2D transTex = AssetsLoader.instance.GetTransitionEffects(texName);
        float spd = 2f;
        bool smth = false;

        for (int i = 2; i < parameters.Length; i++)
        {
            string p = parameters[i];
            float fVal = 0;
            bool bVal = false;
            if (float.TryParse(p, out fVal))
            { spd = fVal; continue; }
            if (bool.TryParse(p, out bVal))
            { smth = bVal; continue; }
        }
        TransitionManager.ShowScene(show, spd, smth, transTex);
    }

    void Command_Load(string chapterName)
    {
        NovelController.instance.LoadChapterFile(chapterName);
    }

    void Command_AddCheckPoint(string checkPoint)
    {
        GameManager.instance.checkpoints.Add(checkPoint);
        //Add CheckPont to The Scene
    }
    #endregion
}