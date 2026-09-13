using UnityEngine;

namespace VolumetricFogAndMist2;

[ExecuteInEditMode]
public class FogVoid : MonoBehaviour
{
	public float radius = 10f;

	[Range(0f, 1f)]
	public float falloff;

	private void OnEnable()
	{
		VolumetricFogManager.fogVoidManager.Refresh();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(1f, 1f, 0f, 0.75f);
		Gizmos.DrawWireSphere(base.transform.position, radius);
	}
}
