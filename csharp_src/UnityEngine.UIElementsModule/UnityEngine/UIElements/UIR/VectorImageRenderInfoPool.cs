using System;

namespace UnityEngine.UIElements.UIR;

internal class VectorImageRenderInfoPool : LinkedPool<VectorImageRenderInfo>
{
	public VectorImageRenderInfoPool()
		: base((Func<VectorImageRenderInfo>)(() => new VectorImageRenderInfo()), (Action<VectorImageRenderInfo>)delegate(VectorImageRenderInfo vectorImageInfo)
		{
			vectorImageInfo.Reset();
		}, 10000)
	{
	}
}
