using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy;

public class G_GraphShader
{
	public const int ArrayMaxSizeFull = 512;

	public const int ArrayMaxSizeLight = 128;

	public int ArrayMaxSize = 128;

	public float[] Array;

	public Image Image;

	private string Name = "GraphValues";

	private string Name_Length = "GraphValues_Length";

	public float Average;

	private int averagePropertyId;

	public float GoodThreshold;

	public float CautionThreshold;

	private int goodThresholdPropertyId;

	private int cautionThresholdPropertyId;

	public Color GoodColor = Color.white;

	public Color CautionColor = Color.white;

	public Color CriticalColor = Color.white;

	private int goodColorPropertyId;

	private int cautionColorPropertyId;

	private int criticalColorPropertyId;

	public void InitializeShader()
	{
		Image.material.SetFloatArray(Name, new float[ArrayMaxSize]);
		averagePropertyId = Shader.PropertyToID("Average");
		goodThresholdPropertyId = Shader.PropertyToID("_GoodThreshold");
		cautionThresholdPropertyId = Shader.PropertyToID("_CautionThreshold");
		goodColorPropertyId = Shader.PropertyToID("_GoodColor");
		cautionColorPropertyId = Shader.PropertyToID("_CautionColor");
		criticalColorPropertyId = Shader.PropertyToID("_CriticalColor");
	}

	public void UpdateArray()
	{
		Image.material.SetInt(Name_Length, Array.Length);
	}

	public void UpdateAverage()
	{
		Image.material.SetFloat(averagePropertyId, Average);
	}

	public void UpdateThresholds()
	{
		Image.material.SetFloat(goodThresholdPropertyId, GoodThreshold);
		Image.material.SetFloat(cautionThresholdPropertyId, CautionThreshold);
	}

	public void UpdateColors()
	{
		Image.material.SetColor(goodColorPropertyId, GoodColor);
		Image.material.SetColor(cautionColorPropertyId, CautionColor);
		Image.material.SetColor(criticalColorPropertyId, CriticalColor);
	}

	public void UpdatePoints()
	{
		Image.material.SetFloatArray(Name, Array);
	}
}
