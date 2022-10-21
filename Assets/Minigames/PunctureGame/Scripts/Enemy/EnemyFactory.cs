using System;
using SharedLibs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHaven.PunctureGame
{
    public enum EnemyType
    {
        Chimpanzee, Pigeon, Bat, Dog, Bacteria, Elk, Fox
    }
    
    [System.Serializable]
    public class EnemyDictionary : UnitySerializedDictionary<EnemyType, GameObject> {}

    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField, DisableInPlayMode]
        private EnemyDictionary enemyDic;

        private Array enemyEnumArr;
        private EnemyType RandomEnemy => (EnemyType)enemyEnumArr.GetValue(UnityEngine.Random.Range(0, enemyEnumArr.Length));

        private void Awake()
        {
            enemyEnumArr = Enum.GetValues(typeof(EnemyType));
        }

        public Enemy Create(EnemyType type, bool active = false)
        {
            return CreateObject(type).GetComponent<Enemy>();
        }
        
        public GameObject CreateObject(EnemyType type, bool active = false)
        {
            var obj = Instantiate(enemyDic[type]);
            obj.SetActive(active);
            return obj;
        }

        public Enemy CreateRandom(bool active = false)
        {
            var obj = Create(RandomEnemy);
            return obj;
        }

        public GameObject CreateRandomObject(bool active = false)
        {
            var obj = CreateObject(RandomEnemy);
            return obj;
        }
    }
}