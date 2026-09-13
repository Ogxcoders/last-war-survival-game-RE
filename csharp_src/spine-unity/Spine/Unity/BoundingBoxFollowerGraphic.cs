using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity;

[ExecuteAlways]
[HelpURL("http://esotericsoftware.com/spine-unity#BoundingBoxFollowerGraphic")]
public class BoundingBoxFollowerGraphic : MonoBehaviour
{
	internal static bool DebugMessages = true;

	public SkeletonGraphic skeletonGraphic;

	[SpineSlot("", "skeletonGraphic", true, true, false)]
	public string slotName;

	public bool isTrigger;

	public bool usedByEffector;

	public bool usedByComposite;

	public bool clearStateOnDisable = true;

	private Slot slot;

	private BoundingBoxAttachment currentAttachment;

	private string currentAttachmentName;

	private PolygonCollider2D currentCollider;

	public readonly Dictionary<BoundingBoxAttachment, PolygonCollider2D> colliderTable = new Dictionary<BoundingBoxAttachment, PolygonCollider2D>();

	public readonly Dictionary<BoundingBoxAttachment, string> nameTable = new Dictionary<BoundingBoxAttachment, string>();

	public Slot Slot => slot;

	public BoundingBoxAttachment CurrentAttachment => currentAttachment;

	public string CurrentAttachmentName => currentAttachmentName;

	public PolygonCollider2D CurrentCollider => currentCollider;

	public bool IsTrigger => isTrigger;

	private void Start()
	{
		Initialize();
	}

	private void OnEnable()
	{
		if (skeletonGraphic != null)
		{
			skeletonGraphic.OnRebuild -= HandleRebuild;
			skeletonGraphic.OnRebuild += HandleRebuild;
		}
		Initialize();
	}

	private void HandleRebuild(SkeletonGraphic sr)
	{
		Initialize();
	}

	public void Initialize(bool overwrite = false)
	{
		if (skeletonGraphic == null)
		{
			return;
		}
		skeletonGraphic.Initialize(overwrite: false);
		if (string.IsNullOrEmpty(slotName) || (!overwrite && colliderTable.Count > 0 && slot != null && skeletonGraphic.Skeleton == slot.Skeleton && slotName == slot.Data.Name))
		{
			return;
		}
		slot = null;
		currentAttachment = null;
		currentAttachmentName = null;
		currentCollider = null;
		colliderTable.Clear();
		nameTable.Clear();
		Skeleton skeleton = skeletonGraphic.Skeleton;
		if (skeleton == null)
		{
			return;
		}
		slot = skeleton.FindSlot(slotName);
		if (slot == null)
		{
			if (DebugMessages)
			{
				Debug.LogWarning($"Slot '{slotName}' not found for BoundingBoxFollowerGraphic on '{base.gameObject.name}'. (Previous colliders were disposed.)");
			}
			return;
		}
		int index = slot.Data.Index;
		int collidersCount = 0;
		PolygonCollider2D[] components = GetComponents<PolygonCollider2D>();
		if (base.gameObject.activeInHierarchy)
		{
			Canvas canvas = skeletonGraphic.canvas;
			if (canvas == null)
			{
				canvas = skeletonGraphic.GetComponentInParent<Canvas>();
			}
			float scale = ((canvas != null) ? canvas.referencePixelsPerUnit : 100f);
			foreach (Skin skin in skeleton.Data.Skins)
			{
				AddCollidersForSkin(skin, index, components, scale, ref collidersCount);
			}
			if (skeleton.Skin != null)
			{
				AddCollidersForSkin(skeleton.Skin, index, components, scale, ref collidersCount);
			}
		}
		DisposeExcessCollidersAfter(collidersCount);
		if (DebugMessages && colliderTable.Count == 0)
		{
			if (base.gameObject.activeInHierarchy)
			{
				Debug.LogWarning("Bounding Box Follower not valid! Slot [" + slotName + "] does not contain any Bounding Box Attachments!");
			}
			else
			{
				Debug.LogWarning("Bounding Box Follower tried to rebuild as a prefab.");
			}
		}
	}

