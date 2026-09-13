using System.Collections.Generic;
using GameFramework;
using UnityEngine;

public class DynamicAtlasPage
{
	private Texture2D _texture;

	private Texture2DArray _texArray;

	private readonly List<DynamicAtlasIntegerRectangle> _freeAreasList = new List<DynamicAtlasIntegerRectangle>();

	private readonly int _width;

	private readonly int _height;

	public bool needUploadTexToGpu;

	private static List<DynamicAtlasIntegerRectangle> _waitAddNewAreaList = new List<DynamicAtlasIntegerRectangle>();

	public int index { get; }

	public Texture texture
	{
		get
		{
			if (_texture != null)
			{
				return _texture;
			}
			return _texArray;
		}
	}

	public bool useTexArray => _texArray != null;

	public List<DynamicAtlasIntegerRectangle> freeAreasList => _freeAreasList;

	public void ResetTexArray(Texture2DArray texArray)
	{
		_texArray = texArray;
	}

	public DynamicAtlasPage(int index, int width, int height, TextureFormat textureFormat, string usageName, Texture2DArray texArray)
	{
		this.index = index;
		_width = width;
		_height = height;
		needUploadTexToGpu = false;
		_texArray = texArray;
		if (texArray == null)
		{
			_texture = DynamicAtlasManager.Instance.CreateTexture(_width, _height, textureFormat, string.Format("{0}_{1}_{2}", "_DA", usageName, index));
		}
		DynamicAtlasIntegerRectangle item = DynamicAtlasManager.Instance.AllocateIntegerRectangle(0, 0, _width, _height);
		_freeAreasList.Add(item);
	}

	public void UploadTextureToGpuIfDirty()
	{
		if (needUploadTexToGpu)
		{
			_texture?.Apply(updateMipmaps: false, makeNoLongerReadable: false);
			_texArray?.Apply(updateMipmaps: false, makeNoLongerReadable: false);
		}
		needUploadTexToGpu = false;
	}

	public void UpdateTexArrayPreview()
	{
	}

	public void RemoveTexture(Rect rect)
	{
	}

	public int GetFreeAreaIndex(int width, int height, int padding)
	{
		if (width > _width || height >= _height)
		{
			Log.Error("DynamicAtlasPage.GetFreeAreaIndex too large texture for atlas");
			return -1;
		}
		DynamicAtlasIntegerRectangle dynamicAtlasIntegerRectangle = DynamicAtlasManager.Instance.AllocateIntegerRectangle(_width + 1, _height + 1, 0, 0);
		int result = -1;
		int num = width + padding;
		int num2 = height + padding;
		for (int num3 = _freeAreasList.Count - 1; num3 >= 0; num3--)
		{
			DynamicAtlasIntegerRectangle dynamicAtlasIntegerRectangle2 = _freeAreasList[num3];
			if (dynamicAtlasIntegerRectangle2.x < dynamicAtlasIntegerRectangle.x && num <= dynamicAtlasIntegerRectangle2.width && num2 <= dynamicAtlasIntegerRectangle2.height)
			{
				result = num3;
				if ((num == dynamicAtlasIntegerRectangle2.width && dynamicAtlasIntegerRectangle2.width <= dynamicAtlasIntegerRectangle2.height && dynamicAtlasIntegerRectangle2.right < _width) || (num2 == dynamicAtlasIntegerRectangle2.height && dynamicAtlasIntegerRectangle2.height <= width))
				{
					break;
				}
				dynamicAtlasIntegerRectangle = dynamicAtlasIntegerRectangle2;
			}
			else if (dynamicAtlasIntegerRectangle2.x < dynamicAtlasIntegerRectangle.x && width <= dynamicAtlasIntegerRectangle2.width && height <= dynamicAtlasIntegerRectangle2.height)
			{
				result = num3;
				if ((width == dynamicAtlasIntegerRectangle2.width && dynamicAtlasIntegerRectangle2.width <= dynamicAtlasIntegerRectangle2.height && dynamicAtlasIntegerRectangle2.right < _width) || (height == dynamicAtlasIntegerRectangle2.height && dynamicAtlasIntegerRectangle2.height <= dynamicAtlasIntegerRectangle2.width))
				{
					break;
				}
				dynamicAtlasIntegerRectangle = dynamicAtlasIntegerRectangle2;
			}
		}
		return result;
	}

