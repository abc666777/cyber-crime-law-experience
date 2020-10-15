using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelController : MonoBehaviour
{   
    public static NovelController instance;
    List<string> data = new List<string>();
    //int progress = 0;

    //string _line = null;
    // Start is called before the first frame update
    void Awake() {
        instance = this;
    }
    int activeGameFileNumber = 0;
    GAMEFILE activeGameFile = null;
    string activeChapterFile = "";
    void Start()
    {
        //set mode
        switch(PlayerPrefs.GetString("Load Mode")){
            case("new"):
                activeGameFile = new GAMEFILE();
                LoadChapterFile("test");
                break;
            case("load"):
                LoadGameFile(PlayerPrefs.GetInt("Load File"));
                break;
        }
    }

    public void LoadGameFile(int gameFileNumber){
        activeGameFileNumber = gameFileNumber;
        string filePath = FileManager.savPath + "Resources/gameFiles/" + gameFileNumber.ToString() + ".txt";

        activeGameFile = FileManager.LoadJSON<GAMEFILE>(filePath);

        data = FileManager.LoadFile("Assets/Resources/Story/" + activeGameFile.chapterName);
        activeChapterFile = activeGameFile.chapterName;
        cachedLastSpeaker = activeGameFile.cachedLastSpeaker;
        
        for(int i = 0; i < activeGameFile.charactersInScene.Count; i++){
            GAMEFILE.CHARACTERDATA data = activeGameFile.charactersInScene[i];
            Character character = CharacterManager.instance.GetCharacter(data.characterName, data.enabled);
            character.SetTexture(data.expression);
            if(data.facingRight)
                character.FaceRight();
            else
                character.FaceLeft();
            character.SetPosition(data.position);
        }

        if(activeGameFile.background != null)
            BackgroundManager.instance.background.SetTexture(activeGameFile.background);
        else
            BackgroundManager.instance.background.SetTexture(null);
        if(activeGameFile.cinematic != null)
            BackgroundManager.instance.cinematic.SetTexture(activeGameFile.cinematic);
        else
            BackgroundManager.instance.cinematic.SetTexture(null);
        if(activeGameFile.foreground != null)
            BackgroundManager.instance.foreground.SetTexture(activeGameFile.foreground);
        else
            BackgroundManager.instance.foreground.SetTexture(null);

        if(activeGameFile.music != null)
            AudioManager.instance.PlayBGM(activeGameFile.music);
        else
            AudioManager.instance.PlayBGM(null);

        if(handlingChapterFile != null){
            StopCoroutine(handlingChapterFile);
        }
        chapterProgress = activeGameFile.chapterProgress-1;
        handlingChapterFile = StartCoroutine(HandlingChapterFile(chapterProgress));
        Next();
    }

    public void SaveGameFile(float saveIndex){
        string filePath = FileManager.savPath + "Resources/gameFiles/" + saveIndex.ToString() + ".txt";

        activeGameFile.chapterName = activeChapterFile;
        activeGameFile.chapterProgress = chapterProgress;
        activeGameFile.cachedLastSpeaker = cachedLastSpeaker;
        activeGameFile.currentDate = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss",new System.Globalization.CultureInfo("th-TH"));

        activeGameFile.currentTextSystemDisplayText = data[chapterProgress-1];
        for(int i = 0; i < CharacterManager.instance.characters.Count; i++){
            Character character = CharacterManager.instance.characters[i];
            GAMEFILE.CHARACTERDATA data = new GAMEFILE.CHARACTERDATA(character);
            activeGameFile.charactersInScene.Add(data);
        }
        BackgroundManager b = BackgroundManager.instance;
        activeGameFile.background = b.background.activeImage != null ? b.background.activeImage.texture : null;
        activeGameFile.cinematic = b.cinematic.activeImage != null ? b.cinematic.activeImage.texture : null;
        activeGameFile.foreground = b.foreground.activeImage != null ? b.foreground.activeImage.texture : null;

        activeGameFile.music = AudioManager.activeSong != null ? AudioManager.activeSong.clip : null;
        FileManager.SaveJSON(filePath, activeGameFile);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_next);
        if(Input.GetKeyDown(KeyCode.Space)){
            Next();
        }

        if(Input.GetKeyDown(KeyCode.S)){
            SaveGameFile(0);
        }
        if(Input.GetKeyDown(KeyCode.L)){
            LoadGameFile(0);
        }
    }

    public void LoadChapterFile(string fileName){
        activeChapterFile = fileName;
        data = FileManager.LoadFile("Assets/Resources/Story/" + fileName);
        cachedLastSpeaker = "";

        if(handlingChapterFile != null){
            StopCoroutine(handlingChapterFile);
        }
        handlingChapterFile = StartCoroutine(HandlingChapterFile());

        Next();
    }

    bool _next = false;

    public void Next(){
        _next = true;
    }
    public bool isHandlingChapterFile {get{return handlingChapterFile != null;}}
    Coroutine handlingChapterFile = null;
    [HideInInspector] public int chapterProgress = 0;
    IEnumerator HandlingChapterFile(int progress = 0){
        chapterProgress = progress;

        while(chapterProgress < data.Count){

            if(_next){
                string line = data[chapterProgress];

                if(line.StartsWith("choice"))
                {
                    yield return HandlingChoiceLine(line);    
                    chapterProgress++;
                }
                else
                {
                    HandleLine(line);
                    chapterProgress++;
                    while(isHandlingLine){
                        yield return new WaitForEndOfFrame();
                    }
                }
                
            }
            yield return new WaitForEndOfFrame();
        }

        handlingChapterFile = null;
    }

    IEnumerator HandlingChoiceLine(string line){
        //string title = line.Split('"')[1];
        List<string> choices = new List<string>();
        List<string> actions = new List<string>();

        while(true){
            chapterProgress++;
            line = data[chapterProgress];

            if(line == "{")
                continue;

            line = line.Replace("    ","");

            if(line != "}"){
                choices.Add(line.Split('"')[1]);
                actions.Add(data[chapterProgress+1].Replace("    ",""));
                chapterProgress++;
            }
            else
                break;
        }
        if(choices.Count > 0){
            ChoiceScreen.Show(choices.ToArray());
            yield return new WaitForEndOfFrame();
            while(ChoiceScreen.isWaitingForChoiceToBeMade)
                yield return new WaitForEndOfFrame();

            string action = actions[ChoiceScreen.lastChoiceMade.index];
            HandleLine(action);

            while(isHandlingLine)
                yield return new WaitForEndOfFrame();
        }
        else{
            Debug.LogError("Invalid choice operation. No choices were found.");
        }
    }

    void HandleLine(string rawLine){
        ChapterLineManager.Line line = ChapterLineManager.interpret(rawLine);
        StopHandlingLine();
        handlingLine = StartCoroutine(HandlingLine(line));
    }

    void StopHandlingLine(){
        if(isHandlingLine){
            StopCoroutine(handlingLine);
        }
        handlingLine = null;
    }

  
    [HideInInspector] public bool isHandlingLine{get{return handlingLine != null;}}
    Coroutine handlingLine = null;
    IEnumerator HandlingLine(ChapterLineManager.Line line){
        _next = false;
        int lineProgress = 0;

        while(lineProgress < line.segments.Count){
            _next = false;
            ChapterLineManager.Line.Segment segment = line.segments[lineProgress];
            if(lineProgress > 0){
                if(segment.trigger == ChapterLineManager.Line.Segment.Trigger.autoDelay){
                    for(float timer = segment.autoDelay; timer >= 0; timer -= Time.deltaTime){
                        yield return new WaitForEndOfFrame();
                        if(_next) break;
                    }
                }
                else{
                    while(!_next) yield return new WaitForEndOfFrame();

                }
            }
            _next = false;
            segment.Run();

            while(segment.isRunning){
                yield return new WaitForEndOfFrame();
                if(_next){
                    segment.ForceFinish(); 
                    _next = false;
                }
            }

            lineProgress++;
            yield return new WaitForEndOfFrame();
        }
        foreach(string action in line.actions){
            HandleAction(action);
        }
        handlingLine = null;
    }
    
    [HideInInspector] public string cachedLastSpeaker = "";

    public void HandleAction(string action){
        //print("Handle event [" + action + "]");
        string[] data = action.Split('(' , ')');
        switch(data[0]){
            case "setBackground":
                Command_SetLayerImage(data[1], BackgroundManager.instance.background);
                break;
            case "setCinematic":
                Command_SetLayerImage(data[1], BackgroundManager.instance.cinematic);
                break;
            case "setForeground":
                Command_SetLayerImage(data[1], BackgroundManager.instance.foreground);
                break;
            case "transBackground":
                Command_TransLayer(data[1], BackgroundManager.instance.background);
                break;
            case "transCinematic":
                Command_TransLayer(data[1], BackgroundManager.instance.cinematic);
                break;
            case "transForeground":
                Command_TransLayer(data[1], BackgroundManager.instance.foreground);
                break;
            case "showScene":
                Command_ShowScene(data[1]);
                break;
            case "playSFX":
                Command_PlaySFX(data[1]);
                break;
            case "playBGM":
                Command_PlayBGM(data[1]);
                break;
            case "move":
                Command_MoveCharacter(data[1]);
                break;
            case "setPosition":
                Command_SetPosition(data[1]);
                break;
            case "setExpression":
                Command_SetExpression(data[1]);
                break;
            case "flip":
                Command_Flip(data[1]);
                break;
            case "faceLeft":
                Command_FaceLeft(data[1]);
                break;
            case "faceRight":
                Command_FaceRight(data[1]);
                break;
            case "enter":
                Command_Enter(data[1]);
                break;
            case "exit":
                Command_Exit(data[1]);
                break;
            case "Load":
                Command_Load(data[1]);
                break;
        }
    }

    void Command_Load(string chapterName){
        NovelController.instance.LoadChapterFile(chapterName);
    }

    void Command_SetLayerImage(string data, BackgroundManager.Layer layer){
        string fileName = data.Contains(",") ? data.Split(',')[0] : data;
        Texture tex = fileName == "null" ? null : Resources.Load("Images/UI/Backdrops/" + fileName) as Texture;
        float speed = 2f;
        bool smooth = false;

        if(data.Contains(",")){
            string[] parameters = data.Split(',');
            foreach(string param in parameters){
                float fValue = 0;
                bool bValue = false;
                if(float.TryParse(param, out fValue)){
                    speed = fValue; continue;
                }
                if(bool.TryParse(param, out bValue)){
                    smooth = bValue; continue;
                } 
            }      
        }
        layer.TransitionToTexture(tex, speed, smooth);
    }

    void Command_TransLayer(string data, BackgroundManager.Layer layer){
        string[] parameters = data.Split(',');

        string imageName = parameters[0];
        string transName = parameters[1];
        Texture tex = imageName == "null" ? null : Resources.Load("Images/UI/Backdrops/" + imageName) as Texture;
        Texture transTex = Resources.Load("Images/TransitionEffects/" + transName) as Texture;

        float speed = 2f;
        bool smooth = false;

        if(data.Contains(",")){
            string[] _parameters = data.Split(',');
            foreach(string param in _parameters){
                float fValue = 0;
                bool bValue = false;
                if(float.TryParse(param, out fValue)){
                    speed = fValue; continue;
                }
                if(bool.TryParse(param, out bValue)){
                    smooth = bValue; continue;
                } 
            }      
        }
        TransitionManager.transitionLayer(layer, tex, transTex, speed, smooth);
    }

    void Command_ShowScene(string data){
        string[] parameters = data.Split(',');
        bool show = bool.Parse(parameters[0]);
        string texName = parameters[1];
        Texture transTex = Resources.Load("Images/TransitionEffects/" + texName) as Texture;
        float speed = 2f;
        bool smooth = false;

        for(int i = 2; i < parameters.Length; i++){
            string p = parameters[i];
            float fVal = 0;
            bool bVal =  false;
            if(float.TryParse(p, out fVal)){
                speed = fVal; continue;
            }
            if(bool.TryParse(p, out bVal)){
                smooth = bVal; continue;
            }

        }

        TransitionManager.ShowScene(show, speed, smooth, transTex);
    }

    void Command_PlaySFX(string fileName){
        AudioClip clip = Resources.Load("Audio/SFX/" + fileName) as AudioClip;
        AudioManager.instance.PlaySFX(clip);
    }
    void Command_PlayBGM(string fileName){
        AudioClip clip = Resources.Load("Audio/BGM/" + fileName) as AudioClip;
        AudioManager.instance.PlayBGM(clip);
    }

    void Command_MoveCharacter(string data){
        string[] parameters = data.Split(',');
        string character = parameters[0];
        float locationX = float.Parse(parameters[1]);
        float locationY = parameters.Length >= 3 ? float.Parse(parameters[2]) : 0;
        float speed = parameters.Length >= 4 ? float.Parse(parameters[3]) : 10f;
        bool smooth = parameters.Length == 5 ? bool.Parse(parameters[4]) : true;
        
        Character c = CharacterManager.instance.GetCharacter(character);
        c.MoveTo(new Vector2(locationX, locationY), speed, smooth);
    }

    void Command_SetPosition(string data){
        string[] parameters = data.Split(',');
        string character = parameters[0];
        float locationX = float.Parse(parameters[1]);
        float locationY = float.Parse(parameters[2]);

        Character c = CharacterManager.instance.GetCharacter(character);
        c.SetPosition(new Vector2(locationX, locationY));
    }

    void Command_SetExpression(string data){
        string[] parameters = data.Split(',');
        string character = parameters[0];
        string expression = parameters[1];
        float speed = parameters.Length >= 3 ? float.Parse(parameters[2]) : 5f;
        bool smooth = parameters.Length == 4 ? bool.Parse(parameters[3]) : false;

        Character c = CharacterManager.instance.GetCharacter(character);
        c.TransitionBody(c.GetTexture(expression), speed, smooth);
    }

    void Command_Flip(string data){
        Character c = CharacterManager.instance.GetCharacter(data);
        c.Flip();
    }

    void Command_FaceLeft(string data){
        Character c = CharacterManager.instance.GetCharacter(data);
        c.FaceLeft();
    }

    void Command_FaceRight(string data){
        Character c = CharacterManager.instance.GetCharacter(data);
        c.FaceRight();
    }

    void Command_Enter(string data){
        string character = data.Contains(",") ? data.Split(',')[0] : data;
        float speed = 3;
        bool smooth = false;
        if(data.Contains(",")){
            string[] parameters = data.Split(',');
            foreach(string param in parameters){
                float fValue = 0;
                bool bValue = false;
                if(float.TryParse(param, out fValue)){
                    speed = fValue; continue;
                }
                if(bool.TryParse(param, out bValue)){
                    smooth = bValue; continue;
                } 
            }
        }
        Character c = CharacterManager.instance.GetCharacter(character, true, false);
        if(!c.enabled){
            c.renderers.renderer.color = new Color(1,1,1,0);
            c.enabled = true;

            c.TransitionBody(c.renderers.renderer.texture, speed, smooth);
        }
        else{
            c.FadeIn(speed, smooth);
        }
    }

    void Command_Exit(string data){
        string character = data.Contains(",") ? data.Split(',')[0] : data;
        float speed = 3;
        bool smooth = false;
        if(data.Contains(",")){
            string[] parameters = data.Split(',');
            foreach(string param in parameters){
                float fValue = 0;
                bool bValue = false;
                if(float.TryParse(param, out fValue)){
                    speed = fValue; continue;
                }
                if(bool.TryParse(param, out bValue)){
                    smooth = bValue; continue;
                } 
            }
        }
        Character c = CharacterManager.instance.GetCharacter(character);
        c.FadeOut(speed, smooth);
    }
}
