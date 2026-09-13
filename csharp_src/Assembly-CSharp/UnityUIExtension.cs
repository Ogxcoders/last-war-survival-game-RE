using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using VEngine;

public static class UnityUIExtension
{
	private static List<UnityEngine.Object> keyToRemove = new List<UnityEngine.Object>(100);

	private static readonly Dictionary<UnityEngine.Object, Asset> AllRefs = new Dictionary<UnityEngine.Object, Asset>();

	private static uint s_NextCallbackUID = 1u;

	private static readonly Dictionary<UnityEngine.Object, uint> s_ObjectToLatestCallbackUID = new Dictionary<UnityEngine.Object, uint>();

	private static bool _EnableAsyncLoadImageCallbackCheck = false;

	private static bool _HasGetSwitchData = false;

	private static readonly HashSet<string> _dynamicAtlasPrefixes = new HashSet<string> { "Assets/Main/Sprites/ItemIcons", "Assets/Main/Sprites/HeroIconsSmall", "Assets/Main/Sprites/BuildIconOutCity", "Assets/Main/Sprites/HeroIconsBig" };

	private static readonly int _maxPrefixLength = _dynamicAtlasPrefixes.Max((string p) => p.Length);

	private static string _dynamicAtlasReplaceTest = "Assets/Main/Sprites/UI/UILWScienceTest/science";

	public static readonly int k_SpriteMaxTexelCountFitIntoDynamicAtlas = 65536;

	private static bool s_UseAsyncLoadByDefault = false;

	public static bool EnableAsyncLoadImageCallbackCheck
	{
		get
		{
			if (_HasGetSwitchData)
			{
				return _EnableAsyncLoadImageCallbackCheck;
			}
			if (ClientSwitch.HasSwitchData(30))
			{
				_EnableAsyncLoadImageCallbackCheck = ClientSwitch.IsOn(30);
				_HasGetSwitchData = true;
				return _EnableAsyncLoadImageCallbackCheck;
			}
			return false;
		}
	}

	private static uint GetNextCallbackUID()
	{
		uint result = s_NextCallbackUID++;
		if (s_NextCallbackUID == 0)
		{
			s_NextCallbackUID = 1u;
			Log.Warning("CallbackUID溢出，已重置为1，保持现有映射");
		}
		return result;
	}

	public static void ScriptOnComplete<T>(this T tween, TweenCallback action) where T : Tween
	{
		tween.OnComplete(delegate
		{
			try
			{
				action();
			}
			catch (Exception ex)
			{
				Log.Error("ScriptOnComplete failed:" + ex.Message);
			}
		});
	}

	public static void LoadSprite(this Image image, string spritePath, string defaultSprite = null)
	{
		Color color = image.color;
		color.a = 1f;
		image.color = color;
		LoadSpriteHelper(image, spritePath, defaultSprite, delegate(Sprite sprite)
		{
			image.sprite = sprite;
		});
	}

	public static void LoadSpriteAsync(this Image image, string spritePath, string defaultSprite = null)
	{
		Color color = image.color;
		color.a = 0f;
		image.color = color;
		LoadSpriteHelper(image, spritePath, defaultSprite, delegate(Sprite sprite)
		{
			if (!(sprite == null) && !(image == null))
			{
				image.sprite = sprite;
				Color color2 = image.color;
				color2.a = 1f;
				image.color = color2;
			}
		}, isAsync: true);
	}

	public static void LoadSpriteAsync(this Image image, string spritePath, Action<Sprite> completeCallback, string defaultSprite = null)
	{
		Color color = image.color;
		color.a = 0f;
		image.color = color;
		LoadSpriteHelper(image, spritePath, defaultSprite, delegate(Sprite sprite)
		{
			if (!(sprite == null) && !(image == null))
			{
				image.sprite = sprite;
				Color color2 = image.color;
				color2.a = 1f;
				image.color = color2;
				completeCallback?.Invoke(sprite);
			}
		}, isAsync: true);
	}

