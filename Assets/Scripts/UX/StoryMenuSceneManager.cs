using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.UIUX
{
    public class StoryMenuSceneManager : MonoBehaviour
    {
        [SerializeField] AudioClip buttonSound;
        private void Awake()
        {
            AudioSource.PlayClipAtPoint(buttonSound, Vector3.zero);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("ModeSelectScene");
            }
        }
    }
}
