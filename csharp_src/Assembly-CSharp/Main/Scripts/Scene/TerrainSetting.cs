using UnityEngine;

namespace Main.Scripts.Scene;

[CreateAssetMenu(fileName = "TerrainSetting", menuName = "APS - TerrainSetting", order = 0)]
public class TerrainSetting : ScriptableObject
{
	public Texture2D control;

	public Texture2D splat0;

	public Texture2D splat1;

	public Vector4 control_st;

	public Vector4 splat0_st;

	public Vector4 splat1_st;

	public Vector4 terrainBounds;
}
