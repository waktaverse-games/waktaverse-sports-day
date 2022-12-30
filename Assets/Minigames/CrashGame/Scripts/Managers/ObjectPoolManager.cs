using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class ObjectPoolManager : MonoBehaviour
    {
        [Serializable]
        public class SingleQueueData
        {
            public int num;
            public string objectName;
            public GameObject prefab;
        }
        [SerializeField] 
        public List<SingleQueueData> SingleQueueObjects;

        public Dictionary<string, Queue<GameObject>> queueDic = new Dictionary<string, Queue<GameObject>>();

        private void Awake()
        {
            InitQueue();
        }

        private void InitQueue()
        {
            for (int i = 0; i < SingleQueueObjects.Count; i++)
            {
                queueDic.Add(SingleQueueObjects[i].objectName, new Queue<GameObject>());

                for (int j = 0; j < SingleQueueObjects[i].num; j++)
                {
                    queueDic[SingleQueueObjects[i].objectName].Enqueue(CreateNewObject(SingleQueueObjects[i].prefab));
                }
            }
        }

        private GameObject CreateNewObject(GameObject obj)
        {
            var newObj = Instantiate(obj);
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(transform);
            return newObj;
        }

        //void CheckDictionaryIndex(string key)
        //{
        //    for (int i = 0; i < queueDic.Count; i++)
        //    {
        //        if (queueDic.ContainsKey(key))
        //        {
        //            DictionaryIndex = i;
        //            break;
        //        }
        //    }
        //}

        private int CheckObjectIndex(string key)
        {
            for (int i = 0; i < SingleQueueObjects.Count; i++)
            {
                if (SingleQueueObjects[i].objectName == key)
                {
                    return i;
                }
            }
            return -1;
        }

        public GameObject GetObject(string key)
        {
            Queue<GameObject> objQueue = queueDic[key];
            if (objQueue.Count > 0)
            {
                var oldObj = objQueue.Dequeue();
                oldObj.transform.SetParent(null);
                oldObj.gameObject.SetActive(true);
                return oldObj;
            }
            else
            {
                var newObj = MiniGameManager.ObjectPool.CreateNewObject(SingleQueueObjects[CheckObjectIndex(key)].prefab);
                newObj.gameObject.SetActive(true);
                newObj.transform.SetParent(null);
                return newObj;
            }
        }

        public void ReturnObject(string key, GameObject obj)
        {
            Queue<GameObject> objQueue = queueDic[key];
            if (obj.GetComponentInChildren<Rigidbody>() != null) obj.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
            obj.gameObject.SetActive(false);
            obj.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.SetParent(MiniGameManager.ObjectPool.transform);
            objQueue.Enqueue(obj);
        }


    }
}
