using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    [SerializeField] AudioSource buttonSFX1, buttonSFX2;
    [SerializeField] AudioSource puzzleButtonSFX;

    [SerializeField] AudioClip puzzleButtonSFXClip;

    #region Singleton
    public static UISoundManager Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
                Destroy(this.gameObject);
        }
    }
    #endregion

    void Start()
    {
        SetSFXVolume(SharedLibs.SoundManager.Instance.SFXVolume);
    }

    private void OnEnable()
    {
        SharedLibs.SoundManager.Instance.OnSFXVolumeChanged += SetSFXVolume;
    }
    private void OnDisable()
    {
        SharedLibs.SoundManager.Instance.OnSFXVolumeChanged -= SetSFXVolume;
    }

    public void PlayButtonSFX1() => buttonSFX1.Play();
    public void PlayButtonSFX2() => buttonSFX2.Play();
    public void PlayPuzzleButtonSFX() => puzzleButtonSFX.PlayOneShot(puzzleButtonSFXClip);

    public void SetSFXVolume(float volume)
    {
        buttonSFX1.volume = volume;
        buttonSFX2.volume = volume;
        puzzleButtonSFX.volume = volume;
    }
}