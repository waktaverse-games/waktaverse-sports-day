using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHaven.RunGame
{
    public class PlayerControl : MonoBehaviour
    {

        public GameObject Caracter;

        [SerializeField]
        float Speed;
        [SerializeField]
        float ChangeSpeed;

        int CoinCount = 0;

        Vector2 dir = Vector2.left;
        bool Dirleft = true;

        public Animator Gold;
        public Animator Silver;
        public Animator Bronz;



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Dirleft)
                {
                    Dirleft = false;
                    dir = Vector2.right;
                    Caracter.GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    Dirleft = true;
                    dir = Vector2.left;
                    Caracter.GetComponent<SpriteRenderer>().flipX = false;
                }
            }

            Caracter.GetComponent<Rigidbody2D>().velocity = dir * Speed;

            
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Enemy")
                Destroy(gameObject);
            else if (other.gameObject.tag == "Coin")
            {
                GetCoin(other.gameObject.name);
                Destroy(other.gameObject);
                Debug.Log(CoinCount);
            }
        }

        void GetCoin(string coinName)
        {
            if (coinName.Contains("GoldCoin"))
            {
                Gold.SetBool("Get", true);
                CoinCount += 5;
            }
            else if (coinName.Contains("SilverCoin"))
            {
                Silver.SetBool("Get", true);
                CoinCount += 3;
            }
            else if (coinName.Contains("BronzCoin"))
            {
                Bronz.SetBool("Get", true);
                CoinCount += 1;
            }
        }
    }
}