using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameHeaven.CrossGame
{
    public class ObjectController : MonoBehaviour
    {
        [Header("Objects")]
        public CrossGameManager Manager;

        public GameObject PlatformPrefab;
        public Player Player;
        public GameObject StarPrefab;

        public Transform ObjectGroup;

        [Header("Setting")]

        [Range(1f, 20f)]
        public float MovementSpeed;
        [Range(1f, 10f)]
        public float JumpSpeed;
        List<Platform> Platforms = new List<Platform>();
        [HideInInspector]
        public List<GameObject> Stars = new List<GameObject>();

        public float DefaultMovementSpeed;


        //플랫폼 이동 관련
        [Header("Platform")]
        public float PlatformSpace;
        [Range(0f, 1f)]
        public float TrapProbability;
        int PlatformCurosr = 0;
        float MoveCount;
        bool CurrentFlatformIsActive;

        //플레이어 이동 관련
        float JumpTime;
        int JumpCount = 0;
        Vector3 LandPos;
        Sequence JumpSequence;
        bool PlayerPositionIsLimited;

        private void Awake()
        {
            Vector3 ObjPos = new Vector3(-10, -3);
            for (int i = 0; i < 11; i++)
            {
                GameObject tmp = Instantiate(PlatformPrefab, ObjPos, Quaternion.identity, ObjectGroup);
                ObjPos += Vector3.right * 2;
                Platforms.Add(tmp.GetComponent<Platform>());
            }
        }

        private void Update()
        {
            if (Manager.IsOver)
            {
                return;
            }
            //플레이어 점프
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (JumpCount == 0 && JumpTime == 0)
                {
                    PlayerJump();
                }
                else if (JumpCount == 1 && JumpTime * JumpSpeed < 0.5f)
                {
                    PlayerDoubleJump();
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                MakeStar();
            }

            //점프 중일 때 처리
            if (JumpCount >= 1)
            {
                //오른쪽 벽에 가까워졌을 때 포지션 제한
                if (Player.transform.position.x > 4 && !PlayerPositionIsLimited)
                {
                    DefaultMovementSpeed = MovementSpeed;
                    MovementSpeed = DefaultMovementSpeed * 0.5f + JumpSpeed * 4;
                    UpdateLandPoint(DefaultMovementSpeed, MovementSpeed);
                    PlayerPositionIsLimited = true;
                }
                //포지션 제한 해제
                else if (Player.transform.position.x <= 4 && PlayerPositionIsLimited)
                {
                    UpdateLandPoint(MovementSpeed, DefaultMovementSpeed);
                    MovementSpeed = DefaultMovementSpeed;
                    PlayerPositionIsLimited = false;
                }
                //점프가 지속된 시간 체크
                JumpTime += Time.deltaTime;
            }
            else
            {
                //포지션 제한 해제
                if (Player.transform.position.x <= 4 && PlayerPositionIsLimited)
                {
                    MovementSpeed = DefaultMovementSpeed;
                    PlayerPositionIsLimited = false;
                }
                //점프가 지속된 시간 초기화
                JumpTime = 0;
            }

            //플레이어 이동
            float DeltaDistance = MovementSpeed * Time.deltaTime;
            MoveCount += DeltaDistance;
            Player.transform.position += Vector3.left * DeltaDistance;

            //플랫폼 이동
            foreach (Platform tmp in Platforms)
            {
                tmp.transform.position += Vector3.left * DeltaDistance;
            }
            if (MoveCount >= 2)
            {
                MoveCount -= 2;
                RepositionPlatform();
            }

            //별 이동
            for (int i = 0; i < Stars.Count; i++)
            {
                Stars[i].transform.position += Vector3.left * DeltaDistance;
                if (Stars[i].transform.position.x < -11)
                {
                    GameObject tmp = Stars[i--];
                    Stars.Remove(tmp);
                    tmp.GetComponent<Star>().Kill();
                }
            }
        }
        public void PlayerJump()
        {
            JumpCount++;
            Player.CntAnimator.SetFloat("JumpSpeed", 0.4f * JumpSpeed);
            Player.CntAnimator.SetTrigger("Jump");
            float Time = 1f / JumpSpeed;
            LandPos = Player.transform.position + Vector3.right * (2 - Time * MovementSpeed);
            JumpSequence = Player.transform.DOJump(LandPos, 2f, 1, Time);
            JumpSequence.AppendCallback(() => {
                if (JumpCount == 1)
                {
                    JumpCallBack();
                    Manager.AddScore(10);
                }
            });
        }

        public void UpdateLandPoint(float OldSpeed, float NewSpeed)
        {
            float Time = (1f / JumpSpeed) - JumpTime;
            LandPos = LandPos - Vector3.right * Time * (NewSpeed - OldSpeed);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(Player.transform.DOMoveX(LandPos.x, Time)).AppendCallback(() =>
            {
                JumpCallBack();
            });
        }

        public void PlayerDoubleJump()
        {
            JumpCount++;
            float Time = (1f / JumpSpeed) - JumpTime;
            LandPos = LandPos + Vector3.right * 2;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(Player.transform.DOMoveX(LandPos.x, Time)).AppendCallback(() =>
            {
                JumpSequence.Kill();
                JumpCallBack();
                Manager.AddScore(20);
            });
        }



        void JumpCallBack()
        {
            //오차 보정
            float num = Player.transform.position.x;
            num = Mathf.Round(num + MoveCount) - MoveCount;
            //아래 0.1f는 플레이어와 플랫폼 스프라이트 크기 차이에 의한 보정(스프라이트가 바뀌면 같이 바뀜)
            Player.transform.position = new Vector3(num - 0.1f, Player.transform.position.y);

            if (!Player.OnBottom)
            {
                Manager.GameOver();
            }

            JumpCount = 0;
        }

        public void RepositionPlatform()
        {
            if (!CurrentFlatformIsActive || Random.Range(0f, 1f) > TrapProbability)
            {
                Platforms[PlatformCurosr].gameObject.SetActive(true);
                CurrentFlatformIsActive = true;
            }
            else
            {
                Platforms[PlatformCurosr].gameObject.SetActive(false);
                CurrentFlatformIsActive = false;
            }

            Platforms[PlatformCurosr++].transform.position += Vector3.right * PlatformSpace * 11;

            if (Platforms.Count <= PlatformCurosr) PlatformCurosr = 0;
        }

        public void MakeStar()
        {
            GameObject InstanceStar = Instantiate(StarPrefab, new Vector3(11, -2), Quaternion.identity, ObjectGroup);
            Star InstanceScript = InstanceStar.GetComponent<Star>();
            float num = Random.Range(0f, 1f);
            if (num < 0.5f)
            {
                InstanceScript.code = CoinCode.Bronze;
            }
            else if (num < 0.75f)
            {
                InstanceScript.code = CoinCode.Silver;
            }
            else
            {
                InstanceScript.code = CoinCode.Gold;
            }
            InstanceScript.SetAnim();
            InstanceScript.Move();
            InstanceScript.Manager = Manager;
            Stars.Add(InstanceStar);
        }
    }
}

