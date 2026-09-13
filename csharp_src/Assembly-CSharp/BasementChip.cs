using LuaScriptInterface;
using Sfs2X.Entities.Data;
using UnityEngine;

public class BasementChip : CityBuilding
{
	public enum state
	{
		None,
		Init,
		Growth,
		Mature
	}

	[SerializeField]
	private GameObject _productEffect;

	[SerializeField]
	private GameObject clickEffect;

	private long startTime;

	private long endTime;

	private long centerTime;

	private WorldResourceItemBase resItemObj;

	private state curState;

	private InstanceRequest tempInstance;

	private bool _isShow = true;

	private bool isDark;

	private bool _isMine;

	private bool isDestroy;

	protected internal override void CSInit(object userData)
	{
		base.CSInit(userData);
		startTime = 0L;
		endTime = 0L;
		centerTime = 0L;
		curState = state.None;
		isDark = false;
		clickEffect.gameObject.SetActive(value: false);
		_isMine = IsSelf();
		CheckShowFarmState(null);
		if (_isMine)
		{
			GameEntry.Event.Subscribe(EventId.BuildResourcesStart, ShowPlantState);
			GameEntry.Event.Subscribe(EventId.AddSpeedSuccess, ShowPlantState);
			GameEntry.Event.Subscribe(EventId.ShowCapacity, OnGather);
			GameEntry.Event.Subscribe(EventId.ClickFarmBuildShowEffect, ShowClickEffect);
			GameEntry.Event.Subscribe(EventId.ClickFarmBuildHideEffect, HideClickEffect);
			GameEntry.Event.Subscribe(EventId.FarmGuideFakePlant, FarmGuideFakePlantSignal);
			GameEntry.Event.Subscribe(EventId.FarmGuideFakePlantShowState, FarmGuideFakePlantShowStateSignal);
		}
		GameEntry.Event.Subscribe(EventId.ChangeShowFarmState, CheckShowFarmState);
	}

	protected internal override void CSUninit()
	{
		if (_isMine)
		{
			GameEntry.Event.Unsubscribe(EventId.BuildResourcesStart, ShowPlantState);
			GameEntry.Event.Unsubscribe(EventId.AddSpeedSuccess, ShowPlantState);
			GameEntry.Event.Unsubscribe(EventId.ShowCapacity, OnGather);
			GameEntry.Event.Unsubscribe(EventId.ClickFarmBuildShowEffect, ShowClickEffect);
			GameEntry.Event.Unsubscribe(EventId.ClickFarmBuildHideEffect, HideClickEffect);
			GameEntry.Event.Unsubscribe(EventId.FarmGuideFakePlant, FarmGuideFakePlantSignal);
			GameEntry.Event.Unsubscribe(EventId.FarmGuideFakePlantShowState, FarmGuideFakePlantShowStateSignal);
		}
		GameEntry.Event.Unsubscribe(EventId.ChangeShowFarmState, CheckShowFarmState);
		if (resItemObj != null)
		{
			resItemObj.UnInit();
		}
		curState = state.None;
		if (tempInstance != null)
		{
			tempInstance.Destroy();
			tempInstance = null;
		}
		base.CSUninit();
	}

	protected internal void Update()
	{
		if (isDestroy)
		{
			return;
		}
		if (resItemObj != null)
		{
			resItemObj.OnUpdateTime();
		}
		if (CheckStateChange())
		{
			if (_isMine)
			{
				ShowPlantState(null);
			}
			else
			{
				RefreshOtherPlayer();
			}
		}
	}

