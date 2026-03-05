using FishNet;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Object;
using System;
using FishNet.Connection;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace AnkleBreaker.Utils.ExtensionMethods
{
    public static partial class IntExtensions
    {
        public static bool TryGetNetworkObjectFromObjectId(this int objectId, out NetworkObject networkObject)
        {
            networkObject = null;
            if (InstanceFinder.IsClient)
            {
                ClientManager clientManager = InstanceFinder.ClientManager;
                if (clientManager == null || !clientManager.Objects.Spawned.ContainsKey(objectId))
                    return false;

                networkObject = clientManager.Objects.Spawned[objectId];
                return true;
            }
            else if (InstanceFinder.IsServer)
            {
                ServerManager serverManager = InstanceFinder.ServerManager;
                if (serverManager == null || !serverManager.Objects.Spawned.ContainsKey(objectId))
                    return false;

                networkObject = serverManager.Objects.Spawned[objectId];
                return true;
            }

            return false;
        }

        public static bool L_IsLocalPlayerNobId(this int objectId)
        {
            if (!InstanceFinder.IsClient) return false;
            
            return objectId.TryGetNetworkObjectFromObjectId(out NetworkObject nob)
                   && nob.Owner == InstanceFinder.ClientManager.Connection;
        }
    }
}