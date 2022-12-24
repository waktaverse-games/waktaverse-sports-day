using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBGM : MonoBehaviour
{
    [SerializeField] AudioSource bgm;

    [SerializeField] private float multiplier = 0.6f;

    void Start()
    {
        SetUIBGMVolume(SharedLibs.SoundManager.Instance.BGMVolume);
        bgm.enabled = true;
        bgm.loop = true;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SharedLibs.SoundManager.Instance.OnBGMVolumeChanged += SetUIBGMVolume;
    }
    private void OnDisable()
    {
        SharedLibs.SoundManager.Instance.OnBGMVolumeChanged -= SetUIBGMVolume;
    }

    public void OffUIBGM()
    {
        bgm.enabled = false;
    }
    public void OnUIBGM()
    {
        bgm.enabled = true;
    }
    public void SetUIBGMVolume(float volume)
    {
        bgm.volume = volume * multiplier;
    }
}
