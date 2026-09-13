using System.Collections.Generic;
using UnityEngine;

public class BasementGetArmyResMetal : MonoBehaviour
{
	public enum PercentState
	{
		None = -1,
		Zero = 0,
		Few = 25,
		Middle = 50,
		Max = 100
	}

	private enum RealPercent
	{
		Zero = 0,
		Few = 5,
		Middle = 25,
		Max = 100
	}

	public static Dictionary<PercentState, string> MetalRealModelPath = new Dictionary<PercentState, string>
	{
		{
			PercentState.Zero,
			null
		},
		{
			PercentState.Few,
			"Assets/Main/Prefabs/Building/BuildMetalFew.prefab"
		},
		{
			PercentState.Middle,
			"Assets/Main/Prefabs/Building/BuildMetalMiddle.prefab"
		},
		{
			PercentState.Max,
			"Assets/Main/Prefabs/Building/BuildMetalMax.prefab"
		}
	};

	[SerializeField]
	protected GameObject buildModel;

	private PercentState _statePro;

	private long marchUuid;

	private InstanceRequest _instanceRequest;

	public void Init(long Uuid)
	{
		marchUuid = Uuid;
		Refresh(isEvent: false);
		GameEntry.Event.Subscribe(EventId.WorldArmyCollectAnimEnd, CollectAnimEndSignal);
	}

	public void UnInit()
	{
		DeleteModel();
		GameEntry.Event.Unsubscribe(EventId.WorldArmyCollectAnimEnd, CollectAnimEndSignal);
	}

	private void CollectAnimEndSignal(object userData)
	{
		if ((long)userData == marchUuid)
		{
			Refresh(isEvent: true);
		}
	}

	private void Refresh(bool isEvent)
	{
		if (_instanceRequest != null && _instanceRequest.gameObject != null)
		{
			_instanceRequest.gameObject.SetActive(value: true);
		}
		WorldMarch march = SceneManager.World.GetMarch(marchUuid);
		PercentState percentStateByState = GetPercentStateByState(isEvent, march);
		if (_statePro != percentStateByState)
		{
			_statePro = percentStateByState;
			LoadModel(GetRealModelByBuildId(percentStateByState));
		}
	}

	private PercentState GetPercentStateByState(bool isEvent, WorldMarch marchInfo)
	{
		if (marchInfo.GetResourcePercent() * 100f >= 25f)
		{
			return PercentState.Middle;
		}
		if (marchInfo.GetResourcePercent() * 100f >= 5f)
		{
			return PercentState.Few;
		}
		_ = marchInfo.GetResourcePercent() * 100f;
		_ = 5f;
		return PercentState.Zero;
	}

	private string GetRealModelByBuildId(PercentState state)
	{
		return MetalRealModelPath[state];
	}

	private void LoadModel(string modelName)
	{
		DeleteModel();
		if (!string.IsNullOrEmpty(modelName))
		{
			_instanceRequest = GameEntry.Resource.InstantiateAsync(modelName);
			_instanceRequest.completed += delegate
			{
				GameObject obj = _instanceRequest.gameObject;
				obj.transform.SetParent(buildModel.transform);
				obj.transform.localPosition = Vector3.zero;
			};
		}
	}

	private void DeleteModel()
	{
		if (_instanceRequest != null)
		{
			_instanceRequest.Destroy();
			_instanceRequest = null;
		}
	}
}
