using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class CheckPointManager : MonoBehaviour
    {
        public GameObject[] children;

        [SerializeField] TreeManager leftTree, rightTree;

        private void Start()
        {
            leftTree = children[0].GetComponent<TreeManager>();
            rightTree = children[1].GetComponent<TreeManager>();
        }

        public void DisableOtherCollider()
        {
            foreach(var obj in children)
            {
                if (obj.GetComponent<BoxCollider2D>())
                {
                    obj.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
        public void ResetCheckpoint()
        {
            EnableColliders();
            ResetTree();
        }

        void EnableColliders()
        {
            foreach(var obj in children)
            {
                obj.GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        void ChooseTreeType()
        {
            leftTree.SetTreeType(0);
            rightTree.SetTreeType(1);
        }

        void ResetTree()
        {
            leftTree.ResetTree();
            rightTree.ResetTree();
            ChooseTreeType();
        }

    }
}