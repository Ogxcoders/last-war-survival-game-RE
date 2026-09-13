using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Sfs2X.Entities.Data;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DCFog : BaseDataContainer
{
	public static int FogSizeX = 2;

	public static int FogSizeY = 2;

	private const int FogMinIndex = 1;

	public static Vector2Int FogTileCount = new Vector2Int(50, 50);

	private BitArray _allFog;

	public override void CSInit(ISFSObject obj)
	{
		InitFogDataByCacheInWasteLand();
	}

	public void InitFogDataByCacheInWasteLand()
	{
		FogSizeX = 1;
		FogSizeY = 1;
		FogTileCount = new Vector2Int(100, 100);
		_allFog = new BitArray(20000);
	}

	public void InitAllPoints(ISFSObject obj)
	{
	}

	public int GetFogIndexByPointId(int pointId)
	{
		Vector2Int vector2Int = SceneManager.World.IndexToTilePos(pointId);
		Vector2Int tileCount = SceneManager.World.TileCount;
		return vector2Int.x / FogSizeX + vector2Int.y / FogSizeY * (tileCount.x / FogSizeX) + 1;
	}

	public List<int> GetCanOpenNeighbourFogIds(int fogId)
	{
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		Vector2Int tileCount = SceneManager.World.TileCount;
		list2.Add(fogId - 1);
		list2.Add(fogId + tileCount.x / FogSizeX);
		list2.Add(fogId + 1);
		list2.Add(fogId - tileCount.x / FogSizeX);
		int count = _allFog.Count;
		foreach (int item in list2)
		{
			if (item >= 0 && item < count)
			{
				_ = _allFog[item];
				if (!IsUnlockByFogId(item))
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	public List<int> GetPointIdsByFogIndex(int fogId)
	{
		List<int> list = new List<int>();
		Vector2Int tileCount = SceneManager.World.TileCount;
		int num = fogId - 1;
		int num2 = tileCount.x / FogSizeX;
		int num3 = num % num2 * FogSizeX;
		int num4 = num / num2 * FogSizeY;
		for (int i = 0; i < FogSizeX; i++)
		{
			for (int j = 0; j < FogSizeY; j++)
			{
				list.Add(SceneManager.World.TilePosToIndex(new Vector2Int(num3 + i, num4 + j)));
			}
		}
		return list;
	}

	public bool IsUnlock(int pointId)
	{
		return IsUnlockByFogId(GetFogIndexByPointId(pointId));
	}

	public bool IsUnlockByFogId(int fogId)
	{
		if (SceneManager.World == null)
		{
			return true;
		}
		Vector2Int tileCount = SceneManager.World.TileCount;
		int num = tileCount.x / FogSizeX * (tileCount.y / FogSizeY);
		if (fogId < 1 || fogId > num)
		{
			return false;
		}
		if (_allFog.Get(fogId))
		{
			return true;
		}
		return false;
	}

	public BitArray GetAllFogData()
	{
		return _allFog;
	}

	public void UnlockFog(int fogId)
	{
		if (_allFog != null && fogId < _allFog.Length)
		{
			_allFog[fogId] = true;
		}
	}

	public int GetCanUnlockPointByLine(Vector2Int startPos, Vector2Int endPos)
	{
		Vector2Int firstLockPosByLine = GetFirstLockPosByLine(startPos, endPos);
		return SceneManager.World.TilePosToIndex(firstLockPosByLine);
	}

	private Vector2Int GetFirstLockPosByLine(Vector2Int p0, Vector2Int p1)
	{
		int num = p1.x - p0.x;
		int num2 = p1.y - p0.y;
		int num3 = Math.Abs(num);
		int num4 = Math.Abs(num2);
		int num5 = ((num > 0) ? 1 : (-1));
		int num6 = ((num2 > 0) ? 1 : (-1));
		Vector2Int vector2Int = new Vector2Int(p0.x, p0.y);
		int num7 = 0;
		int num8 = 0;
		while (num7 < num3 || num8 < num4)
		{
			if ((1 + 2 * num7) * num4 == (1 + 2 * num8) * num3)
			{
				vector2Int.x += num5;
				vector2Int.y += num6;
				num7++;
				num8++;
			}
			else if ((0.5 + (double)num7) / (double)num3 < (0.5 + (double)num8) / (double)num4)
			{
				vector2Int.x += num5;
				num7++;
			}
			else
			{
				vector2Int.y += num6;
				num8++;
			}
			Vector2Int vector2Int2 = new Vector2Int(vector2Int.x, vector2Int.y);
			if (!IsUnlock(SceneManager.World.TilePosToIndex(vector2Int2)))
			{
				return vector2Int2;
			}
		}
		return p1;
	}

	public int GetPointIdCenterByFogIndex(int fogId, int sizeIndex)
	{
		List<int> pointIdsByFogIndex = GetPointIdsByFogIndex(fogId);
		if (pointIdsByFogIndex != null && pointIdsByFogIndex.Count > sizeIndex)
		{
			return pointIdsByFogIndex[sizeIndex];
		}
		return 0;
	}

	public Dictionary<int, Dictionary<int, int>> GetCanUnlockFog()
	{
		Dictionary<int, Dictionary<int, int>> dictionary = new Dictionary<int, Dictionary<int, int>>();
		Vector2Int tileCount = SceneManager.World.TileCount;
		int num = tileCount.x / FogSizeX * (tileCount.y / FogSizeY);
		for (int i = 1; i <= num; i++)
		{
			if (IsUnlockByFogId(i))
			{
				continue;
			}
			foreach (int value in GetUnlockFlagByFogId(i).Values)
			{
				if (value != 1)
				{
					dictionary.Add(i, GetUnlockFlagByFogId(i));
					break;
				}
			}
		}
		return dictionary;
	}

	public Dictionary<int, int> GetUnlockFlagByFogId(int fogId)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		bool flag = IsUnlockByFogId(GetFogIdByOffset(fogId, 0, -1));
		bool flag2 = IsUnlockByFogId(GetFogIdByOffset(fogId, -1, 0));
		bool flag3 = IsUnlockByFogId(GetFogIdByOffset(fogId, 1, 0));
		bool flag4 = IsUnlockByFogId(GetFogIdByOffset(fogId, 0, 1));
		dictionary.Add(1, (!(flag || flag2)) ? 1 : 2);
		dictionary.Add(2, (!(flag || flag3)) ? 1 : 2);
		dictionary.Add(3, (!(flag4 || flag3)) ? 1 : 2);
		dictionary.Add(4, (!(flag4 || flag2)) ? 1 : 2);
		return dictionary;
	}

	public int GetFogIdByOffset(int index, int x, int y)
	{
		int fogIndexByOffsetX = GetFogIndexByOffsetX(index, x);
		if (fogIndexByOffsetX > 0)
		{
			return GetFogIndexByOffsetY(fogIndexByOffsetX, y);
		}
		return 0;
	}

	private int GetFogIndexByOffsetX(int index, int offset = 1)
	{
		int num = SceneManager.World.TileCount.x / FogSizeX;
		int num2 = index - 1;
		num2 %= num;
		num2 += offset;
		if (num2 >= 0 && num2 < num)
		{
			return index + offset;
		}
		return 0;
	}

	private int GetFogIndexByOffsetY(int index, int offset = 1)
	{
		int num = SceneManager.World.TileCount.x / FogSizeX;
		int num2 = index - 1;
		num2 /= num;
		num2 += offset;
		if (num2 >= 0 && num2 < num)
		{
			return index + num * offset;
		}
		return 0;
	}

	public Vector3 GetFogPositionByFogId(int fogId)
	{
		float tileSize = SceneManager.World.TileSize;
		int num = (fogId - 1) / FogTileCount.x;
		int num2 = (fogId - 1) % FogTileCount.x;
		return new Vector3(((float)num2 + 0.5f) * (tileSize * (float)FogSizeX), 0f, ((float)num + 0.5f) * (tileSize * (float)FogSizeY));
	}

	public int GetSmallDirectionByPointId(int pointId)
	{
		int fogIndexByPointId = GetFogIndexByPointId(pointId);
		Vector2Int tileCount = SceneManager.World.TileCount;
		int num = fogIndexByPointId - 1;
		int num2 = tileCount.x / FogSizeX;
		int num3 = num % num2 * FogSizeX;
		int num4 = num / num2 * FogSizeY;
		Vector2Int vector2Int = SceneManager.World.IndexToTilePos(pointId);
		int num5 = vector2Int.x - num3;
		int num6 = vector2Int.y - num4;
		if (num5 < FogSizeX / 2)
		{
			if (num6 < FogSizeY / 2)
			{
				return 1;
			}
			return 4;
		}
		if (num6 < FogSizeY / 2)
		{
			return 2;
		}
		return 3;
	}

	public int GetSpecialFogIdByFodIdAndDirection(int fogId, int direction)
	{
		int num = (fogId - 1) % FogTileCount.x * 2;
		int num2 = (fogId - 1) / FogTileCount.x * 2 * FogTileCount.x * 2 + num + 1;
		return (CanUnlockFogSmallDirection)direction switch
		{
			CanUnlockFogSmallDirection.LeftDown => num2, 
			CanUnlockFogSmallDirection.RightDown => num2 + 1, 
			CanUnlockFogSmallDirection.LeftTop => num2 + 50, 
			CanUnlockFogSmallDirection.RightTop => num2 + 51, 
			_ => num2, 
		};
	}

	private void InitFogParam(float height, float posY, Color color)
	{
		UniversalRenderPipelineAsset universalRenderPipelineAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
		if (!(universalRenderPipelineAsset != null))
		{
			return;
		}
		universalRenderPipelineAsset.shadowCascadeOption = ShadowCascadesOption.NoCascades;
		universalRenderPipelineAsset.supportsMainLightShadows = false;
		ScriptableRendererData[] obj = (ScriptableRendererData[])universalRenderPipelineAsset.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(universalRenderPipelineAsset);
		ScriptableRendererData scriptableRendererData = ((obj != null) ? obj[0] : null);
		foreach (ScriptableRendererFeature rendererFeature in scriptableRendererData.rendererFeatures)
		{
			if (rendererFeature is HeightFogRenderFeature)
			{
				HeightFogRenderFeature obj2 = rendererFeature as HeightFogRenderFeature;
				obj2.fogSetting._FogDisappearHeight = height;
				obj2.fogSetting._FogPosY = posY;
				obj2.fogSetting.unexploredColor = color;
				obj2.fogSetting.exploredColor = new Color(color.r, color.g, color.b, 0f);
				obj2.fogSetting.FogIntensity = 0f;
			}
		}
		scriptableRendererData.SetDirty();
	}
}
