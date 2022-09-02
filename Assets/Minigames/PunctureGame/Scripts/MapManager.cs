using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class MapManager : MonoBehaviour
    {
        private static MapManager _instance;
        public static MapManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var instances = FindObjectsOfType<MapManager>();
                    if (instances.Length == 0)
                    {
                        Debug.LogError($"Instance of {nameof(MapManager)} is not available");
                    }
                    else
                    {
                        if (instances.Length > 1)
                        {
                            Debug.LogError($"Instance of {nameof(MapManager)} is not only one");
                            for (var i = 1; i < instances.Length; i++)
                            {
                                Destroy(instances[i]);
                            }
                        }
                    }
                    _instance = instances[0];
                }
                return _instance;
            }
        }

        [SerializeField] private PlayerController player;

        [SerializeField] private Material[] blockMats;
        
        [SerializeField] private GameObject platformObj;
        private float PlatformWidth => platformObj.transform.lossyScale.x;
        private float PlatformHeight => platformObj.transform.lossyScale.y;

        [SerializeField] private Vector2Int platformPlaceCount;
        
        [SerializeField] private float blockVertGap;

        [SerializeField] private List<Transform> leftEnds;
        [SerializeField] private List<Transform> rightEnds;
        [SerializeField] private List<Transform> bottomEnds;

        [SerializeField] private Transform leftEnd;
        [SerializeField] private Transform rightEnd;
        [SerializeField] private Transform bottomEnd;
        
        [SerializeField] private float horzCreateDist;
        [SerializeField] private float vertCreateDist;
        
        private void Awake()
        {
            _instance = this;

            leftEnds = new List<Transform>();
            rightEnds = new List<Transform>();
            bottomEnds = new List<Transform>();
        }

        private void Start()
        {
            var creationPos = transform.position;
            
            for (var i = 0; i < platformPlaceCount.y; i++)
            {
                var obj = Instantiate(platformObj,
                    new Vector3(0.0f,
                        creationPos.y - (PlatformHeight + blockVertGap) * i), Quaternion.identity);
                leftEnds.Add(obj.transform.Find("LeftEndPoint"));
                rightEnds.Add(obj.transform.Find("RightEndPoint"));
            }
        }

        private void Update()
        {
            if (player.IsFacingLeft)
            {
                for (var i = 0; i < leftEnds.Count; i++)
                {
                    var end = leftEnds[i];
                    if (Mathf.Abs(player.transform.position.x - end.position.x) < horzCreateDist)
                    {
                        var parentPos = end.parent.position;
                        var obj = Instantiate(platformObj, new Vector3(parentPos.x - PlatformWidth, parentPos.y), Quaternion.identity);
                        leftEnds.Remove(end);
                        leftEnds.Add(obj.transform.Find("LeftEndPoint"));
                    }
                }
            }
            else
            {
                for (var i = 0; i < rightEnds.Count; i++)
                {
                    var end = rightEnds[i];
                    if (Mathf.Abs(player.transform.position.x - end.position.x) < horzCreateDist)
                    {
                        var parentPos = end.parent.position;
                        var obj = Instantiate(platformObj, new Vector3(parentPos.x + PlatformWidth, parentPos.y), Quaternion.identity);
                        rightEnds.Remove(end);
                        rightEnds.Add(obj.transform.Find("RightEndPoint"));
                    }
                }
            }
        }

        // private void Start()
        // {
        //     var height = 2 * _cam.orthographicSize;
        //     var width = height * _cam.aspect;
        //     
        //     var generateFloorsHeight = height / 2 + playerTransform.position.y;
        //     var oneFloorHeight = floorGap + blocks[0].transform.lossyScale.y;
        //     var floorsCount = Mathf.CeilToInt(generateFloorsHeight / oneFloorHeight) + 1;
        //     var chunkBlocksCount = Mathf.CeilToInt(width / blocks[0].transform.lossyScale.x) + 2;
        //     
        //     Debug.Log($"{generateFloorsHeight}, {oneFloorHeight}, {floorsCount}, {chunkBlocksCount}");
        //     
        //     for (var i = 0; i < floorsCount; i++)
        //     {
        //         var newFloor = new Floor();
        //         for (var j = 0; j < chunkBlocksCount; j++)
        //         {
        //             var position = playerTransform.position;
        //             var obj = Instantiate(
        //                 blocks[0], 
        //                 new Vector3(position.x + (-chunkBlocksCount / 2 + j) * blocks[0].transform.lossyScale.x, -oneFloorHeight * i), 
        //                 Quaternion.identity, 
        //                 parentTransform);
        //             newFloor.Blocks.Add(obj.transform);
        //         }
        //         _floors.Add(newFloor);
        //     }
        // }

        // [Button]
        // private void FloorGenerate()
        // {
        //     var height = 2 * _cam.orthographicSize;
        //     var width = height * _cam.aspect;
        //     
        //     var generateFloorsHeight = height / 2 + playerTransform.position.y;
        //     var oneFloorHeight = floorGap + blocks[0].transform.lossyScale.y;
        //     var floorsCount = Mathf.CeilToInt(generateFloorsHeight / oneFloorHeight) + 1;
        //     var chunkBlocksCount = Mathf.RoundToInt(width / blocks[0].transform.lossyScale.x) + 2;
        //     
        //     Debug.Log($"{generateFloorsHeight}, {oneFloorHeight}, {floorsCount}, {chunkBlocksCount}");
        //     for (var j = 0; j < chunkBlocksCount; j++)
        //     {
        //         var position = playerTransform.position;
        //         var obj = Instantiate(
        //             blocks[0], 
        //             new Vector3(position.x + (-chunkBlocksCount / 2 + j) * blocks[0].transform.lossyScale.x, 0.0f), 
        //             Quaternion.identity, 
        //             parentTransform);
        //     }
        // }
    }
}
