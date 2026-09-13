using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity;

[ExecuteAlways]
[RequireComponent(typeof(ISkeletonAnimation))]
[HelpURL("http://esotericsoftware.com/spine-unity#SkeletonUtility")]
public sealed class SkeletonUtility : MonoBehaviour
{
	public delegate void SkeletonUtilityDelegate();

	public Transform boneRoot;

	public bool flipBy180DegreeRotation;

	[HideInInspector]
	public SkeletonRenderer skeletonRenderer;

	[HideInInspector]
	public SkeletonGraphic skeletonGraphic;

	private Canvas canvas;

	[NonSerialized]
	public ISkeletonAnimation skeletonAnimation;

	private ISkeletonComponent skeletonComponent;

	[NonSerialized]
	public List<SkeletonUtilityBone> boneComponents = new List<SkeletonUtilityBone>();

	[NonSerialized]
	public List<SkeletonUtilityConstraint> constraintComponents = new List<SkeletonUtilityConstraint>();

	private float positionScale = 1f;

	private bool hasOverrideBones;

	private bool hasConstraints;

	private bool needToReprocessBones;

	public ISkeletonComponent SkeletonComponent
	{
		get
		{
			if (skeletonComponent == null)
			{
				skeletonComponent = ((skeletonRenderer != null) ? skeletonRenderer.GetComponent<ISkeletonComponent>() : ((skeletonGraphic != null) ? skeletonGraphic.GetComponent<ISkeletonComponent>() : GetComponent<ISkeletonComponent>()));
			}
			return skeletonComponent;
		}
	}

	public Skeleton Skeleton
	{
		get
		{
			if (SkeletonComponent == null)
			{
				return null;
			}
			return skeletonComponent.Skeleton;
		}
	}

	public bool IsValid
	{
		get
		{
			if (!(skeletonRenderer != null) || !skeletonRenderer.valid)
			{
				if (skeletonGraphic != null)
				{
					return skeletonGraphic.IsValid;
				}
				return false;
			}
			return true;
		}
	}

	public float PositionScale => positionScale;

	public event SkeletonUtilityDelegate OnReset;

	public static PolygonCollider2D AddBoundingBoxGameObject(Skeleton skeleton, string skinName, string slotName, string attachmentName, Transform parent, bool isTrigger = true)
	{
		Skin skin = (string.IsNullOrEmpty(skinName) ? skeleton.Data.DefaultSkin : skeleton.Data.FindSkin(skinName));
		if (skin == null)
		{
			Debug.LogError("Skin " + skinName + " not found!");
			return null;
		}
		Slot slot = skeleton.FindSlot(slotName);
		Attachment attachment = ((slot != null) ? skin.GetAttachment(slot.Data.Index, attachmentName) : null);
		if (attachment == null)
		{
			Debug.LogFormat("Attachment in slot '{0}' named '{1}' not found in skin '{2}'.", slotName, attachmentName, skin.Name);
			return null;
		}
		if (attachment is BoundingBoxAttachment boundingBoxAttachment)
		{
			return AddBoundingBoxGameObject(boundingBoxAttachment.Name, boundingBoxAttachment, slot, parent, isTrigger);
		}
		Debug.LogFormat("Attachment '{0}' was not a Bounding Box.", attachmentName);
		return null;
	}

	public static PolygonCollider2D AddBoundingBoxGameObject(string name, BoundingBoxAttachment box, Slot slot, Transform parent, bool isTrigger = true)
	{
		GameObject gameObject = new GameObject("[BoundingBox]" + (string.IsNullOrEmpty(name) ? box.Name : name));
		Transform obj = gameObject.transform;
		obj.parent = parent;
		obj.localPosition = Vector3.zero;
		obj.localRotation = Quaternion.identity;
		obj.localScale = Vector3.one;
		return AddBoundingBoxAsComponent(box, slot, gameObject, isTrigger);
	}

	public static PolygonCollider2D AddBoundingBoxAsComponent(BoundingBoxAttachment box, Slot slot, GameObject gameObject, bool isTrigger = true)
	{
		if (box == null)
		{
			return null;
		}
		PolygonCollider2D polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
		polygonCollider2D.isTrigger = isTrigger;
		SetColliderPointsLocal(polygonCollider2D, slot, box);
		return polygonCollider2D;
	}

