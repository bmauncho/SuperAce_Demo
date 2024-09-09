using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UISettingController : MonoBehaviour
{
    Toggle SoundToogle;
    public bool TheSound;

    public void Start ()
    {
        SoundToogle = GetComponentInChildren<Toggle>();
        CommandCentre.Instance.SettingsManager_.RefreshSettings();
        RefreshSound();
    }

    private void OnEnable ()
    {
        RefreshSound();
    }

    public void SoundPressed ()
    {
        if (SoundToogle != null)
        {
            CommandCentre.Instance.SettingsManager_.ToogleSound(SoundToogle.isOn);
            TheSound = CommandCentre.Instance.SettingsManager_.Sound;
        }
    }

    public void RefreshSound ()
    {
        if (SoundToogle != null)
        {
            TheSound = CommandCentre.Instance.SettingsManager_.Sound;
            SoundToogle.isOn = TheSound;
        }
    }
}
