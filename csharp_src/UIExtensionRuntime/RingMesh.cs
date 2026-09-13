using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RingMesh : MonoBehaviour
{
	public enum PlaneType
	{
		XY,
		XZ
	}

	[SerializeField]
	private float innerRadius = 1f;

	[SerializeField]
	private float outerRadius = 1f;

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

	private MaterialPropertyBlock mbp;

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

	public void SetMaterialFloat(int id, float value)
	{
		if (mbp == null)
		{
			mbp = new MaterialPropertyBlock();
		}
		mbp.SetFloat(id, value);
		MeshRenderer.SetPropertyBlock(mbp);
	}

	public void SetMaterialColor(int id, Color value)
	{
		if (mbp == null)
		{
			mbp = new MaterialPropertyBlock();
		}
		mbp.SetColor(id, value);
		MeshRenderer.SetPropertyBlock(mbp);
	}

	private void GenerateMesh()
	{
		mesh = new Mesh();
		Vector3[] array = new Vector3[segmentCount * 2];
		Vector2[] array2 = new Vector2[segmentCount * 2];
		int[] array3 = new int[segmentCount * 6];
		array[0] = Vector3.zero;
		array2[0] = new Vector2(0.5f, 0.5f);
		for (int i = 0; i < segmentCount; i++)
		{
			float f = MathF.PI * 2f * (float)i / (float)segmentCount;
			float num = Mathf.Cos(f);
			float num2 = Mathf.Sin(f);
			switch (planeType)
			{
			case PlaneType.XY:
				array[i * 2] = new Vector3(num * innerRadius, num2 * innerRadius, 0f);
				array[i * 2 + 1] = new Vector3(num * outerRadius, num2 * outerRadius, 0f);
				break;
			case PlaneType.XZ:
				array[i * 2] = new Vector3(num * innerRadius, 0f, num2 * innerRadius);
				array[i * 2 + 1] = new Vector3(num * outerRadius, 0f, num2 * outerRadius);
				break;
			}
			array2[i * 2] = new Vector2(0f, 0f);
			array2[i * 2 + 1] = new Vector2(1f, 1f);
			int num3 = i * 6;
			array3[num3] = i * 2;
			array3[num3 + 1] = (i * 2 + 2) % (segmentCount * 2);
			array3[num3 + 2] = i * 2 + 1;
			array3[num3 + 3] = i * 2 + 1;
			array3[num3 + 4] = (i * 2 + 2) % (segmentCount * 2);
			array3[num3 + 5] = (i * 2 + 3) % (segmentCount * 2);
		}
		mesh.vertices = array;
		mesh.triangles = array3;
		mesh.uv = array2;
		mesh.RecalculateNormals();
		GetComponent<MeshFilter>().mesh = mesh;
	}

	public void RebuildMesh(float innerRadius, float outerRadius, int segmentCount)
	{
		this.innerRadius = innerRadius;
		this.outerRadius = outerRadius;
		this.segmentCount = segmentCount;
		GenerateMesh();
	}
}
