using System;
using System.Collections.Generic;
using Spine.Unity.AnimationTools;
using UnityEngine;

namespace Spine.Unity;

public abstract class SkeletonRootMotionBase : MonoBehaviour
{
	public struct RootMotionInfo
	{
		public Vector2 start;

		public Vector2 current;

		public Vector2 mid;

		public Vector2 end;

		public bool timeIsPastMid;
	}

	[SpineBone("", "", true, false)]
	[SerializeField]
	protected string rootMotionBoneName = "root";

	public bool transformPositionX = true;

	public bool transformPositionY = true;

	public float rootMotionScaleX = 1f;

	public float rootMotionScaleY = 1f;

	public float rootMotionTranslateXPerY;

	public float rootMotionTranslateYPerX;

	[Header("Optional")]
	public Rigidbody2D rigidBody2D;

	public bool applyRigidbody2DGravity;

	public Rigidbody rigidBody;

	protected ISkeletonComponent skeletonComponent;

	protected Bone rootMotionBone;

	protected int rootMotionBoneIndex;

	protected List<Bone> topLevelBones = new List<Bone>();

	protected Vector2 initialOffset = Vector2.zero;

	protected Vector2 tempSkeletonDisplacement;

	protected Vector2 rigidbodyDisplacement;

	public bool UsesRigidbody
	{
		get
		{
			if (!(rigidBody != null))
			{
				return rigidBody2D != null;
			}
			return true;
		}
	}

	protected virtual float AdditionalScale => 1f;

	protected virtual void Reset()
	{
		FindRigidbodyComponent();
	}

	protected virtual void Start()
	{
		skeletonComponent = GetComponent<ISkeletonComponent>();
		GatherTopLevelBones();
		SetRootMotionBone(rootMotionBoneName);
		if (rootMotionBone != null)
		{
			initialOffset = new Vector2(rootMotionBone.X, rootMotionBone.Y);
		}
		if (skeletonComponent is ISkeletonAnimation skeletonAnimation)
		{
			skeletonAnimation.UpdateLocal -= HandleUpdateLocal;
			skeletonAnimation.UpdateLocal += HandleUpdateLocal;
		}
	}

