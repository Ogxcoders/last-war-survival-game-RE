using UnityEngine;

public class MissileWarningAnimation : MonoBehaviour
{
	public class Param
	{
		public Vector3 pos;
	}

	public Param param;

	protected internal void CSShow(object userData)
	{
		param = userData as Param;
		_ = param;
		base.transform.position = param.pos;
	}
}
