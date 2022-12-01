using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SharedLibs;
using SharedLibs.Score;

namespace GameHeaven.StickyGame
{
    public class Move : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed; // 회전속도
        [SerializeField] private Vector2 curAxis; // 현재 회전 축 좌표
        [SerializeField] private int dir; // 1 : 시계반대방향, -1 : 시계방향

        [SerializeField] private List<Transform> backRunners; // 현재까지 associate한 backRunner들
        public bool isPlayer, isAssociated;
        public float cumulativeCoin;
        private SpriteRenderer spriteRenderer;
        [SerializeField] private Vector2 randomDir;
        [SerializeField] private bool isDeath;
        [SerializeField] private GameObject acquireEffect;

        private Statistics statistics;
        private bool moveLock = false;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            statistics = FindObjectOfType<Statistics>();

            curAxis = Vector2.zero;
            dir = 1;

            backRunners = new List<Transform>();

            if (!isAssociated)
            {
                StartCoroutine(RandomMove(0.5f));
            }

            if (isPlayer)
            {
                moveLock = true;
                Invoke("ResetMoveLock", 3.0f);
            }
        }

        void ResetMoveLock()
        {
            moveLock = false;
        }

        private void Update()
        {
            if (moveLock) return;
            if (isAssociated)
            {
                if (transform.position.y > curAxis.y)
                {
                    if (dir > 0) spriteRenderer.flipX = false;
                    else spriteRenderer.flipX = true;
                }
                else
                {
                    if (dir > 0) spriteRenderer.flipX = true;
                    else spriteRenderer.flipX = false;
                }
                transform.RotateAround(curAxis, dir * Vector3.forward, Time.deltaTime * rotateSpeed);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.position = new Vector2(Mathf.Clamp(transform.position.x + randomDir.x, -3.5f, 3.5f), 
                    Mathf.Clamp(transform.position.y + randomDir.y, -4f, 4f));
            }

            if (isPlayer && Input.GetButtonDown("Jump")) // Space Bar 입력시 방향 전환
            {
                SoundManager.Instance.PlaySpaceSound();
                //sound
                curAxis = 2 * (Vector2)transform.position - curAxis; // 축 중심 변경
                dir *= -1;  // 축 방향 변경

                if (backRunners.Count > 0)
                {
                    StartCoroutine(changeDirOfBackRunners());
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (isPlayer)
            {
                if (collider.CompareTag("Runner"))
                {
                    if (collider.GetComponent<Move>().isAssociated)
                    {
                        if (collider.GetComponent<SpriteRenderer>().sortingOrder <= -5) // 게임 오버
                        {
                            Time.timeScale = 0;
                            ScoreManager.Instance.SetGameHighScore(MinigameType.StickyGame, statistics.score);
                            ResultSceneManager.ShowResult(MinigameType.StickyGame);
                        }
                    }
                    else
                    {
                        SoundManager.Instance.PlayAssociateSound();
                        statistics.score += 70 + 20 * statistics.curRunner;
                        statistics.cumulRunner++;
                        statistics.curRunner++;
                        Associate(collider);
                    }
                }
                else if (collider.CompareTag("Outline")) // 게임 오버
                {
                    Time.timeScale = 0;
                    ScoreManager.Instance.SetGameHighScore(MinigameType.StickyGame, statistics.score);
                    ResultSceneManager.ShowResult(MinigameType.StickyGame);
                }
                else if (collider.CompareTag("Coin"))
                {
                    SoundManager.Instance.PlayAcquireSound();
                    Instantiate(acquireEffect, collider.transform.position, acquireEffect.transform.rotation); // 획득 이펙트
                    statistics.score += 70 + 20 * statistics.curRunner;
                    if (collider.name[0] == 'G') statistics.goldCoin++;
                    else if (collider.name[0] == 'S') statistics.silverCoin++;
                    else if (collider.name[0] == 'B') statistics.bronzeCoin++;

                    Destroy(collider.gameObject);
                }
                else if (collider.CompareTag("CutItem"))
                {
                    SoundManager.Instance.PlayCutSound();
                    statistics.score += 70;
                    if (statistics.curRunner != 0) statistics.curRunner--;
                    Destroy(collider.gameObject);
                    if (backRunners.Count > 0)
                    {
                        GameObject del = backRunners[backRunners.Count - 1].gameObject;
                        backRunners.RemoveAt(backRunners.Count - 1);
                        Destroy(del);
                    }
                }
            }
        }

        IEnumerator RandomMove(float sec)
        {
            WaitForSeconds wait = new WaitForSeconds(sec);

            while (!isAssociated)
            {
                if (Random.Range(0, 2) == 0) randomDir = Vector2.zero;
                else randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 0.0002f * statistics.curRunner; // 점수 비례 증가

                if (randomDir.x > 0) spriteRenderer.flipX = true;
                else spriteRenderer.flipX = false;

                yield return wait;
            }
        }
        IEnumerator changeDirOfBackRunners() // backRunners의 방향을 차례대로 바꿔주는 함수
        {
            WaitForSeconds wait = new WaitForSeconds(60 / rotateSpeed);

            for (int idx = 0; idx < backRunners.Count; idx++)
            {
                Vector3 prevAxis = curAxis, prevPos = transform.position;
                int prevDir = dir;

                if (idx > 0)
                {
                    prevAxis = backRunners[idx - 1].GetComponent<Move>().curAxis;
                    prevPos = backRunners[idx - 1].position;
                    prevDir = backRunners[idx - 1].GetComponent<Move>().dir;
                }

                yield return wait;

                Move curRunner = backRunners[idx].GetComponent<Move>();

                curRunner.transform.position = prevPos; // 오차 정정

                // 방향 변경
                curRunner.curAxis = prevAxis;
                curRunner.dir = prevDir;  // 축 방향 변경
            }
        }
        private void Associate(Collider2D collider) // 충돌시 뒤 주자로 Associate
        {
            Move moveCS = collider.GetComponent<Move>();
            
            if (backRunners.Count == 0)
            {
                collider.transform.position = transform.position;
                moveCS.isAssociated = true;
                moveCS.curAxis = curAxis;
                moveCS.dir = dir;
                collider.transform.RotateAround(curAxis, dir * Vector3.forward, -60); // 30도만큼 이전에 위치
            }
            else
            {
                Move lastRunner = backRunners[backRunners.Count - 1].GetComponent<Move>();

                collider.transform.position = lastRunner.transform.position;
                moveCS.isAssociated = true;
                moveCS.curAxis = lastRunner.curAxis;
                moveCS.dir = lastRunner.dir;
                collider.transform.RotateAround(lastRunner.curAxis, lastRunner.dir * Vector3.forward, -60); // 30도만큼 이전에 위치
            }

            moveCS.spriteRenderer.sortingOrder = -statistics.cumulRunner;

            StopCoroutine("RandomMove");
            moveCS.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
            backRunners.Add(collider.transform);
        }
    }
}