using UnityEngine;

[ExecuteInEditMode]
public class XYLayoutItem : XYLayoutElement
{
	[SerializeField]
	private Vector2 size = new Vector2(1f, 1f);

	private readonly Vector2 ITEM_ANCHOR = new Vector2(0.5f, 0.5f);

	public override XYLayoutElementType ElementType => XYLayoutElementType.Item;

	public override Vector2 Anchor => ITEM_ANCHOR;

	public override Vector2 Size => size;

	public void UpdateSize(float width, float height)
	{
		size = new Vector2(width, height);
		TryUpdateParentLayout();
	}

	private void Awake()
	{
		localRect.size = size;
	}

	public override void PresetCenter(Vector2 centerXY)
	{
		localRect.Set(0f, 0f, 0f, 0f);
		localRect.size = size;
		localRect.center = centerXY;
	}

	public override void RefreshLocalPosition(Vector2 parentStartPosition)
	{
		base.transform.localPosition = parentStartPosition + localRect.center;
		localRect.center = Vector2.zero;
	}
}
