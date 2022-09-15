using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace GameHeaven.AttackGame
{
    public class Coin : MonoBehaviour
    {
        public int tweenId;

        private bool isFalling = false;
        private void OnEnable()
        {
            transform.position = new Vector3(UnityEngine.Random.Range(50.6f, 64.6f),
                UnityEngine.Random.Range(2.5f, 4.5f), 0);
            isFalling = false;
        }

        private void OnDisable()
        {
            isFalling = false;
        }

        public void StartFallCount(float time)
        {
            StartCoroutine(Fall(time));
        }

        IEnumerator Fall(float time)
        {
            yield return new WaitForSeconds(time);
            isFalling = true;
        }

        private void Update()
        {
            if (isFalling)
            {
                Vector3 currPos = transform.position;
                Vector3 pos = new Vector3(currPos.x, (float)(currPos.y - 1.4 * Time.deltaTime), currPos.z);
                transform.Translate(pos);
            }

            if (transform.position.y < 0f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}