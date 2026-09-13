using UnityEngine;

public class MissileExplodeAnimation : MonoBehaviour
{
	public class Param
	{
		public Vector3 pos;
	}

	protected internal void CSShow(object userData)
	{
		if (userData is Param param)
		{
			base.transform.position = param.pos;
		}
	}

	public void OnAnimationDone()
	{
		Object.Destroy(base.gameObject);
		MissileExplodeAnimationControll.Instance.HideAni(base.gameObject);
	}
}
