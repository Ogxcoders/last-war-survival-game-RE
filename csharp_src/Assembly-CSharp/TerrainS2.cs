using System;
using System.Collections.Generic;
using UnityEngine;
using VEngine;

public class TerrainS2
{
	public const int MIPMAP = 3;

	private Vector2Int _lastBlockLB;

	private Vector2Int _lastBlockRT;

	private HashSet<Vector2Int> _processQueue1;

	private HashSet<Vector2Int> _processQueue2;

	private HashSet<Vector2Int> _toLoadQueue;

	private Dictionary<string, Asset> _materials = new Dictionary<string, Asset>();

	private Asset _activeMaterial;

	public int BLOCK_SIZE => terrainQuadLayer.blockSize;

	public virtual int Mipmap => 3;

	public ITerrainQuadLayer terrainQuadLayer { get; private set; }

	public void Init(ITerrainQuadLayer renderer)
	{
		_processQueue1 = new HashSet<Vector2Int>();
		_processQueue2 = new HashSet<Vector2Int>();
		_toLoadQueue = new HashSet<Vector2Int>();
		terrainQuadLayer = renderer;
		terrainQuadLayer.Setup(1000, 3);
		Reset();
	}

	public void UnInit()
	{
		Reset();
		_activeMaterial = null;
		foreach (KeyValuePair<string, Asset> material in _materials)
		{
			material.Value?.Release();
		}
		terrainQuadLayer.SetupMaterial(null);
	}

	private void Reset()
	{
		_lastBlockLB = new Vector2Int(-1, -1);
		_lastBlockRT = new Vector2Int(-1, -1);
		_processQueue1.Clear();
		_processQueue2.Clear();
		_toLoadQueue.Clear();
		terrainQuadLayer.Clear();
	}

