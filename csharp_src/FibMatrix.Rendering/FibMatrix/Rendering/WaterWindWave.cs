using UnityEngine;

namespace FibMatrix.Rendering;

[HelpURL("https://rivergame.feishu.cn/wiki/wikcnfGI77uvaYoRTTVavTf2o3b")]
public class WaterWindWave : MonoBehaviour
{
	private static string s_TimeOffsetPropertyName = "_WindWaveTimeOffset";

	private static int s_TimeOffsetPropertyId = Shader.PropertyToID(s_TimeOffsetPropertyName);

	public float timeOffset;

	public bool enableRandomTimeOffset;

	private void Start()
	{
		Renderer component = GetComponent<Renderer>();
		if (!(component == null))
		{
			float value = (enableRandomTimeOffset ? Random.Range(0f, 1f) : timeOffset);
			component.material.SetFloat(s_TimeOffsetPropertyId, value);
		}
	}
}
