using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace vanhaodev.sceneloader
{
	public partial class SceneLoader
	{
		public void SetUnloadScenes(IEnumerable<int> sceneIndexes)
		{
			_unloadSceneIndexes = new List<int>(sceneIndexes);
		}

		public void SetUnloadScenes(IEnumerable<string> sceneNames)
		{
			_unloadSceneIndexes = new List<int>();

			foreach (var sceneName in sceneNames)
			{
				int index = GetSceneIndexByName(sceneName);
				if (index != -1)
				{
					_unloadSceneIndexes.Add(index);
				}
				else
				{
					Debug.LogWarning($"Scene not found in BuildSettings: {sceneName}");
				}
			}
		}

		private async Task HandleLoadSceneAdditiveAsync()
		{
			//--------------------Show loading UI---------------------\\
			_currentProgress = 0f;
			UpdateProgress(_currentProgress);
			CalculateProgress();
			_ui.ShowLoading();

			//--------------------Load start tasks---------------------\\
			await LoadStartTasksAsync();

			//--------------------Load new scene---------------------\\
			AsyncOperation op = SceneManager.LoadSceneAsync(_sceneIndex, LoadSceneMode.Additive);
			op.allowSceneActivation = false;
			_ui.SetLoadName("Load scene");
			while (op.progress < 0.9f)
			{
				float sceneProgress = op.progress / 0.9f;
				float totalProgress = 0.2f + sceneProgress * 0.4f;

				_currentProgress = totalProgress; // 🔥 sync

				UpdateProgress(_currentProgress);

				await Task.Yield();
			}

			op.allowSceneActivation = true;
			while (!op.isDone)
			{
				await Task.Yield();
			}

			_currentProgress = 0.6f;
			UpdateProgress(_currentProgress);
			Scene newScene = SceneManager.GetSceneByBuildIndex(_sceneIndex);
			SceneManager.SetActiveScene(newScene);

			//--------------------Unload old scenes---------------------\\
			_ui.SetLoadName("Unload old scenes");
			for (int i = 0; i < _unloadSceneIndexes?.Count; i++)
			{
				int index = _unloadSceneIndexes[i];

				Scene scene = SceneManager.GetSceneByBuildIndex(index);

				if (scene.IsValid() && scene.isLoaded)
				{
					await SceneManager.UnloadSceneAsync(scene);
					_currentProgress += _unloadSceneTasksProgressPercent;
					UpdateProgress(_currentProgress);
				}
			}

			_currentProgress = 0.8f;
			_onProgress?.Invoke(_currentProgress);
			_ui.SetProgress(_currentProgress);

			//--------------------Load complete tasks---------------------\\
			await LoadCompleteTasksAsync();
		
			_currentProgress = 1f;
			UpdateProgress(_currentProgress);
			await Task.Delay(200);

			//--------------------Hide loading UI---------------------\\
			_ui.HideLoading();
			OnFinish();
		}
	}
}