using System;
using GameFramework.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[AddComponentMenu("UI/阿正水平布局附加控制（Arabic Horizon Layout）")]
public class ArabicHorizonLayout : MonoBehaviour
{
	private enum _HorizontalAlignmentOptions
	{
		Left = 1,
		Center = 2,
		Right = 4,
		Justified = 8,
		Flush = 0x10,
		Geometry = 0x20
	}

	private enum _VerticalAlignmentOptions
	{
		Top = 0x100,
		Middle = 0x200,
		Bottom = 0x400,
		Baseline = 0x800,
		Geometry = 0x1000,
		Capline = 0x2000
	}

	public enum AlignmentType
	{
		NoControl = 1,
		AllLeftAlign,
		AllRightAlign,
		FirstLeftAndLastRight,
		FirstRightAndLastLeft
	}

	public bool IsReverseImage;

	public bool IsControlChildWidth = true;

	public bool IsChildForceExpandWidth = true;

	public AlignmentType TextAlignType = AlignmentType.NoControl;

	public TextAnchor ChildAlignment = TextAnchor.MiddleRight;

	public bool IsDisableChildContentSizeFitter = true;

	[Header("Mirror版本控制")]
	[Tooltip("是否与Mirror版本一起上线")]
	public bool IsControlledByMirror;

