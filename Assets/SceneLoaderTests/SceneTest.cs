using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using vanhaodev.sceneloader;

public class SceneTest : MonoBehaviour
{
    [SerializeField] private int _sceneIndex;

    public async void Load()
    {
        var all = FindObjectsByType<SceneLoader>(FindObjectsSortMode.None);
        Debug.Log($"Found: {string.Join("", all.Select(x => x.name).ToArray())}");
        var sceneLoader = all[0];
        sceneLoader
            .RegisterOnLoadStartTask(() => Task.Delay(Random.Range(200, 1000)))
            .RegisterOnLoadStartTask(() => Task.Delay(Random.Range(200, 1000)))
            .RegisterOnLoadStartTask(() => Task.Delay(Random.Range(200, 1000)));
        sceneLoader
            .RegisterOnLoadCompleteTask(() => Task.Delay(Random.Range(200, 1000)))
            .RegisterOnLoadCompleteTask(() => Task.Delay(Random.Range(200, 1000)))
            .RegisterOnLoadCompleteTask(() => Task.Delay(Random.Range(200, 1000)));
        await sceneLoader.LoadScene(_sceneIndex);
    }
}
