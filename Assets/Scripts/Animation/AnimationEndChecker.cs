using UnityEngine;

public class AnimationEndChecker : MonoBehaviour
{
    private bool isAnimationEnded = false;
    public bool IsAnimationEnded
    {
        get => isAnimationEnded;
        private set => isAnimationEnded = value;
    }
    
    public void ObserveAnimationEnd()
    {
        IsAnimationEnded = true;
    }
}