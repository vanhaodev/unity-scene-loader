using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using vanhaodev.sceneloader;
using Random = UnityEngine.Random;

namespace vanhaodev.sceneloader.samples.loadadditive
{
	public class AdditiveGameEntry : MonoBehaviour
	{
		[SerializeField] private int _homeSceneIndex;

		private async void Start()
		{
			var sceneLoader = FindFirstObjectByType<SceneLoader>();
			sceneLoader
				.RegisterOnLoadStartTask(new SceneLoaderTaskModel("Load item", () => Task.Delay(Random.Range(200, 1000))))
				.RegisterOnLoadStartTask(new SceneLoaderTaskModel("Load character", () => Task.Delay(Random.Range(200, 1000))))
				.RegisterOnLoadStartTask(new SceneLoaderTaskModel("Load map", () => Task.Delay(Random.Range(200, 1000))))
				.RegisterOnLoadStartTask(new SceneLoaderTaskModel("Load monsters", () => Task.Delay(Random.Range(200, 1000))));
			sceneLoader
				.RegisterOnLoadStartTask(new SceneLoaderTaskModel("Load item", () => Task.Delay(Random.Range(200, 1000))))
				.RegisterOnLoadStartTask(new SceneLoaderTaskModel("Load character", () => Task.Delay(Random.Range(200, 1000))))
				.RegisterOnLoadStartTask(new SceneLoaderTaskModel("Load map", () => Task.Delay(Random.Range(200, 1000))))
				.RegisterOnLoadStartTask(new SceneLoaderTaskModel("Load monsters", () => Task.Delay(Random.Range(200, 1000))));
			await sceneLoader.LoadScene(_homeSceneIndex, LoadSceneMode.Additive);
			Debug.Log($"Scene Loaded {_homeSceneIndex}");
		}
	}
}