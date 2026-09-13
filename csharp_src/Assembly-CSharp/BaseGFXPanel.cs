using UnityEngine;

public class BaseGFXPanel
{
	public string name { get; private set; }

	public BaseGFXPanel(string p_name)
	{
		name = p_name;
	}

	public virtual void Init()
	{
	}

	public virtual void DrawGUI()
	{
	}

	protected float DrawSlider(string label, float v, float min, float max)
	{
		GUILayout.Label(label + ":" + v);
		return GUILayout.HorizontalSlider(v, min, max);
	}

	protected string DrawInputField(string label, string inputTex)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(label);
		string result = GUILayout.TextField(inputTex);
		GUILayout.EndHorizontal();
		return result;
	}
}
