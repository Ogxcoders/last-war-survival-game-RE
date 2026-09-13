using UnityEngine;

namespace UnityMeshSimplifier;

[AddComponentMenu("")]
internal class LODBackupComponent : MonoBehaviour
{
	[SerializeField]
	private Renderer[] originalRenderers;

	public Renderer[] OriginalRenderers
	{
		get
		{
			return originalRenderers;
		}
		set
		{
			originalRenderers = value;
		}
	}
}
