Add the following scenes in Build Settings:

SceneLoader.Bootstrap → index 0
SceneLoader.Scene1 → index 1
SceneLoader.Scene2 → index 2

Start play at SceneLoader.Bootstrap

This ensures the loader system can find scene indices correctly.