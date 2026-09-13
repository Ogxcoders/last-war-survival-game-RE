using System;
using UnityEngine;

namespace SuperScrollView;

[Serializable]
public class ItemPrefabConfData
{
	public GameObject mItemPrefab;

	public float mPadding;

	public int mInitCreateCount;

	public float mStartPosOffset;

	public float mItemHeight;

	public bool mAnimationCancle;

	public string mAnimatorPath = string.Empty;
}
