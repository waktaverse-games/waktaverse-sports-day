using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GameHeaven.PunctureGame
{
    public class BackgroundScroll : MonoBehaviour
    {
        [SerializeField] private Sprite[] bgSprites;
        [SerializeField] private PingPongRange rangeIndex;

        [SerializeField] private PlayerController controller;
        [SerializeField] private Transform upperTransform, bottomTransform;
        [SerializeField] private SpriteRenderer upperSprite, bottomSprite;

        [SerializeField] [ReadOnly] private int spriteIndex;
        [SerializeField] [ReadOnly] private int spriteMoveCount = 1;

        [SerializeField] private int changeInterval;
        
        private float upperFixYPos, bottomFixYPos;
        private float gap;

        public event Action<int> OnBackgroundChanged;

        private void Start()
        {
            gap = upperTransform.position.y - bottomTransform.position.y;

            bottomTransform.position = new Vector3(controller.Position.x, controller.Position.y - gap);
            upperTransform.position = controller.Position;

            var upperPos = upperTransform.position;
            var bottomPos = bottomTransform.position;

            upperFixYPos = upperPos.y;
            bottomFixYPos = bottomPos.y;

            spriteIndex = rangeIndex.GetPingPongValue();
        }

        private void Update()
        {
            upperTransform.position = new Vector3(controller.Position.x, upperFixYPos);
            bottomTransform.position = new Vector3(controller.Position.x, bottomFixYPos);

            if (controller.Position.y < bottomFixYPos)
            {
                upperFixYPos = bottomFixYPos;
                bottomFixYPos = bottomTransform.position.y - gap;
                upperTransform.position = new Vector3(controller.Position.x, bottomFixYPos);

                (upperTransform, bottomTransform) = (bottomTransform, upperTransform);
                (upperSprite, bottomSprite) = (bottomSprite, upperSprite);

                spriteMoveCount++;
                if (spriteMoveCount % changeInterval == 0)
                {
                    spriteIndex = rangeIndex.GetPingPongValue();
                    OnBackgroundChanged?.Invoke(spriteIndex);
                }

                bottomSprite.sprite = bgSprites[spriteIndex];
            }
        }
    }
}