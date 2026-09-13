using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameFramework;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class TerrainQuadSeason<T> : ITerrainQuadLayer
{
	protected class _MatrixPoolItem
	{
		public Matrix4x4[] matrix = new Matrix4x4[511];

		public int length;
	}

	protected class _MipMap
	{
		public int level;

		public int quadCellWidth;

		public int yOffset;

		public Mesh mesh;

		public Dictionary<int, T> data;
	}

	public delegate T GetStateByXY(int x, int y);

	protected const int MASK = 65535;

	private int _maxX;

	private int _maxY;

	protected int _quadSize;

	private Material _cellMaterial;

	protected const int MatrixMaxCount = 511;

	protected List<_MatrixPoolItem> _pool;

	protected int _cursor;

	private bool _hasData;

	private bool __rebuild;

	private int _mipmapCount;

	protected _MipMap[] _mipmaps;

	private int _showMipmapLevel;

	protected Vector2Int _lastBlockLB;

	protected Vector2Int _lastBlockRT;

	private GetStateByXY _getStateByXY;

	public string passId { get; protected set; }

	public virtual int blockSize { get; protected set; }

	public virtual int minCellSize { get; protected set; }

	public float scaler { get; protected set; }

	public float invScaler { get; protected set; }

	public int showMipmapLevel
	{
		get
		{
			return _showMipmapLevel;
		}
		set
		{
			if (_showMipmapLevel != value && value >= 0 && value < _mipmapCount)
			{
				_showMipmapLevel = value;
				Rebuild();
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Rebuild()
	{
		__rebuild = true;
	}

	public void SetGetStateFunc(GetStateByXY func)
	{
		_getStateByXY = func;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected virtual T DefaultStateValue()
	{
		return default(T);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private T __GetStateByXY(int x, int y)
	{
		x = (int)((float)x * invScaler);
		y = (int)((float)y * invScaler);
		if (_getStateByXY == null)
		{
			return DefaultStateValue();
		}
		return _getStateByXY(x, y);
	}

	public void Setup(int quadSize, int mipmapCount)
	{
		BeforeSetup();
		scaler = 2f / (float)minCellSize;
		invScaler = 1f / scaler;
		_maxX = (int)(1000f * scaler);
		_maxY = (int)(1000f * scaler);
		_quadSize = quadSize;
		_mipmapCount = ((mipmapCount < 1) ? 1 : mipmapCount);
		_pool = new List<_MatrixPoolItem>();
		_cursor = -1;
		_hasData = false;
		__rebuild = false;
		_mipmaps = new _MipMap[_mipmapCount];
		for (int i = 0; i < _mipmapCount; i++)
		{
			_MipMap mipMap = new _MipMap();
			mipMap.level = i;
			mipMap.quadCellWidth = ((i == 0) ? minCellSize : (_mipmaps[i - 1].quadCellWidth * 2));
			mipMap.yOffset = 0;
			mipMap.mesh = CreateQuadBase(mipMap.quadCellWidth);
			mipMap.data = new Dictionary<int, T>();
			_mipmaps[i] = mipMap;
		}
		showMipmapLevel = 0;
		AfterSetup();
	}

	protected virtual void BeforeSetup()
	{
	}

	protected virtual void AfterSetup()
	{
	}

	public void Clear()
	{
		_getStateByXY = null;
		ClearMipmaps();
	}

	public void SetupMaterial(Material material)
	{
		_cellMaterial = material;
	}

	public void SetViewRect(Vector2Int lb, Vector2Int rt)
	{
		_lastBlockLB = lb;
		_lastBlockRT = rt;
		_hasData = true;
		Rebuild();
	}

	public virtual void SetTransit()
	{
	}

	public void AddMipmap0(int x, int y)
	{
		int key = (x << 16) | (y & 0xFFFF);
		T value = DefaultStateValue();
		if (x >= 0 && x <= _maxX && y >= 0 && y <= _maxY)
		{
			value = __GetStateByXY(x, y);
		}
		_mipmaps[0].data[key] = value;
	}

	public void ChangeMipmap0(int x, int y)
	{
		int num = (x << 16) | (y & 0xFFFF);
		T current = DefaultStateValue();
		if (x >= 0 && x <= _maxX && y >= 0 && y <= _maxY)
		{
			current = __GetStateByXY(x, y);
		}
		T last = _mipmaps[0].data[num];
		OnChangeMipmap0(num, ref last, ref current);
		_mipmaps[0].data[num] = current;
	}

	protected virtual void OnChangeMipmap0(int index, ref T last, ref T current)
	{
	}

	public void RemoveMipmap0(int x, int y)
	{
		int key = (x << 16) | (y & 0xFFFF);
		_mipmaps[0].data.Remove(key);
	}

	public void UpdateMipmaps(Vector2Int block)
	{
		int num = blockSize;
		for (int i = 1; i < _mipmapCount; i++)
		{
			num /= 2;
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < num; k++)
				{
					int num2 = k + block.x * num;
					int num3 = j + block.y * num;
					int key = (num2 << 16) | (num3 & 0xFFFF);
					_mipmaps[i].data[key] = CalcMipmap(num2, num3, i);
				}
			}
		}
	}

	protected abstract T CalcMipmap(int xPos, int yPos, int mipmapLevel);

	public void RemoveMipmaps(Vector2Int block)
	{
		int num = blockSize;
		for (int i = 1; i < _mipmapCount; i++)
		{
			num /= 2;
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < num; k++)
				{
					int num2 = k + block.x * num;
					int num3 = j + block.y * num;
					int key = (num2 << 16) | (num3 & 0xFFFF);
					_mipmaps[i].data.Remove(key);
				}
			}
		}
	}

	public void ClearMipmaps()
	{
		for (int i = 0; i < _mipmapCount; i++)
		{
			_mipmaps[i].data.Clear();
		}
		int j = 0;
		for (int count = _pool.Count; j < count; j++)
		{
			_pool[j].length = 0;
		}
		_cursor = -1;
		__rebuild = false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected T GetMipmapData(int x, int y, int level)
	{
		T result = DefaultStateValue();
		if (x >= -32768 && x <= 32767 && y >= -32768 && y <= 32767)
		{
			int key = (x << 16) | (y & 0xFFFF);
			if (_mipmaps[level].data.TryGetValue(key, out var value))
			{
				result = value;
			}
		}
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected T GetMipmapData(int x, int y, _MipMap mipmap)
	{
		T result = DefaultStateValue();
		if (x >= -32768 && x <= 32767 && y >= -32768 && y <= 32767)
		{
			int key = (x << 16) | (y & 0xFFFF);
			if (mipmap.data.TryGetValue(key, out var value))
			{
				result = value;
			}
		}
		return result;
	}

	public bool Ready()
	{
		if (_cellMaterial == null)
		{
			return false;
		}
		if (!_hasData)
		{
			return false;
		}
		return true;
	}

	public bool Prepare()
	{
		if (_cellMaterial == null)
		{
			return false;
		}
		if (!_hasData)
		{
			return false;
		}
		try
		{
			if (__rebuild || InTransit())
			{
				SetPoolCursor(0);
				BuildItems();
				__rebuild = false;
				return true;
			}
		}
		catch (Exception message)
		{
			_cursor = -1;
			__rebuild = false;
			Log.Error(message);
		}
		return false;
	}

	protected virtual bool InTransit()
	{
		return false;
	}

	protected virtual void BuildItems()
	{
	}

	protected void SetPoolCursor(int cursor)
	{
		_cursor = cursor;
		if (_cursor >= _pool.Count)
		{
			_pool.Add(new _MatrixPoolItem());
		}
		_pool[_cursor].length = 0;
	}

	public void DrawNoise(CommandBuffer cmd)
	{
		if (_cellMaterial.passCount < 2)
		{
			Log.Error($"TerrainQuadSeason DrawNoise passCount < 2, {passId}, {_cellMaterial.name}, {_cellMaterial.shader}.");
			return;
		}
		_MipMap mipMap = _mipmaps[showMipmapLevel];
		for (int i = 0; i < _cursor + 1; i++)
		{
			_MatrixPoolItem matrixPoolItem = _pool[i];
			cmd.DrawMeshInstanced(mipMap.mesh, 0, _cellMaterial, 1, matrixPoolItem.matrix, matrixPoolItem.length);
		}
	}

	public void DrawLayers(CommandBuffer cmd)
	{
		_MipMap mipMap = _mipmaps[showMipmapLevel];
		for (int i = 0; i < _cursor + 1; i++)
		{
			_MatrixPoolItem matrixPoolItem = _pool[i];
			cmd.DrawMeshInstanced(mipMap.mesh, 0, _cellMaterial, 0, matrixPoolItem.matrix, matrixPoolItem.length);
		}
	}

	public void DrawQuads(CommandBuffer cmd)
	{
		_MipMap mipMap = _mipmaps[showMipmapLevel];
		for (int i = 0; i < _cursor + 1; i++)
		{
			_MatrixPoolItem matrixPoolItem = _pool[i];
			for (int j = 0; j < matrixPoolItem.length; j++)
			{
				cmd.DrawMesh(mipMap.mesh, matrixPoolItem.matrix[j], _cellMaterial, 0, 2);
			}
		}
	}

	private void AddQuad(List<Vector3> vertices, List<Vector2> uvs, List<int> triangles, Vector2 posMin, Vector2 posMax, Vector2 uvMin, Vector2 uvMax)
	{
		int count = vertices.Count;
		vertices.Add(new Vector3(posMin.x, 0f, posMin.y));
		vertices.Add(new Vector3(posMax.x, 0f, posMin.y));
		vertices.Add(new Vector3(posMin.x, 0f, posMax.y));
		vertices.Add(new Vector3(posMax.x, 0f, posMax.y));
		uvs.Add(new Vector2(uvMin.x, uvMin.y));
		uvs.Add(new Vector2(uvMax.x, uvMin.y));
		uvs.Add(new Vector2(uvMin.x, uvMax.y));
		uvs.Add(new Vector2(uvMax.x, uvMax.y));
		triangles.Add(count);
		triangles.Add(count + 3);
		triangles.Add(count + 1);
		triangles.Add(count + 3);
		triangles.Add(count);
		triangles.Add(count + 2);
	}

	private Mesh CreateQuadBase(int width)
	{
		Mesh mesh = new Mesh();
		Vector2 posMin = new Vector2(0f, 0f);
		Vector2 posMax = new Vector2(width, width);
		List<Vector3> vertices = new List<Vector3>(4);
		List<Color> list = new List<Color>(4);
		List<Vector2> uvs = new List<Vector2>(4);
		List<int> list2 = new List<int>(4);
		AddQuad(vertices, uvs, list2, posMin, posMax, new Vector2(0f, 0f), new Vector2(1f, 1f));
		list.Add(new Color(1f, 0f, 0f, 0f));
		list.Add(new Color(0f, 1f, 0f, 0f));
		list.Add(new Color(0f, 0f, 1f, 0f));
		list.Add(new Color(0f, 0f, 0f, 1f));
		mesh.SetVertices(vertices);
		mesh.SetColors(list);
		mesh.SetUVs(0, uvs);
		mesh.SetIndices(list2, MeshTopology.Triangles, 0);
		return mesh;
	}

	private void AddQuadColor(List<Color> colors, Color color)
	{
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}

	public void DrawGizmos()
	{
		GUIStyle style = new GUIStyle
		{
			normal = new GUIStyleState
			{
				textColor = Color.black
			},
			alignment = TextAnchor.MiddleCenter
		};
		for (int i = 0; i < _mipmapCount; i++)
		{
		}
		try
		{
			DrawMipmapGizmos(_mipmaps[showMipmapLevel], style);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	protected virtual void DrawMipmapGizmos(_MipMap mipMap, GUIStyle style)
	{
	}
}
