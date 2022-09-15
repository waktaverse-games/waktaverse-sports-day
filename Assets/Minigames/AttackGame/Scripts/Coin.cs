using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace GameHeaven.AttackGame
{
    public class Coin : MonoBehaviour
    {
        public int tweenId;
        public GameManager gameManager;

        private bool isFalling = false;
        private void OnEnable()
        { 
            isFalling = false;
        }

        private void OnDisable()
        {
            isFalling = false;
        }

        public void StartFall(float time)
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
                gameManager.DeleteCoin();
                gameObject.SetActive(false);
            }
        }
    }
}