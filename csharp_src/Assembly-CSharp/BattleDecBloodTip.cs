using System;
using UnityEngine;

public class BattleDecBloodTip : MonoBehaviour
{
	public class Param
	{
		public Vector3 startPos;

		public int num;

		public string path;
	}

	[SerializeField]
	private SuperTextMesh _num;

	[SerializeField]
	private float _delay = 0.5f;

	[SerializeField]
	public float duringTime;

	[SerializeField]
	private SimpleAnimation anim;

	private Vector3 newpos;

	private float distance;

	private bool _isShow;

	private bool _isOnce;

	private float time;

	private InstanceRequest _InstanceRequest;

	protected internal void CSShow(object userData, InstanceRequest instanceRequest)
	{
		_InstanceRequest = instanceRequest;
		time = 0f;
		if (userData is Param)
		{
			Param param = userData as Param;
			if (param.path != null && param.path == "Assets/Main/Prefabs/UI/BattleWord/BattleDecBloodTip.prefab")
			{
				base.transform.position = param.startPos + new Vector3(0f, 2.5f, 0f);
				newpos = base.transform.localPosition;
			}
			else
			{
				base.transform.position = param.startPos + new Vector3(0f, 2.5f, 0f);
				newpos = base.transform.localPosition;
			}
			_isShow = true;
			if (param.num <= 0)
			{
				param.num = Math.Abs(param.num);
				_num.text = $"+{param.num.ToString()}";
			}
			else
			{
				_num.text = $"-{param.num.ToString()}";
			}
			_isOnce = true;
		}
	}

	private void Update()
	{
		if (!_isShow)
		{
			return;
		}
		distance = Vector3.Distance(base.transform.localPosition, newpos);
		time += Time.deltaTime;
		if (distance < 2f)
		{
			if (_isOnce)
			{
				if (_InstanceRequest != null)
				{
					if (anim != null)
					{
						anim.Play("showScale");
					}
					_isShow = false;
					UnityEngine.Object.Destroy(_InstanceRequest.gameObject, duringTime);
				}
			}
			else
			{
				base.gameObject.SetActive(value: false);
				_isShow = false;
			}
		}
		else if (time > _delay)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, newpos, 0.05f);
		}
	}
}
