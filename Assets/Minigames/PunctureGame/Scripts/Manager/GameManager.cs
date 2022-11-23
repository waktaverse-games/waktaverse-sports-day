using System.Collections;
using SharedLibs;
using SharedLibs.Score;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class GameManager : DisposableSingleton<GameManager>
    {
        private LogicBehaviour[] logicBhvrs;

        [SerializeField] private ScoreCollector scoreCollector;

        private void Start()
        {
            // GameStart();
            
            IEnumerator TestMethod()
            {
                GameReady();
                yield return new WaitForSeconds(3.0f);
                GameStart();
            }
            
            StartCoroutine(TestMethod());
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
            PunctureBGMCollection.Instance.PlayBGM(PunctureBGMType.Default, true);

            foreach (var bhvr in logicBhvrs) bhvr.GameStart();
        }

        public void GameOver()
        {
            PunctureBGMCollection.Instance.StopBGM();
            PunctureSFXCollection.Instance.PlaySFX(PunctureSFXType.GameOver);

            foreach (var bhvr in logicBhvrs) bhvr.GameOver();
            
            ScoreManager.Instance.AddGameRoundScore(MinigameType.PunctureGame, scoreCollector.TotalScore);
        }
    }
}