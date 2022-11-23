using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class SpeedUpItem : Item
    {
        public override void ActivateItem()
        {
            // ÇÃ·§ÆûÀÌ 1.2¹è »¡¶óÁü: 3ÁßÃ¸±îÁö
            Debug.Log("SpeedUp Item Acquired");
            if (GameManager.Instance.platform.Speed * 1.2f < GameManager.Instance.platform.InitialSpeed * 2f)
            {
                GameManager.Instance.platform.Speed *= 1.2f;
            }
            GameManager.Instance.UI.ShowItemEffect("ÀÌµ¿¼Óµµ Áõ°¡!");
        }
    }
}

