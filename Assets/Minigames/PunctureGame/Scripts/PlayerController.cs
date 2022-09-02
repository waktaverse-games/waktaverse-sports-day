using GameHeaven.PunctureGame.Utils;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1.0f;
        [SerializeField] private bool isFacingLeft = true;
        [SerializeField] private float groundDetectDistance = 0.1f;
        [SerializeField] private float blockDetectDistance = 1.0f;
        [SerializeField] private Vector2 blockDetectBoxSize;
        [SerializeField] private float blockDetectGap = 0.1f;
        
        public bool IsFacingLeft => isFacingLeft;

        [SerializeField] private Rigidbody2D playerRigid;
        [SerializeField] private LayerMask blockLayerMask;
        [SerializeField] private LayerMask platformLayerMask;
        
        private Vector3 PlayerPos => transform.position;
        private Vector3 PlayerScale => transform.localScale;

        private RaycastHit2D GroundHit =>
            Physics2D.CircleCast(PlayerPos, groundDetectDistance, Vector2.zero, 0f, blockLayerMask);

        private void Update()
        {
            // transform.Translate(moveSpeed * (isFacingLeft ? -1 : 1) * Time.deltaTime, 0.0f, 0.0f);
            playerRigid.velocity = new Vector2(moveSpeed * (isFacingLeft ? -1 : 1), playerRigid.velocity.y);

            if (Input.GetKeyDown(KeyCode.C))
            {
                var blockHits = BottomBlockBoxHit();
                if (blockHits.Length > 0)
                {
                    foreach (var hit in blockHits)
                    {
                        var block = hit.transform.GetComponent<Block>();
                        block.Crash();
                    }
                }

                var platformHit = BottomPlatformHit();
                if (platformHit)
                {
                    Debug.Log(platformHit.transform.name);
                    platformHit.transform.gameObject.SetActive(false);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // isFacingLeft = !isFacingLeft;
                isFacingLeft = isFacingLeft.UnaryNegation();
            }
        }
        
        /// <summary>
        /// Raycast를 통해 바로 아래의 플랫폼의 hit 정보를 가져온다
        /// </summary>
        /// <returns>바로 아래 플랫폼의 hit 정보</returns>
        private RaycastHit2D BottomPlatformHit() {
            return Physics2D.Raycast(PlayerPos, Vector3.down, blockDetectDistance, platformLayerMask);
        }

        /// <summary>
        /// Raycast를 통해 아래 박스범위의 블록들의 hit 정보를 가져온다
        /// </summary>
        /// <returns>아래 박스범위 블록들의 hit 정보</returns>
        private RaycastHit2D[] BottomBlockBoxHit()
        {
            var fixVecWithGap = new Vector3(PlayerScale.x / 2.0f + blockDetectGap, 0.0f);
            var pivot = PlayerPos + (isFacingLeft ? -fixVecWithGap : fixVecWithGap);

            return Physics2D.BoxCastAll(pivot, blockDetectBoxSize, 0f, Vector2.zero, 0f, blockLayerMask);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Player Scale
            GizmosUtil.DrawCube(PlayerPos + new Vector3(0.0f, PlayerScale.y / 2.0f), PlayerScale, new Color(0.7f, 0.1f, 0.1f, 0.4f));

            // Jump Detect Area
            GizmosUtil.DrawWireSphere(PlayerPos, groundDetectDistance, GroundHit ? Color.cyan : Color.blue);

            // platform Detect Ray
            GizmosUtil.DrawLine(PlayerPos, PlayerPos + Vector3.down * blockDetectDistance, Color.green);
            // GizmosUtil.DrawString("platform detect ray", PlayerPos + Vector3.down * blockDetectDistance, Color.white, Vector2.down, 12f);
            
            // platform Detected Sphere
            var platformHit = BottomPlatformHit();
            if (platformHit)
            {
                GizmosUtil.DrawCube(platformHit.transform.position, platformHit.transform.lossyScale, Color.yellow);
                // GizmosUtil.DrawWireSphere(platformHit.point, 0.2f, Color.yellow);
            }
            
            var fixVecWithGap = new Vector3(PlayerScale.x / 2.0f + blockDetectGap, 0.0f);
            var pivot = PlayerPos + (isFacingLeft ? -fixVecWithGap : fixVecWithGap);
            
            // Block Detect Box
            GizmosUtil.DrawWireCube(pivot, blockDetectBoxSize, Color.magenta);
            // GizmosUtil.DrawString("block detect box", pivot, Color.white, Vector2.up * 0.2f, 12f);
        }
#endif
    }
}