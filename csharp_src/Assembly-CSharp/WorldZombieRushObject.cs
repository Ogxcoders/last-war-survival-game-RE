using Protobuf;
using UnityEngine;

public class WorldZombieRushObject : WorldPointObject
{
	private enum ZombieRushAllianceStatus
	{
		Prepare = -1,
		Ready,
		InBattle,
		Failure,
		Success
	}

	private SuperTextMesh nameText;

	private SuperTextMesh timeText;

	private GameObject timeTextBg;

	private ZombieRushInfo zombieRushInfo;

	private long lastCheckTime;

	private bool isShowTime;

	private TextMeshProEx timeTextTmp;

	private TextMeshProEx nameTextTmp;

	public WorldZombieRushObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		CreateAllianceBuildObject();
	}

	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
		if (isShowTime)
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			if (serverTime - lastCheckTime > 1000)
			{
				lastCheckTime = serverTime;
				UpdateCutDownEndTime();
			}
		}
	}

	private void CreateAllianceBuildObject()
	{
		AllianceBuildPointInfo info = world.GetPointInfo(pointIndex) as AllianceBuildPointInfo;
		if (info == null)
		{
			return;
		}
		serverId = info.serverId;
		AddOldObject();
		string text = GameEntry.Lua.CallWithReturn<string, PointInfo>("CSharpCallLuaInterface.GetWorldPointModelPath", info);
		if (text.IsNullOrEmpty())
		{
			return;
		}
		instance = GameEntry.Resource.InstantiateAsync(text);
		instance.completed += delegate
		{
			ClearOldObject();
			gameObject = instance.gameObject;
			if (gameObject != null)
			{
				gameObject.transform.SetParent(world.DynamicObjNode);
				gameObject.transform.position = base.WorldPosition;
				gameObject.SetActive(isVisible);
				if ((bool)gameObject.transform.Find("ModelGo/CityLabel/NameLabel/NameTmp"))
				{
					nameTextTmp = gameObject.transform.Find("ModelGo/CityLabel/NameLabel/NameTmp").GetComponent<TextMeshProEx>();
				}
				if ((bool)gameObject.transform.Find("ModelGo/CityLabel/Time/TimeTextTmp"))
				{
					timeTextTmp = gameObject.transform.Find("ModelGo/CityLabel/Time/TimeTextTmp").GetComponent<TextMeshProEx>();
				}
				if ((bool)gameObject.transform.Find("ModelGo/CityLabel/Time"))
				{
					timeTextBg = gameObject.transform.Find("ModelGo/CityLabel/Time").gameObject;
				}
				SetAutoAdjustLod();
				CheckShowTroopDestination();
				SetClickEvent();
				if (_touchObject != null)
				{
					string name = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetZombieRushName", info.buildId);
					_touchObject.previewIconPath = "Assets/Main/Sprites/LodIcon/zombieRush_red.png";
					_touchObject.previewName = UIUtils.FormatServerAllianceName(info.srcServerId, info.alAbbr, name);
					_touchObject.previewType = WorldPreviewType.ZombieRushBuilding;
				}
				DoWhenInfoChange();
			}
		};
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		DoWhenInfoChange();
	}

	private void DoWhenInfoChange()
	{
		if (!(world.GetPointInfo(pointIndex) is AllianceBuildPointInfo allianceBuildPointInfo))
		{
			return;
		}
		string allianceId = GameEntry.Data.Player.GetAllianceId();
		AllianceBuildingPointInfo allianceBuildingPointInfo = AllianceBuildingPointInfo.Parser.ParseFrom(allianceBuildPointInfo.extraInfo);
		if (allianceBuildingPointInfo != null && allianceBuildingPointInfo.ZombieRushInfo != null)
		{
			zombieRushInfo = allianceBuildingPointInfo.ZombieRushInfo;
			string text = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetZombieRushName", allianceBuildPointInfo.buildId);
			if (nameTextTmp != null)
			{
				nameTextTmp.text = "[" + allianceBuildPointInfo.alAbbr + "]" + text;
			}
			isShowTime = !string.IsNullOrEmpty(allianceId) && allianceBuildPointInfo.allianceId == allianceId;
			if (timeTextBg != null)
			{
				timeTextBg.SetActive(isShowTime);
			}
			if (isShowTime)
			{
				UpdateCutDownEndTime();
			}
		}
	}

	private void UpdateCutDownEndTime()
	{
		if (zombieRushInfo != null)
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			long num = zombieRushInfo.StateEndTime - serverTime;
			string text = "";
			if (zombieRushInfo.State == 0)
			{
				text = GameEntry.Localization.GetString("zombieRush_state_01");
			}
			else if (zombieRushInfo.State == 1)
			{
				text = GameEntry.Localization.GetString("zombieRush_state_02");
			}
			if (num > 0)
			{
				if (timeTextTmp != null)
				{
					if (zombieRushInfo.State == -1)
					{
						timeTextTmp.text = GameEntry.Localization.GetString("zombieRush_state_04", GameEntry.Timer.MilliSecondToFmtString(num));
						return;
					}
					timeTextTmp.text = GameEntry.Localization.GetString("zombieRush_tips_23", zombieRushInfo.Round, text, GameEntry.Timer.MilliSecondToFmtString(num));
				}
			}
			else if (timeTextTmp != null)
			{
				if (zombieRushInfo.State == -1)
				{
					timeTextTmp.text = GameEntry.Localization.GetString("zombieRush_state_04", GameEntry.Timer.MilliSecondToFmtString(0L));
					return;
				}
				timeTextTmp.text = GameEntry.Localization.GetString("zombieRush_tips_23", zombieRushInfo.Round, text, GameEntry.Timer.MilliSecondToFmtString(0L));
			}
		}
		else if (timeTextTmp != null)
		{
			timeTextTmp.text = "";
		}
	}

	public override void Destroy()
	{
		nameText = null;
		timeText = null;
		timeTextTmp = null;
		nameTextTmp = null;
		zombieRushInfo = null;
		base.Destroy();
	}
}
