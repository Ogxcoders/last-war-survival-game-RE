using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.VFX;

[NativeType(Header = "Modules/VFX/Public/VFXExpressionNoiseFunctions.h")]
[RequiredByNativeCode]
[StaticAccessor("VFXExpressionNoiseFunctions", StaticAccessorType.DoubleColon)]
internal class VFXExpressionNoise
{
	[NativeName("Value::Generate")]
	internal static Vector2 GenerateValueNoise1D(float coordinate, float frequency, int octaveCount, float persistence, float lacunarity)
	{
		GenerateValueNoise1D_Injected(coordinate, frequency, octaveCount, persistence, lacunarity, out var ret);
		return ret;
	}

	[NativeName("Value::Generate")]
	internal static Vector3 GenerateValueNoise2D(Vector2 coordinate, float frequency, int octaveCount, float persistence, float lacunarity)
	{
		GenerateValueNoise2D_Injected(ref coordinate, frequency, octaveCount, persistence, lacunarity, out var ret);
		return ret;
	}

	[NativeName("Value::Generate")]
	internal static Vector4 GenerateValueNoise3D(Vector3 coordinate, float frequency, int octaveCount, float persistence, float lacunarity)
	{
		GenerateValueNoise3D_Injected(ref coordinate, frequency, octaveCount, persistence, lacunarity, out var ret);
		return ret;
	}

	[NativeName("Value::GenerateCurl")]
	internal static Vector2 GenerateValueCurlNoise2D(Vector2 coordinate, float frequency, int octaveCount, float persistence, float lacunarity)
	{
		GenerateValueCurlNoise2D_Injected(ref coordinate, frequency, octaveCount, persistence, lacunarity, out var ret);
		return ret;
	}

	[NativeName("Value::GenerateCurl")]
	internal static Vector3 GenerateValueCurlNoise3D(Vector3 coordinate, float frequency, int octaveCount, float persistence, float lacunarity)
	{
		GenerateValueCurlNoise3D_Injected(ref coordinate, frequency, octaveCount, persistence, lacunarity, out var ret);
		return ret;
	}

	[NativeName("Perlin::Generate")]
	internal static Vector2 GeneratePerlinNoise1D(float coordinate, float frequency, int octaveCount, float persistence, float lacunarity)
	{
		GeneratePerlinNoise1D_Injected(coordinate, frequency, octaveCount, persistence, lacunarity, out var ret);
		return ret;
	}

	[NativeName("Perlin::Generate")]
	internal static Vector3 GeneratePerlinNoise2D(Vector2 coordinate, float frequency, int octaveCount, float persistence, float lacunarity)
	{
		GeneratePerlinNoise2D_Injected(ref coordinate, frequency, octaveCount, persistence, lacunarity, out var ret);
		return ret;
	}

	[NativeName("Perlin::Generate")]
	internal static Vector4 GeneratePerlinNoise3D(Vector3 coordinate, float frequency, int octaveCount, float persistence, float lacunarity)
	{
		GeneratePerlinNoise3D_Injected(ref coordinate, frequency, octaveCount, persistence, lacunarity, out var ret);
		return ret;
	}

	[NativeName("Perlin::GenerateCurl")]
	internal static Vector2 GeneratePerlinCurlNoise2D(Vector2 coordinate, float frequency, int octaveCount, float persistence, float lacunarity)
	{
		GeneratePerlinCurlNoise2D_Injected(ref coordinate, frequency, octaveCount, persistence, lacunarity, out var ret);
		return ret;
	}

	[NativeName("Perlin::GenerateCurl")]
	internal static Vector3 GeneratePerlinCurlNoise3D(Vector3 coordinate, float frequency, int octaveCount, float persistence, float lacunarity)
	{
		GeneratePerlinCurlNoise3D_Injected(ref coordinate, frequency, octaveCount, persistence, lacunarity, out var ret);
		return ret;
	}

	[NativeName("Cellular::Generate")]
	internal static Vector2 GenerateCellularNoise1D(float coordinate, float frequency, int octaveCount, float persistence, float lacunarity)
	{
		GenerateCellularNoise1D_Injected(coordinate, frequency, octaveCount, persistence, lacunarity, out var ret);
		return ret;
	}

