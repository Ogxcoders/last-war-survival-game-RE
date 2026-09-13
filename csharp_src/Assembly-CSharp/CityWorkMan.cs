using UnityEngine;

public class CityWorkMan : MonoBehaviour
{
	public struct WorkManMoveStruct
	{
		public Vector3 endPos;

		public Quaternion endQuat;

		public Vector3 startPos;

		public Quaternion startQuat;

		public Quaternion moveQuat;

		public Vector3 gatheredPos;

		public Quaternion gatheredQuat;

		public float gatheredTime;

		public Vector3 spreadPos;

		public Quaternion spreadQuat;

		public float spreadTime;

		public float time;
	}

	public class AnimName
	{
		public const string Idle = "idle";

		public const string Move = "xiaoren_run";

		public const string OpenFog = "xiaoren_scanning";

		public const string Garbage = "xiaoren_work";

		public const string GarbageSuccess = "xiaoren_show";

		public const string GarbageFail = "xiaoren_fail";

		public const string Attack = "xiaoren_work";
	}

	[SerializeField]
	private SimpleAnimation _anim;

	private const string scanEffectPath = "Assets/_Art/Effect/prefab/scene/xinshou/VFX_xishou_saomiao.prefab";

	private GameObject scanEffectObject;

	private InstanceRequest scanEffectInst;

	private const string effectHangPoint = "A_soldie_shxr_env/A_soldie@xiaoren_skin/To_unity/DeformationSystem/Root";

	public int Index { get; set; }

	public WorkManMoveStruct MoveStruct { get; set; }

	public void PlayAnim(string animName)
	{
		_anim.Play(animName);
		if (animName == "xiaoren_run" || animName == "idle")
		{
			float normalizedTime = 1f * (float)Random.Range(0, 50) / 50f;
			_anim[animName].normalizedTime = normalizedTime;
		}
		if (animName == "xiaoren_scanning")
		{
			AddScanEffect();
		}
		else
		{
			RemoveScanEffect();
		}
	}

	private void AddScanEffect()
	{
		if (scanEffectInst != null)
		{
			scanEffectInst.Destroy();
			scanEffectInst = null;
		}
		scanEffectInst = GameEntry.Resource.InstantiateAsync("Assets/_Art/Effect/prefab/scene/xinshou/VFX_xishou_saomiao.prefab");
		scanEffectInst.completed += delegate
		{
			GameObject gameObject = scanEffectInst.gameObject;
			Transform transform = base.gameObject.transform.Find("A_soldie_shxr_env/A_soldie@xiaoren_skin/To_unity/DeformationSystem/Root");
			if (transform != null)
			{
				gameObject.transform.SetParent(transform);
				gameObject.gameObject.SetActive(value: true);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localPosition = new Vector3(0.28f, 0.27f, 0.23f);
				gameObject.transform.localRotation = Quaternion.Euler(0f, 90f, 90f);
				gameObject.transform.localScale = Vector3.one;
			}
		};
	}

	private void RemoveScanEffect()
	{
		if (scanEffectInst != null)
		{
			scanEffectInst.Destroy();
			scanEffectInst = null;
			scanEffectObject = null;
		}
	}

	public void ChangePosition(Vector3 pos)
	{
		base.transform.position = pos;
	}

	public void ChangeRotation(Quaternion rot)
	{
		base.transform.rotation = rot;
	}

	public Vector3 GetPosition()
	{
		return base.transform.position;
	}

	public Quaternion GetRotation()
	{
		return base.transform.rotation;
	}
}
