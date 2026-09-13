using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Spine.Unity;

[ExecuteAlways]
[AddComponentMenu("Spine/BoneFollower")]
[HelpURL("http://esotericsoftware.com/spine-unity#BoneFollower")]
public class BoneFollower : MonoBehaviour
{
	public enum AxisOrientation
	{
		XAxis = 1,
		YAxis
	}

	public SkeletonRenderer skeletonRenderer;

	[SpineBone("", "skeletonRenderer", true, false)]
	public string boneName;

	public bool followXYPosition = true;

	public bool followZPosition = true;

	public bool followBoneRotation = true;

	[Tooltip("Follows the skeleton's flip state by controlling this Transform's local scale.")]
	public bool followSkeletonFlip = true;

	[Tooltip("Follows the target bone's local scale. BoneFollower cannot inherit world/skewed scale because of UnityEngine.Transform property limitations.")]
	[FormerlySerializedAs("followScale")]
	public bool followLocalScale;

	[Tooltip("Applies when 'Follow Skeleton Flip' is disabled but 'Follow Bone Rotation' is enabled. When flipping the skeleton by scaling its Transform, this follower's rotation is adjusted instead of its scale to follow the bone orientation. When one of the axes is flipped,  only one axis can be followed, either the X or the Y axis, which is selected here.")]
	public AxisOrientation maintainedAxisOrientation = AxisOrientation.XAxis;

	[FormerlySerializedAs("resetOnAwake")]
	public bool initializeOnAwake = true;

	[NonSerialized]
	public bool valid;

	[NonSerialized]
	public Bone bone;

	private Transform skeletonTransform;

	private bool skeletonTransformIsParent;

	public SkeletonRenderer SkeletonRenderer
	{
		get
		{
			return skeletonRenderer;
		}
		set
		{
			skeletonRenderer = value;
			Initialize();
		}
	}

	public bool SetBone(string name)
	{
		bone = skeletonRenderer.skeleton.FindBone(name);
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

	public void HandleRebuildRenderer(SkeletonRenderer skeletonRenderer)
	{
		Initialize();
	}

	public void Initialize()
	{
		bone = null;
		valid = skeletonRenderer != null && skeletonRenderer.valid;
		if (valid)
		{
			skeletonTransform = skeletonRenderer.transform;
			skeletonRenderer.OnRebuild -= HandleRebuildRenderer;
			skeletonRenderer.OnRebuild += HandleRebuildRenderer;
			skeletonTransformIsParent = (object)skeletonTransform == base.transform.parent;
			if (!string.IsNullOrEmpty(boneName))
			{
				bone = skeletonRenderer.skeleton.FindBone(boneName);
			}
		}
	}

	private void OnDestroy()
	{
		if (skeletonRenderer != null)
		{
			skeletonRenderer.OnRebuild -= HandleRebuildRenderer;
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
			bone = skeletonRenderer.skeleton.FindBone(boneName);
			if (!SetBone(boneName))
			{
				return;
			}
		}
		Transform transform = base.transform;
		float num = 1f;
		if (skeletonTransformIsParent)
		{
			transform.localPosition = new Vector3(followXYPosition ? bone.WorldX : transform.localPosition.x, followXYPosition ? bone.WorldY : transform.localPosition.y, followZPosition ? 0f : transform.localPosition.z);
			if (followBoneRotation)
			{
				float num2 = Mathf.Atan2(bone.C, bone.A) * 0.5f;
				if (followLocalScale && bone.ScaleX < 0f)
				{
					num2 += MathF.PI / 2f;
				}
				transform.localRotation = new Quaternion
				{
					z = Mathf.Sin(num2),
					w = Mathf.Cos(num2)
				};
			}
		}
		else
		{
			Vector3 position = skeletonTransform.TransformPoint(new Vector3(bone.WorldX, bone.WorldY, 0f));
			if (!followZPosition)
			{
				position.z = transform.position.z;
			}
			if (!followXYPosition)
			{
				position.x = transform.position.x;
				position.y = transform.position.y;
			}
			Vector3 lossyScale = skeletonTransform.lossyScale;
			Transform parent = transform.parent;
			Vector3 vector = ((parent != null) ? parent.lossyScale : Vector3.one);
			if (followBoneRotation)
			{
				float num3 = bone.WorldRotationX;
				if (lossyScale.x * lossyScale.y < 0f)
				{
					num3 = 0f - num3;
				}
				if (followSkeletonFlip || maintainedAxisOrientation == AxisOrientation.XAxis)
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
				transform.SetPositionAndRotation(position, Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + num3));
			}
			else
			{
				transform.position = position;
			}
			num = Mathf.Sign(lossyScale.x * vector.x * lossyScale.y * vector.y);
		}
		Vector3 localScale = (followLocalScale ? new Vector3(bone.ScaleX, bone.ScaleY, 1f) : new Vector3(1f, 1f, 1f));
		if (followSkeletonFlip)
		{
			localScale.y *= Mathf.Sign(bone.Skeleton.ScaleX * bone.Skeleton.ScaleY) * num;
		}
		transform.localScale = localScale;
	}
}