	public void GenerateNewFreeAreas(DynamicAtlasIntegerRectangle target, DynamicAtlasIntegerRectangle freeArea, int padding)
	{
		int x = target.x;
		int y = target.y;
		int num = target.right + padding;
		int num2 = target.top + padding;
		DynamicAtlasIntegerRectangle dynamicAtlasIntegerRectangle = null;
		if (padding == 0)
		{
			dynamicAtlasIntegerRectangle = target;
		}
		for (int num3 = freeAreasList.Count - 1; num3 >= 0; num3--)
		{
			DynamicAtlasIntegerRectangle dynamicAtlasIntegerRectangle2 = freeAreasList[num3];
			if (x <= dynamicAtlasIntegerRectangle2.right && num >= dynamicAtlasIntegerRectangle2.x && y <= dynamicAtlasIntegerRectangle2.top && num2 >= dynamicAtlasIntegerRectangle2.y)
			{
				if (dynamicAtlasIntegerRectangle == null)
				{
					dynamicAtlasIntegerRectangle = DynamicAtlasManager.Instance.AllocateIntegerRectangle(target.x, target.y, target.width + padding, target.height + padding);
				}
				DividedArea(dynamicAtlasIntegerRectangle, dynamicAtlasIntegerRectangle2, _waitAddNewAreaList);
				DynamicAtlasIntegerRectangle value = freeAreasList.Pop();
				if (num3 < freeAreasList.Count)
				{
					freeAreasList[num3] = value;
				}
			}
		}
		if (dynamicAtlasIntegerRectangle != null && dynamicAtlasIntegerRectangle != target)
		{
			DynamicAtlasManager.Instance.ReleaseIntegerRectangle(dynamicAtlasIntegerRectangle);
		}
		CheckSubAreas(_waitAddNewAreaList);
		while (_waitAddNewAreaList.Count > 0)
		{
			DynamicAtlasIntegerRectangle item = _waitAddNewAreaList.Pop();
			_freeAreasList.Add(item);
		}
		_freeAreasList.Remove(freeArea);
	}

	private static void DividedArea(DynamicAtlasIntegerRectangle divider, DynamicAtlasIntegerRectangle area, List<DynamicAtlasIntegerRectangle> results)
	{
		int num = 0;
		int num2 = area.right - divider.right;
		if (num2 > 0)
		{
			results.Add(DynamicAtlasManager.Instance.AllocateIntegerRectangle(divider.right, area.y, num2, area.height));
			num++;
		}
		int num3 = divider.x - area.x;
		if (num3 > 0)
		{
			results.Add(DynamicAtlasManager.Instance.AllocateIntegerRectangle(area.x, area.y, num3, area.height));
			num++;
		}
		int num4 = divider.y - area.y;
		if (num4 > 0)
		{
			results.Add(DynamicAtlasManager.Instance.AllocateIntegerRectangle(area.x, area.y, area.width, num4));
			num++;
		}
		int num5 = area.top - divider.top;
		if (num5 > 0)
		{
			results.Add(DynamicAtlasManager.Instance.AllocateIntegerRectangle(area.x, divider.top, area.width, num5));
			num++;
		}
		if (num == 0 && (divider.width < area.width || divider.height < area.height))
		{
			results.Add(area);
		}
	}

	private static void CheckSubAreas(List<DynamicAtlasIntegerRectangle> areas)
	{
		for (int num = areas.Count - 1; num >= 0; num--)
		{
			DynamicAtlasIntegerRectangle dynamicAtlasIntegerRectangle = areas[num];
			for (int num2 = areas.Count - 1; num2 >= 0; num2--)
			{
				if (num != num2)
				{
					DynamicAtlasIntegerRectangle dynamicAtlasIntegerRectangle2 = areas[num2];
					if (dynamicAtlasIntegerRectangle.x >= dynamicAtlasIntegerRectangle2.x && dynamicAtlasIntegerRectangle.y >= dynamicAtlasIntegerRectangle2.y && dynamicAtlasIntegerRectangle.right <= dynamicAtlasIntegerRectangle2.right && dynamicAtlasIntegerRectangle.top <= dynamicAtlasIntegerRectangle2.top)
					{
						DynamicAtlasManager.Instance.ReleaseIntegerRectangle(dynamicAtlasIntegerRectangle);
						DynamicAtlasIntegerRectangle value = areas.Pop();
						if (num < areas.Count)
						{
							areas[num] = value;
						}
						break;
					}
				}
			}
		}
	}

	public void Clear()
	{
		int i = 0;
		for (int count = _freeAreasList.Count; i < count; i++)
		{
			DynamicAtlasManager.Instance.ReleaseIntegerRectangle(_freeAreasList[i]);
		}
		_freeAreasList.Clear();
		DynamicAtlasManager.Instance.DestroyTexture(_texture);
		_texture = null;
	}

	public static void MakeTextureReadableInEditor(Texture srcTex)
	{
	}

	public static void SetTextureReadableInEditor(Texture srcTex, bool value)
	{
	}
}
