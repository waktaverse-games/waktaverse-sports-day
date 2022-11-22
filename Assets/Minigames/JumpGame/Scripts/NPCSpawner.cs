using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibs;
namespace GameHeaven.JumpGame
{
    public class NPCSpawner : MonoBehaviour
    {
        [SerializeField] GameObject[] NPC;
        [SerializeField] GameObject parent;
        public void SetFandomCharacter(CharacterType player)
        {
            int playerType = (int)player;
            Instantiate(NPC[playerType], parent.transform, false);
        }
    }
}
