using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBGM : MonoBehaviour
{
    [SerializeField] AudioSource bgm;

    #region Singleton
    public static UIBGM Instance = null;
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
        SetUIBGMVolume(SharedLibs.SoundManager.Instance.BGMVolume);
        bgm.enabled = true;
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
        bgm.volume = volume / 2;
    }
}
