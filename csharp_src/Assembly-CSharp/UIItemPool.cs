using System;
using UnityEngine;
using UnityEngine.UI;
using VEngine;

[Serializable]
public class UIItemPool : MonoBehaviour
{
	[SerializeField]
	private string uiName;

	[SerializeField]
	private string atlasName;

	[SerializeField]
	private string atlasPath;

	[SerializeField]
	private string spriteName;

	[SerializeField]
	private string spritePath;

	[SerializeField]
	private ResType resType;

	private GameObject templateGO;

	private bool loading;

	private bool fail;

	private Action<GameObject> successHandle;

	private Asset assetRequest;

	public string UIName => uiName;

	public string AtlasName => atlasName;

	public string SpriteName => spriteName;

	private void Awake()
	{
		if (resType == ResType.Prefab)
		{
			TryGetGameObject();
		}
	}

	public void TryGetSprite(Image image, string spriteName)
	{
		fail = true;
		image.LoadSprite(spriteName);
	}

	public void TryGetSprite(Action<Sprite> callback)
	{
		TryGetSprite(callback, spriteName);
	}

	public void TryGetSprite(Action<Sprite> callback, string spriteName)
	{
		if (!string.IsNullOrEmpty(spriteName) && !string.IsNullOrEmpty(AtlasName))
		{
			fail = true;
		}
	}

	public void TryGetGameObject(Action<GameObject> action = null)
	{
		if (!fail)
		{
			if (templateGO != null)
			{
				action?.Invoke(templateGO);
				return;
			}
			successHandle = (Action<GameObject>)Delegate.Combine(successHandle, action);
			GetTemplate(uiName);
		}
	}

	public void RemoveSccessHandle(Action<GameObject> action)
	{
		if (action != null && successHandle != null)
		{
			successHandle = (Action<GameObject>)Delegate.Remove(successHandle, action);
		}
	}

	private void GetTemplate(string AssetName)
	{
		if (loading)
		{
			return;
		}
		loading = true;
		assetRequest = GameEntry.Resource.LoadAssetAsync(AssetName, typeof(GameObject));
		Asset asset = assetRequest;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			if (!assetRequest.isError)
			{
				LoadUIFormSuccessCallback(AssetName, assetRequest.asset, 0f, null);
			}
			else
			{
				LoadUIFormFailureCallback(AssetName, assetRequest.error, null);
			}
		});
	}

	private void LoadUIFormSuccessCallback(string uiFormAssetName, object uiFormAsset, float duration, object userData)
	{
		templateGO = (GameObject)uiFormAsset;
		loading = false;
		successHandle?.Invoke(templateGO);
	}

	private void LoadUIFormFailureCallback(string uiFormAssetName, string errorMessage, object userData)
	{
		loading = false;
		fail = true;
	}

	private void OnDestroy()
	{
		DoDestroy();
	}

	protected virtual void DoDestroy()
	{
		if (resType == ResType.Prefab && assetRequest != null)
		{
			assetRequest.Release();
		}
		if (resType == ResType.Atlas || resType == ResType.Sprite)
		{
			string.IsNullOrEmpty(atlasName);
		}
	}
}
