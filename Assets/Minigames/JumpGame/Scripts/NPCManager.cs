using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.JumpGame
{
    public class NPCManager : MonoBehaviour
    {
        [SerializeField] HearthManager hearth;
        void Start()
        {
            Invoke("SearthHearthManager", 1f);
        }
        void SearthHearthManager() { hearth = GameObject.FindObjectOfType<HearthManager>(); }
        public void EnableHearth() => hearth.EnableHeart();
        public void DisableHearth() => hearth.DisableHearth();
    }
}