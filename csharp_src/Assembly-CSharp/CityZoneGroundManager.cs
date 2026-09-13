using System.Collections.Generic;
using UnityEngine;
using XLua;

public class CityZoneGroundManager : CityManagerBase
{
	private enum EdgeDirectionType
	{
		None,
		TopLeft,
		Top,
		TopRight,
		Right,
		BottomRight,
		Bottom,
		BottomLeft,
		Left
	}

	private class CityZoneGroundSubMesh
	{
		private Vector3[] _vertices = new Vector3[4];

		private Vector2[] _uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};

		private int[] _triangles = new int[6] { 0, 1, 3, 3, 1, 2 };

		public int zoneId { get; private set; }

		public Vector3 landPos { get; private set; }

		public int edgeDirectionState { get; private set; }

		public Dictionary<int, CityZoneGroundEdgeSubMesh> edgeSubMeshMap { get; private set; }

		public void SetMeshData(int id, Vector3 pos, int edgeStage)
		{
			zoneId = id;
			if (landPos != pos)
			{
				landPos = pos;
				float num = 6f;
				Vector3 vector = new Vector3(landPos.x - num, 0f, landPos.z - num);
				Vector3 vector2 = new Vector3(landPos.x - num, 0f, landPos.z + num);
				Vector3 vector3 = new Vector3(landPos.x + num, 0f, landPos.z + num);
				Vector3 vector4 = new Vector3(landPos.x + num, 0f, landPos.z - num);
				_vertices[0] = vector;
				_vertices[1] = vector2;
				_vertices[2] = vector3;
				_vertices[3] = vector4;
			}
			if (edgeDirectionState != edgeStage)
			{
				edgeDirectionState = edgeStage;
				edgeSubMeshMap = edgeSubMeshMap ?? new Dictionary<int, CityZoneGroundEdgeSubMesh>();
				UpdateEdgeData();
			}
		}

		public void FillMeshData(int index, List<Vector3> vertices, List<Vector2> uv, List<int> triangles)
		{
			for (int i = 0; i < 4; i++)
			{
				vertices.Add(_vertices[i]);
				uv.Add(_uv[i]);
			}
			for (int j = 0; j < _triangles.Length; j++)
			{
				triangles.Add(index * 4 + _triangles[j]);
			}
		}

		private void UpdateEdgeData()
		{
			UpdateEdgeSubMeshData(EdgeDirectionType.TopLeft);
			UpdateEdgeSubMeshData(EdgeDirectionType.Top);
			UpdateEdgeSubMeshData(EdgeDirectionType.TopRight);
			UpdateEdgeSubMeshData(EdgeDirectionType.Right);
			UpdateEdgeSubMeshData(EdgeDirectionType.BottomRight);
			UpdateEdgeSubMeshData(EdgeDirectionType.Bottom);
			UpdateEdgeSubMeshData(EdgeDirectionType.BottomLeft);
			UpdateEdgeSubMeshData(EdgeDirectionType.Left);
		}

		private void UpdateEdgeSubMeshData(EdgeDirectionType dir)
		{
			int num = 1 << (int)dir;
			if ((edgeDirectionState & num) != 0)
			{
				if (!edgeSubMeshMap.TryGetValue((int)dir, out var value))
				{
					value = new CityZoneGroundEdgeSubMesh();
					edgeSubMeshMap.Add((int)dir, value);
				}
				Vector3 edgePos = GetEdgePos(dir);
				value.SetMeshData(zoneId, edgePos, dir);
			}
			else if (edgeSubMeshMap.ContainsKey((int)dir))
			{
				edgeSubMeshMap.Remove((int)dir);
			}
		}

