using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace AnkleBreaker.FishNetCore.Editor
{
    public class FishNetRequiredWindow : EditorWindow
    {
        private const string FISHNET_GIT_URL = "https://github.com/FirstGearGames/FishNet.git";
        private const string FISHNET_GITHUB_PAGE = "https://github.com/FirstGearGames/FishNet";
        private const string FISHNET_ASSET_STORE_URL = "https://assetstore.unity.com/packages/tools/network/fish-net-networking-evolved-207815";
        private const string FISHNET_ASSET_STORE_DEEP_LINK = "com.unity3d.kharma:content/207815";

        private static GUIStyle _titleStyle;
        private static GUIStyle _messageStyle;
        private static GUIStyle _warningStyle;
        private static GUIStyle _sectionLabelStyle;

        public static void ShowWindow()
        {
            var window = GetWindow<FishNetRequiredWindow>(true, "AnkleBreaker FishNet Core - Missing FishNet", true);
            window.minSize = new Vector2(520, 320);
            window.maxSize = new Vector2(520, 320);
            window.ShowUtility();
        }

        private void OnGUI()
        {
            InitStyles();

            bool fishnetInstalled = false;
#if FISHNET
            fishnetInstalled = true;
#endif

            EditorGUILayout.Space(15);
            EditorGUILayout.LabelField("FishNet Required", _titleStyle);
            EditorGUILayout.Space(10);

            if (fishnetInstalled)
            {
                EditorGUILayout.LabelField(
                    "FishNet has been detected in the project.\n\nAll dependencies are satisfied. You're good to go!",
                    _messageStyle);
            }
            else
            {
                EditorGUILayout.LabelField(
                    "AnkleBreaker FishNet Core requires FishNet (Fish-Networking) to function.\n\n" +
                    "FishNet was not detected in this project. Please install FishNet before using this package.\n\n" +
                    "You can install FishNet from the Unity Asset Store or from the official GitHub repository.",
                    _messageStyle);

                EditorGUILayout.Space(10);

                EditorGUILayout.LabelField(
                    "Please install FishNet to continue.",
                    _warningStyle);
            }

            EditorGUILayout.Space(15);

            if (!fishnetInstalled)
            {
                // --- Section labels row (100 + 6 + 100 = 206px per group, 20px gap) ---
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("GitHub", _sectionLabelStyle, GUILayout.Width(206));
                GUILayout.Space(20);
                EditorGUILayout.LabelField("Asset Store", _sectionLabelStyle, GUILayout.Width(206));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(4);

                // --- Buttons row ---
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                // GitHub buttons
                if (GUILayout.Button("Visit", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    Application.OpenURL(FISHNET_GITHUB_PAGE);
                }

                GUILayout.Space(6);

                var githubInstallContent = new GUIContent("Install",
                    "Install FishNet via the Unity Package Manager from the GitHub repository.");

                if (GUILayout.Button(githubInstallContent, GUILayout.Width(100), GUILayout.Height(30)))
                {
                    Client.Add(FISHNET_GIT_URL);
                    EditorUtility.DisplayDialog(
                        "FishNet Installation",
                        "FishNet is being installed via the Unity Package Manager.\nPlease wait for the import to complete.",
                        "OK");
                }

                GUILayout.Space(20);

                // Asset Store buttons
                if (GUILayout.Button("Visit", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    Application.OpenURL(FISHNET_ASSET_STORE_URL);
                }

                GUILayout.Space(6);

                var assetStoreInstallContent = new GUIContent("Install",
                    "Opens the Package Manager on FishNet. You must first add FishNet to your Unity account (free) via the Asset Store website.");

                if (GUILayout.Button(assetStoreInstallContent, GUILayout.Width(100), GUILayout.Height(30)))
                {
                    Application.OpenURL(FISHNET_ASSET_STORE_DEEP_LINK);
                }

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.FlexibleSpace();

            // --- Bottom button ---
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (fishnetInstalled)
            {
                if (GUILayout.Button("Ok", GUILayout.Width(120), GUILayout.Height(30)))
                {
                    Close();
                }
            }
            else
            {
                if (GUILayout.Button("I understand, this package will not work", GUILayout.Width(300), GUILayout.Height(30)))
                {
                    SessionState.SetBool(FishNetCoreDependenciesInstaller.DISMISSED_KEY, true);
                    Close();
                }
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

            if (_sectionLabelStyle == null)
            {
                _sectionLabelStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = 12,
                    alignment = TextAnchor.MiddleCenter
                };
            }
        }
    }
}
