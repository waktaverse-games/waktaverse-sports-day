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

        float Camsize;
        Vector3 Carictorsize;
        float ItemTime = 0;
        public static bool GetItem = false;

        int CoinCount = 0;

        Vector2 dir = Vector2.left;
        bool Dirleft = true;

        Animator coin;
        Animator run;

        AudioSource audio;

        // Start is called before the first frame update
        void Start()
        {
            run = Caracter.GetComponent<Animator>();

        }

        // Update is called once per frame
        void Update()
        {
            

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Dirleft) //왼쪽을 보고 있으면
                {
                    Dirleft = false; //왼쪽X
                    dir = Vector2.right; //오른쪽으로 이동
                    if (Cam.GetComponent<Transform>().rotation.z <= -55 && Cam.GetComponent<Transform>().rotation.z > -140 )
                    {
                        Caracter.GetComponent<SpriteRenderer>().flipX = false; //캐릭터 좌측
                    }
                    else
                    {
                        Caracter.GetComponent<SpriteRenderer>().flipX = true; // 캐릭터 우측
                    }
                }
                else
                {
                    Dirleft = true;
                    dir = Vector2.left;
                    Caracter.GetComponent<SpriteRenderer>().flipX = false;
                }
            }

            if (GetItem == true)
            {
                ItemTime += Time.deltaTime;
                Caracter.GetComponent<CapsuleCollider2D>().enabled = false;

                if (ItemTime > 2 && ItemTime <=2.7)
                {
                    Camsize = Cam.GetComponent<Camera>().orthographicSize;
                    Carictorsize = Caracter.GetComponent<Transform>().localScale;
                    Cam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(Camsize, 6.5f, Time.deltaTime * 2f);
                    Caracter.GetComponent<Transform>().localScale = Vector3.Lerp(Carictorsize, new Vector3(2, 2, 0), Time.deltaTime * 4f);
                }
                else if (ItemTime >2.7 )
                {
                    ItemTime = 0;
                    Caracter.GetComponent<CapsuleCollider2D>().enabled = true;
                    GetItem = false;
                    run.SetBool("Stop", false);
                }
                else
                {
                    Camsize = Cam.GetComponent<Camera>().orthographicSize;
                    Carictorsize = Caracter.GetComponent<Transform>().localScale;
                    Cam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(Camsize, 9f, Time.deltaTime * 1.8f);
                    Caracter.GetComponent<Transform>().localScale = Vector3.Lerp(Carictorsize, new Vector3(3.5f, 3.5f, 0), Time.deltaTime * 1.8f);
                }
            }

            Caracter.GetComponent<Rigidbody2D>().velocity = dir * Speed;

            
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {//Destroy(gameObject);
                GameManager.instance.GameOver();
            }
            else if (other.gameObject.tag == "Coin")
            {
                audio = other.GetComponent<AudioSource>();
                coin = other.GetComponent<Animator>();
                audio.Play();
                coin.SetBool("Get", true);

                Destroy(other.gameObject, 3);

                if (other.gameObject.name.Contains("PungSin"))
                {
                    GetItem = true;
                    GameManager.instance.ItemScore(15);
                    run.SetBool("Stop", true);
                }
                else
                {
                    GameManager.instance.ItemScore(5);
                }
            }
        }
    }
}