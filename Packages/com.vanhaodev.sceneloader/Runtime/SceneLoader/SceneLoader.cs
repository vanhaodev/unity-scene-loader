using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace vanhaodev.sceneloader
{
	public class SceneLoader : MonoBehaviour
	{
		#region Private properties

		private readonly List<Func<Task>> _onLoadStartTask = new();
		private readonly List<Func<Task>> _onLoadCompleteTask = new();
		private Action<float> _onProgress;
		private int _sceneIndex = -1;
		private List<int> _unloadSceneIndexes;
		private float _startTasksProgressPercent;
		private float _unloadSceneTasksProgressPercent;
		private float _completeTasksProgressPercent;

		[SerializeField] private SceneLoaderUI _ui;

		#endregion

		#region Public methods

		public SceneLoader RegisterOnLoadStartTask(Func<Task> onLoadStartTask)
		{
			_onLoadStartTask.Add(onLoadStartTask);
			return this;
		}

		public SceneLoader RegisterOnLoadCompleteTask(Func<Task> onLoadCompleteTask)
		{
			_onLoadCompleteTask.Add(onLoadCompleteTask);
			return this;
		}

		public void OnProgress(Action<float> callback)
		{
			_onProgress += callback;
		}

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

		public Task LoadScene(string sceneName)
		{
			_sceneIndex = GetSceneIndexByName(sceneName);
			return HandleLoadSceneAsync();
		}

		public Task LoadScene(int sceneIndex)
		{
			_sceneIndex = sceneIndex;
			return HandleLoadSceneAsync();
		}

		#endregion

		#region Private methods

		private int GetSceneIndexByName(string sceneName)
		{
			int sceneCount = SceneManager.sceneCountInBuildSettings;

			for (int i = 0; i < sceneCount; i++)
			{
				string path = SceneUtility.GetScenePathByBuildIndex(i);
				string name = System.IO.Path.GetFileNameWithoutExtension(path);

				if (name == sceneName)
					return i;
			}

			return -1;
		}

		private void CalculateProgress()
		{
			int startCount = _onLoadStartTask?.Count ?? 0;
			int unloadCount = _unloadSceneIndexes?.Count ?? 0;
			int completeCount = _onLoadCompleteTask?.Count ?? 0;

			float startTotal = 0.2f;
			float unloadTotal = 0.2f;
			float completeTotal = 0.2f;

			// Start
			_startTasksProgressPercent = startCount > 0 ? startTotal / startCount : 0f;

			// Unload scene
			_unloadSceneTasksProgressPercent = unloadCount > 0 ? unloadTotal / unloadCount : 0f;

			// Complete
			_completeTasksProgressPercent = completeCount > 0 ? completeTotal / completeCount : 0f;
		}

		private void UpdateProgress(float progress)
		{
			_onProgress?.Invoke(progress);
			_ui.SetProgress(progress);
		}

		private async Task HandleLoadSceneAsync()
		{
			//--------------------Show loading UI---------------------\\
			float currentProgress = 0f;
			UpdateProgress(currentProgress);
			CalculateProgress();
			_ui.ShowLoading();
			
			//--------------------Load start tasks---------------------\\
			for (int i = 0; i < _onLoadStartTask.Count; i++)
			{
				await _onLoadStartTask[i]();
				currentProgress += _startTasksProgressPercent;
				UpdateProgress(currentProgress);
			}

			//--------------------Load new scene---------------------\\
			AsyncOperation op = SceneManager.LoadSceneAsync(_sceneIndex, LoadSceneMode.Additive);
			op.allowSceneActivation = false;
			while (op.progress < 0.9f)
			{
				float sceneProgress = op.progress / 0.9f;
				float totalProgress = 0.2f + sceneProgress * 0.4f;

				currentProgress = totalProgress; // 🔥 sync

				UpdateProgress(currentProgress);

				await Task.Yield();
			}
			op.allowSceneActivation = true;
			while (!op.isDone)
			{
				await Task.Yield();
			}
			currentProgress = 0.6f;
			UpdateProgress(currentProgress);
			Scene newScene = SceneManager.GetSceneByBuildIndex(_sceneIndex);
			SceneManager.SetActiveScene(newScene);

			//--------------------Unload old scenes---------------------\\
			for (int i = 0; i < _unloadSceneIndexes?.Count; i++)
			{
				int index = _unloadSceneIndexes[i];

				Scene scene = SceneManager.GetSceneByBuildIndex(index);

				if (scene.IsValid() && scene.isLoaded)
				{
					await SceneManager.UnloadSceneAsync(scene);
					currentProgress += _unloadSceneTasksProgressPercent;
					UpdateProgress(currentProgress);
				}
			}
			
			currentProgress = 0.8f;
			_onProgress?.Invoke(currentProgress);
			_ui.SetProgress(currentProgress);
		
			//--------------------Load complete tasks---------------------\\
			Debug.Log("Loading complete tasks");
			for (int i = 0; i < _onLoadCompleteTask.Count; i++)
			{
				await _onLoadCompleteTask[i]();
				currentProgress += _completeTasksProgressPercent;
				UpdateProgress(currentProgress);
			}
			
			currentProgress = 1f;
			UpdateProgress(currentProgress);
			await Task.Delay(200);
		
			//--------------------Hide loading UI---------------------\\
			_ui.HideLoading();
			OnFinish();
		}

		private void OnFinish()
		{
			_onLoadStartTask?.Clear();
			_onLoadCompleteTask?.Clear();
			_unloadSceneIndexes?.Clear();
		}

		#endregion
	}
}