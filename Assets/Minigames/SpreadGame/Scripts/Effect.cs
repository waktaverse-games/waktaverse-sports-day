using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private float cool;

    private void Awake()
    {
        Invoke("MyDestroy", 0.5f);
    }

    void MyDestroy()
    {
        Destroy(this);
    }
}
