using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SpriteMeshRenderer : MonoBehaviour
{
	private class MeshReference
	{
		public Mesh mesh;

		public int refCount;
	}

	private static readonly int Prop_MainTex = Shader.PropertyToID("_MainTex");

	private static readonly int Prop_Color = Shader.PropertyToID("_Color");

	private static Dictionary<Sprite, MeshReference> _meshes = new Dictionary<Sprite, MeshReference>();

	private MeshFilter _meshFilter;

	private MeshRenderer _meshRenderer;

	private Material _material;

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
			if (_material != null)
			{
				_material.SetColor(Prop_Color, _color);
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
				if (_material != null)
				{
					_material.SetTexture(Prop_MainTex, _sprite.texture);
				}
				_meshFilter.sharedMesh = GetMesh(_sprite);
			}
		}
	}

	private void Awake()
	{
		_meshRenderer = GetComponent<MeshRenderer>();
		_meshRenderer.sortingLayerID = _sortingLayerID;
		_meshRenderer.sortingOrder = _sortingOrder;
		_meshRenderer.hideFlags = HideFlags.HideInInspector;
		if (_sharedMaterial != null)
		{
			_material = new Material(_sharedMaterial);
			if (_sprite != null)
			{
				_material.SetTexture(Prop_MainTex, _sprite.texture);
			}
			_meshRenderer.material = _material;
		}
		else
		{
			_meshRenderer.sharedMaterial = null;
		}
		_meshFilter = GetComponent<MeshFilter>();
		_meshFilter.hideFlags = HideFlags.HideInInspector;
		_meshFilter.sharedMesh = GetMesh(_sprite);
	}

	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(_material);
		ReleaseMesh(_sprite);
	}

	private static Mesh GetMesh(Sprite sprite)
	{
		if (sprite == null)
		{
			return null;
		}
		if (_meshes.TryGetValue(sprite, out var value))
		{
			value.refCount++;
			return value.mesh;
		}
		value = new MeshReference
		{
			mesh = SpriteToMesh(sprite),
			refCount = 1
		};
		_meshes.Add(sprite, value);
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
			uv = sprite.uv,
			triangles = Array.ConvertAll(sprite.triangles, (Converter<ushort, int>)((ushort i) => i)),
			name = sprite.name
		};
	}

	private static void ReleaseMesh(Sprite sprite)
	{
		if (!(sprite == null) && _meshes.TryGetValue(sprite, out var value))
		{
			value.refCount--;
			if (value.refCount <= 0)
			{
				UnityEngine.Object.Destroy(value.mesh);
				_meshes.Remove(sprite);
			}
		}
	}
}
