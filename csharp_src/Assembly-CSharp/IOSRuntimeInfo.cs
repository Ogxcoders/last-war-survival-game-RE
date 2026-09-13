using System.Text;
using GameFramework;
using UnityEngine;

public class IOSRuntimeInfo : DefaultRuntimeInfo
{
	private const string TAG = "[iOSRuntimeInfo]:";

	private DeviceLevel _deviceLevel = DeviceLevel.UnInitialized;

	private float _deviceScore = -2f;

	public override DeviceLevel deviceLevel
	{
		get
		{
			if (_deviceLevel == DeviceLevel.UnInitialized)
			{
				if (deviceScore >= 140f)
				{
					_deviceLevel = DeviceLevel.UltraHigh;
				}
				else if (deviceScore >= 120f)
				{
					_deviceLevel = DeviceLevel.High;
				}
				else if (deviceScore >= 100f)
				{
					_deviceLevel = DeviceLevel.MidHigh;
				}
				else if (deviceScore >= 80f)
				{
					_deviceLevel = DeviceLevel.Mid;
				}
				else if (deviceScore >= 60f)
				{
					_deviceLevel = DeviceLevel.MidLow;
				}
				else if (deviceScore >= 40f)
				{
					_deviceLevel = DeviceLevel.Low;
				}
				else
				{
					_deviceLevel = DeviceLevel.UltraLow;
				}
			}
			return _deviceLevel;
		}
	}

	public override float deviceScore
	{
		get
		{
			if (_deviceScore < -1f)
			{
				_deviceScore = IOSDevicesScore.QueryModelScore(deviceModel);
			}
			if (_deviceScore < 0f)
			{
				_deviceScore = 100f;
			}
			return _deviceScore;
		}
	}

	public IOSRuntimeInfo(string runtimeName)
		: base(runtimeName)
	{
	}

	public override void LogRunTimeInfo()
	{
		DeviceLevel deviceLevel = this.deviceLevel;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("[iOSRuntimeInfo]:");
		stringBuilder.AppendLine("name:" + base.name);
		stringBuilder.AppendLine("model:" + deviceModel);
		stringBuilder.AppendLine("gpu:" + SystemInfo.graphicsDeviceName);
		stringBuilder.AppendLine($"benchmarkScore:{deviceScore}");
		stringBuilder.AppendLine($"level:{deviceLevel}");
		stringBuilder.AppendLine($"firstInstall:{ClientConfig.IsAppFirstInstalled}");
		Log.Info(stringBuilder.ToString());
		ClientConfig.IsAppFirstInstalled = false;
	}
}
