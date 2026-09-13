using UnityEngine;

namespace FibMatrix.Rendering;

public class TerrainComponent : MonoBehaviour
{
	public const string RayMarchingStepCountName = "_RayMarchingStepCount";

	public readonly int RayMarchingStepCountId = Shader.PropertyToID("_RayMarchingStepCount");

	public int step = 5;

	private Renderer render;

	private void Update()
	{
		if (render == null)
		{
			render = GetComponent<Renderer>();
			if (render != null)
			{
				step = render.sharedMaterial.GetInt(RayMarchingStepCountId);
			}
		}
		if (render != null)
		{
			render.sharedMaterial.SetInt(RayMarchingStepCountId, step);
		}
	}
}
