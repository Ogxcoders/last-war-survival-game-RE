using UnityEngine;

public interface ITouchObjectEndDragHandler : ITouchObject
{
	bool OnEndDrag(Vector3 dragStopPos);
}
