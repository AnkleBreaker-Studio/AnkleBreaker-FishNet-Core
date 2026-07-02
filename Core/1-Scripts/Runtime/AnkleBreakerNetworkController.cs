using FishNet.Object;

namespace AnkleBreaker.Core.MasterClasses
{
    /// <summary>
    /// Base class for networked Controllers (the "body" of the Manager/HandlerData/Controller
    /// triad). A Controller never subscribes to the event bus — this base carries none of the
    /// registration/readiness machinery (no EventHandlerRegister, no IIsReady): there is
    /// nothing to override, nothing to subscribe with. It holds per-instance state via
    /// SyncVars, is driven by its Manager through a direct intra-feature reference, and pushes
    /// intent via the feature HandlerData Request helpers. For pooled objects, override
    /// FishNet's ResetState(bool asServer) and call base (it resets the SyncTypes).
    /// </summary>
    public abstract class AnkleBreakerNetworkController : NetworkBehaviour
    {
        public virtual void Awake()
        {
            RegisterSyncVarEvents();
        }

        /// <summary>
        /// Conventional slot for local SyncVar .OnChange subscriptions (instance-scoped,
        /// not the static bus). Called once from Awake.
        /// </summary>
        protected virtual void RegisterSyncVarEvents() { }
    }
}
