using System;
using UnityEngine;

namespace GameHeaven.UIUX
{
    public class LoopScrollHorizontal : MonoBehaviour
    {
        [SerializeField] private RectTransform parent;
        [SerializeField] private RectTransform[] items;
        [SerializeField] private float[] speeds;

        private void Update()
        {
            for (var i = 0; i < items.Length; i++)
            {
                items[i].position += new Vector3(Time.deltaTime * speeds[i], 0f, 0f);
                if (speeds[i] > 0)
                {
                    if (items[i].position.x > parent.sizeDelta.x + items[i].sizeDelta.x / 2f)
                    {
                        items[i].position = new Vector3(-items[i].sizeDelta.x / 2f, items[i].position.y, items[i].position.z);
                    }
                }
                else
                {
                    if (items[i].position.x < -items[i].sizeDelta.x / 2f)
                    {
                        items[i].position = new Vector3(parent.sizeDelta.x + items[i].sizeDelta.x / 2f, items[i].position.y, items[i].position.z);
                    }
                }
            }
        }
    }
}