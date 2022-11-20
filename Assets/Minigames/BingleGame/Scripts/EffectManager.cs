using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class EffectManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, 3f);
        }

    }
}