	[NativeName("Cellular::Generate")]
	internal static Vector3 GenerateCellularNoise2D(Vector2 coordinate, float frequency, int octaveCount, float persistence, float lacunarity)
	{
		GenerateCellularNoise2D_Injected(ref coordinate, frequency, octaveCount, persistence, lacunarity, out var ret);
		return ret;
	}

	[NativeName("Cellular::Generate")]
	internal static Vector4 GenerateCellularNoise3D(Vector3 coordinate, float frequency, int octaveCount, float persistence, float lacunarity)
	{
		GenerateCellularNoise3D_Injected(ref coordinate, frequency, octaveCount, persistence, lacunarity, out var ret);
		return ret;
	}

	[NativeName("Cellular::GenerateCurl")]
	internal static Vector2 GenerateCellularCurlNoise2D(Vector2 coordinate, float frequency, int octaveCount, float persistence, float lacunarity)
	{
		GenerateCellularCurlNoise2D_Injected(ref coordinate, frequency, octaveCount, persistence, lacunarity, out var ret);
		return ret;
	}

	[NativeName("Cellular::GenerateCurl")]
	internal static Vector3 GenerateCellularCurlNoise3D(Vector3 coordinate, float frequency, int octaveCount, float persistence, float lacunarity)
	{
		GenerateCellularCurlNoise3D_Injected(ref coordinate, frequency, octaveCount, persistence, lacunarity, out var ret);
		return ret;
	}

	[NativeName("Voro::Generate")]
	internal static float GenerateVoroNoise2D(Vector2 coordinate, float frequency, float warp, float smoothness)
	{
		return GenerateVoroNoise2D_Injected(ref coordinate, frequency, warp, smoothness);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GenerateValueNoise1D_Injected(float coordinate, float frequency, int octaveCount, float persistence, float lacunarity, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GenerateValueNoise2D_Injected(ref Vector2 coordinate, float frequency, int octaveCount, float persistence, float lacunarity, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GenerateValueNoise3D_Injected(ref Vector3 coordinate, float frequency, int octaveCount, float persistence, float lacunarity, out Vector4 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GenerateValueCurlNoise2D_Injected(ref Vector2 coordinate, float frequency, int octaveCount, float persistence, float lacunarity, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GenerateValueCurlNoise3D_Injected(ref Vector3 coordinate, float frequency, int octaveCount, float persistence, float lacunarity, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GeneratePerlinNoise1D_Injected(float coordinate, float frequency, int octaveCount, float persistence, float lacunarity, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GeneratePerlinNoise2D_Injected(ref Vector2 coordinate, float frequency, int octaveCount, float persistence, float lacunarity, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GeneratePerlinNoise3D_Injected(ref Vector3 coordinate, float frequency, int octaveCount, float persistence, float lacunarity, out Vector4 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GeneratePerlinCurlNoise2D_Injected(ref Vector2 coordinate, float frequency, int octaveCount, float persistence, float lacunarity, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GeneratePerlinCurlNoise3D_Injected(ref Vector3 coordinate, float frequency, int octaveCount, float persistence, float lacunarity, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GenerateCellularNoise1D_Injected(float coordinate, float frequency, int octaveCount, float persistence, float lacunarity, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GenerateCellularNoise2D_Injected(ref Vector2 coordinate, float frequency, int octaveCount, float persistence, float lacunarity, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GenerateCellularNoise3D_Injected(ref Vector3 coordinate, float frequency, int octaveCount, float persistence, float lacunarity, out Vector4 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GenerateCellularCurlNoise2D_Injected(ref Vector2 coordinate, float frequency, int octaveCount, float persistence, float lacunarity, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GenerateCellularCurlNoise3D_Injected(ref Vector3 coordinate, float frequency, int octaveCount, float persistence, float lacunarity, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern float GenerateVoroNoise2D_Injected(ref Vector2 coordinate, float frequency, float warp, float smoothness);
}
