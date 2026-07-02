using FishNet.Object;

namespace AnkleBreaker.Core.MasterClasses
{
    /// <summary>Base class for networked Controllers: no bus registration, no readiness —
    /// driven by its Manager, keeps SyncVars. Pooling: override ResetState and call base.</summary>
    public abstract class AnkleBreakerNetworkController : NetworkBehaviour
    {
        public virtual void Awake()
        {
            RegisterSyncVarEvents();
        }

        /// <summary>Slot for local SyncVar .OnChange subscriptions (instance-scoped, not the
        /// static bus). Called once from Awake.</summary>
        protected virtual void RegisterSyncVarEvents() { }
    }
}
