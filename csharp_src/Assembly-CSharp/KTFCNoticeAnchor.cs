using System;
using UnityEngine;
using VEngine;

public class KTFCNoticeAnchor : MonoBehaviour
{
	[Header("配置")]
	[Tooltip("anchor - 锚点位置 (0-1 范围)")]
	public Vector2 anchor = new Vector2(0.5f, 0.5f);

	[Tooltip("anchoredPosition - 相对于父节点的偏移")]
	public Vector2 anchoredPosition = new Vector2(0f, 0f);

	[Tooltip("sizeDelta - 大小")]
	public Vector2 sizeDelta = new Vector2(800f, 120f);

	[Tooltip("文字颜色覆盖（可选）")]
	public Color textColor = Color.white;

	[Tooltip("对齐方式")]
	public TextAnchor alignment = TextAnchor.MiddleCenter;

	private Asset _noticeAsset;

	private GameObject _noticeObject;

	private void OnEnable()
	{
		if (GameEntry.Sdk.IsKoreaRegion())
		{
			if (_noticeObject == null)
			{
				LoadAndInstantiateNotice();
			}
			else
			{
				_noticeObject.SetActive(value: true);
			}
		}
	}

	private void OnDisable()
	{
		if (_noticeObject != null)
		{
			_noticeObject.SetActive(value: false);
		}
	}

	private void OnDestroy()
	{
		if (_noticeAsset != null)
		{
			if (_noticeObject != null)
			{
				UnityEngine.Object.Destroy(_noticeObject);
				_noticeObject = null;
			}
			if (_noticeAsset != null)
			{
				_noticeAsset.Release();
				_noticeAsset = null;
			}
		}
	}

	private void LoadAndInstantiateNotice()
	{
		string path = "Assets/Main/Prefabs/UICommonSubPartNew/LastWar/KR_RefundNotice.prefab";
		if (_noticeAsset == null)
		{
			_noticeAsset = GameEntry.Resource.LoadAssetAsync(path, typeof(GameObject));
			Asset noticeAsset = _noticeAsset;
			noticeAsset.completed = (Action<Asset>)Delegate.Combine(noticeAsset.completed, (Action<Asset>)delegate(Asset prefab)
			{
				_noticeObject = UnityEngine.Object.Instantiate(prefab.asset as GameObject);
				RectTransform component = _noticeObject.GetComponent<RectTransform>();
				component.SetParent(base.transform, worldPositionStays: false);
				component.localScale = Vector3.one;
				component.anchorMin = anchor;
				component.anchorMax = anchor;
				component.anchoredPosition = anchoredPosition;
				component.sizeDelta = sizeDelta;
				component.SetAsLastSibling();
			});
		}
	}

	private void OnDrawGizmos()
	{
		RectTransform rectTransform = base.transform as RectTransform;
		if (!(rectTransform == null))
		{
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
			Vector2 size = rectTransform.rect.size;
			Vector2 pivot = rectTransform.pivot;
			float x = (anchor.x - pivot.x) * size.x + anchoredPosition.x;
			float y = (anchor.y - pivot.y) * size.y + anchoredPosition.y;
			Gizmos.DrawWireCube(new Vector3(x, y, 0f), new Vector3(sizeDelta.x, sizeDelta.y, 0f));
			Gizmos.matrix = matrix;
		}
	}
}
