using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.UIUX
{
    public class IntroSceneManager : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}