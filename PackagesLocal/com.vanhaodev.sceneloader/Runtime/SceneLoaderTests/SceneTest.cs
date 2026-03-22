using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using vanhaodev.sceneloader;

namespace vanhaodev.sceneloader.tests
{
    public class SceneTest : MonoBehaviour
    {
        [SerializeField] private int _sceneIndex;

        public async void Load()
        {
            var all = FindObjectsByType<SceneLoader>(FindObjectsSortMode.None);
            var sceneLoader = all[0];
            Debug.Log($"Found: {string.Join("", all.Select(x => x.name).ToArray())}\n{sceneLoader != null}");
            sceneLoader
                .RegisterOnLoadStartTask(() => Task.Delay(Random.Range(200, 1000)))
                .RegisterOnLoadStartTask(() => Task.Delay(Random.Range(200, 1000)))
                .RegisterOnLoadStartTask(() => Task.Delay(Random.Range(200, 1000)));
            sceneLoader
                .RegisterOnLoadCompleteTask(() => Task.Delay(Random.Range(200, 1000)))
                .RegisterOnLoadCompleteTask(() => Task.Delay(Random.Range(200, 1000)))
                .RegisterOnLoadCompleteTask(() => Task.Delay(Random.Range(200, 1000)));
            sceneLoader.SetUnloadScenes(new List<int>()
            {
                //unload others
                _sceneIndex == 1 ? 2 : 1
            });
            await sceneLoader.LoadScene(_sceneIndex);
        }
    }
}