using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibs;

namespace GameHaven.RunGame
{
    public class PlayerControl : MonoBehaviour
    {

        public GameObject Caracter;
        public GameObject Effect;
        public GameObject Cam;
        public GameManager gameManager;

        [SerializeField]
        float Speed;
        [SerializeField]
        float ChangeSpeed;

        float Camsize;
        Vector3 Carictorsize;
        float ItemTime = 0;
        public bool GetItem = false;



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
            Speed = 2.5f;
            run.SetBool("Stop", true);
        }

        // Update is called once per frame
        void Update()
        {

            if (gameManager.gameStop == false && gameManager.gameStart == true)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (Dirleft) //왼쪽을 보고 있으면
                    {
                        Dirleft = false; //왼쪽X
                        dir = Vector2.right; //오른쪽으로 이동
                        Caracter.GetComponent<SpriteRenderer>().flipX = true; //캐릭터 좌측
                        Effect.GetComponent<SpriteRenderer>().flipX = false; //캐릭터 좌측
                        
                    }
                    else
                    {
                        Dirleft = true;
                        dir = Vector2.left;
                        Caracter.GetComponent<SpriteRenderer>().flipX = false;
                        Effect.GetComponent<SpriteRenderer>().flipX = true; //캐릭터 좌측

                    }
                }

                if (GetItem == true)
                {
                    ItemTime += Time.deltaTime;
                    Caracter.GetComponent<CapsuleCollider2D>().enabled = false;
                    Effect.SetActive(true);

                    if (ItemTime > 2 && ItemTime <= 2.7)
                    {
                        Camsize = Cam.GetComponent<Camera>().orthographicSize;
                        Carictorsize = Caracter.GetComponent<Transform>().localScale;
                        Cam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(Camsize, 6.5f, Time.deltaTime * 2f);
                        Caracter.GetComponent<Transform>().localScale = Vector3.Lerp(Carictorsize, new Vector3(2, 2, 0), Time.deltaTime * 4f);
                    }
                    else if (ItemTime > 2.7)
                    {
                        ItemTime = 0;
                        Caracter.GetComponent<CapsuleCollider2D>().enabled = true;
                        GetItem = false;
                        Effect.SetActive(false);
                        run.SetBool("Jump", false);
                    }
                    else
                    {
                        Camsize = Cam.GetComponent<Camera>().orthographicSize;
                        Carictorsize = Caracter.GetComponent<Transform>().localScale;
                        Cam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(Camsize, 9f, Time.deltaTime * 1.8f);
                        Caracter.GetComponent<Transform>().localScale = Vector3.Lerp(Carictorsize, new Vector3(3.5f, 3.5f, 0), Time.deltaTime * 1.8f);
                    }
                }

                if (gameManager.gameTime < 30)
                {
                    Speed = Mathf.Lerp(Speed, 5f, Time.deltaTime * 0.04f);
                }
                else if (gameManager.gameTime >= 30 && gameManager.gameTime < 60)
                {
                    Speed = Mathf.Lerp(Speed, 8f, Time.deltaTime * 0.07f);
                }
                else
                    Speed = Mathf.Lerp(Speed, 8.5f, Time.deltaTime * 0.005f);

                Caracter.GetComponent<Rigidbody2D>().velocity = dir * Speed;

            }
            else if (gameManager.gameStop == true)
            {
                Caracter.GetComponent<Rigidbody2D>().velocity = dir * 0;
                run.SetBool("Over", true);
            }

            
           /* Vector3 pos1 = Camera.main.WorldToViewportPoint(Caracter.GetComponent<Transform>().position + new Vector3(-1,-1,0));
            Vector3 pos2 = Camera.main.WorldToViewportPoint(Caracter.GetComponent<Transform>().position + new Vector3(1, -1, 0));
            Vector3 pos3 = Camera.main.WorldToViewportPoint(Caracter.GetComponent<Transform>().position + new Vector3(1, 1, 0));
            Vector3 pos4 = Camera.main.WorldToViewportPoint(Caracter.GetComponent<Transform>().position + new Vector3(-1, 1, 0));
            Vector3 pos5 = Camera.main.WorldToViewportPoint(Caracter.GetComponent<Transform>().position);
            pos1 = CameraOut(pos1);
            pos2 = CameraOut(pos2);
            pos3 = CameraOut(pos3);
            pos4 = CameraOut(pos4);

            Caracter.GetComponent<Transform>().position = Camera.main.ViewportToWorldPoint((pos1+pos2+pos3+pos4)/4);*/

        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                gameManager.GameOver();
            }
            else if (other.gameObject.tag == "Coin")
            {
                audio = other.GetComponent<AudioSource>();
                coin = other.GetComponent<Animator>();
                audio.volume = SoundManager.Instance.SFXVolume;
                audio.Play();
                coin.SetBool("Get", true);

                Destroy(other.gameObject, 3);

                if (other.gameObject.name.Contains("PungSin"))
                {
                    GetItem = true;
                    gameManager.ItemScore(15);
                    run.SetBool("Jump", true);
                }
                else
                {
                    gameManager.ItemScore(5);
                }
            }
        }

        public void GameStart()
        {
            run.SetBool("Stop", false);
        }

        /*public Vector3 CameraOut(Vector3 pos)
        {
            if (pos.x < 0f)
            {
                pos.x = 0.000001f;
                dir.x = 0f;
            }
            if (pos.x > 1f)
            {
                pos.x = 0.999999f;
                dir.x = 0f;
            }

            if (pos.y < 0f)
            {
                pos.y = 0.000001f;
                dir.x = 0f;
            }

            if (pos.y > 1f)
            {
                pos.y = 0.999999f;
                dir.x = 0f;
            }

            return pos;

        }*/

    }
}