using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class GrowPlateItem : Item
    {
        private PlayerPlatform player;
        Transform platform;
        public override string GetName() => "GrowPlateItem";
        public override void ActivateItem()
        {
            player = MiniGameManager.Instance.platform;
            platform = player.Platform;
            // «√∑ß∆˚¿Ã 1.2πË ≥–æÓ¡¸: 3¡ﬂ√∏±Ó¡ˆ
            //Debug.Log("GrowPlate Item Acquired");
            Vector3 platformScale = platform.localScale;
            if (platformScale.x * 1.2 <= platform.GetComponentInParent<PlayerPlatform>().InitialPlatformXScale * 2)
            {
                platformScale = new Vector3(platformScale.x * 1.2f, platformScale.y, platformScale.z);
                platform.localScale = platformScale;
            }
            MiniGameManager.Instance.UI.ShowItemEffect("¿Ôπ› ≈©±‚ ¡ı∞°!");
        }
    }
}

