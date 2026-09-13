using System;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VEngine;

public class WorldMapElectricityRenderer : WorldManagerBase, IWorldGPUInstancingRenderer
{
	private bool inited;

	private const int DEFAULT_SIZE = 1000;

	private const int MAX_INSTANCING_SIZE = 1023;

	private Matrix4x4[] matrixArray;

	private float[] stateArray;

	private float[] alphaMultipleArray;

	private Vector2Int v2StoveCenter;

	private int nStoveCenterSize;

	private int centerIndex = -1;

	private Vector2Int centerPosition;

	private int lodLevel = -1;

	private int rowRadius;

	private int columnRadius;

	private int gridCount;

	private Asset materialRequester;

	private Material rendererMaterial;

	private Mesh renderMesh;

	private MaterialPropertyBlock mpb;

	private Matrix4x4[] renderMatrixArray;

	private float[] renderStateArray;

	private float[] renderAlphaMultipleArray;

	private WorldCamera worldCamera;

	private bool needRefreshGrids;

	private readonly int shaderKeyGridStates = Shader.PropertyToID("_GridStates");

	private readonly int shaderKeyAlpha = Shader.PropertyToID("_AlphaMultiple");

	private readonly int shaderKeyLod1Rate = Shader.PropertyToID("_Lod1Rate");

	private readonly int shaderKeyLod2Rate = Shader.PropertyToID("_Lod2Rate");

	private readonly int shaderKeyLod3Rate = Shader.PropertyToID("_Lod3Rate");

	private bool switchOn;

	private float lod1Rate = 1f;

	private float lod2Rate;

	private float lod3Rate;

	private const int VISIBLE_LOD_THRESHOLD = 5;

	private const float L1 = 150f;

	private const float L2 = 250f;

	private const float L3 = 310f;

	private const int GRID_STATE_EMPTY = 0;

	private const int GRID_STATE_BORDER = 1;

	private const int GRID_STATE_KING = 2;

	private const int GRID_STATE_OBSTACLE = 3;

	private const int GRID_STATE_MARCH = 4;

	private const int GRID_STATE_POINTS = 5;

	public bool IsShowing => worldCamera != null;

	public int SortingOrder => 5;

	public string Name => "[WorldInstancing]MapElectricity";

	public RenderPassEvent RenderPassEvent => RenderPassEvent.AfterRenderingOpaques;

	public bool IsRenderable
	{
		get
		{
			if (lodLevel >= 5)
			{
				return false;
			}
			if (GameEntry.Data?.Player?.IsInBattleField() ?? false)
			{
				return false;
			}
			if (rendererMaterial == null || renderMesh == null || gridCount <= 0)
			{
				return false;
			}
			return true;
		}
	}

