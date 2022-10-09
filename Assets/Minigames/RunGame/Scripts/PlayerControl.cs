using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHaven.RunGame
{
    public class PlayerControl : MonoBehaviour
    {

        public GameObject Caracter;
        public GameObject Cam;

        [SerializeField]
        float Speed;
        [SerializeField]
        float ChangeSpeed;

        int CoinCount = 0;

        Vector2 dir = Vector2.left;
        bool Dirleft = true;

        Animator coin;

        AudioSource audio;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Dirleft) //������ ���� ������
                {
                    Dirleft = false; //����X
                    dir = Vector2.right; //���������� �̵�
                    if (Cam.GetComponent<Transform>().rotation.z <= -55 && Cam.GetComponent<Transform>().rotation.z > -140 )
                    {
                        Caracter.GetComponent<SpriteRenderer>().flipX = false; //ĳ���� ����
                    }
                    else
                    {
                        Caracter.GetComponent<SpriteRenderer>().flipX = true; // ĳ���� ����
                    }
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
            {//Destroy(gameObject);
                Debug.Log("die");
            }
            else if (other.gameObject.tag == "Coin")
            {
                audio = other.GetComponent<AudioSource>();
                coin = other.GetComponent<Animator>();

                GetCoin(other.gameObject.name);
                audio.Play();
                coin.SetBool("Get", true);

                Destroy(other.gameObject,3);
                Debug.Log(CoinCount);
            }
        }

        void GetCoin(string coinName)
        {
            if (coinName.Contains("GoldCoin"))
            {
                CoinCount += 5;
            }
            else if (coinName.Contains("SilverCoin"))
            {
                CoinCount += 3;
            }
            else if (coinName.Contains("BronzCoin"))
            {
                CoinCount += 1;
            }
        }
    }
}