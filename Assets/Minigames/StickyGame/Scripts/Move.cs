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
        [SerializeField] private float rotateSpeed; // ȸ���ӵ�
        [SerializeField] private Vector2 curAxis; // ���� ȸ�� �� ��ǥ
        [SerializeField] private int dir; // 1 : �ð�ݴ����, -1 : �ð����

        [SerializeField] private List<Transform> backRunners; // ������� associate�� backRunner��
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
                Invoke("SetBGM", 3.0f);
            }
        }

        private void SetBGM()
        {
            FindObjectOfType<SoundManager>().transform.GetChild(0).gameObject.SetActive(true);
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

            if (isPlayer && Input.GetButtonDown("Jump")) // Space Bar �Է½� ���� ��ȯ
            {
                SoundManager.Instance.PlaySpaceSound();
                //sound
                curAxis = 2 * (Vector2)transform.position - curAxis; // �� �߽� ����
                dir *= -1;  // �� ���� ����

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
                        if (collider.GetComponent<SpriteRenderer>().sortingOrder <= -5) // ���� ����
                        {
                            Die();
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
                else if (collider.CompareTag("Outline")) // ���� ����
                {
                    Die();
                }
                else if (collider.CompareTag("Coin"))
                {
                    SoundManager.Instance.PlayAcquireSound();
                    Instantiate(acquireEffect, collider.transform.position, acquireEffect.transform.rotation); // ȹ�� ����Ʈ
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
                else randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 0.0002f * statistics.curRunner; // ���� ��� ����

                if (randomDir.x > 0) spriteRenderer.flipX = true;
                else spriteRenderer.flipX = false;

                yield return wait;
            }
        }
        IEnumerator changeDirOfBackRunners() // backRunners�� ������ ���ʴ�� �ٲ��ִ� �Լ�
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

                curRunner.transform.position = prevPos; // ���� ����

                // ���� ����
                curRunner.curAxis = prevAxis;
                curRunner.dir = prevDir;  // �� ���� ����
            }
        }
        private void Associate(Collider2D collider) // �浹�� �� ���ڷ� Associate
        {
            Move moveCS = collider.GetComponent<Move>();
            
            if (backRunners.Count == 0)
            {
                collider.transform.position = transform.position;
                moveCS.isAssociated = true;
                moveCS.curAxis = curAxis;
                moveCS.dir = dir;
                collider.transform.RotateAround(curAxis, dir * Vector3.forward, -60); // 30����ŭ ������ ��ġ
            }
            else
            {
                Move lastRunner = backRunners[backRunners.Count - 1].GetComponent<Move>();

                collider.transform.position = lastRunner.transform.position;
                moveCS.isAssociated = true;
                moveCS.curAxis = lastRunner.curAxis;
                moveCS.dir = lastRunner.dir;
                collider.transform.RotateAround(lastRunner.curAxis, lastRunner.dir * Vector3.forward, -60); // 30����ŭ ������ ��ġ
            }

            moveCS.spriteRenderer.sortingOrder = -statistics.cumulRunner;

            StopCoroutine("RandomMove");
            moveCS.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
            backRunners.Add(collider.transform);
        }
        private void Die()
        {
            GetComponent<Animator>().SetTrigger("GameOver");
            FindObjectOfType<SpawnManager>().gameObject.SetActive(false);
            foreach (Move obj in FindObjectsOfType<Move>())
            {
                obj.rotateSpeed = 0;
            }
            FindObjectOfType<SoundManager>().transform.GetChild(0).gameObject.SetActive(false);
            SoundManager.Instance.PlayGameOverSound();
            ScoreManager.Instance.SetGameHighScore(MinigameType.StickyGame, statistics.score);
            GameResultManager.ShowResult(MinigameType.StickyGame, statistics.score);
        }
    }
}