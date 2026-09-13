using UnityEngine;

public class CacheItem
{
	public string assetKey;

	public Texture2D textureAsset;

	public CacheItem()
	{
	}

	public CacheItem(string key, Texture2D texture2D)
	{
		assetKey = key;
		textureAsset = texture2D;
	}

	public void SetData(string key, Texture2D texture2D)
	{
		assetKey = key;
		textureAsset = texture2D;
	}
}
