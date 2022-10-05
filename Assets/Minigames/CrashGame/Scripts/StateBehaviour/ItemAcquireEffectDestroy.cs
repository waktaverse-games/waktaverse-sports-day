using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    [System.Obsolete]
    public class ItemAcquireEffectDestroy : EffectDestroy
    {

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!stateExitCalled)
            {
                base.OnStateExit(animator, stateInfo, layerIndex);
                animator.GetComponentInParent<Item>().DestroyAfterEffect();
                stateExitCalled = true;
            }

        }
    }
}

