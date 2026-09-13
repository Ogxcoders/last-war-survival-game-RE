using UnityEngine;

public class ArabicEffectMirror : MonoBehaviour
{
	public enum MirrorType
	{
		Horizontal,
		Vertical
	}

	public MirrorType mirrorType;

	private void Start()
	{
		Vector3 localScale = base.transform.localScale;
		if (mirrorType == MirrorType.Horizontal)
		{
			localScale.x *= -1f;
		}
		else if (mirrorType == MirrorType.Vertical)
		{
			localScale.y *= -1f;
		}
		base.transform.localScale = localScale;
	}
}
