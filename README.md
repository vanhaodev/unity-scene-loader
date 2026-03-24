# SceneLoader

**SceneLoader** is a simple Unity package for managing scene loading with built-in UI examples.

It comes with a **loading UI** including a smooth **progress bar**, and allows you to register tasks:

- Tasks to execute **before** the scene starts loading.  
- Tasks to execute **after** the scene finishes loading.

---

## Features

- Supports **single & additive scene loading**:  
  - **Single**: Replaces the currently active scene.  
  - **Additive**: Loads a new scene alongside existing ones.  
  You can optionally provide a **list of scenes to unload** after loading.

- Provides example **UI prefab** for loading screen.

- Supports **Unity 6 or higher**.

---

## Example Setup

This package includes two samples:
- `LoadSingle`
- `LoadAdditive`

Each sample contains its own `README.md` with detailed setup instructions.

### To get started:

1. Open **Package Manager** in Unity.
2. Select the **Scene Loader** package.
3. In the **Samples** section, click **Import**.
4. The samples will be copied to:
   `Assets/Samples/com.vanhaodev.sceneloader/`
5. Open a sample and follow the instructions in its `README.md`.

> ⚠️ Samples are not included automatically in your project. They must be imported via the Package Manager.

---

## Installation via Unity Package Manager (UPM)

Add the following line to your project's `Packages/manifest.json`:

```json
"com.vanhaodev.sceneloader": "https://github.com/vanhaodev/unity-scene-loader.git?path=Exported/com.vanhaodev.sceneloader#<version>"
