using System;
using System.Collections.Generic;
using SharedLibs;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace GameHeaven.PunctureGame
{
    public enum EnemyType
    {
        Panzee, Gorani, Segyun, Dulgi, PoopPuppy, Bat, Jupokdo
    }

    [Serializable]
    public class EnemyData
    {
        public EnemyType type;
        public int layerIndex;
    }
    
    public class Enemy : TwoAxisEntity
    {
        [SerializeField] private List<EnemyData> enemies;

        [ReadOnly] public EnemyType enemyType;

        public IObjectPool<Enemy> enemyPool;
        [ReadOnly] public PlayerController playerController;

        [SerializeField] private float releaseHeight;
        [SerializeField] private float respawnOppositeWidth;

        [SerializeField] private SpriteRenderer spRender;
        [SerializeField] private Animator anim;
        private static int layerCount = 2;

        private void OnEnable()
        {
            // IsGrounded = true;
        }

        private void Update()
        {
            CheckRelease();
            CheckXAxisOverflow();
            
            Move();
            FloorDetect();
        }

        public override void ChangeFacing()
        {
            base.ChangeFacing();
            spRender.flipX = !isFacingLeft;
        }

        private void CheckRelease()
        {
            // Mathf.Abs(transform.position.y - playerController.currentPos.y)
            if (transform.position.y - playerController.currentPos.y > releaseHeight)
            {
                Release();
            }
        }

        private void CheckXAxisOverflow()
        {
            var position = transform.position;
            
            var dist = position.x - playerController.currentPos.x;
            if (dist < -respawnOppositeWidth)
            {
                position = new Vector3(playerController.currentPos.x + respawnOppositeWidth,
                    position.y, position.z);
            }
            else if (dist > respawnOppositeWidth)
            {
                position = new Vector3(playerController.currentPos.x - respawnOppositeWidth,
                    position.y, position.z);
            }
            
            transform.position = position;
        }
        
        public void Spawn(Vector3 point, bool isLeft)
        {
            var data = GetRandomEnemyData();
            enemyType = data.type;
            transform.position = point;
            isFacingLeft = isLeft;
            spRender.flipX = !isLeft;
            SetLayerActivate(data.layerIndex);
        }

        public void Release()
        {
            if (gameObject.activeInHierarchy)
            {
                enemyPool.Release(this);
            }
        }

        private EnemyData GetRandomEnemyData()
        {
            var rand = new System.Random();
            var data = enemies[rand.Next(0, enemies.Count - 1)];
            return data;
        }

        private void SetLayerActivate(int index)
        {
            for (int i = 1; i <= layerCount; i++)
            {
                anim.SetLayerWeight(i, 0.0f);
            }
            anim.SetLayerWeight(index, 1.0f);
        }

        private void OnDrawGizmos()
        {
            GroundedGizmos();
        }
    }
}