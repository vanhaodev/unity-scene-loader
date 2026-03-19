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
        private List<Func<Task>> _onLoadStartTask = new();
        private List<Func<Task>> _onLoadCompleteTask = new();
        private Action<float> _onProgress;
        private int _sceneIndex = -1;
        private float _startTasksProgressPercent;
        private float _completeTasksProgressPercent;
        #endregion
        
        #region Public methods
        public void RegisterOnLoadStartTask(Func<Task> onLoadStartTask)
        {
            _onLoadStartTask.Add(onLoadStartTask);
        }

        public void RegisterOnLoadCompleteTask(Func<Task> onLoadCompleteTask)
        {
            _onLoadCompleteTask.Add(onLoadCompleteTask);
        }
        
        public void OnProgress(Action<float> callback)
        {
            _onProgress += callback;
        }
        
        public Task LoadScene(string sceneName)
        {
            _sceneIndex = SceneManager.GetActiveScene().buildIndex;
            return HandleLoadSceneAsync();
        }
        
        public Task LoadScene(int sceneIndex)
        {
            _sceneIndex = sceneIndex;
            return HandleLoadSceneAsync();
        }
        #endregion

        #region Private methods
        private void CalculateProgress()
        {
            int startCount = _onLoadStartTask?.Count ?? 0;
            int completeCount = _onLoadCompleteTask?.Count ?? 0;

            float startTotal = 0.2f;
            float completeTotal = 0.2f;

            // Start
            if (startCount < 2)
                _startTasksProgressPercent = startTotal;
            else
                _startTasksProgressPercent = startTotal / startCount;

            // Complete
            if (completeCount < 2)
                _completeTasksProgressPercent = completeTotal;
            else
                _completeTasksProgressPercent = completeTotal / completeCount;
        }
        private async Task HandleLoadSceneAsync()
        {
            float currentProgress = 0f;
            CalculateProgress();
            //call ui show
            //do _onLoadStartTask
            for (int i = 0; i < _onLoadStartTask.Count; i++)
            {
                await _onLoadStartTask[i]();
                currentProgress += _startTasksProgressPercent;
                _onProgress?.Invoke(currentProgress);
            }
            //do loading scene
            AsyncOperation op = SceneManager.LoadSceneAsync(_sceneIndex);

            while (!op.isDone)
            {
                float sceneProgress = op.progress / 0.9f;
                float totalProgress = 0.2f + sceneProgress * 0.6f;

                _onProgress?.Invoke(totalProgress);

                await Task.Yield();
            }
            
            //do loading _onLoadCompleteTask
            for (int i = 0; i < _onLoadCompleteTask.Count; i++)
            {
                await _onLoadCompleteTask[i]();
                currentProgress += _completeTasksProgressPercent;
                _onProgress?.Invoke(currentProgress);
            }
            await Task.Yield();
            OnFinish();
            //call ui hide
        }

        private void OnFinish()
        {
            _onLoadStartTask?.Clear();
            _onLoadStartTask = null;
            _onLoadCompleteTask?.Clear();
            _onLoadCompleteTask = null;
        }
        #endregion
    }
}
