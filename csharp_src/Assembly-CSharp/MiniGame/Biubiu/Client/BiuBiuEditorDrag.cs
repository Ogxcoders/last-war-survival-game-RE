using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class BiuBiuEditorDrag : MonoBehaviour
{
	public Vector3 _mousePress;

	public Vector3 _dragStart;

	public Vector3 Position { get; private set; }

	public bool IsDirty { get; private set; }

	public void BeginDrag(Vector3 mousePosition)
	{
		mousePosition = base.transform.parent.InverseTransformPoint(mousePosition);
		_mousePress = mousePosition;
		_dragStart = base.transform.localPosition;
		Position = _dragStart;
	}

	public void UpdateDrag(Vector3 mousePosition)
	{
		mousePosition = base.transform.parent.InverseTransformPoint(mousePosition);
		Position = _dragStart + mousePosition - _mousePress;
		IsDirty = true;
	}

	public void EndDrag()
	{
		IsDirty = false;
	}
}
