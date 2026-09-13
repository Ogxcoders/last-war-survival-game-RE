using UnityEngine;

public class InfinityItem : MonoBehaviour
{
	private InfinityRect infinityRect;

	public InfinityRect Rect
	{
		get
		{
			return infinityRect;
		}
		set
		{
			infinityRect = value;
			base.gameObject.transform.localScale = ((value == null) ? Vector3.zero : Vector3.one);
		}
	}
}
