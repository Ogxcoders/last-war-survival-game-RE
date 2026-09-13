using UnityEngine;

namespace UnityGameFramework.Runtime;

public abstract class GameFrameworkComponent : MonoBehaviour
{
	protected virtual void Awake()
	{
		GameEntry.RegisterComponent(this);
	}
}
