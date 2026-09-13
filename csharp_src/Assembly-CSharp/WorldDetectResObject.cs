using System.Collections.Generic;
using UnityEngine;

public class WorldDetectResObject : WorldDetectEventItemObject
{
	private GameObject headObj;

	private SpriteRenderer headBg;

	private SpriteRenderer headIcon;

	private CircleMeshInstanced headIconNew;

	private SpriteRenderer stateIcon_Icon;

	private UIWorldLabel ModelLable;

	private SceneProductLineFlyTextAni FlyTextAni;

	private UIWorldLabel IconLable;

	private long _gatherMarchUuid;

	private float collectSpd;

	public WorldDetectResObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void AsyncCompleteCallBack(InstanceRequest instance)
	{
		if (!(world.GetPointInfo(pointIndex) is ResPointInfo resPointInfo))
		{
			return;
		}
		DestroyModel();
		ClearOldObject();
		base.gameObject = instance.gameObject;
		if (!(base.gameObject != null))
		{
			return;
		}
		base.gameObject.name = "WorldPointObject_" + pointIndex;
		base.gameObject.transform.SetParent(world.DynamicObjNode);
		base.gameObject.transform.position = base.WorldPosition;
		base.gameObject.SetActive(isVisible);
		ModelLable = base.gameObject.transform.Find("Model/MonsterLabel")?.GetComponent<UIWorldLabel>();
		FlyTextAni = base.gameObject.transform.Find("Model/WorldDetectFlyTextAni")?.GetComponent<SceneProductLineFlyTextAni>();
		IconLable = base.gameObject.transform.Find("Icon/IconLabel")?.GetComponent<UIWorldLabel>();
		SetAutoAdjustLod();
		model = base.gameObject.transform.Find("Model").gameObject;
		model.gameObject.transform.localScale = Vector3.one;
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_gather_resource", resPointInfo.id, "level");
		if (ModelLable != null)
		{
			ModelLable.gameObject.SetActive(value: true);
			ModelLable.SetLevel(templateData.ToInt());
		}
		if (IconLable != null)
		{
			IconLable.gameObject.SetActive(value: true);
			IconLable.SetLevel(templateData.ToInt());
		}
		GameObject gameObject = base.gameObject.transform.Find("Icon/troopIcon")?.gameObject;
		if (gameObject != null)
		{
			headObj = gameObject.transform.Find("head")?.gameObject;
			if (headObj != null)
			{
				headBg = headObj.transform.Find("headbg")?.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
				headIconNew = headObj.transform.Find("headIconNew")?.GetComponentInChildren<CircleMeshInstanced>(includeInactive: true);
				if (headIconNew == null)
				{
					headIcon = headObj.transform.Find("headIcon")?.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
				}
			}
			stateIcon_Icon = gameObject.transform.Find("stateIcon")?.GetComponent<SpriteRenderer>();
		}
		bool flag = false;
		string spritePath = "";
		string text = "";
		if (resPointInfo != null && resPointInfo.gatherMarchUuid != 0L)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(value: true);
			}
			WorldMarch march = world.GetMarch(resPointInfo.gatherMarchUuid);
			if (march == null || !march.IsMine())
			{
				text = ((march == null || !(march.allianceUid == GameEntry.Data.Player.GetAllianceId())) ? "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect_other" : "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect_alliance");
			}
			else
			{
				text = "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect";
				_gatherMarchUuid = resPointInfo.gatherMarchUuid;
				flag = true;
				ArmyInfo firstArmyInfo = march.GetFirstArmyInfo();
				if (firstArmyInfo != null && firstArmyInfo.HeroInfos != null && firstArmyInfo.HeroInfos.Count > 0)
				{
					HeroInfo heroInfo = firstArmyInfo.HeroInfos[0];
					if (heroInfo != null)
					{
						string templateData2 = GameEntry.ConfigCache.GetTemplateData("lw_hero", heroInfo.heroId, "quality");
						bool isAllSKillReachMax = heroInfo.GetIsAllSKillReachMax();
						GameEntry.Lua.CallWithReturn<string, int, bool>("CSharpCallLuaInterface.GetHeroQuality", templateData2.ToInt(), isAllSKillReachMax);
						spritePath = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetHeroIcon", heroInfo.heroId);
					}
				}
			}
			if (flag)
			{
				if (headObj != null)
				{
					headObj.SetActive(value: true);
				}
				if (headIconNew != null)
				{
					headIconNew.LoadSprite(spritePath);
				}
				else if (headIcon != null)
				{
					headIcon.LoadSprite(spritePath);
				}
				if (stateIcon_Icon != null)
				{
					stateIcon_Icon.gameObject.SetActive(value: false);
				}
			}
			else
			{
				if (headObj != null)
				{
					headObj.SetActive(value: false);
				}
				if (stateIcon_Icon != null)
				{
					stateIcon_Icon.gameObject.SetActive(value: true);
					stateIcon_Icon.LoadSprite(text);
				}
			}
		}
		else
		{
			if (stateIcon_Icon != null)
			{
				stateIcon_Icon.gameObject.SetActive(value: false);
			}
			if (gameObject != null)
			{
				gameObject.SetActive(value: false);
			}
		}
		SetAutoAdjustLod();
		ShowDetectEvent();
		DoWhenMarchInfoChange(null);
		DoWhenCreateComplete(model);
		CheckShowTroopDestination();
		SetClickEvent();
		SetMultiSelect();
	}

	protected override string GetModePath()
	{
		ResPointInfo resPointInfo = world.GetPointInfo(pointIndex) as ResPointInfo;
		return GetAssetPath(resPointInfo.id);
	}

	public override void Destroy()
	{
		headIconNew?.Release();
		DestroyModel();
		base.Destroy();
	}

	private void DestroyModel()
	{
		if (_gatherMarchUuid != 0L)
		{
			GameEntry.Event.Fire(EventId.CollectPointOut, _gatherMarchUuid);
			world.DestroyArmyAnimalObject(_gatherMarchUuid);
			_gatherMarchUuid = 0L;
		}
	}

	private string GetAssetPath(int id)
	{
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_gather_resource", id, "model");
		if (!templateData.IsNullOrEmpty())
		{
			return "Assets/Main/Prefabs/CollectResource/" + templateData + ".prefab";
		}
		return "Assets/Main/Prefabs/CollectResource/CollectResourcesOil_world.prefab";
	}

	public override void CheckShowTroopDestination()
	{
		bool flag = isSHowDestination;
		List<WorldMarch> ownerMarches = world.GetOwnerMarches(GameEntry.Data.Player.Uid);
		ResPointInfo resPointInfo = world.GetPointInfo(pointIndex) as ResPointInfo;
		foreach (WorldMarch item in ownerMarches)
		{
			if (item.IsVisibleMarch() && resPointInfo != null && resPointInfo.gatherMarchUuid != 0L && item.targetUuid == resPointInfo.gatherMarchUuid)
			{
				flag = false;
				Vector3 realPos = base.WorldPosition;
				int num = 1;
				EnumDestinationSignalType destinationType = world.GetDestinationType(item.uuid, item.targetUuid, pointIndex, item.target, isFormation: false, ref realPos, ref num);
				ShowTroopDestinationSignal(realPos, destinationType, num);
				break;
			}
		}
		if (flag)
		{
			HideTroopDestinationSignal();
		}
	}

	protected override bool NeedShowTime()
	{
		if (!(world.GetPointInfo(pointIndex) is ResPointInfo resPointInfo))
		{
			return false;
		}
		bool result = false;
		pickupStartTime = 0L;
		pickupEndTime = 0L;
		List<WorldMarch> ownerMarches = world.GetOwnerMarches(GameEntry.Data.Player.Uid);
		int count = ownerMarches.Count;
		for (int i = 0; i < count; i++)
		{
			WorldMarch worldMarch = ownerMarches[i];
			if (worldMarch != null && (((worldMarch.status == MarchStatus.PICKING || worldMarch.status == MarchStatus.SAMPLING) && worldMarch.targetUuid == resPointInfo.uuid) || (worldMarch.status == MarchStatus.COLLECTING && worldMarch.targetPos == pointIndex && resPointInfo.gatherMarchUuid != 0L)))
			{
				result = true;
				pickupStartTime = worldMarch.startTime;
				pickupEndTime = worldMarch.endTime;
				collectSpd = worldMarch.collectSpd;
				break;
			}
		}
		return result;
	}

	protected override void DoWhenMarchInfoChange(object userData)
	{
		if (!NeedShowTime())
		{
			if (pickGarbageInst != null)
			{
				pickGarbageInst.Destroy();
				pickGarbageInst = null;
			}
			if (FlyTextAni != null)
			{
				FlyTextAni.StopAni();
				FlyTextAni.gameObject.SetActive(value: false);
			}
			UpdateDetectEventActive();
			if (detectEventInst != null && detectEventInst.gameObject != null)
			{
				detectEventInst.gameObject.SetActive(value: true);
			}
			return;
		}
		if (pickGarbageInst == null)
		{
			pickGarbageInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/CollectGarbageUI.prefab");
			pickGarbageInst.completed += delegate
			{
				if (!(gameObject == null) && !(pickGarbageInst.gameObject == null))
				{
					pickGarbageInst.gameObject.transform.SetParent(gameObject.transform);
					pickGarbageInst.gameObject.transform.localPosition = Vector3.zero;
					timeText = pickGarbageInst.gameObject.transform.Find("PosGo/TimeText").GetComponent<SuperTextMesh>();
					timeText.gameObject.SetActive(value: true);
					pickGarbageInst.gameObject.GetComponent<ChangeSceneCircleSlider>().Init(pickupStartTime, pickupEndTime);
					UpdateProgress();
					SpriteRenderer component = pickGarbageInst.gameObject.transform.Find("PosGo/Icon").GetComponent<SpriteRenderer>();
					string spritePath = GameEntry.Lua.CallWithReturn<string>("CSharpCallLuaInterface.GetResourceDetectCollectingIcon");
					component.LoadSprite(spritePath);
				}
			};
		}
		if (FlyTextAni != null)
		{
			FlyTextAni.gameObject.SetActive(value: true);
			ResPointInfo resPointInfo = world.GetPointInfo(pointIndex) as ResPointInfo;
			int num = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetProductFlyTxtAniIntervalTime");
			int num2 = (int)collectSpd;
			string text = "+" + num * num2;
			int length = text.Length;
			float newValue = 0.1f * (float)(length - 2);
			FlyTextAni.gameObject.transform.SetLocalPositionX(newValue);
			int param = GameEntry.ConfigCache.GetTemplateData("lw_gather_resource", resPointInfo.id, "resource_type").ToInt();
			string iconPath = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetResourceIconByType", param);
			FlyTextAni.Init(text, iconPath);
			FlyTextAni.PlayAni();
		}
		UpdateDetectEventActive();
		if (detectEventInst != null && detectEventInst.gameObject != null)
		{
			detectEventInst.gameObject.SetActive(value: false);
		}
	}

	protected override bool NeedShowDetectEventIcon()
	{
		if (!string.IsNullOrEmpty(eventId))
		{
			string value = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetDetectEventIdByPointId", pointIndex);
			bool flag = NeedShowTime();
			if (eventId.Equals(value) && !flag)
			{
				return true;
			}
		}
		return false;
	}

	protected override string GetEventId()
	{
		return GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetDetectEventIdByPointId", pointIndex);
	}
}
