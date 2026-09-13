using System;
using UnityEngine;

namespace Spine.Unity;

[ExecuteAlways]
[AddComponentMenu("Spine/Point Follower")]
[HelpURL("http://esotericsoftware.com/spine-unity#PointFollower")]
public class PointFollower : MonoBehaviour, IHasSkeletonRenderer, ISpineComponent, IHasSkeletonComponent
{
	public SkeletonRenderer skeletonRenderer;

	[SpineSlot("", "skeletonRenderer", false, true, false)]
	public string slotName;

	[SpineAttachment(true, false, false, "slotName", "skeletonRenderer", "", true, true)]
	public string pointAttachmentName;

	public bool followRotation = true;

	public bool followSkeletonFlip = true;

	public bool followSkeletonZPosition;

	private Transform skeletonTransform;

	private bool skeletonTransformIsParent;

	private PointAttachment point;

	private Bone bone;

	private bool valid;

	public SkeletonRenderer SkeletonRenderer => skeletonRenderer;

	public ISkeletonComponent SkeletonComponent => skeletonRenderer;

	public bool IsValid => valid;

	public void Initialize()
	{
		valid = skeletonRenderer != null && skeletonRenderer.valid;
		if (valid)
		{
			UpdateReferences();
		}
	}

	private void HandleRebuildRenderer(SkeletonRenderer skeletonRenderer)
	{
		Initialize();
	}

	private void UpdateReferences()
	{
		skeletonTransform = skeletonRenderer.transform;
		skeletonRenderer.OnRebuild -= HandleRebuildRenderer;
		skeletonRenderer.OnRebuild += HandleRebuildRenderer;
		skeletonTransformIsParent = (object)skeletonTransform == base.transform.parent;
		bone = null;
		point = null;
		if (!string.IsNullOrEmpty(pointAttachmentName))
		{
			Skeleton skeleton = skeletonRenderer.Skeleton;
			Slot slot = skeleton.FindSlot(slotName);
			if (slot != null)
			{
				int index = slot.Data.Index;
				bone = slot.Bone;
				point = skeleton.GetAttachment(index, pointAttachmentName) as PointAttachment;
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
		if (point == null)
		{
			if (string.IsNullOrEmpty(pointAttachmentName))
			{
				return;
			}
			UpdateReferences();
			if (point == null)
			{
				return;
			}
		}
		Vector2 vector = default(Vector2);
		point.ComputeWorldPosition(bone, out vector.x, out vector.y);
		float num = point.ComputeWorldRotation(bone);
		Transform transform = base.transform;
		if (skeletonTransformIsParent)
		{
			transform.localPosition = new Vector3(vector.x, vector.y, followSkeletonZPosition ? 0f : transform.localPosition.z);
			if (followRotation)
			{
				float f = num * 0.5f * (MathF.PI / 180f);
				transform.localRotation = new Quaternion
				{
					z = Mathf.Sin(f),
					w = Mathf.Cos(f)
				};
			}
		}
		else
		{
			Vector3 position = skeletonTransform.TransformPoint(new Vector3(vector.x, vector.y, 0f));
			if (!followSkeletonZPosition)
			{
				position.z = transform.position.z;
			}
			Transform parent = transform.parent;
			if (parent != null)
			{
				Matrix4x4 localToWorldMatrix = parent.localToWorldMatrix;
				if (localToWorldMatrix.m00 * localToWorldMatrix.m11 - localToWorldMatrix.m01 * localToWorldMatrix.m10 < 0f)
				{
					num = 0f - num;
				}
			}
			if (followRotation)
			{
				Vector3 eulerAngles = skeletonTransform.rotation.eulerAngles;
				transform.SetPositionAndRotation(position, Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + num));
			}
			else
			{
				transform.position = position;
			}
		}
		if (followSkeletonFlip)
		{
			Vector3 localScale = transform.localScale;
			localScale.y = Mathf.Abs(localScale.y) * Mathf.Sign(bone.Skeleton.ScaleX * bone.Skeleton.ScaleY);
			transform.localScale = localScale;
		}
	}
}