		private Vector3 GetEdgePos(EdgeDirectionType dir)
		{
			return dir switch
			{
				EdgeDirectionType.TopLeft => new Vector3(landPos.x - 12f, 0f, landPos.z + 12f), 
				EdgeDirectionType.Top => new Vector3(landPos.x, 0f, landPos.z + 12f), 
				EdgeDirectionType.TopRight => new Vector3(landPos.x + 12f, 0f, landPos.z + 12f), 
				EdgeDirectionType.Right => new Vector3(landPos.x + 12f, 0f, landPos.z), 
				EdgeDirectionType.BottomRight => new Vector3(landPos.x + 12f, 0f, landPos.z - 12f), 
				EdgeDirectionType.Bottom => new Vector3(landPos.x, 0f, landPos.z - 12f), 
				EdgeDirectionType.BottomLeft => new Vector3(landPos.x - 12f, 0f, landPos.z - 12f), 
				EdgeDirectionType.Left => new Vector3(landPos.x - 12f, 0f, landPos.z), 
				_ => Vector3.zero, 
			};
		}
	}

	private class CityZoneGroundEdgeSubMesh
	{
		private Vector3[] _vertices = new Vector3[4];

		private Vector2[] _uv;

		private int[] _triangles = new int[6] { 0, 1, 3, 3, 1, 2 };

		public int zoneId { get; private set; }

		public Vector3 landPos { get; private set; }

		public EdgeDirectionType edgeDir { get; private set; }

		public void SetMeshData(int zoneId, Vector3 pos, EdgeDirectionType dir)
		{
			this.zoneId = zoneId;
			if (landPos != pos)
			{
				landPos = pos;
				float num = 6f;
				Vector3 vector = new Vector3(landPos.x - num, 0f, landPos.z - num);
				Vector3 vector2 = new Vector3(landPos.x - num, 0f, landPos.z + num);
				Vector3 vector3 = new Vector3(landPos.x + num, 0f, landPos.z + num);
				Vector3 vector4 = new Vector3(landPos.x + num, 0f, landPos.z - num);
				_vertices[0] = vector;
				_vertices[1] = vector2;
				_vertices[2] = vector3;
				_vertices[3] = vector4;
			}
			if (edgeDir != dir)
			{
				edgeDir = dir;
				EdgeDirectionType dirType = dir;
				if (zoneId == 1 && dir == EdgeDirectionType.Bottom)
				{
					dirType = EdgeDirectionType.BottomRight;
				}
				else if (zoneId == 3 && dir == EdgeDirectionType.Bottom)
				{
					dirType = EdgeDirectionType.BottomLeft;
				}
				_uv = GetUVData(dirType);
			}
		}

		public void FillMeshData(int index, List<Vector3> vertices, List<Vector2> uv, List<int> triangles)
		{
			for (int i = 0; i < 4; i++)
			{
				vertices.Add(_vertices[i]);
				uv.Add(_uv[i]);
			}
			for (int j = 0; j < _triangles.Length; j++)
			{
				triangles.Add(index * 4 + _triangles[j]);
			}
		}

		private Vector2[] GetUVData(EdgeDirectionType dirType)
		{
			return dirType switch
			{
				EdgeDirectionType.TopLeft => new Vector2[4]
				{
					new Vector2(0.0068f, 0.0214f),
					new Vector2(0.0068f, 0.3398f),
					new Vector2(0.3232f, 0.3398f),
					new Vector2(0.3232f, 0.0214f)
				}, 
				EdgeDirectionType.Top => new Vector2[4]
				{
					new Vector2(0.0117f, 0.371f),
					new Vector2(0.0117f, 0.6894f),
					new Vector2(0.3232f, 0.6894f),
					new Vector2(0.3232f, 0.371f)
				}, 
				EdgeDirectionType.TopRight => new Vector2[4]
				{
					new Vector2(0.3232f, 0.0214f),
					new Vector2(0.0068f, 0.0214f),
					new Vector2(0.0068f, 0.3398f),
					new Vector2(0.3232f, 0.3398f)
				}, 
				EdgeDirectionType.Right => new Vector2[4]
				{
					new Vector2(0.3232f, 0.371f),
					new Vector2(0.0117f, 0.371f),
					new Vector2(0.0117f, 0.6894f),
					new Vector2(0.3232f, 0.6894f)
				}, 
				EdgeDirectionType.BottomRight => new Vector2[4]
				{
					new Vector2(0.3232f, 0.3398f),
					new Vector2(0.3232f, 0.0214f),
					new Vector2(0.0068f, 0.0214f),
					new Vector2(0.0068f, 0.3398f)
				}, 
				EdgeDirectionType.Bottom => new Vector2[4]
				{
					new Vector2(0.9863f, 0.6855f),
					new Vector2(0.9863f, 0.3681f),
					new Vector2(0.6718f, 0.3681f),
					new Vector2(0.6718f, 0.6855f)
				}, 
				EdgeDirectionType.BottomLeft => new Vector2[4]
				{
					new Vector2(0.0068f, 0.3398f),
					new Vector2(0.3232f, 0.3398f),
					new Vector2(0.3232f, 0.0214f),
					new Vector2(0.0068f, 0.0214f)
				}, 
				EdgeDirectionType.Left => new Vector2[4]
				{
					new Vector2(0.0117f, 0.6894f),
					new Vector2(0.3232f, 0.6894f),
					new Vector2(0.3232f, 0.371f),
					new Vector2(0.0117f, 0.371f)
				}, 
				_ => new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(0f, 1f),
					new Vector2(1f, 1f),
					new Vector2(1f, 0f)
				}, 
			};
		}
	}

	public const string GROUND_PATH = "Assets/Main/Prefabs/City/Zone/CityZoneGroundCell.prefab";

	public const string EDGE_GROUND_PATH = "Assets/Main/Prefabs/City/Zone/CityZoneGroundEdge.prefab";

	public const string EDGE_GROUND_SEASON_PATH = "Assets/Main/Prefabs/City/Season2/CityZoneGroundEdge_swsj.prefab";

	public const string EDGE_GROUND_SEASON_S5_PATH = "Assets/Main/SeasonRes/S5/Prefabs/Building/neicheng/CityZoneGroundEdge_s5.prefab";

	public const float GROUND_WIDTH = 12f;

	private Transform _parent;

	private bool _inSnowSeason;

	private bool _inSeasonS5;

	private CityZoneGround _ground;

	private List<Vector3> _vertices = new List<Vector3>();

	private List<Vector2> _uv = new List<Vector2>();

	private List<int> _triangles = new List<int>();

	private CityZoneGround _edgeGround;

	private List<Vector3> _edgeVertices = new List<Vector3>();

	private List<Vector2> _edgeUv = new List<Vector2>();

	private List<int> _edgeTriangles = new List<int>();

	private Dictionary<int, CityZoneGroundSubMesh> _subMeshMap = new Dictionary<int, CityZoneGroundSubMesh>();

	public CityZoneGroundManager(CityScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		GameEntry.Event.Subscribe(EventId.InitShowCityZoneGround, OnInitShowCityZoneGround);
		GameEntry.Event.Subscribe(EventId.UpdateShowCityZoneGround, OnUpdateShowCityZoneGround);
		GameEntry.Event.Subscribe(EventId.DestroyCityZoneGround, OnDestroyCityZoneGround);
	}

	public override void UnInit()
	{
		GameEntry.Event.Unsubscribe(EventId.InitShowCityZoneGround, OnInitShowCityZoneGround);
		GameEntry.Event.Unsubscribe(EventId.UpdateShowCityZoneGround, OnUpdateShowCityZoneGround);
		GameEntry.Event.Unsubscribe(EventId.DestroyCityZoneGround, OnDestroyCityZoneGround);
		UnloadGroundMesh();
		UnloadGroundEdgeMesh();
		_subMeshMap.Clear();
	}

	private void OnInitShowCityZoneGround(object data)
	{
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		_inSnowSeason = curSkinMeta?.IsSnowMode() ?? false;
		_inSeasonS5 = curSkinMeta?.IsNineNationMode() ?? false;
		LuaTable luaTable = (LuaTable)data;
		if (luaTable == null)
		{
			return;
		}
		_subMeshMap.Clear();
		if (luaTable.ContainsKey("parent"))
		{
			_parent = luaTable.Get<Transform>("parent");
		}
		if (luaTable.ContainsKey("landLockDataDict"))
		{
			luaTable.Get<LuaTable>("landLockDataDict").ForEach(delegate(int _, LuaTable landData)
			{
				if (landData.ContainsKey("zoneId") && landData.ContainsKey("pos") && landData.ContainsKey("edgeDirectionState"))
				{
					int zoneId = landData.Get<int>("zoneId");
					Vector3 landPos = landData.Get<Vector3>("pos");
					int edgeDirectionState = landData.Get<int>("edgeDirectionState");
					UpdateSubMesh(zoneId, landPos, edgeDirectionState);
				}
			});
		}
		RebuildMesh();
	}

	private void OnUpdateShowCityZoneGround(object data)
	{
		LuaTable luaTable = (LuaTable)data;
		if (luaTable == null)
		{
			return;
		}
		luaTable.ForEach(delegate(int _, LuaTable landData)
		{
			if (landData.ContainsKey("zoneId") && landData.ContainsKey("pos") && landData.ContainsKey("edgeDirectionState"))
			{
				int zoneId = landData.Get<int>("zoneId");
				Vector3 landPos = landData.Get<Vector3>("pos");
				int edgeDirectionState = landData.Get<int>("edgeDirectionState");
				UpdateSubMesh(zoneId, landPos, edgeDirectionState);
			}
		});
		RebuildMesh();
	}

	private void OnDestroyCityZoneGround(object data)
	{
		UnloadGroundMesh();
		UnloadGroundEdgeMesh();
		_subMeshMap.Clear();
	}

	private void UpdateSubMesh(int zoneId, Vector3 landPos, int edgeDirectionState)
	{
		if (!_subMeshMap.TryGetValue(zoneId, out var value))
		{
			value = new CityZoneGroundSubMesh();
			_subMeshMap.Add(zoneId, value);
		}
		value.SetMeshData(zoneId, landPos, edgeDirectionState);
	}

	private void RebuildMesh()
	{
		int num = 0;
		int num2 = 0;
		_triangles.Clear();
		_vertices.Clear();
		_uv.Clear();
		_edgeTriangles.Clear();
		_edgeVertices.Clear();
		_edgeUv.Clear();
		if (_subMeshMap.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<int, CityZoneGroundSubMesh> item in _subMeshMap)
		{
			item.Value.FillMeshData(num, _vertices, _uv, _triangles);
			num++;
			if (item.Value.edgeSubMeshMap == null)
			{
				continue;
			}
			foreach (KeyValuePair<int, CityZoneGroundEdgeSubMesh> item2 in item.Value.edgeSubMeshMap)
			{
				item2.Value.FillMeshData(num2, _edgeVertices, _edgeUv, _edgeTriangles);
				num2++;
			}
		}
		ShowGroundMesh();
		ShowGroundEdgeMesh();
	}

	private void ShowGroundMesh()
	{
		if (_ground == null)
		{
			_ground = new CityZoneGround();
			_ground.InitGroundData(_parent, "Assets/Main/Prefabs/City/Zone/CityZoneGroundCell.prefab");
		}
		if (_vertices.Count > 0)
		{
			_ground.ShowMesh(_vertices, _uv, _triangles);
		}
	}

	private void UnloadGroundMesh()
	{
		if (_ground != null)
		{
			_ground.UnloadGround();
			_ground = null;
		}
		_triangles.Clear();
		_vertices.Clear();
		_uv.Clear();
	}

	private void ShowGroundEdgeMesh()
	{
		if (_edgeGround == null)
		{
			_edgeGround = new CityZoneGround();
		}
		string path = (_inSnowSeason ? "Assets/Main/Prefabs/City/Season2/CityZoneGroundEdge_swsj.prefab" : ((!_inSeasonS5) ? "Assets/Main/Prefabs/City/Zone/CityZoneGroundEdge.prefab" : "Assets/Main/SeasonRes/S5/Prefabs/Building/neicheng/CityZoneGroundEdge_s5.prefab"));
		_edgeGround.InitGroundData(_parent, path);
		if (_edgeVertices.Count > 0)
		{
			_edgeGround.ShowMesh(_edgeVertices, _edgeUv, _edgeTriangles);
		}
	}

	private void UnloadGroundEdgeMesh()
	{
		if (_edgeGround != null)
		{
			_edgeGround.UnloadGround();
			_edgeGround = null;
		}
		_edgeTriangles.Clear();
		_edgeVertices.Clear();
		_edgeUv.Clear();
	}
}