	public void ShowPlantState(object userData)
	{
		if (userData != null && (long)userData != base.Uuid)
		{
			return;
		}
		QueueData queueByBuildUuidForFarm = GameEntry.Lua.QueueDataManager.GetQueueByBuildUuidForFarm(base.Uuid);
		if (queueByBuildUuidForFarm == null)
		{
			return;
		}
		if (queueByBuildUuidForFarm.GetQueueState() == 3)
		{
			startTime = 0L;
			endTime = 0L;
			centerTime = 0L;
			if (curState == state.Mature)
			{
				return;
			}
			if (resItemObj != null)
			{
				resItemObj.UnInit();
			}
			string str = GetModelNameByProductId(queueByBuildUuidForFarm.itemId.ToInt());
			if (str.IsNullOrEmpty() || tempInstance != null)
			{
				return;
			}
			string prefabPath = "Assets/Main/Prefabs/World/" + str + "Mature.prefab";
			tempInstance = GameEntry.Resource.InstantiateAsync(prefabPath);
			tempInstance.completed += delegate
			{
				if (!(tempInstance.gameObject == null))
				{
					curState = state.Mature;
					GameObject gameObject = tempInstance.gameObject;
					gameObject.transform.SetParent(buildModel.transform);
					gameObject.transform.localPosition = Vector3.zero;
					resItemObj = gameObject.GetComponent<PotatoesMature>();
					resItemObj.Init(base.Uuid, str);
					tempInstance = null;
					resItemObj.gameObject.SetActive(_isShow);
				}
			};
		}
		else if (queueByBuildUuidForFarm.GetQueueState() == 2)
		{
			startTime = queueByBuildUuidForFarm.startTime;
			endTime = queueByBuildUuidForFarm.endTime;
			centerTime = (startTime + endTime) / 2;
			long serverTime = GameEntry.Timer.GetServerTime();
			if (serverTime <= centerTime && startTime <= centerTime)
			{
				if (curState == state.Init)
				{
					return;
				}
				if (resItemObj != null)
				{
					resItemObj.UnInit();
				}
				string str2 = GetModelNameByProductId(queueByBuildUuidForFarm.itemId.ToInt());
				if (str2.IsNullOrEmpty())
				{
					return;
				}
				string prefabPath2 = "Assets/Main/Prefabs/World/" + str2 + "Init.prefab";
				if (tempInstance != null)
				{
					return;
				}
				tempInstance = GameEntry.Resource.InstantiateAsync(prefabPath2);
				tempInstance.completed += delegate
				{
					if (!(tempInstance.gameObject == null))
					{
						curState = state.Init;
						GameObject gameObject = tempInstance.gameObject;
						gameObject.transform.SetParent(buildModel.transform);
						gameObject.transform.localPosition = Vector3.zero;
						resItemObj = gameObject.GetComponent<PotatoesInit>();
						resItemObj.Init(base.Uuid, str2);
						resItemObj.gameObject.SetActive(_isShow);
						tempInstance = null;
					}
				};
			}
			else
			{
				if (serverTime <= centerTime || endTime < centerTime || curState == state.Growth)
				{
					return;
				}
				if (resItemObj != null)
				{
					resItemObj.UnInit();
				}
				string str3 = GetModelNameByProductId(queueByBuildUuidForFarm.itemId.ToInt());
				if (str3.IsNullOrEmpty() || tempInstance != null)
				{
					return;
				}
				string prefabPath3 = "Assets/Main/Prefabs/World/" + str3 + "Growth.prefab";
				tempInstance = GameEntry.Resource.InstantiateAsync(prefabPath3);
				tempInstance.completed += delegate
				{
					if (!(tempInstance.gameObject == null))
					{
						curState = state.Growth;
						GameObject gameObject = tempInstance.gameObject;
						gameObject.transform.SetParent(buildModel.transform);
						gameObject.transform.localPosition = Vector3.zero;
						resItemObj = gameObject.GetComponent<PotatoesGrowth>();
						resItemObj.Init(base.Uuid, str3);
						resItemObj.gameObject.SetActive(_isShow);
						tempInstance = null;
					}
				};
			}
		}
		else if (queueByBuildUuidForFarm.GetQueueState() == 0)
		{
			startTime = 0L;
			endTime = 0L;
			centerTime = 0L;
			if (resItemObj != null)
			{
				resItemObj.UnInit();
			}
		}
	}

	public bool CheckStateChange()
	{
		bool result = false;
		long serverTime = GameEntry.Timer.GetServerTime();
		if (curState == state.Init)
		{
			if (serverTime > centerTime)
			{
				result = true;
			}
		}
		else if (curState == state.Growth && (serverTime <= centerTime || serverTime > endTime))
		{
			result = true;
		}
		return result;
	}

