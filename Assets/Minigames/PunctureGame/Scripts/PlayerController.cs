using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1.0f;
        [SerializeField] private bool isFacingLeft = true;
        [SerializeField] private float blockDetectDistance = 1.0f;
        [SerializeField] private float blockDetectGap = 0.1f;

        [SerializeField] private Rigidbody2D playerRigid;
        [SerializeField] private Block detectedBlock = null;
        [SerializeField] private LayerMask brickLayerMask;
        private Block DetectedBlock
        {
            get => detectedBlock;
            set
            {
                if (detectedBlock)
                {
                    if (!detectedBlock.Equals(value))
                    {
                        detectedBlock = value;
                    }
                }
                else
                {
                    detectedBlock = value;
                }
            }
        }

        // Update is called once per frame
        private void Update()
        {
            DetectedBlock = DetectBlock();

            // transform.Translate(moveSpeed * (isFacingLeft ? -1 : 1) * Time.deltaTime, 0.0f, 0.0f);
            playerRigid.velocity = new Vector2(moveSpeed * (isFacingLeft ? -1 : 1), playerRigid.velocity.y);

            if (Input.GetKeyDown(KeyCode.C))
            {
                DetectedBlock.Crash();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isFacingLeft = !isFacingLeft;
            }
        }

        private Block DetectBlock()
        {
            var playerScale = transform.localScale;
            var playerPos = transform.position;
            
            var fixVecWithGap = new Vector3(playerScale.x / 2.0f + blockDetectGap, 0.0f);
            var pivot = playerPos - new Vector3(0.0f, playerScale.y / 2.0f) + (isFacingLeft ? -fixVecWithGap : fixVecWithGap);
            
            var hitInfo = Physics2D.Raycast(pivot, Vector3.down, blockDetectDistance, brickLayerMask);
            return hitInfo ? hitInfo.transform.GetComponent<Block>() : null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var playerScale = transform.localScale;
            var playerPos = transform.position;
            
            // Player Scale
            Gizmos.color = new Color(0.7f, 0.1f, 0.1f, 0.4f);
            Gizmos.DrawCube(playerPos, playerScale);
            
            var fixVecWithGap = new Vector3(playerScale.x / 2.0f + blockDetectGap, 0.0f);
            var pivot = playerPos - new Vector3(0.0f, playerScale.y / 2.0f) + (isFacingLeft ? -fixVecWithGap : fixVecWithGap);
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pivot, pivot + Vector3.down * blockDetectDistance);
            var hitInfo = Physics2D.Raycast(pivot, Vector3.down, blockDetectDistance, brickLayerMask);
            if (hitInfo)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(hitInfo.point, 0.2f);
                Debug.Log($"{hitInfo.collider.name}: {hitInfo.point}");
            }
        }
#endif
    }
}