using System;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Manager Components")]
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private EnemyManager enemyManager;
        
        [Header("Audio Components")]
        [SerializeField] private PunctureBGMCollection bgmCollection;
        [SerializeField] private PunctureSFXCollection sfxCollection;

        private LogicBehaviour[] logicBhvrs;

        private void Awake()
        {
            Instance = this;

            logicBhvrs = FindObjectsOfType<LogicBehaviour>();
        }

        private void Start()
        {
            // TODO: Ready
            GameStart();
        }

        public void GameStart()
        {
            bgmCollection.PlayBGM(PunctureBGMType.Default, true);

            foreach (var bhvr in logicBhvrs)
            {
                bhvr.Active();
            }
        }

        public void GameOver()
        {
            playerManager.enabled = false;
            enemyManager.enabled = false;

            bgmCollection.StopBGM();
            sfxCollection.PlaySFX(PunctureSFXType.GameOver);

            foreach (var bhvr in logicBhvrs)
            {
                bhvr.Inactive();
            }
        }
    }
}