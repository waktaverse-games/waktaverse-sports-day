using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    [Obsolete]
    public class BrickEffectDestroy : StateMachineBehaviour
    {
        private bool stateExitCalled;
        private void Awake()
        {
            stateExitCalled = false;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!stateExitCalled)
            {
                base.OnStateExit(animator, stateInfo, layerIndex);
                animator.GetComponentInParent<Brick>().DestroyAfterEffect();
                stateExitCalled = true;
            }

        }
    }
}

