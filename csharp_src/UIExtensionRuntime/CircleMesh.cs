using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CircleMesh : MonoBehaviour
{
	public enum PlaneType
	{
		XY,
		XZ
	}

	[SerializeField]
	private float radius = 1f;

	[SerializeField]
	private int segmentCount = 36;

	[SerializeField]
	private PlaneType planeType;

	[SerializeField]
	private float spriteScale = 1f;

	[SerializeField]
	private Vector2 spriteOffset = Vector2.zero;

	private Mesh mesh;

	private MeshRenderer _meshRenderer;

	private Material _material;

	public MeshRenderer MeshRenderer
	{
		get
		{
			_meshRenderer = _meshRenderer ?? GetComponent<MeshRenderer>();
			return _meshRenderer;
		}
	}

	public Material MainMaterial
	{
		get
		{
			if (_material == null)
			{
				_material = MeshRenderer.material;
			}
			return _material;
		}
	}

	private void GenerateCircleMesh()
	{
		mesh = new Mesh();
		Vector3[] array = new Vector3[segmentCount + 1];
		int[] array2 = new int[segmentCount * 3];
		Vector2[] array3 = new Vector2[array.Length];
		array[0] = Vector3.zero;
		array3[0] = new Vector2(0.5f, 0.5f);
		float num = MathF.PI * 2f / (float)segmentCount;
		for (int i = 1; i <= segmentCount; i++)
		{
			float f = (float)i * num;
			float num2 = Mathf.Cos(f) * radius;
			float num3 = Mathf.Sin(f) * radius;
			switch (planeType)
			{
			case PlaneType.XY:
				array[i] = new Vector3(num2, num3, 0f);
				break;
			case PlaneType.XZ:
				array[i] = new Vector3(num2, 0f, num3);
				break;
			default:
				array[i] = new Vector3(num2, num3, 0f);
				break;
			}
			array3[i] = new Vector2((num2 / radius + 1f) * 0.5f, (num3 / radius + 1f) * 0.5f);
		}
		for (int j = 0; j < segmentCount; j++)
		{
			array2[j * 3] = ((j + 2 > segmentCount) ? 1 : (j + 2));
			array2[j * 3 + 1] = j + 1;
			array2[j * 3 + 2] = 0;
		}
		mesh.vertices = array;
		mesh.triangles = array2;
		mesh.uv = array3;
		mesh.RecalculateNormals();
		GetComponent<MeshFilter>().mesh = mesh;
	}

	public void SetupTexture(Texture texture)
	{
		if (mesh == null)
		{
			GenerateCircleMesh();
		}
		MainMaterial.mainTexture = texture;
	}

	public void SetupSprite(Sprite sprite)
	{
		if (mesh == null)
		{
			GenerateCircleMesh();
		}
		MainMaterial.mainTexture = sprite.texture;
		Rect textureRect = sprite.textureRect;
		Vector2 vector = new Vector2(textureRect.width / (float)sprite.texture.width, textureRect.height / (float)sprite.texture.height);
		Vector2 vector2 = new Vector2(textureRect.x / (float)sprite.texture.width, textureRect.y / (float)sprite.texture.height);
		MainMaterial.mainTextureOffset = vector2 + spriteOffset * vector;
		MainMaterial.mainTextureScale = vector * spriteScale;
	}

	public void RebuildMesh(float radius, int segmentCount)
	{
		this.radius = radius;
		this.segmentCount = segmentCount;
		GenerateCircleMesh();
	}

	public void Release()
	{
		if (_material != null)
		{
			_material.mainTexture = null;
		}
	}
}
