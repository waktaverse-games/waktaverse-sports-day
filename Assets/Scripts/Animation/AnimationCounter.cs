using System;
using System.Collections;
using System.Collections.Generic;
using SharedLibs;
using UnityEngine;

public class AnimationCounter : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private static readonly int CountStart = Animator.StringToHash("CountStart");
    
    [SerializeField] private AnimationEndChecker animEndChecker;

    [SerializeField] private AudioSource countdownAudio;

    public event Action OnEndCount;

    private void Awake()
    {
        countdownAudio.volume = SoundManager.Instance.SFXVolume;
    }

    private void Start()
    {
        animator.SetTrigger(CountStart);
        StartCoroutine(AnimationCounterRoutine());
    }

    private IEnumerator AnimationCounterRoutine()
    {
        yield return new WaitUntil(() => animEndChecker.IsAnimationEnded);
        
        OnEndCount?.Invoke();
        Debug.Log("Start");
    }
}
