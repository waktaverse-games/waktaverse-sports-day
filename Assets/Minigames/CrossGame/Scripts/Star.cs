using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameHeaven.CrossGame
{
    public class Star : MonoBehaviour
    {
        public CrossGameManager Manager;
        public void Move()
        {
            transform.DOMoveY(4, 1).SetRelative().SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        public void Kill()
        {
            DOTween.Kill(gameObject.transform);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //�±׳� ���̾ ���� �� �� �� ���Ƽ� �ӽ� ��ġ
            if(collision.gameObject.GetComponent<Player>() as Player)
            {
                Manager.AddStar();
                Manager.ObjectController.Stars.Remove(gameObject);
                Kill();
            }
        }
    }
}

