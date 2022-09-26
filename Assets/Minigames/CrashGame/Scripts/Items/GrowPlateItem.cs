using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class GrowPlateItem : Item
    {
        private PlayerPlatform player;
        Transform platform;

        public override void ActivateItem()
        {
            player = GameManager.Instance.platform;
            platform = player.Platform;
            // ÇÃ·§ÆûÀÌ 1.2¹è ³Ð¾îÁü: 3ÁßÃ¸±îÁö
            Debug.Log("GrowPlate Item Acquired");
            Vector3 platformScale = platform.localScale;
            if (platformScale.x * 1.2 <= platform.GetComponentInParent<PlayerPlatform>().InitialPlatformXScale * 2)
            {
                platformScale = new Vector3(platformScale.x * 1.2f, platformScale.y, platformScale.z);
                platform.localScale = platformScale;
            }
        }
    }
}

