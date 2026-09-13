using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RectangleMeshInstanced : MonoBehaviour
{
	public enum PlaneType
	{
		XY,
		XZ
	}

	private const float width = 1f;

	private const float height = 1f;

	private const float lineWidth = 0.1f;

	private static Mesh MeshXY;

	private static Mesh MeshXZ;

	public Color lineColor;

	public float thickness = 1f;

	public float alpha = 1f;

	private readonly int Key_BaseColor = Shader.PropertyToID("_BaseColor");

	private readonly int Key_Thickness = Shader.PropertyToID("_Thickness");

	private readonly int Key_Alpha = Shader.PropertyToID("_Alpha");

	public PlaneType planeType;

	private MeshRenderer _meshRenderer;

	private MeshFilter meshFilter;

	private MaterialPropertyBlock mbp;

	public MeshRenderer MeshRenderer
	{
		get
		{
			_meshRenderer = _meshRenderer ?? GetComponent<MeshRenderer>();
			return _meshRenderer;
		}
	}

	public MeshFilter MeshFilter
	{
		get
		{
			meshFilter = meshFilter ?? GetComponent<MeshFilter>();
			return meshFilter;
		}
	}

	private MaterialPropertyBlock Mbp
	{
		get
		{
			mbp = mbp ?? new MaterialPropertyBlock();
			return mbp;
		}
	}

	public Color LineColor
	{
		set
		{
			if (lineColor != value)
			{
				lineColor = value;
				MeshRenderer.GetPropertyBlock(Mbp);
				mbp.SetColor(Key_BaseColor, lineColor);
				MeshRenderer.SetPropertyBlock(mbp);
			}
		}
	}

	public float Thickness
	{
		set
		{
			if (!Mathf.Approximately(value, thickness))
			{
				thickness = value;
				MeshRenderer.GetPropertyBlock(Mbp);
				mbp.SetFloat(Key_Thickness, thickness);
				MeshRenderer.SetPropertyBlock(mbp);
			}
		}
	}

	public float Alpha
	{
		set
		{
			if (!Mathf.Approximately(value, alpha))
			{
				alpha = value;
				MeshRenderer.GetPropertyBlock(Mbp);
				mbp.SetFloat(Key_Alpha, alpha);
				MeshRenderer.SetPropertyBlock(mbp);
			}
		}
	}

	private Mesh GetMesh()
	{
		if (planeType == PlaneType.XY)
		{
			if (MeshXY == null)
			{
				MeshXY = GenerateMeshByPlaneType(planeType);
			}
			return MeshXY;
		}
		if (planeType == PlaneType.XZ)
		{
			if (MeshXZ == null)
			{
				MeshXZ = GenerateMeshByPlaneType(planeType);
			}
			return MeshXZ;
		}
		return null;
	}

	private Mesh GenerateMeshByPlaneType(PlaneType planeType)
	{
		Mesh mesh = new Mesh();
		Vector3[] array = new Vector3[8];
		Vector2[] array2 = new Vector2[8];
		float num = 0.5f;
		float num2 = 0.5f;
		float num3 = 0.05f;
		Vector2 vector = new Vector2(0f, 0f);
		switch (planeType)
		{
		case PlaneType.XY:
			array[0] = new Vector3(vector.x - num - num3, vector.y - num2 - num3, 0f);
			array[1] = new Vector3(vector.x + num + num3, vector.y - num2 - num3, 0f);
			array[2] = new Vector3(vector.x + num + num3, vector.y + num2 + num3, 0f);
			array[3] = new Vector3(vector.x - num - num3, vector.y + num2 + num3, 0f);
			array[4] = new Vector3(vector.x - num + num3, vector.y - num2 + num3, 0f);
			array[5] = new Vector3(vector.x + num - num3, vector.y - num2 + num3, 0f);
			array[6] = new Vector3(vector.x + num - num3, vector.y + num2 - num3, 0f);
			array[7] = new Vector3(vector.x - num + num3, vector.y + num2 - num3, 0f);
			break;
		case PlaneType.XZ:
			array[0] = new Vector3(vector.x - num - num3, 0f, vector.y - num2 - num3);
			array[1] = new Vector3(vector.x + num + num3, 0f, vector.y - num2 - num3);
			array[2] = new Vector3(vector.x + num + num3, 0f, vector.y + num2 + num3);
			array[3] = new Vector3(vector.x - num - num3, 0f, vector.y + num2 + num3);
			array[4] = new Vector3(vector.x - num + num3, 0f, vector.y - num2 + num3);
			array[5] = new Vector3(vector.x + num - num3, 0f, vector.y - num2 + num3);
			array[6] = new Vector3(vector.x + num - num3, 0f, vector.y + num2 - num3);
			array[7] = new Vector3(vector.x - num + num3, 0f, vector.y + num2 - num3);
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
		return mesh;
	}

	private void Start()
	{
		MeshFilter.sharedMesh = GetMesh();
		UpdateValues(lineColor, thickness, alpha);
		MeshRenderer.allowOcclusionWhenDynamic = false;
	}

	public void UpdateValues(Color lineColor, float thickness, float alpha)
	{
		this.lineColor = lineColor;
		this.thickness = thickness;
		this.alpha = alpha;
		MeshRenderer.GetPropertyBlock(Mbp);
		mbp.SetColor(Key_BaseColor, lineColor);
		mbp.SetFloat(Key_Alpha, alpha);
		mbp.SetFloat(Key_Thickness, thickness);
		MeshRenderer.SetPropertyBlock(mbp);
	}
}
