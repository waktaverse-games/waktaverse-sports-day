using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public abstract class LogicBehaviour : MonoBehaviour, IManageLogic
    {
        public abstract void GameReady();
        public abstract void GameStart();
        public abstract void GameOver();
    }
}