using System;
using UnityEngine;

namespace Spine.Unity;

[ExecuteAlways]
[AddComponentMenu("Spine/SkeletonUtilityBone")]
[HelpURL("http://esotericsoftware.com/spine-unity#SkeletonUtilityBone")]
public class SkeletonUtilityBone : MonoBehaviour
{
	public enum Mode
	{
		Follow,
		Override
	}

	public enum UpdatePhase
	{
		Local,
		World,
		Complete
	}

	public string boneName;

	public Transform parentReference;

	public Mode mode;

	public bool position;

	public bool rotation;

	public bool scale;

	public bool zPosition = true;

	[Range(0f, 1f)]
	public float overrideAlpha = 1f;

	public SkeletonUtility hierarchy;

	[NonSerialized]
	public Bone bone;

	[NonSerialized]
	public bool transformLerpComplete;

	[NonSerialized]
	public bool valid;

	private Transform cachedTransform;

	private Transform skeletonTransform;

	private bool incompatibleTransformMode;

	public bool IncompatibleTransformMode => incompatibleTransformMode;

	public void Reset()
	{
		bone = null;
		cachedTransform = base.transform;
		valid = hierarchy != null && hierarchy.IsValid;
		if (valid)
		{
			skeletonTransform = hierarchy.transform;
			hierarchy.OnReset -= HandleOnReset;
			hierarchy.OnReset += HandleOnReset;
			DoUpdate(UpdatePhase.Local);
		}
	}

	private void OnEnable()
	{
		if (hierarchy == null)
		{
			hierarchy = base.transform.GetComponentInParent<SkeletonUtility>();
		}
		if (!(hierarchy == null))
		{
			hierarchy.RegisterBone(this);
			hierarchy.OnReset += HandleOnReset;
		}
	}

	private void HandleOnReset()
	{
		Reset();
	}

	private void OnDisable()
	{
		if (hierarchy != null)
		{
			hierarchy.OnReset -= HandleOnReset;
			hierarchy.UnregisterBone(this);
		}
	}

	public void DoUpdate(UpdatePhase phase)
	{
		if (!valid)
		{
			Reset();
			return;
		}
		Skeleton skeleton = hierarchy.Skeleton;
		if (bone == null)
		{
			if (string.IsNullOrEmpty(boneName))
			{
				return;
			}
			bone = skeleton.FindBone(boneName);
			if (bone == null)
			{
				Debug.LogError("Bone not found: " + boneName, this);
				return;
			}
		}
		if (!bone.Active)
		{
			return;
		}
		float positionScale = hierarchy.PositionScale;
		Transform transform = cachedTransform;
		float num = Mathf.Sign(skeleton.ScaleX * skeleton.ScaleY);
		if (mode == Mode.Follow)
		{
			switch (phase)
			{
			case UpdatePhase.Local:
				if (position)
				{
					transform.localPosition = new Vector3(bone.X * positionScale, bone.Y * positionScale, 0f);
				}
				if (rotation)
				{
					if (bone.Data.TransformMode.InheritsRotation())
					{
						transform.localRotation = Quaternion.Euler(0f, 0f, bone.Rotation);
					}
					else
					{
						Vector3 eulerAngles2 = skeletonTransform.rotation.eulerAngles;
						transform.rotation = Quaternion.Euler(eulerAngles2.x, eulerAngles2.y, eulerAngles2.z + bone.WorldRotationX * num);
					}
				}
				if (scale)
				{
					transform.localScale = new Vector3(bone.ScaleX, bone.ScaleY, 1f);
					incompatibleTransformMode = BoneTransformModeIncompatible(bone);
				}
				break;
			case UpdatePhase.World:
			case UpdatePhase.Complete:
				if (position)
				{
					transform.localPosition = new Vector3(bone.AX * positionScale, bone.AY * positionScale, 0f);
				}
				if (rotation)
				{
					if (bone.Data.TransformMode.InheritsRotation())
					{
						transform.localRotation = Quaternion.Euler(0f, 0f, bone.AppliedRotation);
					}
					else
					{
						Vector3 eulerAngles = skeletonTransform.rotation.eulerAngles;
						transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + bone.WorldRotationX * num);
					}
				}
				if (scale)
				{
					transform.localScale = new Vector3(bone.AScaleX, bone.AScaleY, 1f);
					incompatibleTransformMode = BoneTransformModeIncompatible(bone);
				}
				break;
			}
		}
		else
		{
			if (mode != Mode.Override || transformLerpComplete)
			{
				return;
			}
			if (parentReference == null)
			{
				if (position)
				{
					Vector3 vector = transform.localPosition / positionScale;
					bone.X = Mathf.Lerp(bone.X, vector.x, overrideAlpha);
					bone.Y = Mathf.Lerp(bone.Y, vector.y, overrideAlpha);
				}
				if (rotation)
				{
					float appliedRotation = Mathf.LerpAngle(bone.Rotation, transform.localRotation.eulerAngles.z, overrideAlpha);
					bone.Rotation = appliedRotation;
					bone.AppliedRotation = appliedRotation;
				}
				if (scale)
				{
					Vector3 localScale = transform.localScale;
					bone.ScaleX = Mathf.Lerp(bone.ScaleX, localScale.x, overrideAlpha);
					bone.ScaleY = Mathf.Lerp(bone.ScaleY, localScale.y, overrideAlpha);
				}
			}
			else
			{
				if (transformLerpComplete)
				{
					return;
				}
				if (position)
				{
					Vector3 vector2 = parentReference.InverseTransformPoint(transform.position) / positionScale;
					bone.X = Mathf.Lerp(bone.X, vector2.x, overrideAlpha);
					bone.Y = Mathf.Lerp(bone.Y, vector2.y, overrideAlpha);
				}
				if (rotation)
				{
					float appliedRotation2 = Mathf.LerpAngle(bone.Rotation, Quaternion.LookRotation(Vector3.forward, parentReference.InverseTransformDirection(transform.up)).eulerAngles.z, overrideAlpha);
					bone.Rotation = appliedRotation2;
					bone.AppliedRotation = appliedRotation2;
				}
				if (scale)
				{
					Vector3 localScale2 = transform.localScale;
					bone.ScaleX = Mathf.Lerp(bone.ScaleX, localScale2.x, overrideAlpha);
					bone.ScaleY = Mathf.Lerp(bone.ScaleY, localScale2.y, overrideAlpha);
				}
				incompatibleTransformMode = BoneTransformModeIncompatible(bone);
			}
			transformLerpComplete = true;
		}
	}

	public static bool BoneTransformModeIncompatible(Bone bone)
	{
		return !bone.Data.TransformMode.InheritsScale();
	}

	public void AddBoundingBox(string skinName, string slotName, string attachmentName)
	{
		SkeletonUtility.AddBoneRigidbody2D(base.transform.gameObject);
		SkeletonUtility.AddBoundingBoxGameObject(bone.Skeleton, skinName, slotName, attachmentName, base.transform);
	}
}