	public void OnGather(object userData)
	{
		if (userData == null || !(userData is string text))
		{
			return;
		}
		string[] array = text.Split(new char[1] { ';' });
		if (array.Length == 0 || array[0].ToLong() != base.Uuid)
		{
			return;
		}
		if (curState == state.Mature && resItemObj != null)
		{
			PotatoesMature potatoesMature = resItemObj as PotatoesMature;
			if (potatoesMature != null)
			{
				potatoesMature.OnFinish();
			}
			else if (resItemObj != null)
			{
				resItemObj.UnInit();
			}
		}
		else if (resItemObj != null)
		{
			resItemObj.UnInit();
		}
		curState = state.None;
	}

	private string GetModelNameByProductId(int productId)
	{
		string templateData = GameEntry.ConfigCache.GetTemplateData("aps_farming", productId, "model_name");
		if (!templateData.IsNullOrEmpty())
		{
			return templateData;
		}
		return "";
	}

	public void ShowClickEffect(object userData)
	{
		if (!(userData is string str))
		{
			return;
		}
		if (str.ToLong() == base.Uuid)
		{
			if (!isDark)
			{
				clickEffect.gameObject.SetActive(value: true);
				isDark = true;
			}
		}
		else
		{
			HideClickEffect(null);
		}
	}

	public void HideClickEffect(object userData)
	{
		if ((!(userData is string str) || str.ToLong() == base.Uuid) && isDark)
		{
			clickEffect.gameObject.SetActive(value: false);
			isDark = false;
		}
	}

