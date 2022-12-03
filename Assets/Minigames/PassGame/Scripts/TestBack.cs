using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.PassGame
{
    public class TestBack : MonoBehaviour
    {
        public bool shouldMove;

        private float offset;

        private Material mat;
        // Start is called before the first frame update
        void Start()
        {
            mat = GetComponent<Renderer>().material;
            shouldMove = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (shouldMove)
            {
                offset += (Time.deltaTime * 0.4f) / 10f;
                mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
            }
        }
    }
}