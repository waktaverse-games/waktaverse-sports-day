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

        [Range(1f, 10f)]
        public float MovementSpeed;
        [Range(1f, 10f)]
        public float JumpSpeed;
        List<Platform> Platforms = new List<Platform>();
        [HideInInspector]
        public List<GameObject> Stars = new List<GameObject>();
        public float ChangedMovementSpeed;


        //�÷��� �̵� ����
        [Header("Platform")]
        public float PlatformSpace;
        [Range(0f, 1f)]
        public float TrapProbability;
        int PlatformCurosr = 0;
        float MoveCount;
        bool CurrentFlatformIsActive;
        
        //�÷��̾� �̵� ����
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
            //�÷��̾� ����
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (JumpCount == 0 && JumpTime == 0)
                {
                    PlayerJump();
                }
                else if (JumpCount == 1 && JumpTime * JumpSpeed < 0.75f)
                {
                    PlayerDoubleJump();
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                MakeStar();
            }

            //���� ���� �� ó��
            if (JumpCount >= 1)
            {
                //������ ���� ��������� �� ������ ����
                if (Player.transform.position.x > 4 && !PlayerPositionIsLimited)
                {
                    ChangedMovementSpeed = MovementSpeed * 0.5f + JumpSpeed * 4;
                    UpdateLandPoint(MovementSpeed, ChangedMovementSpeed);
                    PlayerPositionIsLimited = true;
                }
                //������ ���� ����
                else if (Player.transform.position.x <= 4 && PlayerPositionIsLimited)
                {
                    UpdateLandPoint(ChangedMovementSpeed, MovementSpeed);
                    ChangedMovementSpeed = MovementSpeed;
                    PlayerPositionIsLimited = false;
                }
                //������ ���ӵ� �ð� üũ
                JumpTime += Time.deltaTime;
            }
            else
            {
                //������ ���� ����
                if (Player.transform.position.x <= 4 && PlayerPositionIsLimited)
                {
                    ChangedMovementSpeed = MovementSpeed;
                    PlayerPositionIsLimited = false;
                }
                //������ ���ӵ� �ð� �ʱ�ȭ
                JumpTime = 0;
            }

            //�÷��̾� �̵�
            float DeltaDistance = ChangedMovementSpeed * Time.deltaTime;
            MoveCount += DeltaDistance;
            Player.transform.position += Vector3.left * DeltaDistance;

            //�÷��� �̵�
            foreach (Platform tmp in Platforms)
            {
                tmp.transform.position += Vector3.left * DeltaDistance;
            }
            if (MoveCount >= 2)
            {
                MoveCount -= 2;
                RepositionPlatform();
            }

            //�� �̵�
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
            float Time = 1f / JumpSpeed;
            LandPos = Player.transform.position + Vector3.right * (2 - Time * ChangedMovementSpeed);
            JumpSequence = Player.transform.DOJump(LandPos, 3f, 1, Time);
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
            //���� ����
            float num = Player.transform.position.x;
            num = Mathf.Round(num + MoveCount) - MoveCount;
            //�Ʒ� 0.1f�� �÷��̾�� �÷��� ��������Ʈ ũ�� ���̿� ���� ����(��������Ʈ�� �ٲ�� ���� �ٲ�)
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
            InstanceScript.Move();
            InstanceScript.Manager = Manager;
            Stars.Add(InstanceStar);
        }
    }
}

