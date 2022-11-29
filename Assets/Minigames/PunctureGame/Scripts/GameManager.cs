using System;
using System.Collections;
using SharedLibs;
using SharedLibs.Score;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class GameManager : DisposableSingleton<GameManager>
    {
        private LogicBehaviour[] logicBhvrs;

        [SerializeField] private PunctureBGMCollection bgmCollection;
        [SerializeField] private PunctureSFXCollection sfxCollection;
        
        [SerializeField] private ScoreCollector scoreCollector;

        [SerializeField] private AnimationCounter counter;

        private void OnEnable()
        {
            counter.OnEndCount += () =>
            {
                GameStart();
            };
        }

        private void Start()
        {
            GameReady();
        }

        protected override void Initialize()
        {
            logicBhvrs = FindObjectsOfType<LogicBehaviour>();
        }

        public void GameReady()
        {
            foreach (var bhvr in logicBhvrs) bhvr.GameReady();
        }

        public void GameStart()
        {
            bgmCollection.PlayBGM(PunctureBGMType.Default, true);

            foreach (var bhvr in logicBhvrs) bhvr.GameStart();
        }

        public void GameOver()
        {
            ScoreManager.Instance.SetGameHighScore(MinigameType.PunctureGame, scoreCollector.TotalScore);
            
            bgmCollection.StopBGM();
            sfxCollection.PlaySFX(PunctureSFXType.GameOver);

            foreach (var bhvr in logicBhvrs) bhvr.GameOver();
            
            ResultSceneManager.ShowResult(MinigameType.PunctureGame);
        }
    }
}