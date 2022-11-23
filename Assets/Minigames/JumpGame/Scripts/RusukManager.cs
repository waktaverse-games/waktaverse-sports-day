using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace GameHeaven.JumpGame
{
    public class RusukManager : MonoBehaviour
    {
        [SerializeField] GameObject VFX;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                SoundManager.Instance.PlayRusukSound();
                GameManager.Instance.SetInvinsible(true);
                GameObject vfx = Instantiate(VFX, transform.position, transform.rotation);
                Destroy(vfx, 2f);
                Destroy(gameObject);
            }
            if (collision.gameObject.tag == "Border")
            {
                Destroy(gameObject);
            }
        }
    }
}