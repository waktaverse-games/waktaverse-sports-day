using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
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
        SceneLoader.LoadSceneAsync(nextScene, () =>
        {
            transform.GetChild(0).GetComponent<Image>().sprite = loadingImg;
        }, 2.0f);
    }

    public static void LoadScene(string sceneName, Sprite loadingImage)
    {
        nextScene = sceneName;
        loadingImg = loadingImage;
        SceneLoader.LoadSceneAsync("LoadingScene");
    }
}
