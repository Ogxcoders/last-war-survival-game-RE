using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SpriteGPUMeshRenderer : MonoBehaviour
{
	private class MeshReference
	{
		public Mesh mesh;

		public int refCount;
	}

	private class MaterialReference
	{
		public Material mat;

		public int refCount;
	}

	private static readonly int Prop_MainTex = Shader.PropertyToID("_MainTex");

	private static readonly int Prop_MainTex_ST = Shader.PropertyToID("_MainTex_ST");

	private static readonly int Prop_Color = Shader.PropertyToID("_Color");

	private static Dictionary<Texture, MeshReference> _meshes = new Dictionary<Texture, MeshReference>();

	private static Vector2[] _sharedUV = new Vector2[4]
	{
		new Vector2(0f, 1f),
		new Vector2(1f, 1f),
		new Vector2(0f, 0f),
		new Vector2(1f, 0f)
	};

	private Vector4 _spriteUV;

	private static MaterialPropertyBlock _mpb = null;

	private MeshFilter _meshFilter;

	private MeshRenderer _meshRenderer;

	private static Dictionary<Texture, MaterialReference> _sharedMaterials = new Dictionary<Texture, MaterialReference>();

	[SerializeField]
	private Material _sharedMaterial;

	[SerializeField]
	private Sprite _sprite;

	[SerializeField]
	private int _sortingLayerID;

	[SerializeField]
	private int _sortingOrder;

	[SerializeField]
	private Color _color = Color.white;

	public Material sharedMaterial
	{
		get
		{
			return _sharedMaterial;
		}
		set
		{
			_sharedMaterial = value;
		}
	}

	public int sortingLayerID
	{
		get
		{
			return _sortingLayerID;
		}
		set
		{
			_sortingLayerID = value;
		}
	}

	public int sortingOrder
	{
		get
		{
			return _sortingOrder;
		}
		set
		{
			_sortingOrder = value;
		}
	}

	public Color color
	{
		get
		{
			return _color;
		}
		set
		{
			_color = value;
			if (_meshRenderer != null)
			{
				_meshRenderer.GetPropertyBlock(_mpb);
				_mpb.SetColor(Prop_Color, _color);
				_mpb.SetVector(Prop_MainTex_ST, _spriteUV);
				_meshRenderer.SetPropertyBlock(_mpb);
			}
		}
	}

	public Sprite sprite
	{
		get
		{
			return _sprite;
		}
		set
		{
			if (_sprite != value)
			{
				if (_meshFilter.sharedMesh != null)
				{
					ReleaseMesh(_sprite);
				}
				_sprite = value;
				Texture2D texture = _sprite.texture;
				if (_sharedMaterials.TryGetValue(texture, out var value2))
				{
					value2.mat.SetTexture(Prop_MainTex, texture);
				}
				_meshFilter.sharedMesh = GetMesh(_sprite);
			}
		}
	}

	private void Awake()
	{
		if (_mpb == null)
		{
			_mpb = new MaterialPropertyBlock();
		}
		_meshRenderer = GetComponent<MeshRenderer>();
		_meshRenderer.sortingLayerID = _sortingLayerID;
		_meshRenderer.sortingOrder = _sortingOrder;
		_meshRenderer.hideFlags = HideFlags.HideInInspector;
		if (_sharedMaterial != null && _sprite != null)
		{
			Texture2D texture = _sprite.texture;
			_meshRenderer.sharedMaterial = GetInstancedMaterial(texture, _sharedMaterial);
		}
		else
		{
			_meshRenderer.sharedMaterial = null;
		}
		_meshFilter = GetComponent<MeshFilter>();
		_meshFilter.hideFlags = HideFlags.HideInInspector;
		_meshFilter.sharedMesh = GetMesh(_sprite);
		float x = _sprite.uv[2].x;
		float y = _sprite.uv[2].y;
		float x2 = _sprite.uv[1].x;
		float y2 = _sprite.uv[1].y;
		_spriteUV = new Vector4(x2 - x, y2 - y, x, y);
		if (_meshRenderer != null)
		{
			_meshRenderer.GetPropertyBlock(_mpb);
			_mpb.SetColor(Prop_Color, _color);
			_mpb.SetVector(Prop_MainTex_ST, _spriteUV);
			_meshRenderer.SetPropertyBlock(_mpb);
		}
	}

	private void OnDestroy()
	{
		ReleaseMesh(_sprite);
		if (_sprite != null)
		{
			ReleaseInstancedMaterial(_sprite.texture);
		}
	}

	private static Material GetInstancedMaterial(Texture texture, Material originMat)
	{
		if (!_sharedMaterials.TryGetValue(texture, out var value))
		{
			value = new MaterialReference
			{
				mat = UnityEngine.Object.Instantiate(originMat),
				refCount = 1
			};
			value.mat.SetTexture(Prop_MainTex, texture);
			_sharedMaterials.Add(texture, value);
			return value.mat;
		}
		value.refCount++;
		return value.mat;
	}

	private static Mesh GetMesh(Sprite sprite)
	{
		if (sprite == null)
		{
			return null;
		}
		if (_meshes.TryGetValue(sprite.texture, out var value))
		{
			value.refCount++;
			return value.mesh;
		}
		value = new MeshReference
		{
			mesh = SpriteToMesh(sprite),
			refCount = 1
		};
		_meshes.Add(sprite.texture, value);
		return value.mesh;
	}

	private static Mesh SpriteToMesh(Sprite sprite)
	{
		if (sprite == null)
		{
			return null;
		}
		return new Mesh
		{
			vertices = Array.ConvertAll(sprite.vertices, (Converter<Vector2, Vector3>)((Vector2 i) => i)),
			uv = _sharedUV,
			triangles = Array.ConvertAll(sprite.triangles, (Converter<ushort, int>)((ushort i) => i)),
			name = sprite.name
		};
	}

	private static void ReleaseMesh(Sprite sprite)
	{
		if (!(sprite == null) && _meshes.TryGetValue(sprite.texture, out var value))
		{
			value.refCount--;
			if (value.refCount <= 0)
			{
				UnityEngine.Object.Destroy(value.mesh);
				_meshes.Remove(sprite.texture);
			}
		}
	}

	private static void ReleaseInstancedMaterial(Texture texture)
	{
		if (_sharedMaterials.TryGetValue(texture, out var value))
		{
			value.refCount--;
			if (value.refCount <= 0)
			{
				UnityEngine.Object.Destroy(value.mat);
				_sharedMaterials.Remove(texture);
			}
		}
	}
}
