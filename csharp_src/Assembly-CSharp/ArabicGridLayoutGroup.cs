using UnityEngine;
using UnityEngine.UI;

public class ArabicGridLayoutGroup : MonoBehaviour
{
	public GridLayoutGroup.Corner ArabicStartCorner = GridLayoutGroup.Corner.UpperRight;

	[Header("Mirror版本控制")]
	[Tooltip("是否与Mirror版本一起上线")]
	public bool IsControlledByMirror;

	private void Start()
	{
	}
}
