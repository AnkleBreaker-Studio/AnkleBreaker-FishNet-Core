using FishNet.Connection;
using FishNet.Object;

namespace AnkleBreaker.Utils.ExtensionMethods
{
    public static class NetworkConnection_Extensions
    {
        public static int GetPlayerObjectId(this NetworkConnection source)
        {
            return source != null && source.IsValid && source.FirstObject ? source.FirstObject.ObjectId : -1;
        }
        public static NetworkObject GetPlayerObject(this NetworkConnection source)
        {
            return source != null && source.IsValid && source.FirstObject ? source.FirstObject : null;
        }
    }
}
