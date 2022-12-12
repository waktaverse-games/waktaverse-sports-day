using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    private static Sprite loadingImg;

    [SerializeField] private string[] tipMessages;

    private void Start()
    {
        SceneLoader.LoadSceneAsync(nextScene, () =>
        {
            transform.GetChild(0).GetComponent<Image>().sprite = loadingImg;
            transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "TMI: " + tipMessages[Random.Range(0, tipMessages.Length)];
        }, 2.0f);
    }

    public static void LoadScene(string sceneName, Sprite loadingImage)
    {
        nextScene = sceneName;
        loadingImg = loadingImage;
        SceneLoader.LoadSceneAsync("LoadingScene");
    }
}
