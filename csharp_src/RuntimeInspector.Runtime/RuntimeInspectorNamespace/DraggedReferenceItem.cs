using UnityEngine.EventSystems;

namespace RuntimeInspectorNamespace;

public class DraggedReferenceItem : PopupBase, IDragHandler, IEventSystemHandler, IEndDragHandler
{
	private object[] m_references;

	public object[] References => m_references;

	public void SetContent(object[] references, PointerEventData draggingPointer)
	{
		m_references = references;
		label.text = ((references.Length == 1) ? references[0].GetNameWithType() : (references[0].GetNameWithType() + " (and " + (references.Length - 1).ToString(RuntimeInspectorUtils.numberFormat) + " more)"));
		draggingPointer.pointerDrag = base.gameObject;
		draggingPointer.dragging = true;
		SetPointer(draggingPointer);
	}

	protected override void DestroySelf()
	{
		RuntimeInspectorUtils.PoolDraggedReferenceItem(this);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (eventData.pointerId == pointer.pointerId)
		{
			RepositionSelf();
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		DestroySelf();
	}
}
