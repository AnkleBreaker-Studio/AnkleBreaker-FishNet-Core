# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.2.0] - 2026-07-02

### Added
* AnkleBreakerNetworkController: base class for networked Controllers — derives plain NetworkBehaviour and carries none of the bus registration/readiness machinery (no EventHandlerRegister, no IIsReady): a Controller has nothing to subscribe with; keeps the Awake -> RegisterSyncVarEvents slot for local SyncVar .OnChange subscriptions
* Script templates (moved from AnkleBreaker-Core and split): AnkleBreaker_NetworkBehaviour (register guards suggested in Register only, UnRegister always unconditional), AnkleBreaker_NetworkController (no registration blocks), AnkleBreaker_NetworkHandlerData (canonical Notifications/Commands/Queries sections, S_/C_ naming, one XML summary per event)

### Changed
* AnkleBreakerNetworkBehaviour: flattened to its actual role (networked Managers) — Awake -> RegisterSyncVarEvents slot, OnStartNetwork -> EventHandlerRegister -> IsLocallyReady, OnStopNetwork -> EventHandlerUnRegister; profiler sample label fixed (was "StartServer", it measures the register), IsLocallyReady tooltip fixed (OnStartNetwork, not OnStartClient/Server)

### Removed
* AnkleBreakerNetworkBehaviour: ResetBehaviour / Sync_resetBehaviour — pooling reset is native in FishNet 4: override ResetState(bool asServer) and call base, which also resets all SyncTypes; every networked object no longer carries an extra SyncVar
* AnkleBreakerNetworkBehaviour: owner-ready ServerRpc (SR_ClientOwnerIsReady / S_OnClientOwnerIsReady) — use OnStartClient + IsOwner, or OnSpawnServer (2 usages in studio history)
* AnkleBreakerNetworkBehaviour: OnOwnership-driven S_/C_OnPlayerConnect/Disconnect hooks — misleading names (ownership change, not connection); use FishNet connection events (ServerManager.OnRemoteConnectionState, ClientManager.OnClientConnectionState) or override OnOwnershipServer/Client directly

## [1.1.1] - 2026-03-06

### Changed
* Migrated dependencies from deprecated AnkleBreaker-Utils to new split packages (AnkleBreaker-Core, AnkleBreaker-Utils-Inspector)
* Replaced GUID-based assembly references with assembly name references in all .asmdef files

## [1.1.0] - 2026-03-06

### Added
* [FEAT] FishNetCoreDependenciesInstaller — auto-detects missing FishNet (Fish-Networking) on editor load via `#if !FISHNET` and shows a popup
* [FEAT] FishNetRequiredWindow — info-only EditorWindow notifying users that FishNet is required, with links to Asset Store and GitHub, remind later and dismiss per session
* [FEAT] Dedicated Installer assembly definition (AnkleBreaker.FishNetCore.Installer.Editor) — Editor-only, no defineConstraints to ensure it compiles without FishNet

## [1.0.0] - 2026-03-05

* [FEAT] Initial public release of AnkleBreaker FishNet Core
* [FEAT] AnkleBreakerNetworkBehaviour - abstract base class extending NetworkBehaviour with lifecycle management, SyncVar reset, ownership tracking, and player connect/disconnect callbacks
* [FEAT] AKSinglePrefabs - custom SinglePrefabObjects wrapper with null entry cleanup
* [FEAT] AKSinglePrefabsEditor - custom inspector with "Remove null occurrences" button
* [FEAT] NetPrefabsSelectorWindow - searchable EditorWindow for selecting network prefabs with multi-select support
* [FEAT] IntExtensions - TryGetNetworkObjectFromObjectId and L_IsLocalPlayerNobId extension methods
* [FEAT] NetworkConnection_Extensions - GetPlayerObjectId and GetPlayerObject extension methods
* [FEAT] SinglePrefabObjects_Extensions - LookupSpawnablePrefab extension method
* [FEAT] Assembly definitions for Runtime (Anklebreaker.Core.Fishnet, AnkleBreaker.Utils.Fishnet) and Editor (AnkleBreaker.Utils.Fishnet.Editor)
