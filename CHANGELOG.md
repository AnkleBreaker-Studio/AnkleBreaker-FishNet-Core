# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.1.2] - 2026-10-22

### Fixed
* Missing FishNet no longer opens the dependency window on editor startup or domain reload. The installer remains available from Tools > AnkleBreaker > Dependencies Installer > FishNet Core.
* Closing the manual installer no longer records a session dismissal. Automatic FishNet version detection and FISHNET_V4 define synchronization are preserved.

### Added
* EditMode regression tests for background dependency checks and manual window access.

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
