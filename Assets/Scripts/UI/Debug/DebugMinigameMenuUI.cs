using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.Temp
{
    public class DebugMinigameMenuUI : MonoBehaviour
    {
        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MinigameMenuScene", LoadSceneMode.Single);
        }
    }
}