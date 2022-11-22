using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.JumpGame
{
    public class HearthManager : MonoBehaviour
    {
        [SerializeField] GameObject[] heartEffects;

        public void EnableHeart()
        {
            foreach (var effect in heartEffects)
            {
                effect.SetActive(true);
            }
        }

        public void DisableHearth()
        {
            foreach (var effect in heartEffects)
            {
                effect.SetActive(false);
            }
        }
    }
}
