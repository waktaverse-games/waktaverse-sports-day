using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameHeaven.CrossGame
{
    public class ObjectController : MonoBehaviour
    {
        public GameObject PlatformPrefab;
        public float PlatformSpace;
        public GameObject Player;
        public List<Platform> Platforms = new List<Platform>();

        public float Speed;

        int PlatformCurosr = 0;
        float MoveCount;
        public bool AllowJump;
        Sequence JumpSequence;

        float num = 0;
        int frame = 0;
        private void Awake()
        {
            Vector3 ObjPos = new Vector3(-10, -3);
            for (int i = 0; i < 11; i++)
            {
                GameObject tmp = Instantiate(PlatformPrefab, ObjPos, Quaternion.identity);
                ObjPos += Vector3.right * 2;
                Platforms.Add(tmp.GetComponent<Platform>());
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && AllowJump)
            {
                Jump();
                AllowJump = false;
            }
            
            float DeltaDistance = Speed * Time.deltaTime;
            MoveCount += DeltaDistance;
            Player.transform.position += Vector3.left * DeltaDistance;
            
            foreach (Platform tmp in Platforms)
            {
                tmp.transform.position += Vector3.left * DeltaDistance;
            }
            if(MoveCount >= 2)
            {
                MoveCount -= 2;
                RepositionPlatform();
            }
        }

        public void Jump()
        {
            Vector3 LandPos = Player.transform.position + Vector3.right;
            JumpSequence = Player.transform.DOJump(LandPos, 3f, 1, 1f/Speed).AppendCallback(()=> {
                //오차 보정
                float num = Player.transform.position.x;
                num = Mathf.Round(num + MoveCount) - MoveCount;
                Player.transform.position = new Vector3(num + 0.035f, Player.transform.position.y);

                AllowJump = true;
            }).SetAutoKill(false);
            Tweener CircleMoveTween = transform.DOMove(LandPos, 1).SetAutoKill(false);
        }

        public void RepositionPlatform()
        {
            Platforms[PlatformCurosr++].transform.position += Vector3.right * PlatformSpace * 11;
            if (Platforms.Count <= PlatformCurosr) PlatformCurosr = 0;
        }
    }
}

