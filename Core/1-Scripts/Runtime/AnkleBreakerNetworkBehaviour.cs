using FishNet.Object;
using UnityEngine;
using AnkleBreaker.Core.MasterInterfaces;
using AnkleBreaker.Utils.Inspector;

namespace AnkleBreaker.Core.MasterClasses
{
    /// <summary>Base class for networked Managers. OnStartNetwork registers the bus then
    /// flips IsLocallyReady — the flag the ManagersLoader waits on.</summary>
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

        /// <summary>Slot for local SyncVar .OnChange subscriptions (instance-scoped, not the
        /// static bus). Called once from Awake.</summary>
        protected virtual void RegisterSyncVarEvents() { }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            EventHandlerRegister();
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
