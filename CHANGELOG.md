# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

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
