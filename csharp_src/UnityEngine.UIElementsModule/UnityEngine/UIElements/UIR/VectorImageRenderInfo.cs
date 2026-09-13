namespace UnityEngine.UIElements.UIR;

internal class VectorImageRenderInfo : LinkedPoolItem<VectorImageRenderInfo>
{
	public int useCount;

	public GradientRemap firstGradientRemap;

	public Alloc gradientSettingsAlloc;

	public void Reset()
	{
		useCount = 0;
		firstGradientRemap = null;
		gradientSettingsAlloc = default(Alloc);
	}
}
