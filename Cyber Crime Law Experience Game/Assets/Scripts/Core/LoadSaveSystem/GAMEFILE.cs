using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GAMEFILE
{
    public string chapterName;
    public int chapterProgress;
    public string cachedLastSpeaker = "";
    public string currentTextSystemSpeakerDisplayText = "";
    public string currentTextSystemDisplayText = "";

    public Texture background = null;
    public Texture cinematic = null;
    public Texture foreground = null;

    public AudioClip music = null;

    public List<CHARACTERDATA> charactersInScene = new List<CHARACTERDATA>();

    public GAMEFILE(){
        this.chapterName = "test";
        this.chapterProgress = 0;
        this.cachedLastSpeaker = "";

        charactersInScene = new List<CHARACTERDATA>();
    }

    [System.Serializable]
    public class CHARACTERDATA{
        public string characterName = "";
        public bool enabled = true;
        public Texture expression = null;
        public bool facingRight = true;
        public Vector2 position = Vector2.zero;

        public CHARACTERDATA(Character character){
            this.characterName = character.characterName;
            this.enabled = character.isVisibleInScene;
            this.expression = character.renderers.renderer.texture;
            this.facingRight = character.isFacingRight;
            this.position = character._targetPosition;
        }
    }
}
