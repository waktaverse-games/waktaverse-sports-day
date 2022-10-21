using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField, DisableInPlayMode] private GameObject[] rightChunks;
        [SerializeField, DisableInPlayMode] private GameObject[] leftChunks;
        
        [SerializeField] private PlayerController playerController;
        
        [SerializeField] private float floorWidth = 8.5f;
        [SerializeField] private float mapHeight = 10.0f;

        [SerializeField, ReadOnly] private Vector3 rightTargetPos = Vector3.zero;
        [SerializeField, ReadOnly] private Vector3 leftTargetPos = Vector3.zero;
        [SerializeField, ReadOnly] private float bottomTargetHeight = 0.0f;
        
        [SerializeField] private float allowDist = 0.2f;

        private void Start()
        {
            bottomTargetHeight -= mapHeight / 2 + mapHeight;
            rightTargetPos.y = bottomTargetHeight;
            leftTargetPos.y = bottomTargetHeight;
        }

        private void Update()
        {
            if (playerController.IsFacingLeft)
            {
                if ((playerController.currentPos - leftTargetPos).x < -allowDist)
                {
                    print("To LEFT");
                    var fixPosVec = new Vector3(-floorWidth, 0.0f);
                    rightChunks[0].transform.position = leftChunks[0].transform.position + fixPosVec;
                    rightChunks[1].transform.position = leftChunks[1].transform.position + fixPosVec;

                    var idx0Chunk = leftChunks[0];
                    var idx1Chunk = leftChunks[1];

                    leftChunks[0] = rightChunks[0];
                    leftChunks[1] = rightChunks[1];

                    rightChunks[0] = idx0Chunk;
                    rightChunks[1] = idx1Chunk;

                    rightTargetPos = leftTargetPos;
                    leftTargetPos.x -= floorWidth;
                }
            }
            else
            {
                if ((playerController.currentPos - rightTargetPos).x > allowDist)
                {
                    print("To RIGHT");
                    var fixPosVec = new Vector3(floorWidth, 0.0f);
                    leftChunks[0].transform.position = rightChunks[0].transform.position + fixPosVec;
                    leftChunks[1].transform.position = rightChunks[1].transform.position + fixPosVec;

                    var idx0Chunk = rightChunks[0];
                    var idx1Chunk = rightChunks[1];

                    rightChunks[0] = leftChunks[0];
                    rightChunks[1] = leftChunks[1];

                    leftChunks[0] = idx0Chunk;
                    leftChunks[1] = idx1Chunk;
                    
                    leftTargetPos = rightTargetPos;
                    rightTargetPos.x += floorWidth;
                }
            }
            if (playerController.currentPos.y < bottomTargetHeight - allowDist)
            {
                leftChunks[0] = ResetMap(leftChunks[0].transform);
                rightChunks[0] = ResetMap(rightChunks[0].transform);
                
                var fixPosVec = new Vector3(0.0f, mapHeight);
                leftChunks[0].transform.position = leftChunks[1].transform.position - fixPosVec;
                rightChunks[0].transform.position = rightChunks[1].transform.position - fixPosVec;

                var leftChunk = leftChunks[1];
                var rightChunk = rightChunks[1];

                leftChunks[1] = leftChunks[0];
                rightChunks[1] = rightChunks[0];

                leftChunks[0] = leftChunk;
                rightChunks[0] = rightChunk;
                    
                bottomTargetHeight -= mapHeight;
                rightTargetPos.y -= mapHeight;
                leftTargetPos.y -= mapHeight;
            }
        }

        private GameObject ResetMap(Transform map)
        {
            // TODO: 적들 초기화 호출
            var enemyManager = map.GetComponent<EnemyManager>();

            foreach (Transform floor in map)
            {
                foreach (Transform block in floor)
                {
                    if (!block.gameObject.activeInHierarchy)
                    {
                        block.gameObject.SetActive(true);
                    }
                }
            }

            return map.gameObject;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(rightTargetPos, playerController.currentPos);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(leftTargetPos, playerController.currentPos);
            Gizmos.color = Color.black;
            Gizmos.DrawLine(leftTargetPos, rightTargetPos);
        }
    }
}