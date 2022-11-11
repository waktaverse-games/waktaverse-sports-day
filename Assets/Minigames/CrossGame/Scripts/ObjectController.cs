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
        public List<GameObject> BackGround = new List<GameObject>();
        public List<Sprite> normalSprite = new List<Sprite>();

        [Header("Setting")]

        [Range(1f, 20f)]
        public float MovementSpeed;
        [Range(1f, 10f)]
        public float JumpSpeed;
        List<Platform> Platforms = new List<Platform>();
        [HideInInspector]
        public List<GameObject> Stars = new List<GameObject>();
        public float DefaultMovementSpeed;


        //플랫폼 이동
        [Header("Platform")]
        public float PlatformSpace;
        [Range(0f, 1f)]
        public float TrapProbability;
        [Range(0f, 1f)]
        public float StarProbability;
        [Range(0f, 1f)]
        public float ItemProbability;
        int PlatformCurosr = 0;
        int LandPlatformNum = 0, MakePlatformNum = 6;
        Dictionary<int, bool> PlatformInformation = new Dictionary<int, bool>();
        float MoveCount;
        bool CurrentFlatformIsActive;

        //점프
        float JumpTime;
        int JumpCount = 0;
        Vector3 LandPos;
        Sequence MoveSequence;
        bool PlayerPositionIsLimited;

        //배경
        float BackGroundMoveCount;

        //아이템
        [Header("Item")]
        public GameObject FlyItemPrefab;
        bool IsFly = false;
        float FlyCount = 0;
        float FlyCountInPlatform = 0;
        int FLyLandFlatformNum;
        int FlyDistance = 20;
        int FlatformNumWhenMakeFlyItem = -20;
        [HideInInspector]
        public List<GameObject> FlyItems = new List<GameObject>();

        private void Awake()
        {
            Vector3 ObjPos = new Vector3(-10, -3);
            for (int i = 0; i < 11; i++)
            {
                GameObject tmp = Instantiate(PlatformPrefab, ObjPos, Quaternion.identity, ObjectGroup);
                ObjPos += Vector3.right * 2;
                Platforms.Add(tmp.GetComponent<Platform>());
            }
            for (int i = 1; i <= 6; i++)
            {
                PlatformInformation.Add(i, true);
            }
        }

        private void Update()
        {
            if (Manager.IsOver)
            {
                return;
            }

            //점프
            if (Input.GetKeyDown(KeyCode.Space) && !IsFly)
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
            if (Input.GetKeyDown(KeyCode.C))
            {
                MakeFlyItem();
            }

            //플레이어 이동
            float DeltaDistance;
            if (IsFly)
            {
                DeltaDistance = 20 * Time.deltaTime;
                FlyCount += DeltaDistance;
                FlyCountInPlatform += DeltaDistance;
                if(FlyCountInPlatform > 2){
                    FlyCountInPlatform -= 2;
                    Manager.AddScore(10);
                }
                if (FlyCount > 40)
                {
                    FlyCount = 0;
                    EndFly();
                }
            }
            else
            {
                DeltaDistance = MovementSpeed * Time.deltaTime;
                Player.transform.position += Vector3.left * DeltaDistance;
            }

            MoveCount += DeltaDistance;

            //배경 이동
            BackGround[0].transform.position += Vector3.left * DeltaDistance * 0.3f;
            BackGround[1].transform.position += Vector3.left * DeltaDistance * 0.3f;
            BackGroundMoveCount += DeltaDistance * 0.3f;
            if (BackGroundMoveCount >= 26.78f)
            {
                BackGroundMoveCount -= 26.78f;
                BackGround[0].transform.position += Vector3.right * 26.78f;
                BackGround[1].transform.position += Vector3.right * 26.78f;
            }

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

            //아이템 이동
            for (int i = 0; i < Stars.Count; i++)
            {
                Stars[i].transform.position += Vector3.left * DeltaDistance;
            }

            for (int i = 0; i < FlyItems.Count; i++)
            {
                FlyItems[i].transform.position += Vector3.left * DeltaDistance * 1.5f;
            }

            if (IsFly) return;
            //오른쪽 벽에 근접한 상황에서 속도를 조절하는 로직
            if (JumpCount >= 1)
            {
                //다가갈 때
                if (Player.transform.position.x > 4 && !PlayerPositionIsLimited)
                {
                    DefaultMovementSpeed = MovementSpeed;
                    MovementSpeed = DefaultMovementSpeed * 0.5f + JumpSpeed * 4;
                    UpdateLandPoint(DefaultMovementSpeed, MovementSpeed);
                    PlayerPositionIsLimited = true;
                }
                //다시 멀어졌을 떄
                else if (Player.transform.position.x <= 4 && PlayerPositionIsLimited)
                {
                    UpdateLandPoint(MovementSpeed, DefaultMovementSpeed);
                    MovementSpeed = DefaultMovementSpeed;
                    PlayerPositionIsLimited = false;
                }
                //점프 시간 계산
                JumpTime += Time.deltaTime;
            }
            else
            {
                //점프하지 않은 상황에서 멀어졌을 때
                if (Player.transform.position.x <= 4 && PlayerPositionIsLimited)
                {
                    MovementSpeed = DefaultMovementSpeed;
                    PlayerPositionIsLimited = false;
                }
                //점프 시간 초기화
                JumpTime = 0;
            }
        }
        public void PlayerJump()
        {
            JumpCount++;
            LandPlatformNum++;
            //print(LandPlatformNum);
            Player.CntAnimator.SetFloat("JumpSpeed", 0.4f * JumpSpeed);
            Player.CntAnimator.SetTrigger("Jump");
            Manager.SoundManager.Play("Jump1");
            float Time = 1f / JumpSpeed;
            LandPos = Player.transform.position + Vector3.right * (2 - Time * MovementSpeed);
            //JumpSequence = Player.transform.DOJump(LandPos, 2f, 1, Time);
            Sequence UpDownSequence = DOTween.Sequence().Append(Player.transform.DOMoveY(LandPos.y + 2, Time / 2)).Append(Player.transform.DOMoveY(LandPos.y, Time / 2)).AppendCallback(() => {
                if (JumpCount == 1)
                {
                    JumpCallBack();
                }
            });
            MoveSequence = DOTween.Sequence().Append(Player.transform.DOMoveX(LandPos.x, Time)).Join(UpDownSequence);
        }

        public void UpdateLandPoint(float OldSpeed, float NewSpeed)
        {
            //MoveSequence.Kill();
            float Time = (1f / JumpSpeed) - JumpTime;
            LandPos = LandPos - Vector3.right * Time * (NewSpeed - OldSpeed);
            MoveSequence = DOTween.Sequence();
            MoveSequence.Append(Player.transform.DOMoveX(LandPos.x, Time)).AppendCallback(() =>
            {
                JumpCallBack();
            });
        }

        public void PlayerDoubleJump()
        {
            MoveSequence.Kill();
            JumpCount++;
            LandPlatformNum++;
            //print(LandPlatformNum);
            float Time = (1f / JumpSpeed) - JumpTime;
            float UpTime = Time - (1f / JumpSpeed) / 2;
            float DownTime = (1f / JumpSpeed) / 2;
            LandPos = LandPos + Vector3.right * 2;
            Sequence UpDownsequence = DOTween.Sequence().Append(Player.transform.DOMoveY(LandPos.y + 2.3f, UpTime)).Append(Player.transform.DOMoveY(LandPos.y, DownTime));
            MoveSequence = DOTween.Sequence();
            MoveSequence.Append(Player.transform.DOMoveX(LandPos.x, Time)).Join(UpDownsequence).AppendCallback(() =>
            {
                JumpCallBack();
            });
        }

        public void Fly()
        {
            if (IsFly) return;
            MoveSequence.Pause<Sequence>();
            Player.CntAnimator.SetBool("Fly", true);
            FLyLandFlatformNum = LandPlatformNum + FlyDistance;
            LandPlatformNum += FlyDistance;
            //print(FLyLandFlatformNum);
            IsFly = true;
        }

        public void EndFly()
        {
            MoveSequence.Play<Sequence>();
            Player.CntAnimator.SetBool("Fly", false);
            IsFly = false;
        }

        void JumpCallBack()
        {
            Manager.AddScore(JumpCount * 10);
            float num = Player.transform.position.x;
            num = Mathf.Round(num + MoveCount) - MoveCount;
            //0.1은 스프라이트 크기에 의한 보정
            Player.transform.position = new Vector3(num - 0.1f, Player.transform.position.y);

            if (!PlatformInformation[LandPlatformNum])
            {
                Manager.GameOver();
            }

            JumpCount = 0;
        }

        public void RepositionPlatform()
        {
            MakePlatformNum++;
            //print("m:" + MakePlatformNum);
            if (MakePlatformNum > 11) PlatformInformation.Remove(MakePlatformNum - 11);

            if (MakePlatformNum == FLyLandFlatformNum || !CurrentFlatformIsActive || Random.Range(0f, 1f) > TrapProbability)
            {
                Platforms[PlatformCurosr].gameObject.SetActive(true);
                CurrentFlatformIsActive = true;
                PlatformInformation.Add(MakePlatformNum, true);
                Platforms[PlatformCurosr].Set();
            }
            else
            {
                Platforms[PlatformCurosr].gameObject.SetActive(false);
                CurrentFlatformIsActive = false;
                PlatformInformation.Add(MakePlatformNum, false);
            }

            Platforms[PlatformCurosr++].transform.position += Vector3.right * PlatformSpace * 11;

            if (Platforms.Count <= PlatformCurosr) PlatformCurosr = 0;

            if ((MakePlatformNum > FlatformNumWhenMakeFlyItem + 30) && Random.Range(0f, 1f) < ItemProbability) MakeFlyItem();
            if (Random.Range(0f, 1f) < StarProbability) MakeStar();
        }

        public void MakeStar()
        {
            GameObject InstanceStar = Instantiate(StarPrefab, new Vector3(10, 1.7f), Quaternion.identity, ObjectGroup);
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

        public void MakeFlyItem()
        {
            FlatformNumWhenMakeFlyItem = MakePlatformNum;
            GameObject InstanceItem = Instantiate(FlyItemPrefab, new Vector3(11, 0.5f), Quaternion.identity, ObjectGroup);
            FlyItem InstanceScript = InstanceItem.GetComponent<FlyItem>();
            float num = Random.Range(0f, 1f);
            InstanceScript.Move();
            InstanceScript.Manager = Manager;
            FlyItems.Add(InstanceItem);
        }
    }
}

