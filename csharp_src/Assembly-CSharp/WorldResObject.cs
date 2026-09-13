using System.Collections.Generic;
using UnityEngine;

public class WorldResObject : WorldPointObject, IWorldLodWatcher
{
	private GameObject model;

	private GameObject headObj;

	private SpriteRenderer headBg;

	private SpriteRenderer headIcon;

	private CircleMeshInstanced headIconNew;

	private SpriteRenderer stateIcon;

	private SpriteRenderer stateIcon_Icon;

	private BuildingLabel label;

	private BasementGetArmyResMetal army;

	private UIWorldLabel ModelLable;

	private UIWorldLabel IconLable;

	private long _gatherMarchUuid;

	private string stateIconPath;

	private GameObject icon;

	protected bool fullRenderModeLoaded;

	private WorldIconRendererFacade.HappyIcon happyIcon;

	private WorldIconRendererFacade.HappyIcon happyLevel;

	private int debugGPUInstanceProp = -1;

	private int _littleSmartIconIndex = -1;

	private int resShowLevel;

	public long Uid => (long)pointType * 10000000L + pointIndex;

	protected ResPointInfo ResPointInfo => world?.GetPointInfo(pointIndex) as ResPointInfo;

	private int LittleSmartIconIndex
	{
		get
		{
			if (_littleSmartIconIndex < 0)
			{
				_littleSmartIconIndex = 3;
				if (world.GetPointInfo(pointIndex) is ResPointInfo resPointInfo)
				{
					resShowLevel = resPointInfo.GetResLevel();
					switch (resPointInfo.GetResType())
					{
					case 1:
						_littleSmartIconIndex = 1;
						break;
					case 2:
						_littleSmartIconIndex = 0;
						break;
					case 14:
						_littleSmartIconIndex = 2;
						break;
					}
				}
			}
			return _littleSmartIconIndex;
		}
	}

	public bool CanShowLittleSmartMode
	{
		get
		{
			if (!WorldInstancingRenderers.DeviceSupportInstancing)
			{
				return false;
			}
			WorldScene worldScene = world;
			if ((object)worldScene == null || !worldScene.EnableWorldIconGPUInstancing)
			{
				return false;
			}
			ResPointInfo resPointInfo = ResPointInfo;
			if (resPointInfo == null)
			{
				return false;
			}
			if (resPointInfo.gatherMarchUuid != 0L)
			{
				return false;
			}
			if (GMSwitch.IsGM)
			{
				if (debugGPUInstanceProp < 0)
				{
					debugGPUInstanceProp = Random.Range(1, 101);
				}
				if (debugGPUInstanceProp > GMSwitch.GetInt("DebugWorldPointGPUInstanceProp", 100))
				{
					return false;
				}
			}
			return true;
		}
	}

	public WorldResObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void CreateGameObject()
	{
		world?.RegisterLodWatcher(this);
		UpdateLod(world.CurrentLodLevel);
	}

	public override void Destroy()
	{
		world?.UnregisterLodWatcher(this);
		happyIcon?.Destroy();
		happyIcon = null;
		happyLevel?.Destroy();
		happyLevel = null;
		headIconNew?.Release();
		DestroyModel();
		base.Destroy();
	}

	private void DestroyModel()
	{
		if (army != null)
		{
			army.UnInit();
			army = null;
		}
		if (_gatherMarchUuid != 0L)
		{
			GameEntry.Event.Fire(EventId.CollectPointOut, _gatherMarchUuid);
			world.DestroyArmyAnimalObject(_gatherMarchUuid);
			_gatherMarchUuid = 0L;
		}
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		UpdateLod(world.CurrentLodLevel);
	}

	private void ShowFullRenderMode()
	{
		if (fullRenderModeLoaded)
		{
			return;
		}
		base.CreateGameObject();
		ResPointInfo info = world.GetPointInfo(pointIndex) as ResPointInfo;
		if (info == null)
		{
			return;
		}
		AddOldObject();
		string text = GetAssetPath(info.id);
		if (text.EndsWith("A_build_youjing_huang.prefab"))
		{
			WorldMarch march = world.GetMarch(info.gatherMarchUuid);
			if (march != null)
			{
				text = (march.IsMine() ? text.Replace("_huang.prefab", "_lan.prefab") : ((!(march.allianceUid == GameEntry.Data.Player.GetAllianceId())) ? text.Replace("_huang.prefab", "_hong.prefab") : text.Replace("_huang.prefab", "_lan.prefab")));
			}
		}
		fullRenderModeLoaded = true;
		instance = GameEntry.Resource.InstantiateAsync(text);
		instance.completed += delegate
		{
			DestroyModel();
			ClearOldObject();
			if (SceneManager.World != null && !(world.DynamicObjNode == null))
			{
				gameObject = instance.gameObject;
				if (gameObject != null)
				{
					gameObject.transform.SetParent(world.DynamicObjNode);
					gameObject.transform.position = base.WorldPosition;
					gameObject.SetActive(isVisible);
					ModelLable = gameObject.transform.Find("Model/MonsterLabel")?.GetComponent<UIWorldLabel>();
					IconLable = gameObject.transform.Find("Icon/IconLabel")?.GetComponent<UIWorldLabel>();
					SetAutoAdjustLod();
					string templateData = GameEntry.ConfigCache.GetTemplateData("lw_gather_resource", info.id, "level");
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
					icon = gameObject.transform.Find("Icon/troopIcon")?.gameObject;
					if (icon != null)
					{
						headObj = icon.transform.Find("head")?.gameObject;
						if (headObj != null)
						{
							headBg = headObj.transform.Find("headbg")?.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
							headIconNew = headObj.transform.Find("headIconNew")?.GetComponentInChildren<CircleMeshInstanced>(includeInactive: true);
							if (headIconNew == null)
							{
								headIcon = headObj.transform.Find("headIcon")?.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
							}
						}
						stateIcon_Icon = icon.transform.Find("stateIcon")?.GetComponent<SpriteRenderer>();
					}
					stateIcon = gameObject.transform.Find("Model/stateIcon")?.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
					UpdateMarch();
					CheckShowTroopDestination();
					SetClickEvent();
					if (_touchObject != null)
					{
						_touchObject.previewIconPath = GameEntry.ConfigCache.GetTemplateData("lw_gather_resource", info.id, "pic");
						string templateData2 = GameEntry.ConfigCache.GetTemplateData("lw_gather_resource", info.id, "name");
						_touchObject.previewName = GameEntry.Localization.GetString("140205", templateData, GameEntry.Localization.GetString(templateData2));
						_touchObject.previewType = WorldPreviewType.GatherResource;
					}
				}
			}
		};
	}

