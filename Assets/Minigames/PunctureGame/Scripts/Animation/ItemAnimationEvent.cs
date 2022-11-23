using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class ItemAnimationEvent : MonoBehaviour
    {
        [SerializeField] [Required] private Item item;

        public void Release()
        {
            item.ReleaseItem();
        }
    }
}