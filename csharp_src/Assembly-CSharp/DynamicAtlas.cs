using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameFramework;
using Unity.Collections;
using UnityEngine;

public class DynamicAtlas
{
	public class TextureRect
	{
		public DynamicAtlasPage page;

		public Rect pixelRect;

		public Vector2 uvTiling;

		public Vector2 uvOffset;

		public Sprite sprite;
	}

	private readonly int _width;

	private readonly int _height;

	private readonly int _padding;

	private readonly TextureFormat _textureFormat;

	private readonly DynamicAtlasUsage _usage;

	public const string AssetNameFlag = "_DA";

	private readonly List<DynamicAtlasPage> _pageList = new List<DynamicAtlasPage>();

	private Texture2DArray _texArray;

	private readonly Dictionary<string, TextureRect> _usingTextureRectMap = new Dictionary<string, TextureRect>();

	public static readonly bool k_UseGpuCopyTexture = true;

	public int blockPixelSize { get; }

	public List<DynamicAtlasPage> PageList => _pageList;

	public bool useTexArray { get; }

	public DynamicAtlas(DynamicAtlasUsage usage, DynamicAtlasSize group, TextureFormat textureFormat, bool useTexArray)
	{
		Log.Info($"DynamicAtlas copyTextureSupport : {SystemInfo.copyTextureSupport}");
		_usage = usage;
		this.useTexArray = useTexArray;
		blockPixelSize = GetBlockPixelSize(textureFormat);
		_height = (_width = (int)group);
		_textureFormat = textureFormat;
	}

	public Sprite TryGetDynamicSprite(string spritePathKey)
	{
		if (string.IsNullOrEmpty(spritePathKey))
		{
			return null;
		}
		if (_usingTextureRectMap.TryGetValue(spritePathKey, out var value))
		{
			return value.sprite;
		}
		return null;
	}

	public Sprite TryConvertToDynamicSprite(Sprite origSprite, string spritePathKey, string debugGoName)
	{
		if (!DynamicAtlasManager.DynamicAtlasSupport || origSprite == null)
		{
			return null;
		}
		if (string.IsNullOrEmpty(spritePathKey))
		{
			spritePathKey = origSprite.name;
		}
		Sprite sprite = TryGetDynamicSprite(spritePathKey);
		if (sprite != null)
		{
			return sprite;
		}
		if (origSprite.name.EndsWith("_DA"))
		{
			return origSprite;
		}
		if (origSprite.texture == null)
		{
			return null;
		}
		Texture2D texture = origSprite.texture;
		if (!k_UseGpuCopyTexture && !texture.isReadable)
		{
			return null;
		}
		if (!IsValidTextureFormat(texture.format))
		{
			if (string.IsNullOrEmpty(debugGoName))
			{
				debugGoName = "_unknown_";
			}
			if (Application.isEditor)
			{
				Debug.LogWarning($"dynamic atlas unsupported texture format:{texture.format}, sprite: {origSprite.name}, goName:{debugGoName}", origSprite);
			}
			return null;
		}
		return CreateDynamicSprite(spritePathKey, origSprite);
	}

	public TextureRect TryAddTexture(Texture2D srcTex)
	{
		if (srcTex == null || !DynamicAtlasManager.DynamicAtlasSupport)
		{
			return null;
		}
		if (!k_UseGpuCopyTexture && !srcTex.isReadable)
		{
			return null;
		}
		if (srcTex.name.EndsWith("_DA"))
		{
			return null;
		}
		if (!_usingTextureRectMap.TryGetValue(srcTex.name, out var value))
		{
			Rect srcRect = new Rect(0f, 0f, srcTex.width, srcTex.height);
			value = CopyTextureRect(srcTex, srcRect);
			if (value != null)
			{
				_usingTextureRectMap.Add(srcTex.name, value);
			}
		}
		if (!Application.isEditor && value != null)
		{
			Resources.UnloadAsset(srcTex);
		}
		return value;
	}

	public void UploadDirtyPages()
	{
		foreach (DynamicAtlasPage page in _pageList)
		{
			page.UploadTextureToGpuIfDirty();
		}
	}

