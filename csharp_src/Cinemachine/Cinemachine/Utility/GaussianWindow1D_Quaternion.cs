using UnityEngine;

namespace Cinemachine.Utility;

internal class GaussianWindow1D_Quaternion : GaussianWindow1d<Quaternion>
{
	public GaussianWindow1D_Quaternion(float sigma, int maxKernelRadius = 10)
		: base(sigma, maxKernelRadius)
	{
	}

	protected override Quaternion Compute(int windowPos)
	{
		Quaternion quaternion = new Quaternion(0f, 0f, 0f, 0f);
		Quaternion quaternion2 = mData[mCurrentPos];
		Quaternion quaternion3 = Quaternion.Inverse(quaternion2);
		for (int i = 0; i < base.KernelSize; i++)
		{
			float num = mKernel[i];
			Quaternion b = quaternion3 * mData[windowPos];
			if (Quaternion.Dot(Quaternion.identity, b) < 0f)
			{
				num = 0f - num;
			}
			quaternion.x += b.x * num;
			quaternion.y += b.y * num;
			quaternion.z += b.z * num;
			quaternion.w += b.w * num;
			if (++windowPos == base.KernelSize)
			{
				windowPos = 0;
			}
		}
		return quaternion2 * quaternion;
	}
}
