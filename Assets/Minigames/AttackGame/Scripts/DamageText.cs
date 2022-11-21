using System;
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
            StartCoroutine(Disable(1f));
        }

        private void Update()
        {
            transform.Translate(0, 2 * Time.deltaTime, 0);
        }

        private IEnumerator Disable(float time)
        {
            yield return new WaitForSeconds(time);
            gameObject.SetActive(false);
        }
    }
}