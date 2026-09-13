using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VEngine;
using XLua;

public class WorldMapGridRenderer : WorldManagerBase, IWorldGPUInstancingRenderer
{
	public struct KingCity
	{
		public int minX;

		public int maxX;

		public int minY;

		public int maxY;

		public void Reset()
		{
			minX = 0;
			maxY = 0;
			minY = 0;
			maxY = 0;
		}
	}

	private bool inited;

	private const int DEFAULT_SIZE = 1000;

	private const int MAX_INSTANCING_SIZE = 1023;

	private Matrix4x4[] matrixArray;

	private float[] stateArray;

	private float[] alphaMultipleArray;

	private Vector3 centerPosition;

	private int lodLevel = -1;

	private int rowRadius;

	private int columnRadius;

	private int gridCount;

	private KingCity kingCity;

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

	private bool switchOn = true;

	private int battleFieldSide = -1;

	private float lod1Rate = 1f;

	private float lod2Rate;

	private float lod3Rate;

	private const int VISIBLE_LOD_THRESHOLD = 5;

	public bool mIsNineNationMode;

	private bool inBattle;

	private const float L1 = 150f;

	private const float L2 = 250f;

	private const float L3 = 310f;

	private Dictionary<int, int> _curBlockRange;

	private const int GRID_STATE_EMPTY = 0;

	private const int GRID_STATE_BORDER = 1;

	private const int GRID_STATE_KING = 2;

	private const int GRID_STATE_OBSTACLE = 3;

	private const int GRID_STATE_MARCH = 4;

	private const int GRID_STATE_POINTS = 5;

	public int SortingOrder => 5;

	public bool IsShowing => worldCamera != null;

	public string Name => "[WorldInstancing]MapGrid";

	public RenderPassEvent RenderPassEvent => RenderPassEvent.AfterRenderingOpaques;

	public bool IsRenderable
	{
		get
		{
			if (lodLevel >= 5)
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

	private void InitDragon()
	{
	}

	private void InitWinterStorm()
	{
	}

	private void InitEpidemicZone()
	{
	}

	public WorldMapGridRenderer(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		base.Init();
		inited = false;
		switchOn = GameEntry.Data?.Player?.CheckSwitch("show_move_city_grid", defaultVal: false) ?? false;
		if (!switchOn || !WorldInstancingRenderers.DeviceSupportInstancing)
		{
			return;
		}
		if (rendererMaterial == null)
		{
			if (materialRequester != null)
			{
				materialRequester.Release();
				materialRequester = null;
			}
			materialRequester = GameEntry.Resource.LoadAssetAsync("Assets/Main/Material/WorldMapGridInstancing.mat", typeof(Material));
			Asset asset = materialRequester;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
			{
				if (materialRequester != null && materialRequester.status == LoadableStatus.SuccessToLoad)
				{
					Material source = materialRequester.asset as Material;
					rendererMaterial = new Material(source);
					renderMesh = WorldGpuInstancingUtils.CreateXZMesh(2f, -0.5f, -0.5f);
					WorldInstancingRenderers.AddRenderer(this);
					inited = true;
				}
			});
		}
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null && curSkinMeta.IsNineNationMode())
		{
			mIsNineNationMode = true;
		}
		else
		{
			mIsNineNationMode = false;
		}
		GameEntry.Event.Subscribe(EventId.UPDATE_POINTS_DATA, OnWorldPointUpdate);
		GameEntry.Event.Subscribe(EventId.UpdateWorldBlockData, OnWorldBlockDataUpdate);
	}

	public override void UnInit()
	{
		HideMapGrid();
		if (materialRequester != null)
		{
			materialRequester.Release();
			materialRequester = null;
		}
		if (rendererMaterial != null)
		{
			UnityEngine.Object.Destroy(rendererMaterial);
			rendererMaterial = null;
		}
		if (inited)
		{
			WorldInstancingRenderers.RemoveRenderer(this);
		}
		_curBlockRange?.Clear();
		GameEntry.Event.Unsubscribe(EventId.UPDATE_POINTS_DATA, OnWorldPointUpdate);
		GameEntry.Event.Unsubscribe(EventId.UpdateWorldBlockData, OnWorldBlockDataUpdate);
		base.UnInit();
	}

	public void ShowMapGrid(Vector3 worldPos)
	{
		if (!WorldInstancingRenderers.DeviceSupportInstancing || !switchOn || IsShowing)
		{
			return;
		}
		int num = GameEntry.Data?.Player?.GetWorldId() ?? (-1);
		if (num >= 0)
		{
			worldCamera = world.Camera;
			if (worldCamera != null)
			{
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
				PrepareBlocks();
				lodLevel = world.GetLodLevel();
				UpdateMapGridCenter(worldPos);
				AfterCameraUpdate();
				GameEntry.Event.Subscribe(EventId.ChangeCameraLod, OnCameraLodChanged);
				GameEntry.Event.Subscribe(EventId.WORLD_CAMERA_CHANGE_POINT, OnCameraChangePoint);
				worldCamera.AfterUpdate += AfterCameraUpdate;
				GameEntry.Event.Fire(EventId.MapGridRenderChange, IsShowing);
			}
		}
	}

