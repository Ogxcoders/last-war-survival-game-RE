using UnityEngine;
using UnityEngine.Rendering;

public sealed class RuntimeInfoManager
{
	private IRuntimeInfo _runtimeInfo;

	private string _model;

	private DeviceLevel _deviceLevel = DeviceLevel.UnInitialized;

	private string _operatingSystem;

	public void Init()
	{
		_runtimeInfo = new AndroidRuntimeInfo("Android");
		_runtimeInfo.Init();
	}

	public string GetModel()
	{
		if (string.IsNullOrEmpty(_model))
		{
			_model = _runtimeInfo.deviceModel;
		}
		return _model;
	}

	public DeviceLevel GetDeviceLevel()
	{
		if (_deviceLevel == DeviceLevel.UnInitialized)
		{
			_deviceLevel = _runtimeInfo.deviceLevel;
			bool flag = SystemInfo.copyTextureSupport != CopyTextureSupport.None;
			if (!(SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth) || flag))
			{
				_deviceLevel = DeviceLevel.Low;
			}
		}
		return _deviceLevel;
	}

	public float GetDeviceScore()
	{
		return _runtimeInfo.deviceScore;
	}

	public string GetOperatingSystem()
	{
		if (string.IsNullOrEmpty(_operatingSystem))
		{
			_operatingSystem = _runtimeInfo.operatingSystem;
		}
		return _operatingSystem;
	}

	public void LogRuntimeInfo()
	{
		_runtimeInfo.LogRunTimeInfo();
	}
}
