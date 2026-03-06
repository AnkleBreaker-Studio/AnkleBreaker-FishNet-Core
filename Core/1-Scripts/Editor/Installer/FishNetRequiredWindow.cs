using UnityEditor;
using UnityEngine;

namespace AnkleBreaker.FishNetCore.Editor
{
    public class FishNetRequiredWindow : EditorWindow
    {
        private static GUIStyle _titleStyle;
        private static GUIStyle _messageStyle;
        private static GUIStyle _warningStyle;

        public static void ShowWindow()
        {
            var window = GetWindow<FishNetRequiredWindow>(true, "AnkleBreaker FishNet Core - Missing FishNet", true);
            window.minSize = new Vector2(500, 280);
            window.maxSize = new Vector2(500, 280);
            window.ShowUtility();
        }

        private void OnGUI()
        {
            InitStyles();

            EditorGUILayout.Space(15);
            EditorGUILayout.LabelField("FishNet Required", _titleStyle);
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField(
                "AnkleBreaker FishNet Core requires FishNet (Fish-Networking) to function.\n\n" +
                "FishNet was not detected in this project. Please install FishNet before using this package.\n\n" +
                "You can get FishNet from the Unity Asset Store or from the official GitHub repository.",
                _messageStyle);

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField(
                "Please install FishNet to continue.",
                _warningStyle);

            EditorGUILayout.Space(15);

            // Action buttons
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Open Asset Store", GUILayout.Width(150), GUILayout.Height(35)))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/network/fish-net-networking-evolved-207815");
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Open GitHub", GUILayout.Width(150), GUILayout.Height(35)))
            {
                Application.OpenURL("https://github.com/FirstGearGames/FishNet");
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Remind Me Later", GUILayout.Width(140), GUILayout.Height(25)))
            {
                Close();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Don't Show Again (this session)", GUILayout.Width(250), GUILayout.Height(25)))
            {
                SessionState.SetBool(FishNetCoreDependenciesInstaller.DISMISSED_KEY, true);
                Close();
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);
        }

        private static void InitStyles()
        {
            if (_titleStyle == null)
            {
                _titleStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = 16,
                    alignment = TextAnchor.MiddleCenter
                };
            }

            if (_messageStyle == null)
            {
                _messageStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
                {
                    fontSize = 12,
                    padding = new RectOffset(15, 15, 0, 0)
                };
            }

            if (_warningStyle == null)
            {
                _warningStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
                {
                    fontSize = 12,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter
                };
                _warningStyle.normal.textColor = new Color(1f, 0.6f, 0f);
            }
        }
    }
}
