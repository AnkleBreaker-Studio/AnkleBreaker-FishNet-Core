using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace AnkleBreaker.FishNetCore.Editor.Tests
{
    public class DependencyPromptTests
    {
        [Test]
        public void DomainChecks_UpdateVersionDefineWithoutOpeningWindow()
        {
            var check = typeof(FishNetCoreDependenciesInstaller).GetMethod("CheckOnLoad",
                BindingFlags.Static | BindingFlags.NonPublic);
            Assert.IsNotNull(check.GetCustomAttribute<InitializeOnLoadMethodAttribute>(),
                "Version detection must remain automatic even though the window is manual");

            const string key = FishNetCoreDependenciesInstaller.SESSION_KEY;
            bool previousDismissal = SessionState.GetBool(key, false);
            var previousDelayCall = EditorApplication.delayCall;
            try
            {
                CloseTestWindows();
                SessionState.SetBool(key, false);
                check.Invoke(null, null);
                check.Invoke(null, null);
                var scheduled = EditorApplication.delayCall.GetInvocationList()
                    .Skip(previousDelayCall?.GetInvocationList().Length ?? 0).ToArray();
                EditorApplication.delayCall = previousDelayCall;
                Assert.AreEqual(2, scheduled.Length);
                foreach (var callback in scheduled)
                    callback.DynamicInvoke();

                Assert.IsEmpty(Resources.FindObjectsOfTypeAll<FishNetRequiredWindow>());
                Assert.IsFalse(SessionState.GetBool(key, false),
                    "A background check must not dismiss a window on the user's behalf");

                var target = EditorUserBuildSettings.selectedBuildTargetGroup;
                bool hasVersionDefine = PlayerSettings.GetScriptingDefineSymbolsForGroup(target)
                    .Split(';').Contains(FishNetCoreDependenciesInstaller.FISHNET_V4_DEFINE);
                Assert.AreEqual(FishNetCoreDependenciesInstaller.IsFishNetV4OrHigher(), hasVersionDefine);
            }
            finally
            {
                EditorApplication.delayCall = previousDelayCall;
                SessionState.SetBool(key, previousDismissal);
                CloseTestWindows();
            }
        }

        [UnityTest]
        public IEnumerator Menu_OpensDependencyWindowAndClosingDoesNotReopenIt()
        {
            try
            {
                Assert.IsEmpty(Resources.FindObjectsOfTypeAll<FishNetRequiredWindow>(),
                    "The dependency window must not open on project startup");
                Assert.IsTrue(EditorApplication.ExecuteMenuItem(
                    "Tools/AnkleBreaker/Dependencies Installer/FishNet Core"));
                yield return null;
                Assert.AreEqual(1, Resources.FindObjectsOfTypeAll<FishNetRequiredWindow>().Length);

                CloseTestWindows();
                yield return null;
                yield return null;
                Assert.IsEmpty(Resources.FindObjectsOfTypeAll<FishNetRequiredWindow>());
            }
            finally
            {
                CloseTestWindows();
            }
        }

        private static void CloseTestWindows()
        {
            foreach (var window in Resources.FindObjectsOfTypeAll<FishNetRequiredWindow>())
                window.Close();
        }
    }
}