	public void Clear()
	{
		foreach (KeyValuePair<string, TextureRect> item in _usingTextureRectMap)
		{
			UnityEngine.Object.Destroy(item.Value.sprite);
		}
		_usingTextureRectMap.Clear();
		int i = 0;
		for (int count = _pageList.Count; i < count; i++)
		{
			_pageList[i].Clear();
		}
		DynamicAtlasManager.Instance.DestroyTexture(_texArray);
		_texArray = null;
		_pageList.Clear();
	}

	public void RemoveTexture(string name, bool clearAtlas = false)
	{
	}

	private DynamicAtlasPage CreateNewPage(string usageName)
	{
		if (useTexArray)
		{
			int num = _pageList.Count + 1;
			if (_texArray == null)
			{
				num = Mathf.Max(9, num);
				_texArray = DynamicAtlasManager.Instance.CreateTextureArray(_width, _height, _textureFormat, num, string.Format("{0}_{1}_{2}", "_DA", usageName, num));
			}
			else if (num > _texArray.depth)
			{
				Log.Info($"DynamicAtlas increase texArray slice: {num}");
				Texture2DArray texture2DArray = DynamicAtlasManager.Instance.CreateTextureArray(_width, _height, _textureFormat, num, string.Format("{0}_{1}_{2}", "_DA", usageName, num));
				for (int i = 0; i < num - 1; i++)
				{
					if (k_UseGpuCopyTexture)
					{
						Graphics.CopyTexture(_texArray, i, texture2DArray, i);
						_pageList[i].ResetTexArray(texture2DArray);
						continue;
					}
					throw new NotSupportedException("unity 2019 don't support texArray.GetPixelData");
				}
				UnityEngine.Object.Destroy(_texArray);
				_texArray = texture2DArray;
			}
		}
		DynamicAtlasPage dynamicAtlasPage = new DynamicAtlasPage(_pageList.Count, _width, _height, _textureFormat, usageName, _texArray);
		_pageList.Add(dynamicAtlasPage);
		return dynamicAtlasPage;
	}

	private static bool IsValidTextureFormat(TextureFormat format)
	{
		switch (format)
		{
		case TextureFormat.DXT1:
		case TextureFormat.DXT5:
			return true;
		case TextureFormat.ASTC_4x4:
		case TextureFormat.ASTC_5x5:
		case TextureFormat.ASTC_6x6:
		case TextureFormat.ASTC_8x8:
		case TextureFormat.ASTC_10x10:
		case TextureFormat.ASTC_12x12:
			return true;
		default:
			return false;
		}
	}

	private Sprite CreateDynamicSprite(string pathKey, Sprite oriSprite)
	{
		Rect textureRect = oriSprite.textureRect;
		if (textureRect.width == 0f || textureRect.height == 0f)
		{
			if (oriSprite.packed && oriSprite.packingMode == SpritePackingMode.Tight)
			{
				Debug.LogError("sprite " + pathKey + " has zero textureRect, because it's tight packed in spriteAtlas", oriSprite);
			}
			else
			{
				Debug.LogError($"sprite error, width:{textureRect.width}, height:{textureRect.height}, name:{pathKey}", oriSprite);
			}
			return null;
		}
		TextureRect textureRect2 = CopyTextureRect(oriSprite.texture, textureRect);
		Sprite sprite = null;
		if (textureRect2 != null)
		{
			Vector2 pivot = (oriSprite.pivot - oriSprite.rect.min) / oriSprite.rect.size;
			sprite = (textureRect2.sprite = Sprite.Create(textureRect2.page.texture as Texture2D, textureRect2.pixelRect, pivot, oriSprite.pixelsPerUnit, 0u, SpriteMeshType.FullRect, oriSprite.border));
			sprite.name = oriSprite.name + "_DA";
			_usingTextureRectMap[pathKey] = textureRect2;
		}
		return sprite;
	}

