using System;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework;
using UnityEngine;

public class SiegeTreasureObject4FlowerTrain : SiegeTreasureObject
{
	private const string FLOWER_FLY_EFFECT_PATH = "Assets/Main/Prefabs/World/FlowerTrain_World_Prefab/Halloween/Effect/Eff_ljw_2025wanshengjie_liwu1_fly.prefab";

	private const string FLOWER_BOOM_EFFECT_PATH = "Assets/Main/Prefabs/World/FlowerTrain_World_Prefab/Halloween/Effect/Eff_ljw_2025wanshengjie_liwu1_born.prefab";

	private static int SEGMENT_COUNT = 20;

	private Vector3 EFF_BIRTH_OFFSET = new Vector3(0f, 13f, 0f);

	private const float BOX_APPEAR_DELAY = 0.5f;

	private const float DROP_TIME = 1f;

	private const float BOX_DROP_EFF_DELAY = 0.8f;

	private InstanceRequest flyEffectReq;

	private Sequence flyEffectSeq;

	private Coroutine showDelayCor;

	private Coroutine boxAppearEffDelayCor;

	private static Vector3[] _paths = new Vector3[SEGMENT_COUNT];

	public SiegeTreasureObject4FlowerTrain(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void Destroy()
	{
		base.Destroy();
		if (flyEffectReq != null)
		{
			flyEffectReq.Destroy();
			flyEffectReq = null;
		}
		if (flyEffectSeq != null)
		{
			flyEffectSeq.Kill(complete: true);
		}
		if (showDelayCor != null)
		{
			YieldUtils.StopDelayActionWithOutContext(showDelayCor);
			showDelayCor = null;
		}
		if (boxAppearEffDelayCor != null)
		{
			YieldUtils.StopDelayActionWithOutContext(boxAppearEffDelayCor);
			boxAppearEffDelayCor = null;
		}
	}

	protected override string GetModePath()
	{
		string text = "";
		int key = eventId.ToInt();
		int key2 = 1001;
		if (WorldScene.ModelPathDic.ContainsKey(key))
		{
			Dictionary<int, string> dictionary = WorldScene.ModelPathDic[key];
			if (dictionary.ContainsKey(key2))
			{
				text = dictionary[key2];
			}
		}
		if (text.IsNullOrEmpty())
		{
			text = GameEntry.ConfigCache.GetTemplateData("world_treasure", key, "fullPathModels");
			if (string.IsNullOrEmpty(text))
			{
				text = GameEntry.ConfigCache.GetTemplateData("world_treasure", key, "models");
				text = "Assets/Main/Prefabs/Garbage/" + text + ".prefab";
			}
			if (!WorldScene.ModelPathDic.ContainsKey(key))
			{
				WorldScene.ModelPathDic[key] = new Dictionary<int, string>();
			}
			WorldScene.ModelPathDic[key][key2] = text;
		}
		return text;
	}

	public override void PlayFlyAnim()
	{
		int num = eventId.ToInt();
		int num2 = GameEntry.ConfigCache.GetTemplateData("world_treasure", num, "custom_para").ToInt();
		string trailEffPath = null;
		string boomEffPath = null;
		if (num2 > 0)
		{
			trailEffPath = GameEntry.ConfigCache.GetTemplateData("treasure_box_show", num2, "boxAppearTrailEff");
			boomEffPath = GameEntry.ConfigCache.GetTemplateData("treasure_box_show", num2, "dropEff");
			trailEffPath = (trailEffPath.IsNullOrEmpty() ? "Assets/Main/Prefabs/World/FlowerTrain_World_Prefab/Halloween/Effect/Eff_ljw_2025wanshengjie_liwu1_fly.prefab" : trailEffPath);
			boomEffPath = (boomEffPath.IsNullOrEmpty() ? "Assets/Main/Prefabs/World/FlowerTrain_World_Prefab/Halloween/Effect/Eff_ljw_2025wanshengjie_liwu1_born.prefab" : boomEffPath);
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		if (!(world.GetPointInfo(pointIndex) is TreasurePointInfo treasurePointInfo) || serverTime > treasurePointInfo.startTime + 2000)
		{
			return;
		}
		int pointId = treasurePointInfo.fromPoint;
		if (pointId <= 0)
		{
			return;
		}
		gameObject.SetActive(value: false);
		showDelayCor = YieldUtils.DelayActionWithOutContext(delegate
		{
			boxAppearEffDelayCor = YieldUtils.DelayActionWithOutContext(delegate
			{
				if (gameObject != null)
				{
					world.CreateVFX(boomEffPath, gameObject.transform.position, 2f);
				}
			}, 0.8f);
			flyEffectReq = GameEntry.Resource.InstantiateAsync(trailEffPath);
			flyEffectReq.completed += delegate
			{
				Transform transform = flyEffectReq.gameObject.transform;
				Vector3 vector = world.TileIndexToWorld(pointId, serverId) + EFF_BIRTH_OFFSET;
				Vector3 vector2 = gameObject.transform.position + Vector3.up * 0.5f;
				Vector3 controlPos = Vector3.Lerp(vector, vector2, UnityEngine.Random.Range(0.2f, 0.4f)) + Vector3.up * UnityEngine.Random.Range(5, 10);
				Vector3[] path = Bezier2Path(vector, controlPos, vector2);
				float duration = 1f * UnityEngine.Random.Range(0.8f, 1.2f);
				transform.position = vector;
				flyEffectSeq = DOTween.Sequence().Append(transform.DOPath(path, duration, PathType.Linear, PathMode.Full3D, 10, Color.cyan).SetEase(Ease.Linear)).OnComplete(delegate
				{
					flyEffectSeq = null;
					try
					{
						if (flyEffectReq != null)
						{
							flyEffectReq.Destroy();
							flyEffectReq = null;
						}
						if (gameObject != null)
						{
							gameObject.SetActive(value: true);
							SimpleAnimation componentInChildren = gameObject.transform.GetComponentInChildren<SimpleAnimation>();
							if ((bool)componentInChildren)
							{
								componentInChildren.Play("Born");
							}
						}
					}
					catch (Exception message)
					{
						Log.Error(message);
						if (flyEffectReq != null)
						{
							flyEffectReq.Destroy();
							flyEffectReq = null;
						}
						if (gameObject != null)
						{
							gameObject.SetActive(value: true);
						}
					}
				});
			};
		}, 0.5f);
	}

	private Vector3[] Bezier2Path(Vector3 startPos, Vector3 controlPos, Vector3 endPos)
	{
		for (int i = 1; i <= SEGMENT_COUNT; i++)
		{
			float t = (float)i / (float)SEGMENT_COUNT;
			Vector3 vector = CalculateCubicBezierPointfor2C(t, startPos, controlPos, endPos);
			_paths[i - 1] = vector;
		}
		return _paths;
	}

	private Vector3 CalculateCubicBezierPointfor2C(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float num = 1f - t;
		float num2 = t * t;
		return num * num * p0 + 2f * num * t * p1 + num2 * p2;
	}
}
