using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class Block : MonoBehaviour
    {
        public void Crash()
        {
            Debug.Log("Crashed");
            gameObject.SetActive(false);
        }
    }
}