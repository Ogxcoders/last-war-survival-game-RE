using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteAlways]
public class UIDropdownEx : Dropdown
{
	[SerializeField]
	public Color itemColor1 = Color.white;

	[SerializeField]
	public Color itemColor2 = Color.white;

	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);
		ColoredItems();
	}

	public override void OnSubmit(BaseEventData eventData)
	{
		base.OnSubmit(eventData);
		ColoredItems();
	}

	private void ColoredItems()
	{
		Transform transform = base.transform.Find("Dropdown List");
		if (!(transform != null))
		{
			return;
		}
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (child != null)
			{
				Image component = child.GetComponent<Image>();
				if (component != null)
				{
					component.color = ((i % 2 == 0) ? itemColor2 : itemColor1);
				}
			}
		}
	}
}
