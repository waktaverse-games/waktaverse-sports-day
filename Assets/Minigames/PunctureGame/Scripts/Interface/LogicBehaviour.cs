using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public abstract class LogicBehaviour : MonoBehaviour
    {
        public abstract void Active();
        public abstract void Inactive();
    }
}