using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Spine.Unity;

[CreateAssetMenu(fileName = "New Spine Atlas Asset", menuName = "Spine/Spine Atlas Asset")]
public class SpineAtlasAsset : AtlasAssetBase
{
	public TextAsset atlasFile;

	public Material[] materials;

	protected Atlas atlas;

	public override bool IsLoaded => atlas != null;

	public override IEnumerable<Material> Materials => materials;

	public override int MaterialCount
	{
		get
		{
			if (materials != null)
			{
				return materials.Length;
			}
			return 0;
		}
	}

	public override Material PrimaryMaterial => materials[0];

	public static SpineAtlasAsset CreateRuntimeInstance(TextAsset atlasText, Material[] materials, bool initialize)
	{
		SpineAtlasAsset spineAtlasAsset = ScriptableObject.CreateInstance<SpineAtlasAsset>();
		spineAtlasAsset.Reset();
		spineAtlasAsset.atlasFile = atlasText;
		spineAtlasAsset.materials = materials;
		if (initialize)
		{
			spineAtlasAsset.GetAtlas();
		}
		return spineAtlasAsset;
	}

	public static SpineAtlasAsset CreateRuntimeInstance(TextAsset atlasText, Texture2D[] textures, Material materialPropertySource, bool initialize)
	{
		string[] array = atlasText.text.Replace("\r", "").Split(new char[1] { '\n' });
		List<string> list = new List<string>();
		for (int i = 0; i < array.Length - 1; i++)
		{
			string text = array[i].Trim();
			if (text.EndsWith(".png"))
			{
				list.Add(text.Replace(".png", ""));
			}
		}
		Material[] array2 = new Material[list.Count];
		int j = 0;
		for (int count = list.Count; j < count; j++)
		{
			Material material = null;
			string a = list[j];
			int k = 0;
			for (int num = textures.Length; k < num; k++)
			{
				if (string.Equals(a, textures[k].name, StringComparison.OrdinalIgnoreCase))
				{
					material = new Material(materialPropertySource);
					material.mainTexture = textures[k];
					break;
				}
			}
			if (material != null)
			{
				array2[j] = material;
				continue;
			}
			throw new ArgumentException("Could not find matching atlas page in the texture array.");
		}
		return CreateRuntimeInstance(atlasText, array2, initialize);
	}

	public static SpineAtlasAsset CreateRuntimeInstance(TextAsset atlasText, Texture2D[] textures, Shader shader, bool initialize)
	{
		if (shader == null)
		{
			shader = Shader.Find("Spine/Skeleton");
		}
		Material materialPropertySource = new Material(shader);
		return CreateRuntimeInstance(atlasText, textures, materialPropertySource, initialize);
	}

	private void Reset()
	{
		Clear();
	}

	public override void Clear()
	{
		atlas = null;
	}

	public override Atlas GetAtlas(bool onlyMetaData = false)
	{
		if (atlasFile == null)
		{
			Debug.LogError("Atlas file not set for atlas asset: " + base.name, this);
			Clear();
			return null;
		}
		if (!onlyMetaData && (materials == null || materials.Length == 0))
		{
			Debug.LogError("Materials not set for atlas asset: " + base.name, this);
			Clear();
			return null;
		}
		if (atlas != null)
		{
			return atlas;
		}
		try
		{
			atlas = new Atlas(textureLoader: onlyMetaData ? ((TextureLoader)new NoOpTextureLoader()) : ((TextureLoader)new MaterialsTextureLoader(this)), reader: new StringReader(atlasFile.text), imagesDir: "");
			atlas.FlipV();
			return atlas;
		}
		catch (Exception ex)
		{
			Debug.LogError("Error reading atlas file for atlas asset: " + base.name + "\n" + ex.Message + "\n" + ex.StackTrace, this);
			return null;
		}
	}

	public Mesh GenerateMesh(string name, Mesh mesh, out Material material, float scale = 0.01f)
	{
		AtlasRegion atlasRegion = atlas.FindRegion(name);
		material = null;
		if (atlasRegion != null)
		{
			if (mesh == null)
			{
				mesh = new Mesh();
				mesh.name = name;
			}
			Vector3[] array = new Vector3[4];
			Vector2[] array2 = new Vector2[4];
			Color[] colors = new Color[4]
			{
				Color.white,
				Color.white,
				Color.white,
				Color.white
			};
			int[] triangles = new int[6] { 0, 1, 2, 2, 3, 0 };
			float num = (float)atlasRegion.width / -2f;
			float x = num * -1f;
			float num2 = (float)atlasRegion.height / 2f;
			float y = num2 * -1f;
			array[0] = new Vector3(num, y, 0f) * scale;
			array[1] = new Vector3(num, num2, 0f) * scale;
			array[2] = new Vector3(x, num2, 0f) * scale;
			array[3] = new Vector3(x, y, 0f) * scale;
			float u = atlasRegion.u;
			float v = atlasRegion.v;
			float u2 = atlasRegion.u2;
			float v2 = atlasRegion.v2;
			if (atlasRegion.degrees == 90)
			{
				array2[0] = new Vector2(u2, v2);
				array2[1] = new Vector2(u, v2);
				array2[2] = new Vector2(u, v);
				array2[3] = new Vector2(u2, v);
			}
			else
			{
				array2[0] = new Vector2(u, v2);
				array2[1] = new Vector2(u, v);
				array2[2] = new Vector2(u2, v);
				array2[3] = new Vector2(u2, v2);
			}
			mesh.triangles = new int[0];
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.colors = colors;
			mesh.triangles = triangles;
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			material = (Material)atlasRegion.page.rendererObject;
		}
		else
		{
			mesh = null;
		}
		return mesh;
	}
}
