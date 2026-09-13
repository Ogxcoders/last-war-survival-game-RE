using System;
using UnityEngine;

namespace FibMatrix.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class WaterHelper : MonoBehaviour
{
	[Range(-180f, 180f)]
	public float specLightDirYDegree;

	[Range(-180f, 180f)]
	public float waveDirection;

	private Renderer m_Render;

	private void Update()
	{
		if (!Application.isPlaying && Application.isEditor)
		{
			if (m_Render == null)
			{
				m_Render = GetComponent<Renderer>();
			}
			if (m_Render != null)
			{
				float f = MathF.PI / 180f * specLightDirYDegree;
				Vector3 direction = new Vector3(0f, Mathf.Sin(f), Mathf.Cos(f));
				Vector3 vector = Camera.main.transform.TransformDirection(direction);
				m_Render.sharedMaterial?.SetVector("_SpecLightDir", vector);
				float f2 = MathF.PI / 180f * waveDirection;
				Vector4 zero = Vector4.zero;
				zero.x = Mathf.Cos(f2);
				zero.z = Mathf.Sin(f2);
				m_Render.sharedMaterial?.SetVector("_WaveDirection", zero);
			}
		}
	}
}