	private TextureRect CopyTextureRect(Texture2D srcTex, Rect srcRect)
	{
		if (srcTex.format != _textureFormat)
		{
			Debug.LogError($"can't copy, src format:{srcTex.format} is not the same with dst format:{_textureFormat}, obj:{srcTex.name}", srcTex);
			return null;
		}
		if (k_UseGpuCopyTexture && (srcTex.width % blockPixelSize != 0 || srcTex.height % blockPixelSize != 0))
		{
			return null;
		}
		RectInt blockPixelRect = GetBlockPixelRect(srcRect, blockPixelSize);
		int index = -1;
		DynamicAtlasIntegerRectangle dynamicAtlasIntegerRectangle = InsertArea(blockPixelRect.width, blockPixelRect.height, out index, _usage.ToString());
		DynamicAtlasPage dynamicAtlasPage = _pageList[index];
		blockPixelRect.width = Mathf.Min(blockPixelRect.width, srcTex.width - blockPixelRect.x);
		blockPixelRect.height = Mathf.Min(blockPixelRect.height, srcTex.height - blockPixelRect.y);
		int dstElement = (useTexArray ? index : 0);
		if (k_UseGpuCopyTexture)
		{
			if (!DoGpuCopyTexture(srcTex, 0, 0, blockPixelRect.x, blockPixelRect.y, blockPixelRect.width, blockPixelRect.height, dynamicAtlasPage.texture, dstElement, 0, dynamicAtlasIntegerRectangle.x, dynamicAtlasIntegerRectangle.y))
			{
				return null;
			}
		}
		else
		{
			BlockCopyTextureInCpu(srcTex, blockPixelRect, dynamicAtlasPage.texture, dstElement, dynamicAtlasIntegerRectangle.x, dynamicAtlasIntegerRectangle.y);
			dynamicAtlasPage.needUploadTexToGpu = true;
		}
		Rect rect = new Rect((float)dynamicAtlasIntegerRectangle.x + srcRect.x - (float)blockPixelRect.x, (float)dynamicAtlasIntegerRectangle.y + srcRect.y - (float)blockPixelRect.y, srcRect.width, srcRect.height);
		rect = new Rect(rect.x + 0.5f, rect.y + 0.5f, rect.width - 1f, rect.height - 1f);
		return new TextureRect
		{
			page = dynamicAtlasPage,
			pixelRect = rect,
			uvTiling = new Vector2(rect.width / (float)_width, rect.height / (float)_height),
			uvOffset = new Vector2(rect.xMin / (float)_width, rect.yMin / (float)_height)
		};
	}

	private static bool DoGpuCopyTexture(Texture src, int srcElement, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight, Texture dst, int dstElement, int dstMip, int dstX, int dstY)
	{
		Graphics.CopyTexture(src, srcElement, srcMip, srcX, srcY, srcWidth, srcHeight, dst, dstElement, dstMip, dstX, dstY);
		return true;
	}