	private void Start()
	{
		if ((IsControlledByMirror && !MirrorVersionConfig.IsMirrorVersionOpen) || GameEntry.Localization.Language != Language.Arabic || (TryGetComponent<BidirectionalHorizontalLayoutGroup>(out var _) && MirrorVersionConfig.IsMirrorVersionOpen))
		{
			return;
		}
		HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup = GetComponent<BidirectionalHorizontalLayoutGroup>();
		if (horizontalOrVerticalLayoutGroup == null)
		{
			horizontalOrVerticalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
		}
		if (horizontalOrVerticalLayoutGroup == null)
		{
			horizontalOrVerticalLayoutGroup = base.gameObject.AddComponent<BidirectionalHorizontalLayoutGroup>();
		}
		horizontalOrVerticalLayoutGroup.childAlignment = ChildAlignment;
		horizontalOrVerticalLayoutGroup.childForceExpandWidth = IsChildForceExpandWidth;
		horizontalOrVerticalLayoutGroup.childControlWidth = IsControlChildWidth;
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount / 2; i++)
		{
			Transform child = base.transform.GetChild(i);
			Transform child2 = base.transform.GetChild(childCount - i - 1);
			child.SetSiblingIndex(childCount - i - 1);
			child2.SetSiblingIndex(i);
		}
		int num = childCount - 1;
		int num2 = 0;
		for (int j = 0; j < childCount; j++)
		{
			Transform child3 = base.transform.GetChild(j);
			Text component2;
			bool flag = child3.gameObject.TryGetComponent<Text>(out component2);
			TextMeshProUGUIEx component3;
			bool flag2 = child3.gameObject.TryGetComponent<TextMeshProUGUIEx>(out component3);
			if (child3.gameObject.activeInHierarchy && (flag || flag2))
			{
				num = Math.Min(num, j);
				num2 = Math.Max(num2, j);
			}
			if (child3.gameObject.TryGetComponent<Image>(out var _) || child3.gameObject.TryGetComponent<RawImage>(out var _))
			{
				float x = child3.localScale.x;
				child3.SetLocalScaleX(IsReverseImage ? (0f - x) : x);
			}
			if (TextAlignType != AlignmentType.NoControl)
			{
				if (flag)
				{
					TextAnchor alignment = component2.alignment;
					switch (alignment)
					{
					case TextAnchor.LowerLeft:
					case TextAnchor.LowerCenter:
					case TextAnchor.LowerRight:
						if (TextAlignType == AlignmentType.AllLeftAlign)
						{
							component2.alignment = TextAnchor.LowerLeft;
						}
						else if (TextAlignType == AlignmentType.AllRightAlign)
						{
							component2.alignment = TextAnchor.LowerRight;
						}
						else if (TextAlignType == AlignmentType.FirstLeftAndLastRight)
						{
							component2.alignment = ((j == num) ? TextAnchor.LowerLeft : ((j == num2) ? TextAnchor.LowerRight : alignment));
						}
						else if (TextAlignType == AlignmentType.FirstRightAndLastLeft)
						{
							component2.alignment = ((j == num) ? TextAnchor.LowerRight : ((j == num2) ? TextAnchor.LowerLeft : alignment));
						}
						break;
					case TextAnchor.MiddleLeft:
					case TextAnchor.MiddleCenter:
					case TextAnchor.MiddleRight:
						if (TextAlignType == AlignmentType.AllLeftAlign)
						{
							component2.alignment = TextAnchor.MiddleLeft;
						}
						else if (TextAlignType == AlignmentType.AllRightAlign)
						{
							component2.alignment = TextAnchor.MiddleRight;
						}
						else if (TextAlignType == AlignmentType.FirstLeftAndLastRight)
						{
							component2.alignment = ((j == num) ? TextAnchor.MiddleLeft : ((j == num2) ? TextAnchor.MiddleRight : alignment));
						}
						else if (TextAlignType == AlignmentType.FirstRightAndLastLeft)
						{
							component2.alignment = ((j == num) ? TextAnchor.MiddleRight : ((j == num2) ? TextAnchor.MiddleLeft : alignment));
						}
						break;
					case TextAnchor.UpperLeft:
					case TextAnchor.UpperCenter:
					case TextAnchor.UpperRight:
						if (TextAlignType == AlignmentType.AllLeftAlign)
						{
							component2.alignment = TextAnchor.UpperLeft;
						}
						else if (TextAlignType == AlignmentType.AllRightAlign)
						{
							component2.alignment = TextAnchor.UpperRight;
						}
						else if (TextAlignType == AlignmentType.FirstLeftAndLastRight)
						{
							component2.alignment = ((j != num) ? ((j == num2) ? TextAnchor.UpperRight : alignment) : TextAnchor.UpperLeft);
						}
						else if (TextAlignType == AlignmentType.FirstRightAndLastLeft)
						{
							component2.alignment = ((j == num) ? TextAnchor.UpperRight : ((j != num2) ? alignment : TextAnchor.UpperLeft));
						}
						break;
					}
				}
				else if (flag2)
				{
					TextAlignmentOptions alignment2 = component3.alignment;
					if ((alignment2 & (TextAlignmentOptions)1024) != 0)
					{
						if (TextAlignType == AlignmentType.AllLeftAlign)
						{
							component3.alignment = TextAlignmentOptions.BottomLeft;
						}
						else if (TextAlignType == AlignmentType.AllRightAlign)
						{
							component3.alignment = TextAlignmentOptions.BottomRight;
						}
						else if (TextAlignType == AlignmentType.FirstLeftAndLastRight)
						{
							component3.alignment = ((j == num) ? TextAlignmentOptions.BottomLeft : ((j == num2) ? TextAlignmentOptions.BottomRight : alignment2));
						}
						else if (TextAlignType == AlignmentType.FirstRightAndLastLeft)
						{
							component3.alignment = ((j == num) ? TextAlignmentOptions.BottomRight : ((j == num2) ? TextAlignmentOptions.BottomLeft : alignment2));
						}
					}
					else if ((alignment2 & (TextAlignmentOptions)256) != 0)
					{
						if (TextAlignType == AlignmentType.AllLeftAlign)
						{
							component3.alignment = TextAlignmentOptions.TopLeft;
						}
						else if (TextAlignType == AlignmentType.AllRightAlign)
						{
							component3.alignment = TextAlignmentOptions.TopRight;
						}
						else if (TextAlignType == AlignmentType.FirstLeftAndLastRight)
						{
							component3.alignment = ((j == num) ? TextAlignmentOptions.TopLeft : ((j == num2) ? TextAlignmentOptions.TopRight : alignment2));
						}
						else if (TextAlignType == AlignmentType.FirstRightAndLastLeft)
						{
							component3.alignment = ((j == num) ? TextAlignmentOptions.TopRight : ((j == num2) ? TextAlignmentOptions.TopLeft : alignment2));
						}
					}
					else if (TextAlignType == AlignmentType.AllLeftAlign)
					{
						component3.alignment = TextAlignmentOptions.MidlineLeft;
					}
					else if (TextAlignType == AlignmentType.AllRightAlign)
					{
						component3.alignment = TextAlignmentOptions.MidlineRight;
					}
					else if (TextAlignType == AlignmentType.FirstLeftAndLastRight)
					{
						component3.alignment = ((j == num) ? TextAlignmentOptions.MidlineLeft : ((j == num2) ? TextAlignmentOptions.MidlineRight : alignment2));
					}
					else if (TextAlignType == AlignmentType.FirstRightAndLastLeft)
					{
						component3.alignment = ((j == num) ? TextAlignmentOptions.MidlineRight : ((j == num2) ? TextAlignmentOptions.MidlineLeft : alignment2));
					}
				}
			}
			if (child3.gameObject.TryGetComponent<ContentSizeFitter>(out var component6) && IsDisableChildContentSizeFitter)
			{
				component6.enabled = false;
			}
		}
	}
}
