using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using VEngine;

public class WorldTroopLine : MonoBehaviour
{
	private class PathMesh
	{
		public enum MeshType
		{
			Home,
			Path,
			Drag
		}

		private class SubMesh
		{
			public bool enable;

			private const float WIDTH = 0.3f;

			private Vector3[] _v = new Vector3[4];

			private Vector2[] _uv = new Vector2[4];

			private Color[] _col = new Color[4];

			private float _widthScale = 1f;

			public static int[] triangles = new int[6] { 0, 2, 3, 0, 3, 1 };

			private Vector3 _cacheFrom;

			private Vector3 _cacheTo;

			private Color _cacheColor;

			private float _cacheWidth = -1f;

			public bool dirty { get; private set; }

			public MeshType type { get; private set; }

			public SubMesh(MeshType type)
			{
				this.type = type;
				_widthScale = 1f;
			}

			public void SetWidthScale(float w)
			{
				_widthScale = w;
				dirty = true;
			}

			public void SetMesh(bool mEnable, Vector3 from, Vector3 to)
			{
				if (enable != mEnable || !(_cacheFrom == from) || !(_cacheTo == to) || !Mathf.Approximately(_cacheWidth, _widthScale))
				{
					enable = mEnable;
					if (!enable)
					{
						dirty = true;
						return;
					}
					_cacheFrom = from;
					_cacheTo = to;
					Vector3 normalized = Vector3.Cross(to - from, Vector3.up).normalized;
					_v[0] = from + normalized * (0.3f * _widthScale / 2f);
					_v[1] = from - normalized * (0.3f * _widthScale / 2f);
					_v[2] = to + normalized * (0.3f * _widthScale / 2f);
					_v[3] = to - normalized * (0.3f * _widthScale / 2f);
					_cacheWidth = _widthScale;
					float x = Vector3.Distance(from, to) / 4f / (0.3f * _widthScale);
					_uv[0] = new Vector2(0f, 1f);
					_uv[1] = new Vector2(0f, 0f);
					_uv[2] = new Vector2(x, 1f);
					_uv[3] = new Vector2(x, 0f);
					dirty = true;
				}
			}

			public void SetColor(Color col)
			{
				if (!(_cacheColor == col))
				{
					_cacheColor = col;
					_col[0] = col;
					_col[1] = col;
					_col[2] = col;
					_col[3] = col;
					dirty = true;
				}
			}

			public void Fill(int index, List<Vector3> vertices, List<Vector2> uv, List<Color> color, List<int> t)
			{
				for (int i = 0; i < 4; i++)
				{
					vertices.Add(_v[i]);
					uv.Add(_uv[i]);
					color.Add(_col[i]);
				}
				for (int j = 0; j < triangles.Length; j++)
				{
					t.Add(index * 4 + triangles[j]);
				}
				dirty = false;
			}
		}

		private Dictionary<MeshType, SubMesh> _subMesh = new Dictionary<MeshType, SubMesh>();

		private Dictionary<MeshType, Color> _tmpColor = new Dictionary<MeshType, Color>();

		private bool _dirty = true;

		private static SimplePool<Mesh> _pool = new SimplePool<Mesh>(null, null);

		private Mesh _mesh;

		private List<Vector3> _vertices = new List<Vector3>();

		private List<Vector2> _uv = new List<Vector2>();

		private List<Color> _color = new List<Color>();

		private List<int> _triangles = new List<int>();

		private static Dictionary<int, Material> _speedGearMat = new Dictionary<int, Material>();

		private static Material _templateMat;

		private const float DEFAULT_SPEED = 0.775f;

		private float _curSpeed = 0.775f;

		private Material _useMat;

		private bool _isUseMatNotNull;

		private static bool _isTemplateMatNotNull = false;

		private float _widthScale = 1f;

		public PathMesh()
		{
			_mesh = _pool.Get();
			if (!_isTemplateMatNotNull)
			{
				Asset matReq = GameEntry.Resource.LoadAssetAsync("Assets/Main/Material/TroopLineDrag 2.mat", typeof(Material));
				Asset asset = matReq;
				asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
				{
					Material templateMat = matReq.asset as Material;
					if (!_isTemplateMatNotNull)
					{
						_templateMat = templateMat;
						_isTemplateMatNotNull = true;
					}
					UpdateSpeed(_curSpeed);
				});
			}
			else
			{
				UpdateSpeed(_curSpeed);
			}
		}

