using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class SmallBullet : MonoBehaviour
    {
        private Rigidbody2D rigidBody;
        [SerializeField]
        private float fireForce = 500f;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            
        }

        public void FireBullet(Vector2 direction)
        {
            // direction's magnitude has to be 1
            rigidBody.AddForce(direction * fireForce);
        }

        private void DestroySelf()
        {
            // 이펙트 발생
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Brick"))
            {
                collision.gameObject.GetComponent<Brick>().BallCollide();
            }
            GameManager.Instance.Sound.PlayEffect("tick", volume: .25f);
            DestroySelf();
        }


    }
}
