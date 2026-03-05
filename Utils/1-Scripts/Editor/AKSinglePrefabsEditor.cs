using UnityEditor;
using UnityEngine;
using AnkleBreaker.Utils.Fishnet;

namespace AnkleBreaker.Utils.Fishnet.Editor
{
    [CustomEditor(typeof(AKSinglePrefabs))]
    public class AKSinglePrefabsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space(8);

            if (GUILayout.Button("Remove null occurrences", GUILayout.Height(28)))
            {
                var target = (AKSinglePrefabs)this.target;
                Undo.RecordObject(target, "Remove Null Occurrences");
                target.RemoveNull();
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssetIfDirty(target);
                AssetDatabase.ForceReserializeAssets(new[] { AssetDatabase.GetAssetPath(target) });
            }
        }
    }
}