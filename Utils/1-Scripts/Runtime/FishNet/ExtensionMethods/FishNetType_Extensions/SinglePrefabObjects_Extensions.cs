using FishNet.Managing.Object;
using FishNet.Object;
using System.Linq;
using UnityEngine;

namespace AnkleBreaker.Utils.ExtensionMethods
{
    public static class SinglePrefabObjects_Extensions
    {
        public static NetworkObject LookupSpawnablePrefab(this SinglePrefabObjects prefabsList, string name)
        {
            if (prefabsList == null)
            {
                Debug.LogError("LookupSpawnablePrefab :: No SinglePrefabObjects was provided");
                return null;
            }

            if (prefabsList.Prefabs == null)
            {
                Debug.LogError("LookupSpawnablePrefab :: The variable Prefabs of SinglePrefabObjects is null");
                return null;
            }

            return prefabsList.Prefabs.FirstOrDefault(n => n != null && n.name == name);
        }
    }
}
