#if UNITY_EDITOR

using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GameHeaven.PunctureGame.Utils
{
    public class DebugGizmos : MonoBehaviour
    {
        [SerializeField] private GizmosShape shape;

        [SerializeField] private bool isFilled;
        [SerializeField] private bool isCustom;

        [SerializeField] [ShowIf(nameof(isCustom), false)]
        private Vector3 customPos = Vector3.zero;

        [SerializeField] [ShowIf(nameof(isCustom))] [ShowIf(nameof(shape), GizmosShape.Cube, false)]
        private Vector3 customSize = new(1.0f, 1.0f, 1.0f);

        [SerializeField] [ShowIf(nameof(isCustom))] [ShowIf(nameof(shape), GizmosShape.Sphere, false)]
        private float customRadius = 1.0f;

        [SerializeField] private Color gizmosColor;

        private Vector3 position => isCustom ? transform.position + customPos : transform.position;
        private Vector3 scale => isCustom ? customSize : Vector3.one;
        private float radius => isCustom ? customRadius : 1f;

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmosColor;
            if (isFilled)
                switch (shape)
                {
                    case GizmosShape.Cube:
                        Gizmos.DrawCube(position, scale);
                        break;
                    case GizmosShape.Sphere:
                        Gizmos.DrawSphere(position, radius);
                        break;
                }
            else
                switch (shape)
                {
                    case GizmosShape.Cube:
                        Gizmos.DrawWireCube(position, scale);
                        break;
                    case GizmosShape.Sphere:
                        Gizmos.DrawWireSphere(position, radius);
                        break;
                }

            Gizmos.color = Color.white;
        }

        [Button("콜라이더와 동기화하기")]
        public void SyncCollider()
        {
            switch (shape)
            {
                case GizmosShape.Cube:
                    var boxCol = GetComponent<BoxCollider>();
                    var boxCol2D = GetComponent<BoxCollider2D>();
                    if (boxCol)
                    {
                        boxCol.center = customPos;
                        boxCol.size = scale;
                    }
                    else if (boxCol2D)
                    {
                        boxCol2D.offset = customPos;
                        boxCol2D.size = scale;
                    }
                    else
                    {
                        Debug.Log(name + " Object has no collider!");
                    }

                    break;
                case GizmosShape.Sphere:
                    var sphereCol = GetComponent<SphereCollider>();
                    var circleCol = GetComponent<CircleCollider2D>();
                    if (sphereCol)
                    {
                        sphereCol.center = customPos;
                        sphereCol.radius = customRadius;
                    }
                    else if (circleCol)
                    {
                        circleCol.offset = customPos;
                        circleCol.radius = customRadius;
                    }
                    else
                    {
                        Debug.Log(name + " Object has no collider!");
                    }

                    break;
            }
        }

        // https://gist.github.com/Arakade/9dd844c2f9c10e97e3d0?permalink_comment_id=4043513#gistcomment-4043513
        public static void DrawString(string text,
            Vector3 worldPosition,
            Color textColor,
            Vector2 anchor,
            float textSize = 15f)
        {
            var view = SceneView.currentDrawingSceneView;
            if (!view)
                return;
            var screenPosition = view.camera.WorldToScreenPoint(worldPosition);
            if (screenPosition.y < 0 || screenPosition.y > view.camera.pixelHeight || screenPosition.x < 0 ||
                screenPosition.x > view.camera.pixelWidth || screenPosition.z < 0)
                return;
            var pixelRatio = HandleUtility.GUIPointToScreenPixelCoordinate(Vector2.right).x -
                             HandleUtility.GUIPointToScreenPixelCoordinate(Vector2.zero).x;
            Handles.BeginGUI();
            var style = new GUIStyle(GUI.skin.label)
            {
                fontSize = (int)textSize,
                normal = new GUIStyleState { textColor = textColor }
            };
            var size = style.CalcSize(new GUIContent(text)) * pixelRatio;
            var alignedPosition =
                ((Vector2)screenPosition +
                 size * ((anchor + Vector2.left + Vector2.up) / 2f)) * (Vector2.right + Vector2.down) +
                Vector2.up * view.camera.pixelHeight;
            GUI.Label(new Rect(alignedPosition / pixelRatio, size / pixelRatio), text, style);
            Handles.EndGUI();
        }

        [EnumToggleButtons]
        private enum GizmosShape
        {
            Cube,
            Sphere
        }
    }
}

#endif