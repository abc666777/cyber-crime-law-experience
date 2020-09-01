using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [HideInInspector]
    public static Song activeSong = null;
    public static List<Song> allSongs = new List<Song>();

    public float songTransition = 2f;
    public bool smoothSongTransition;
    // Start is called before the first frame update
    void Awake() {
        
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            DestroyImmediate(gameObject);
        }
    }

    public void PlaySFX(AudioClip effect, float volume = 1f , float pitch = 1f){
        AudioSource source = CreateNewSource(string.Format("SFX [{0}]", effect.name));
        source.clip = effect;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();

        Destroy(source.gameObject, effect.length);
    }

    public void PlayBGM(AudioClip song, float maxVolume = 1f, float pitch = 1f, float startingVolume = 0, bool playOnStart = true, bool loop = true){
        if(song != null){
            foreach(Song songObject in allSongs){
                if(songObject.clip == song){
                    activeSong = songObject;
                    break;
                }
            }
            if(activeSong == null || activeSong.clip != song){
                activeSong = new Song(song, maxVolume, pitch, startingVolume, playOnStart, loop);
            }
        }
        else{
            activeSong = null;
        }

        StopAllCoroutines();
        StartCoroutine(VolumeLeveling());
    }


    IEnumerator VolumeLeveling(){
        while(TransitionSongs()){
            yield return new WaitForEndOfFrame(); 
        }
    }

    bool TransitionSongs(){
        bool anyValueChanged = false;

        float speed = songTransition * Time.deltaTime;
        for(int i = allSongs.Count - 1; i >= 0; i--){
            Song song = allSongs[i];

            if(song == activeSong){
                if(song.volume < song.maxVolume){
                    song.volume = smoothSongTransition ? Mathf.Lerp(song.volume, song.maxVolume, speed) : Mathf.MoveTowards(song.volume, song.maxVolume, speed);
                    anyValueChanged = true;
                }
                
            }
            else{
                if(song.volume > 0){
                    song.volume = smoothSongTransition ? Mathf.Lerp(song.volume, 0, speed) : Mathf.MoveTowards(song.volume, 0, speed);
                    anyValueChanged = true;
                }
                
                else{
                    allSongs.RemoveAt(i) ;
                    song.Destroy();
                    continue;
                }
            }
        }

        return anyValueChanged;
    }

    public static AudioSource CreateNewSource(string _name){
        AudioSource newSource = new GameObject(_name).AddComponent<AudioSource>();
        newSource.transform.SetParent(instance.transform);
        return newSource;
    }

    [System.Serializable]
    public class Song{
        public AudioSource source;
        public AudioClip clip {get{return source.clip;} set{source.clip = value;}}
        public float maxVolume = 1f;
       

        public Song(AudioClip clip, float maxVolume, float pitch, float startingVolume, bool playOnStart, bool loop){
            source = AudioManager.CreateNewSource(string.Format("BGM [{0}]", clip.name));
            source.clip = clip;
            source.volume = startingVolume;
            this.maxVolume = maxVolume;
            source.pitch = pitch;
            source.loop = loop;

            AudioManager.allSongs.Add(this);

            if(playOnStart){
                source.Play();
            }
        }

        public float volume {get{return source.volume;} set{source.volume = value;}}
        public float pitch {get{return source.pitch;} set{source.pitch = value;}}

        public void Play(){
            source.Play();
        }

        public void Stop(){
            source.Stop();
        }

        public void Pause(){
            source.Pause();
        }

        public void UnPause(){
            source.UnPause();
        }

        public void Destroy(){
            AudioManager.allSongs.Remove(this);
            DestroyImmediate(source.gameObject);
        }
    }
}