	public static void SetColliderPointsLocal(PolygonCollider2D collider, Slot slot, BoundingBoxAttachment box, float scale = 1f)
	{
		if (box == null)
		{
			return;
		}
		if (box.IsWeighted())
		{
			Debug.LogWarning("UnityEngine.PolygonCollider2D does not support weighted or animated points. Collider points will not be animated and may have incorrect orientation. If you want to use it as a collider, please remove weights and animations from the bounding box in Spine editor.");
		}
		Vector2[] localVertices = box.GetLocalVertices(slot, null);
		if (scale != 1f)
		{
			int i = 0;
			for (int num = localVertices.Length; i < num; i++)
			{
				localVertices[i] *= scale;
			}
		}
		collider.SetPath(0, localVertices);
	}

	public static Bounds GetBoundingBoxBounds(BoundingBoxAttachment boundingBox, float depth = 0f)
	{
		float[] vertices = boundingBox.Vertices;
		int num = vertices.Length;
		Bounds result = new Bounds
		{
			center = new Vector3(vertices[0], vertices[1], 0f)
		};
		for (int i = 2; i < num; i += 2)
		{
			result.Encapsulate(new Vector3(vertices[i], vertices[i + 1], 0f));
		}
		Vector3 size = result.size;
		size.z = depth;
		result.size = size;
		return result;
	}

	public static Rigidbody2D AddBoneRigidbody2D(GameObject gameObject, bool isKinematic = true, float gravityScale = 0f)
	{
		Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
		if (rigidbody2D == null)
		{
			rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
			rigidbody2D.isKinematic = isKinematic;
			rigidbody2D.gravityScale = gravityScale;
		}
		return rigidbody2D;
	}

	private void Update()
	{
		Skeleton skeleton = skeletonComponent.Skeleton;
		if (skeleton != null && boneRoot != null)
		{
			if (flipBy180DegreeRotation)
			{
				boneRoot.localScale = new Vector3(Mathf.Abs(skeleton.ScaleX), Mathf.Abs(skeleton.ScaleY), 1f);
				boneRoot.eulerAngles = new Vector3((!(skeleton.ScaleY > 0f)) ? 180 : 0, (!(skeleton.ScaleX > 0f)) ? 180 : 0, 0f);
			}
			else
			{
				boneRoot.localScale = new Vector3(skeleton.ScaleX, skeleton.ScaleY, 1f);
			}
		}
		if (canvas != null)
		{
			positionScale = canvas.referencePixelsPerUnit;
		}
	}

	public void ResubscribeEvents()
	{
		OnDisable();
		OnEnable();
	}

	private void OnEnable()
	{
		if (skeletonRenderer == null)
		{
			skeletonRenderer = GetComponent<SkeletonRenderer>();
		}
		if (skeletonGraphic == null)
		{
			skeletonGraphic = GetComponent<SkeletonGraphic>();
		}
		if (skeletonAnimation == null)
		{
			skeletonAnimation = ((skeletonRenderer != null) ? skeletonRenderer.GetComponent<ISkeletonAnimation>() : ((skeletonGraphic != null) ? skeletonGraphic.GetComponent<ISkeletonAnimation>() : GetComponent<ISkeletonAnimation>()));
		}
		if (skeletonComponent == null)
		{
			skeletonComponent = ((skeletonRenderer != null) ? skeletonRenderer.GetComponent<ISkeletonComponent>() : ((skeletonGraphic != null) ? skeletonGraphic.GetComponent<ISkeletonComponent>() : GetComponent<ISkeletonComponent>()));
		}
		if (skeletonRenderer != null)
		{
			skeletonRenderer.OnRebuild -= HandleRendererReset;
			skeletonRenderer.OnRebuild += HandleRendererReset;
		}
		else if (skeletonGraphic != null)
		{
			skeletonGraphic.OnRebuild -= HandleRendererReset;
			skeletonGraphic.OnRebuild += HandleRendererReset;
			canvas = skeletonGraphic.canvas;
			if (canvas == null)
			{
				canvas = skeletonGraphic.GetComponentInParent<Canvas>();
			}
			if (canvas == null)
			{
				positionScale = 100f;
			}
		}
		if (skeletonAnimation != null)
		{
			skeletonAnimation.UpdateLocal -= UpdateLocal;
			skeletonAnimation.UpdateLocal += UpdateLocal;
		}
		CollectBones();
	}

