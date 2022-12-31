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
            // ÇÃ·§ÆûÀÌ 1.2¹è »¡¶óÁü: 3ÁßÃ¸±îÁö
            //Debug.Log("SpeedUp Item Acquired");
            if (MiniGameManager.Instance.platform.Speed * 1.2f < MiniGameManager.Instance.platform.InitialSpeed * 2f)
            {
                MiniGameManager.Instance.platform.Speed *= 1.2f;
            }
            MiniGameManager.Instance.UI.ShowItemEffect("ÀÌµ¿¼Óµµ Áõ°¡!");
        }
    }
}

