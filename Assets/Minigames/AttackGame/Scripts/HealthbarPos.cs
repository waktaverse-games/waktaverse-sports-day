using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.AttackGame
{
    public class HealthbarPos : MonoBehaviour
    {
        private RectTransform _rectTransform;
        // Start is called before the first frame update
        void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            _rectTransform.anchoredPosition = transform.parent.transform.localPosition;
        }
    }
}
