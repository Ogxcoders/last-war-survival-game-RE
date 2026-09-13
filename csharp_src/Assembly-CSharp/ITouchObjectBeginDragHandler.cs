using UnityEngine;

public interface ITouchObjectBeginDragHandler : ITouchObject
{
	bool OnBeginDrag(Vector3 dragStartPos);
}
