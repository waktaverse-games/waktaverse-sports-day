using UnityEngine;

namespace GameHeaven.PunctureGame.Utils
{
    public static class GizmosUtil
    {
        // https://gist.github.com/Arakade/9dd844c2f9c10e97e3d0?permalink_comment_id=4043513#gistcomment-4043513
        public static void DrawString(string text, Vector3 worldPosition, Color textColor, Vector2 anchor, float textSize = 15f)
        {
#if UNITY_EDITOR
            var view = UnityEditor.SceneView.currentDrawingSceneView;
            if (!view)
                return;
            Vector3 screenPosition = view.camera.WorldToScreenPoint(worldPosition);
            if (screenPosition.y < 0 || screenPosition.y > view.camera.pixelHeight || screenPosition.x < 0 || screenPosition.x > view.camera.pixelWidth || screenPosition.z < 0)
                return;
            var pixelRatio = UnityEditor.HandleUtility.GUIPointToScreenPixelCoordinate(Vector2.right).x - UnityEditor.HandleUtility.GUIPointToScreenPixelCoordinate(Vector2.zero).x;
            UnityEditor.Handles.BeginGUI();
            var style = new GUIStyle(GUI.skin.label)
            {
                fontSize = (int)textSize,
                normal = new GUIStyleState() { textColor = textColor }
            };
            Vector2 size = style.CalcSize(new GUIContent(text)) * pixelRatio;
            var alignedPosition =
                ((Vector2)screenPosition +
                 size * ((anchor + Vector2.left + Vector2.up) / 2f)) * (Vector2.right + Vector2.down) +
                Vector2.up * view.camera.pixelHeight;
            GUI.Label(new Rect(alignedPosition / pixelRatio, size / pixelRatio), text, style);
            UnityEditor.Handles.EndGUI();
#endif
        }

        public static void DrawLine(Vector3 from, Vector3 to)
        {
#if UNITY_EDITOR
            DrawLine(from, to, Color.white);
#endif
        }
        public static void DrawLine(Vector3 from, Vector3 to, Color color)
        {
#if UNITY_EDITOR
            Gizmos.color = color;
            Gizmos.DrawLine(from, to);
            ResetGizmoColor();
#endif
        }

        public static void DrawCube(Vector3 center, Vector3 size)
        {
#if UNITY_EDITOR
            DrawCube(center, size, Color.white);
#endif
        }
        public static void DrawCube(Vector3 center, Vector3 size, Color color)
        {
#if UNITY_EDITOR
            Gizmos.color = color;
            Gizmos.DrawCube(center, size);
            ResetGizmoColor();
#endif
        }

        public static void DrawWireCube(Vector3 center, Vector3 size)
        {
#if UNITY_EDITOR
            DrawWireCube(center, size, Color.white);
#endif
        }
        public static void DrawWireCube(Vector3 center, Vector3 size, Color color)
        {
#if UNITY_EDITOR
            Gizmos.color = color;
            Gizmos.DrawWireCube(center, size);
            ResetGizmoColor();
#endif
        }

        public static void DrawWireSphere(Vector3 center, float radius)
        {
#if UNITY_EDITOR
            DrawWireSphere(center, radius, Color.white);
#endif
        }
        public static void DrawWireSphere(Vector3 center, float radius, Color color)
        {
#if UNITY_EDITOR
            Gizmos.color = color;
            Gizmos.DrawWireSphere(center, radius);
            ResetGizmoColor();
#endif
        }

        public static void ResetGizmoColor()
        {
#if UNITY_EDITOR
            if (!Gizmos.color.Equals(Color.white))
            {
                Gizmos.color = Color.white;
            }
#endif
        }
    }
}