	private void Start()
	{
		CollectBones();
	}

	private void OnDisable()
	{
		if (skeletonRenderer != null)
		{
			skeletonRenderer.OnRebuild -= HandleRendererReset;
		}
		if (skeletonGraphic != null)
		{
			skeletonGraphic.OnRebuild -= HandleRendererReset;
		}
		if (skeletonAnimation != null)
		{
			skeletonAnimation.UpdateLocal -= UpdateLocal;
			skeletonAnimation.UpdateWorld -= UpdateWorld;
			skeletonAnimation.UpdateComplete -= UpdateComplete;
		}
	}

	private void HandleRendererReset(SkeletonRenderer r)
	{
		if (this.OnReset != null)
		{
			this.OnReset();
		}
		CollectBones();
	}

	private void HandleRendererReset(SkeletonGraphic g)
	{
		if (this.OnReset != null)
		{
			this.OnReset();
		}
		CollectBones();
	}

	public void RegisterBone(SkeletonUtilityBone bone)
	{
		if (!boneComponents.Contains(bone))
		{
			boneComponents.Add(bone);
			needToReprocessBones = true;
		}
	}

	public void UnregisterBone(SkeletonUtilityBone bone)
	{
		boneComponents.Remove(bone);
	}

	public void RegisterConstraint(SkeletonUtilityConstraint constraint)
	{
		if (!constraintComponents.Contains(constraint))
		{
			constraintComponents.Add(constraint);
			needToReprocessBones = true;
		}
	}

	public void UnregisterConstraint(SkeletonUtilityConstraint constraint)
	{
		constraintComponents.Remove(constraint);
	}

	public void CollectBones()
	{
		Skeleton skeleton = skeletonComponent.Skeleton;
		if (skeleton == null)
		{
			return;
		}
		if (boneRoot != null)
		{
			List<object> list = new List<object>();
			ExposedList<IkConstraint> ikConstraints = skeleton.IkConstraints;
			int i = 0;
			for (int count = ikConstraints.Count; i < count; i++)
			{
				list.Add(ikConstraints.Items[i].Target);
			}
			ExposedList<TransformConstraint> transformConstraints = skeleton.TransformConstraints;
			int j = 0;
			for (int count2 = transformConstraints.Count; j < count2; j++)
			{
				list.Add(transformConstraints.Items[j].Target);
			}
			List<SkeletonUtilityBone> list2 = boneComponents;
			int k = 0;
			for (int count3 = list2.Count; k < count3; k++)
			{
				SkeletonUtilityBone skeletonUtilityBone = list2[k];
				if (skeletonUtilityBone.bone == null)
				{
					skeletonUtilityBone.DoUpdate(SkeletonUtilityBone.UpdatePhase.Local);
					if (skeletonUtilityBone.bone == null)
					{
						continue;
					}
				}
				hasOverrideBones |= skeletonUtilityBone.mode == SkeletonUtilityBone.Mode.Override;
				hasConstraints |= list.Contains(skeletonUtilityBone.bone);
			}
			hasConstraints |= constraintComponents.Count > 0;
			if (skeletonAnimation != null)
			{
				skeletonAnimation.UpdateWorld -= UpdateWorld;
				skeletonAnimation.UpdateComplete -= UpdateComplete;
				if (hasOverrideBones || hasConstraints)
				{
					skeletonAnimation.UpdateWorld += UpdateWorld;
				}
				if (hasConstraints)
				{
					skeletonAnimation.UpdateComplete += UpdateComplete;
				}
			}
			needToReprocessBones = false;
		}
		else
		{
			boneComponents.Clear();
			constraintComponents.Clear();
		}
	}

