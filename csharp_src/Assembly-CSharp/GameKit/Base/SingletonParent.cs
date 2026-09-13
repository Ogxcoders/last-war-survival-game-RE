using UnityEngine;

namespace GameKit.Base;

public class SingletonParent : SingletonBehaviour<SingletonParent>
{
	private void OnApplicationQuit()
	{
		base.transform.BroadcastMessage("Release", SendMessageOptions.DontRequireReceiver);
	}
}
