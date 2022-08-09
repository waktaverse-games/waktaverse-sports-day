using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameHeaven.CrossGame
{
    public class Platform : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D collision)
        {
            transform.DOScale(new Vector3(1.2f, 0.8f), 0.5f).SetEase(Ease.OutBounce);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(new Vector3(0.8f, 1.5f), 0.2f).SetEase(Ease.OutBounce)).Append(transform.DOScale(new Vector3(1f, 1f), 0.5f).SetEase(Ease.OutBounce));
        }
    }
}

