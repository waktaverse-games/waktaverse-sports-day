using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class PassAreaManager : MonoBehaviour
    {
        public void OnCollision()
        {                
            transform.parent.GetComponent<CheckPointManager>().DisableOtherCollider();
        }
    }
}