using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class SpeedUpItem : Item
    {
        public override string GetName() => "SpeedUpItem";
        public override void ActivateItem()
        {
            // �÷����� 1.2�� ������: 3��ø����
            //Debug.Log("SpeedUp Item Acquired");
            if (MiniGameManager.Instance.platform.Speed * 1.2f < MiniGameManager.Instance.platform.InitialSpeed * 2f)
            {
                MiniGameManager.Instance.platform.Speed *= 1.2f;
            }
            MiniGameManager.Instance.UI.ShowItemEffect("�̵��ӵ� ����!");
        }
    }
}

