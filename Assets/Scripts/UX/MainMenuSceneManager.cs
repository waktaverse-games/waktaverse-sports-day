using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.UIUX
{
    public class MainMenuSceneManager : MonoBehaviour
    {
        [SerializeField] private DisplayNameChangeUI nameChangeUI;
        
        private void Update()
        {
            if (Input.anyKeyDown)
            {
                if (GameDatabase.Instance.IsFirstTime)
                {
                    nameChangeUI.gameObject.SetActive(true);
                }
                else
                {
                    SceneLoader.LoadSceneAsync("ModeSelectScene");
                }
            }
        }
    }
}