	private void AddCollidersForSkin(Skin skin, int slotIndex, PolygonCollider2D[] previousColliders, float scale, ref int collidersCount)
	{
		if (skin == null)
		{
			return;
		}
		List<Skin.SkinEntry> list = new List<Skin.SkinEntry>();
		skin.GetAttachments(slotIndex, list);
		foreach (Skin.SkinEntry item in list)
		{
			Attachment attachment = skin.GetAttachment(slotIndex, item.Name);
			BoundingBoxAttachment boundingBoxAttachment = attachment as BoundingBoxAttachment;
			if (DebugMessages && attachment != null && boundingBoxAttachment == null)
			{
				Debug.Log("BoundingBoxFollowerGraphic tried to follow a slot that contains non-boundingbox attachments: " + slotName);
			}
			if (boundingBoxAttachment != null && !colliderTable.ContainsKey(boundingBoxAttachment))
			{
				PolygonCollider2D polygonCollider2D = ((collidersCount < previousColliders.Length) ? previousColliders[collidersCount] : base.gameObject.AddComponent<PolygonCollider2D>());
				collidersCount++;
				SkeletonUtility.SetColliderPointsLocal(polygonCollider2D, slot, boundingBoxAttachment, scale);
				polygonCollider2D.isTrigger = isTrigger;
				polygonCollider2D.usedByEffector = usedByEffector;
				polygonCollider2D.usedByComposite = usedByComposite;
				polygonCollider2D.enabled = false;
				polygonCollider2D.hideFlags = HideFlags.NotEditable;
				colliderTable.Add(boundingBoxAttachment, polygonCollider2D);
				nameTable.Add(boundingBoxAttachment, item.Name);
			}
		}
	}

	private void OnDisable()
	{
		if (clearStateOnDisable)
		{
			ClearState();
		}
		if (skeletonGraphic != null)
		{
			skeletonGraphic.OnRebuild -= HandleRebuild;
		}
	}

	public void ClearState()
	{
		if (colliderTable != null)
		{
			foreach (PolygonCollider2D value in colliderTable.Values)
			{
				value.enabled = false;
			}
		}
		currentAttachment = null;
		currentAttachmentName = null;
		currentCollider = null;
	}

	private void DisposeExcessCollidersAfter(int requiredCount)
	{
		PolygonCollider2D[] components = GetComponents<PolygonCollider2D>();
		if (components.Length == 0)
		{
			return;
		}
		for (int i = requiredCount; i < components.Length; i++)
		{
			PolygonCollider2D polygonCollider2D = components[i];
			if (polygonCollider2D != null)
			{
				Object.Destroy(polygonCollider2D);
			}
		}
	}

	private void LateUpdate()
	{
		if (slot != null && slot.Attachment != currentAttachment)
		{
			MatchAttachment(slot.Attachment);
		}
	}

	private void MatchAttachment(Attachment attachment)
	{
		BoundingBoxAttachment boundingBoxAttachment = attachment as BoundingBoxAttachment;
		if (DebugMessages && attachment != null && boundingBoxAttachment == null)
		{
			Debug.LogWarning("BoundingBoxFollowerGraphic tried to match a non-boundingbox attachment. It will treat it as null.");
		}
		if (currentCollider != null)
		{
			currentCollider.enabled = false;
		}
		if (boundingBoxAttachment == null)
		{
			currentCollider = null;
			currentAttachment = null;
			currentAttachmentName = null;
			return;
		}
		colliderTable.TryGetValue(boundingBoxAttachment, out var value);
		if (value != null)
		{
			currentCollider = value;
			currentCollider.enabled = true;
			currentAttachment = boundingBoxAttachment;
			currentAttachmentName = nameTable[boundingBoxAttachment];
			return;
		}
		currentCollider = null;
		currentAttachment = boundingBoxAttachment;
		currentAttachmentName = null;
		if (DebugMessages)
		{
			Debug.LogFormat("Collider for BoundingBoxAttachment named '{0}' was not initialized. It is possibly from a new skin. currentAttachmentName will be null. You may need to call BoundingBoxFollowerGraphic.Initialize(overwrite: true);", boundingBoxAttachment.Name);
		}
	}
}
