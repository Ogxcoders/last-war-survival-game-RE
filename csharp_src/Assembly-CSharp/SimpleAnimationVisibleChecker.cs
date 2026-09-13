using UnityEngine;

public class SimpleAnimationVisibleChecker : MonoBehaviour
{
	public Renderer renderer;

	public Transform bone;

	public float thresholds = 0.001f;

	public void Update()
	{
		if (!(bone == null))
		{
			bool flag = bone.localScale.sqrMagnitude > thresholds;
			renderer.enabled = flag;
		}
	}

	public void OnEnable()
	{
	}

	public void OnDisable()
	{
	}

	public void OnValidate()
	{
		if (renderer == null)
		{
			renderer = GetComponent<SkinnedMeshRenderer>();
		}
		if (bone == null && renderer != null && renderer is SkinnedMeshRenderer skinnedMeshRenderer)
		{
			bone = skinnedMeshRenderer.rootBone;
		}
	}
}
