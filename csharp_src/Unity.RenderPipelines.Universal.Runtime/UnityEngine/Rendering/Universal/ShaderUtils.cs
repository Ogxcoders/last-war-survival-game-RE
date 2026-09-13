using System;
using System.Linq;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal;

[MovedFrom("UnityEngine.Rendering.LWRP")]
public static class ShaderUtils
{
	private static readonly string[] s_ShaderPaths = new string[12]
	{
		"Universal Render Pipeline/Lit", "Universal Render Pipeline/Simple Lit", "Universal Render Pipeline/Unlit", "Universal Render Pipeline/Terrain/Lit", "Universal Render Pipeline/Particles/Lit", "Universal Render Pipeline/Particles/Simple Lit", "Universal Render Pipeline/Particles/Unlit", "Universal Render Pipeline/Baked Lit", "", "Universal Render Pipeline/Nature/SpeedTree7",
		"Universal Render Pipeline/Nature/SpeedTree7 Billboard", "Universal Render Pipeline/Nature/SpeedTree8"
	};

	public static string GetShaderPath(ShaderPathID id)
	{
		int num = s_ShaderPaths.Length;
		if (num > 0 && id >= ShaderPathID.Lit && (int)id < num)
		{
			return s_ShaderPaths[(int)id];
		}
		Debug.LogError(string.Concat("Trying to access universal shader path out of bounds: (", id, ": ", (int)id, ")"));
		return "";
	}

	public static ShaderPathID GetEnumFromPath(string path)
	{
		return (ShaderPathID)Array.FindIndex(s_ShaderPaths, (string m) => m == path);
	}

	public static bool IsLWShader(Shader shader)
	{
		return s_ShaderPaths.Contains(shader.name);
	}
}
