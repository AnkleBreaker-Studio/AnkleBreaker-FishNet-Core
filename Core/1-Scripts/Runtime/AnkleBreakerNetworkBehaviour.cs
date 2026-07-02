using FishNet.Object;
using UnityEngine;
using UnityEngine.Profiling;
using AnkleBreaker.Core.MasterInterfaces;
using AnkleBreaker.Utils.Inspector;

namespace AnkleBreaker.Core.MasterClasses
{
    /// <summary>
    /// Base class for networked Managers (and any loader-managed, bus-subscribing behaviour).
    /// Lifecycle: Awake -> RegisterSyncVarEvents (local .OnChange subscriptions);
    /// OnStartNetwork -> EventHandlerRegister then IsLocallyReady = true (the readiness flag
    /// the ManagersLoader waits on); OnStopNetwork -> EventHandlerUnRegister.
    /// </summary>
    public abstract class AnkleBreakerNetworkBehaviour : NetworkBehaviour, IIsReady
    {
        #region Properties
        [field: SerializeField, HideInNormalInspector,
                Tooltip("Set to true once OnStartNetwork has run (events registered)")]
        public bool IsLocallyReady { get; protected set; }
        #endregion

        #region Events Registering
        public virtual void Awake()
        {
            RegisterSyncVarEvents();
        }

        /// <summary>
        /// Conventional slot for local SyncVar .OnChange subscriptions (instance-scoped,
        /// not the static bus). Called once from Awake.
        /// </summary>
        protected virtual void RegisterSyncVarEvents() { }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            Profiler.BeginSample("ABNetwork.EventHandlerRegister." + GetType().Name);
            EventHandlerRegister();
            Profiler.EndSample();
            IsLocallyReady = true;
        }

        public override void OnStopNetwork()
        {
            base.OnStopNetwork();
            EventHandlerUnRegister();
        }

        protected abstract void EventHandlerRegister();
        protected abstract void EventHandlerUnRegister();
        #endregion

        #region Other Methods
        public virtual void OnDestroy() { }
        #endregion
    }
}
