using System;
using System.Collections.Generic;
using UnityEngine;

public class ActivityAlarmClockCityBuildingEffect : MonoBehaviour
{
	private List<MeshRenderer> _renderers;

	private MaterialPropertyBlock _materialPropertyBlock;

	private int _curHour;

	private int _curMinute;

	private long _lastCheckTime;

	private long _lastGetOffsetTime;

	private bool _isShowServerTime = true;

	private const string TARGET_OBJ_PATH = "ModelGo/Normal/A_build_shibaoguangchang/shibaoguangchang_number/shibaoguangchang_plan_0{0}";

	private float _luaOffset;

	private void Awake()
	{
		if (!(base.transform != null))
		{
			return;
		}
		_renderers = _renderers ?? new List<MeshRenderer>();
		_renderers.Clear();
		_materialPropertyBlock = new MaterialPropertyBlock();
		for (int i = 1; i <= 5; i++)
		{
			if (i != 3)
			{
				CollectRenderers(i);
			}
		}
	}

	private void Update()
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		if (serverTime - _lastGetOffsetTime > 60000)
		{
			_lastGetOffsetTime = serverTime;
			_luaOffset = GameEntry.Lua.CallWithReturn<float>("CSharpCallLuaInterface.GetLuaLocalUTCOffset");
		}
		if (serverTime - _lastCheckTime > 1000)
		{
			_lastCheckTime = serverTime;
			UpdateTimeView(serverTime);
		}
	}

	private void CollectRenderers(int num)
	{
		if (!(base.transform != null))
		{
			return;
		}
		Transform transform = base.transform.Find($"ModelGo/Normal/A_build_shibaoguangchang/shibaoguangchang_number/shibaoguangchang_plan_0{num}");
		if (transform != null)
		{
			MeshRenderer component = transform.GetComponent<MeshRenderer>();
			if (component != null)
			{
				_renderers.Add(component);
			}
		}
	}

	public void UpdateData(bool isShowServerTime)
	{
		_isShowServerTime = isShowServerTime;
		long serverTime = GameEntry.Timer.GetServerTime();
		UpdateTimeView(serverTime, isForceChange: true);
	}

	private void UpdateTimeView(long timeStamp, bool isForceChange = false)
	{
		int num = 0;
		int num2 = 0;
		if (_isShowServerTime)
		{
			DateTime dateTime = GameEntry.Timer.TimeStampToServerTime(timeStamp);
			num = dateTime.Hour;
			num2 = dateTime.Minute;
		}
		else
		{
			DateTime localTimeFromUtcOffset = GameEntry.Timer.GetLocalTimeFromUtcOffset(timeStamp, _luaOffset);
			num = localTimeFromUtcOffset.Hour;
			num2 = localTimeFromUtcOffset.Minute;
		}
		if (num != _curHour || isForceChange)
		{
			_curHour = num;
			int value = 0;
			int value2 = 0;
			if (_curHour > 0)
			{
				value = Mathf.FloorToInt((float)num / 10f);
				value2 = Mathf.FloorToInt((float)num % 10f);
			}
			SetMaterialPropertyBlock(0, value);
			SetMaterialPropertyBlock(1, value2);
		}
		if (num2 != _curMinute || isForceChange)
		{
			_curMinute = num2;
			int value3 = 0;
			int value4 = 0;
			if (num2 > 0)
			{
				value3 = Mathf.FloorToInt((float)num2 / 10f);
				value4 = Mathf.FloorToInt((float)num2 % 10f);
			}
			SetMaterialPropertyBlock(2, value3);
			SetMaterialPropertyBlock(3, value4);
		}
	}

	private void SetMaterialPropertyBlock(int index, int value)
	{
		if (_renderers.Count > 0 && index < _renderers.Count)
		{
			_materialPropertyBlock.SetInt("_SetF", value);
			_renderers[index].SetPropertyBlock(_materialPropertyBlock);
		}
	}
}