	public static void LoadSprite(this CircleImage image, string spritePath, string defaultSprite = null)
	{
		LoadSpriteHelper(image, spritePath, defaultSprite, delegate(Sprite sprite)
		{
			image.sprite = sprite;
		});
	}

	public static void LoadSprite(this SpriteRenderer spriteRenderer, string spritePath, string defaultSprite = null)
	{
		Color color = spriteRenderer.color;
		color.a = 1f;
		spriteRenderer.color = color;
		LoadSpriteHelper(spriteRenderer, spritePath, defaultSprite, delegate(Sprite sprite)
		{
			spriteRenderer.sprite = sprite;
		});
	}

	public static void LoadSpriteAsync(this SpriteRenderer spriteRenderer, string spritePath, string defaultSprite = null)
	{
		Color color = spriteRenderer.color;
		color.a = 0f;
		spriteRenderer.color = color;
		LoadSpriteHelper(spriteRenderer, spritePath, defaultSprite, delegate(Sprite sprite)
		{
			if (!(sprite == null) && !(spriteRenderer == null))
			{
				spriteRenderer.sprite = sprite;
				Color color2 = spriteRenderer.color;
				color2.a = 1f;
				spriteRenderer.color = color2;
			}
		}, isAsync: true);
	}

	public static void LoadSpriteAsync(this SpriteRenderer spriteRenderer, string spritePath, Action<Sprite> completeCallback, string defaultSprite = null)
	{
		Color color = spriteRenderer.color;
		color.a = 0f;
		spriteRenderer.color = color;
		LoadSpriteHelper(spriteRenderer, spritePath, defaultSprite, delegate(Sprite sprite)
		{
			if (!(sprite == null) && !(spriteRenderer == null))
			{
				spriteRenderer.sprite = sprite;
				Color color2 = spriteRenderer.color;
				color2.a = 1f;
				spriteRenderer.color = color2;
				completeCallback?.Invoke(sprite);
			}
		}, isAsync: true);
	}

	public static void LoadSprite(this CircleMesh circleMesh, string spritePath, string defaultSprite = null)
	{
		LoadSpriteHelper(circleMesh, spritePath, defaultSprite, delegate(Sprite sprite)
		{
			circleMesh.SetupSprite(sprite);
		});
	}

	public static void LoadSprite(this CircleMeshInstanced circleMesh, string spritePath, string defaultSprite = null)
	{
		LoadSpriteHelper(circleMesh, spritePath, defaultSprite, delegate(Sprite sprite)
		{
			circleMesh.SetupSprite(sprite);
		});
	}

	public static void LoadSprite(this SpriteMeshRenderer meshRenderer, string spritePath, string defaultSprite = null, bool isAsync = false)
	{
		LoadSpriteHelper(meshRenderer, spritePath, defaultSprite, delegate(Sprite sprite)
		{
			meshRenderer.sprite = sprite;
		}, isAsync);
	}

	private static void LoadSpriteHelper(UnityEngine.Object host, string spritePath, string defaultSprite, Action<Sprite> onSetSprite, bool isAsync = false)
	{
		ReleaseOldSpriteAsset(host);
		if (s_UseAsyncLoadByDefault || isAsync)
		{
			LoadSpriteAsyncImpl(spritePath, defaultSprite, host, onSetSprite);
			return;
		}
		Sprite obj = LoadSpriteImpl(spritePath, defaultSprite, host);
		onSetSprite?.Invoke(obj);
	}

	private static void ReleaseOldSpriteAsset(UnityEngine.Object spriteHost)
	{
		if (AllRefs.TryGetValue(spriteHost, out var value))
		{
			value.Release();
			AllRefs.Remove(spriteHost);
		}
		if (EnableAsyncLoadImageCallbackCheck && spriteHost != null)
		{
			s_ObjectToLatestCallbackUID.Remove(spriteHost);
		}
	}

