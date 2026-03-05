# AnkleBreaker FishNet Core

Core FishNet networking layer for AnkleBreaker Studio. Provides the foundational networking abstractions and utilities for FishNet-based multiplayer games.

## Features

- **AnkleBreakerNetworkBehaviour**: Abstract base class extending `NetworkBehaviour` with:
  - Automatic event handler registration/unregistration on network start/stop
  - SyncVar-based behaviour reset mechanism
  - Ownership change tracking with server and client callbacks for player connect/disconnect
  - `IsLocallyReady` state tracking
  - `S_OnClientOwnerIsReady` server callback when the owning client is ready
- **AKSinglePrefabs**: Custom `SinglePrefabObjects` wrapper with null entry cleanup and custom editor
- **NetPrefabsSelectorWindow**: Searchable EditorWindow for selecting network prefabs with multi-select support
- **Extension Methods**:
  - `NetworkConnection.GetPlayerObjectId()` / `GetPlayerObject()` - Quick access to a connection's player object
  - `int.TryGetNetworkObjectFromObjectId()` - Resolve ObjectId to NetworkObject (client or server)
  - `int.L_IsLocalPlayerNobId()` - Check if an ObjectId belongs to the local player
  - `SinglePrefabObjects.LookupSpawnablePrefab(name)` - Find a prefab by name in a prefab collection

## Installation

To use this package in your Unity project, you can install it via the Package Manager:

### Via Package Manager URL

1. Open the Package Manager (Window > Package Manager)
2. Click the **+** button and select **Add package from git URL**
3. Enter: `https://github.com/AnkleBreaker-Studio/AnkleBreaker-FishNet-Core.git`

### Via AnkleBreaker Package Updater

Select the package in the Unity toolbar: **Anklebreaker/Packages/Package Updater**
*(Requires [Unity-Tools](https://github.com/AnkleBreaker-Studio/Unity-Tools) to be installed)*

## Dependencies

- [FishNet (Fish-Networking)](https://fish-networking.gitbook.io/docs)

## Package Structure

```
AnkleBreaker-FishNet-Core/
├── Core/
│   └── 1-Scripts/
│       └── Runtime/
│           ├── Anklebreaker.Core.Fishnet.asmdef
│           └── AnkleBreakerNetworkBehaviour.cs
├── Utils/
│   └── 1-Scripts/
│       ├── Editor/
│       │   ├── AKSinglePrefabsEditor.cs
│       │   ├── AnkleBreaker.Utils.Fishnet.Editor.asmdef
│       │   └── NetPrefabsSelectorWindow.cs
│       └── Runtime/
│           └── FishNet/
│               ├── AKSinglePrefabs.cs
│               ├── AnkleBreaker.Utils.Fishnet.asmdef
│               └── ExtensionMethods/
│                   ├── Built-inType_Extensions/
│                   │   └── IntExtensions.cs
│                   └── FishNetType_Extensions/
│                       ├── NetworkConnection_Extensions.cs
│                       └── SinglePrefabObjects_Extensions.cs
├── package.json
├── README.md
└── CHANGELOG.md
```

## Quick Start

1. Install the package via the Package Manager
2. Ensure FishNet is installed in your project
3. Create your networked scripts by extending `AnkleBreakerNetworkBehaviour`
4. Override `EventHandlerRegister()` and `EventHandlerUnRegister()` for event lifecycle
5. Use `S_OnClientOwnerIsReady()` to react when the owning client is ready
6. Use `S_OnPlayerConnect()` / `S_OnPlayerDisconnect()` for ownership change callbacks

## Packages Hosting Setup

### On Plastic

- Pull the GitHubPackagesHosting project on plastic
- **Create your branch from Development**

### On GitHub Desktop

- Pull the package repo in the Assets folder of the pulled GitHubPackagesHosting project
- **Create your branch from Development**

## Working with the Package

### Add a new feature

- Create its own folder & its assembly with the standard feature organisation. The assembly must be named "AnkleBreaker.YourFeature"
- Make sure to create an assembly Editor if your feature needs Editor scripts
- All your scripts should have a namespace to not conflict with local scripts in another project

### Add a package dependency

If a feature needs another package from the studio or external, add it in `package.json` in the "dependencies" section.
**If the package cannot be installed from the package manager automatically, indicate the link to download it.**

### Testing your changes

- Open the package updater in the Unity toolbar: **Anklebreaker/Packages/Package Updater**
- Select your package & write the name of your branch
- **Make sure to update your clones**

## Push your changes on Development

- **MAKE SURE TO UPDATE FROM DEVELOPMENT**
- Update changelogs and version using one of these methods:
  - Use **Anklebreaker/Packages/Versioning** *(requires Unity-Tools)*
  - Manually edit `CHANGELOG.md` and update the version in `package.json`
- Push on your branch
- Create a **Pull Request** to merge into Development
