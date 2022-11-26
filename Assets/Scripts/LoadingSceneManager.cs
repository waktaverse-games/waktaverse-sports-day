using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] static Image[] loadingImages;

    private void Start()
    {
        nextScene = "SpreadGame";
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;

        // loading image change;

        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);

        op.allowSceneActivation = false;

        yield return new WaitForSeconds(1.0f);
        yield return new WaitUntil(() => op.progress >= 0.9f);

        op.allowSceneActivation = true;
    }
}
