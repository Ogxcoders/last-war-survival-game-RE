using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class SceneLowFpsMonitor : MonoBehaviour
{
	private struct LowFpsConfig
	{
		public int fpsCount;

		public int lowCount;

		public float fps30Threshold;

		public float fps45Threshold;

		public float fps60Threshold;

		public float countdown;
	}

	private float _unscaledDeltaTime;

	private float _avgFps;

	private float _currentFps;

	private List<float> _fpsSamples = new List<float>();

	private List<float> _avgFpsSamples = new List<float>();

	[SerializeField]
	private int AvgFpsThreshold = 20;

	[SerializeField]
	private int FpsSamplesMaxCount = 200;

	[SerializeField]
	private float AverageSampleInterval = 10f;

	[SerializeField]
	private int AverageSampleCount = 3;

	private float _avgSampleTime;

	private bool _startStats;

	private static SceneLowFpsMonitor instance;

	private LowFpsConfig lowFpsConfig;

	private bool lowFpsConfigInited;

	private float sampleCd;

	private int lowFpsCount;

	private int normalFpsCount;

	public float CurrentFPS => _currentFps;

	public float AvgFps => _avgFps;

	public static SceneLowFpsMonitor Instance => instance;

	public float NewAvgFps => _avgFps;

	public int TargetFrameRate => Application.targetFrameRate;

	public void StartStats()
	{
		_startStats = true;
	}

	private void Update()
	{
		if (instance == null)
		{
			instance = this;
		}
		UpdateLowFpsWarning();
		if (!_startStats)
		{
			return;
		}
		_unscaledDeltaTime = Time.unscaledDeltaTime;
		_avgFps = 0f;
		_currentFps = 1f / _unscaledDeltaTime;
		if (_fpsSamples.Count >= FpsSamplesMaxCount)
		{
			_fpsSamples.RemoveAt(0);
		}
		_fpsSamples.Add(_currentFps);
		for (int i = 0; i < _fpsSamples.Count; i++)
		{
			_avgFps += _fpsSamples[i];
		}
		_avgFps /= _fpsSamples.Count;
		_avgSampleTime += _unscaledDeltaTime;
		if (_avgSampleTime > AverageSampleInterval)
		{
			_avgSampleTime = 0f;
			if (_avgFpsSamples.Count >= AverageSampleCount)
			{
				_avgFpsSamples.RemoveAt(0);
			}
			_avgFpsSamples.Add(_avgFps);
		}
		if (_avgFpsSamples.Count < AverageSampleCount)
		{
			return;
		}
		bool flag = true;
		foreach (float avgFpsSample in _avgFpsSamples)
		{
			if (avgFpsSample > (float)AvgFpsThreshold)
			{
				flag = false;
			}
		}
		if (flag)
		{
			_startStats = false;
		}
	}

	private void UpdateLowFpsWarning()
	{
		if (!lowFpsConfigInited)
		{
			lowFpsConfigInited = true;
			try
			{
				LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetLowFpsConfig");
				lowFpsConfig.fpsCount = luaTable.Get<int>("fpsCount");
				lowFpsConfig.lowCount = luaTable.Get<int>("lowCount");
				lowFpsConfig.fps30Threshold = luaTable.Get<float>("fps30Threshold");
				lowFpsConfig.fps45Threshold = luaTable.Get<float>("fps45Threshold");
				lowFpsConfig.fps60Threshold = luaTable.Get<float>("fps60Threshold");
				lowFpsConfig.countdown = luaTable.Get<float>("countdown");
			}
			catch (Exception)
			{
				lowFpsConfig.fpsCount = 0;
			}
		}
		if (lowFpsConfig.fpsCount <= 0)
		{
			return;
		}
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		if (unscaledDeltaTime <= 0f)
		{
			return;
		}
		sampleCd -= unscaledDeltaTime;
		if (sampleCd > 0f)
		{
			return;
		}
		float num = 1f / unscaledDeltaTime;
		int targetFrameRate = Application.targetFrameRate;
		float num2 = lowFpsConfig.fps30Threshold;
		switch (targetFrameRate)
		{
		case 30:
			num2 = lowFpsConfig.fps30Threshold;
			break;
		case 45:
			num2 = lowFpsConfig.fps45Threshold;
			break;
		case 60:
			num2 = lowFpsConfig.fps60Threshold;
			break;
		}
		if (num < num2)
		{
			lowFpsCount++;
		}
		else
		{
			normalFpsCount++;
		}
		if (lowFpsCount + normalFpsCount >= lowFpsConfig.fpsCount)
		{
			if (lowFpsCount > lowFpsConfig.lowCount)
			{
				GameEntry.Event?.Fire(EventId.LowFps);
				sampleCd = lowFpsConfig.countdown;
			}
			lowFpsCount = 0;
			normalFpsCount = 0;
		}
	}

	private void OnDisable()
	{
		instance = null;
	}
}
