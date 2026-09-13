using System.Collections.Generic;
using LS.UnityEngine.UI;
using Spine.Unity;
using SuperScrollView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArabicMirror : MonoBehaviour
{
	[HideInInspector]
	public List<GameObject> mirrorObjects = new List<GameObject>();

	[SerializeField]
	[HideInInspector]
	private List<SingleMirrorObjectData> mirrorObjectDatas = new List<SingleMirrorObjectData>();

	public bool IsApplyAutoMirror = true;

	public bool IsNoLuaControl;

	[Tooltip("以自身中心而不是以屏幕中心做镜像")]
	public bool IsInnerMirror;

	[SerializeField]
	private List<GameObject> _autoMirrorCullObjects = new List<GameObject>();

	public static void MirrorEntry(bool isInnerMirror, bool isProcessRootAnchorAndPivot, GameObject go)
	{
		if (go.TryGetComponent<ArabicMirror>(out var component) && !component.IsApplyAutoMirror)
		{
			return;
		}
		bool flag = component?.IsNoLuaControl ?? false;
		if (UIRunTimeConfig.IsArabic && (flag || MirrorVersionConfig.IsMirrorVersionOpen) && go.transform is RectTransform rectTransform && rectTransform.localScale.x != 0f && rectTransform.localScale.y != 0f)
		{
			List<GameObject> cullingObjects = new List<GameObject>();
			if (go.TryGetComponent<ArabicMirror>(out var component2))
			{
				cullingObjects = component2._autoMirrorCullObjects;
			}
			List<AutoMirrorPositionData> preOrderDataList = new List<AutoMirrorPositionData>();
			RecordAutoOriginalData(rectTransform, preOrderDataList, cullingObjects);
			Counter counter = new Counter
			{
				count = 0
			};
			Mirror(rectTransform, rectTransform, preOrderDataList, cullingObjects, counter, isInnerMirror, isProcessRootAnchorAndPivot, isProcessSelfPos: true);
		}
	}

	public void Awake()
	{
		if (IsNoLuaControl && IsApplyAutoMirror)
		{
			MirrorEntry(isInnerMirror: false, isProcessRootAnchorAndPivot: true, base.gameObject);
		}
	}

	private static void RecordAutoOriginalData(RectTransform rect, List<AutoMirrorPositionData> preOrderDataList, List<GameObject> cullingObjects)
	{
		if (rect == null || !IsProcessSelf(rect, cullingObjects) || rect.localScale.x == 0f || rect.localScale.y == 0f)
		{
			return;
		}
		PreProcessInRecord(rect, cullingObjects);
		preOrderDataList.Add(new AutoMirrorPositionData(rect.gameObject));
		foreach (Transform item in rect)
		{
			if (item is RectTransform rect2)
			{
				RecordAutoOriginalData(rect2, preOrderDataList, cullingObjects);
			}
		}
	}

	private static void PreProcessInRecord(RectTransform rect, List<GameObject> cullingObjects)
	{
		if (rect != null && rect.TryGetComponent<Slider>(out var component))
		{
			if ((bool)component.fillRect)
			{
				component.fillRect.gameObject.AddComponent<ArabicImageMirror>().mirrorType = ArabicImageMirror.MirrorType.Horizontal;
				cullingObjects.Add(component.fillRect.gameObject);
			}
			if ((bool)component.handleRect)
			{
				component.handleRect.gameObject.AddComponent<ArabicImageMirror>().mirrorType = ArabicImageMirror.MirrorType.Horizontal;
				cullingObjects.Add(component.handleRect.gameObject);
			}
		}
	}

	private static void Mirror(RectTransform rect, RectTransform rootRect, List<AutoMirrorPositionData> preOrderDataList, List<GameObject> cullingObjects, Counter counter, bool isInnerMirror, bool isProcessRootAnchorAndPivot, bool isProcessSelfPos, AutoMirrorPositionData rootRectData = null)
	{
		if (!(rect != null))
		{
			return;
		}
		List<GameObject> cullingObjects2 = cullingObjects ?? new List<GameObject>();
		if (!IsProcessSelf(rect, cullingObjects2) || rect.localScale.x == 0f || rect.localScale.y == 0f)
		{
			return;
		}
		bool flag = rect == rootRect;
		bool flag2 = isProcessRootAnchorAndPivot || !flag;
		AutoMirrorPositionData autoMirrorPositionData = preOrderDataList[counter.count++];
		rootRectData = (flag ? autoMirrorPositionData : rootRectData);
		if (isProcessSelfPos)
		{
			Vector2 vector = new Vector2(Screen.width / 2, Screen.height / 2);
			Vector3 vector2 = GameEntry.UICamera.ScreenToWorldPoint(new Vector3(vector.x, vector.y, 0f));
			if (isInnerMirror)
			{
				Vector3 vector3 = rootRectData?.position ?? rootRect.position;
				vector3.x += (0.5f - rootRect.pivot.x) * rootRect.rect.width * rootRect.lossyScale.x;
				vector3.y += (0.5f - rootRect.pivot.y) * rootRect.rect.height * rootRect.lossyScale.y;
				vector2 = new Vector3(vector3.x, vector3.y, 0f);
			}
			Vector3 vector4 = autoMirrorPositionData.position - vector2;
			Vector3 position = new Vector3(vector2.x - vector4.x, autoMirrorPositionData.position.y, autoMirrorPositionData.position.z);
			if (flag2)
			{
				float x = rect.anchorMin.x;
				float x2 = rect.anchorMax.x;
				rect.anchorMin = new Vector2(1f - x2, rect.anchorMin.y);
				rect.anchorMax = new Vector2(1f - x, rect.anchorMax.y);
			}
			rect.position = position;
			Vector3 localPosition = rect.localPosition;
			float b = ((counter.count == 1 && rootRect.parent == null) ? rootRect.lossyScale.x : rect.localScale.x);
			localPosition.x += 2f * (autoMirrorPositionData.pivot.x - 0.5f) * autoMirrorPositionData.width * Mathf.Min(1f, b);
			rect.localPosition = localPosition;
			Vector3 eulerAngles = autoMirrorPositionData.localRotation.eulerAngles;
			eulerAngles.z = 0f - eulerAngles.z;
			rect.localRotation = Quaternion.Euler(eulerAngles);
			if (flag2)
			{
				ChangePivot(rect, 1f - autoMirrorPositionData.pivot.x);
			}
		}
		ProcessImageOrRawImage(rect);
		ProcessSlider(rect);
		ProcessTextAlign(rect);
		ProcessLayout(rect);
		ProcessTMPInputFieldEx(rect);
		ProcessEffect(rect);
		bool flag3 = IsProcessChildPos(rect);
		foreach (Transform item in rect)
		{
			if (item is RectTransform rectTransform)
			{
				LayoutElement component;
				bool flag4 = item.TryGetComponent<LayoutElement>(out component) && component.ignoreLayout;
				if (flag3 || flag4)
				{
					Mirror(rectTransform, rootRect, preOrderDataList, cullingObjects2, counter, isInnerMirror, isProcessRootAnchorAndPivot, isProcessSelfPos: true);
				}
				else
				{
					Mirror(rectTransform, rectTransform, preOrderDataList, cullingObjects2, counter, isInnerMirror: true, isProcessRootAnchorAndPivot: false, isProcessSelfPos: true);
				}
			}
		}
	}

	private static void ChangePivot(RectTransform rect, float pivotX)
	{
		Vector2 pivot = rect.pivot;
		Vector2 offsetMin = rect.offsetMin;
		Vector2 offsetMax = rect.offsetMax;
		Vector2 anchoredPosition = rect.anchoredPosition;
		rect.pivot = new Vector2(pivotX, rect.pivot.y);
		if ((double)Mathf.Abs(rect.anchorMin.x - rect.anchorMax.x) < 0.01)
		{
			Vector2 vector = rect.pivot - pivot;
			Vector2 vector2 = new Vector2(rect.rect.width * vector.x, 0f);
			Vector2 anchoredPosition2 = anchoredPosition + vector2 * rect.localScale.x;
			rect.anchoredPosition = anchoredPosition2;
		}
		else
		{
			rect.offsetMin = offsetMin;
			rect.offsetMax = offsetMax;
		}
	}

	private static void ProcessImageOrRawImage(RectTransform rect)
	{
		string empty = string.Empty;
		ForceArabicImage component;
		bool flag = rect.TryGetComponent<ForceArabicImage>(out component);
		Image component2;
		bool num = rect.TryGetComponent<Image>(out component2);
		RawImage component3;
		bool flag2 = rect.TryGetComponent<RawImage>(out component3);
		UIPolygonImage component4;
		bool flag3 = rect.TryGetComponent<UIPolygonImage>(out component4);
		bool flag4 = (num || flag3) && flag && component.IsReverseImage;
		bool flag5 = (num || flag3) && flag && !component.IsReverseImage;
		if (num && component2.sprite != null)
		{
			empty = component2.sprite.name;
		}
		else if (flag3 && component4.sprite != null)
		{
			empty = component4.sprite.name;
		}
		bool flag6 = flag2 && (!flag || component.IsReverseImage);
		if (flag2 && component3.texture != null)
		{
			empty = component3.texture.name;
		}
		bool flag7 = !string.IsNullOrEmpty(empty) && AutoReverseImageNameList.IsAutoReverseImage(empty);
		bool flag8 = !string.IsNullOrEmpty(empty) && AutoReverseImageNameList.IsDontAutoReverseRawImage(empty);
		if (flag4 || (flag6 && !flag8) || (flag7 && !flag5))
		{
			rect.gameObject.AddComponent<ArabicImageMirror>().mirrorType = ArabicImageMirror.MirrorType.Horizontal;
		}
	}

	private static void ProcessTMPInputFieldEx(RectTransform rect)
	{
		if (rect.TryGetComponent<TMP_InputFieldEx>(out var component) && component != null)
		{
			component.CheckisArabicLang();
		}
	}

	private static void ProcessSlider(RectTransform rect)
	{
		if (!(rect == null) && rect.TryGetComponent<Slider>(out var component))
		{
			if (component.direction == Slider.Direction.LeftToRight)
			{
				component.direction = Slider.Direction.RightToLeft;
			}
			else if (component.direction == Slider.Direction.RightToLeft)
			{
				component.direction = Slider.Direction.LeftToRight;
			}
		}
	}

	private static void ProcessTextAlign(RectTransform rect)
	{
		if (rect == null || (rect.TryGetComponent<ForceArabicText>(out var component) && component.isControlAlignByTextInAutoMirror))
		{
			return;
		}
		TextMeshProUGUIEx component3;
		if (rect.TryGetComponent<Text>(out var component2))
		{
			TextAnchor alignment = component2.alignment;
			switch (component2.alignment)
			{
			case TextAnchor.LowerLeft:
				alignment = TextAnchor.LowerRight;
				break;
			case TextAnchor.MiddleLeft:
				alignment = TextAnchor.MiddleRight;
				break;
			case TextAnchor.UpperLeft:
				alignment = TextAnchor.UpperRight;
				break;
			case TextAnchor.LowerRight:
				alignment = TextAnchor.LowerLeft;
				break;
			case TextAnchor.MiddleRight:
				alignment = TextAnchor.MiddleLeft;
				break;
			case TextAnchor.UpperRight:
				alignment = TextAnchor.UpperLeft;
				break;
			}
			component2.alignment = alignment;
		}
		else if (rect.TryGetComponent<TextMeshProUGUIEx>(out component3))
		{
			TextAlignmentOptions textAlignmentOptions = component3.alignment;
			if ((textAlignmentOptions & (TextAlignmentOptions)1) != 0)
			{
				textAlignmentOptions &= (TextAlignmentOptions)(-2);
				textAlignmentOptions |= (TextAlignmentOptions)4;
			}
			else if ((textAlignmentOptions & (TextAlignmentOptions)4) != 0)
			{
				textAlignmentOptions &= (TextAlignmentOptions)(-5);
				textAlignmentOptions |= (TextAlignmentOptions)1;
			}
			component3.alignment = textAlignmentOptions;
			float x = component3.margin.x;
			float z = component3.margin.z;
			component3.margin = new Vector4(z, component3.margin.y, x, component3.margin.w);
		}
	}

	private static void ProcessLayout(RectTransform rect)
	{
		if (rect == null)
		{
			return;
		}
		GridLayoutGroup component2;
		GridInfinityScrollView component3;
		BidirectionalHorizontalLayoutGroup component4;
		HorizontalLayoutGroup component6;
		LoopListView2 component7;
		FlowLayoutGroup component8;
		if (rect.TryGetComponent<VerticalLayoutGroup>(out var component) && component.enabled)
		{
			switch (component.childAlignment)
			{
			case TextAnchor.LowerLeft:
				component.childAlignment = TextAnchor.LowerRight;
				break;
			case TextAnchor.LowerRight:
				component.childAlignment = TextAnchor.LowerLeft;
				break;
			case TextAnchor.MiddleLeft:
				component.childAlignment = TextAnchor.MiddleRight;
				break;
			case TextAnchor.MiddleRight:
				component.childAlignment = TextAnchor.MiddleLeft;
				break;
			case TextAnchor.UpperLeft:
				component.childAlignment = TextAnchor.UpperRight;
				break;
			case TextAnchor.UpperRight:
				component.childAlignment = TextAnchor.UpperLeft;
				break;
			}
			int left = component.padding.left;
			int right = component.padding.right;
			component.padding.left = right;
			component.padding.right = left;
		}
		else if (rect.TryGetComponent<GridLayoutGroup>(out component2) && component2.enabled)
		{
			switch (component2.startCorner)
			{
			case GridLayoutGroup.Corner.LowerLeft:
				component2.startCorner = GridLayoutGroup.Corner.LowerRight;
				break;
			case GridLayoutGroup.Corner.LowerRight:
				component2.startCorner = GridLayoutGroup.Corner.LowerLeft;
				break;
			case GridLayoutGroup.Corner.UpperLeft:
				component2.startCorner = GridLayoutGroup.Corner.UpperRight;
				break;
			case GridLayoutGroup.Corner.UpperRight:
				component2.startCorner = GridLayoutGroup.Corner.UpperLeft;
				break;
			}
			switch (component2.childAlignment)
			{
			case TextAnchor.LowerLeft:
				component2.childAlignment = TextAnchor.LowerRight;
				break;
			case TextAnchor.LowerRight:
				component2.childAlignment = TextAnchor.LowerLeft;
				break;
			case TextAnchor.MiddleLeft:
				component2.childAlignment = TextAnchor.MiddleRight;
				break;
			case TextAnchor.MiddleRight:
				component2.childAlignment = TextAnchor.MiddleLeft;
				break;
			case TextAnchor.UpperLeft:
				component2.childAlignment = TextAnchor.UpperRight;
				break;
			case TextAnchor.UpperRight:
				component2.childAlignment = TextAnchor.UpperLeft;
				break;
			}
			int left2 = component2.padding.left;
			int right2 = component2.padding.right;
			component2.padding.left = right2;
			component2.padding.right = left2;
		}
		else if (rect.TryGetComponent<GridInfinityScrollView>(out component3) && component3.enabled)
		{
			component3.isRTLInArabic = true;
		}
		else if (rect.TryGetComponent<BidirectionalHorizontalLayoutGroup>(out component4) && component4.enabled)
		{
			ForceArabicBiHorizontalLayout component5;
			bool flag = rect.TryGetComponent<ForceArabicBiHorizontalLayout>(out component5);
			component4.IsReverse = !flag || component5.IsReverseHorizontalLayout;
			int left3 = component4.padding.left;
			int right3 = component4.padding.right;
			component4.padding.left = right3;
			component4.padding.right = left3;
		}
		else if (rect.TryGetComponent<HorizontalLayoutGroup>(out component6) && component6.enabled)
		{
			switch (component6.childAlignment)
			{
			case TextAnchor.LowerLeft:
				component6.childAlignment = TextAnchor.LowerRight;
				break;
			case TextAnchor.LowerRight:
				component6.childAlignment = TextAnchor.LowerLeft;
				break;
			case TextAnchor.MiddleLeft:
				component6.childAlignment = TextAnchor.MiddleRight;
				break;
			case TextAnchor.MiddleRight:
				component6.childAlignment = TextAnchor.MiddleLeft;
				break;
			case TextAnchor.UpperLeft:
				component6.childAlignment = TextAnchor.UpperRight;
				break;
			case TextAnchor.UpperRight:
				component6.childAlignment = TextAnchor.UpperLeft;
				break;
			}
			int left4 = component6.padding.left;
			int right4 = component6.padding.right;
			component6.padding.left = right4;
			component6.padding.right = left4;
		}
		else if (rect.TryGetComponent<LoopListView2>(out component7) && component7.enabled)
		{
			if (component7.ArrangeType == ListItemArrangeType.LeftToRight)
			{
				component7.ArrangeType = ListItemArrangeType.RightToLeft;
			}
			else if (component7.ArrangeType == ListItemArrangeType.RightToLeft)
			{
				component7.ArrangeType = ListItemArrangeType.LeftToRight;
			}
			component7.InitArabicItemPrefabData();
		}
		else if (rect.TryGetComponent<FlowLayoutGroup>(out component8) && component8.enabled)
		{
			switch (component8.childAlignment)
			{
			case TextAnchor.LowerLeft:
				component8.childAlignment = TextAnchor.LowerRight;
				break;
			case TextAnchor.LowerRight:
				component8.childAlignment = TextAnchor.LowerLeft;
				break;
			case TextAnchor.MiddleLeft:
				component8.childAlignment = TextAnchor.MiddleRight;
				break;
			case TextAnchor.MiddleRight:
				component8.childAlignment = TextAnchor.MiddleLeft;
				break;
			case TextAnchor.UpperLeft:
				component8.childAlignment = TextAnchor.UpperRight;
				break;
			case TextAnchor.UpperRight:
				component8.childAlignment = TextAnchor.UpperLeft;
				break;
			}
			int left5 = component8.padding.left;
			int right5 = component8.padding.right;
			component8.padding.left = right5;
			component8.padding.right = left5;
		}
	}

	private static bool IsProcessChildPos(RectTransform rect)
	{
		if (rect == null)
		{
			return false;
		}
		HorizontalLayoutGroup component;
		bool num = rect.gameObject.TryGetComponent<HorizontalLayoutGroup>(out component);
		BidirectionalHorizontalLayoutGroup component2;
		bool flag = rect.gameObject.TryGetComponent<BidirectionalHorizontalLayoutGroup>(out component2);
		GridLayoutGroup component3;
		bool flag2 = rect.gameObject.TryGetComponent<GridLayoutGroup>(out component3);
		if ((!num || !component.enabled) && (!flag || !component2.enabled))
		{
			if (flag2)
			{
				return !component3.enabled;
			}
			return true;
		}
		return false;
	}

	private static bool IsProcessSelf(RectTransform rect, List<GameObject> cullingObjects)
	{
		if (rect == null || cullingObjects.Contains(rect.gameObject))
		{
			return false;
		}
		return true;
	}

	private static void ProcessEffect(RectTransform rect)
	{
		ForceArabicEffect component;
		bool flag = rect.TryGetComponent<ForceArabicEffect>(out component);
		bool num = rect.GetComponent<ParticleSystem>() != null;
		bool flag2 = rect.GetComponent<SkeletonGraphic>() != null;
		if ((num || flag2) && flag && component.IsReverseEffect)
		{
			ArabicEffectMirror arabicEffectMirror = rect.gameObject.GetComponent<ArabicEffectMirror>();
			if (arabicEffectMirror == null)
			{
				arabicEffectMirror = rect.gameObject.AddComponent<ArabicEffectMirror>();
			}
			arabicEffectMirror.mirrorType = ArabicEffectMirror.MirrorType.Horizontal;
		}
		if (rect.TryGetComponent<ArabicEffectReverseScaleMirror>(out var component2))
		{
			component2.CanReverseScale = true;
		}
	}

	private void EnsureDataListSize(int index)
	{
		if (index >= 0 && index >= mirrorObjectDatas.Count)
		{
			for (int i = mirrorObjectDatas.Count - 1; i < index; i++)
			{
				mirrorObjectDatas.Add(null);
			}
		}
	}

	public void WriteData(int index, GameObject gameObject)
	{
		EnsureDataListSize(index);
		mirrorObjectDatas[index] = mirrorObjectDatas[index] ?? new SingleMirrorObjectData();
		mirrorObjectDatas[index].mirrorData = new MirrorPositionData(gameObject);
		mirrorObjectDatas[index].isUsingMirrorData = true;
	}

	public void RecordOriginalData(int index, GameObject gameObject)
	{
		EnsureDataListSize(index);
		mirrorObjectDatas[index] = mirrorObjectDatas[index] ?? new SingleMirrorObjectData();
		mirrorObjectDatas[index].originalData = new MirrorPositionData(gameObject);
		mirrorObjectDatas[index].isUsingMirrorData = false;
	}

	public bool SwitchData(int index, GameObject gameObject)
	{
		if (index < 0 || index >= mirrorObjectDatas.Count)
		{
			return false;
		}
		SingleMirrorObjectData singleMirrorObjectData = mirrorObjectDatas[index];
		if (singleMirrorObjectData.isUsingMirrorData)
		{
			if (singleMirrorObjectData.originalData != null)
			{
				ApplyDataToGameObject(gameObject, singleMirrorObjectData.originalData);
				singleMirrorObjectData.isUsingMirrorData = false;
				return true;
			}
		}
		else if (singleMirrorObjectData.mirrorData != null)
		{
			singleMirrorObjectData.originalData = new MirrorPositionData(gameObject);
			ApplyDataToGameObject(gameObject, singleMirrorObjectData.mirrorData);
			singleMirrorObjectData.isUsingMirrorData = true;
			return true;
		}
		return false;
	}

	public void ClearData(int index)
	{
		if (index > 0 && index < mirrorObjects.Count)
		{
			mirrorObjects[index] = null;
			mirrorObjectDatas[index] = null;
		}
	}

	private static void ApplyDataToGameObject(GameObject gameObject, MirrorPositionData data)
	{
		if (gameObject != null && data != null && gameObject.transform is RectTransform rectTransform)
		{
			rectTransform.anchoredPosition = data.anchoredPosition;
			rectTransform.anchorMin = data.anchorMin;
			rectTransform.anchorMax = data.anchorMax;
			rectTransform.pivot = data.pivot;
			rectTransform.localRotation = data.localRotation;
			rectTransform.localScale = data.localScale;
		}
	}
}
