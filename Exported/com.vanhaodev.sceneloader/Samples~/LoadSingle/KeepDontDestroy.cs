using UnityEngine;

namespace vanhaodev.sceneloader.samples.LoadSingle
{
	public class KeepDontDestroy : MonoBehaviour
	{
		private static KeepDontDestroy _instance;

		private void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(gameObject);
				return;
			}

			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
}