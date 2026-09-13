using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Spine.Unity;

[CreateAssetMenu(fileName = "New Spine SpriteAtlas Asset", menuName = "Spine/Spine SpriteAtlas Asset")]
public class SpineSpriteAtlasAsset : AtlasAssetBase
{
	[Serializable]
	protected class SavedRegionInfo
	{
		public float x;

		public float y;

		public float width;

		public float height;

		public SpritePackingRotation packingRotation;
	}

	public SpriteAtlas spriteAtlasFile;

	public Material[] materials;

	protected Atlas atlas;

	public bool updateRegionsInPlayMode;

	[SerializeField]
	protected SavedRegionInfo[] savedRegions;

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

	public static SpineSpriteAtlasAsset CreateRuntimeInstance(SpriteAtlas spriteAtlasFile, Material[] materials, bool initialize)
	{
		SpineSpriteAtlasAsset spineSpriteAtlasAsset = ScriptableObject.CreateInstance<SpineSpriteAtlasAsset>();
		spineSpriteAtlasAsset.Reset();
		spineSpriteAtlasAsset.spriteAtlasFile = spriteAtlasFile;
		spineSpriteAtlasAsset.materials = materials;
		if (initialize)
		{
			spineSpriteAtlasAsset.GetAtlas();
		}
		return spineSpriteAtlasAsset;
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
		if (spriteAtlasFile == null)
		{
			Debug.LogError("SpriteAtlas file not set for SpineSpriteAtlasAsset: " + base.name, this);
			Clear();
			return null;
		}
		if (!onlyMetaData && (materials == null || materials.Length == 0))
		{
			Debug.LogError("Materials not set for SpineSpriteAtlasAsset: " + base.name, this);
			Clear();
			return null;
		}
		if (atlas != null)
		{
			return atlas;
		}
		try
		{
			atlas = LoadAtlas(spriteAtlasFile);
			return atlas;
		}
		catch (Exception ex)
		{
			Debug.LogError("Error analyzing SpriteAtlas for SpineSpriteAtlasAsset: " + base.name + "\n" + ex.Message + "\n" + ex.StackTrace, this);
			return null;
		}
	}

	protected void AssignRegionsFromSavedRegions(Sprite[] sprites, Atlas usedAtlas)
	{
		if (savedRegions == null || savedRegions.Length != sprites.Length)
		{
			return;
		}
		int num = 0;
		foreach (AtlasRegion usedAtla in usedAtlas)
		{
			SavedRegionInfo savedRegionInfo = savedRegions[num];
			AtlasPage page = usedAtla.page;
			usedAtla.degrees = ((savedRegionInfo.packingRotation != SpritePackingRotation.None) ? 90 : 0);
			float x = savedRegionInfo.x;
			float y = savedRegionInfo.y;
			float width = savedRegionInfo.width;
			float height = savedRegionInfo.height;
			usedAtla.u = x / (float)page.width;
			usedAtla.v = y / (float)page.height;
			if (usedAtla.degrees == 90)
			{
				usedAtla.u2 = (x + height) / (float)page.width;
				usedAtla.v2 = (y + width) / (float)page.height;
			}
			else
			{
				usedAtla.u2 = (x + width) / (float)page.width;
				usedAtla.v2 = (y + height) / (float)page.height;
			}
			usedAtla.x = (int)x;
			usedAtla.y = (int)y;
			usedAtla.width = Math.Abs((int)width);
			usedAtla.height = Math.Abs((int)height);
			float v = usedAtla.v;
			usedAtla.v = usedAtla.v2;
			usedAtla.v2 = v;
			usedAtla.originalWidth = (int)width;
			usedAtla.originalHeight = (int)height;
			usedAtla.offsetX = 0f;
			usedAtla.offsetY = 0f;
			num++;
		}
	}

	private Atlas LoadAtlas(SpriteAtlas spriteAtlas)
	{
		List<AtlasPage> list = new List<AtlasPage>();
		List<AtlasRegion> list2 = new List<AtlasRegion>();
		Sprite[] array = new Sprite[spriteAtlas.spriteCount];
		spriteAtlas.GetSprites(array);
		if (array.Length == 0)
		{
			return new Atlas(list, list2);
		}
		Texture2D texture2D = null;
		texture2D = AccessPackedTexture(array);
		Material material = materials[0];
		material.mainTexture = texture2D;
		AtlasPage atlasPage = new AtlasPage();
		atlasPage.name = spriteAtlas.name;
		atlasPage.width = texture2D.width;
		atlasPage.height = texture2D.height;
		atlasPage.format = Format.RGBA8888;
		atlasPage.minFilter = TextureFilter.Linear;
		atlasPage.magFilter = TextureFilter.Linear;
		atlasPage.uWrap = TextureWrap.ClampToEdge;
		atlasPage.vWrap = TextureWrap.ClampToEdge;
		atlasPage.rendererObject = material;
		list.Add(atlasPage);
		array = AccessPackedSprites(spriteAtlas);
		for (int i = 0; i < array.Length; i++)
		{
			Sprite sprite = array[i];
			AtlasRegion atlasRegion = new AtlasRegion();
			atlasRegion.name = sprite.name.Replace("(Clone)", "");
			atlasRegion.page = atlasPage;
			atlasRegion.degrees = ((sprite.packingRotation != SpritePackingRotation.None) ? 90 : 0);
			atlasRegion.u2 = 1f;
			atlasRegion.v2 = 1f;
			atlasRegion.width = atlasPage.width;
			atlasRegion.height = atlasPage.height;
			atlasRegion.originalWidth = atlasPage.width;
			atlasRegion.originalHeight = atlasPage.height;
			atlasRegion.index = i;
			list2.Add(atlasRegion);
		}
		Atlas atlas = new Atlas(list, list2);
		AssignRegionsFromSavedRegions(array, atlas);
		return atlas;
	}

	public static Texture2D AccessPackedTexture(Sprite[] sprites)
	{
		return sprites[0].texture;
	}

	public static Sprite[] AccessPackedSprites(SpriteAtlas spriteAtlas)
	{
		Sprite[] array = null;
		if (array == null)
		{
			array = new Sprite[spriteAtlas.spriteCount];
			spriteAtlas.GetSprites(array);
			if (array.Length == 0)
			{
				return null;
			}
		}
		return array;
	}
}
