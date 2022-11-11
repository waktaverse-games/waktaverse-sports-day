using System.Collections.Generic;
using GameHeaven.PunctureGame.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class Map : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private List<GameObject> blockObjects;
        [SerializeField] [ReadOnly] private List<SpriteColorize> spriteColorizes;

        public Vector3 CurrentPosition => transform.position;

        private void Awake()
        {
            spriteColorizes = new List<SpriteColorize>();
            foreach (Transform floorTf in transform)
            foreach (Transform blockTf in floorTf)
            {
                var blockObj = blockTf.gameObject;
                var colorize = blockTf.GetComponentInChildren<SpriteColorize>();
                blockObjects.Add(blockObj);
                spriteColorizes.Add(colorize);
            }
        }

        public void MovePosition(Vector3 pivot, Vector3 moveVec)
        {
            var prevPos = transform.position;
            transform.position = pivot + moveVec;
        }

        public Map MapReset()
        {
            SetBlocksActive();
            SetBlocksColor();
            return this;
        }

        private void SetBlocksActive(bool active = true)
        {
            foreach (var blockObj in blockObjects)
                if (blockObj.activeInHierarchy != active)
                    blockObj.SetActive(active);
        }

        private void SetBlocksColor()
        {
            foreach (var colorize in spriteColorizes) colorize.SetRandomColor();
        }
    }
}