using UnityEngine;

[ExecuteInEditMode]
public abstract class XYLayoutElement : MonoBehaviour
{
	protected XYLayoutGroup parentGroup;

	protected Rect localRect;

	public abstract XYLayoutElementType ElementType { get; }

	public Rect LocalRect => localRect;

	public abstract Vector2 Anchor { get; }

	public abstract Vector2 Size { get; }

	public abstract void PresetCenter(Vector2 centerXY);

	public abstract void RefreshLocalPosition(Vector2 parentCenter);

	private void OnEnable()
	{
		TryUpdateParentLayout();
	}

	private void OnTransformParentChanged()
	{
		TryUpdateParentLayout();
	}

	public void TryUpdateParentLayout()
	{
		if (parentGroup != null)
		{
			parentGroup.UnregisterElement(this);
			parentGroup = null;
		}
		parentGroup = base.transform.parent?.GetComponent<XYLayoutGroup>();
		parentGroup?.RegisterElement(this);
	}

	private void OnDisable()
	{
		if (parentGroup != null)
		{
			parentGroup.UnregisterElement(this);
			parentGroup = null;
		}
	}

	protected Rect MergeRect(Rect a, Rect b)
	{
		float xmin = Mathf.Min(a.xMin, b.xMin);
		float ymin = Mathf.Min(a.yMin, b.yMin);
		float xmax = Mathf.Max(a.xMax, b.xMax);
		float ymax = Mathf.Max(a.yMax, b.yMax);
		return Rect.MinMaxRect(xmin, ymin, xmax, ymax);
	}
}
