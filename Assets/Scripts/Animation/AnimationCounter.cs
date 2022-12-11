using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCounter : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private static readonly int CountStart = Animator.StringToHash("CountStart");
    
    [SerializeField] private AnimationEndChecker animEndChecker;

    public event Action OnEndCount;

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
