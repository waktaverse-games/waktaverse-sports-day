using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameHaven.RunGame
{
    public class PanControl : MonoBehaviour
    {
        Animator move;
        public GameObject pan;

        // Start is called before the first frame update
        void Start()
        {
            move = pan.GetComponent<Animator>();

        }


        void OnTriggerEnter2D(Collider2D other)
        {

            if (other.gameObject.name == "PlayableCharacter")
            {
                Debug.Log("test");
               
                move.SetBool("Stop", false);
                move.SetBool("Over", true); 
            }
        }
    }
}