using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine.Experimental.XR;

[NativeConditional("ENABLE_VR")]
public static class Boundary
{
	public enum Type
	{
		PlayArea,
		TrackedArea
	}

	[NativeName("BoundaryVisible")]
	[StaticAccessor("GetIVRDevice()", StaticAccessorType.ArrowWithDefaultReturnIfNull)]
	public static extern bool visible
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeName("BoundaryConfigured")]
	[StaticAccessor("GetIVRDevice()", StaticAccessorType.ArrowWithDefaultReturnIfNull)]
	public static extern bool configured
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public static bool TryGetDimensions(out Vector3 dimensionsOut)
	{
		return TryGetDimensions(out dimensionsOut, Type.PlayArea);
	}

	public static bool TryGetDimensions(out Vector3 dimensionsOut, [DefaultValue("Type.PlayArea")] Type boundaryType)
	{
		return TryGetDimensionsInternal(out dimensionsOut, boundaryType);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetIVRDevice()", StaticAccessorType.ArrowWithDefaultReturnIfNull)]
	[NativeName("TryGetBoundaryDimensions")]
	private static extern bool TryGetDimensionsInternal(out Vector3 dimensionsOut, Type boundaryType);

	public static bool TryGetGeometry(List<Vector3> geometry)
	{
		return TryGetGeometry(geometry, Type.PlayArea);
	}

	public static bool TryGetGeometry(List<Vector3> geometry, [DefaultValue("Type.PlayArea")] Type boundaryType)
	{
		if (geometry == null)
		{
			throw new ArgumentNullException("geometry");
		}
		geometry.Clear();
		return TryGetGeometryScriptingInternal(geometry, boundaryType);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool TryGetGeometryScriptingInternal(List<Vector3> geometry, Type boundaryType);
}
