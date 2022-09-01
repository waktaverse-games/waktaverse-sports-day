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
        [SerializeField] private float blockDetectGap = 0.1f;

        [SerializeField] private Rigidbody2D playerRigid;
        [SerializeField] private LayerMask brickLayerMask;

        private Vector3 PlayerPos => transform.position;
        private Vector3 PlayerScale => transform.localScale;

        private RaycastHit2D GroundHit =>
            Physics2D.CircleCast(PlayerPos, groundDetectDistance, Vector2.zero, 0f, brickLayerMask);

        private void Update()
        {
            // transform.Translate(moveSpeed * (isFacingLeft ? -1 : 1) * Time.deltaTime, 0.0f, 0.0f);
            playerRigid.velocity = new Vector2(moveSpeed * (isFacingLeft ? -1 : 1), playerRigid.velocity.y);

            if (Input.GetKeyDown(KeyCode.C))
            {
                var hitInfo = BottomBlockHit();
                if (hitInfo)
                {
                    var block = hitInfo.transform.GetComponent<Block>();
                    block.Crash();
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isFacingLeft = !isFacingLeft;
            }
        }
        
        /// <summary>
        /// Raycast를 통해 아래의 블록의 hit 정보를 가져온다
        /// </summary>
        /// <returns>아래 블록의 hit 정보</returns>
        private RaycastHit2D BottomBlockHit() {
            var fixVecWithGap = new Vector3(PlayerScale.x / 2.0f + blockDetectGap, 0.0f);
            var pivot = PlayerPos + (isFacingLeft ? -fixVecWithGap : fixVecWithGap);
                
            return Physics2D.Raycast(pivot, Vector3.down, blockDetectDistance, brickLayerMask);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Player Scale
            GizmosUtil.DrawCube(PlayerPos + new Vector3(0.0f, PlayerScale.y / 2.0f), PlayerScale, new Color(0.7f, 0.1f, 0.1f, 0.4f));

            // Jump Detect Area
            GizmosUtil.DrawWireSphere(PlayerPos, groundDetectDistance, GroundHit ? Color.cyan : Color.blue);
            
            var fixVecWithGap = new Vector3(PlayerScale.x / 2.0f + blockDetectGap, 0.0f);
            var pivot = PlayerPos + (isFacingLeft ? -fixVecWithGap : fixVecWithGap);
            
            // Block Detect Ray
            GizmosUtil.DrawString("detect ray", pivot, Color.white, Vector2.up);
            GizmosUtil.DrawLine(pivot, pivot + Vector3.down * blockDetectDistance, Color.green);
            
            // Block Detected Sphere
            var hitInfo = BottomBlockHit();
            if (hitInfo)
            {
                GizmosUtil.DrawWireSphere(hitInfo.point, 0.2f, Color.yellow);
            }
        }
#endif
    }
}