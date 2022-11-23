using UnityEngine;

namespace GameHeaven.PunctureGame.Utils
{
    public static class ColliderUtil
    {
        public static bool CheckColliderHitLayer(Collider2D col, LayerMask mask)
        {
            return (mask & (1 << col.gameObject.layer)) != 0;
        }
    }
}