using System;
using UnityEngine;

public class ResourceUpEffect : MonoBehaviour
{
	public class Param
	{
		public Vector3 worldPos;

		public Transform target;

		public ResourceType resType;

		public Action EffectUpDone;

		public bool notTrans;
	}

	private Param paramData;

	private float _time;

	private Vector3 _startPos;

	public void Init(object userData)
	{
		base.transform.SetParent(SceneManager.World.DynamicObjNode);
		base.transform.localScale = Vector3.one;
		paramData = userData as Param;
		if (userData != null)
		{
			Vector3 worldPos = paramData.worldPos;
			if (!paramData.notTrans)
			{
				_startPos = Camera.main.WorldToScreenPoint(worldPos);
			}
			else
			{
				_startPos = worldPos;
			}
		}
		_time = 0f;
	}

	private void Update()
	{
		if (paramData != null && !(paramData.target == null))
		{
			_time += Time.deltaTime;
			base.transform.position = Vector3.Lerp(_startPos, paramData.target.transform.position, _time);
			if ((double)_time > 0.99)
			{
				paramData.EffectUpDone?.Invoke();
			}
		}
	}

	private void OnDisable()
	{
		paramData.EffectUpDone = null;
		paramData = null;
	}
}
