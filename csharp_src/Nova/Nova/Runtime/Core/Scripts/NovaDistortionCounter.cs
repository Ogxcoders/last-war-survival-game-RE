using UnityEngine;

namespace Nova.Runtime.Core.Scripts;

[ExecuteInEditMode]
[DisallowMultipleComponent]
public class NovaDistortionCounter : MonoBehaviour
{
	[Tooltip("用nova扭曲的才影响计数")]
	private bool _rendererUseNovaDistortion;

	public static int Count { get; private set; }

	private void Awake()
	{
		UpdateValidity();
	}

	private void OnEnable()
	{
		if (Application.isEditor)
		{
			UpdateValidity();
		}
		if (_rendererUseNovaDistortion)
		{
			Count++;
		}
	}

	private void OnDisable()
	{
		if (_rendererUseNovaDistortion)
		{
			Count--;
		}
	}

	private void UpdateValidity()
	{
		_rendererUseNovaDistortion = false;
		Renderer component = GetComponent<Renderer>();
		if (component != null && component.sharedMaterial != null && component.sharedMaterial.shader != null && component.sharedMaterial.shader.name == "Nova/Particles/Distortion")
		{
			_rendererUseNovaDistortion = true;
		}
	}
}
