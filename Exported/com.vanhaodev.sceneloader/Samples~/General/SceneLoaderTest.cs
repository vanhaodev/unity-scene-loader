using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using vanhaodev.sceneloader;

namespace vanhaodev.sceneloader.samples.general
{
    public class AdditiveSceneTest : MonoBehaviour
    {
        [SerializeField] private int _loadToSceneIndex;
        
        public async void LoadSingle()
        {
            var sceneLoader = FindAnyObjectByType<SceneLoader>();
            
            sceneLoader
                .RegisterOnLoadStartTask(new SceneLoaderTaskModel("Load item", () => Task.Delay(Random.Range(200, 1000))))
                .RegisterOnLoadStartTask(new SceneLoaderTaskModel("Load character", () => Task.Delay(Random.Range(200, 1000))))
                .RegisterOnLoadStartTask(new SceneLoaderTaskModel("Load map", () => Task.Delay(Random.Range(200, 1000))))
                .RegisterOnLoadStartTask(new SceneLoaderTaskModel("Load monsters", () => Task.Delay(Random.Range(200, 1000))));
            sceneLoader
                .RegisterOnLoadCompleteTask(new SceneLoaderTaskModel("Load item", () => Task.Delay(Random.Range(200, 1000))))
                .RegisterOnLoadCompleteTask(new SceneLoaderTaskModel("Load character", () => Task.Delay(Random.Range(200, 1000))))
                .RegisterOnLoadCompleteTask(new SceneLoaderTaskModel("Load map", () => Task.Delay(Random.Range(200, 1000))))
                .RegisterOnLoadCompleteTask(new SceneLoaderTaskModel("Load monsters", () => Task.Delay(Random.Range(200, 1000))));
            
            await sceneLoader.LoadScene(_loadToSceneIndex);
        }
        
        public async void LoadAdditive()
        {
            var sceneLoader = FindAnyObjectByType<SceneLoader>();
            
            sceneLoader
                .RegisterOnLoadStartTask(new SceneLoaderTaskModel("Load item", () => Task.Delay(Random.Range(200, 1000))))
                .RegisterOnLoadStartTask(new SceneLoaderTaskModel("Load character", () => Task.Delay(Random.Range(200, 1000))))
                .RegisterOnLoadStartTask(new SceneLoaderTaskModel("Load map", () => Task.Delay(Random.Range(200, 1000))))
                .RegisterOnLoadStartTask(new SceneLoaderTaskModel("Load monsters", () => Task.Delay(Random.Range(200, 1000))));
            sceneLoader
                .RegisterOnLoadCompleteTask(new SceneLoaderTaskModel("Load item", () => Task.Delay(Random.Range(200, 1000))))
                .RegisterOnLoadCompleteTask(new SceneLoaderTaskModel("Load character", () => Task.Delay(Random.Range(200, 1000))))
                .RegisterOnLoadCompleteTask(new SceneLoaderTaskModel("Load map", () => Task.Delay(Random.Range(200, 1000))))
                .RegisterOnLoadCompleteTask(new SceneLoaderTaskModel("Load monsters", () => Task.Delay(Random.Range(200, 1000))));
            
            sceneLoader.SetUnloadScenes(new List<int>()
            {
                //unload others
                _loadToSceneIndex == 1 ? 2 : 1
            });
            await sceneLoader.LoadScene(_loadToSceneIndex, LoadSceneMode.Additive);
        }
    }
}