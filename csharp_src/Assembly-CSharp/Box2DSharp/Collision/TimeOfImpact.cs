using System;
using System.Diagnostics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Collision;

public static class TimeOfImpact
{
	public static void ComputeTimeOfImpact(out ToiOutput output, in ToiInput input, ToiProfile toiProfile = null, GJkProfile gjkProfile = null)
	{
		long num = ((toiProfile == null) ? 0 : Stopwatch.GetTimestamp());
		output = default(ToiOutput);
		if (toiProfile != null)
		{
			toiProfile.ToiCalls++;
		}
		output.State = ToiOutput.ToiState.Unknown;
		output.Time = input.Tmax;
		ref readonly DistanceProxy proxyA = ref input.ProxyA;
		ref readonly DistanceProxy proxyB = ref input.ProxyB;
		Sweep sweepA = input.SweepA;
		Sweep sweepB = input.SweepB;
		sweepA.Normalize();
		sweepB.Normalize();
		FP tmax = input.Tmax;
		FP x = proxyA.Radius + proxyB.Radius;
		FP x2 = FP.Max(Settings.LinearSlop, x - (FP)3 * Settings.LinearSlop);
		FP y = (FP)0.25f * Settings.LinearSlop;
		FP fP = FP.Zero;
		int num2 = 0;
		SimplexCache cache = default(SimplexCache);
		DistanceInput input2 = new DistanceInput
		{
			ProxyA = input.ProxyA,
			ProxyB = input.ProxyB,
			UseRadii = false
		};
		while (true)
		{
			sweepA.GetTransform(out var xf, fP);
			sweepB.GetTransform(out var xf2, fP);
			input2.TransformA = xf;
			input2.TransformB = xf2;
			DistanceAlgorithm.Distance(out var output2, ref cache, in input2, in gjkProfile);
			if (output2.Distance <= 0f)
			{
				output.State = ToiOutput.ToiState.Overlapped;
				output.Time = 0f;
				break;
			}
			if (output2.Distance < x2 + y)
			{
				output.State = ToiOutput.ToiState.Touching;
				output.Time = fP;
				break;
			}
			SeparationFunction separationFunction = default(SeparationFunction);
			separationFunction.Initialize(ref cache, proxyA, in sweepA, proxyB, in sweepB, fP);
			bool flag = false;
			FP fP2 = tmax;
			int num3 = 0;
			do
			{
				int indexA;
				int indexB;
				FP x3 = separationFunction.FindMinSeparation(out indexA, out indexB, fP2);
				if (x3 > x2 + y)
				{
					output.State = ToiOutput.ToiState.Separated;
					output.Time = tmax;
					flag = true;
					break;
				}
				if (x3 > x2 - y)
				{
					fP = fP2;
					break;
				}
				FP y2 = separationFunction.Evaluate(indexA, indexB, fP);
				if (y2 < x2 - y)
				{
					output.State = ToiOutput.ToiState.Failed;
					output.Time = fP;
					flag = true;
					break;
				}
				if (y2 <= x2 + y)
				{
					output.State = ToiOutput.ToiState.Touching;
					output.Time = fP;
					flag = true;
					break;
				}
				int num4 = 0;
				FP x4 = fP;
				FP y3 = fP2;
				do
				{
					FP fP3 = (((num4 & 1) == 0) ? ((FP)0.5f * (x4 + y3)) : (x4 + (x2 - y2) * (y3 - x4) / (x3 - y2)));
					num4++;
					if (toiProfile != null)
					{
						toiProfile.ToiRootIters++;
					}
					FP x5 = separationFunction.Evaluate(indexA, indexB, fP3);
					if (FP.Abs(x5 - x2) < y)
					{
						fP2 = fP3;
						break;
					}
					if (x5 > x2)
					{
						x4 = fP3;
						y2 = x5;
					}
					else
					{
						y3 = fP3;
						x3 = x5;
					}
				}
				while (num4 != 50);
				if (toiProfile != null)
				{
					toiProfile.ToiMaxRootIters = Math.Max(toiProfile.ToiMaxRootIters, num4);
				}
				num3++;
			}
			while (num3 != 8);
			num2++;
			if (toiProfile != null)
			{
				toiProfile.ToiIters++;
			}
			if (flag)
			{
				break;
			}
			if (num2 == 20)
			{
				output.State = ToiOutput.ToiState.Failed;
				output.Time = fP;
				break;
			}
		}
		if (toiProfile != null)
		{
			float num5 = (float)(Stopwatch.GetTimestamp() - num) / 10000f;
			toiProfile.ToiMaxIters = Math.Max(toiProfile.ToiMaxIters, num2);
			toiProfile.ToiMaxTime = Math.Max(toiProfile.ToiMaxTime, num5);
			toiProfile.ToiTime += num5;
		}
	}
}
