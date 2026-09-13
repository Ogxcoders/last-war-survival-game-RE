using UnityEngine;

[AddComponentMenu("Fog of War/Renderers")]
public class FOWRenderers : MonoBehaviour
{
	private Transform mTrans;

	private Renderer[] mRenderers;

	private float mNextUpdate;

	private bool mIsVisible = true;

	private bool mUpdate = true;

	public bool isVisible => mIsVisible;

	public void Rebuild()
	{
		mUpdate = true;
	}

	private void Awake()
	{
		mTrans = base.transform;
	}

	private void LateUpdate()
	{
		if (mNextUpdate < Time.time)
		{
			UpdateNow();
		}
	}

	private void UpdateNow()
	{
	}
}
