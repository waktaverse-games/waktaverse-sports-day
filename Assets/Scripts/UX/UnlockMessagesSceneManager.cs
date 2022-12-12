using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.UIUX
{
    public class UnlockMessagesSceneManager : MonoBehaviour
    {
        private void Start()
        {
            for (int i = 0; i < 5; i++)
            {
                transform.GetChild(i).GetComponent<Animator>().SetTrigger("On");
            }
        }

        public void CloseButton(int idx)
        {
            UISoundManager.Instance.PlayButtonSFX1();
            transform.GetChild(2 + idx).GetComponent<Animator>().SetTrigger("Off");
            if (idx == 0) SceneLoader.UnloadSceneAsync("UnlockMessagesScene");
        }
    }
}
