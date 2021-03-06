using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AssetsLoader : MonoBehaviour
{
    public static AssetsLoader instance;
    private GameObject[] panels;
    private AudioClip[] sfxs;
    private AudioClip[] bgms;
    private GameObject[] characters;
    private TextAsset _archiveData;
    private Texture2D[] texture2Ds;
    private Texture2D[] transitionEffects;
    public TextAsset archiveData
    {
        get { return _archiveData; }
    }
    public TextAsset checkpointData;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        panels = Resources.LoadAll<GameObject>(GlobalReferences.Path.PrefabPanelsPath);

        characters = Resources.LoadAll<GameObject>(GlobalReferences.Path.PrefabCharacterPath);

        sfxs = Resources.LoadAll<AudioClip>(GlobalReferences.Path.SFXPath);
        bgms = Resources.LoadAll<AudioClip>(GlobalReferences.Path.BGMPath);

        _archiveData = Resources.Load<TextAsset>(GlobalReferences.Path.ArchivePath + GlobalReferences.Data.ArchiveData) as TextAsset;
        checkpointData = Resources.Load<TextAsset>(GlobalReferences.Path.CheckpointPath + GlobalReferences.Data.CheckPointData) as TextAsset;

        characterSprite.GabiSpriteList = Resources.LoadAll<Sprite>(GlobalReferences.Path.SpriteCharacterPath + GlobalReferences.CharacterName.Gabi);
        characterSprite.KanaoSpriteList = Resources.LoadAll<Sprite>(GlobalReferences.Path.SpriteCharacterPath + GlobalReferences.CharacterName.Kanao);
        characterSprite.BillSpriteList = Resources.LoadAll<Sprite>(GlobalReferences.Path.SpriteCharacterPath + GlobalReferences.CharacterName.Bill);
        //characterSprite.BlackGuySpriteList = Resources.LoadAll<Sprite>(GlobalReferences.Path.SpriteCharacterPath + GlobalReferences.CharacterName.BlackGuy);
        characterSprite.MissSundaySpriteList = Resources.LoadAll<Sprite>(GlobalReferences.Path.SpriteCharacterPath + GlobalReferences.CharacterName.MissSunday);
        characterSprite.NompangSpriteList = Resources.LoadAll<Sprite>(GlobalReferences.Path.SpriteCharacterPath + GlobalReferences.CharacterName.Nompang);
        characterSprite.HaruSpriteList = Resources.LoadAll<Sprite>(GlobalReferences.Path.SpriteCharacterPath + GlobalReferences.CharacterName.Haru);
        characterSprite.BettySpriteList = Resources.LoadAll<Sprite>(GlobalReferences.Path.SpriteCharacterPath + GlobalReferences.CharacterName.Betty);
        characterSprite.BotakSpriteList = Resources.LoadAll<Sprite>(GlobalReferences.Path.SpriteCharacterPath + GlobalReferences.CharacterName.Botak);
        characterSprite.DaijiSpriteList = Resources.LoadAll<Sprite>(GlobalReferences.Path.SpriteCharacterPath + GlobalReferences.CharacterName.Daiji);
        characterSprite.PoliceSpriteList = Resources.LoadAll<Sprite>(GlobalReferences.Path.SpriteCharacterPath + GlobalReferences.CharacterName.Police);
        characterSprite.AuntieSpriteList = Resources.LoadAll<Sprite>(GlobalReferences.Path.SpriteCharacterPath + GlobalReferences.CharacterName.Auntie);
        characterSprite.BadguySpriteList = Resources.LoadAll<Sprite>(GlobalReferences.Path.SpriteCharacterPath + GlobalReferences.CharacterName.Badguy);

        texture2Ds = Resources.LoadAll<Texture2D>(GlobalReferences.Path.BackgroundPath);

        transitionEffects = Resources.LoadAll<Texture2D>(GlobalReferences.Path.TransitonEffectPath);
    }

    public GameObject PanelLoader(string name)
    {
        foreach (GameObject panel in panels)
        {
            if (panel.name == name)
                return panel;
        }
        Debug.LogError("ERROR: " + name + "does not exist. (Panel)");
        return null;
    }

    public AudioClip AudioLoader(string name, string type)
    {
        if (name == "null") return null;
        AudioClip[] audios = null;
        switch (type)
        {
            case "SFX":
                audios = sfxs;
                break;
            case "BGM":
                audios = bgms;
                break;
            default:
                break;
        }

        foreach (AudioClip audio in audios)
        {
            if (audio.name == name)
            {
                return audio;
            }
        }
        Debug.LogError("ERROR: " + name + "does not exist. (Audio)");
        return null;
    }

    public GameObject CharacterLoader(string name)
    {
        foreach (GameObject character in characters)
        {
            if (character.name == name)
            {
                return character;
            }
        }
        Debug.LogError("ERROR: " + name + " does not exist. (Character)");
        return null;
    }

    private class CharacterSprite
    {
        public Sprite[] GabiSpriteList;
        public Sprite[] KanaoSpriteList;
        public Sprite[] BillSpriteList;
        //public Sprite[] BlackGuySpriteList;
        public Sprite[] MissSundaySpriteList;
        public Sprite[] NompangSpriteList;
        public Sprite[] HaruSpriteList;
        public Sprite[] BettySpriteList;
        public Sprite[] BotakSpriteList;
        public Sprite[] DaijiSpriteList;
        public Sprite[] PoliceSpriteList;
        public Sprite[] AuntieSpriteList;
        public Sprite[] BadguySpriteList;
    }

    private CharacterSprite characterSprite = new CharacterSprite();
    public Sprite GetCharacterSprite(string CharacterName, string expression)

    {
        Sprite[] sprites = null;
        switch (CharacterName)
        {
            case GlobalReferences.CharacterName.Gabi:
                sprites = characterSprite.GabiSpriteList;
                break;
            case GlobalReferences.CharacterName.Kanao:
                sprites = characterSprite.KanaoSpriteList;
                break;
            case GlobalReferences.CharacterName.Bill:
                sprites = characterSprite.BillSpriteList;
                break;
            //case GlobalReferences.CharacterName.BlackGuy:
            //sprites = characterSprite.BlackGuySpriteList;
            //break;
            case GlobalReferences.CharacterName.MissSunday:
                sprites = characterSprite.MissSundaySpriteList;
                break;
            case GlobalReferences.CharacterName.Nompang:
                sprites = characterSprite.NompangSpriteList;
                break;
            case GlobalReferences.CharacterName.Haru:
                sprites = characterSprite.HaruSpriteList;
                break;
            case GlobalReferences.CharacterName.Betty:
                sprites = characterSprite.BettySpriteList;
                break;
            case GlobalReferences.CharacterName.Botak:
                sprites = characterSprite.BotakSpriteList;
                break;
            case GlobalReferences.CharacterName.Daiji:
                sprites = characterSprite.DaijiSpriteList;
                break;
            case GlobalReferences.CharacterName.Police:
                sprites = characterSprite.PoliceSpriteList;
                break;
            case GlobalReferences.CharacterName.Auntie:
                sprites = characterSprite.AuntieSpriteList;
                break;
            case GlobalReferences.CharacterName.Badguy:
                sprites = characterSprite.BadguySpriteList;
                break;
            default:
                Debug.LogError("ERROR: " + CharacterName + "does not exist. (Character Prefabs)");
                break;
        }
        foreach (Sprite sprite in sprites)
        {
            if (sprite.name == expression)
            {
                return sprite;
            }
        }
        Debug.LogError("ERROR: " + CharacterName + "does not have " + expression + ". (Expression Sprites)");
        return null;
    }

    public Texture2D GetBackground(string backgroundName)
    {
        if (backgroundName == "null") return null;
        foreach (Texture2D texture2D in texture2Ds)
        {
            if (texture2D.name == backgroundName)
            {
                return texture2D;
            }
        }
        Debug.LogError("ERROR: " + backgroundName + "does not exist. (Background)");
        return null;
    }

    public Texture2D GetTransitionEffects(string effectName)
    {
        if (effectName == "null") return null;
        foreach (Texture2D effects in transitionEffects)
        {
            if (effects.name == effectName)
            {
                return effects;
            }
        }
        Debug.LogError("ERROR: " + effectName + "does not exist. (Transition Effect)");
        return null;
    }
}