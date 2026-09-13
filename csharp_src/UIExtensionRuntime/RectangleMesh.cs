using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RectangleMesh : MonoBehaviour
{
	public enum PlaneType
	{
		XY,
		XZ
	}

	public Vector2 center = new Vector2(0f, 0f);

	public float width = 1f;

	public float height = 1f;

	public float lineWidth = 0.1f;

	public PlaneType planeType;

	private Mesh mesh;

	private int currentOrder = -1;

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

	public void SetRenderOrder(int order)
	{
		if (currentOrder != order)
		{
			currentOrder = order;
			MeshRenderer.sortingOrder = currentOrder;
		}
	}

	private void GenerateMesh()
	{
		mesh = new Mesh();
		Vector3[] array = new Vector3[8];
		Vector2[] array2 = new Vector2[8];
		float num = height / 2f;
		float num2 = width / 2f;
		float num3 = lineWidth / 2f;
		switch (planeType)
		{
		case PlaneType.XY:
			array[0] = new Vector3(center.x - num - num3, center.y - num2 - num3, 0f);
			array[1] = new Vector3(center.x + num + num3, center.y - num2 - num3, 0f);
			array[2] = new Vector3(center.x + num + num3, center.y + num2 + num3, 0f);
			array[3] = new Vector3(center.x - num - num3, center.y + num2 + num3, 0f);
			array[4] = new Vector3(center.x - num + num3, center.y - num2 + num3, 0f);
			array[5] = new Vector3(center.x + num - num3, center.y - num2 + num3, 0f);
			array[6] = new Vector3(center.x + num - num3, center.y + num2 - num3, 0f);
			array[7] = new Vector3(center.x - num + num3, center.y + num2 - num3, 0f);
			break;
		case PlaneType.XZ:
			array[0] = new Vector3(center.x - num - num3, 0f, center.y - num2 - num3);
			array[1] = new Vector3(center.x + num + num3, 0f, center.y - num2 - num3);
			array[2] = new Vector3(center.x + num + num3, 0f, center.y + num2 + num3);
			array[3] = new Vector3(center.x - num - num3, 0f, center.y + num2 + num3);
			array[4] = new Vector3(center.x - num + num3, 0f, center.y - num2 + num3);
			array[5] = new Vector3(center.x + num - num3, 0f, center.y - num2 + num3);
			array[6] = new Vector3(center.x + num - num3, 0f, center.y + num2 - num3);
			array[7] = new Vector3(center.x - num + num3, 0f, center.y + num2 - num3);
			break;
		}
		array2[0] = new Vector2(1f, 1f);
		array2[1] = new Vector2(1f, 1f);
		array2[2] = new Vector2(1f, 1f);
		array2[3] = new Vector2(1f, 1f);
		array2[4] = new Vector2(0f, 0f);
		array2[5] = new Vector2(0f, 0f);
		array2[6] = new Vector2(0f, 0f);
		array2[7] = new Vector2(0f, 0f);
		int[] triangles = new int[24]
		{
			0, 4, 1, 1, 4, 5, 1, 5, 2, 2,
			5, 6, 2, 6, 3, 3, 6, 7, 3, 7,
			0, 0, 7, 4
		};
		mesh.vertices = array;
		mesh.triangles = triangles;
		mesh.uv = array2;
		mesh.RecalculateNormals();
		GetComponent<MeshFilter>().mesh = mesh;
	}

	public void RebuildMesh(float width, float height, float lineWidth)
	{
		this.width = width;
		this.height = height;
		this.lineWidth = lineWidth;
		GenerateMesh();
	}
}
