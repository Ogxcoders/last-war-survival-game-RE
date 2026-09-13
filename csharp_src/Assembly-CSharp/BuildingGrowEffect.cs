using System;
using System.Collections.Generic;
using UnityEngine;
using VEngine;

public class BuildingGrowEffect : MonoBehaviour
{
	[Serializable]
	public class RendererMaterial
	{
		public Renderer renderer;

		public Material[] normalMaterials;

		public Material[] growingMaterials;

		public Material[] disappearMaterials;

		public Material[] normalHighMaterials;
	}

	[SerializeField]
	private BuildingGrowAnimationCurve animCurve;

	[SerializeField]
	private GameObject normalObj;

	[SerializeField]
	private GameObject shadowObj;

	private float EffectPlayLeftTime = 0.2f;

	private float GlassGridAlpha = 0.15f;

	[SerializeField]
	private string[] prefabInstanceOnStart;

	private List<InstanceRequest> attachmentList = new List<InstanceRequest>();

	[SerializeField]
	private float height;

	[SerializeField]
	private float heightDelta;

	private Vector4 bounds;

	private Asset constructMaterial;

	[SerializeField]
	private GameObject[] rendererMaterialsExclude;

	[Space(10f)]
	[SerializeField]
	private RendererMaterial[] rendererMaterials;

	private int _startTime;

	private int _endTime;

	private int _animStage = int.MinValue;

	private long _uuid;

	private int _buildId;

	private float _time;

	public bool isWorking;

	public bool isHiding;

	private MaterialPropertyBlock propBlock;

	private float progressCurveFinalTime = 0.7320341f;

	private float progressCurveFinalTimeEnd = 0.7512913f;

	private float scanCurveFinalTime = 0.8127133f;

	private float scanCurveFinalTimeEnd = 0.8505808f;

	private float gridFinalTime = 0.6180536f;

	private float gridFinalTimeEnd = 0.6335183f;

	private float glassCoverCurveFinalTime = 0.85f;

	private float glassCoverCurveFinalTimeEnd = 0.95f;

	private bool isOther;

	public float ProgressTime = 6.8f;

	private void Awake()
	{
		propBlock = new MaterialPropertyBlock();
	}

