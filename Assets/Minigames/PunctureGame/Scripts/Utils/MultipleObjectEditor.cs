#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameHeaven.PunctureGame.Utils {
    public class MultipleObjectEditor : EditorWindow {
        private Vector2 _scrollPos = Vector2.zero;

        private bool addAtCurrent = true;
        private bool isAdditive = true;
        private bool isFoldout = true;

        private bool isLocal = true;

        private Vector3 posVec;
        private Vector3 rotateVec;
        private Vector3 scaleVec;
        private List<GameObject> selObjs;

        private void OnEnable() {
            selObjs = null;
            Selection.selectionChanged += SelectionChanged;
        }

        private void OnDisable() {
            selObjs = null;
            Selection.selectionChanged -= SelectionChanged;
        }

        private void OnGUI() {
            addAtCurrent = EditorGUILayout.Toggle("선택시 오브젝트 추가", addAtCurrent);
            if (selObjs == null) {
                GUILayout.FlexibleSpace();
                using (var ctx = new EditorGUILayout.HorizontalScope()) {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("하이어라키에서 수정하고자 하는 오브젝트를 선택해주세요");
                    GUILayout.FlexibleSpace();
                }

                GUILayout.FlexibleSpace();
                return;
            }

            isFoldout = EditorGUILayout.Foldout(isFoldout, "선택된 오브젝트" + $" ({selObjs.Count})");
            if (isFoldout)
                using (var scrollView = new EditorGUILayout.ScrollViewScope(_scrollPos)) {
                    _scrollPos = scrollView.scrollPosition;
                    for (var i = 0; i < selObjs.Count; i++)
                        using (var ctx = new EditorGUILayout.HorizontalScope()) {
                            selObjs[i] = (GameObject)EditorGUILayout.ObjectField(selObjs[i], typeof(GameObject), true);
                            if (GUILayout.Button("X",
                                    new GUIStyle(GUI.skin.button) { fixedWidth = 30f, fixedHeight = 18f }))
                                selObjs.Remove(selObjs[i]);
                        }
                }

            posVec = EditorGUILayout.Vector3Field("Position", posVec);
            rotateVec = EditorGUILayout.Vector3Field("Position", rotateVec);
            scaleVec = EditorGUILayout.Vector3Field("Position", scaleVec);

            isLocal = EditorGUILayout.Toggle(isLocal ? "로컬 변화값" : "월드 변화값", isLocal);
            isAdditive = EditorGUILayout.Toggle("기존값에 추가", isAdditive);

            if (GUILayout.Button("Edit Selected Objects"))
                foreach (var obj in selObjs)
                    if (isAdditive) {
                        obj.transform.Translate(posVec, Space.Self);
                        obj.transform.Rotate(rotateVec);
                        obj.transform.localScale += scaleVec;
                    }
                    else {
                        obj.transform.localPosition = posVec;
                        obj.transform.localRotation = Quaternion.Euler(rotateVec);
                        obj.transform.localScale = scaleVec;
                    }
        }

        [MenuItem("MayBUtil/Multiple Object Editor")]
        private static void WindowOpen() {
            var window = GetWindow<MultipleObjectEditor>("다중 오브젝트 수정");
        }

        private void SelectionChanged() {
            var transforms = Selection.GetTransforms(SelectionMode.ExcludePrefab | SelectionMode.Editable);
            if (transforms.Length > 0) {
                if (selObjs == null) selObjs = new List<GameObject>();
                if (!addAtCurrent) selObjs.Clear();

                foreach (var tf in transforms) {
                    var obj = tf.gameObject;
                    if (!selObjs.Contains(obj)) selObjs.Add(tf.gameObject);
                }

                Repaint();
            }
        }
    }
}

#endif