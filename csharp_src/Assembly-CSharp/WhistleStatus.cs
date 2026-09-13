using Protobuf;
using UnityEngine;

public class WhistleStatus : StatusStateBase
{
	private Transform _root;

	private InstanceRequest req;

	private UIWhistleBuffLabel uiWhistleLabel;

	private const string PREFAB_PATH = "Assets/Main/SeasonRes/S4/Prefabs/World/WhistleBuffLabel.prefab";

	private const string WHISTLE_VFX = "Assets/Main/SeasonRes/S4/Prefabs/World/VFX_build_whistle.prefab";

	public WhistleStatus(Status status, Transform modelGo)
		: base(status.Id, status.BeginTime, status.ExpireTime)
	{
		_root = modelGo;
	}

	public override void Start()
	{
		if (_root == null)
		{
			return;
		}
		req = GameEntry.Resource.InstantiateAsync("Assets/Main/SeasonRes/S4/Prefabs/World/WhistleBuffLabel.prefab");
		req.completed += delegate
		{
			GameObject gameObject = req.gameObject;
			if (!(_root == null) && !(gameObject == null))
			{
				gameObject.transform.SetParent(_root);
				gameObject.transform.localPosition = new Vector3(0f, 0f, -3f);
				gameObject.transform.localScale = Vector3.one;
				uiWhistleLabel = gameObject.GetComponent<UIWhistleBuffLabel>();
				uiWhistleLabel.SetData(startTime, endTime);
			}
		};
		if (GameEntry.Timer.GetServerTime() < startTime + 1)
		{
			SceneManager.World.CreateVFX("Assets/Main/SeasonRes/S4/Prefabs/World/VFX_build_whistle.prefab", _root.position, 6f);
		}
	}

	public override StateType Update(float deltaTime)
	{
		if (GameEntry.Timer.GetServerTime() < endTime)
		{
			return StateType.Continue;
		}
		return StateType.Finish;
	}

	public override void Dispose()
	{
		base.Dispose();
		if (req != null)
		{
			req.Destroy();
		}
		req = null;
		uiWhistleLabel = null;
		_root = null;
		statusId = -1;
	}
}