		public void SetWidthScale(float s)
		{
			_widthScale = s;
			foreach (KeyValuePair<MeshType, SubMesh> item in _subMesh)
			{
				item.Value.SetWidthScale(s);
			}
		}

		public void UpdateSubMesh(MeshType type, bool enable, Vector3 from, Vector3 to)
		{
			if (!_subMesh.TryGetValue(type, out var value))
			{
				value = new SubMesh(type);
				value.SetWidthScale(_widthScale);
				if (_tmpColor.TryGetValue(type, out var value2))
				{
					value.SetColor(value2);
					_tmpColor.Remove(type);
				}
				_subMesh.Add(type, value);
			}
			value.SetMesh(enable, from, to);
			if (value.dirty)
			{
				_dirty = true;
			}
		}

		public void UpdateSubMesh(MeshType type, Color col)
		{
			if (_subMesh.TryGetValue(type, out var value))
			{
				value.SetColor(col);
			}
			else
			{
				_tmpColor[type] = col;
			}
		}

		public void UpdateSpeed(float speed)
		{
			if (!_isTemplateMatNotNull)
			{
				return;
			}
			if (speed <= 0f)
			{
				speed = 0.775f;
			}
			if (!_isUseMatNotNull || !(Math.Abs(_curSpeed - speed) < 0.01f))
			{
				int num = Mathf.RoundToInt(Mathf.Clamp(speed / 0.775f / 2f, 1f, 10f));
				if (!_speedGearMat.TryGetValue(num, out var value))
				{
					Material material = UnityEngine.Object.Instantiate(_templateMat);
					material.SetFloat(_speedKey, num);
					_speedGearMat[num] = material;
					_useMat = material;
				}
				else
				{
					_useMat = value;
				}
				_isUseMatNotNull = true;
				_curSpeed = speed;
			}
		}

		public void Fill()
		{
			if (!_dirty)
			{
				return;
			}
			int num = 0;
			_triangles.Clear();
			_vertices.Clear();
			_uv.Clear();
			_color.Clear();
			_mesh.Clear();
			if (_subMesh.Count > 0)
			{
				foreach (KeyValuePair<MeshType, SubMesh> item in _subMesh)
				{
					if (item.Value.enable)
					{
						item.Value.Fill(num, _vertices, _uv, _color, _triangles);
						num++;
					}
				}
				_mesh.vertices = _vertices.ToArray();
				_mesh.uv = _uv.ToArray();
				_mesh.colors = _color.ToArray();
				_mesh.triangles = _triangles.ToArray();
				_mesh.RecalculateNormals();
				_mesh.RecalculateBounds();
			}
			_dirty = false;
		}

		public void Clear()
		{
			_subMesh.Clear();
			_dirty = true;
		}

		public void GetMesh(out Mesh mesh, out Material mat)
		{
			mesh = _mesh;
			mat = _useMat;
		}

