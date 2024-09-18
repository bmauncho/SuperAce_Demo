using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioSource> PossibleAudioSource = new List<AudioSource>();
    public AudioSource AmbientSound;
    public float minSound, maxSound;
    private void Start ()
    {
        LowerAmbientSound();
    }
    public void LowerAmbientSound ()
    {
        AmbientSound.volume = minSound;
    }

    public void NormalAmbientSound ()
    {
        AmbientSound.volume = maxSound;
    }

    public void PlaySound ( string AudioFile , bool loopState = false , float Volume = 1f )
    {
        AudioClip _Clip = (AudioClip)Resources.Load("Sounds/" + AudioFile);
        AudioSource AS = ReturnOpenSource();
        AS.clip = _Clip;
        AS.loop = loopState;
        AS.volume = Volume;
        AS.Play();
    }

    public void PlayAmbientSound ( string AudioFile)
    {
        AudioClip _Clip = (AudioClip)Resources.Load("Sounds/" + AudioFile);
        AmbientSound.clip = _Clip;
        AmbientSound.Play();
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
}
