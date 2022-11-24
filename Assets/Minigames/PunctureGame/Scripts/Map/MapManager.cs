using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] [DisableInPlayMode] private Map[] rightSideMaps;
        [SerializeField] [DisableInPlayMode] private Map[] leftSideMaps;

        [SerializeField] private PlayerController controller;

        [SerializeField] private float floorWidth = 10.0f;
        [SerializeField] private float mapHeight = 10.0f;

        [SerializeField] [ReadOnly] private float leftTargetX;
        [SerializeField] [ReadOnly] private float rightTargetX;
        [SerializeField] [ReadOnly] private float bottomTargetY;

        [SerializeField] private float allowRelocateDistance = -0.5f;

        private void Awake()
        {
            controller = FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            bottomTargetY -= mapHeight / 2 + mapHeight;
        }

        private void Update()
        {
            SwapHorizontal();
            // Player is moved down, Upper side maps will be moved to bottom side and swap each other
            if (controller.Position.y < bottomTargetY - allowRelocateDistance) SwapVertical();
        }

        private void OnDrawGizmos()
        {
            if (!controller) return;

            var leftTargetVec = new Vector3(leftTargetX, bottomTargetY);
            var rightTargetVec = new Vector3(rightTargetX, bottomTargetY);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(new Vector3(leftTargetX, bottomTargetY), controller.Position);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(new Vector3(rightTargetX, bottomTargetY), controller.Position);
            Gizmos.color = Color.black;
            Gizmos.DrawLine(leftTargetVec, rightTargetVec);
        }

        private void SwapHorizontal()
        {
            if (!controller.IsRightSide)
            {
                // Player is facing left, Right side maps will be moved to left side and swap each other
                if (controller.Position.x < leftTargetX - allowRelocateDistance)
                {
                    var fixPosVec = new Vector3(-floorWidth, 0.0f);
                    for (var i = 0; i < rightSideMaps.Length; i++)
                    {
                        rightSideMaps[i].MovePosition(leftSideMaps[i].CurrentPosition, fixPosVec);
                        (leftSideMaps[i], rightSideMaps[i]) = (rightSideMaps[i], leftSideMaps[i]);
                    }

                    rightTargetX = leftTargetX;
                    leftTargetX -= floorWidth;
                }
            }
            else
            {
                // Player is facing right, Left side maps will be moved to right side and swap each other
                if (controller.Position.x > rightTargetX + allowRelocateDistance)
                {
                    var fixPosVec = new Vector3(floorWidth, 0.0f);
                    for (var i = 0; i < leftSideMaps.Length; i++)
                    {
                        leftSideMaps[i].MovePosition(rightSideMaps[i].CurrentPosition, fixPosVec);
                        (rightSideMaps[i], leftSideMaps[i]) = (leftSideMaps[i], rightSideMaps[i]);
                    }

                    leftTargetX = rightTargetX;
                    rightTargetX += floorWidth;
                }
            }
        }

        private void SwapVertical()
        {
            var fixPosVec = new Vector3(0.0f, -mapHeight);
            leftSideMaps[0].MapReset().MovePosition(leftSideMaps[1].CurrentPosition, fixPosVec);
            rightSideMaps[0].MapReset().MovePosition(rightSideMaps[1].CurrentPosition, fixPosVec);
            (leftSideMaps[0], leftSideMaps[1]) = (leftSideMaps[1], leftSideMaps[0]);
            (rightSideMaps[0], rightSideMaps[1]) = (rightSideMaps[1], rightSideMaps[0]);

            bottomTargetY -= mapHeight;
        }
    }
}