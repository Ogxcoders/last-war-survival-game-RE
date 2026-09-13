using System;
using UnityEngine;

public abstract class AsyncMono<T> : MonoBehaviour where T : AsyncMono<T>
{
	public class Handle
	{
		private InstanceRequest request;

		private Action callback;

		private string path;

		private Transform parent;

		private bool active = true;

		public T MonoInstance { get; private set; }

		public Transform Transform => MonoInstance?.transform;

		public static Handle Load(string path, Transform parent, Action callback)
		{
			Handle handle = new Handle();
			handle.path = path;
			handle.callback = callback;
			handle.parent = parent;
			handle.request = GameEntry.Resource.InstantiateAsync(path);
			handle.request.completed += delegate(InstanceRequest _req)
			{
				if (handle.request == null)
				{
					_req.Destroy();
				}
				else
				{
					if (_req.state == InstanceRequest.State.Instanced)
					{
						GameObject gameObject = _req.gameObject;
						if (gameObject != null)
						{
							gameObject.transform.SetParent(handle.parent, worldPositionStays: false);
							gameObject.transform.localPosition = Vector3.zero;
							handle.MonoInstance = gameObject.GetComponent<T>();
							gameObject.SetActive(handle.active);
						}
					}
					handle.callback?.Invoke();
				}
			};
			return handle;
		}

		public void Destroy()
		{
			request?.Destroy();
			request = null;
			parent = null;
			callback = null;
			MonoInstance = null;
		}

		public void SetActive(bool active)
		{
			this.active = active;
			MonoInstance?.gameObject.SetActive(active);
		}
	}
}
