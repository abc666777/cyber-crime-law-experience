using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public static BGM activeBGM = null;
    public static List<BGM> allBGMs = new List<BGM>();
    public float bgmTransitionSpeed = 1f;
    public bool bgmSmoothTransition = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public void PlaySFX(AudioClip effect, float volume = 1f, float pitch = 1f)
    {
        AudioSource source = CreateNewSource(string.Format("SFX [{0}]", effect.name));
        source.clip = effect;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();

        Destroy(source.gameObject, effect.length);
    }

    public void PlayBGM(AudioClip bgm, float maxVolume = 1f, float pitch = 1f, float startingVolume = 0f, bool playOnStart = true, bool loop = true)
    {
        if (bgm != null)
        {
            foreach (BGM _bgm in allBGMs)
            {
                if (_bgm.clip == bgm)
                {
                    activeBGM = _bgm;
                    break;
                }
            }
            if (activeBGM == null || activeBGM.clip != bgm)
                activeBGM = new BGM(bgm, maxVolume, pitch, startingVolume, playOnStart, loop);
        }
        else
            activeBGM = null;

        StopAllCoroutines();
        StartCoroutine(VolumeLeveling());
    }

    IEnumerator VolumeLeveling()
    {
        while (TransitionBGMs())
            yield return new WaitForEndOfFrame();
    }

    bool TransitionBGMs()
    {
        bool anyValueChanged = false;
        float speed = bgmTransitionSpeed * Time.deltaTime;

        for (int i = allBGMs.Count - 1; i >= 0; i--)
        {
            BGM bgm = allBGMs[i];
            if (bgm == activeBGM)
            {
                if (bgm.volume < bgm.maxVolume)
                {
                    bgm.volume = bgmSmoothTransition ? Mathf.Lerp(bgm.volume, bgm.maxVolume, speed) : Mathf.MoveTowards(bgm.volume, bgm.maxVolume, speed);
                    anyValueChanged = true;
                }

            }
            else
            {
                if (bgm.volume > 0)
                {
                    bgm.volume = bgmSmoothTransition ? Mathf.Lerp(bgm.volume, 0, speed) : Mathf.MoveTowards(bgm.volume, 0, speed);
                    anyValueChanged = true;
                }

                else
                {
                    allBGMs.RemoveAt(i);
                    bgm.Destroy();
                    continue;
                }
            }
        }

        return anyValueChanged;
    }

    public static AudioSource CreateNewSource(string _name)
    {
        AudioSource newSource = new GameObject(_name).AddComponent<AudioSource>();
        newSource.transform.SetParent(instance.transform);
        return newSource;
    }

    [System.Serializable]
    public class BGM
    {
        public AudioSource source;
        public AudioClip clip { get { return source.clip; } set { source.clip = value; } }
        public float maxVolume = 1f;
        public BGM(AudioClip clip, float _maxVolume, float pitch, float startingVolume, bool playOnStart, bool loop = true)
        {
            source = AudioManager.CreateNewSource(string.Format("BGM [{0}]", clip.name));
            source.clip = clip;
            source.volume = startingVolume;
            maxVolume = _maxVolume;
            source.pitch = pitch;
            source.loop = loop;

            AudioManager.allBGMs.Add(this);

            if (playOnStart)
                source.Play();
        }

        public float volume { get { return source.volume; } set { source.volume = value; } }
        public float pitch { get { return source.pitch; } set { source.pitch = value; } }

        public void Play()
        {
            source.Play();
        }
        public void Stop()
        {
            source.Stop();
        }
        public void Pause()
        {
            source.Pause();
        }

        public void UnPause()
        {
            source.UnPause();
        }

        public void Destroy()
        {
            AudioManager.allBGMs.Remove(this);
            DestroyImmediate(source.gameObject);
        }
    }
}