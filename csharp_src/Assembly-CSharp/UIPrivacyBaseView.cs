using System;
using UnityEngine;
using VEngine;

public abstract class UIPrivacyBaseView
{
	private Asset privacyViewAsset;

	private GameObject privacyViewObject;

	public abstract string GetLoadAssetPath();

	public abstract void OnCreate(GameObject go);

	public abstract void OnDestroy();

	public void OpenPrivacyView()
	{
		string loadAssetPath = GetLoadAssetPath();
		if (privacyViewAsset == null)
		{
			privacyViewAsset = GameEntry.Resource.LoadAssetAsync(loadAssetPath, typeof(GameObject));
			Asset asset = privacyViewAsset;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate(Asset prefab)
			{
				privacyViewObject = UnityEngine.Object.Instantiate(prefab.asset as GameObject);
				SetUILoadingPosition(privacyViewObject);
				OnCreate(privacyViewObject);
			});
		}
	}

	public void ClosePrivacyView()
	{
		if (privacyViewAsset != null)
		{
			OnDestroy();
			if (privacyViewObject != null)
			{
				UnityEngine.Object.Destroy(privacyViewObject);
				privacyViewObject = null;
			}
			if (privacyViewAsset != null)
			{
				privacyViewAsset.Release();
				privacyViewAsset = null;
			}
		}
	}

	private void SetUILoadingPosition(GameObject uiObject)
	{
		RectTransform component = uiObject.GetComponent<RectTransform>();
		Transform transform = GameEntry.UIContainer.Find("Dialog");
		RectTransform rectTransform = null;
		if (transform != null)
		{
			rectTransform = transform.GetComponent<RectTransform>();
		}
		if (rectTransform == null)
		{
			rectTransform = new GameObject("Dialog", typeof(RectTransform)).GetComponent<RectTransform>();
			rectTransform.gameObject.layer = LayerMask.NameToLayer("UI");
			rectTransform.SetParent(GameEntry.UIContainer, worldPositionStays: false);
			rectTransform.localScale = Vector3.one;
			rectTransform.offsetMin = Vector3.zero;
			rectTransform.offsetMax = Vector3.zero;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			if (GameEntry.UIContainer.Find("Normal") != null)
			{
				rectTransform.SetAsLastSibling();
			}
		}
		component.SetParent(rectTransform, worldPositionStays: false);
		component.localScale = Vector3.one;
		component.offsetMin = Vector3.zero;
		component.offsetMax = Vector3.zero;
		component.anchorMin = Vector2.zero;
		component.anchorMax = Vector2.one;
		component.pivot = new Vector2(0.5f, 0.5f);
		component.SetAsLastSibling();
	}
}