	public void StartBuild(long uuid, int buildId, int startTime, int endTime, int tileSizeX, int tileSizeY, string ownerUid, bool isDomeUpdate = false, bool isShowRobet = true, bool needGridAlpha = true)
	{
		_uuid = uuid;
		_buildId = buildId;
		_startTime = startTime;
		_endTime = endTime;
		if (isWorking)
		{
			SceneManager.World.ChangeBuildRobotFinishTime(_uuid, endTime - startTime);
		}
		else
		{
			if (_animStage == 0)
			{
				return;
			}
			_animStage = 0;
			_time = GameEntry.Timer.GetServerTimeSeconds() - startTime;
			if (_time < 0f)
			{
				_time = 0f;
			}
			bounds = new Vector4((float)(-tileSizeX) * 0.5f, (float)(-tileSizeY) * 0.5f, tileSizeX, tileSizeY) * SceneManager.World.TileSize;
			if (shadowObj != null)
			{
				shadowObj.SetActive(value: false);
			}
			SetNormalMaterials();
			RendererMaterial[] array = rendererMaterials;
			foreach (RendererMaterial rendererMaterial in array)
			{
				if (!(rendererMaterial.renderer == null))
				{
					rendererMaterial.renderer.sharedMaterials = rendererMaterial.growingMaterials;
				}
			}
			if (isDomeUpdate)
			{
				if (needGridAlpha)
				{
					propBlock.SetFloat("_GridAlpha", GlassGridAlpha);
				}
				else
				{
					propBlock.SetFloat("_GridAlpha", 0f);
				}
			}
			propBlock.SetFloat("_ScanProgress", 0f);
			propBlock.SetFloat("_Progress", 0f);
			propBlock.SetFloat("_GridProgress", 0f);
			propBlock.SetFloat("_GridYOffset", 0f);
			propBlock.SetFloat("_GlassProgress", 0f);
			propBlock.SetFloat("_BuildingHeight", height);
			propBlock.SetVector("_WorldPivot", base.transform.position);
			propBlock.SetVector("_Bounds", bounds);
			array = rendererMaterials;
			foreach (RendererMaterial rendererMaterial2 in array)
			{
				if (!(rendererMaterial2.renderer == null))
				{
					rendererMaterial2.renderer.SetPropertyBlock(propBlock);
				}
			}
			isOther = ownerUid != GameEntry.Data.Player.Uid;
			if (isShowRobet)
			{
				float duration = endTime - startTime;
				if (!isOther)
				{
					SceneManager.World.CreateBuildRobot(_uuid, base.transform.position, height, duration, tileSizeX, tileSizeY, !GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.IsShowBuildFlyPath") || _time > 0f);
				}
				else
				{
					SceneManager.World.CreateOtherBuildRobot(_uuid, base.transform.position, height, duration, tileSizeX, tileSizeY);
				}
			}
			if (prefabInstanceOnStart == null || isDomeUpdate)
			{
				return;
			}
			string[] array2 = prefabInstanceOnStart;
			foreach (string prefabPath in array2)
			{
				InstanceRequest req = GameEntry.Resource.InstantiateAsync(prefabPath);
				req.completed += delegate
				{
					req.gameObject.transform.SetParent(base.transform);
					req.gameObject.transform.localPosition = Vector3.zero;
				};
				attachmentList.Add(req);
			}
		}
	}

	public void DisappearBuild(long uuid, int buildId, int startTime, int endTime, int tileSizeX, int tileSizeY, string ownerUid)
	{
		_uuid = uuid;
		_buildId = buildId;
		_startTime = startTime;
		_endTime = endTime;
		if (isHiding)
		{
			return;
		}
		_time = GameEntry.Timer.GetServerTimeSeconds() - startTime;
		if (_time < 0f)
		{
			_time = 0f;
		}
		bounds = new Vector4((float)(-tileSizeX) * 0.5f, (float)(-tileSizeY) * 0.5f, tileSizeX, tileSizeY) * SceneManager.World.TileSize;
		if (shadowObj != null)
		{
			shadowObj.SetActive(value: false);
		}
		SetNormalMaterials();
		RendererMaterial[] array = rendererMaterials;
		foreach (RendererMaterial rendererMaterial in array)
		{
			if (!(rendererMaterial.renderer == null))
			{
				rendererMaterial.renderer.sharedMaterials = rendererMaterial.disappearMaterials;
			}
		}
		propBlock.SetFloat("_ScanProgress", 1f);
		propBlock.SetFloat("_Progress", 1f);
		propBlock.SetFloat("_GridProgress", 1f);
		propBlock.SetFloat("_GridYOffset", 0f);
		propBlock.SetFloat("_GlassProgress", 1f);
		propBlock.SetFloat("_BuildingHeight", height);
		propBlock.SetVector("_WorldPivot", base.transform.position);
		propBlock.SetVector("_Bounds", bounds);
		array = rendererMaterials;
		foreach (RendererMaterial rendererMaterial2 in array)
		{
			if (!(rendererMaterial2.renderer == null))
			{
				rendererMaterial2.renderer.SetPropertyBlock(propBlock);
			}
		}
		isOther = ownerUid != GameEntry.Data.Player.Uid;
		isHiding = true;
	}

	public void ShowNormal(PlayerType playType = PlayerType.PlayerNone)
	{
		_startTime = 0;
		_endTime = 0;
		_time = 0f;
		_animStage = int.MinValue;
		if (constructMaterial != null)
		{
			GameEntry.Resource.UnloadAsset(constructMaterial);
			constructMaterial = null;
		}
		SetNormalMaterials();
		if (shadowObj != null)
		{
			shadowObj.SetActive(value: true);
		}
	}

	public void ShowBuildGridSelection()
	{
		if (constructMaterial != null || rendererMaterials.Length == 0)
		{
			return;
		}
		constructMaterial = GameEntry.Resource.LoadAssetAsync("Assets/Main/Material/building_construct.mat", typeof(Material));
		Asset asset = constructMaterial;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			Material material = constructMaterial.asset as Material;
			RendererMaterial[] array = rendererMaterials;
			foreach (RendererMaterial rendererMaterial in array)
			{
				if (!(rendererMaterial.renderer == null))
				{
					Material[] array2 = new Material[rendererMaterial.normalMaterials.Length];
					for (int j = 0; j < array2.Length; j++)
					{
						array2[j] = material;
					}
					rendererMaterial.renderer.sharedMaterials = array2;
				}
			}
		});
	}

	public void ShowCanPlace(bool canPlace)
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		materialPropertyBlock.SetFloat("_ColorSwitch", (!canPlace) ? 1 : 0);
		RendererMaterial[] array = rendererMaterials;
		foreach (RendererMaterial rendererMaterial in array)
		{
			if (!(rendererMaterial.renderer == null))
			{
				rendererMaterial.renderer.SetPropertyBlock(materialPropertyBlock);
			}
		}
	}

	public float GetHeight()
	{
		return height + heightDelta;
	}

	private bool canStartGrow()
	{
		if (_buildId == 10100000)
		{
			return true;
		}
		WorldBuildRobot buildRobot = SceneManager.World.GetBuildRobot(_uuid);
		if (buildRobot != null)
		{
			return buildRobot.isWorking();
		}
		return false;
	}

	private void Update()
	{
		if (_endTime <= 0)
		{
			return;
		}
		if (!isWorking)
		{
			isWorking = canStartGrow();
		}
		int serverTimeSeconds = GameEntry.Timer.GetServerTimeSeconds();
		if (_startTime >= serverTimeSeconds)
		{
			return;
		}
		_time += Time.deltaTime;
		float num = 0.001f;
		if (_animStage == 0)
		{
			num = _endTime - _startTime;
		}
		else if (_animStage == 1)
		{
			num = EffectPlayLeftTime;
		}
		float num2 = Mathf.Clamp01(_time / num);
		float num3 = num2;
		float num4 = num2;
		float num5 = num2;
		float num6 = num2;
		if (animCurve != null)
		{
			if (_animStage == 0)
			{
				num3 = animCurve.progressCurve.Evaluate(num2);
				num6 = animCurve.glassCoverCurve.Evaluate(num2);
				num4 = animCurve.scanCurve.Evaluate(num2);
				num5 = animCurve.gridCurve.Evaluate(num2);
			}
			else if (_animStage == 1)
			{
				num3 = animCurve.progressCurve1.Evaluate(num2);
				num6 = animCurve.glassCoverCurve1.Evaluate(num2);
				num4 = animCurve.scanCurve1.Evaluate(num2);
				num5 = animCurve.gridCurve1.Evaluate(num2);
			}
		}
		if (isHiding)
		{
			propBlock.SetFloat("_ScanProgress", 1f - num4);
			propBlock.SetFloat("_Progress", 1f - num3);
			propBlock.SetFloat("_GridProgress", 1f - num5);
			propBlock.SetFloat("_GlassProgress", 1f - num6);
		}
		else
		{
			propBlock.SetFloat("_ScanProgress", num4);
			propBlock.SetFloat("_Progress", num3);
			propBlock.SetFloat("_GridProgress", num5);
			propBlock.SetFloat("_GlassProgress", num6);
		}
		propBlock.SetFloat("_BuildingHeight", height);
		propBlock.SetVector("_WorldPivot", base.transform.position);
		propBlock.SetVector("_Bounds", bounds);
		RendererMaterial[] array = rendererMaterials;
		foreach (RendererMaterial rendererMaterial in array)
		{
			if (!(rendererMaterial.renderer == null))
			{
				rendererMaterial.renderer.SetPropertyBlock(propBlock);
			}
		}
		if (num2 >= 1f && _animStage == 0)
		{
			EndAnimStage0();
			isWorking = false;
			_time = 0f;
			_animStage = 1;
		}
	}

	private void OnDisable()
	{
		bool flag = SceneManager.IsInCity();
		RendererMaterial[] array = rendererMaterials;
		foreach (RendererMaterial rendererMaterial in array)
		{
			if (!(rendererMaterial.renderer == null))
			{
				if (flag)
				{
					rendererMaterial.renderer.sharedMaterials = rendererMaterial.normalHighMaterials;
				}
				else
				{
					rendererMaterial.renderer.sharedMaterials = rendererMaterial.normalMaterials;
				}
			}
		}
		if (SceneManager.World != null)
		{
			if (isOther)
			{
				SceneManager.World.AddToNeedRemoveList(_uuid);
			}
			else
			{
				SceneManager.World.ChangeBuildRobotState(_uuid, WorldBuildRobot.State.GoBackRotation);
			}
		}
	}

	private void SetNormalMaterials()
	{
		bool flag = SceneManager.IsInCity();
		int graphicLevel = SceneQualitySetting.GetGraphicLevel();
		RendererMaterial[] array = rendererMaterials;
		foreach (RendererMaterial rendererMaterial in array)
		{
			if (!(rendererMaterial.renderer == null))
			{
				if (flag && graphicLevel != 1)
				{
					rendererMaterial.renderer.sharedMaterials = rendererMaterial.normalHighMaterials;
				}
				else
				{
					rendererMaterial.renderer.sharedMaterials = rendererMaterial.normalMaterials;
				}
			}
		}
	}

	private void EndAnimStage0()
	{
		if (_animStage == 0)
		{
			_ = isHiding;
		}
		if (isOther)
		{
			SceneManager.World.AddToNeedRemoveList(_uuid);
		}
		else
		{
			SceneManager.World.ChangeBuildRobotState(_uuid, WorldBuildRobot.State.GoBackRotation);
		}
		if (shadowObj != null)
		{
			shadowObj.SetActive(value: true);
		}
		foreach (InstanceRequest attachment in attachmentList)
		{
			attachment.Destroy();
		}
		attachmentList.Clear();
	}

	public void EndAnim()
	{
		EndAnimStage0();
		isWorking = false;
		_startTime = 0;
		_endTime = 0;
		_time = 0f;
		_animStage = int.MinValue;
	}

	public void SetAlphaValue(float alpha)
	{
		RendererMaterial[] array = rendererMaterials;
		foreach (RendererMaterial rendererMaterial in array)
		{
			if (!(rendererMaterial.renderer == null) && rendererMaterial.renderer.sharedMaterial.HasProperty("_Color"))
			{
				rendererMaterial.renderer.GetPropertyBlock(propBlock);
				Color color = propBlock.GetColor("_Color");
				color.a = alpha;
				propBlock.SetColor("_Color", color);
				rendererMaterial.renderer.SetPropertyBlock(propBlock);
				if (alpha <= 0f)
				{
					rendererMaterial.renderer.gameObject.SetActive(value: false);
				}
				else
				{
					rendererMaterial.renderer.gameObject.SetActive(value: true);
				}
			}
		}
	}

	private void RefreshShowColor(PlayerType playType)
	{
		float value = 1f;
		float value2 = 1f;
		switch (playType)
		{
		case PlayerType.PlayerNone:
		case PlayerType.PlayerSelf:
			value = 1f;
			value2 = 1f;
			break;
		case PlayerType.PlayerAlliance:
		case PlayerType.PlayerAllianceLeader:
			value = 0f;
			value2 = 1f;
			break;
		case PlayerType.PlayerOther:
			value = 0f;
			value2 = 0f;
			break;
		}
		RendererMaterial[] array = rendererMaterials;
		foreach (RendererMaterial rendererMaterial in array)
		{
			if (!(rendererMaterial.renderer == null))
			{
				rendererMaterial.renderer.material.SetFloat("_Fresnel_switch", value);
				rendererMaterial.renderer.material.SetFloat("_Fresnel_Color_switch", value2);
			}
		}
	}

	public bool IsUseFakeShadow()
	{
		if (SceneManager.IsInCity())
		{
			RendererMaterial[] array = rendererMaterials;
			for (int i = 0; i < array.Length; i++)
			{
				Material[] normalHighMaterials = array[i].normalHighMaterials;
				for (int j = 0; j < normalHighMaterials.Length; j++)
				{
					if (normalHighMaterials[j].name.Contains("_high"))
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public void SetRendererMaterials()
	{
	}

	private void TestStartBuild()
	{
		base.enabled = true;
		int serverTimeSeconds = GameEntry.Timer.GetServerTimeSeconds();
		int endTime = serverTimeSeconds + Mathf.RoundToInt(ProgressTime);
		StartBuild(0L, 0, serverTimeSeconds, endTime, 2, 2, GameEntry.Data.Player.Uid);
	}

	private void TestStopBuild()
	{
		base.enabled = false;
		ShowNormal();
	}
}