	private static bool CompareWithTolerance(Texture2D tex1, Texture2D tex2, int maxTolerance = 20, bool ignoreAlpha = false)
	{
		if (tex1.width != tex2.width || tex1.height != tex2.height)
		{
			return false;
		}
		Color32[] pixels = tex1.GetPixels32();
		Color32[] pixels2 = tex2.GetPixels32();
		bool result = true;
		for (int i = 0; i < pixels.Length; i++)
		{
			int num = Math.Abs(pixels[i].r - pixels2[i].r);
			int num2 = Math.Abs(pixels[i].g - pixels2[i].g);
			int num3 = Math.Abs(pixels[i].b - pixels2[i].b);
			int num4 = ((!ignoreAlpha) ? Math.Abs(pixels[i].a - pixels2[i].a) : 0);
			if (num + num2 + num3 + num4 > maxTolerance)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	private void BlockCopyTextureInCpu(Texture2D srcTex, RectInt srcBlockedRect, Texture tex, int dstElement, int posX, int posY)
	{
		int widthBlockCount = GetWidthBlockCount(srcBlockedRect.width, blockPixelSize);
		int widthBlockCount2 = GetWidthBlockCount(srcBlockedRect.height, blockPixelSize);
		int length = widthBlockCount * 16;
		int num = GetWidthBlockCount(srcTex.width, blockPixelSize) * 16;
		int num2 = GetWidthBlockCount(_width, blockPixelSize) * 16;
		int num3 = srcBlockedRect.y / blockPixelSize * num + srcBlockedRect.x / blockPixelSize * 16;
		int num4 = posY / blockPixelSize * num2 + posX / blockPixelSize * 16;
		NativeArray<byte> rawTextureData = srcTex.GetRawTextureData<byte>();
		if (useTexArray)
		{
			throw new NotSupportedException("unity 2019 don't support texArray.GetPixelData");
		}
		NativeArray<byte> rawTextureData2 = (tex as Texture2D).GetRawTextureData<byte>();
		for (int i = 0; i < widthBlockCount2; i++)
		{
			NativeArray<byte>.Copy(rawTextureData, num3, rawTextureData2, num4, length);
			num3 += num;
			num4 += num2;
		}
	}

	private static RectInt GetBlockPixelRect(Rect rect, int blockSize)
	{
		int num = (int)rect.x / blockSize * blockSize;
		int num2 = (int)rect.y / blockSize * blockSize;
		int num3 = GetWidthBlockCount((int)rect.xMax, blockSize) * blockSize;
		int num4 = GetWidthBlockCount((int)rect.yMax, blockSize) * blockSize;
		return new RectInt(num, num2, num3 - num, num4 - num2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetWidthBlockCount(int width, int blockSize)
	{
		int num = width / blockSize;
		if (width % blockSize != 0)
		{
			num++;
		}
		return num;
	}

	private static int GetBlockPixelSize(TextureFormat textureFormat)
	{
		return textureFormat switch
		{
			TextureFormat.ASTC_5x5 => 5, 
			TextureFormat.ASTC_6x6 => 6, 
			TextureFormat.ASTC_8x8 => 8, 
			TextureFormat.ASTC_10x10 => 10, 
			TextureFormat.ASTC_12x12 => 12, 
			_ => 4, 
		};
	}

	private DynamicAtlasIntegerRectangle InsertArea(int width, int height, out int index, string usageName)
	{
		if (_padding == 0)
		{
			width = GetWidthBlockCount(width, blockPixelSize) * blockPixelSize;
			height = GetWidthBlockCount(height, blockPixelSize) * blockPixelSize;
		}
		else
		{
			width = Mathf.CeilToInt((float)width * 1f / (float)_padding) * _padding;
			height = Mathf.CeilToInt((float)height * 1f / (float)_padding) * _padding;
		}
		DynamicAtlasIntegerRectangle dynamicAtlasIntegerRectangle = null;
		DynamicAtlasPage dynamicAtlasPage = null;
		int i = 0;
		for (int count = _pageList.Count; i < count; i++)
		{
			DynamicAtlasPage dynamicAtlasPage2 = _pageList[i];
			int freeAreaIndex = dynamicAtlasPage2.GetFreeAreaIndex(width, height, _padding);
			if (freeAreaIndex >= 0)
			{
				dynamicAtlasPage = dynamicAtlasPage2;
				dynamicAtlasIntegerRectangle = dynamicAtlasPage.freeAreasList[freeAreaIndex];
				break;
			}
		}
		if (dynamicAtlasIntegerRectangle == null)
		{
			Log.Info($"DynamicAtlas.InsertArea no free area,create new page. index : {_pageList.Count}");
			dynamicAtlasPage = CreateNewPage(usageName);
			dynamicAtlasIntegerRectangle = dynamicAtlasPage.freeAreasList[0];
		}
		DynamicAtlasIntegerRectangle dynamicAtlasIntegerRectangle2 = DynamicAtlasManager.Instance.AllocateIntegerRectangle(dynamicAtlasIntegerRectangle.x, dynamicAtlasIntegerRectangle.y, width, height);
		dynamicAtlasPage.GenerateNewFreeAreas(dynamicAtlasIntegerRectangle2, dynamicAtlasIntegerRectangle, _padding);
		index = dynamicAtlasPage.index;
		return dynamicAtlasIntegerRectangle2;
	}
}
