using UnityEngine;

public class ShaderGFXPanel : BaseGFXPanel
{
	private string keywordName = "";

	public ShaderGFXPanel()
		: base("Shader")
	{
	}

	public override void DrawGUI()
	{
		GUILayout.Label($"ShaderLOD:{Shader.globalMaximumLOD}");
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("601"))
		{
			Shader.globalMaximumLOD = 601;
		}
		if (GUILayout.Button("401"))
		{
			Shader.globalMaximumLOD = 401;
		}
		if (GUILayout.Button("201"))
		{
			Shader.globalMaximumLOD = 201;
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(30f);
		keywordName = DrawInputField("Shader关键字", keywordName);
		GUILayout.Label($"{keywordName}开关 {Shader.IsKeywordEnabled(keywordName)}");
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("开"))
		{
			Shader.EnableKeyword(keywordName);
		}
		if (GUILayout.Button("关"))
		{
			Shader.DisableKeyword(keywordName);
		}
		GUILayout.EndHorizontal();
	}
}
