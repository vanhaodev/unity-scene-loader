# SceneLoader

**SceneLoader** is a simple Unity package for managing scene loading with built-in UI examples.

It comes with a **loading UI** including a smooth **progress bar**, and allows you to register tasks:

- Tasks to execute **before** the scene starts loading.  
- Tasks to execute **after** the scene finishes loading.

---

## Features

- Supports **additive scene loading**:  
  The new scene is added to the currently open scene instead of replacing it.  
  You can optionally provide a **list of scenes to unload** after the new scene is loaded.

- Provides example **UI prefab** for loading screen.

- Supports **Unity 6 or higher**.

---

## Example Setup
This package includes example scenes in the `Samples~` folder.

To try them:

1. Open **Package Manager** in Unity.
2. Select **Scene Loader** package.
3. In the **Samples** section on the right, click **Import**.
4. The example scene will be copied to `Assets/Samples/com.vanhaodev.sceneloader/`.
5. Open the scene from there to test the Scene Loader.

Note: Example scenes are not included automatically in the project—they must be imported via the Package Manager.

1. Add the following scenes in **Build Settings**:  
   - `SceneLoader.Bootstrap` → index 0  
   - `SceneLoader.Scene1` → index 1  
   - `SceneLoader.Scene2` → index 2  

   This ensures the loader system can find scene indices correctly.

2. In `SceneLoader.Bootstrap`, there is a **GameObject prefab** named `SceneLoader`.  
   - This prefab contains the loading UI and core SceneLoader logic.  

3. Use the `SceneLoader` prefab to manage scene transitions, register tasks, and display loading progress.

---

## Installation via Unity Package Manager (UPM)

Add the following line to your project's `Packages/manifest.json`:

```json
"com.vanhaodev.sceneloader": "https://github.com/vanhaodev/unity-scene-loader.git?path=Exported/com.vanhaodev.sceneloader#<version>"
