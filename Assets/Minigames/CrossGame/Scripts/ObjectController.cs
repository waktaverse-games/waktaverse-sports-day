using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameHeaven.CrossGame
{
    public class ObjectController : MonoBehaviour
    {
        [Header("Objects")]
        public CrossGameManager manager;
        public GameObject platformPrefab;
        public Player player;
        public GameObject starPrefab;
        public Transform objectGroup;
        public List<GameObject> backGround = new List<GameObject>();
        public List<Sprite> normalSprite = new List<Sprite>();

        [Header("Setting")]

        [Range(1f, 20f)]
        public float movementSpeed;
        [Range(1f, 10f)]
        public float jumpSpeed;
        List<Platform> platforms = new List<Platform>();
        [HideInInspector]
        public List<GameObject> stars = new List<GameObject>();
        public float defaultMovementSpeed;


        //플랫폼 이동
        [Header("Platform")]
        public float platformSpace;
        [Range(0f, 1f)]
        public float trapProbability;
        [Range(0f, 1f)]
        public float starProbability;
        [Range(0f, 1f)]
        public float itemProbability;
        int platformCurosr = 0;
        int landPlatformNum = 0, makePlatformNum = 6;
        Dictionary<int, bool> platformInformation = new Dictionary<int, bool>();
        float moveCount;
        bool currentFlatformIsActive;

        //점프
        float jumpTime;
        int jumpCount = 0;
        bool readyJump = false;
        Vector3 landPos;
        Sequence moveSequence;
        Sequence upDownSequence;
        bool playerPositionIsLimited;
        float totalMoveCountLeft = 0;
        float totalMoveCountRight = 0;

        //배경
        float backGroundMoveCount;

        //아이템
        [Header("Item")]
        public GameObject flyItemPrefab;
        bool isFly = false;
        float flyCount = 0;
        float flyCountInPlatform = 0;
        int flyLandFlatformNum;
        int flyDistance = 20;
        int flatformNumWhenMakeFlyItem = -20;
        public Animator effect;
        [HideInInspector]
        public List<GameObject> flyItems = new List<GameObject>();

        private void Awake()
        {
            Vector3 ObjPos = new Vector3(-10, -3);
            for (int i = 0; i < 11; i++)
            {
                GameObject tmp = Instantiate(platformPrefab, ObjPos, Quaternion.identity, objectGroup);
                ObjPos += Vector3.right * 2;
                platforms.Add(tmp.GetComponent<Platform>());
            }
            for (int i = 1; i <= 6; i++)
            {
                platformInformation.Add(i, true);
            }
        }

        private void Update()
        {
            if (manager.IsStop)
            {
                return;
            }

            //점프 선입력
            if (readyJump && !isFly)
            {
                if (jumpCount == 0 && jumpTime == 0)
                {
                    PlayerJump();
                    readyJump = false;
                }
            }

            //점프
            if (Input.GetKeyDown(KeyCode.Space) && !isFly)
            {
                if (jumpCount == 0 && jumpTime == 0)
                {
                    PlayerJump();
                }
                else if (jumpCount == 1 && jumpTime * jumpSpeed < 0.5f)
                {
                    PlayerDoubleJump();
                }

                if (jumpTime * jumpSpeed > 0.7f)
                {
                    readyJump = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                effect.transform.position = player.transform.position;
                effect.SetTrigger("Boom");
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                MakeFlyItem();
            }

            //플레이어 이동
            float DeltaDistance;
            if (isFly)
            {
                DeltaDistance = 20 * Time.deltaTime;
                flyCount += DeltaDistance;
                flyCountInPlatform += DeltaDistance;
                if(flyCountInPlatform > 2){
                    flyCountInPlatform -= 2;
                    manager.AddScore(10);
                    Balanceing();
                }
                if (flyCount > 40)
                {
                    totalMoveCountLeft -= 40;
                    flyCount = 0;
                    EndFly();
                }
            }
            else
            {
                DeltaDistance = movementSpeed * Time.deltaTime;
                player.transform.position += Vector3.left * DeltaDistance;
            }

            moveCount += DeltaDistance;
            totalMoveCountLeft += DeltaDistance;

            //배경 이동
            backGround[0].transform.position += Vector3.left * DeltaDistance * 0.3f;
            backGround[1].transform.position += Vector3.left * DeltaDistance * 0.3f;
            backGroundMoveCount += DeltaDistance * 0.3f;
            if (backGroundMoveCount >= 26.78f)
            {
                backGroundMoveCount -= 26.78f;
                backGround[0].transform.position += Vector3.right * 26.78f;
                backGround[1].transform.position += Vector3.right * 26.78f;
            }

            //플랫폼 이동
            foreach (Platform tmp in platforms)
            {
                tmp.transform.position += Vector3.left * DeltaDistance;
            }
            if (moveCount >= 2)
            {
                moveCount -= 2;
                RepositionPlatform();
            }

            //아이템 이동
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].transform.position += Vector3.left * DeltaDistance;
            }

            for (int i = 0; i < flyItems.Count; i++)
            {
                flyItems[i].transform.position += Vector3.left * DeltaDistance * 1.5f;
            }

            //이펙트 이동
            if (-15 < effect.transform.position.x) effect.transform.position += Vector3.left * DeltaDistance;

            if (isFly) return;
            //오른쪽 벽에 근접한 상황에서 속도를 조절하는 로직
            if (jumpCount >= 1)
            {
                //다가갈 때
                if (player.transform.position.x > 4 && !playerPositionIsLimited)
                {
                    defaultMovementSpeed = movementSpeed;
                    movementSpeed = defaultMovementSpeed * 0.5f + jumpSpeed * 4;
                    UpdateLandPoint(defaultMovementSpeed, movementSpeed);
                    playerPositionIsLimited = true;
                }
                //다시 멀어졌을 떄
                else if (player.transform.position.x <= 4 && playerPositionIsLimited)
                {
                    UpdateLandPoint(movementSpeed, defaultMovementSpeed);
                    movementSpeed = defaultMovementSpeed;
                    playerPositionIsLimited = false;
                }
                //점프 시간 계산
                jumpTime += Time.deltaTime;
            }
            else
            {
                //점프하지 않은 상황에서 멀어졌을 때
                if (player.transform.position.x <= 4 && playerPositionIsLimited)
                {
                    movementSpeed = defaultMovementSpeed;
                    playerPositionIsLimited = false;
                }
                //점프 시간 초기화
                jumpTime = 0;
            }
        }
        public void PlayerJump()
        {
            jumpCount++;
            landPlatformNum++;
            totalMoveCountRight += 2;
            //print(LandPlatformNum);
            player.cntAnimator.SetFloat("JumpSpeed", 0.4f * jumpSpeed);
            player.cntAnimator.SetTrigger("Jump");
            manager.soundManager.Play("Jump1");
            float Time = 1f / jumpSpeed;
            landPos = player.transform.position + Vector3.right * (2 - Time * movementSpeed);
            //JumpSequence = Player.transform.DOJump(LandPos, 2f, 1, Time);
            upDownSequence = DOTween.Sequence().Append(player.transform.DOMoveY(landPos.y + 2, Time / 2)).Append(player.transform.DOMoveY(landPos.y, Time / 2));
            moveSequence = DOTween.Sequence().Append(player.transform.DOMoveX(landPos.x, Time)).AppendCallback(() => {
                if (jumpCount == 1)
                {
                    JumpCallBack();
                }
            });
        }

        public void UpdateLandPoint(float OldSpeed, float NewSpeed)
        {
            moveSequence.Kill();
            float Time = (1f / jumpSpeed) - jumpTime;
            landPos = landPos - Vector3.right * Time * (NewSpeed - OldSpeed);
            moveSequence = DOTween.Sequence();
            moveSequence.Append(player.transform.DOMoveX(landPos.x, Time)).AppendCallback(() =>
            {
                JumpCallBack();
            });
        }

        public void PlayerDoubleJump()
        {
            if (isFly)
            {
                return;
            }
            else
            {
                upDownSequence.Kill();
                moveSequence.Kill();
            }
            jumpCount++;
            landPlatformNum++;
            totalMoveCountRight += 2;
            //print(LandPlatformNum);
            float Time = (1f / jumpSpeed) - jumpTime;
            float UpTime = Time - (1f / jumpSpeed) / 2;
            float DownTime = (1f / jumpSpeed) / 2;
            landPos = landPos + Vector3.right * 2;
            upDownSequence = DOTween.Sequence().Append(player.transform.DOMoveY(landPos.y + 2.3f, UpTime)).Append(player.transform.DOMoveY(landPos.y, DownTime));
            moveSequence = DOTween.Sequence();
            moveSequence.Append(player.transform.DOMoveX(landPos.x, Time)).AppendCallback(() =>
            {
                JumpCallBack();
            });
        }

        public void Fly()
        {
            if (isFly) return;
            moveSequence.Pause<Sequence>();
            upDownSequence.Pause<Sequence>();
            effect.transform.position = player.transform.position;
            effect.SetTrigger("Boom");
            player.cntAnimator.SetBool("Fly", true);
            manager.soundManager.Play("PickUp1");
            flyLandFlatformNum = landPlatformNum + flyDistance;
            landPlatformNum += flyDistance;
            //print(FLyLandFlatformNum);
            isFly = true;
        }

        public void EndFly()
        {
            moveSequence.Play<Sequence>();
            upDownSequence.Play<Sequence>();
            effect.transform.position = player.transform.position;
            effect.SetTrigger("Boom");
            player.cntAnimator.SetBool("Fly", false);
            isFly = false;
        }

        void JumpCallBack()
        {
            manager.AddScore(jumpCount * 10);
            Balanceing();
            if (jumpCount == 2) Balanceing();
            //float num = Player.transform.position.x;
            //num = Mathf.Round(num + MoveCount) - MoveCount;
            float num = totalMoveCountRight - totalMoveCountLeft - 2.1f;
            //0.1은 스프라이트 크기에 의한 보정
            player.transform.position = new Vector3(num, -1.7f);

            if (landPlatformNum <= makePlatformNum && !platformInformation[landPlatformNum])
            {
                manager.GameOver();
            }

            jumpCount = 0;
        }

        public void RepositionPlatform()
        {
            makePlatformNum++;
            //print("m:" + MakePlatformNum);
            if (makePlatformNum > 11) platformInformation.Remove(makePlatformNum - 11);

            if (makePlatformNum == flyLandFlatformNum || !currentFlatformIsActive || Random.Range(0f, 1f) > trapProbability)
            {
                platforms[platformCurosr].gameObject.SetActive(true);
                currentFlatformIsActive = true;
                platformInformation.Add(makePlatformNum, true);
                platforms[platformCurosr].Set();
            }
            else
            {
                platforms[platformCurosr].gameObject.SetActive(false);
                currentFlatformIsActive = false;
                platformInformation.Add(makePlatformNum, false);
            }

            platforms[platformCurosr++].transform.position += Vector3.right * platformSpace * 11;

            if (platforms.Count <= platformCurosr) platformCurosr = 0;

            if ((makePlatformNum > flatformNumWhenMakeFlyItem + 30) && Random.Range(0f, 1f) < itemProbability) MakeFlyItem();
            if (Random.Range(0f, 1f) < starProbability) MakeStar(currentFlatformIsActive);
        }

        public void MakeStar(bool CurrentFlatformIsActive)
        {
            GameObject InstanceStar = Instantiate(starPrefab, new Vector3(10, 1.3f), Quaternion.identity, objectGroup);
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
            num = Random.Range(0f, 1f);
            if (num < 0.5f && CurrentFlatformIsActive)
            {
                InstanceStar.transform.position += new Vector3(0, -3.2f, 0);
            }
            InstanceScript.SetAnim();
            InstanceScript.Move();
            InstanceScript.manager = manager;
            stars.Add(InstanceStar);
        }

        public void MakeFlyItem()
        {
            flatformNumWhenMakeFlyItem = makePlatformNum;
            GameObject InstanceItem = Instantiate(flyItemPrefab, new Vector3(11, 0.5f), Quaternion.identity, objectGroup);
            FlyItem InstanceScript = InstanceItem.GetComponent<FlyItem>();
            float num = Random.Range(0f, 1f);
            InstanceScript.Move();
            InstanceScript.manager = manager;
            flyItems.Add(InstanceItem);
        }

        void Balanceing()
        {
            if (movementSpeed < 6.5f)
            {
                movementSpeed += 0.03f;
            }
            else if (movementSpeed < 8.8f)
            {
                movementSpeed += 0.005f;
            }
        }
    }
}

