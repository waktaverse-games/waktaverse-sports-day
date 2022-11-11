using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public interface ISpawner<T> where T : MonoBehaviour
    {
        public T Spawn();
        public void Release(T elem);
    }
}