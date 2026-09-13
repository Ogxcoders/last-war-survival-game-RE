using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class UIHero100CardOpenAni : MonoBehaviour
{
	public float PostExposure = 1f;

	public Volume volume;

	private void Update()
	{
		volume.profile.TryGet<ColorAdjustments>(out var component);
		if (component != null)
		{
			component.postExposure.value = PostExposure;
		}
	}
}
