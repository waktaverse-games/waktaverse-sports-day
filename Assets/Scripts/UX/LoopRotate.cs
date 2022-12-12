using UnityEngine;

namespace GameHeaven.UIUX
{
    public class LoopRotate : MonoBehaviour
    {
        [SerializeField] private RectTransform target;

        [SerializeField] private float speed;
        
        private void Update()
        {
            target.Rotate(0, 0, speed * Time.deltaTime);
        }
    }
}