	public void SetBlockRangeData(int realL, LuaArrAccess blockData)
	{
		if (blockData.GetArrayCapacity() == 0 || realL == 0)
		{
			return;
		}
		if (_curBlockRange != null)
		{
			_curBlockRange.Clear();
		}
		else
		{
			_curBlockRange = new Dictionary<int, int>();
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		for (int i = 1; i <= realL; i++)
		{
			num = blockData.GetInt(i);
			switch (i % 4)
			{
			case 1:
				num3 = num;
				break;
			case 2:
				num4 = num;
				break;
			case 3:
				num5 = num;
				break;
			case 0:
			{
				num6 = num;
				int j = num4;
				int num7 = num3;
				for (; j <= num5; j++)
				{
					_curBlockRange[j + num7 * 1000 + 1] = num6;
				}
				break;
			}
			}
		}
	}

	private void PrepareBlocks()
	{
		LuaTable luaTable = null;
		DCPlayer dCPlayer = GameEntry.Data?.Player;
		inBattle = dCPlayer?.IsInBattleField() ?? false;
		battleFieldSide = -1;
		if (!inBattle)
		{
			luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetKingCityOccupiedRange");
		}
		else
		{
			int num = 4;
			for (int i = 1; i <= num; i++)
			{
				if (dCPlayer != null && dCPlayer.IsInBattleField(i))
				{
					luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetBattleFieldRange", i);
					battleFieldSide = GameEntry.Lua.CallWithReturn<int, int>("CSharpCallLuaInterface.GetPlayerSideInBattleField", i);
					break;
				}
			}
		}
		if (luaTable == null || !luaTable.ContainsKey("minX"))
		{
			kingCity.Reset();
		}
		else
		{
			try
			{
				kingCity.minX = luaTable.Get<int>("minX");
				kingCity.maxX = luaTable.Get<int>("maxX");
				kingCity.minY = luaTable.Get<int>("minY");
				kingCity.maxY = luaTable.Get<int>("maxY");
			}
			catch (Exception ex)
			{
				kingCity.Reset();
				Log.Error("WorldMapGridRenderer.PrepareBlocks exception:" + ex.Message);
			}
		}
		if (mIsNineNationMode)
		{
			world.StaticManager.SetKingCityObstacle_S5(kingCity);
		}
	}

	public void UpdateMapGridCenter(Vector3 worldPos)
	{
		if (IsShowing && !(centerPosition == worldPos))
		{
			RefreshCenter(worldPos);
		}
	}

	public void HideMapGrid()
	{
		if (IsShowing)
		{
			world.PointManager?.StopRecordTileBlock();
			world.MarchDataManager?.StopRecordMarchBlock();
			centerPosition = Vector3.zero;
			lodLevel = -1;
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
			int num = Mathf.CeilToInt(viewSize.x / 2f) + 1;
			int num2 = Mathf.CeilToInt(viewSize.y / 2f) + 3;
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

	private void RefreshCenter(Vector3 worldPos)
	{
		centerPosition = worldPos;
		needRefreshGrids = true;
	}

	private void OnWorldPointUpdate(object args)
	{
		needRefreshGrids = true;
	}

	private void OnWorldBlockDataUpdate(object args)
	{
		needRefreshGrids = true;
	}

	private void RefreshVisibleGrids()
	{
		if (lodLevel >= 5)
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
		bool flag = world.WorldSize > 1000;
		int num2 = Math.Max((int)((centerPosition.x - (float)columnRadius) / 2f), 0);
		int num3 = Math.Min((int)((centerPosition.x + (float)columnRadius) / 2f), flag ? 2999 : 999);
		int num4 = Math.Max((int)((centerPosition.z - (float)rowRadius) / 2f), 0);
		int num5 = Math.Min((int)((centerPosition.z + (float)rowRadius) / 2f), flag ? 2999 : 999);
		DCPlayer obj = GameEntry.Data?.Player;
		bool flag2 = obj?.IsInBattleField(1) ?? false;
		bool flag3 = obj?.IsInBattleField(2) ?? false;
		bool flag4 = obj?.IsInBattleField(3) ?? false;
		int value = 0;
		bool flag5 = obj?.IsInBattleField(4) ?? false;
		float num6 = 2f;
		Vector2Int tileCount = world.TileCount;
		Matrix4x4 identity = Matrix4x4.identity;
		gridCount = 0;
		HashSet<int> hashSet = (inBattle ? null : world.StaticManager?.ObstaclesSet);
		Dictionary<int, int> dictionary = world.PointManager?.TileBlockIndex;
		Dictionary<int, int> dictionary2 = world.MarchDataManager?.TileBlockIndex;
		for (int i = num2; i <= num3; i++)
		{
			for (int j = num4; j <= num5; j++)
			{
				int num7 = i % 1000;
				int num8 = j % 1000;
				int num9 = num7 + num8 * tileCount.x + 1;
				int num10 = 0;
				if (inBattle)
				{
					if (i >= kingCity.maxX || j >= kingCity.maxY || i <= kingCity.minX || j <= kingCity.minY)
					{
						num10 = 1;
					}
				}
				else if (mIsNineNationMode)
				{
					if (world.StaticManager.IsInKingCityRange(i, j))
					{
						num10 = 2;
					}
				}
				else if (i <= kingCity.maxX && j <= kingCity.maxY && i >= kingCity.minX && j >= kingCity.minY)
				{
					num10 = 2;
				}
				if (!mIsNineNationMode && num10 == 0 && hashSet != null)
				{
					num10 = (hashSet.Contains(num9) ? 3 : 0);
				}
				if (num10 == 0 && dictionary != null && dictionary.TryGetValue(num9, out value))
				{
					num10 = 5;
				}
				if (num10 == 0 && dictionary2 != null && dictionary2.TryGetValue(num9, out value))
				{
					num10 = 4;
				}
				if (num10 == 0)
				{
					if (flag2)
					{
						num10 = (InDesertWorldBlock(num9) ? 3 : 0);
					}
					else if (flag3)
					{
						num10 = (InWinterStormWorldBlock(num9) ? 3 : 0);
					}
					else if (flag4)
					{
						num10 = (InEpidemicZoneWorldBlock(num9) ? 3 : 0);
					}
					else if (flag5)
					{
						num10 = (InDsbWorldBlock(num9) ? 3 : 0);
					}
				}
				if (mIsNineNationMode && num10 == 0 && hashSet != null && world.StaticManager.IsObstacle(new Vector2Int(i, j)))
				{
					num10 = 3;
				}
				matrixArray[gridCount] = identity;
				matrixArray[gridCount].m03 = ((float)i + 0.5f) * num6;
				matrixArray[gridCount].m23 = ((float)j + 0.5f) * num6;
				stateArray[gridCount] = num10;
				alphaMultipleArray[gridCount] = ((num10 <= 0) ? 0.5f : 2f);
				gridCount++;
			}
		}
		needRefreshGrids = false;
	}

	private void OnCameraChangePoint(object obj)
	{
		Vector3 curTarget = world.CurTarget;
		UpdateMapGridCenter(curTarget);
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
		stringBuilder.AppendLine("[WorldInstancing]WorldMapGridRenderer");
		stringBuilder.AppendLine($"开关是否打开:{switchOn}");
		stringBuilder.AppendLine($"世界ID:{GameEntry.Data?.Player.GetWorldId()}");
		stringBuilder.AppendLine($"InBattle:{inBattle}");
		stringBuilder.AppendLine($"是否在沙漠战场:{GameEntry.Data?.Player.IsInBattleField(1)}");
		stringBuilder.AppendLine($"是否在冬日战场:{GameEntry.Data?.Player.IsInBattleField(2)}");
		stringBuilder.AppendLine($"是否在疫变禁区战场:{GameEntry.Data?.Player.IsInBattleField(3)}");
		stringBuilder.AppendLine($"当前BlockRange容量：{_curBlockRange?.Count}");
		stringBuilder.AppendLine($"在战场中的side是：{battleFieldSide}");
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
		if (lodLevel >= 5 || !(rendererMaterial != null) || !(renderMesh != null) || gridCount <= 0)
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

	private bool InDesertWorldBlock(int pointIndex)
	{
		if (_curBlockRange == null || _curBlockRange.Count <= 0)
		{
			return false;
		}
		if (_curBlockRange.TryGetValue(pointIndex, out var value))
		{
			switch (value)
			{
			case 1:
				return true;
			case 2:
				return battleFieldSide == 0;
			case 3:
				return battleFieldSide == 1;
			}
		}
		return false;
	}

	private bool InWinterStormWorldBlock(int pointIndex)
	{
		if (_curBlockRange == null || _curBlockRange.Count <= 0)
		{
			return false;
		}
		if (_curBlockRange.TryGetValue(pointIndex, out var value))
		{
			switch (value)
			{
			case 1:
				return true;
			case 2:
				return battleFieldSide == 1;
			case 3:
				return battleFieldSide == 2;
			}
		}
		return false;
	}

	private bool InEpidemicZoneWorldBlock(int pointIndex)
	{
		if (_curBlockRange == null || _curBlockRange.Count <= 0)
		{
			return false;
		}
		if (_curBlockRange.TryGetValue(pointIndex, out var value))
		{
			switch (value)
			{
			case 1:
				return true;
			case 2:
			case 3:
				return battleFieldSide != 1;
			case 4:
				return battleFieldSide != 2;
			case 5:
				return battleFieldSide != 3;
			}
		}
		return false;
	}

	private bool InDsbWorldBlock(int pointIndex)
	{
		if (_curBlockRange == null || _curBlockRange.Count <= 0)
		{
			return false;
		}
		if (_curBlockRange.TryGetValue(pointIndex, out var value))
		{
			switch (value)
			{
			case 1:
				return true;
			case 2:
			case 3:
			case 4:
			case 5:
				return battleFieldSide + 1 != value;
			}
		}
		return false;
	}
}
