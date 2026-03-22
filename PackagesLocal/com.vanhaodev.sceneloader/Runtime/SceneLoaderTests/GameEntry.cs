using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace vanhaodev.sceneloader.tests
{
	public class GameEntry : MonoBehaviour
	{
		[SerializeField] private int _homeSceneIndex;

		private async void Start()
		{
			var sceneLoader = FindFirstObjectByType<SceneLoader>();
			sceneLoader
				.RegisterOnLoadStartTask(() => Task.Delay(Random.Range(200, 1000)))
				.RegisterOnLoadStartTask(() => Task.Delay(Random.Range(200, 1000)));
			sceneLoader
				.RegisterOnLoadCompleteTask(() => Task.Delay(Random.Range(200, 1000)))
				.RegisterOnLoadCompleteTask(() => Task.Delay(Random.Range(200, 1000)));
			await sceneLoader.LoadScene(_homeSceneIndex);
			Debug.Log($"Scene Loaded {_homeSceneIndex}");
		}
	}
}