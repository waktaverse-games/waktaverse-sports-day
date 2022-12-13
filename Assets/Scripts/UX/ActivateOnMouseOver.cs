using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameHeaven.UIUX
{
    public class ActivateOnMouseOver : MonoBehaviour
    {
        [SerializeField] private GameObject obj;

        private void OnMouseEnter()
        {
            obj.SetActive(true);
        }
        private void OnMouseExit()
        {
            obj.SetActive(false);
        }
    }
}
