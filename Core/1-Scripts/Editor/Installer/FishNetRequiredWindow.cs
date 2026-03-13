using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AnkleBreaker.FishNetCore.Editor
{
    public class FishNetRequiredWindow : EditorWindow
    {
        private bool _isInstalling;

        public static void ShowWindow()
        {
            var window = GetWindow<FishNetRequiredWindow>(
                true, "AnkleBreaker FishNet Core \u2014 Dependencies");
            window.minSize = new Vector2(540, 440);
            window.maxSize = new Vector2(540, 500);
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(10);

            // ── Header ──
            var headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter
            };
            EditorGUILayout.LabelField("AnkleBreaker FishNet Core", headerStyle);
            EditorGUILayout.LabelField("Dependencies", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.Space(8);

            var deps = FishNetCoreDependenciesInstaller.GetDependencies();
            bool fishnetInstalled = FishNetCoreDependenciesInstaller.IsFishNetInstalled();
            bool fishnetV4 = FishNetCoreDependenciesInstaller.IsFishNetV4OrHigher();
            bool allSatisfied = fishnetV4;

            // ── Status box with per-dependency rows ──
            EditorGUILayout.BeginVertical("box");
            foreach (var dep in deps)
            {
                DrawDependencyRow(dep, fishnetInstalled, fishnetV4);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(8);

            if (allSatisfied)
            {
                // ── All good ──
                EditorGUILayout.HelpBox(
                    "All dependencies are installed! AnkleBreaker FishNet Core is ready to use.",
                    MessageType.Info);
            }
            else if (fishnetInstalled && !fishnetV4)
            {
                // ── FishNet installed but wrong version ──
                EditorGUILayout.HelpBox(
                    "FishNet is installed but the detected version is NOT compatible.\n\n" +
                    "AnkleBreaker FishNet Core requires FishNet 4.x+ which uses the " +
                    "SyncVar<T> generic class with .Value property.\n\n" +
                    "Your current version uses the old attribute-based [SyncVar] system " +
                    "which is NOT supported. Please update FishNet to version 4.x or higher.",
                    MessageType.Error);

                EditorGUILayout.Space(8);
                DrawInstallButtons(deps.First());
            }
            else
            {
                // ── FishNet not installed at all ──
                EditorGUILayout.HelpBox(
                    "AnkleBreaker FishNet Core requires FishNet 4.x+ (the version with " +
                    "SyncVar<T> generic class and .Value property).\n\n" +
                    "Older versions using the attribute-based [SyncVar] system are NOT compatible.\n\n" +
                    "You can install FishNet from GitHub (via UPM) or from the Unity Asset Store.",
                    MessageType.Warning);

                EditorGUILayout.Space(8);
                DrawInstallButtons(deps.First());
            }

            GUILayout.FlexibleSpace();

            // ── Bottom buttons ──
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (allSatisfied)
            {
                if (GUILayout.Button("OK", GUILayout.Width(100), GUILayout.Height(28)))
                    Close();
            }
            else
            {
                if (GUILayout.Button("I understand, this package will not work", GUILayout.Width(300), GUILayout.Height(28)))
                {
                    SessionState.SetBool(FishNetCoreDependenciesInstaller.SESSION_KEY, true);
                    Close();
                }

                GUILayout.Space(8);

                if (GUILayout.Button("Recheck", GUILayout.Width(80), GUILayout.Height(28)))
                {
                    // Re-evaluate defines
                    if (FishNetCoreDependenciesInstaller.IsFishNetV4OrHigher())
                        FishNetCoreDependenciesInstaller.EnsureFishNetV4Define();
                    Repaint();
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(8);
        }

        private void DrawInstallButtons(FishNetDependencyInfo dep)
        {
            var sectionLabelStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            // Section labels row
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("GitHub", sectionLabelStyle, GUILayout.Width(206));
            GUILayout.Space(20);
            EditorGUILayout.LabelField("Asset Store", sectionLabelStyle, GUILayout.Width(206));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(4);

            // Buttons row
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // GitHub: Visit + Install
            if (GUILayout.Button("Visit", GUILayout.Width(100), GUILayout.Height(30)))
                Application.OpenURL(dep.GitPageUrl);

            GUILayout.Space(6);

            var gitInstallContent = new GUIContent("Install",
                "Install FishNet via the Unity Package Manager from the GitHub repository.");

            EditorGUI.BeginDisabledGroup(_isInstalling);
            if (GUILayout.Button(gitInstallContent, GUILayout.Width(100), GUILayout.Height(30)))
            {
                _isInstalling = true;
                FishNetCoreDependenciesInstaller.InstallViaGit(dep);
                EditorUtility.DisplayDialog(
                    "FishNet Installation",
                    "FishNet is being installed via the Unity Package Manager.\nPlease wait for the import to complete.",
                    "OK");
            }
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(20);

            // Asset Store: Visit + Install
            if (GUILayout.Button("Visit", GUILayout.Width(100), GUILayout.Height(30)))
                Application.OpenURL(dep.AssetStoreUrl);

            GUILayout.Space(6);

            var assetStoreInstallContent = new GUIContent("Install",
                "Opens the Package Manager on FishNet. You must first add FishNet to your Unity account (free) via the Asset Store website.");

            if (GUILayout.Button(assetStoreInstallContent, GUILayout.Width(100), GUILayout.Height(30)))
                Application.OpenURL(dep.AssetStoreDeepLink);

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawDependencyRow(FishNetDependencyInfo dep, bool fishnetInstalled, bool fishnetV4)
        {
            EditorGUILayout.BeginHorizontal();

            // Status icon
            bool installing = !fishnetV4 && _isInstalling;
            Color dotColor;
            string status;

            if (fishnetV4)
            {
                dotColor = Color.green;
                status = "Installed (4.x+)";
            }
            else if (fishnetInstalled)
            {
                dotColor = new Color(1f, 0.6f, 0f); // Orange — wrong version
                status = "WRONG VERSION (needs 4.x+)";
            }
            else if (installing)
            {
                dotColor = new Color(1f, 0.6f, 0f);
                status = "Installing...";
            }
            else
            {
                dotColor = Color.red;
                status = "MISSING";
            }

            var iconName = fishnetV4 ? "d_greenLight" : fishnetInstalled ? "d_orangeLight" : "d_redLight";
            var icon = EditorGUIUtility.IconContent(iconName);
            if (icon != null && icon.image != null)
            {
                GUILayout.Label(icon, GUILayout.Width(20), GUILayout.Height(20));
            }
            else
            {
                var rect = GUILayoutUtility.GetRect(20, 20, GUILayout.Width(20), GUILayout.Height(20));
                EditorGUI.DrawRect(new Rect(rect.x + 4, rect.y + 4, 12, 12), dotColor);
            }

            // Label
            var style = new GUIStyle(EditorStyles.label)
            {
                fontSize = 13,
                fontStyle = fishnetV4 ? FontStyle.Normal : FontStyle.Bold
            };
            EditorGUILayout.LabelField($"{dep.DisplayName}  \u2014  {status}", style);

            EditorGUILayout.EndHorizontal();
        }
    }
}
