using UnityEngine;

[DefaultExecutionOrder(-1)]
public class SimpleAnimationAdditionalData : MonoBehaviour
{
	private SimpleAnimation simpleAnimation;

	public bool updateManual;

	private void OnEnable()
	{
		if (simpleAnimation == null)
		{
			simpleAnimation = GetComponent<SimpleAnimation>();
		}
		if (simpleAnimation != null)
		{
			simpleAnimation.UpdateManual = updateManual;
		}
	}
}
