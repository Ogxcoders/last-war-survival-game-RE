using UnityEngine;

public class WorldAllianceCollectResourceObject : WorldPointObject
{
	private SpriteRenderer stateIcon;

	private SpriteRenderer _collectResourceIconSprite;

	private UIWorldLabel _label;

	private InstanceRequest _createEffect;

	private bool _isFinishCreateEffect = true;

	private Coroutine _finishShowCreateEffect;

	public override int AutoLookAtThreshold => 5;

	public WorldAllianceCollectResourceObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		CreateAllianceBuildObject();
	}

	public void CreateAllianceBuildObject()
	{
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo == null)
		{
			return;
		}
		WorldAllianceCollectResource collectResource = pointInfo as WorldAllianceCollectResource;
		AddOldObject();
		string text = GameEntry.Lua.CallWithReturn<string, PointInfo>("CSharpCallLuaInterface.GetWorldPointModelPath", pointInfo);
		if (!text.IsNullOrEmpty())
		{
			instance = GameEntry.Resource.InstantiateAsync(text);
			instance.completed += delegate
			{
				gameObject = instance.gameObject;
				if (gameObject != null)
				{
					gameObject.transform.SetParent(world.DynamicObjNode);
					gameObject.transform.position = base.WorldPosition;
					if (_isFinishCreateEffect)
					{
						gameObject.SetActive(isVisible);
					}
					else
					{
						gameObject.SetActive(value: false);
					}
					stateIcon = gameObject.transform.Find("ModelGo/stateIcon")?.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
					Transform transform = gameObject.transform.Find("Icon");
					if (transform != null)
					{
						Transform transform2 = transform.transform.Find("CollectResourceIconSprite");
						if (transform2 != null)
						{
							_collectResourceIconSprite = transform2.GetComponent<SpriteRenderer>();
							if (_collectResourceIconSprite != null)
							{
								if (collectResource.allianceId == GameEntry.Data.Player.GetAllianceId())
								{
									_collectResourceIconSprite.enabled = true;
								}
								else
								{
									_collectResourceIconSprite.enabled = false;
								}
							}
						}
					}
					UIWorldLabel uIWorldLabel = gameObject.transform.Find("ModelGo/CityLabel")?.GetComponentInChildren<UIWorldLabel>(includeInactive: true);
					if (uIWorldLabel != null)
					{
						uIWorldLabel.transform.Find("NameLabel")?.gameObject.SetActive(value: false);
						if (collectResource != null)
						{
							int level = GameEntry.ConfigCache.GetTemplateData("alliance_res_build", collectResource.configId, "city_level").ToInt();
							uIWorldLabel.SetLevel(level);
						}
						else
						{
							uIWorldLabel.SetLevel(0);
						}
					}
					if (collectResource != null)
					{
						if (stateIcon != null)
						{
							stateIcon.gameObject.SetActive(collectResource.state == 1);
						}
						string text2 = "";
						UnityUIExtension.LoadSprite(spritePath: (!(collectResource.allianceId == GameEntry.Data.Player.GetAllianceId())) ? "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect_other" : "Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_march_collect", spriteRenderer: stateIcon);
					}
					else if (stateIcon != null)
					{
						stateIcon.gameObject.SetActive(value: false);
					}
					SetAutoAdjustLod();
					UpdateGameObject();
					SetClickEvent();
					SetPreviewType(collectResource);
				}
			};
		}
		if (collectResource != null && collectResource.isCreate)
		{
			ShowCrateEffect();
		}
	}

	private void SetPreviewType(WorldAllianceCollectResource info)
	{
		if (_touchObject != null)
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("alliance_res_build", info.configId, "icon");
			string templateData2 = GameEntry.ConfigCache.GetTemplateData("alliance_res_build", info.configId, "name");
			_touchObject.previewName = GameEntry.Localization.GetString(templateData2);
			_touchObject.previewIconPath = "Assets/Main/Sprites/ItemIcons/" + templateData + ".png";
			_touchObject.previewType = WorldPreviewType.GatherResource;
		}
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
	}

	public override void Destroy()
	{
		if (_createEffect != null)
		{
			_createEffect.Destroy();
			_createEffect = null;
		}
		stateIcon = null;
		if (_collectResourceIconSprite != null)
		{
			_collectResourceIconSprite.enabled = true;
		}
		_collectResourceIconSprite = null;
		_label = null;
		_isFinishCreateEffect = true;
		if (_finishShowCreateEffect != null)
		{
			YieldUtils.StopDelayActionWithOutContext(_finishShowCreateEffect);
			_finishShowCreateEffect = null;
		}
		base.Destroy();
	}

	public override void CheckShowTroopDestination()
	{
		base.CheckShowTroopDestination();
	}

	private void ShowCrateEffect()
	{
		_createEffect = GameEntry.Resource.InstantiateAsync("Assets/_Art/Effect/prefab/scene/VFX_libao_open_01.prefab");
		_isFinishCreateEffect = false;
		_createEffect.completed += delegate
		{
			if (_createEffect.gameObject != null)
			{
				_createEffect.gameObject.transform.SetParent(world.DynamicObjNode);
				_createEffect.gameObject.transform.position = base.WorldPosition + new Vector3(0f, 0f, -2f);
				_createEffect.gameObject.transform.localScale = Vector3.one;
				_createEffect.gameObject.SetActive(value: true);
				_finishShowCreateEffect = YieldUtils.DelayActionWithOutContext(delegate
				{
					_isFinishCreateEffect = true;
					if (gameObject != null)
					{
						gameObject.SetActive(isVisible);
					}
				}, 0.5f);
			}
		};
	}
}
