using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GAMEFILE
{
    public string chapterName;
    public int chapterProgress = 0;

    public string cachedLastSpeaker = "";

    public string currentShowSpeaker;
    public string currentDialogue;
    public int episodeNumber = 1;
    public string date;
    public List<string> checkpoints;
    public List<CHARACTERDATA> characters = new List<CHARACTERDATA>();
    public Texture background = null;
    public Texture foreground = null;
    public Texture cinematic = null;
    public AudioClip bgm;

    public GAMEFILE(string chapterName, int chapterProgress, string cachedLastSpeaker, string currentShowSpeaker, string currentDialogue, int episodeNumber, DateTime date, List<string> checkpoints)
    {
        this.chapterName = chapterName;
        this.chapterProgress = chapterProgress;
        this.currentShowSpeaker = currentShowSpeaker;
        this.cachedLastSpeaker = cachedLastSpeaker;
        this.currentDialogue = currentDialogue;
        this.episodeNumber = episodeNumber;
        this.date = date.ToString("dddd, dd MMMM yyyy HH:mm:ss");
        this.checkpoints = checkpoints;

    }

    [System.Serializable]
    public class CHARACTERDATA
    {
        public string characterName;
        public Sprite characterExpression;
        public Vector2 position;
        public bool enabled;

        public CHARACTERDATA(Character data)
        {
            this.characterName = data.characterName;
            this.characterExpression = data.renderers.renderer.sprite;
            this.position = data.targetPosition;
            this.enabled = data.enabled;
        }

    }
}