	private static Sprite LoadSpriteImpl(string spritePath, string defaultSpritePath, UnityEngine.Object spriteRequester)
	{
		bool flag = ShouldSpriteBeConvetedToDynamicAtlas(ref spritePath);
		if (!spritePath.EndsWith(".png"))
		{
			spritePath += ".png";
		}
		ReleaseOldSpriteAsset(spriteRequester);
		Sprite sprite = DynamicAtlasManager.GetUIDynamicAtlas().TryGetDynamicSprite(spritePath);
		if (sprite != null)
		{
			return sprite;
		}
		Asset asset = ResourceManager.LoadAssetStatic(spritePath, typeof(Sprite));
		if (asset != null)
		{
			AllRefs.Add(spriteRequester, asset);
		}
		if (asset != null && !asset.isError)
		{
			Sprite sprite2 = asset.asset as Sprite;
			if (flag && sprite2 != null)
			{
				return TryConvertToDynamicUISprite(sprite2, spritePath, spriteRequester.name, spriteRequester);
			}
			return sprite2;
		}
		if ((asset == null || asset.isError) && !string.IsNullOrEmpty(defaultSpritePath))
		{
			return LoadSpriteImpl(defaultSpritePath, null, spriteRequester);
		}
		return null;
	}

	private static void LoadSpriteAsyncImpl(string spritePath, string defaultSprite, UnityEngine.Object spriteRequester, Action<Sprite> onSetSprite)
	{
		if (spriteRequester == null)
		{
			return;
		}
		bool convertToDynamicSprite = ShouldSpriteBeConvetedToDynamicAtlas(ref spritePath);
		if (!spritePath.EndsWith(".png"))
		{
			spritePath += ".png";
		}
		ReleaseOldSpriteAsset(spriteRequester);
		Sprite finalSprite = DynamicAtlasManager.GetUIDynamicAtlas().TryGetDynamicSprite(spritePath);
		if (finalSprite != null)
		{
			onSetSprite?.Invoke(finalSprite);
			return;
		}
		Asset asset = ResourceManager.LoadAssetAsyncStatic(spritePath, typeof(Sprite));
		if (asset != null)
		{
			AllRefs.Add(spriteRequester, asset);
			uint callbackUID = 0u;
			if (EnableAsyncLoadImageCallbackCheck)
			{
				callbackUID = GetNextCallbackUID();
				s_ObjectToLatestCallbackUID[spriteRequester] = callbackUID;
			}
			if (onSetSprite == null || asset.isError)
			{
				return;
			}
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate(Asset asset2)
			{
				if (!(spriteRequester == null) && (!EnableAsyncLoadImageCallbackCheck || (s_ObjectToLatestCallbackUID.TryGetValue(spriteRequester, out var value) && value == callbackUID)))
				{
					Sprite sprite = asset2.asset as Sprite;
					if (convertToDynamicSprite)
					{
						finalSprite = TryConvertToDynamicUISprite(sprite, spritePath, spriteRequester.name, spriteRequester);
					}
					else
					{
						finalSprite = sprite;
					}
					if (spriteRequester != null)
					{
						onSetSprite(finalSprite);
					}
				}
			});
		}
		else if (!string.IsNullOrEmpty(defaultSprite))
		{
			LoadSpriteAsyncImpl(defaultSprite, null, spriteRequester, onSetSprite);
		}
		else
		{
			Log.Error("LoadSpriteAsyncImpl failed:" + spritePath);
		}
	}

	public static void LoadSprite(this RawImage image, string spritePath, string defaultSprite = null)
	{
		ReleaseOldSpriteAsset(image);
		Color color = image.color;
		color.a = 1f;
		image.color = color;
		if (s_UseAsyncLoadByDefault)
		{
			LoadTextureAsyncImpl(spritePath, defaultSprite, image, delegate(Asset req)
			{
				image.texture = req.asset as Texture;
			});
		}
		else
		{
			image.texture = LoadTextureImpl(spritePath, defaultSprite, image);
		}
	}

	public static void LoadSpriteAsync(this RawImage image, string spritePath, string defaultSprite = null)
	{
		Color color = image.color;
		color.a = 0f;
		image.color = color;
		ReleaseOldSpriteAsset(image);
		LoadTextureAsyncImpl(spritePath, defaultSprite, image, delegate(Asset req)
		{
			image.texture = req.asset as Texture;
			Color color2 = image.color;
			color2.a = 1f;
			image.color = color2;
		});
	}

	public static void LoadSpriteAsync(this RawImage image, string spritePath, Action<Texture> completeCallback, string defaultSprite = null)
	{
		Color color = image.color;
		color.a = 0f;
		image.color = color;
		ReleaseOldSpriteAsset(image);
		LoadTextureAsyncImpl(spritePath, defaultSprite, image, delegate(Asset req)
		{
			image.texture = req.asset as Texture;
			Color color2 = image.color;
			color2.a = 1f;
			image.color = color2;
			completeCallback?.Invoke(req.asset as Texture);
		});
	}

	private static Texture LoadTextureImpl(string texturePath, string defaultTexture, UnityEngine.Object textureRequester)
	{
		if (!texturePath.EndsWith(".png"))
		{
			texturePath += ".png";
		}
		ReleaseOldSpriteAsset(textureRequester);
		Asset asset = ResourceManager.LoadAssetStatic(texturePath, typeof(Texture));
		if (asset != null)
		{
			AllRefs.Add(textureRequester, asset);
		}
		if (asset != null && !asset.isError)
		{
			return asset.asset as Texture;
		}
		if ((asset == null || asset.isError) && !string.IsNullOrEmpty(defaultTexture))
		{
			LoadTextureImpl(defaultTexture, null, textureRequester);
		}
		return null;
	}

	private static void LoadTextureAsyncImpl(string texturePath, string defaultTexture, UnityEngine.Object texRequester, Action<Asset> cb)
	{
		if (texRequester == null)
		{
			return;
		}
		if (!texturePath.EndsWith(".png"))
		{
			texturePath += ".png";
		}
		ReleaseOldSpriteAsset(texRequester);
		Asset asset = ResourceManager.LoadAssetAsyncStatic(texturePath, typeof(Texture));
		if (asset != null)
		{
			AllRefs.Add(texRequester, asset);
			uint callbackUID = 0u;
			if (EnableAsyncLoadImageCallbackCheck)
			{
				callbackUID = GetNextCallbackUID();
				s_ObjectToLatestCallbackUID[texRequester] = callbackUID;
			}
			if (cb == null || asset.isError)
			{
				return;
			}
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate(Asset obj)
			{
				if (!(texRequester == null) && (!EnableAsyncLoadImageCallbackCheck || (s_ObjectToLatestCallbackUID.TryGetValue(texRequester, out var value) && value == callbackUID)))
				{
					cb(obj);
				}
			});
		}
		else if (!string.IsNullOrEmpty(defaultTexture))
		{
			LoadTextureAsyncImpl(defaultTexture, null, texRequester, cb);
		}
		else
		{
			Log.Error("LoadTextureAsyncImpl failed:" + texturePath);
		}
	}

	public static void OverrideImageSpriteByDynamicAtlas(Image image, Sprite sprite)
	{
		Sprite sprite2 = TryConvertToDynamicUISprite(sprite, sprite.name, image.name, null);
		image.sprite = sprite2;
	}

	private static Sprite TryConvertToDynamicUISprite(Sprite sprite, string pathKey, string debugGoName, UnityEngine.Object spriteRequester)
	{
		if (sprite == null)
		{
			return null;
		}
		int width = (int)sprite.rect.width;
		int height = (int)sprite.rect.height;
		if (!IsSpriteSmallEnoughToFitInDynamicAtlas(width, height))
		{
			return sprite;
		}
		Sprite sprite2 = DynamicAtlasManager.GetUIDynamicAtlas().TryConvertToDynamicSprite(sprite, pathKey, debugGoName);
		if (sprite2 != null)
		{
			if (!DynamicAtlas.k_UseGpuCopyTexture && spriteRequester != null)
			{
				ReleaseOldSpriteAsset(spriteRequester);
			}
		}
		else
		{
			sprite2 = sprite;
		}
		return sprite2;
	}

	private static bool ShouldSpriteBeConvetedToDynamicAtlas(ref string spritePath)
	{
		if (!DynamicAtlas.k_UseGpuCopyTexture)
		{
			return true;
		}
		if (string.IsNullOrEmpty(spritePath))
		{
			return false;
		}
		int length = Math.Min(_maxPrefixLength, spritePath.Length);
		ReadOnlySpan<char> span = spritePath.AsSpan(0, length);
		foreach (string dynamicAtlasPrefix in _dynamicAtlasPrefixes)
		{
			if (span.StartsWith(dynamicAtlasPrefix.AsSpan(), StringComparison.Ordinal))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsSpriteSmallEnoughToFitInDynamicAtlas(int width, int height)
	{
		if (width * height <= k_SpriteMaxTexelCountFitIntoDynamicAtlas)
		{
			return true;
		}
		return false;
	}

	public static void Update()
	{
		foreach (KeyValuePair<UnityEngine.Object, Asset> allRef in AllRefs)
		{
			UnityEngine.Object key = allRef.Key;
			if (key == null)
			{
				allRef.Value.Release();
				keyToRemove.Add(key);
			}
		}
		bool enableAsyncLoadImageCallbackCheck = EnableAsyncLoadImageCallbackCheck;
		if (keyToRemove.Count <= 0)
		{
			return;
		}
		foreach (UnityEngine.Object item in keyToRemove)
		{
			AllRefs.Remove(item);
			if (enableAsyncLoadImageCallbackCheck)
			{
				s_ObjectToLatestCallbackUID.Remove(item);
			}
		}
		keyToRemove.Clear();
	}

	public static void TryActive(this GameObject obj, bool active)
	{
		if (obj.activeSelf != active)
		{
			obj.SetActive(active);
		}
	}

	public static SpriteMeshRenderer ConvertToSpriteMeshRender(this SpriteRenderer spriteRenderer)
	{
		if (spriteRenderer == null || spriteRenderer.drawMode != SpriteDrawMode.Simple)
		{
			return null;
		}
		GameObject gameObject = spriteRenderer.transform.gameObject;
		Sprite sprite = spriteRenderer.sprite;
		Material sharedMaterial = spriteRenderer.sharedMaterial;
		_ = spriteRenderer.renderingLayerMask;
		int sortingLayerID = spriteRenderer.sortingLayerID;
		int sortingOrder = spriteRenderer.sortingOrder;
		UnityEngine.Object.DestroyImmediate(spriteRenderer);
		SpriteMeshRenderer spriteMeshRenderer = gameObject.AddComponent<SpriteMeshRenderer>();
		spriteMeshRenderer.sharedMaterial = sharedMaterial;
		spriteMeshRenderer.sortingLayerID = sortingLayerID;
		spriteMeshRenderer.sortingOrder = sortingOrder;
		spriteMeshRenderer.sprite = sprite;
		return spriteMeshRenderer;
	}

	public static void SetAlpha(this Image image, float alpha)
	{
		Color color = image.color;
		color.a = alpha;
		image.color = color;
	}

	public static void SetHorizontalNormalizedPosition(this ScrollRect scrollRect, float ratio)
	{
		scrollRect.horizontalNormalizedPosition = ratio;
	}

	public static float GetHorizontalNormalizedPosition(this ScrollRect scrollRect)
	{
		return scrollRect.horizontalNormalizedPosition;
	}
}
