using UnityEngine;

public class WorldCollectObjectView : MonoBehaviour
{
	[SerializeField]
	private UIWorldLabel label;

	private int _pointIndex;

	private ResourceType _resourceType;

	private void Awake()
	{
	}

	public void ShowLabel(bool show)
	{
		label.gameObject.SetActive(show);
	}

	public void SetLevel(int lv)
	{
		label.SetLevel(lv);
	}

	public void SetName(string name)
	{
	}

	public void ShowNameTitle(bool isShow)
	{
		label.ShowNameTitle(isShow);
	}

	private void Update()
	{
	}

	public void Init(int point, ResourceType type)
	{
		_pointIndex = point;
		_resourceType = type;
	}

	public void UnInit()
	{
	}
}