	protected virtual void FixedUpdate()
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		if (rigidBody2D != null)
		{
			Vector2 vector = Vector2.zero;
			if (applyRigidbody2DGravity)
			{
				float fixedDeltaTime = Time.fixedDeltaTime;
				float num = fixedDeltaTime * fixedDeltaTime;
				rigidBody2D.velocity += rigidBody2D.gravityScale * Physics2D.gravity * fixedDeltaTime;
				vector = 0.5f * rigidBody2D.gravityScale * Physics2D.gravity * num + rigidBody2D.velocity * fixedDeltaTime;
			}
			rigidBody2D.MovePosition(vector + new Vector2(base.transform.position.x, base.transform.position.y) + rigidbodyDisplacement);
		}
		if (rigidBody != null)
		{
			rigidBody.MovePosition(base.transform.position + new Vector3(rigidbodyDisplacement.x, rigidbodyDisplacement.y, 0f));
		}
		GetScaleAffectingRootMotion(out var parentBoneScale);
		ClearEffectiveBoneOffsets(parentBoneScale);
		rigidbodyDisplacement = Vector2.zero;
		tempSkeletonDisplacement = Vector2.zero;
	}

	protected virtual void OnDisable()
	{
		rigidbodyDisplacement = Vector2.zero;
		tempSkeletonDisplacement = Vector2.zero;
	}

	protected void FindRigidbodyComponent()
	{
		rigidBody2D = GetComponent<Rigidbody2D>();
		if (!rigidBody2D)
		{
			rigidBody = GetComponent<Rigidbody>();
		}
		if (!rigidBody2D && !rigidBody)
		{
			rigidBody2D = GetComponentInParent<Rigidbody2D>();
			if (!rigidBody2D)
			{
				rigidBody = GetComponentInParent<Rigidbody>();
			}
		}
	}

	protected abstract Vector2 CalculateAnimationsMovementDelta();

	public abstract Vector2 GetRemainingRootMotion(int trackIndex = 0);

	public abstract RootMotionInfo GetRootMotionInfo(int trackIndex = 0);

	public void SetRootMotionBone(string name)
	{
		Skeleton skeleton = skeletonComponent.Skeleton;
		Bone bone = skeleton.FindBone(name);
		if (bone != null)
		{
			rootMotionBoneIndex = bone.Data.Index;
			rootMotionBone = bone;
		}
		else
		{
			Debug.Log("Bone named \"" + name + "\" could not be found.");
			rootMotionBoneIndex = 0;
			rootMotionBone = skeleton.RootBone;
		}
	}

	public void AdjustRootMotionToDistance(Vector2 distanceToTarget, int trackIndex = 0, bool adjustX = true, bool adjustY = true, float minX = 0f, float maxX = float.MaxValue, float minY = 0f, float maxY = float.MaxValue, bool allowXTranslation = false, bool allowYTranslation = false)
	{
		Vector2 vector = base.transform.InverseTransformVector(distanceToTarget);
		Vector2 scaleAffectingRootMotion = GetScaleAffectingRootMotion();
		if (UsesRigidbody)
		{
			vector -= tempSkeletonDisplacement;
		}
		Vector2 remainingRootMotion = GetRemainingRootMotion(trackIndex);
		remainingRootMotion.Scale(scaleAffectingRootMotion);
		if (remainingRootMotion.x == 0f)
		{
			remainingRootMotion.x = 0.0001f;
		}
		if (remainingRootMotion.y == 0f)
		{
			remainingRootMotion.y = 0.0001f;
		}
		if (adjustX)
		{
			rootMotionScaleX = Math.Min(maxX, Math.Max(minX, vector.x / remainingRootMotion.x));
		}
		if (adjustY)
		{
			rootMotionScaleY = Math.Min(maxY, Math.Max(minY, vector.y / remainingRootMotion.y));
		}
		if (allowXTranslation)
		{
			rootMotionTranslateXPerY = (vector.x - remainingRootMotion.x * rootMotionScaleX) / remainingRootMotion.y;
		}
		if (allowYTranslation)
		{
			rootMotionTranslateYPerX = (vector.y - remainingRootMotion.y * rootMotionScaleY) / remainingRootMotion.x;
		}
	}

	public Vector2 GetAnimationRootMotion(Animation animation)
	{
		return GetAnimationRootMotion(0f, animation.Duration, animation);
	}

	public Vector2 GetAnimationRootMotion(float startTime, float endTime, Animation animation)
	{
		TranslateTimeline translateTimeline = animation.FindTranslateTimelineForBone(rootMotionBoneIndex);
		if (translateTimeline != null)
		{
			return GetTimelineMovementDelta(startTime, endTime, translateTimeline, animation);
		}
		return Vector2.zero;
	}

	public RootMotionInfo GetAnimationRootMotionInfo(Animation animation, float currentTime)
	{
		RootMotionInfo result = default(RootMotionInfo);
		TranslateTimeline translateTimeline = animation.FindTranslateTimelineForBone(rootMotionBoneIndex);
		if (translateTimeline != null)
		{
			float duration = animation.Duration;
			float num = duration * 0.5f;
			result.start = translateTimeline.Evaluate(0f);
			result.current = translateTimeline.Evaluate(currentTime);
			result.mid = translateTimeline.Evaluate(num);
			result.end = translateTimeline.Evaluate(duration);
			result.timeIsPastMid = currentTime > num;
		}
		return result;
	}

	private Vector2 GetTimelineMovementDelta(float startTime, float endTime, TranslateTimeline timeline, Animation animation)
	{
		if (startTime > endTime)
		{
			return timeline.Evaluate(animation.Duration) - timeline.Evaluate(startTime) + (timeline.Evaluate(endTime) - timeline.Evaluate(0f));
		}
		if (startTime != endTime)
		{
			return timeline.Evaluate(endTime) - timeline.Evaluate(startTime);
		}
		return Vector2.zero;
	}

	private void GatherTopLevelBones()
	{
		topLevelBones.Clear();
		foreach (Bone bone in skeletonComponent.Skeleton.Bones)
		{
			if (bone.Parent == null)
			{
				topLevelBones.Add(bone);
			}
		}
	}

	private void HandleUpdateLocal(ISkeletonAnimation animatedSkeletonComponent)
	{
		if (base.isActiveAndEnabled)
		{
			Vector2 boneLocalDelta = CalculateAnimationsMovementDelta();
			Vector2 parentBoneScale;
			Vector2 skeletonSpaceMovementDelta = GetSkeletonSpaceMovementDelta(boneLocalDelta, out parentBoneScale);
			ApplyRootMotion(skeletonSpaceMovementDelta, parentBoneScale);
		}
	}

	private void ApplyRootMotion(Vector2 skeletonDelta, Vector2 parentBoneScale)
	{
		if (UsesRigidbody)
		{
			rigidbodyDisplacement += (Vector2)base.transform.TransformVector(skeletonDelta);
			tempSkeletonDisplacement += skeletonDelta;
			SetEffectiveBoneOffsetsTo(tempSkeletonDisplacement, parentBoneScale);
		}
		else
		{
			base.transform.position += base.transform.TransformVector(skeletonDelta);
			ClearEffectiveBoneOffsets(parentBoneScale);
		}
	}

	private Vector2 GetScaleAffectingRootMotion()
	{
		Vector2 parentBoneScale;
		return GetScaleAffectingRootMotion(out parentBoneScale);
	}

	private Vector2 GetScaleAffectingRootMotion(out Vector2 parentBoneScale)
	{
		Skeleton skeleton = skeletonComponent.Skeleton;
		Vector2 one = Vector2.one;
		one.x *= skeleton.ScaleX;
		one.y *= skeleton.ScaleY;
		parentBoneScale = Vector2.one;
		Bone parent = rootMotionBone;
		while ((parent = parent.Parent) != null)
		{
			parentBoneScale.x *= parent.ScaleX;
			parentBoneScale.y *= parent.ScaleY;
		}
		one = Vector2.Scale(one, parentBoneScale);
		return one * AdditionalScale;
	}

	private Vector2 GetSkeletonSpaceMovementDelta(Vector2 boneLocalDelta, out Vector2 parentBoneScale)
	{
		Vector2 result = boneLocalDelta;
		Vector2 scaleAffectingRootMotion = GetScaleAffectingRootMotion(out parentBoneScale);
		result.Scale(scaleAffectingRootMotion);
		Vector2 vector = new Vector2(rootMotionTranslateXPerY * result.y, rootMotionTranslateYPerX * result.x);
		result.x *= rootMotionScaleX;
		result.y *= rootMotionScaleY;
		result.x += vector.x;
		result.y += vector.y;
		if (!transformPositionX)
		{
			result.x = 0f;
		}
		if (!transformPositionY)
		{
			result.y = 0f;
		}
		return result;
	}

	private void SetEffectiveBoneOffsetsTo(Vector2 displacementSkeletonSpace, Vector2 parentBoneScale)
	{
		Skeleton skeleton = skeletonComponent.Skeleton;
		foreach (Bone topLevelBone in topLevelBones)
		{
			if (topLevelBone == rootMotionBone)
			{
				if (transformPositionX)
				{
					topLevelBone.X = displacementSkeletonSpace.x / skeleton.ScaleX;
				}
				if (transformPositionY)
				{
					topLevelBone.Y = displacementSkeletonSpace.y / skeleton.ScaleY;
				}
				continue;
			}
			float num = (initialOffset.x - rootMotionBone.X) * parentBoneScale.x;
			float num2 = (initialOffset.y - rootMotionBone.Y) * parentBoneScale.y;
			if (transformPositionX)
			{
				topLevelBone.X = displacementSkeletonSpace.x / skeleton.ScaleX + num;
			}
			if (transformPositionY)
			{
				topLevelBone.Y = displacementSkeletonSpace.y / skeleton.ScaleY + num2;
			}
		}
	}

	private void ClearEffectiveBoneOffsets(Vector2 parentBoneScale)
	{
		SetEffectiveBoneOffsetsTo(Vector2.zero, parentBoneScale);
	}
}
