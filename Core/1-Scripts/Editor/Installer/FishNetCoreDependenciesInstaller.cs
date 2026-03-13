using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace AnkleBreaker.FishNetCore.Editor
{
    // ═══════════════════════════════════════════════════════════
    // Data Structures
    // ═══════════════════════════════════════════════════════════

    [Serializable]
    public class FishNetDependencyInfo
    {
        public string DisplayName;
        public string AssemblyName;
        // Git install
        public string GitUrl;
        public string GitPageUrl;
        // Asset Store install
        public string AssetStoreUrl;
        public string AssetStoreDeepLink;
    }

    // ═══════════════════════════════════════════════════════════
    // Static Installer — Detection, Installation, Auto-check
    // ═══════════════════════════════════════════════════════════

    public static class FishNetCoreDependenciesInstaller
    {
        public const string SESSION_KEY = "AB_FishNetCore_DepsCheck";
        public const string FISHNET_V4_DEFINE = "FISHNET_V4";

        private static readonly FishNetDependencyInfo[] Dependencies = new[]
        {
            new FishNetDependencyInfo
            {
                DisplayName        = "FishNet (Fish-Networking) 4.x+",
                AssemblyName       = "FishNet.Runtime",
                GitUrl             = "https://github.com/FirstGearGames/FishNet.git",
                GitPageUrl         = "https://github.com/FirstGearGames/FishNet",
                AssetStoreUrl      = "https://assetstore.unity.com/packages/tools/network/fish-net-networking-evolved-207815",
                AssetStoreDeepLink = "com.unity3d.kharma:content/207815"
            }
        };

        // ═══════════════════════════════════════
        // Auto-check on Domain Reload
        // ═══════════════════════════════════════

        [InitializeOnLoadMethod]
        private static void CheckOnLoad()
        {
            EditorApplication.delayCall += () =>
            {
                // Manage FISHNET_V4 scripting define
                if (IsFishNetV4OrHigher())
                    EnsureFishNetV4Define();
                else
                    RemoveFishNetV4DefineIfNeeded();

                if (AllDependenciesSatisfied()) return;
                if (SessionState.GetBool(SESSION_KEY, false)) return;
                SessionState.SetBool(SESSION_KEY, true);
                FishNetRequiredWindow.ShowWindow();
            };
        }

        // ═══════════════════════════════════════
        // Detection
        // ═══════════════════════════════════════

        /// <summary>
        /// Returns true only if FishNet 4.x+ is detected (assembly loaded AND correct API).
        /// </summary>
        public static bool AllDependenciesSatisfied()
        {
            return IsFishNetV4OrHigher();
        }

        public static bool IsFishNetInstalled()
        {
            return IsAssemblyLoaded("FishNet.Runtime");
        }

        /// <summary>
        /// Detects FishNet 4.x+ by checking if SyncVar&lt;T&gt; has a public 'Value' property.
        /// FishNet 2.x uses GetValue()/SetValue() instead — no 'Value' property.
        /// </summary>
        public static bool IsFishNetV4OrHigher()
        {
            var asm = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "FishNet.Runtime");
            if (asm == null) return false;

            // FishNet 4.x+ exposes SyncVar<T> as a generic class with a public 'Value' property
            var syncVarOpen = asm.GetType("FishNet.Object.Synchronizing.SyncVar`1");
            if (syncVarOpen == null) return false;

            return syncVarOpen.GetProperty("Value",
                BindingFlags.Public | BindingFlags.Instance) != null;
        }

        public static bool IsAssemblyLoaded(string assemblyName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => a.GetName().Name == assemblyName);
        }

        public static FishNetDependencyInfo[] GetDependencies() => Dependencies;

        // ═══════════════════════════════════════
        // FISHNET_V4 Scripting Define
        // ═══════════════════════════════════════

        public static void EnsureFishNetV4Define()
        {
            var target = EditorUserBuildSettings.selectedBuildTargetGroup;
            if (target == BuildTargetGroup.Unknown) return;

            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
            if (defines.Contains(FISHNET_V4_DEFINE)) return;

            string newDefines = string.IsNullOrEmpty(defines)
                ? FISHNET_V4_DEFINE
                : defines + ";" + FISHNET_V4_DEFINE;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, newDefines);
            Debug.Log("[AB FishNet Core] Added FISHNET_V4 scripting define — FishNet 4.x+ detected.");
        }

        public static void RemoveFishNetV4DefineIfNeeded()
        {
            if (IsFishNetV4OrHigher()) return;

            var target = EditorUserBuildSettings.selectedBuildTargetGroup;
            if (target == BuildTargetGroup.Unknown) return;

            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
            if (!defines.Contains(FISHNET_V4_DEFINE)) return;

            var list = defines.Split(';').Where(d => d.Trim() != FISHNET_V4_DEFINE);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, string.Join(";", list));
            Debug.Log("[AB FishNet Core] Removed FISHNET_V4 scripting define — FishNet 4.x+ not detected.");
        }

        // ═══════════════════════════════════════
        // Installation
        // ═══════════════════════════════════════

        public static void InstallViaGit(FishNetDependencyInfo dep)
        {
            Debug.Log($"[AB FishNet Core] Installing {dep.DisplayName} from {dep.GitUrl}...");
            Client.Add(dep.GitUrl);
        }

        public static void InstallAllMissingGit()
        {
            foreach (var dep in Dependencies)
            {
                if (!IsFishNetV4OrHigher())
                    InstallViaGit(dep);
            }
        }

        // ═══════════════════════════════════════
        // Menu Items
        // ═══════════════════════════════════════

        [MenuItem("Tools/AnkleBreaker/Dependencies Installer/FishNet Core", priority = 100)]
        public static void OpenWindow()
        {
            FishNetRequiredWindow.ShowWindow();
        }
    }
}
