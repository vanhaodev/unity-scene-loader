using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace vanhaodev.sceneloader
{
	public partial class SceneLoader
	{
		private async Task HandleLoadSceneSingleAsync()
		{
			//--------------------Show loading UI---------------------\\
			_currentProgress = 0f;
			UpdateProgress(_currentProgress);
			CalculateProgress();
			_ui.ShowLoading();
	
			//--------------------Load start tasks---------------------\\
			await LoadStartTasksAsync();

			//--------------------Load new scene (Single)---------------------\\
			_ui.SetLoadName("Load scene");

			AsyncOperation op = SceneManager.LoadSceneAsync(_sceneIndex, LoadSceneMode.Single);
			op.allowSceneActivation = false;

			while (op.progress < 0.9f)
			{
				float sceneProgress = op.progress / 0.9f;

				// ví dụ: scene chiếm 60% tổng progress (0.2 -> 0.8)
				float totalProgress = 0.2f + sceneProgress * 0.6f;

				_currentProgress = totalProgress;
				UpdateProgress(_currentProgress);

				await Task.Yield();
			}

			// cho activate (lúc này Unity sẽ tự unload scene cũ)
			op.allowSceneActivation = true;

			while (!op.isDone)
			{
				await Task.Yield();
			}

			_currentProgress = 0.8f;
			UpdateProgress(_currentProgress);

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