using UnityEngine;

public class ArabicEffectReverseScaleMirror : MonoBehaviour
{
	public enum MirrorType
	{
		Horizontal,
		Vertical
	}

	public MirrorType mirrorType;

	private bool canReverseScale;

	public bool CanReverseScale
	{
		get
		{
			return canReverseScale;
		}
		set
		{
			canReverseScale = value;
		}
	}

	private void Start()
	{
		Vector3 localScale = base.transform.localScale;
		if (canReverseScale)
		{
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
}
