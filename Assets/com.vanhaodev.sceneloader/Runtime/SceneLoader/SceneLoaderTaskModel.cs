using System;
using System.Threading.Tasks;

namespace vanhaodev.sceneloader
{
	public class SceneLoaderTaskModel
	{
		public string Name { get; }
		public Func<Task> Task { get; }

		public SceneLoaderTaskModel()
		{
			
		}
		public SceneLoaderTaskModel(string name, Func<Task> task)
		{
			Name = name;
			Task = task;
		}

		public virtual Task Execute()
		{
			return Task();
		}
	}
}