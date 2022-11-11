using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class ItemManager : MonoBehaviour, IManagerLogic
    {
        [SerializeField] private Player player;
        [SerializeField] private ItemPoolSpawner spawner;
        
        [SerializeField] private float upperReleaseHeight;
        [SerializeField] private float relocateWidth;

        private void OnDisable()
        {
            Stop();
        }

        private void Update()
        {
            var items = spawner.GetActiveElements();
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item && item.enabled)
                {
                    ItemXAxisRelocate(item.transform);
                }
            }
        }

        private void ItemXAxisRelocate(Transform tf)
        {
            var position = tf.position;

            var dist = position.x - player.currentPos.x;
            if (dist < -relocateWidth)
                position = new Vector3(player.currentPos.x + relocateWidth,
                    position.y, position.z);
            else if (dist > relocateWidth)
                position = new Vector3(player.currentPos.x - relocateWidth,
                    position.y, position.z);

            tf.position = position;
        }
        
        public void Run()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
}