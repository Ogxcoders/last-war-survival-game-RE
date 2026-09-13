using UnityEngine;

public interface ITouchObjectDragHandler : ITouchObject
{
	bool OnDrag(Vector3 dragStartPos, Vector3 dragCurrPos);
}
