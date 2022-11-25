using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.Temp
{
    public class DebugMainMenuUI : MonoBehaviour
    {
        public void GoToMainMenu()
        {
            SceneManager.LoadScene("TestGameSelectScene", LoadSceneMode.Single);
        }
    }
}