using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioSource> PossibleAudioSource = new List<AudioSource>();
    public AudioSource AmbientSound;
    public float minSound, maxSound;
    public SoundSettings soundSettings;
    private void Start ()
    {
        PlayAmbientSound("FunkCasino");
    }

    private void Update ()
    {
        setBGVolume(soundSettings.setBagroundMusicVolume());
        setSFXVolume(soundSettings.setSoundEffectsVolume());
    }

    public void LowerAmbientSound ()
    {
        AmbientSound.volume = minSound;
    }

    public void NormalAmbientSound ()
    {
        AmbientSound.volume = maxSound;
    }

    public void PlaySound ( string AudioFile , bool loopState = false)
    {
        AudioClip _Clip = (AudioClip)Resources.Load("mp3/" + AudioFile);
        AudioSource AS = ReturnOpenSource();
        AS.clip = _Clip;
        AS.loop = loopState;
        AS.volume = soundSettings.setSoundEffectsVolume();
        AS.Play();
    }

    public void PlayAmbientSound ( string AudioFile)
    {
        AudioClip _Clip = (AudioClip)Resources.Load("mp3/" + AudioFile);
        AmbientSound.clip = _Clip;
        AmbientSound.Play();
        AmbientSound.volume = soundSettings.setBagroundMusicVolume();
    }

    public void StopAudio ( string ClipName )
    {
        for (int i = 0 ; i < PossibleAudioSource.Count ; i++)
        {
            if (PossibleAudioSource [i].clip)
            {
                if (PossibleAudioSource [i].clip.name == ClipName)
                {
                    PossibleAudioSource [i].Stop();
                    PossibleAudioSource [i].loop = false;
                    PossibleAudioSource [i].volume = 1f;
                }
            }
        }
    }

    AudioSource ReturnOpenSource ()
    {
        for (int i = 0 ; i < PossibleAudioSource.Count ; i++)
        {
            if (!PossibleAudioSource [i].isPlaying)
            {
                return PossibleAudioSource [i];
            }
        }

        return PossibleAudioSource [0];
    }

    public void PauseAllSounds ()
    {
        for (int i = 0 ; i < PossibleAudioSource.Count ; i++)
        {
            if (PossibleAudioSource [i].clip)
            {
                PossibleAudioSource [i].Pause();
            }
        }

        AmbientSound.Pause();
    }


    public void UnPauseAllSounds ()
    {
        for (int i = 0 ; i < PossibleAudioSource.Count ; i++)
        {
            if (PossibleAudioSource [i].clip)
            {
                PossibleAudioSource [i].UnPause();
            }
        }

        AmbientSound.UnPause();
    }

    public void setSFXVolume(float volume )
    {
        for (int i = 0 ; i < PossibleAudioSource.Count ; i++)
        {
            if (PossibleAudioSource [i].clip)
            {
                PossibleAudioSource [i].volume = volume;
            }
        }
    }

    public void setBGVolume(float volume )
    {
        AmbientSound.volume = volume;
    }
    [ContextMenu("Play start sound")]
    public void startSound ()
    {
        StartCoroutine(Playstartsound ());
    }

    IEnumerator Playstartsound ()
    {
        PlaySound("4");
        yield return new WaitForSeconds(.4f);
        PlaySound("3");
        yield return new WaitForSeconds(.4f);
        PlaySound("2");
        yield return new WaitForSeconds(.4f);
        PlaySound("5");
        yield return new WaitForSeconds(.75f);
        PlaySound("final");
    }
}
