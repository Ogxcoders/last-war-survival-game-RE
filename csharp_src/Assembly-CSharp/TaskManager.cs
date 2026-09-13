using System.Collections;
using UnityEngine;

internal class TaskManager : MonoBehaviour
{
	public class TaskState
	{
		public delegate void FinishedHandler(bool manual);

		private IEnumerator coroutine;

		private bool running;

		private bool paused;

		private bool stopped;

		public bool Running => running;

		public bool Paused => paused;

		public event FinishedHandler Finished;

		public TaskState(IEnumerator c)
		{
			coroutine = c;
		}

		public void Pause()
		{
			paused = true;
		}

		public void Unpause()
		{
			paused = false;
		}

		public void Start()
		{
			running = true;
			singleton.StartCoroutine(CallWrapper());
		}

		public void Stop()
		{
			stopped = true;
			running = false;
		}

		private IEnumerator CallWrapper()
		{
			yield return null;
			IEnumerator e = coroutine;
			while (running)
			{
				if (paused)
				{
					yield return null;
				}
				else if (e != null && e.MoveNext())
				{
					yield return e.Current;
				}
				else
				{
					running = false;
				}
			}
			this.Finished?.Invoke(stopped);
		}
	}

	private static TaskManager singleton;

	public static TaskState CreateTask(IEnumerator coroutine)
	{
		if (singleton == null)
		{
			GameObject gameObject = new GameObject("TaskManager");
			singleton = gameObject.AddComponent<TaskManager>();
			if (GameEntry.GameBase != null)
			{
				gameObject.transform.SetParent(GameEntry.GameBase.transform);
			}
		}
		return new TaskState(coroutine);
	}
}
