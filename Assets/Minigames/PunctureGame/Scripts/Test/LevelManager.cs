using SharedLibs;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameHeaven.PunctureGame.Test
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        [SerializeField] private GameObject progressBar;
        [SerializeField] private Image filledBar;
        [SerializeField] private TextMeshProUGUI progressPercent;

        private float _target;

        private void Update()
        {
            // filledBar.fillAmount = Mathf.MoveTowards(filledBar.fillAmount, _target, 2 * Time.deltaTime);
        }

        public override void Init() { }

        public async void LoadScene(string sceneName)
        {
            GameManager.Instance.GameOver();
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            // _target = 0.0f;
            // filledBar.fillAmount = 0.0f;
            //
            // var scene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            // scene.allowSceneActivation = false;
            //
            // progressBar.SetActive(true);
            //
            // do
            // {
            //     await Task.Delay(100);
            //     progressPercent.text = scene.progress.ToString();
            //     _target = scene.progress;
            // } while (scene.progress < 0.9);
            //
            // print("DONE!!");
            //
            // // await Task.Delay(1000);
            //
            // scene.allowSceneActivation = true;
            // progressBar.SetActive(false);
        }
    }
}