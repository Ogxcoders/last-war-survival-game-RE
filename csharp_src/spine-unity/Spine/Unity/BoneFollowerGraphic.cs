using System;
using UnityEngine;

namespace Spine.Unity;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
[AddComponentMenu("Spine/UI/BoneFollowerGraphic")]
[HelpURL("http://esotericsoftware.com/spine-unity#BoneFollowerGraphic")]
public class BoneFollowerGraphic : MonoBehaviour
{
	public SkeletonGraphic skeletonGraphic;

	public bool initializeOnAwake = true;

	[SpineBone("", "skeletonGraphic", true, false)]
	public string boneName;

	public bool followBoneRotation = true;

	[Tooltip("Follows the skeleton's flip state by controlling this Transform's local scale.")]
	public bool followSkeletonFlip = true;

	[Tooltip("Follows the target bone's local scale. BoneFollower cannot inherit world/skewed scale because of UnityEngine.Transform property limitations.")]
	public bool followLocalScale;

	public bool followXYPosition = true;

	public bool followZPosition = true;

	[Tooltip("Applies when 'Follow Skeleton Flip' is disabled but 'Follow Bone Rotation' is enabled. When flipping the skeleton by scaling its Transform, this follower's rotation is adjusted instead of its scale to follow the bone orientation. When one of the axes is flipped,  only one axis can be followed, either the X or the Y axis, which is selected here.")]
	public BoneFollower.AxisOrientation maintainedAxisOrientation = BoneFollower.AxisOrientation.XAxis;

	[NonSerialized]
	public Bone bone;

	private Transform skeletonTransform;

	private bool skeletonTransformIsParent;

	[NonSerialized]
	public bool valid;

	public SkeletonGraphic SkeletonGraphic
	{
		get
		{
			return skeletonGraphic;
		}
		set
		{
			skeletonGraphic = value;
			Initialize();
		}
	}

	public bool SetBone(string name)
	{
		bone = skeletonGraphic.Skeleton.FindBone(name);
		if (bone == null)
		{
			Debug.LogError("Bone not found: " + name, this);
			return false;
		}
		boneName = name;
		return true;
	}

	public void Awake()
	{
		if (initializeOnAwake)
		{
			Initialize();
		}
	}

	public void Initialize()
	{
		bone = null;
		valid = skeletonGraphic != null && skeletonGraphic.IsValid;
		if (valid)
		{
			skeletonTransform = skeletonGraphic.transform;
			skeletonTransformIsParent = (object)skeletonTransform == base.transform.parent;
			if (!string.IsNullOrEmpty(boneName))
			{
				bone = skeletonGraphic.Skeleton.FindBone(boneName);
			}
		}
	}

	public void LateUpdate()
	{
		if (!valid)
		{
			Initialize();
			return;
		}
		if (bone == null)
		{
			if (string.IsNullOrEmpty(boneName))
			{
				return;
			}
			bone = skeletonGraphic.Skeleton.FindBone(boneName);
			if (!SetBone(boneName))
			{
				return;
			}
		}
		RectTransform rectTransform = base.transform as RectTransform;
		if (rectTransform == null)
		{
			return;
		}
		Canvas canvas = skeletonGraphic.canvas;
		if (canvas == null)
		{
			canvas = skeletonGraphic.GetComponentInParent<Canvas>();
		}
		float num = ((canvas != null) ? canvas.referencePixelsPerUnit : 100f);
		float num2 = 1f;
		if (skeletonTransformIsParent)
		{
			rectTransform.localPosition = new Vector3(followXYPosition ? (bone.WorldX * num) : rectTransform.localPosition.x, followXYPosition ? (bone.WorldY * num) : rectTransform.localPosition.y, followZPosition ? 0f : rectTransform.localPosition.z);
			if (followBoneRotation)
			{
				rectTransform.localRotation = bone.GetQuaternion();
			}
		}
		else
		{
			Vector3 position = skeletonTransform.TransformPoint(new Vector3(bone.WorldX * num, bone.WorldY * num, 0f));
			if (!followZPosition)
			{
				position.z = rectTransform.position.z;
			}
			if (!followXYPosition)
			{
				position.x = rectTransform.position.x;
				position.y = rectTransform.position.y;
			}
			Vector3 lossyScale = skeletonTransform.lossyScale;
			Transform parent = rectTransform.parent;
			Vector3 vector = ((parent != null) ? parent.lossyScale : Vector3.one);
			if (followBoneRotation)
			{
				float num3 = bone.WorldRotationX;
				if (lossyScale.x * lossyScale.y < 0f)
				{
					num3 = 0f - num3;
				}
				if (followSkeletonFlip || maintainedAxisOrientation == BoneFollower.AxisOrientation.XAxis)
				{
					if (lossyScale.x * vector.x < 0f)
					{
						num3 += 180f;
					}
				}
				else if (lossyScale.y * vector.y < 0f)
				{
					num3 += 180f;
				}
				Vector3 eulerAngles = skeletonTransform.rotation.eulerAngles;
				if (followLocalScale && bone.ScaleX < 0f)
				{
					num3 += 180f;
				}
				rectTransform.SetPositionAndRotation(position, Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + num3));
			}
			else
			{
				rectTransform.position = position;
			}
			num2 = Mathf.Sign(lossyScale.x * vector.x * lossyScale.y * vector.y);
		}
		Vector3 localScale = (followLocalScale ? new Vector3(bone.ScaleX, bone.ScaleY, 1f) : new Vector3(1f, 1f, 1f));
		if (followSkeletonFlip)
		{
			localScale.y *= Mathf.Sign(bone.Skeleton.ScaleX * bone.Skeleton.ScaleY) * num2;
		}
		rectTransform.localScale = localScale;
	}
}
