using UnityEngine;

public class ModelHeight : MonoBehaviour
{
	[SerializeField]
	private GameObject _modelObj;

	[SerializeField]
	private float height;

	[SerializeField]
	private float heightDelta;

	public float GetHeight()
	{
		return height + heightDelta;
	}
}
