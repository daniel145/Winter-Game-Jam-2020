using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public AudioManager instance;

    private float musicMultiplier = 0.75f;
    private float soundMultiplier = 0.75f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }


        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume * 0.75f;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = sound.playOnAwake;
        }
    }

    public void Play(string name)
    {
        if (name == "zombieDeath")
        {
            int rand = UnityEngine.Random.Range(0, 3);
            switch(rand)
            {
                case 0: name = "zombieDeath1"; break;
                case 1: name = "zombieDeath2"; break;
                default: name = "zombieDeath3"; break;
            };
        }

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            Debug.Log("No sound with name " + name);
        else
            s.source.Play();
    }

    public void SetMusicMult(float mult)
    {
        musicMultiplier = mult;
        foreach (Sound sound in sounds)
        {
            if (sound.loop)
                sound.source.volume = sound.volume * mult;
        }
    }

    public void SetSoundMult(float mult)
    {
        soundMultiplier = mult;
        foreach (Sound sound in sounds)
        {
            if (!sound.loop)
                sound.source.volume = sound.volume * mult;
        }
    }
}
