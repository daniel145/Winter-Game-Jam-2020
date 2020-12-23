using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    [HideInInspector]
    public AudioManager instance;

    private float musicMultiplier = 0.75f;
    private float soundMultiplier = 0.75f;

    private int combo = 0;
    private bool comboRunning = false;
    private IEnumerator resetCombo;

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
            int rand = UnityEngine.Random.Range(0, 2);
            switch(rand)
            {
                case 0: name = "zombieDeath1"; break;
                case 1: name = "zombieDeath2"; break;
            };
        }
        else if (name == "zombieHit")
        {
            switch(combo % 3)
            {
                case 0: name = "zombieHit1"; break;
                case 1: name = "zombieHit2"; break;
                default: name = "zombieHit3"; break;
            };
            combo++;
            if (comboRunning)
                StopCoroutine(resetCombo);
            resetCombo = ComboReset();
            StartCoroutine(resetCombo);
            comboRunning = true;
        }
        else if (name == "playerHit")
        {
            int rand = UnityEngine.Random.Range(0, 3);
            switch (rand)
            {
                case 0: name = "playerHit1"; break;
                case 1: name = "playerHit2"; break;
                default: name = "playerHit3"; break;
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

    private IEnumerator ComboReset()
    {
        yield return new WaitForSeconds(1.5f);
        combo = 0;
        comboRunning = false;
    }
}