	private void UpdateLocal(ISkeletonAnimation anim)
	{
		if (needToReprocessBones)
		{
			CollectBones();
		}
		List<SkeletonUtilityBone> list = boneComponents;
		if (list != null)
		{
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				list[i].transformLerpComplete = false;
			}
			UpdateAllBones(SkeletonUtilityBone.UpdatePhase.Local);
		}
	}

	private void UpdateWorld(ISkeletonAnimation anim)
	{
		UpdateAllBones(SkeletonUtilityBone.UpdatePhase.World);
		int i = 0;
		for (int count = constraintComponents.Count; i < count; i++)
		{
			constraintComponents[i].DoUpdate();
		}
	}

	private void UpdateComplete(ISkeletonAnimation anim)
	{
		UpdateAllBones(SkeletonUtilityBone.UpdatePhase.Complete);
	}

	private void UpdateAllBones(SkeletonUtilityBone.UpdatePhase phase)
	{
		if (boneRoot == null)
		{
			CollectBones();
		}
		List<SkeletonUtilityBone> list = boneComponents;
		if (list != null)
		{
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				list[i].DoUpdate(phase);
			}
		}
	}

	public Transform GetBoneRoot()
	{
		if (boneRoot != null)
		{
			return boneRoot;
		}
		GameObject gameObject = new GameObject("SkeletonUtility-SkeletonRoot");
		if (skeletonGraphic != null)
		{
			gameObject.AddComponent<RectTransform>();
		}
		boneRoot = gameObject.transform;
		boneRoot.SetParent(base.transform);
		boneRoot.localPosition = Vector3.zero;
		boneRoot.localRotation = Quaternion.identity;
		boneRoot.localScale = Vector3.one;
		return boneRoot;
	}

	public GameObject SpawnRoot(SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
	{
		GetBoneRoot();
		Skeleton skeleton = skeletonComponent.Skeleton;
		GameObject result = SpawnBone(skeleton.RootBone, boneRoot, mode, pos, rot, sca);
		CollectBones();
		return result;
	}

	public GameObject SpawnHierarchy(SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
	{
		GetBoneRoot();
		Skeleton skeleton = skeletonComponent.Skeleton;
		GameObject result = SpawnBoneRecursively(skeleton.RootBone, boneRoot, mode, pos, rot, sca);
		CollectBones();
		return result;
	}

	public GameObject SpawnBoneRecursively(Bone bone, Transform parent, SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
	{
		GameObject gameObject = SpawnBone(bone, parent, mode, pos, rot, sca);
		ExposedList<Bone> children = bone.Children;
		int i = 0;
		for (int count = children.Count; i < count; i++)
		{
			Bone bone2 = children.Items[i];
			SpawnBoneRecursively(bone2, gameObject.transform, mode, pos, rot, sca);
		}
		return gameObject;
	}

	public GameObject SpawnBone(Bone bone, Transform parent, SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
	{
		GameObject gameObject = new GameObject(bone.Data.Name);
		if (skeletonGraphic != null)
		{
			gameObject.AddComponent<RectTransform>();
		}
		Transform transform = gameObject.transform;
		transform.SetParent(parent);
		SkeletonUtilityBone skeletonUtilityBone = gameObject.AddComponent<SkeletonUtilityBone>();
		skeletonUtilityBone.hierarchy = this;
		skeletonUtilityBone.position = pos;
		skeletonUtilityBone.rotation = rot;
		skeletonUtilityBone.scale = sca;
		skeletonUtilityBone.mode = mode;
		skeletonUtilityBone.zPosition = true;
		skeletonUtilityBone.Reset();
		skeletonUtilityBone.bone = bone;
		skeletonUtilityBone.boneName = bone.Data.Name;
		skeletonUtilityBone.valid = true;
		if (mode == SkeletonUtilityBone.Mode.Override)
		{
			if (rot)
			{
				transform.localRotation = Quaternion.Euler(0f, 0f, skeletonUtilityBone.bone.AppliedRotation);
			}
			if (pos)
			{
				transform.localPosition = new Vector3(skeletonUtilityBone.bone.X * positionScale, skeletonUtilityBone.bone.Y * positionScale, 0f);
			}
			transform.localScale = new Vector3(skeletonUtilityBone.bone.ScaleX, skeletonUtilityBone.bone.ScaleY, 0f);
		}
		return gameObject;
	}
}
