using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameHeaven.PunctureGame.UI
{
    public class BackgroundScroll : MonoBehaviour
    {
        [SerializeField] private Sprite[] bgSprites;
        [SerializeField] [ReadOnly] private Sprite currentSprite;
        [SerializeField] [ReadOnly] private int currentSpriteIdx = 0;
        
        [SerializeField] private Transform target;
        [SerializeField] private Transform upperTransform, bottomTransform;
        
        [SerializeField] private float upperFixYPos, bottomFixYPos;
        [SerializeField] private float gap;

        [SerializeField] [ReadOnly] private int changeCount = 1;

        [SerializeField] private UnityEvent onChangeBgSprite;

        private int CurrentSpriteIdx
        {
            get => currentSpriteIdx;
            set => currentSpriteIdx = value >= bgSprites.Length ? 0 : value;
        }

        private void Start()
        {
            gap = upperTransform.position.y - bottomTransform.position.y;
            
            bottomTransform.position = new Vector3(target.position.x, target.position.y - gap);
            upperTransform.position = target.position;
            
            var upperPos = upperTransform.position;
            var bottomPos = bottomTransform.position;
            
            upperFixYPos = upperPos.y;
            bottomFixYPos = bottomPos.y;

            currentSprite = bgSprites[CurrentSpriteIdx];
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

                var image = bottomTransform.GetComponent<Image>();
                image.sprite = bgSprites[currentSpriteIdx];

                changeCount++;
                if (changeCount % 4 == 0)
                {
                    CurrentSpriteIdx++;
                }
                if (changeCount % 4 == 1)
                {
                    onChangeBgSprite.Invoke();
                }
                
            }
        }

        private void Test()
        {
            
        }
    }
}