using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameHeaven.AttackGame
{
    public class DamageText : MonoBehaviour
    {
        public TextMeshProUGUI childText;

        public void SetDamage(int n)
        {
            childText.text = n.ToString();
        }
    }
}