	public void LoadMaterial(string path)
	{
		if (_materials.TryGetValue(path, out var value))
		{
			if (value.isDone)
			{
				terrainQuadLayer.SetupMaterial(value.asset as Material);
			}
		}
		else
		{
			value = GameEntry.Resource.LoadAssetAsync(path, typeof(Material));
			Asset asset = value;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, new Action<Asset>(OnMaterialLoadComplete));
			_materials.Add(path, value);
		}
		_activeMaterial = value;
	}

	public void SetPreLoadMaterial(string path, Asset matAsset)
	{
		if (!_materials.ContainsKey(path))
		{
			_materials.Add(path, matAsset);
		}
	}

	public void SetPreLoadMaterialLoadFinish(Asset matAsset)
	{
		terrainQuadLayer.SetupMaterial(matAsset.asset as Material);
	}

	private void OnMaterialLoadComplete(Asset request)
	{
		if (request == _activeMaterial)
		{
			terrainQuadLayer.SetupMaterial(request.asset as Material);
		}
	}

	public void SetLodLevel(int lod)
	{
		if (lod <= 2)
		{
			terrainQuadLayer.showMipmapLevel = 0;
		}
		else if (lod <= 4)
		{
			terrainQuadLayer.showMipmapLevel = 1;
		}
		else if (lod < 6)
		{
			terrainQuadLayer.showMipmapLevel = 2;
		}
	}

	public int RectBias()
	{
		return BLOCK_SIZE / 4;
	}

	public bool BlockChanged(Vector2Int lb, Vector2Int rt)
	{
		lb.x = (int)((float)lb.x * terrainQuadLayer.scaler);
		lb.y = (int)((float)lb.y * terrainQuadLayer.scaler);
		rt.x = (int)((float)rt.x * terrainQuadLayer.scaler);
		rt.y = (int)((float)rt.y * terrainQuadLayer.scaler);
		int x = ((lb.x < 0) ? ((lb.x - BLOCK_SIZE) / BLOCK_SIZE) : (lb.x / BLOCK_SIZE));
		int y = ((lb.y < 0) ? ((lb.y - BLOCK_SIZE) / BLOCK_SIZE) : (lb.y / BLOCK_SIZE));
		int x2 = ((rt.x < 0) ? ((rt.x - BLOCK_SIZE) / BLOCK_SIZE) : (rt.x / BLOCK_SIZE));
		int y2 = ((rt.y < 0) ? ((rt.y - BLOCK_SIZE) / BLOCK_SIZE) : (rt.y / BLOCK_SIZE));
		Vector2Int vector2Int = new Vector2Int(x, y);
		Vector2Int vector2Int2 = new Vector2Int(x2, y2);
		if (vector2Int != _lastBlockLB || vector2Int2 != _lastBlockRT)
		{
			return true;
		}
		return false;
	}

	public void UpdateView(Vector2Int lb, Vector2Int rt)
	{
		lb.x = (int)((float)lb.x * terrainQuadLayer.scaler);
		lb.y = (int)((float)lb.y * terrainQuadLayer.scaler);
		rt.x = (int)((float)rt.x * terrainQuadLayer.scaler);
		rt.y = (int)((float)rt.y * terrainQuadLayer.scaler);
		int num = ((lb.x < 0) ? ((lb.x - BLOCK_SIZE) / BLOCK_SIZE) : (lb.x / BLOCK_SIZE));
		int num2 = ((lb.y < 0) ? ((lb.y - BLOCK_SIZE) / BLOCK_SIZE) : (lb.y / BLOCK_SIZE));
		int num3 = ((rt.x < 0) ? ((rt.x - BLOCK_SIZE) / BLOCK_SIZE) : (rt.x / BLOCK_SIZE));
		int num4 = ((rt.y < 0) ? ((rt.y - BLOCK_SIZE) / BLOCK_SIZE) : (rt.y / BLOCK_SIZE));
		Vector2Int lastBlockLB = new Vector2Int(num, num2);
		Vector2Int lastBlockRT = new Vector2Int(num3, num4);
		int num5 = num;
		int num6 = num3;
		int num7 = num4;
		_toLoadQueue.Clear();
		for (int i = num2; i <= num7; i++)
		{
			for (int j = num5; j <= num6; j++)
			{
				Vector2Int item = new Vector2Int(j, i);
				if (!_processQueue1.Contains(item))
				{
					_toLoadQueue.Add(item);
				}
				_processQueue1.Add(item);
			}
		}
		if (_toLoadQueue.Count > 0)
		{
			foreach (Vector2Int item2 in _toLoadQueue)
			{
				AddBlock(item2);
			}
			terrainQuadLayer.Rebuild();
		}
		_lastBlockLB = lastBlockLB;
		_lastBlockRT = lastBlockRT;
		terrainQuadLayer.SetViewRect(_lastBlockLB, _lastBlockRT);
	}

	public void UpdateBlocks(int xMin, int yMin, int xMax, int yMax)
	{
		xMin = (int)((float)xMin * terrainQuadLayer.scaler);
		yMin = (int)((float)yMin * terrainQuadLayer.scaler);
		xMax = (int)((float)xMax * terrainQuadLayer.scaler);
		yMax = (int)((float)yMax * terrainQuadLayer.scaler);
		int num = xMin / BLOCK_SIZE;
		int num2 = yMin / BLOCK_SIZE;
		int num3 = xMax / BLOCK_SIZE;
		int num4 = yMax / BLOCK_SIZE;
		for (int i = num2; i <= num4; i++)
		{
			for (int j = num; j <= num3; j++)
			{
				Vector2Int vector2Int = new Vector2Int(j, i);
				if (_processQueue1.Contains(vector2Int))
				{
					UpdateBlock(vector2Int);
				}
			}
		}
		terrainQuadLayer.Rebuild();
	}

	public void AddBlock(Vector2Int block)
	{
		for (int i = 0; i < BLOCK_SIZE; i++)
		{
			for (int j = 0; j < BLOCK_SIZE; j++)
			{
				int x = j + block.x * BLOCK_SIZE;
				int y = i + block.y * BLOCK_SIZE;
				terrainQuadLayer.AddMipmap0(x, y);
			}
		}
		terrainQuadLayer.UpdateMipmaps(block);
	}

	public void RemoveBlock(Vector2Int block)
	{
		for (int i = 0; i < BLOCK_SIZE; i++)
		{
			for (int j = 0; j < BLOCK_SIZE; j++)
			{
				int x = j + block.x * BLOCK_SIZE;
				int y = i + block.y * BLOCK_SIZE;
				terrainQuadLayer.RemoveMipmap0(x, y);
			}
		}
		terrainQuadLayer.RemoveMipmaps(block);
	}

	public void UpdateBlock(Vector2Int block)
	{
		for (int i = 0; i < BLOCK_SIZE; i++)
		{
			for (int j = 0; j < BLOCK_SIZE; j++)
			{
				int x = j + block.x * BLOCK_SIZE;
				int y = i + block.y * BLOCK_SIZE;
				terrainQuadLayer.ChangeMipmap0(x, y);
			}
		}
		terrainQuadLayer.UpdateMipmaps(block);
	}

	public void DrawGizmos()
	{
	}
}
