using UnityEngine;

[ExecuteAlways]
public class FogControll : MonoBehaviour
{
	public static class Uniforms
	{
		internal static readonly int _DistanceFogColor = Shader.PropertyToID("_FogColor");

		internal static readonly int _DistanceFogStart = Shader.PropertyToID("_FogStart");

		internal static readonly int _DistanceFogEnd = Shader.PropertyToID("_FogEnd");

		internal static readonly int _HeightFogStart = Shader.PropertyToID("_FogMinHeight");

		internal static readonly int _HeightFogEnd = Shader.PropertyToID("_FogMaxHeight");
	}

	public Color DistanceFog = Color.white;

	public float DistanceFogStart = 10f;

	public float DistanceFogEnd = 100f;

	public float HeightFogStart;

	public float HeightFogEnd = 10f;

	public bool EnableFog;

	private bool toggle;

	public void Open()
	{
		Shader.SetGlobalColor(Uniforms._DistanceFogColor, DistanceFog);
		Shader.SetGlobalFloat(Uniforms._DistanceFogStart, DistanceFogStart);
		Shader.SetGlobalFloat(Uniforms._DistanceFogEnd, DistanceFogEnd);
		Shader.SetGlobalFloat(Uniforms._HeightFogStart, HeightFogStart);
		Shader.SetGlobalFloat(Uniforms._HeightFogEnd, HeightFogEnd);
		Shader.EnableKeyword("TOGGLE_FOG");
	}

	public void Close()
	{
		Shader.DisableKeyword("TOGGLE_FOG");
	}

	private void OnDestroy()
	{
		Shader.DisableKeyword("TOGGLE_FOG");
	}
}
