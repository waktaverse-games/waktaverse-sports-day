using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;

        public Enemy Create()
        {
            var obj = Instantiate(enemyPrefab);
            return obj.GetComponent<Enemy>();
        }
    }
}