using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] Transform[] sprites;
    int startIndex = 0;
    int endIndex = 2;
    float camHeight;
    private void Awake()
    {
        camHeight = Camera.main.orthographicSize * 2;
    }
    public void MoveBG(int id, float yVal)
    {
        sprites[(id + 1) % 3].position = new Vector3(0, yVal - camHeight * 2, 0);
    }
}
