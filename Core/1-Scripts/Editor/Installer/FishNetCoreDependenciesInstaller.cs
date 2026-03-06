using UnityEditor;

namespace AnkleBreaker.FishNetCore.Editor
{
    public class FishNetCoreDependenciesInstaller
    {
        public const string DISMISSED_KEY = "AB_FishNetCore_FishNet_Dismissed";

        [InitializeOnLoadMethod]
        public static void CheckAllDependencies()
        {
            bool fishnetMissing = false;

#if !FISHNET
            fishnetMissing = true;
#endif

            if (fishnetMissing && !SessionState.GetBool(DISMISSED_KEY, false))
            {
                EditorApplication.delayCall += () =>
                {
                    FishNetRequiredWindow.ShowWindow();
                };
            }
        }
    }
}
