using UnityEngine;

namespace Joker.Client;

public class GameTickManager : IGameManager, IStartupSync, IShutdownSync
{
	internal class GameClientBehaviour : MonoBehaviour
	{
		internal GameTickManager owner;

		private void Update()
		{
			owner.update?.Update();
		}

		private void LateUpdate()
		{
			owner.lateUpdate?.LateUpdate();
		}

		private void FixedUpdate()
		{
			owner.fixedUpdate?.FixedUpdate();
		}

		private void OnGUI()
		{
			owner.onGUI?.Display();
		}
	}

	private IUpdate update;

	private ILateUpdate lateUpdate;

	private IFixedUpdate fixedUpdate;

	private IDisplay onGUI;

	public GameTickManager(IGameClient owner)
	{
		update = owner as IUpdate;
		lateUpdate = owner as ILateUpdate;
		fixedUpdate = owner as IFixedUpdate;
		onGUI = owner as IDisplay;
	}

	public void Startup()
	{
		Runtime.Instance.gameObject.AddComponent<GameClientBehaviour>().owner = this;
	}

	public void Shutdown()
	{
		Object.Destroy(Runtime.Instance.gameObject);
	}
}
