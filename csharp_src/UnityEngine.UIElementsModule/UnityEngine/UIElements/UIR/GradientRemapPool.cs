using System;

namespace UnityEngine.UIElements.UIR;

internal class GradientRemapPool : LinkedPool<GradientRemap>
{
	public GradientRemapPool()
		: base((Func<GradientRemap>)(() => new GradientRemap()), (Action<GradientRemap>)delegate(GradientRemap gradientRemap)
		{
			gradientRemap.Reset();
		}, 10000)
	{
	}
}
