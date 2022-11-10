using System;
using System.Collections.Generic;
using GameHeaven.PunctureGame.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace GameHeaven.PunctureGame.UI
{
    public class BackgroundScroll : MonoBehaviour
    {
        [SerializeField] private Image upperImage, bottomImage;
        [SerializeField] private Texture2D[] bgTextures;

        [SerializeField] private Transform target;
        [SerializeField] private Transform upperTransform, bottomTransform;
        
        [SerializeField] private float upperFixYPos, bottomFixYPos;
        [SerializeField] private float gap;

        private void Start()
        {
            gap = upperTransform.position.y - bottomTransform.position.y;
            
            bottomTransform.position = new Vector3(target.position.x, target.position.y - gap);
            upperTransform.position = target.position;
            
            var upperPos = upperTransform.position;
            var bottomPos = bottomTransform.position;
            
            upperFixYPos = upperPos.y;
            bottomFixYPos = bottomPos.y;
        }

        private void Update()
        {
            upperTransform.position = new Vector3(target.position.x, upperFixYPos);
            bottomTransform.position = new Vector3(target.position.x, bottomFixYPos);

            if (target.position.y < bottomFixYPos)
            {
                upperFixYPos = bottomFixYPos;
                bottomFixYPos = bottomTransform.position.y - gap;
                upperTransform.position = new Vector3(target.position.x, bottomFixYPos);

                (upperTransform, bottomTransform) = (bottomTransform, upperTransform);
            }
        }

        private void Test()
        {
            
        }
    }
}