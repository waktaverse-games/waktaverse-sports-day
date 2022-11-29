using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    private static Sprite loadingImg;
    [SerializeField] Image[] loadingImages;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName, Sprite loadingImage)
    {
        nextScene = sceneName;
        loadingImg = loadingImage;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);

        transform.GetChild(0).GetComponent<Image>().sprite = loadingImg;
        op.allowSceneActivation = false;

        yield return new WaitForSeconds(1.0f);
        yield return new WaitUntil(() => op.progress >= 0.9f);

        op.allowSceneActivation = true;
    }
}
