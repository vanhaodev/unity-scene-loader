using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace vanhaodev.sceneloader
{
	public partial class SceneLoader : MonoBehaviour
	{
		#region Private properties
		private LoadSceneMode _loadSceneMode;	
		private readonly List<SceneLoaderTaskModel> _onLoadStartTask = new();
		private readonly List<SceneLoaderTaskModel> _onLoadCompleteTask = new();
		private Action<float> _onProgress;
		private int _sceneIndex = -1;
		private List<int> _unloadSceneIndexes;
		private float _startTasksProgressPercent;
		private float _unloadSceneTasksProgressPercent;
		private float _completeTasksProgressPercent;
		private float _currentProgress = 0f;
		
		[SerializeField] private SceneLoaderUI _ui;

		#endregion

		#region Public methods

		public SceneLoader RegisterOnLoadStartTask(SceneLoaderTaskModel onLoadStartTask)
		{
			_onLoadStartTask.Add(onLoadStartTask);
			return this;
		}

		public SceneLoader RegisterOnLoadCompleteTask(SceneLoaderTaskModel onLoadCompleteTask)
		{
			_onLoadCompleteTask.Add(onLoadCompleteTask);
			return this;
		}

		public void OnProgress(Action<float> callback)
		{
			_onProgress += callback;
		}

		public Task LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
		{
			_sceneIndex = GetSceneIndexByName(sceneName);
			_loadSceneMode = mode;
			if (_loadSceneMode == LoadSceneMode.Single)
			{
				return HandleLoadSceneSingleAsync();
			}
			else
			{
				return HandleLoadSceneAdditiveAsync();
			}
		}

		public Task LoadScene(int sceneIndex, LoadSceneMode mode = LoadSceneMode.Single)
		{
			_sceneIndex = sceneIndex;
			_loadSceneMode = mode;
			if (_loadSceneMode == LoadSceneMode.Single)
			{
				return HandleLoadSceneSingleAsync();
			}
			else
			{
				return HandleLoadSceneAdditiveAsync();
			}
		}

		#endregion

		#region Private methods

		private async Task LoadStartTasksAsync()
		{
			for (int i = 0; i < _onLoadStartTask.Count; i++)
			{
				_ui.SetLoadName(_onLoadStartTask[i].Name);
				await _onLoadStartTask[i].Execute();
				_currentProgress += _startTasksProgressPercent;
				UpdateProgress(_currentProgress);
			}
		}
		private async Task LoadCompleteTasksAsync()
		{
			for (int i = 0; i < _onLoadCompleteTask.Count; i++)
			{
				_ui.SetLoadName(_onLoadCompleteTask[i].Name);
				await _onLoadCompleteTask[i].Execute();
				_currentProgress += _completeTasksProgressPercent;
				UpdateProgress(_currentProgress);
			}
		}
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

		private void OnFinish()
		{
			_onLoadStartTask?.Clear();
			_onLoadCompleteTask?.Clear();
			_unloadSceneIndexes?.Clear();
			_ui.SetLoadName("");
		}

		#endregion
	}
}