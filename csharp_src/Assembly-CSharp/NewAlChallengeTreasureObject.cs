using System;
using UnityEngine;

public class NewAlChallengeTreasureObject : WorldPointObject
{
	private NewAlChallengeTreasurePointInfo pointInfo;

	private Transform transform;

	private SimpleAnimation simpleAni;

	private ITimer delayTimer;

	private InstanceRequest effectReq;

	private static Color nameColorBlue = new Color(0.329f, 0.768f, 0.949f);

	private string worldModelPath = "Model/WorldModel";

	private readonly string modelPath = "Assets/Main/Prefabs/Building/kill_zombie_world_box.prefab";

	private readonly string effectPath = "Assets/_Art/Effect/prefab/scene/xinshou/VFX_nzw_shouqu_tishi.prefab";

	public NewAlChallengeTreasureObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
		if (world.GetPointInfo(pointIndex) is NewAlChallengeTreasurePointInfo newAlChallengeTreasurePointInfo)
		{
			serverId = newAlChallengeTreasurePointInfo.serverId;
			pointInfo = newAlChallengeTreasurePointInfo;
		}
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		Create();
	}

	public override void SetAutoAdjustLod()
	{
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo != null)
		{
			serverId = pointInfo.serverId;
			adjuster = gameObject.GetComponent<AutoAdjustLod>();
			if (adjuster == null)
			{
				adjuster = gameObject.AddComponent<AutoAdjustLod>();
			}
			adjuster.SetLodType(LodType.Resource);
		}
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo != null)
		{
			serverId = pointInfo.serverId;
			this.pointInfo = pointInfo as NewAlChallengeTreasurePointInfo;
		}
	}

	private void Create()
	{
		PointInfo pointInfo = world.GetPointInfo(pointIndex);
		if (pointInfo == null)
		{
			return;
		}
		serverId = pointInfo.serverId;
		this.pointInfo = pointInfo as NewAlChallengeTreasurePointInfo;
		AddOldObject();
		instance = GameEntry.Resource.InstantiateAsync(modelPath);
		instance.completed += delegate
		{
			ClearOldObject();
			gameObject = instance.gameObject;
			if (!(gameObject == null))
			{
				this.transform = gameObject.transform;
				this.transform.SetParent(world.DynamicObjNode);
				this.transform.position = base.WorldPosition;
				simpleAni = gameObject.GetComponentInChildren<SimpleAnimation>();
				if ((bool)simpleAni)
				{
					long startTime = this.pointInfo.treasurePointInfo.startTime;
					if (startTime > 0)
					{
						long num = GameEntry.Timer.GetServerTime() - startTime;
						if (num < 400)
						{
							gameObject.SetActive(value: false);
							if (isVisible)
							{
								if (delayTimer != null)
								{
									GameEntry.Timer.CancelTimer(delayTimer);
									delayTimer = null;
								}
								delayTimer = GameEntry.Timer.RegisterTimer(1f, delegate
								{
									gameObject.SetActive(isVisible);
									float clipLength2 = simpleAni.GetClipLength("born");
									PlayAnim("born", clipLength2, delegate
									{
										PlayAnim("idle", 0f, PlayEffect);
									});
									GameEntry.Sound.PlayEffect("Gameplay/challenge_zombie/Baoxiang/SFX_Env_Baoxiang_World_Open");
								});
							}
						}
						else if (num < 2000)
						{
							gameObject.SetActive(isVisible);
							if (isVisible)
							{
								float clipLength = simpleAni.GetClipLength("born");
								PlayAnim("born", clipLength, delegate
								{
									PlayAnim("idle", 0f, PlayEffect);
								});
								GameEntry.Sound.PlayEffect("Gameplay/challenge_zombie/Baoxiang/SFX_Env_Baoxiang_World_Open");
							}
						}
						else
						{
							PlayAnim("idle", 0f, PlayEffect);
						}
					}
					else
					{
						PlayAnim("idle", 0f, PlayEffect);
					}
				}
				SetAutoAdjustLod();
				SetClickEvent();
				Transform transform = this.transform.Find("Icon/Sprite");
				if (transform != null)
				{
					SpriteRenderer component = transform.GetComponent<SpriteRenderer>();
					if (component != null)
					{
						string text = "Assets/Main/Sprites/LodIcon/zyf_daditu_box_cheng1.png";
						if (!string.IsNullOrEmpty(text))
						{
							component.LoadSprite(text);
						}
					}
				}
				Transform transform2 = this.transform.Find("Model/LabelRoot/NameLabel/NameText");
				if (!(transform2 == null))
				{
					TextMeshProEx component2 = transform2.GetComponent<TextMeshProEx>();
					if (!(component2 == null))
					{
						string text2 = string.Format(GameEntry.Localization.GetString("challenge_zombie_box_title_alliance"), this.pointInfo.treasurePointInfo.allianceAbbr, GameEntry.Localization.GetString("challenge_zombie_box_title"));
						component2.text = text2;
						string allianceId = GameEntry.Data.Player.GetAllianceId();
						component2.color = ((this.pointInfo.treasurePointInfo.allianceId == allianceId) ? nameColorBlue : Color.white);
					}
				}
			}
		};
	}

	private void PlayAnim(string name, float delay = 0f, Action callback = null)
	{
		if (delayTimer != null)
		{
			GameEntry.Timer.CancelTimer(delayTimer);
			delayTimer = null;
		}
		simpleAni?.Play(name);
		if (delay > 0f)
		{
			delayTimer = GameEntry.Timer.RegisterTimer(delay, callback);
		}
		else
		{
			callback?.Invoke();
		}
	}

	private void PlayEffect()
	{
		if (effectReq != null)
		{
			return;
		}
		effectReq = GameEntry.Resource.InstantiateAsync(effectPath);
		effectReq.completed += delegate
		{
			GameObject gameObject = effectReq.gameObject;
			if (gameObject != null && world != null && world.DynamicObjNode != null)
			{
				gameObject.transform.SetParent(world.DynamicObjNode);
				gameObject.transform.localScale = Vector3.one * 2f;
				gameObject.transform.position = base.WorldPosition;
				gameObject.SetActive(value: true);
			}
		};
	}

	public override void Destroy()
	{
		base.Destroy();
		if (effectReq != null)
		{
			effectReq.Destroy();
			effectReq = null;
		}
	}
}
