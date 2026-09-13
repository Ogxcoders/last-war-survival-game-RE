using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class HierarchyDragDropListener : MonoBehaviour, IDropHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	private const float POINTER_VALIDATE_INTERVAL = 5f;

	[SerializeField]
	private float siblingIndexModificationArea = 5f;

	[SerializeField]
	private float scrollableArea = 75f;

	private float _1OverScrollableArea;

	[SerializeField]
	private float scrollSpeed = 75f;

	[Header("Internal Variables")]
	[SerializeField]
	private RuntimeHierarchy hierarchy;

	[SerializeField]
	private RectTransform content;

	[SerializeField]
	private Image dragDropTargetVisualization;

	private Canvas canvas;

	private RectTransform rectTransform;

	private float height;

	private PointerEventData pointer;

	private Camera worldCamera;

	private float pointerLastYPos;

	private float nextPointerValidation;

	private void Start()
	{
		rectTransform = (RectTransform)base.transform;
		canvas = hierarchy.GetComponentInParent<Canvas>();
		_1OverScrollableArea = 1f / scrollableArea;
	}

	private void OnRectTransformDimensionsChange()
	{
		height = 0f;
	}

	private void Update()
	{
		if (pointer == null)
		{
			return;
		}
		nextPointerValidation -= Time.unscaledDeltaTime;
		if (nextPointerValidation <= 0f)
		{
			nextPointerValidation = 5f;
			if (!pointer.IsPointerValid())
			{
				pointer = null;
				return;
			}
		}
		if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, pointer.position, worldCamera, out var localPoint) || localPoint.y == pointerLastYPos)
		{
			return;
		}
		pointerLastYPos = 0f - localPoint.y;
		if (height <= 0f)
		{
			height = rectTransform.rect.height;
		}
		float num = 0f;
		float num2 = pointerLastYPos;
		if (pointerLastYPos < scrollableArea)
		{
			num = (scrollableArea - pointerLastYPos) * _1OverScrollableArea;
		}
		else if (pointerLastYPos > height - scrollableArea)
		{
			num = (height - scrollableArea - num2) * _1OverScrollableArea;
		}
		float num3 = pointerLastYPos + content.anchoredPosition.y;
		if (num3 < 0f)
		{
			if (dragDropTargetVisualization.gameObject.activeSelf)
			{
				dragDropTargetVisualization.gameObject.SetActive(value: false);
			}
			hierarchy.AutoScrollSpeed = 0f;
			return;
		}
		if (num3 < (float)(hierarchy.ItemCount * hierarchy.Skin.LineHeight))
		{
			if (!dragDropTargetVisualization.gameObject.activeSelf)
			{
				dragDropTargetVisualization.rectTransform.SetAsLastSibling();
				dragDropTargetVisualization.gameObject.SetActive(value: true);
			}
			float num4 = num3 % (float)hierarchy.Skin.LineHeight;
			float num5 = 0f - num3 + num4;
			if (num4 < siblingIndexModificationArea)
			{
				dragDropTargetVisualization.rectTransform.anchoredPosition = new Vector2(0f, num5 + 2f);
				dragDropTargetVisualization.rectTransform.sizeDelta = new Vector2(20f, 4f);
			}
			else if (num4 > (float)hierarchy.Skin.LineHeight - siblingIndexModificationArea)
			{
				dragDropTargetVisualization.rectTransform.anchoredPosition = new Vector2(0f, num5 - (float)hierarchy.Skin.LineHeight + 2f);
				dragDropTargetVisualization.rectTransform.sizeDelta = new Vector2(20f, 4f);
			}
			else
			{
				dragDropTargetVisualization.rectTransform.anchoredPosition = new Vector2(0f, num5);
				dragDropTargetVisualization.rectTransform.sizeDelta = new Vector2(20f, hierarchy.Skin.LineHeight);
			}
		}
		else if (dragDropTargetVisualization.gameObject.activeSelf)
		{
			dragDropTargetVisualization.gameObject.SetActive(value: false);
		}
		hierarchy.AutoScrollSpeed = num * scrollSpeed;
	}

	void IDropHandler.OnDrop(PointerEventData eventData)
	{
		((IPointerExitHandler)this).OnPointerExit(eventData);
		if (!hierarchy.CanReorganizeItems || hierarchy.IsInSearchMode)
		{
			return;
		}
		Transform[] assignableObjectsFromDraggedReferenceItem = RuntimeInspectorUtils.GetAssignableObjectsFromDraggedReferenceItem<Transform>(eventData);
		if (assignableObjectsFromDraggedReferenceItem == null || assignableObjectsFromDraggedReferenceItem.Length == 0)
		{
			return;
		}
		if (assignableObjectsFromDraggedReferenceItem.Length > 1)
		{
			Array.Sort(assignableObjectsFromDraggedReferenceItem, (Transform t, Transform t2) => CompareHierarchySiblingIndices(t, t2));
		}
		bool flag = false;
		float num = pointerLastYPos + content.anchoredPosition.y;
		int num2 = (int)num / hierarchy.Skin.LineHeight;
		HierarchyData hierarchyData = hierarchy.GetDataAt(num2);
		if (hierarchyData == null)
		{
			for (int num3 = 0; num3 < assignableObjectsFromDraggedReferenceItem.Length; num3++)
			{
				if (assignableObjectsFromDraggedReferenceItem[num3].parent != null)
				{
					assignableObjectsFromDraggedReferenceItem[num3].SetParent(null, worldPositionStays: true);
					flag = true;
				}
			}
			if (!flag)
			{
				return;
			}
		}
		else
		{
			float num4 = num % (float)hierarchy.Skin.LineHeight;
			int num5 = ((num4 < siblingIndexModificationArea) ? (-1) : ((num4 > (float)hierarchy.Skin.LineHeight - siblingIndexModificationArea) ? 1 : 0));
			if (num5 != 0 && !(hierarchyData is HierarchyDataTransform))
			{
				if (num5 < 0 && num2 > 0)
				{
					HierarchyData dataAt = hierarchy.GetDataAt(num2 - 1);
					if (dataAt != null)
					{
						hierarchyData = dataAt;
						num5 = 1;
					}
				}
				else if (num5 > 0 && num2 < hierarchy.ItemCount - 1)
				{
					HierarchyData dataAt2 = hierarchy.GetDataAt(num2 + 1);
					if (dataAt2 != null && dataAt2 is HierarchyDataTransform)
					{
						hierarchyData = dataAt2;
						num5 = -1;
					}
				}
			}
			HierarchyDataRoot newScene = null;
			Transform transform = null;
			int num6 = -1;
			if (!(hierarchyData is HierarchyDataTransform))
			{
				newScene = (HierarchyDataRoot)hierarchyData;
			}
			else
			{
				transform = ((HierarchyDataTransform)hierarchyData).BoundTransform;
				if (!transform)
				{
					return;
				}
				if (num5 != 0)
				{
					if (num5 > 0 && hierarchyData.Height > 1)
					{
						num6 = 0;
					}
					else if (hierarchyData.Depth == 1 && hierarchyData.Root is HierarchyDataRootPseudoScene)
					{
						num6 = ((num5 >= 0) ? (((HierarchyDataRootPseudoScene)hierarchyData.Root).IndexOf(transform) + 1) : ((HierarchyDataRootPseudoScene)hierarchyData.Root).IndexOf(transform));
						transform = null;
					}
					else
					{
						num6 = ((num5 >= 0) ? (transform.GetSiblingIndex() + 1) : transform.GetSiblingIndex());
						transform = transform.parent;
					}
				}
				if (!transform)
				{
					newScene = hierarchyData.Root;
				}
			}
			int num7 = 0;
			for (int num8 = 0; num8 < assignableObjectsFromDraggedReferenceItem.Length; num8++)
			{
				if (DropTransformOnto(assignableObjectsFromDraggedReferenceItem[num8], hierarchyData, newScene, transform, (num6 >= 0) ? (num6 + num7) : num6, out var decrementSiblingIndex, out var shouldFocusObjectInHierarchy))
				{
					num7++;
					flag = flag || shouldFocusObjectInHierarchy;
					if (decrementSiblingIndex)
					{
						num6--;
					}
				}
			}
			if (num7 == 0)
			{
				return;
			}
		}
		if (flag)
		{
			hierarchy.SelectInternal(assignableObjectsFromDraggedReferenceItem, RuntimeHierarchy.SelectOptions.FocusOnSelection | RuntimeHierarchy.SelectOptions.ForceRevealSelection);
		}
		else
		{
			hierarchy.Refresh();
		}
	}

	private bool DropTransformOnto(Transform droppedTransform, HierarchyData target, HierarchyDataRoot newScene, Transform newParent, int newSiblingIndex, out bool decrementSiblingIndex, out bool shouldFocusObjectInHierarchy)
	{
		shouldFocusObjectInHierarchy = false;
		decrementSiblingIndex = false;
		if (droppedTransform.parent == newParent)
		{
			if ((bool)newParent || (newScene is HierarchyDataRootScene && ((HierarchyDataRootScene)newScene).Scene == droppedTransform.gameObject.scene))
			{
				if (newSiblingIndex > droppedTransform.GetSiblingIndex())
				{
					newSiblingIndex--;
					decrementSiblingIndex = true;
				}
			}
			else if (newScene is HierarchyDataRootPseudoScene)
			{
				int num = newScene.IndexOf(droppedTransform);
				if (num >= 0 && newSiblingIndex > num)
				{
					newSiblingIndex--;
					decrementSiblingIndex = true;
				}
			}
		}
		if ((bool)newParent)
		{
			if (!hierarchy.CanDropDraggedParentOnChild)
			{
				if (newParent.IsChildOf(droppedTransform))
				{
					return false;
				}
			}
			else
			{
				Transform transform = newParent;
				while (transform.parent != null && transform.parent != droppedTransform)
				{
					transform = transform.parent;
				}
				if (transform.parent == droppedTransform)
				{
					int index;
					if (target.Root is HierarchyDataRootPseudoScene hierarchyDataRootPseudoScene && (index = hierarchyDataRootPseudoScene.IndexOf(droppedTransform)) >= 0 && hierarchy.CanDropDraggedObjectsToPseudoScenes)
					{
						hierarchyDataRootPseudoScene.InsertChild(index, transform);
						hierarchyDataRootPseudoScene.RemoveChild(droppedTransform);
					}
					int siblingIndex = droppedTransform.GetSiblingIndex();
					transform.SetParent(droppedTransform.parent, worldPositionStays: true);
					transform.SetSiblingIndex(siblingIndex);
					shouldFocusObjectInHierarchy = true;
				}
			}
			droppedTransform.SetParent(newParent, worldPositionStays: true);
		}
		else if (newScene is HierarchyDataRootPseudoScene)
		{
			if (!hierarchy.CanDropDraggedObjectsToPseudoScenes)
			{
				return false;
			}
			if (newSiblingIndex < 0)
			{
				((HierarchyDataRootPseudoScene)newScene).AddChild(droppedTransform);
			}
			else
			{
				((HierarchyDataRootPseudoScene)newScene).InsertChild(newSiblingIndex, droppedTransform);
				newSiblingIndex = -1;
				target = newScene;
			}
		}
		else if (newScene is HierarchyDataRootScene)
		{
			if (droppedTransform.parent != null)
			{
				droppedTransform.SetParent(null, worldPositionStays: true);
			}
			Scene scene = ((HierarchyDataRootScene)newScene).Scene;
			if (droppedTransform.gameObject.scene != scene)
			{
				SceneManager.MoveGameObjectToScene(droppedTransform.gameObject, scene);
			}
			if (newSiblingIndex < 0)
			{
				newSiblingIndex = scene.rootCount + 1;
				shouldFocusObjectInHierarchy = true;
			}
		}
		if (newSiblingIndex >= 0)
		{
			droppedTransform.SetSiblingIndex(newSiblingIndex);
		}
		shouldFocusObjectInHierarchy |= newSiblingIndex < 0 && !target.IsExpanded;
		return true;
	}

	private int CompareHierarchySiblingIndices(Transform t1, Transform t2)
	{
		Transform parent = t1.parent;
		Transform parent2 = t2.parent;
		if (parent == parent2)
		{
			return t1.GetSiblingIndex() - t2.GetSiblingIndex();
		}
		int i = 0;
		while ((bool)parent)
		{
			i++;
			parent = parent.parent;
		}
		while ((bool)parent2)
		{
			i--;
			parent2 = parent2.parent;
		}
		while (i > 0)
		{
			t1 = t1.parent;
			if (t1 == t2)
			{
				return 1;
			}
			i--;
		}
		for (; i < 0; i++)
		{
			t2 = t2.parent;
			if (t1 == t2)
			{
				return -1;
			}
		}
		while (t1.parent != t2.parent)
		{
			t1 = t1.parent;
			t2 = t2.parent;
		}
		return t1.GetSiblingIndex() - t2.GetSiblingIndex();
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		if (hierarchy.CanReorganizeItems && !hierarchy.IsInSearchMode && (bool)RuntimeInspectorUtils.GetAssignableObjectFromDraggedReferenceItem<Transform>(eventData))
		{
			pointer = eventData;
			pointerLastYPos = -1f;
			nextPointerValidation = 5f;
			if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || (canvas.renderMode == RenderMode.ScreenSpaceCamera && !canvas.worldCamera))
			{
				worldCamera = null;
			}
			else
			{
				worldCamera = (canvas.worldCamera ? canvas.worldCamera : Camera.main);
			}
			Update();
		}
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		pointer = null;
		worldCamera = null;
		if (dragDropTargetVisualization.gameObject.activeSelf)
		{
			dragDropTargetVisualization.gameObject.SetActive(value: false);
		}
		hierarchy.AutoScrollSpeed = 0f;
	}
}