	public void UpdateMarch()
	{
		ResPointInfo resPointInfo = world.GetPointInfo(pointIndex) as ResPointInfo;
		if (gameObject == null || resPointInfo == null)
		{
			return;
		}
		if (resPointInfo.gatherMarchUuid == 0L)
		{
			stateIconPath = null;
			if (stateIcon != null)
			{
				stateIcon.gameObject.SetActive(value: false);
			}
			if (stateIcon_Icon != null)
			{
				stateIcon_Icon.gameObject.SetActive(value: false);
			}
			if (icon != null)
			{
				icon.SetActive(value: false);
			}
			return;
		}
		bool flag = false;
		string spritePath = "";
		string text = "";
		string text2 = "";
		string text3 = "";
		WorldMarch march = world.GetMarch(resPointInfo.gatherMarchUuid);
		if (march != null)
		{
			text2 = march.ownerUid;
			text3 = march.allianceUid;
		}
		else
		{
			text2 = resPointInfo.gatherUid;
			text3 = resPointInfo.gatherAllianceId;
		}
		if (!(GameEntry.Data.Player.Uid == text2))
		{
			text = ((!(text3 == GameEntry.Data.Player.GetAllianceId())) ? "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect_other" : "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect_alliance");
		}
		else
		{
			text = "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect";
			_gatherMarchUuid = resPointInfo.gatherMarchUuid;
			HeroInfo heroInfo = march?.GetLeaderHero();
			if (heroInfo != null)
			{
				flag = true;
				string templateData = GameEntry.ConfigCache.GetTemplateData("lw_hero", heroInfo.heroId, "quality");
				bool isAllSKillReachMax = heroInfo.GetIsAllSKillReachMax();
				GameEntry.Lua.CallWithReturn<string, int, bool>("CSharpCallLuaInterface.GetHeroQuality", templateData.ToInt(), isAllSKillReachMax);
				spritePath = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetHeroIcon", heroInfo.heroId);
			}
		}
		if (icon != null)
		{
			icon.SetActive(value: true);
		}
		if (stateIcon != null && text != stateIconPath)
		{
			stateIcon.gameObject.SetActive(value: true);
			stateIcon.LoadSprite(text);
			stateIconPath = text;
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

	private string GetAssetPath(int id)
	{
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_gather_resource", id, "model");
		if (!templateData.IsNullOrEmpty())
		{
			if (templateData.StartsWith("Assets/"))
			{
				if (templateData.EndsWith(".prefab"))
				{
					return templateData;
				}
				return templateData + ".prefab";
			}
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
				Vector3 realPos = world.TileIndexToWorld(pointIndex);
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

	public void UpdateLod(int lod)
	{
		if (fullRenderModeLoaded)
		{
			return;
		}
		bool flag = false;
		bool showIcon = false;
		bool showLevel = false;
		if (CanShowLittleSmartMode)
		{
			switch (lod)
			{
			case 1:
			case 2:
				flag = true;
				break;
			case 3:
				showIcon = true;
				showLevel = true;
				break;
			case 4:
				showIcon = true;
				break;
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			ShowFullRenderMode();
		}
		RefreshHappyIcons(showIcon, showLevel);
	}

	private void RefreshHappyIcons(bool showIcon, bool showLevel)
	{
		if (showIcon && happyIcon == null)
		{
			happyIcon = iconRendererFacade.CreateIcon("HappyWorldResIcon");
		}
		if (!showIcon && happyIcon != null)
		{
			happyIcon.Destroy();
			happyIcon = null;
		}
		happyIcon?.Refresh(base.WorldPosition, LittleSmartIconIndex);
		if (showLevel && happyLevel == null)
		{
			happyLevel = iconRendererFacade.CreateIcon("HappyWorldResLv");
		}
		if (!showLevel && happyLevel != null)
		{
			happyLevel.Destroy();
			happyLevel = null;
		}
		happyLevel?.Refresh(base.WorldPosition, resShowLevel);
	}
}
