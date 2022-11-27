using System;
using UnityEngine;

namespace GameHeaven.Temp
{
    public class DebugTimescaleInitializer : MonoBehaviour
    {
        private void Awake()
        {
            Time.timeScale = 1.0f;
        }
    }
}