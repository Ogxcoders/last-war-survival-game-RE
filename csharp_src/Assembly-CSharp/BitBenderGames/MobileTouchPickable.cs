using UnityEngine;

namespace BitBenderGames;

public abstract class MobileTouchPickable : MonoBehaviour
{
	private static MobileTouchCamera mobileTouchCam;

	[SerializeField]
	[Tooltip("可选的。如果可选项的冲突器不在可选项的根对象上，则只需要设置此值。如果可选项的冲突器不在可选项的根对象上，则需要设置此值.")]
	private Transform pickableTransform;

	[SerializeField]
	[Tooltip("启用捕捉时，此值定义一个位置偏移量，该偏移量在拖动时添加到对象的中心。注意，这个值是添加在MobilePickingController中定义的snapOffset之上的。当使用自顶向下相机时，这两个值应用于X/Z位置.")]
	private Vector2 localSnapOffset = Vector2.zero;

	public Transform PickableTransform
	{
		get
		{
			return pickableTransform;
		}
		set
		{
			pickableTransform = value;
		}
	}

	public Vector2 LocalSnapOffset => localSnapOffset;

	public void Awake()
	{
		if (mobileTouchCam == null)
		{
			mobileTouchCam = Object.FindObjectOfType<MobileTouchCamera>();
		}
		if (mobileTouchCam == null)
		{
			Debug.LogError("No MobileTouchCamera found in scene. This script will not work without this.");
		}
		if (pickableTransform == null)
		{
			pickableTransform = base.transform;
		}
		if (base.gameObject.GetComponent<Collider>() == null && base.gameObject.GetComponent<UnityEngine.Collider2D>() == null)
		{
			Debug.LogError("MobileTouchPickable must be placed on a gameObject that also has a Collider or Collider2D component attached.");
		}
		if (mobileTouchCam.GetComponent<MobilePickingController>() == null)
		{
			mobileTouchCam.gameObject.AddComponent<MobilePickingController>();
		}
	}

	public abstract bool PointInPick();
}
