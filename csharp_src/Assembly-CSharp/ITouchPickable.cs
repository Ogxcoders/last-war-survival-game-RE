using UnityEngine;

public interface ITouchPickable
{
	bool CanLongTap();

	Transform GetTransform();

	T GetPickComponent<T>() where T : MonoBehaviour;

	bool PointInPick();

	void Drag(Vector3 pos);

	bool Select();

	void Click();

	bool IsOutRange(Vector3 pos);

	void ChangeTouchPos(int index);

	Vector3 GetClosestPoint(Vector3 pos);
}
