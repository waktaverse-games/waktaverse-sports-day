using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class SpeedUpItem : Item
    {
        public override void ActivateItem()
        {
            // �÷����� 1.2�� ������: 3��ø����
            Debug.Log("SpeedUp Item Acquired");
            if (GameManager.Instance.platform.Speed * 1.2f < GameManager.Instance.platform.InitialSpeed * 2f)
            {
                GameManager.Instance.platform.Speed *= 1.2f;
            }
            GameManager.Instance.UI.ShowItemEffect("�̵��ӵ� ����!");
        }
    }
}

