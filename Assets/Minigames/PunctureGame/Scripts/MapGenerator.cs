using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject[] blocks;
        [SerializeField] private Enemy[] enemies;

        [SerializeField] private float verticalBlockDist;

        [SerializeField] private float floorHorizontalLength = 12;
        [SerializeField] private float floorVerticalLength = 6;

        private Transform _playerTransform;

        private void Awake()
        {
            _playerTransform = FindObjectOfType<PlayerController>().transform;
        }

        private void Start()
        {
            for (var i = 0; i < floorVerticalLength; i++)
            {
                for (var j = 0; j < floorHorizontalLength; j++)
                {
                    var position = _playerTransform.position;
                    Instantiate(
                        blocks[0], 
                        new Vector3(position.x + (-floorHorizontalLength / 2 + j) * blocks[0].transform.lossyScale.x, position.y - verticalBlockDist * i), 
                        Quaternion.identity, 
                        transform);
                }
            }
        }
    }
}
