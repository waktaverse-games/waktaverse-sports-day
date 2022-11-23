using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    [Serializable]
    public struct ItemScoreData
    {
        [SerializeField] private string name;
        [SerializeField] private int score;

        public string Name => name;
        public int Score => score;
    }

    [CreateAssetMenu(fileName = "PunctureGame ItemScore", menuName = "Minigames/PunctureGame", order = 0)]
    public class ItemScore : ScriptableObject
    {
        [SerializeField] private List<ItemScoreData> data;

        public int GetItemScoreByType(string type)
        {
            return data.Find(elem => elem.Name.Equals(type)).Score;
        }
    }
}