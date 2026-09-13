using GameFramework;
using UnityEngine;

public class WorldMeteoritePointObject : WorldPointObject, IWorldDelayDestroyObject
{
	private WorldMeteoritePointObjectHandle handle;

	private WorldPointRelationType relationType;

	private WorldMeteoritePoint.MeteoritePointState curState = WorldMeteoritePoint.MeteoritePointState.Default;

	private string curPlayerUuid;

	private ITimer updateTimer;

	protected InstanceRequest dropWarningRequest;

	private string currentIconPath;

	private ParticlePlayer dropWarningEffectPlayer;

	private float delayPlayDisaTime = -1f;

	private float delayDestroyTime = -1f;

	public bool CanDestroy
	{
		get
		{
			float time = Time.time;
			if (time >= delayDestroyTime)
			{
				return true;
			}
			if (delayPlayDisaTime >= 0f && time >= delayPlayDisaTime)
			{
				handle?.disappearAnimation?.Play("Disa");
				delayPlayDisaTime = -1f;
			}
			return false;
		}
	}

	public WorldMeteoritePointObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		delayPlayDisaTime = -1f;
		delayDestroyTime = -1f;
		WorldMeteoritePoint info = world.GetPointInfo(pointIndex) as WorldMeteoritePoint;
		if (info == null)
		{
			return;
		}
		string templateData = GameEntry.ConfigCache.GetTemplateData("yuntie_battle_entity", info.buildId, "model");
		if (string.IsNullOrEmpty(templateData))
		{
			Log.Error($"WorldMeteoritePointObject.CreateGameObject exception! Missing model path: id = {info.buildId}");
		}
		AddOldObject();
		instance = GameEntry.Resource.InstantiateAsync(templateData);
		instance.completed += delegate
		{
			ClearOldObject();
			if (SceneManager.World != null && !(world.DynamicObjNode == null))
			{
				gameObject = instance.gameObject;
				if (gameObject != null)
				{
					gameObject.transform.SetParent(world.DynamicObjNode);
					gameObject.transform.position = base.WorldPosition;
					gameObject.SetActive(isVisible);
					handle = gameObject.GetComponent<WorldMeteoritePointObjectHandle>();
					handle?.ResetMaterial();
					SetAutoAdjustLod();
					SetClickEvent();
					if (_touchObject != null)
					{
						_touchObject.previewType = WorldPreviewType.MeteoriteRes;
						_touchObject.previewName = GameEntry.Localization.GetString(GameEntry.ConfigCache.GetTemplateData("yuntie_battle_entity", info.buildId, "name"));
						_touchObject.previewIconPath = info.buildId.ToString();
					}
					Refresh();
				}
			}
		};
		if (info.State == WorldMeteoritePoint.MeteoritePointState.Prepare)
		{
			MeteoriteWorldEffectPlayer.Instance?.CreateMiddleFragmentPlayer(info.pointIndex, info.openTime);
		}
		else if (info.State == WorldMeteoritePoint.MeteoritePointState.Drop)
		{
			MeteoriteWorldEffectPlayer.Instance?.CreateMeteoriteDropPlayer(info.fromPoint, info.pointIndex, info.openTime);
		}
	}

	public override void Destroy()
	{
		currentIconPath = string.Empty;
		curState = WorldMeteoritePoint.MeteoritePointState.Default;
		curPlayerUuid = string.Empty;
		DestroyDropWarningEff();
		DestroyUpdateTimer();
		handle?.Dispose();
		base.Destroy();
	}

	public void TryDelayDestroy(long currentTime)
	{
		if (!(world.GetPointInfo(pointIndex) is WorldMeteoritePoint { ExpiredTime: var expiredTime }))
		{
			DestroyImmediate();
		}
		else if (expiredTime <= 0)
		{
			StartDelayDestroy(world?.PointManager);
		}
		else if (Mathf.Abs(currentTime - expiredTime) <= 1000f)
		{
			DestroyImmediate();
		}
		else
		{
			StartDelayDestroy(world?.PointManager);
		}
	}

	private void StartDelayDestroy(WorldPointManager pointManager)
	{
		if (pointManager == null)
		{
			DestroyImmediate();
		}
		else if (!(delayDestroyTime > 0f))
		{
			if (handle == null || handle.disappearAnimation == null)
			{
				DestroyImmediate();
				return;
			}
			float num = (float)Random.Range(900, 1100) / 1000f;
			float num2 = (float)Random.Range(0, 2000) / 1000f;
			delayPlayDisaTime = Time.time + num2;
			delayDestroyTime = delayPlayDisaTime + 2f * num;
			handle.disappearAnimation.SetStateSpeed("Disa", num);
			handle.disappearAnimation.Rewind();
			handle.iconNode?.TryActive(active: false);
			pointManager.AddToDelayDestroyList(this);
		}
	}

	public void DestroyImmediate()
	{
		Destroy();
	}

	public void SetCountDownShow(bool bShow)
	{
		if (!(world.GetPointInfo(pointIndex) is WorldMeteoritePoint { State: var state } worldMeteoritePoint))
		{
			handle.goCountdownNode?.TryActive(active: false);
			return;
		}
		switch (state)
		{
		case WorldMeteoritePoint.MeteoritePointState.Collecting:
			handle.goCountdownNode.TryActive(bShow);
			break;
		case WorldMeteoritePoint.MeteoritePointState.Idle:
			if (worldMeteoritePoint.IsFull)
			{
				handle.goCountdownNode?.TryActive(active: false);
			}
			else
			{
				handle.goCountdownNode?.TryActive(bShow);
			}
			break;
		default:
			handle.goCountdownNode?.TryActive(active: false);
			break;
		}
	}

	private void RefreshMeteoriteSpecialRenderer()
	{
		if (!(world.GetPointInfo(pointIndex) is WorldMeteoritePoint { State: var state } worldMeteoritePoint))
		{
			handle.goResCountNode?.TryActive(active: false);
			handle.goModelNode?.TryActive(active: true);
			handle.goCountdownNode?.TryActive(active: false);
			DestroyUpdateTimer();
			return;
		}
		if (curState != state)
		{
			curState = state;
			Refresh();
		}
		switch (state)
		{
		case WorldMeteoritePoint.MeteoritePointState.Prepare:
			handle.goResCountNode?.TryActive(active: false);
			handle.goModelNode?.TryActive(active: false);
			handle.goCountdownNode?.TryActive(active: false);
			UpdateDropWarning(worldMeteoritePoint);
			StartTimer();
			break;
		case WorldMeteoritePoint.MeteoritePointState.Drop:
			handle.goResCountNode?.TryActive(active: false);
			handle.goModelNode?.TryActive(active: false);
			handle.goCountdownNode?.TryActive(active: false);
			StartTimer();
			break;
		case WorldMeteoritePoint.MeteoritePointState.Collecting:
			handle.goResCountNode?.TryActive(active: true);
			handle.goModelNode?.TryActive(active: true);
			UpdateCountdown(worldMeteoritePoint.VirtualRemainTimeSec);
			ShowAddSpeed(worldMeteoritePoint.pointPerSec);
			UpdateProgress(worldMeteoritePoint.VirtualRemainTimeMs, worldMeteoritePoint.maxGatherTime * 1000);
			StartTimer();
			DestroyDropWarningEff();
			break;
		case WorldMeteoritePoint.MeteoritePointState.Idle:
			handle.goModelNode?.TryActive(active: true);
			DestroyUpdateTimer();
			if (worldMeteoritePoint.IsFull)
			{
				handle.goResCountNode?.TryActive(active: false);
				handle.goCountdownNode?.TryActive(active: false);
			}
			else
			{
				handle.goResCountNode?.TryActive(active: true);
				ShowAddSpeed(worldMeteoritePoint.pointPerSec);
				UpdateCountdown(worldMeteoritePoint.remainTime);
				UpdateProgress(worldMeteoritePoint.VirtualRemainTimeMs, worldMeteoritePoint.maxGatherTime * 1000);
			}
			DestroyDropWarningEff();
			break;
		}
	}

	private void ShowRemainCount(int count)
	{
		if (handle.textRemain != null)
		{
			handle.textRemain.text = $"{count}";
		}
	}

	private void ShowAddSpeed(int spd)
	{
		if (handle.textRemain != null)
		{
			handle.textRemain.text = GameEntry.Localization.GetString("yuntieBattle_interface_1052", spd);
		}
	}

	private void UpdateAddCount(int count)
	{
		if (handle.textRemain != null)
		{
			handle.textRemain.text = $"+{count}";
		}
	}

	private void UpdateCountdown(int second)
	{
		if (handle.textCountdown != null)
		{
			handle.textCountdown.text = second + "s";
		}
	}

	private void UpdateProgress(int countdownMs, int maxMs)
	{
		if (handle.srProgress != null && handle.srMask != null)
		{
			maxMs = ((maxMs <= 0) ? 1 : maxMs);
			float num = (float)countdownMs * 1f / (float)maxMs;
			float num2 = 0.05f;
			Vector2 size = handle.srProgress.size;
			float x = size.x;
			size.x = (size.x - num2) * num + num2;
			handle.srMask.size = size;
			Vector3 localPosition = handle.srProgress.transform.localPosition;
			localPosition.x = (size.x - x) / 2f;
			handle.srMask.transform.localPosition = localPosition;
		}
	}

	private void Refresh()
	{
		WorldMeteoritePoint worldMeteoritePoint = world.GetPointInfo(pointIndex) as WorldMeteoritePoint;
		if (handle == null || worldMeteoritePoint == null)
		{
			return;
		}
		handle.iconNode?.TryActive(active: true);
		handle.srIconState.gameObject.TryActive(active: false);
		handle.srModelState.gameObject.TryActive(active: false);
		string spritePath = "Assets/Main/Sprites/UI/LWCommon/Sprite/lrb_tongyong_jindutiao_hui.png";
		if (worldMeteoritePoint.gatherUUID <= 0)
		{
			curPlayerUuid = string.Empty;
			handle.srPlayer.gameObject.TryActive(active: false);
			handle.goIconTroopNode.TryActive(active: false);
			relationType = WorldPointRelationType.Other;
			RefreshLodIcon(relationType, worldMeteoritePoint.buildId);
			handle.srMask.LoadSprite(spritePath);
			handle.srScoreIcon?.LoadSprite("Assets/Main/Sprites/UI/LWActMeteorite/lrb_zhouliuhuodong_jifen_hui.png");
			RefreshMeteoriteSpecialRenderer();
			return;
		}
		handle.goIconTroopNode.TryActive(active: true);
		string text = string.Empty;
		string text2 = "";
		string text3 = "";
		string text4 = "";
		int num = 0;
		int num2 = 0;
		long num3 = 0L;
		int num4 = 0;
		WorldMarch march = world.GetMarch(worldMeteoritePoint.gatherUUID);
		if (march != null)
		{
			text2 = march.ownerUid;
			text3 = march.allianceUid;
			text4 = march.pic;
			num = march.picVer;
			num2 = march.headSkinId;
			num3 = march.headSkinET;
		}
		else
		{
			text2 = worldMeteoritePoint.gatherUid;
			text3 = worldMeteoritePoint.gatherAllianceId;
			text4 = worldMeteoritePoint.targetPic;
			num = worldMeteoritePoint.targetPicVer;
			num2 = worldMeteoritePoint.targetHeadId;
			num3 = worldMeteoritePoint.targetHeadET;
		}
		num4 = worldMeteoritePoint.srcServerId;
		string allianceId = GameEntry.Data.Player.GetAllianceId();
		int sourceServerId = GameEntry.Data.Player.GetSourceServerId();
		string spritePath2;
		string spritePath3;
		if (GameEntry.Data.Player.Uid == text2)
		{
			relationType = WorldPointRelationType.Self;
			spritePath2 = "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect";
			spritePath = "Assets/Main/Sprites/UI/LWCommon/Sprite/cfm_tongyong_jindutiao_lv.png";
			spritePath3 = "Assets/Main/Sprites/UI/LWActMeteorite/lrb_zhouliuhuodong_jifen_lv.png";
			HeroInfo heroInfo = march?.GetLeaderHero();
			if (heroInfo != null)
			{
				text = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetHeroIcon", heroInfo.heroId);
			}
		}
		else if (!allianceId.IsNullOrEmpty() && text3 == allianceId)
		{
			relationType = WorldPointRelationType.Alliance;
			spritePath2 = "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect_alliance";
			spritePath = "Assets/Main/Sprites/UI/LWCommon/Sprite/lyp_tongyong_jindutiao_lan.png";
			spritePath3 = "Assets/Main/Sprites/UI/LWActMeteorite/lrb_zhouliuhuodong_jifen_lan.png";
		}
		else if (num4 == sourceServerId)
		{
			relationType = WorldPointRelationType.SameSrcServer;
			spritePath2 = "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect_alliance";
			spritePath = "Assets/Main/Sprites/UI/LWCommon/Sprite/cfm_tongyong_jindutiao_huang.png";
			spritePath3 = "Assets/Main/Sprites/UI/LWActMeteorite/lrb_zhouliuhuodong_jifen_huang.png";
		}
		else
		{
			relationType = WorldPointRelationType.Enemy;
			spritePath2 = "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect_other";
			spritePath = "Assets/Main/Sprites/UI/LWCommon/Sprite/lrb_tongyong_jindutiao_hong.png";
			spritePath3 = "Assets/Main/Sprites/UI/LWActMeteorite/lrb_zhouliuhuodong_jifen_hong.png";
		}
		handle.srScoreIcon?.LoadSprite(spritePath3);
		RefreshLodIcon(relationType, worldMeteoritePoint.buildId);
		if (!string.IsNullOrEmpty(text))
		{
			handle.srIconHead.gameObject.TryActive(active: true);
			handle.srIconHead.LoadSprite(text);
		}
		else
		{
			handle.srIconHead.gameObject.TryActive(active: false);
			handle.srIconState.LoadSprite(spritePath2);
		}
		handle.srMask.LoadSprite(spritePath);
		if (curPlayerUuid != text2)
		{
			handle.srPlayer.gameObject.TryActive(active: false);
			handle.srPlayer2.gameObject.TryActive(active: false);
			if (string.IsNullOrEmpty(text2))
			{
				handle.anim.enabled = true;
				handle.anim.Play("Eff_UI_MeteoriteResource_world_01");
			}
			else if (string.IsNullOrEmpty(worldMeteoritePoint.targetPic))
			{
				handle.srPlayer.SetData(text2, text4, num);
				string text5 = GameEntry.Lua.CallWithReturn<string, int, long>("CSharpCallLuaInterface.GetHeadFrame", num2, num3);
				if (!string.IsNullOrEmpty(text5))
				{
					handle.srFrame.LoadSprite(text5);
				}
				handle.anim.enabled = true;
				handle.anim.Play("Eff_UI_MeteoriteResource_world_02");
			}
			else
			{
				handle.srPlayer.SetData("", worldMeteoritePoint.targetPic, worldMeteoritePoint.targetPicVer);
				string text6 = GameEntry.Lua.CallWithReturn<string, int, long>("CSharpCallLuaInterface.GetHeadFrame", worldMeteoritePoint.targetHeadId, worldMeteoritePoint.targetHeadET);
				if (!string.IsNullOrEmpty(text6))
				{
					handle.srFrame.LoadSprite(text6);
				}
				handle.srPlayer2.SetData(text2, text4, num);
				text6 = GameEntry.Lua.CallWithReturn<string, int, long>("CSharpCallLuaInterface.GetHeadFrame", num2, num3);
				if (!string.IsNullOrEmpty(text6))
				{
					handle.srFrame2.LoadSprite(text6);
				}
				handle.anim.enabled = true;
				handle.anim.Play("Eff_UI_MeteoriteResource_world_03");
			}
			curPlayerUuid = text2;
		}
		RefreshMeteoriteSpecialRenderer();
	}

	private void RefreshLodIcon(WorldPointRelationType relationType, int rId)
	{
		string text = string.Empty;
		switch (relationType)
		{
		case WorldPointRelationType.Self:
			switch (rId)
			{
			case 100:
				text = "Assets/Main/Sprites/LodIcon/zyf_yuntiesuipian_wujisuofang_lv.png";
				break;
			case 101:
				text = "Assets/Main/Sprites/LodIcon/zyf_yuntiejiejing_wujisuofang_lv.png";
				break;
			case 102:
				text = "Assets/Main/Sprites/LodIcon/zyf_yuntiejingti_wujisuofang_lv.png";
				break;
			}
			break;
		case WorldPointRelationType.Alliance:
			switch (rId)
			{
			case 100:
				text = "Assets/Main/Sprites/LodIcon/zyf_yuntiesuipian_wujisuofang_lan.png";
				break;
			case 101:
				text = "Assets/Main/Sprites/LodIcon/zyf_yuntiejiejing_wujisuofang_lan.png";
				break;
			case 102:
				text = "Assets/Main/Sprites/LodIcon/zyf_yuntiejingti_wujisuofang_lan.png";
				break;
			}
			break;
		case WorldPointRelationType.Enemy:
			switch (rId)
			{
			case 100:
				text = "Assets/Main/Sprites/LodIcon/zyf_yuntiesuipian_wujisuofang_hong.png";
				break;
			case 101:
				text = "Assets/Main/Sprites/LodIcon/zyf_yuntiejiejing_wujisuofang_hong.png";
				break;
			case 102:
				text = "Assets/Main/Sprites/LodIcon/zyf_yuntiejingti_wujisuofang_hong.png";
				break;
			}
			break;
		case WorldPointRelationType.SameSrcServer:
			switch (rId)
			{
			case 100:
				text = "Assets/Main/Sprites/LodIcon/zyf_yuntiesuipian_wujisuofang_huang.png";
				break;
			case 101:
				text = "Assets/Main/Sprites/LodIcon/zyf_yuntiejiejing_wujisuofang_huang.png";
				break;
			case 102:
				text = "Assets/Main/Sprites/LodIcon/zyf_yuntiejingti_wujisuofang_huang.png";
				break;
			}
			break;
		default:
			switch (rId)
			{
			case 100:
				text = "Assets/Main/Sprites/LodIcon/zyf_yuntiesuipian_wujisuofang_bai.png";
				break;
			case 101:
				text = "Assets/Main/Sprites/LodIcon/zyf_yuntiejiejing_wujisuofang_bai.png";
				break;
			case 102:
				text = "Assets/Main/Sprites/LodIcon/zyf_yuntiejingti_wujisuofang_bai.png";
				break;
			}
			break;
		}
		if (!string.IsNullOrEmpty(text) && currentIconPath != text)
		{
			currentIconPath = text;
			handle.srLodIcon.LoadSprite(currentIconPath);
		}
	}

	private void StartTimer()
	{
		if (updateTimer == null)
		{
			updateTimer = GameEntry.Timer.RegisterTimerRepeat(0f, 0.1f, RefreshMeteoriteSpecialRenderer);
		}
	}

	private void DestroyUpdateTimer()
	{
		if (updateTimer != null)
		{
			GameEntry.Timer.CancelTimer(updateTimer);
			updateTimer = null;
		}
	}

	public override void SetAutoAdjustLod()
	{
		if (world.GetPointInfo(pointIndex) != null)
		{
			adjuster = gameObject.GetComponent<AutoAdjustLod>();
			if (adjuster == null)
			{
				adjuster = gameObject.AddComponent<AutoAdjustLod>();
			}
			adjuster.SetLodType(LodType.MeteoriteRes);
		}
	}

	private void UpdateDropWarning(WorldMeteoritePoint info)
	{
		if (dropWarningEffectPlayer != null)
		{
			info = info ?? (world.GetPointInfo(pointIndex) as WorldMeteoritePoint);
			int num = 5000;
			float progress = 1f - (float)info.OpenTimeCountdownMs * 1f / (float)num;
			dropWarningEffectPlayer.Progress = progress;
		}
		else
		{
			if (dropWarningRequest != null)
			{
				return;
			}
			dropWarningRequest = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/World/Meteorite/EffPrefabs/MeteoriteSonDropWarning.prefab");
			dropWarningRequest.completed += delegate(InstanceRequest request)
			{
				if (request.gameObject == null)
				{
					DestroyDropWarningEff();
				}
				else if (handle == null)
				{
					DestroyDropWarningEff();
				}
				else
				{
					request.gameObject.transform.SetParent(handle.goModelRoot.transform);
					request.gameObject.transform.localPosition = Vector3.zero;
					request.gameObject.transform.localScale = Vector3.one;
					dropWarningEffectPlayer = request.gameObject.GetComponent<ParticlePlayer>();
					if (dropWarningEffectPlayer == null)
					{
						DestroyDropWarningEff();
					}
					else
					{
						request.gameObject.TryActive(active: true);
						RefreshMeteoriteSpecialRenderer();
					}
				}
			};
		}
	}

	private void DestroyDropWarningEff()
	{
		if (dropWarningRequest != null)
		{
			dropWarningRequest.Destroy();
			dropWarningRequest = null;
			dropWarningEffectPlayer = null;
		}
	}
}
