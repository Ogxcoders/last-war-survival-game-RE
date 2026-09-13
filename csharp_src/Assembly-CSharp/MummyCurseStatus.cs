using DG.Tweening;
using Protobuf;
using UnityEngine;

public class MummyCurseStatus : StatusStateBase
{
	private Sequence animScaleBuild;

	private InstanceRequest _inst;

	private WorldBuilding _worldBuilding;

	private GameObject _buildModel;

	private GameObject _buildRoot;

	private int _layer = 1;

	private float _value;

	private float _duration;

	public MummyCurseStatus(Status status, WorldBuilding building, GameObject model, GameObject normalObj)
		: base(status.Id, status.BeginTime, status.ExpireTime)
	{
		_layer = status.Layer;
		_buildRoot = model;
		_buildModel = normalObj;
		_worldBuilding = building;
	}

	public override void Start()
	{
		if (_buildRoot == null || _buildModel == null || _buildRoot.transform == null || _buildModel.transform == null)
		{
			_duration = 0f;
			return;
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		_duration = (float)(endTime - serverTime) / 1000f;
		DoEffect();
	}

	public override void UpdateStatus(Status newData)
	{
		_layer = newData.Layer;
		endTime = newData.ExpireTime;
		if (_buildRoot == null || _buildModel == null || _buildRoot.transform == null || _buildModel.transform == null)
		{
			_duration = 0f;
			return;
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		_duration = (float)(endTime - serverTime) / 1000f;
		DoEffect();
	}

	public override StateType Update(float deltaTime)
	{
		_duration -= deltaTime;
		if (_duration > 0.01f)
		{
			return StateType.Continue;
		}
		return StateType.Finish;
	}

	private void DoEffect()
	{
		if (statusId == GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE1)
		{
			if (!(_duration > 1f) || _inst != null)
			{
				return;
			}
			string prefabPath = "Assets/Main/SeasonRes/Shared/Prefabs/Effect/Eff_s3_munaiyi_attack_fire.prefab";
			InstanceRequest inst = GameEntry.Resource.InstantiateAsync(prefabPath);
			inst.completed += delegate
			{
				GameObject gameObject = inst.gameObject;
				if (gameObject != null && _buildRoot != null)
				{
					gameObject.transform.SetParent(_buildRoot.transform);
					gameObject.transform.localScale = Vector3.one * 1.5f;
					gameObject.transform.localPosition = new Vector3(0f, -0.5f, 0f);
				}
			};
			_inst = inst;
		}
		else if (statusId != GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE2 && statusId == GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE3 && _buildModel != null && _buildModel.transform != null && _layer >= 0 && _layer <= 5)
		{
			Vector3 vector = Vector3.one * (1f - 0.1f * (float)_layer);
			if (animScaleBuild != null)
			{
				animScaleBuild.Kill();
				animScaleBuild = null;
			}
			animScaleBuild = DOTween.Sequence();
			animScaleBuild.Append(_buildModel.transform.DOScale(vector * 0.9f, 0.2f));
			animScaleBuild.Append(_buildModel.transform.DOScale(vector * 1.1f, 0.2f));
			animScaleBuild.Append(_buildModel.transform.DOScale(vector, 0.2f));
			animScaleBuild.onComplete = delegate
			{
				animScaleBuild = null;
			};
		}
	}

	public override void Dispose()
	{
		base.Dispose();
		if (animScaleBuild != null)
		{
			animScaleBuild.Kill();
			animScaleBuild = null;
		}
		if (statusId == GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE3 && _buildModel != null && _buildModel.transform != null)
		{
			_buildModel.transform.localScale = Vector3.one;
		}
		if (_inst != null)
		{
			_inst.Destroy();
		}
		_inst = null;
		_worldBuilding = null;
		_buildModel = null;
		_buildRoot = null;
		statusId = -1;
	}
}
