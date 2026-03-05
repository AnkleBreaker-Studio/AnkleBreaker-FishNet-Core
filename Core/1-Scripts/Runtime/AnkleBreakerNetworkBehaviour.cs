using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.Profiling;
using AnkleBreaker.Core.MasterInterfaces;
using AnkleBreaker.Utils.Inspector;
using FishNet.Connection;

namespace AnkleBreaker.Core.MasterClasses
{
    public abstract class AnkleBreakerNetworkBehaviour : NetworkBehaviour, IIsReady
    {
        #region Properties
        [field: SerializeField, HideInNormalInspector,
                Tooltip("Set to true once OnStartClient/Server is over")]
        public bool IsLocallyReady { get; protected set; }
        
        private readonly SyncVar<ulong> Sync_resetBehaviour = new SyncVar<ulong>(0);
        #endregion

        #region Events Registering

        public virtual void Awake()
        {
            RegisterSyncVarEvents();
        }

        protected virtual void RegisterSyncVarEvents()
        {
            Sync_resetBehaviour.OnChange += OnSyncVarResetBehaviourChanged;
        }
        
        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            Profiler.BeginSample("ABNetwork.StartServer." + this.GetType().Name);
            EventHandlerRegister();
            Profiler.EndSample();
            IsLocallyReady = true;
        }

        public override void OnStopNetwork()
        {
            base.OnStopNetwork();
            EventHandlerUnRegister();
        }

        public void ResetBehaviour()
        {
            Sync_resetBehaviour.Value++;
        }

        private void OnSyncVarResetBehaviourChanged(ulong prev, ulong next, bool asServer)
        {
            OnResetBehaviour(asServer);
        }

        protected virtual void OnResetBehaviour(bool asServer)
        {

        }

        protected abstract void EventHandlerRegister();
        protected abstract void EventHandlerUnRegister();
        #endregion

        #region On Owner is Ready
        public override void OnStartClient()
        {
            base.OnStartClient();
            if (IsOwner)
                SR_ClientOwnerIsReady();
        }

        [ServerRpc] void SR_ClientOwnerIsReady() => S_OnClientOwnerIsReady();

        /// <summary>
        /// Called when Client is ready
        /// </summary>
        [Server]
        protected virtual void S_OnClientOwnerIsReady() { }
        
        public override void OnOwnershipServer(NetworkConnection prevOwner)
        {
            base.OnOwnershipServer(prevOwner);
            if (prevOwner != null && prevOwner.IsValid)
            {
                if (Owner == null || Owner.IsValid == false)
                {
                    S_OnPlayerDisconnect();
                }
            }
            else
            {
                if (Owner != null && Owner.IsValid)
                {
                    S_OnPlayerConnect();
                }
            }
        }

		public override void OnOwnershipClient(NetworkConnection prevOwner)
		{
			base.OnOwnershipClient(prevOwner);
			if (prevOwner != null && prevOwner.IsValid)
			{
				if (Owner == null || Owner.IsValid == false)
				{
					C_OnPlayerDisconnect();
				}
			}
			else
			{
				if (Owner != null && Owner.IsValid)
				{
					C_OnPlayerConnect();
				}
			}
		}

		[Server]
        protected virtual void S_OnPlayerConnect() { }
        
        [Server]
        protected virtual void S_OnPlayerDisconnect() { }

        [Client]
        protected virtual void C_OnPlayerConnect() { }

        [Client]
        protected virtual void C_OnPlayerDisconnect() { }
        
        #endregion

        #region Other Methods
        public virtual void OnDestroy() { }
        #endregion
    }
}