		public void Dispose()
		{
			_pool.Release(_mesh);
			_mesh = null;
			_vertices.Clear();
			_uv.Clear();
			_color.Clear();
			_triangles.Clear();
			_curSpeed = 0.775f;
			_useMat = null;
		}
	}

	[SerializeField]
	private LineRenderer homeLineRenderer;

	[SerializeField]
	private LineRenderer homeLineRenderer2;

	[SerializeField]
	private SpriteRenderer startSprite;

	[SerializeField]
	private Transform startSize;

	[SerializeField]
	private LineRenderer backLineRenderer;

	private bool _backLineRendererValid;

	[SerializeField]
	private LineRenderer backLineRenderer2;

	[SerializeField]
	private SpriteRenderer middleSprite;

	[SerializeField]
	private Transform middleSize;

	private bool _middleSizeValid;

	[SerializeField]
	private LineRenderer frontLineRenderer;

	[SerializeField]
	private LineRenderer frontLineRenderer2;

	private bool _frontLineRendererValid;

	[SerializeField]
	private SpriteRenderer endSprite;

	[SerializeField]
	private Transform endSize;

	public bool canUseMesh;

	[SerializeField]
	private SimpleAnimation simpleAnimation;

	private int pathIndex = -1;

	private Vector3[] dragPath = new Vector3[2];

	private float midCircleRadius;

	[SerializeField]
	private GameObject destEffectObject;

	private Vector3 midCircleOffset;

	private Vector3 homePosition;

	private Vector3 startPosition;

	private Vector3 endPosition;

	private bool initStart;

	private bool initEnd;

	private const bool USE_MESH = true;

	private PathMesh _m_pathMesh;

	private Color _light;

	private Color _dark;

	private static readonly int _speedKey = Shader.PropertyToID("_Speed");

	private bool use_mesh => canUseMesh;

	private PathMesh _pathMesh
	{
		get
		{
			if (_m_pathMesh == null)
			{
				_m_pathMesh = new PathMesh();
			}
			return _m_pathMesh;
		}
	}

	public void OnEnable()
	{
		_middleSizeValid = middleSize != null;
		_backLineRendererValid = backLineRenderer != null;
		_frontLineRendererValid = frontLineRenderer != null;
	}

	public void Update()
	{
		if (use_mesh)
		{
			RenderMesh();
		}
	}

	private void RenderMesh()
	{
		_pathMesh.Fill();
		_pathMesh.GetMesh(out var mesh, out var mat);
		if ((bool)mesh && (bool)mat)
		{
			Graphics.DrawMesh(mesh, Matrix4x4.identity, mat, 0);
		}
	}

	public void Clear()
	{
		if (use_mesh)
		{
			if ((bool)frontLineRenderer)
			{
				frontLineRenderer.gameObject.SetActive(value: false);
			}
			if ((bool)backLineRenderer)
			{
				backLineRenderer.gameObject.SetActive(value: false);
			}
			if ((bool)homeLineRenderer)
			{
				homeLineRenderer.gameObject.SetActive(value: false);
			}
			_pathMesh.Clear();
		}
		else
		{
			if ((bool)frontLineRenderer)
			{
				frontLineRenderer.gameObject.SetActive(value: true);
				frontLineRenderer.positionCount = 0;
			}
			if ((bool)backLineRenderer)
			{
				backLineRenderer.gameObject.SetActive(value: true);
				backLineRenderer.positionCount = 0;
			}
			if ((bool)homeLineRenderer)
			{
				homeLineRenderer.gameObject.SetActive(value: false);
			}
		}
		initStart = false;
		initEnd = false;
	}

	public void SetColor(Color dark, Color light)
	{
		if (startSprite != null)
		{
			startSprite.color = dark;
		}
		if (middleSprite != null)
		{
			middleSprite.color = dark;
		}
		if (endSprite != null)
		{
			endSprite.color = dark;
		}
		if (use_mesh)
		{
			_light = light;
			_dark = dark;
			_pathMesh.UpdateSubMesh(PathMesh.MeshType.Home, light);
			_pathMesh.UpdateSubMesh(PathMesh.MeshType.Path, light);
			_pathMesh.UpdateSubMesh(PathMesh.MeshType.Drag, Color.white);
			return;
		}
		if (frontLineRenderer != null)
		{
			frontLineRenderer.startColor = dark;
			frontLineRenderer.endColor = dark;
		}
		if (homeLineRenderer != null)
		{
			homeLineRenderer.startColor = dark;
			homeLineRenderer.endColor = dark;
		}
		if (backLineRenderer != null)
		{
			backLineRenderer.startColor = light;
			backLineRenderer.endColor = light;
		}
	}

	public void SetWidthScale(float s)
	{
		_pathMesh.SetWidthScale(s);
		if (use_mesh)
		{
			_pathMesh.UpdateSubMesh(PathMesh.MeshType.Path, enable: true, startPosition, endPosition);
		}
	}

	public void SetScale(float start, float mid, float end)
	{
		if (startSize != null)
		{
			startSize.localScale = start * Vector3.one;
		}
		if (middleSize != null)
		{
			middleSize.localScale = mid * Vector3.one;
		}
		if (endSize != null)
		{
			endSize.localScale = end * Vector3.one;
		}
		midCircleRadius = mid * 2f;
	}

	public void SetMidScale(Vector3 scale)
	{
		middleSprite.transform.localScale = scale;
	}

	public void SetRotation(Vector3 dir)
	{
		midCircleOffset = dir * midCircleRadius;
		if (middleSize != null)
		{
			middleSize.transform.localRotation = Quaternion.LookRotation(dir);
		}
	}

	public void InitStart(Vector3 startPos)
	{
		startPosition = startPos;
		initStart = true;
		if (startSize != null)
		{
			startSize.position = startPos;
		}
		if (use_mesh)
		{
			if (initEnd)
			{
				_pathMesh.UpdateSubMesh(PathMesh.MeshType.Path, enable: true, startPos, endPosition);
			}
		}
		else if (backLineRenderer != null)
		{
			backLineRenderer.positionCount = 2;
			backLineRenderer.SetPosition(0, startPos);
		}
	}

	public void InitEnd(Vector3 endPos)
	{
		endPosition = endPos;
		initEnd = true;
		if (endSize != null)
		{
			endSize.position = endPos;
		}
		if (use_mesh)
		{
			if (initStart)
			{
				_pathMesh.UpdateSubMesh(PathMesh.MeshType.Path, enable: true, startPosition, endPosition);
			}
			return;
		}
		if (frontLineRenderer != null)
		{
			frontLineRenderer.positionCount = 2;
			frontLineRenderer.SetPosition(1, endPos);
		}
		if (middleSize != null)
		{
			middleSize.position = new Vector3(-1000f, -1000f, -1000f);
		}
	}

	public void InitHomePath(Vector3 homePos, Vector3 startPos)
	{
		if (use_mesh)
		{
			_pathMesh.UpdateSubMesh(PathMesh.MeshType.Home, enable: true, homePos, startPos);
		}
		else if (homeLineRenderer != null)
		{
			homeLineRenderer.gameObject.SetActive(value: true);
			homeLineRenderer.positionCount = 2;
			homeLineRenderer.SetPosition(0, homePos);
			homeLineRenderer.SetPosition(1, startPos);
		}
	}

	public void UpdateSpeed(float speed)
	{
		if (use_mesh)
		{
			_pathMesh.UpdateSpeed(speed);
		}
	}

	public void UpdatePath(Vector3 curPos)
	{
		if (!initStart || !initEnd)
		{
			return;
		}
		if (_middleSizeValid)
		{
			middleSize.position = curPos;
		}
		if (use_mesh)
		{
			return;
		}
		if (_backLineRendererValid)
		{
			Vector3 vector = curPos - startPosition;
			float num = math.abs(vector.x) + math.abs(vector.z);
			backLineRenderer.positionCount = 2;
			if ((double)num < (double)midCircleRadius * 1.4)
			{
				backLineRenderer.SetPosition(1, startPosition);
			}
			else
			{
				backLineRenderer.SetPosition(1, curPos - midCircleOffset);
			}
		}
		if ((bool)frontLineRenderer)
		{
			Vector3 vector2 = curPos - endPosition;
			float num2 = math.abs(vector2.x) + math.abs(vector2.z);
			frontLineRenderer.positionCount = 2;
			if ((double)num2 < (double)midCircleRadius * 1.4)
			{
				frontLineRenderer.SetPosition(0, endPosition);
			}
			else
			{
				frontLineRenderer.SetPosition(0, curPos + midCircleOffset);
			}
		}
	}

	public void SetMovePath(WorldMarch march, WorldTroopPathSegment[] path, int currPath, Vector3 currPos, int realTargetPos = 0, bool needRefresh = false)
	{
		if (path == null || path.Length == 0)
		{
			return;
		}
		if (startSize != null)
		{
			startSize.transform.position = currPos;
		}
		if (pathIndex == currPath && !needRefresh)
		{
			if (_frontLineRendererValid)
			{
				frontLineRenderer.SetPosition(0, currPos + midCircleOffset);
				frontLineRenderer2.SetPosition(0, currPos + midCircleOffset);
			}
			return;
		}
		pathIndex = currPath;
		midCircleOffset = path[currPath].dir * midCircleRadius;
		if (startSize != null)
		{
			startSize.transform.LookAt(currPos + midCircleOffset);
		}
		if (realTargetPos > 0)
		{
			Vector3 position = SceneManager.World.TileIndexToWorld(realTargetPos);
			if (endSize != null)
			{
				endSize.gameObject.SetActive(value: true);
				endSize.transform.position = position;
			}
			if (!frontLineRenderer)
			{
				return;
			}
			int num = path.Length - currPath + 1;
			frontLineRenderer.positionCount = num;
			frontLineRenderer2.positionCount = num;
			if (num > 1)
			{
				frontLineRenderer.SetPosition(0, currPos + midCircleOffset);
				frontLineRenderer2.SetPosition(0, currPos + midCircleOffset);
				for (int i = currPath + 1; i < path.Length; i++)
				{
					frontLineRenderer.SetPosition(i - currPath, path[i].pos);
					frontLineRenderer2.SetPosition(i - currPath, path[i].pos);
				}
				frontLineRenderer.SetPosition(num - 1, position);
				frontLineRenderer2.SetPosition(num - 1, position);
			}
			return;
		}
		if (endSize != null)
		{
			endSize.gameObject.SetActive(value: true);
			endSize.transform.position = path[^1].pos;
		}
		if (!frontLineRenderer)
		{
			return;
		}
		int num2 = path.Length - currPath;
		frontLineRenderer.positionCount = num2;
		frontLineRenderer2.positionCount = num2;
		if (num2 > 0)
		{
			frontLineRenderer.SetPosition(0, currPos + midCircleOffset);
			frontLineRenderer2.SetPosition(0, currPos + midCircleOffset);
			for (int j = currPath + 1; j < path.Length; j++)
			{
				frontLineRenderer.SetPosition(j - currPath, path[j].pos);
				frontLineRenderer2.SetPosition(j - currPath, path[j].pos);
			}
		}
	}

	public void SetStraightMovePath(Vector3 startPos, Vector3 endPos)
	{
		if (startSize != null)
		{
			startSize.transform.position = startPos;
		}
		if (endSize != null)
		{
			endSize.gameObject.SetActive(value: true);
			endSize.transform.position = endPos;
		}
		if (frontLineRenderer != null)
		{
			frontLineRenderer.SetPosition(0, startPos + midCircleOffset);
			frontLineRenderer.SetPosition(1, endPos);
			frontLineRenderer2.SetPosition(0, startPos + midCircleOffset);
			frontLineRenderer2.SetPosition(1, endPos);
		}
	}

	public void EnableFrontLine(bool enable)
	{
		if (!use_mesh && frontLineRenderer != null)
		{
			frontLineRenderer.enabled = enable;
		}
	}

	public void OnRecycle()
	{
		if (_m_pathMesh != null)
		{
			_m_pathMesh.Dispose();
			_m_pathMesh = null;
		}
	}

	public void FadeIn()
	{
	}

	public void FadeOut()
	{
	}

	public bool IsFadeOutFinish()
	{
		return true;
	}

	public void HideDrag()
	{
		if (simpleAnimation != null)
		{
			simpleAnimation.Play("Hide");
		}
	}

	public void SetDragPath(Vector3 start, Vector3 end)
	{
		if (startSize != null)
		{
			startSize.transform.position = start;
		}
		if (endSize != null)
		{
			endSize.gameObject.SetActive(value: true);
			endSize.transform.position = end;
		}
		if (simpleAnimation != null)
		{
			simpleAnimation.Play("Default");
		}
		if (use_mesh)
		{
			_pathMesh.UpdateSubMesh(PathMesh.MeshType.Drag, enable: true, start, end);
			_pathMesh.UpdateSubMesh(PathMesh.MeshType.Drag, Color.white);
			return;
		}
		dragPath[0] = start;
		dragPath[1] = end;
		if ((bool)frontLineRenderer)
		{
			frontLineRenderer.positionCount = 2;
			frontLineRenderer.SetPositions(dragPath);
		}
	}

	public void SetLineVisible(bool visible)
	{
		base.enabled = visible;
		if (startSprite != null)
		{
			startSprite.enabled = visible;
		}
		if (endSprite != null)
		{
			endSprite.enabled = visible;
		}
	}

	public void SetMidSpriteVisible(bool visible)
	{
		if ((bool)middleSprite)
		{
			middleSprite.enabled = visible;
		}
	}
}