	private void FarmGuideFakePlantSignal(object userData)
	{
		if (!(userData is SFSObject sFSObject) || !sFSObject.ContainsKey("bUuid") || !sFSObject.ContainsKey("itemId") || sFSObject.GetLong("bUuid") != base.Uuid)
		{
			return;
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		startTime = serverTime;
		endTime = long.MaxValue;
		centerTime = endTime;
		if (resItemObj != null)
		{
			resItemObj.UnInit();
		}
		curState = state.Init;
		string str = GetModelNameByProductId(sFSObject.GetInt("itemId"));
		if (str.IsNullOrEmpty())
		{
			return;
		}
		string prefabPath = "Assets/Main/Prefabs/World/" + str + "Init.prefab";
		if (tempInstance != null)
		{
			return;
		}
		tempInstance = GameEntry.Resource.InstantiateAsync(prefabPath);
		tempInstance.completed += delegate
		{
			if (!(tempInstance.gameObject == null))
			{
				GameObject gameObject = tempInstance.gameObject;
				gameObject.transform.SetParent(buildModel.transform);
				gameObject.transform.localPosition = Vector3.zero;
				resItemObj = gameObject.GetComponent<PotatoesInit>();
				resItemObj.Init(base.Uuid, str);
				resItemObj.gameObject.SetActive(_isShow);
				tempInstance = null;
			}
		};
	}

	private void FarmGuideFakePlantShowStateSignal(object userData)
	{
		bool flag = (bool)userData;
		_isShow = flag;
		if (resItemObj != null)
		{
			resItemObj.gameObject.SetActive(flag);
		}
	}

	private void CheckShowFarmState(object userData)
	{
		if (CommonUtils.IsDebug())
		{
			_isShow = GameEntry.Setting.GetPrivateBool("SHOW_FARM", defaultValue: true);
		}
		else
		{
			_isShow = true;
		}
		if (resItemObj != null)
		{
			resItemObj.gameObject.SetActive(_isShow);
		}
	}

	public override void refeshDate()
	{
		base.refeshDate();
		BuildPointInfo buildInfo = GetBuildInfo();
		if (buildInfo != null && buildInfo.destroyStartTime > 0)
		{
			isDestroy = true;
			if (resItemObj != null)
			{
				resItemObj.UnInit();
			}
			curState = state.None;
			if (tempInstance != null)
			{
				tempInstance.Destroy();
				tempInstance = null;
			}
		}
		else
		{
			isDestroy = false;
			_isMine = IsSelf();
			if (!_isMine)
			{
				RefreshOtherPlayer();
			}
			else
			{
				ShowPlantState(null);
			}
		}
	}

	private void RefreshOtherPlayer()
	{
		BuildPointInfo buildInfo = GetBuildInfo();
		if (buildInfo != null && buildInfo.GetShowState() == QueueState.FARMING)
		{
			startTime = buildInfo.queueUpdateTime;
			endTime = buildInfo.queueStartTime;
			if (startTime <= 0 || endTime <= 0)
			{
				startTime = 0L;
				endTime = 0L;
				centerTime = 0L;
				if (resItemObj != null)
				{
					resItemObj.UnInit();
				}
				return;
			}
			centerTime = (startTime + endTime) / 2;
			long serverTime = GameEntry.Timer.GetServerTime();
			if (serverTime <= centerTime && startTime <= centerTime)
			{
				if (curState == state.Init)
				{
					return;
				}
				if (resItemObj != null)
				{
					resItemObj.UnInit();
				}
				string str = GetModelNameByProductId(buildInfo.queueItemId);
				if (str.IsNullOrEmpty())
				{
					return;
				}
				string prefabPath = "Assets/Main/Prefabs/World/" + str + "Init.prefab";
				if (tempInstance != null)
				{
					return;
				}
				tempInstance = GameEntry.Resource.InstantiateAsync(prefabPath);
				tempInstance.completed += delegate
				{
					if (!(tempInstance.gameObject == null))
					{
						curState = state.Init;
						GameObject gameObject = tempInstance.gameObject;
						gameObject.transform.SetParent(buildModel.transform);
						gameObject.transform.localPosition = Vector3.zero;
						resItemObj = gameObject.GetComponent<PotatoesInit>();
						resItemObj.Init(base.Uuid, str);
						resItemObj.gameObject.SetActive(_isShow);
						tempInstance = null;
					}
				};
			}
			else if (serverTime > centerTime && endTime >= serverTime)
			{
				if (curState == state.Growth)
				{
					return;
				}
				if (resItemObj != null)
				{
					resItemObj.UnInit();
				}
				string str2 = GetModelNameByProductId(buildInfo.queueItemId);
				if (str2.IsNullOrEmpty() || tempInstance != null)
				{
					return;
				}
				string prefabPath2 = "Assets/Main/Prefabs/World/" + str2 + "Growth.prefab";
				tempInstance = GameEntry.Resource.InstantiateAsync(prefabPath2);
				tempInstance.completed += delegate
				{
					if (!(tempInstance.gameObject == null))
					{
						curState = state.Growth;
						GameObject gameObject = tempInstance.gameObject;
						gameObject.transform.SetParent(buildModel.transform);
						gameObject.transform.localPosition = Vector3.zero;
						resItemObj = gameObject.GetComponent<PotatoesGrowth>();
						resItemObj.Init(base.Uuid, str2);
						resItemObj.gameObject.SetActive(_isShow);
						tempInstance = null;
					}
				};
			}
			else
			{
				if (serverTime <= endTime || curState == state.Mature)
				{
					return;
				}
				if (resItemObj != null)
				{
					resItemObj.UnInit();
				}
				string str3 = GetModelNameByProductId(buildInfo.queueItemId);
				if (str3.IsNullOrEmpty() || tempInstance != null)
				{
					return;
				}
				string prefabPath3 = "Assets/Main/Prefabs/World/" + str3 + "Mature.prefab";
				tempInstance = GameEntry.Resource.InstantiateAsync(prefabPath3);
				tempInstance.completed += delegate
				{
					if (!(tempInstance.gameObject == null))
					{
						curState = state.Mature;
						GameObject gameObject = tempInstance.gameObject;
						gameObject.transform.SetParent(buildModel.transform);
						gameObject.transform.localPosition = Vector3.zero;
						resItemObj = gameObject.GetComponent<PotatoesMature>();
						resItemObj.Init(base.Uuid, str3);
						tempInstance = null;
						resItemObj.gameObject.SetActive(_isShow);
					}
				};
			}
		}
		else
		{
			startTime = 0L;
			endTime = 0L;
			centerTime = 0L;
			if (resItemObj != null)
			{
				resItemObj.UnInit();
			}
		}
	}
}