	public WorldMapElectricityRenderer(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		base.Init();
		inited = false;
		if (!WorldInstancingRenderers.DeviceSupportInstancing)
		{
			return;
		}
		SceneSkinMeta baseSkinMeta = SceneSkinManager.Instance.GetBaseSkinMeta();
		if (baseSkinMeta != null)
		{
			switchOn = baseSkinMeta.IsDarknessMode();
		}
		if (switchOn)
		{
			switchOn = GameEntry.Data?.Player?.IsInSourceServer() ?? false;
		}
		if (!switchOn)
		{
			return;
		}
		if (rendererMaterial == null)
		{
			materialRequester = GameEntry.Resource.LoadAssetAsync("Assets/Main/SeasonRes/S4/Material/WorldMapElectricityInstancing.mat", typeof(Material));
			Asset asset = materialRequester;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
			{
				if (materialRequester != null && materialRequester.status == LoadableStatus.SuccessToLoad)
				{
					Material source = materialRequester.asset as Material;
					rendererMaterial = new Material(source);
					materialRequester.Release();
					materialRequester = null;
					renderMesh = WorldGpuInstancingUtils.CreateXZMesh(2f, -0.5f, -0.5f);
					WorldInstancingRenderers.AddRenderer(this);
					inited = true;
				}
			});
		}
		GameEntry.Event.Subscribe(EventId.UPDATE_POINTS_DATA, OnWorldPointUpdate);
	}

	public override void UnInit()
	{
		HideMapGrid();
		if (materialRequester != null)
		{
			materialRequester.Release();
			materialRequester = null;
		}
		if (inited)
		{
			WorldInstancingRenderers.RemoveRenderer(this);
		}
		GameEntry.Event.Unsubscribe(EventId.UPDATE_POINTS_DATA, OnWorldPointUpdate);
		base.UnInit();
	}

	public void ShowMapGrid(int center)
	{
		if (!WorldInstancingRenderers.DeviceSupportInstancing || IsShowing)
		{
			return;
		}
		SceneSkinMeta baseSkinMeta = SceneSkinManager.Instance.GetBaseSkinMeta();
		if (baseSkinMeta != null)
		{
			switchOn = baseSkinMeta.IsDarknessMode();
		}
		if (switchOn)
		{
			switchOn = GameEntry.Data?.Player?.IsInSourceServer() ?? false;
		}
		if (!switchOn)
		{
			return;
		}
		int num = GameEntry.Data?.Player?.GetWorldId() ?? (-1);
		if (num != 0)
		{
			return;
		}
		int num2 = GameEntry.Lua.CallWithReturn<int, string>("CSharpCallLuaInterface.GetInt", "AllianceStoveCenterPos");
		if (num2 <= 1)
		{
			return;
		}
		nStoveCenterSize = GameEntry.ConfigCache.GetTemplateData("alliance_res_build", 400001, "offter_range").ToInt();
		if (nStoveCenterSize > 5)
		{
			v2StoveCenter = TileCoord.IndexToTilePos(num2, ForceChangeScene.World);
			worldCamera = world.Camera;
			if (worldCamera != null)
			{
				centerIndex = -1;
				lodLevel = -1;
				gridCount = 0;
				lod1Rate = 1f;
				lod2Rate = 0f;
				lod3Rate = 0f;
				matrixArray = new Matrix4x4[1000];
				stateArray = new float[1000];
				alphaMultipleArray = new float[1000];
				world.PointManager?.StartRecordTileBlock(num);
				world.MarchDataManager?.StartRecordMarchBlock(num);
				lodLevel = world.GetLodLevel();
				UpdateMapGridCenter(center);
				AfterCameraUpdate();
				GameEntry.Event.Subscribe(EventId.ChangeCameraLod, OnCameraLodChanged);
				GameEntry.Event.Subscribe(EventId.WORLD_CAMERA_CHANGE_POINT, OnCameraChangePoint);
				worldCamera.AfterUpdate += AfterCameraUpdate;
				GameEntry.Event.Fire(EventId.MapGridRenderChange, IsShowing);
			}
		}
	}

	public void UpdateMapGridCenter(int center)
	{
		if (centerIndex != center && IsShowing)
		{
			RefreshCenter(center);
		}
	}

	public void HideMapGrid()
	{
		if (IsShowing)
		{
			world.PointManager?.StopRecordTileBlock();
			world.MarchDataManager?.StopRecordMarchBlock();
			lodLevel = -1;
			centerIndex = -1;
			gridCount = 0;
			mpb?.Clear();
			matrixArray = null;
			stateArray = null;
			alphaMultipleArray = null;
			GameEntry.Event.Unsubscribe(EventId.ChangeCameraLod, OnCameraLodChanged);
			GameEntry.Event.Unsubscribe(EventId.WORLD_CAMERA_CHANGE_POINT, OnCameraChangePoint);
			worldCamera.AfterUpdate -= AfterCameraUpdate;
			worldCamera = null;
			GameEntry.Event.Fire(EventId.MapGridRenderChange, IsShowing);
		}
	}

	private void AfterCameraUpdate()
	{
		if (worldCamera != null)
		{
			Vector2 viewSize = worldCamera.ViewSize;
			int num = Mathf.CeilToInt(viewSize.x / 2f / 2f) + 1;
			int num2 = Mathf.CeilToInt(viewSize.y / 2f / 2f) + 3;
			if (num2 != rowRadius || num != columnRadius)
			{
				rowRadius = num2;
				columnRadius = num;
				needRefreshGrids = true;
			}
			float lodDistance = worldCamera.GetLodDistance();
			if (lodDistance > 310f)
			{
				lod1Rate = 0f;
				lod2Rate = 0f;
				lod3Rate = 1f;
			}
			else if (lodDistance > 250f)
			{
				lod1Rate = 0f;
				lod3Rate = (lodDistance - 250f) / 60f;
				lod2Rate = 1f - lod3Rate;
			}
			else if (lodDistance > 150f)
			{
				lod3Rate = 0f;
				lod2Rate = (lodDistance - 150f) / 100f;
				lod1Rate = 1f - lod2Rate;
			}
			else
			{
				lod1Rate = 1f;
				lod2Rate = 0f;
				lod3Rate = 0f;
			}
		}
	}

	private void RefreshCenter(int center)
	{
		centerIndex = center;
		centerPosition = world.IndexToTilePos(center);
		needRefreshGrids = true;
	}

	private void OnWorldPointUpdate(object args)
	{
		needRefreshGrids = true;
	}

	private void RefreshVisibleGrids()
	{
		if (lodLevel >= 5)
		{
			return;
		}
		CustomDataManager data = GameEntry.Data;
		if (data != null && data.Player?.IsInBattleField() == true)
		{
			return;
		}
		int num = (rowRadius * 2 + 1) * (columnRadius * 2 + 1);
		if (num > matrixArray.Length)
		{
			int newSize = Mathf.CeilToInt((float)num * 1.2f);
			Array.Resize(ref matrixArray, newSize);
			Array.Resize(ref stateArray, newSize);
			Array.Resize(ref alphaMultipleArray, newSize);
		}
		int num2 = Mathf.Max(centerPosition.x - columnRadius, 0);
		int num3 = Mathf.Min(centerPosition.x + columnRadius, 999);
		int num4 = Mathf.Max(centerPosition.y - rowRadius, 0);
		int num5 = Mathf.Min(centerPosition.y + rowRadius, 999);
		float num6 = 2f;
		Matrix4x4 identity = Matrix4x4.identity;
		gridCount = 0;
		for (int i = num2; i <= num3; i++)
		{
			for (int j = num4; j <= num5; j++)
			{
				int num7 = 0;
				if (i > v2StoveCenter.x + nStoveCenterSize || j > v2StoveCenter.y + nStoveCenterSize || i < v2StoveCenter.x - nStoveCenterSize || j < v2StoveCenter.y - nStoveCenterSize)
				{
					num7 = 5;
				}
				matrixArray[gridCount] = identity;
				matrixArray[gridCount].m03 = ((float)i + 0.5f) * num6;
				matrixArray[gridCount].m23 = ((float)j + 0.5f) * num6;
				stateArray[gridCount] = num7;
				alphaMultipleArray[gridCount] = ((num7 <= 0) ? 0.5f : 2f);
				gridCount++;
			}
		}
		needRefreshGrids = false;
	}

	private void OnCameraChangePoint(object obj)
	{
		Vector2Int curTilePos = world.CurTilePos;
		UpdateMapGridCenter(world.TilePosToIndex(curTilePos));
	}

	private void OnCameraLodChanged(object obj)
	{
		if (IsShowing)
		{
			int num = (int)obj;
			lodLevel = num;
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		if (IsShowing && needRefreshGrids)
		{
			RefreshVisibleGrids();
		}
	}

	public override string Description()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("[WorldInstancing]WorldMapElectricityRenderer");
		stringBuilder.AppendLine($"开关是否打开:{switchOn}");
		stringBuilder.AppendLine($"世界ID:{GameEntry.Data?.Player.GetWorldId()}");
		stringBuilder.AppendLine($"当前中心:{centerPosition.ToString()}, 行半径:{rowRadius}, 列半径:{columnRadius}");
		object arg = gridCount;
		Matrix4x4[] array = matrixArray;
		stringBuilder.AppendLine($"地格数量:{arg}, 分配数组:{((array != null) ? array.Length : 0)}");
		return stringBuilder.ToString();
	}

	public void OnGizmos()
	{
		if (!IsShowing || gridCount <= 0)
		{
			return;
		}
		Vector3 size = new Vector3(2f, 0f, 2f);
		for (int i = 0; i < gridCount; i++)
		{
			int num = (int)stateArray[i];
			Matrix4x4 matrix4x = matrixArray[i];
			Vector3 center = matrix4x.MultiplyPoint(Vector3.zero);
			switch (num)
			{
			case 0:
				Gizmos.color = Color.yellow;
				break;
			case 1:
				Gizmos.color = Color.red;
				break;
			}
			Gizmos.DrawWireCube(center, size);
		}
	}

	public void OnGizmosSelected()
	{
	}

	public void Draw(CommandBuffer cmb)
	{
		mpb = mpb ?? new MaterialPropertyBlock();
		mpb.Clear();
		mpb.SetFloat(shaderKeyLod1Rate, lod1Rate);
		mpb.SetFloat(shaderKeyLod2Rate, lod2Rate);
		mpb.SetFloat(shaderKeyLod3Rate, lod3Rate);
		if (matrixArray.Length <= 1023)
		{
			mpb.SetFloatArray(shaderKeyGridStates, stateArray);
			mpb.SetFloatArray(shaderKeyAlpha, alphaMultipleArray);
			cmb.DrawMeshInstanced(renderMesh, 0, rendererMaterial, 0, matrixArray, gridCount, mpb);
			return;
		}
		renderMatrixArray = renderMatrixArray ?? new Matrix4x4[1023];
		renderStateArray = renderStateArray ?? new float[1023];
		renderAlphaMultipleArray = renderAlphaMultipleArray ?? new float[1023];
		int num = Mathf.CeilToInt((float)gridCount * 1f / 1023f);
		int num2 = gridCount;
		for (int i = 0; i < num; i++)
		{
			int num3 = Mathf.Min(1023, num2);
			if (num3 > 0)
			{
				Array.Copy(matrixArray, i * 1023, renderMatrixArray, 0, num3);
				Array.Copy(stateArray, i * 1023, renderStateArray, 0, num3);
				Array.Copy(alphaMultipleArray, i * 1023, renderAlphaMultipleArray, 0, num3);
				mpb.SetFloatArray(shaderKeyGridStates, renderStateArray);
				mpb.SetFloatArray(shaderKeyAlpha, renderAlphaMultipleArray);
				cmb.DrawMeshInstanced(renderMesh, 0, rendererMaterial, 0, renderMatrixArray, num3, mpb);
				num2 -= num3;
				continue;
			}
			break;
		}
	}

	public void Draw(Camera camera)
	{
		if (lodLevel >= 5 || !((!(GameEntry.Data?.Player?.IsInBattleField())) ?? true) || !(rendererMaterial != null) || !(renderMesh != null) || gridCount <= 0)
		{
			return;
		}
		mpb = mpb ?? new MaterialPropertyBlock();
		mpb.Clear();
		mpb.SetFloat(shaderKeyLod1Rate, lod1Rate);
		mpb.SetFloat(shaderKeyLod2Rate, lod2Rate);
		mpb.SetFloat(shaderKeyLod3Rate, lod3Rate);
		if (matrixArray.Length <= 1023)
		{
			mpb.SetFloatArray(shaderKeyGridStates, stateArray);
			mpb.SetFloatArray(shaderKeyAlpha, alphaMultipleArray);
			Graphics.DrawMeshInstanced(renderMesh, 0, rendererMaterial, matrixArray, gridCount, mpb, ShadowCastingMode.Off, receiveShadows: false, 0, camera);
			return;
		}
		renderMatrixArray = renderMatrixArray ?? new Matrix4x4[1023];
		renderStateArray = renderStateArray ?? new float[1023];
		renderAlphaMultipleArray = renderAlphaMultipleArray ?? new float[1023];
		int num = Mathf.CeilToInt((float)gridCount * 1f / 1023f);
		int num2 = gridCount;
		for (int i = 0; i < num; i++)
		{
			int num3 = Mathf.Min(1023, num2);
			if (num3 > 0)
			{
				Array.Copy(matrixArray, i * 1023, renderMatrixArray, 0, num3);
				Array.Copy(stateArray, i * 1023, renderStateArray, 0, num3);
				Array.Copy(alphaMultipleArray, i * 1023, renderAlphaMultipleArray, 0, num3);
				mpb.SetFloatArray(shaderKeyGridStates, renderStateArray);
				mpb.SetFloatArray(shaderKeyAlpha, renderAlphaMultipleArray);
				Graphics.DrawMeshInstanced(renderMesh, 0, rendererMaterial, renderMatrixArray, num3, mpb, ShadowCastingMode.Off, receiveShadows: false, 0, camera);
				num2 -= num3;
				continue;
			}
			break;
		}
	}
}
