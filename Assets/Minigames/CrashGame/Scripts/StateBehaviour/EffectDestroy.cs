using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class EffectDestroy : StateMachineBehaviour
    {
        protected Type classType;
        protected bool stateExitCalled;
        private void Awake()
        {
            stateExitCalled = false;
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!stateExitCalled)
            {
                base.OnStateExit(animator, stateInfo, layerIndex);
                animator.GetComponentInParent<IDestroyEffect>().DestroyAfterEffect();
                stateExitCalled = true;
            }

        }
    }
}
