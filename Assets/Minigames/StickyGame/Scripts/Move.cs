using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        private Rigidbody2D rigid;
        private SpriteRenderer spriteRenderer;
        private Animator anim;

        private Statistics statistics;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();

            statistics = FindObjectOfType<Statistics>();

            curAxis = Vector2.zero;
            dir = 1;

            backRunners = new List<Transform>();

            if (isPlayer) anim.SetBool("Walk", true);
        }

        private void Update()
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

            if (isAssociated)
            {
                transform.RotateAround(curAxis, dir * Vector3.forward, Time.deltaTime * rotateSpeed);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (isPlayer && Input.GetButtonDown("Jump")) // Space Bar �Է½� ���� ��ȯ
            {
                curAxis = 2 * (Vector2)transform.position - curAxis; // �� �߽� ����
                dir *= -1;  // �� ���� ����

                if (backRunners.Count > 0)
                {
                    StartCoroutine(changeDirOfBackRunners(0));
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
                        print("RunnerGameOver");
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                    else
                    {
                        statistics.cumulRunner++;
                        statistics.curRunner++;
                        Associate(collider);
                    }
                }
                else if (collider.CompareTag("Outline"))
                {
                    print("BorderGameOver");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                else if (collider.CompareTag("Coin"))
                {
                    if (collider.name[0] == 'G') statistics.goldCoin++;
                    else if (collider.name[0] == 'S') statistics.silverCoin++;
                    else if (collider.name[0] == 'B') statistics.bronzeCoin++;

                    Destroy(collider.gameObject);
                }
                else if (collider.CompareTag("CutItem"))
                {
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

        IEnumerator changeDirOfBackRunners(int idx) // backRunners�� ������ ���ʴ�� �ٲ��ִ� �Լ�
        {
            Vector3 prevAxis = curAxis;

            if (idx > 0) prevAxis = backRunners[idx - 1].GetComponent<Move>().curAxis;

            yield return new WaitForSeconds(60 / rotateSpeed);

            Move curRunner = backRunners[idx].GetComponent<Move>();

            // ���� ����
            curRunner.curAxis = prevAxis;
            curRunner.dir *= -1;  // �� ���� ����

            if (++idx < backRunners.Count) // ������ ���ڰ� �ƴ� ������ �ڷ�ƾ �ݺ�
            {
                StartCoroutine(changeDirOfBackRunners(idx));
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

            moveCS.anim.SetBool("Walk", true);
            backRunners.Add(collider.transform);
        }
    }
}