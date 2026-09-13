using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential)]
[NativeHeader("Runtime/Export/Graphics/Graphics.bindings.h")]
public sealed class LightProbes : Object
{
	public extern Vector3[] positions
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("GetLightProbePositions")]
		[FreeFunction(HasExplicitThis = true)]
		get;
	}

	public extern SphericalHarmonicsL2[] bakedProbes
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("GetBakedCoefficients")]
		[FreeFunction(HasExplicitThis = true)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("SetBakedCoefficients")]
		[FreeFunction(HasExplicitThis = true)]
		set;
	}

	public extern int count
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("GetLightProbeCount")]
		[FreeFunction(HasExplicitThis = true)]
		get;
	}

	public extern int cellCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(HasExplicitThis = true)]
		[NativeName("GetTetrahedraSize")]
		get;
	}

	[Obsolete("Use bakedProbes instead.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public float[] coefficients
	{
		get
		{
			return new float[0];
		}
		set
		{
		}
	}

	public static event Action tetrahedralizationCompleted;

	public static event Action needsRetetrahedralization;

	private LightProbes()
	{
	}

	[RequiredByNativeCode]
	private static void Internal_CallTetrahedralizationCompletedFunction()
	{
		if (LightProbes.tetrahedralizationCompleted != null)
		{
			LightProbes.tetrahedralizationCompleted();
		}
	}

	[RequiredByNativeCode]
	private static void Internal_CallNeedsRetetrahedralizationFunction()
	{
		if (LightProbes.needsRetetrahedralization != null)
		{
			LightProbes.needsRetetrahedralization();
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void Tetrahedralize();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void TetrahedralizeAsync();

	[FreeFunction]
	public static void GetInterpolatedProbe(Vector3 position, Renderer renderer, out SphericalHarmonicsL2 probe)
	{
		GetInterpolatedProbe_Injected(ref position, renderer, out probe);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	internal static extern bool AreLightProbesAllowed(Renderer renderer);

	public static void CalculateInterpolatedLightAndOcclusionProbes(Vector3[] positions, SphericalHarmonicsL2[] lightProbes, Vector4[] occlusionProbes)
	{
		if (positions == null)
		{
			throw new ArgumentNullException("positions");
		}
		if (lightProbes == null && occlusionProbes == null)
		{
			throw new ArgumentException("Argument lightProbes and occlusionProbes cannot both be null.");
		}
		if (lightProbes != null && lightProbes.Length < positions.Length)
		{
			throw new ArgumentException("lightProbes", "Argument lightProbes has less elements than positions");
		}
		if (occlusionProbes != null && occlusionProbes.Length < positions.Length)
		{
			throw new ArgumentException("occlusionProbes", "Argument occlusionProbes has less elements than positions");
		}
		CalculateInterpolatedLightAndOcclusionProbes_Internal(positions, positions.Length, lightProbes, occlusionProbes);
	}

	public static void CalculateInterpolatedLightAndOcclusionProbes(List<Vector3> positions, List<SphericalHarmonicsL2> lightProbes, List<Vector4> occlusionProbes)
	{
		if (positions == null)
		{
			throw new ArgumentNullException("positions");
		}
		if (lightProbes == null && occlusionProbes == null)
		{
			throw new ArgumentException("Argument lightProbes and occlusionProbes cannot both be null.");
		}
		if (lightProbes != null)
		{
			if (lightProbes.Capacity < positions.Count)
			{
				lightProbes.Capacity = positions.Count;
			}
			if (lightProbes.Count < positions.Count)
			{
				NoAllocHelpers.ResizeList(lightProbes, positions.Count);
			}
		}
		if (occlusionProbes != null)
		{
			if (occlusionProbes.Capacity < positions.Count)
			{
				occlusionProbes.Capacity = positions.Count;
			}
			if (occlusionProbes.Count < positions.Count)
			{
				NoAllocHelpers.ResizeList(occlusionProbes, positions.Count);
			}
		}
		CalculateInterpolatedLightAndOcclusionProbes_Internal(NoAllocHelpers.ExtractArrayFromListT(positions), positions.Count, NoAllocHelpers.ExtractArrayFromListT(lightProbes), NoAllocHelpers.ExtractArrayFromListT(occlusionProbes));
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("CalculateInterpolatedLightAndOcclusionProbes")]
	[FreeFunction]
	internal static extern void CalculateInterpolatedLightAndOcclusionProbes_Internal(Vector3[] positions, int positionsCount, SphericalHarmonicsL2[] lightProbes, Vector4[] occlusionProbes);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	[NativeName("GetLightProbeCount")]
	internal static extern int GetCount();

	[Obsolete("Use GetInterpolatedProbe instead.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void GetInterpolatedLightProbe(Vector3 position, Renderer renderer, float[] coefficients)
	{
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetInterpolatedProbe_Injected(ref Vector3 position, Renderer renderer, out SphericalHarmonicsL2 probe);
}
