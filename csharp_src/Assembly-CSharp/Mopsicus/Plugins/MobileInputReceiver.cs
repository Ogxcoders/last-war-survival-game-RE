using NiceJson;
using UnityEngine;

namespace Mopsicus.Plugins;

public abstract class MobileInputReceiver : MonoBehaviour
{
	public delegate void ShowDelegate(int mobilId, bool isShow, int height, int leftOffset, int rightOffset);

	protected int _id;

	public ShowDelegate OnShowKeyboard;

	protected virtual void Start()
	{
	}

	protected void InitId()
	{
		_id = MobileInput.Register(this);
	}

	protected virtual void OnDestroy()
	{
		MobileInput.RemoveReceiver(_id);
	}

	protected void Execute(JsonObject data)
	{
		MobileInput.Execute(_id, data);
	}

	public abstract void Send(JsonObject data);

	public abstract void Hide();
}
