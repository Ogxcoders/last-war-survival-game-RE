using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UnityEngine.Rendering;
using VEngine;

public class DynamicAtlasManager
{
	private static DynamicAtlasManager _instance;

	private static DynamicAtlas s_UIDynamicAtlas;

	private static DynamicAtlas s_SceneDynamicAtlas;

	public static readonly TextureFormat k_DynamicAtlasTextureFormat = TextureFormat.ASTC_6x6;

	private readonly Dictionary<DynamicAtlasUsage, Dictionary<TextureFormat, DynamicAtlas>> _dynamicAtlasMap = new Dictionary<DynamicAtlasUsage, Dictionary<TextureFormat, DynamicAtlas>>();

	private readonly List<DynamicAtlasIntegerRectangle> _integerRectanglePool = new List<DynamicAtlasIntegerRectangle>();

	private static bool s_DynamicSupportInit = false;

	private static bool s_DynamicAtlasSupport = false;

	private static bool s_HasDoneSelfCheck = false;

	private Texture2D _selfCheckGpuCopyTarTexture;

	private Texture2D _selfCheckSourcePixelData;

	public static DynamicAtlasManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new DynamicAtlasManager();
			}
			return _instance;
		}
	}

	public static bool s_EnableSelfCheck { get; private set; }

	public static bool DynamicAtlasSupport
	{
		get
		{
			if (!s_DynamicSupportInit)
			{
				s_DynamicSupportInit = true;
				s_EnableSelfCheck = true;
				if (DynamicAtlas.k_UseGpuCopyTexture)
				{
					s_DynamicAtlasSupport = SystemInfo.copyTextureSupport != CopyTextureSupport.None;
				}
				else
				{
					s_DynamicAtlasSupport = true;
				}
				if (!Debug.isDebugBuild)
				{
					s_EnableSelfCheck = true;
					bool flag = ClientSwitch.IsOn(34);
					s_DynamicAtlasSupport &= flag;
				}
			}
			return s_DynamicAtlasSupport;
		}
	}

	public static void DisableDynamicAtlasSupport()
	{
		s_DynamicAtlasSupport = false;
		s_DynamicSupportInit = true;
	}

	public static DynamicAtlas GetUIDynamicAtlas()
	{
		if (s_UIDynamicAtlas == null)
		{
			s_UIDynamicAtlas = Instance.GetDynamicAtlas(DynamicAtlasUsage.UI, DynamicAtlasSize.Size_2048, k_DynamicAtlasTextureFormat);
		}
		return s_UIDynamicAtlas;
	}

	public static DynamicAtlas GetDynamicAtlas(DynamicAtlasUsage usage)
	{
		if (usage == DynamicAtlasUsage.Scene)
		{
			if (s_SceneDynamicAtlas == null)
			{
				s_SceneDynamicAtlas = Instance.GetDynamicAtlas(usage, DynamicAtlasSize.Size_2048, k_DynamicAtlasTextureFormat, useTexArray: true);
			}
			return s_SceneDynamicAtlas;
		}
		return Instance.GetDynamicAtlas(usage, DynamicAtlasSize.Size_2048, k_DynamicAtlasTextureFormat, useTexArray: true);
	}

	private DynamicAtlas GetDynamicAtlas(DynamicAtlasUsage usage, DynamicAtlasSize size, TextureFormat textureFormat = TextureFormat.ASTC_6x6, bool useTexArray = false)
	{
		if (!_dynamicAtlasMap.TryGetValue(usage, out var value))
		{
			value = new Dictionary<TextureFormat, DynamicAtlas>();
			_dynamicAtlasMap[usage] = value;
		}
		if (value.TryGetValue(textureFormat, out var value2))
		{
			return value2;
		}
		return value[textureFormat] = new DynamicAtlas(usage, size, textureFormat, useTexArray);
	}

	public Texture2D CreateTexture(int width, int height, TextureFormat textureFormat, string namePrefix)
	{
		Texture2D texture2D = new Texture2D(width, height, textureFormat, mipChain: false, linear: false);
		texture2D.filterMode = FilterMode.Bilinear;
		texture2D.wrapMode = TextureWrapMode.Clamp;
		texture2D.name = $"{namePrefix}_{textureFormat}_{width}x{height}";
		if (DynamicAtlas.k_UseGpuCopyTexture)
		{
			texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: true);
		}
		return texture2D;
	}

	public Texture2DArray CreateTextureArray(int width, int height, TextureFormat textureFormat, int sliceCount, string namePrefix)
	{
		Texture2DArray texture2DArray = new Texture2DArray(width, height, sliceCount, textureFormat, mipChain: false, linear: false);
		texture2DArray.filterMode = FilterMode.Bilinear;
		texture2DArray.wrapMode = TextureWrapMode.Clamp;
		texture2DArray.name = $"{namePrefix}_{textureFormat}_{width}x{height}";
		if (DynamicAtlas.k_UseGpuCopyTexture)
		{
			texture2DArray.Apply(updateMipmaps: false, makeNoLongerReadable: true);
		}
		return texture2DArray;
	}

	public void DestroyTexture(Texture texture)
	{
		UnityEngine.Object.Destroy(texture);
	}

	public DynamicAtlasIntegerRectangle AllocateIntegerRectangle(int x, int y, int width, int height)
	{
		if (_integerRectanglePool.Count > 0)
		{
			DynamicAtlasIntegerRectangle dynamicAtlasIntegerRectangle = _integerRectanglePool.Pop();
			dynamicAtlasIntegerRectangle.x = x;
			dynamicAtlasIntegerRectangle.y = y;
			dynamicAtlasIntegerRectangle.width = width;
			dynamicAtlasIntegerRectangle.height = height;
			return dynamicAtlasIntegerRectangle;
		}
		return new DynamicAtlasIntegerRectangle(x, y, width, height);
	}

	public void ReleaseIntegerRectangle(DynamicAtlasIntegerRectangle rectangle)
	{
		_integerRectanglePool.Add(rectangle);
	}

	public void ClearAll()
	{
		foreach (Dictionary<TextureFormat, DynamicAtlas> value in _dynamicAtlasMap.Values)
		{
			if (value == null)
			{
				continue;
			}
			foreach (DynamicAtlas value2 in value.Values)
			{
				value2?.Clear();
			}
		}
		_dynamicAtlasMap.Clear();
		_integerRectanglePool.Clear();
		s_UIDynamicAtlas = null;
		s_SceneDynamicAtlas = null;
		ClearSelfCheckAsset();
	}

	public void Upload()
	{
		if (DynamicAtlas.k_UseGpuCopyTexture)
		{
			return;
		}
		foreach (Dictionary<TextureFormat, DynamicAtlas> value in _dynamicAtlasMap.Values)
		{
			foreach (DynamicAtlas value2 in value.Values)
			{
				value2.UploadDirtyPages();
			}
		}
	}

	public void PrepareSelfCheck()
	{
		if (!DynamicAtlasSupport || !DynamicAtlas.k_UseGpuCopyTexture || !s_EnableSelfCheck || s_HasDoneSelfCheck)
		{
			return;
		}
		ClearSelfCheckAsset();
		Asset asset = ResourceManager.LoadAssetStatic("Assets/Main/Sprites/ItemIcons/7tianle_jifen_icon.png", typeof(Sprite));
		if (asset == null || asset.isError)
		{
			return;
		}
		try
		{
			Sprite sprite = asset.asset as Sprite;
			if (sprite != null)
			{
				Texture2D texture = sprite.texture;
				if (texture != null)
				{
					int width = texture.width;
					int height = texture.height;
					TextureFormat format = texture.format;
					_selfCheckGpuCopyTarTexture = new Texture2D(width, height, format, mipChain: false, linear: false);
					_selfCheckGpuCopyTarTexture.filterMode = texture.filterMode;
					_selfCheckGpuCopyTarTexture.wrapMode = texture.wrapMode;
					_selfCheckGpuCopyTarTexture.Apply(updateMipmaps: false, makeNoLongerReadable: true);
					Graphics.CopyTexture(texture, _selfCheckGpuCopyTarTexture);
					RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
					Graphics.Blit(texture, temporary);
					_selfCheckSourcePixelData = new Texture2D(width, height, TextureFormat.ARGB32, mipChain: false);
					RenderTexture active = RenderTexture.active;
					RenderTexture.active = temporary;
					_selfCheckSourcePixelData.ReadPixels(new Rect(0f, 0f, temporary.width, temporary.height), 0, 0);
					RenderTexture.ReleaseTemporary(temporary);
					RenderTexture.active = active;
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error("DynamicAtlas PrepareSelfCheck exception: " + ex.Message);
		}
		asset.Release();
	}

	public void DoSelfCheck()
	{
		if (!DynamicAtlasSupport || !DynamicAtlas.k_UseGpuCopyTexture || !s_EnableSelfCheck || s_HasDoneSelfCheck)
		{
			return;
		}
		if (_selfCheckGpuCopyTarTexture == null)
		{
			Log.Error("DynamicAtlas DoSelfCheck self check failed : _selfCheckGpuCopyTarTexture is null ! ");
			return;
		}
		try
		{
			int width = _selfCheckGpuCopyTarTexture.width;
			int height = _selfCheckGpuCopyTarTexture.height;
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
			Graphics.Blit(_selfCheckGpuCopyTarTexture, temporary);
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, mipChain: false);
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = temporary;
			texture2D.ReadPixels(new Rect(0f, 0f, temporary.width, temporary.height), 0, 0);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.active = active;
			if (!CompareWithTolerance(_selfCheckSourcePixelData, texture2D, 30, ignoreAlpha: true))
			{
				DisableDynamicAtlasSupport();
				Log.Error("DynamicAtlas DoSelfCheck self check result : false");
			}
			else
			{
				Log.Info("DynamicAtlas DoSelfCheck self check result : true");
			}
			s_HasDoneSelfCheck = true;
			UnityEngine.Object.DestroyImmediate(texture2D);
		}
		catch (Exception ex)
		{
			Log.Error("DynamicAtlas DoSelfCheck exception: " + ex.Message);
		}
		ClearSelfCheckAsset();
	}

	public void ClearSelfCheckAsset()
	{
		if (_selfCheckGpuCopyTarTexture != null)
		{
			UnityEngine.Object.DestroyImmediate(_selfCheckGpuCopyTarTexture);
			_selfCheckGpuCopyTarTexture = null;
		}
		if (_selfCheckSourcePixelData != null)
		{
			UnityEngine.Object.DestroyImmediate(_selfCheckSourcePixelData);
			_selfCheckSourcePixelData = null;
		}
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
}
