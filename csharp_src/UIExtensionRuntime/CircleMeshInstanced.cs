using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CircleMeshInstanced : MonoBehaviour
{
	public enum PlaneType
	{
		XY,
		XZ
	}

	private readonly int KEY_MainTex_ST = Shader.PropertyToID("_MainTex_ST");

	private const float radius = 1f;

	private const int segmentCount = 32;

	[SerializeField]
	private PlaneType planeType;

	private MeshRenderer _meshRenderer;

	[SerializeField]
	private Material templateMaterial;

	private MaterialPropertyBlock mbp;

	private static Mesh xyMesh;

	private static Mesh xzMesh;

	private Mesh mesh;

	private static Dictionary<int, Material> sharedMaterials = new Dictionary<int, Material>();

	private static Dictionary<int, int> sharedCount = new Dictionary<int, int>();

	private int textureID = int.MinValue;

	private Material usingMaterial;

	public MeshRenderer MeshRenderer
	{
		get
		{
			if (_meshRenderer == null)
			{
				_meshRenderer = GetComponent<MeshRenderer>();
			}
			return _meshRenderer;
		}
	}

	private Mesh GetXYMesh()
	{
		if (xyMesh == null)
		{
			xyMesh = new Mesh();
			Vector3[] array = new Vector3[33];
			int[] array2 = new int[96];
			Vector2[] array3 = new Vector2[array.Length];
			array[0] = Vector3.zero;
			array3[0] = new Vector2(0.5f, 0.5f);
			float num = MathF.PI / 16f;
			for (int i = 1; i <= 32; i++)
			{
				float f = (float)i * num;
				float num2 = Mathf.Cos(f) * 1f;
				float num3 = Mathf.Sin(f) * 1f;
				array[i] = new Vector3(num2, num3, 0f);
				array3[i] = new Vector2((num2 / 1f + 1f) * 0.5f, (num3 / 1f + 1f) * 0.5f);
			}
			for (int j = 0; j < 32; j++)
			{
				array2[j * 3] = ((j + 2 > 32) ? 1 : (j + 2));
				array2[j * 3 + 1] = j + 1;
				array2[j * 3 + 2] = 0;
			}
			xyMesh.vertices = array;
			xyMesh.triangles = array2;
			xyMesh.uv = array3;
			xyMesh.RecalculateNormals();
		}
		return xyMesh;
	}

	private Mesh GetXZMesh()
	{
		if (xzMesh == null)
		{
			xzMesh = new Mesh();
			Vector3[] array = new Vector3[33];
			int[] array2 = new int[96];
			Vector2[] array3 = new Vector2[array.Length];
			array[0] = Vector3.zero;
			array3[0] = new Vector2(0.5f, 0.5f);
			float num = MathF.PI / 16f;
			for (int i = 1; i <= 32; i++)
			{
				float f = (float)i * num;
				float num2 = Mathf.Cos(f) * 1f;
				float num3 = Mathf.Sin(f) * 1f;
				array[i] = new Vector3(num2, 0f, num3);
				array3[i] = new Vector2((num2 / 1f + 1f) * 0.5f, (num3 / 1f + 1f) * 0.5f);
			}
			for (int j = 0; j < 32; j++)
			{
				array2[j * 3] = ((j + 2 > 32) ? 1 : (j + 2));
				array2[j * 3 + 1] = j + 1;
				array2[j * 3 + 2] = 0;
			}
			xzMesh.vertices = array;
			xzMesh.triangles = array2;
			xzMesh.uv = array3;
			xzMesh.RecalculateNormals();
		}
		return xzMesh;
	}

	private void ReleaseMaterial()
	{
		if (usingMaterial == null)
		{
			return;
		}
		if (sharedCount.TryGetValue(textureID, out var value))
		{
			value--;
			if (value <= 0)
			{
				sharedCount.Remove(textureID);
				sharedMaterials.Remove(textureID);
				UnityEngine.Object.Destroy(usingMaterial);
			}
			else
			{
				sharedCount[textureID] = value;
			}
		}
		usingMaterial = null;
		textureID = int.MinValue;
	}

	private Material GetSharedMaterial(Texture texture)
	{
		if (texture == null)
		{
			return null;
		}
		int instanceID = texture.GetInstanceID();
		if (instanceID == textureID)
		{
			return usingMaterial;
		}
		ReleaseMaterial();
		textureID = instanceID;
		if (!sharedMaterials.TryGetValue(textureID, out usingMaterial) && templateMaterial != null)
		{
			usingMaterial = new Material(templateMaterial);
			usingMaterial.mainTexture = texture;
			usingMaterial.enableInstancing = true;
			sharedMaterials.Add(textureID, usingMaterial);
		}
		if (usingMaterial != null)
		{
			if (sharedCount.TryGetValue(textureID, out var value))
			{
				sharedCount[textureID] = value + 1;
			}
			else
			{
				sharedCount[textureID] = 1;
			}
		}
		return usingMaterial;
	}

	private void SetupMesh()
	{
		mbp = mbp ?? new MaterialPropertyBlock();
		switch (planeType)
		{
		case PlaneType.XY:
			mesh = GetXYMesh();
			break;
		case PlaneType.XZ:
			mesh = GetXZMesh();
			break;
		}
		GetComponent<MeshFilter>().sharedMesh = mesh;
	}

	public void SetupSprite(Sprite sprite)
	{
		if (mesh == null)
		{
			SetupMesh();
		}
		MeshRenderer.sharedMaterial = GetSharedMaterial(sprite.texture);
		Rect textureRect = sprite.textureRect;
		Vector2 vector = new Vector2(textureRect.width / (float)sprite.texture.width, textureRect.height / (float)sprite.texture.height);
		Vector2 vector2 = new Vector2(textureRect.x / (float)sprite.texture.width, textureRect.y / (float)sprite.texture.height);
		MeshRenderer.GetPropertyBlock(mbp);
		mbp.SetVector(KEY_MainTex_ST, new Vector4(vector.x, vector.y, vector2.x, vector2.y));
		MeshRenderer.SetPropertyBlock(mbp);
	}

	public void Release()
	{
		if (_meshRenderer != null)
		{
			_meshRenderer.sharedMaterial = null;
		}
		ReleaseMaterial();
		mbp?.Clear();
		mesh = null;
	}

	private void OnDestroy()
	{
		Release();
	}
}
