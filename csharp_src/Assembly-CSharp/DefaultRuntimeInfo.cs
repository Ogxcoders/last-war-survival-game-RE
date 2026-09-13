using UnityEngine;

public class DefaultRuntimeInfo : IRuntimeInfo
{
	protected const string DEVICE_SCORE_SAVE_KEY = "DEVICE_SCORE_SAVE_KEY_UNITY";

	public const string UnknownOrHideDeviceModel = "unknown or hide";

	private string _deviceModel = string.Empty;

	public string name { get; private set; }

	public virtual long currentMemory => 0L;

	public virtual string deviceModel
	{
		get
		{
			if (string.IsNullOrEmpty(_deviceModel))
			{
				_deviceModel = SystemInfo.deviceModel;
				if (string.IsNullOrEmpty(_deviceModel))
				{
					_deviceModel = "unknown or hide";
				}
			}
			return _deviceModel;
		}
	}

	public virtual DeviceLevel deviceLevel => DeviceLevel.High;

	public virtual float deviceScore => -1f;

	public virtual string operatingSystem => SystemInfo.operatingSystem;

	public DefaultRuntimeInfo(string runtimeName)
	{
		name = runtimeName;
	}

	public virtual void Init()
	{
	}

	public virtual void LogRunTimeInfo()
	{
	}
}
