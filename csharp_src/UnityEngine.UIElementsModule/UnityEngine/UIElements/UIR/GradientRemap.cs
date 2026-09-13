namespace UnityEngine.UIElements.UIR;

internal class GradientRemap : LinkedPoolItem<GradientRemap>
{
	public int origIndex;

	public int destIndex;

	public RectInt location;

	public GradientRemap next;

	public bool isAtlassed;

	public void Reset()
	{
		origIndex = 0;
		destIndex = 0;
		location = default(RectInt);
		isAtlassed = false;
	}
}
