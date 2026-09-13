using System.Collections.Generic;

namespace UnityEngine.UI.Extensions;

public static class ShaderLibrary
{
	public static Dictionary<string, Shader> shaderInstances = new Dictionary<string, Shader>();

	public static Shader[] preLoadedShaders;

	public static Shader GetShaderInstance(string shaderName)
	{
		if (shaderInstances.ContainsKey(shaderName))
		{
			return shaderInstances[shaderName];
		}
		Shader shader = Shader.Find(shaderName);
		if (shader != null)
		{
			shaderInstances.Add(shaderName, shader);
		}
		return shader;
	}
}
