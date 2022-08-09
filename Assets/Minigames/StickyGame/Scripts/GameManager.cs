using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.StickyGame
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            Screen.SetResolution(640, 360, false);
        }
    }
}