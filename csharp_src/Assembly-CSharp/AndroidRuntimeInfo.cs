using System;
using System.Text;
using System.Text.RegularExpressions;
using GameFramework;
using SFSLitJson;
using UnityEngine;
using UnityEngine.Rendering;

public class AndroidRuntimeInfo : DefaultRuntimeInfo
{
	private const string TAG = "[AndroidRuntimeInfo]:";

	private long _nativeTotalMemory = -1L;

	private int _nativeCPUMaxFreqKHz = -1;

	private static readonly float[] ScoreSeq = new float[7] { 25f, 45f, 70f, 110f, 160f, 240f, 300f };

	private string _deviceModel = string.Empty;

	private DeviceLevel _deviceLevel = DeviceLevel.UnInitialized;

	private float _deviceScore = -2f;

	public long nativeTotalMemory
	{
		get
		{
			if (_nativeTotalMemory <= 0)
			{
				_nativeTotalMemory = (long)SystemInfo.systemMemorySize * 1024L * 1024;
			}
			return _nativeTotalMemory;
		}
		set
		{
			_nativeTotalMemory = value;
		}
	}

	public int nativeCPUMaxFreqKHz
	{
		get
		{
			if (_nativeCPUMaxFreqKHz <= 0)
			{
				_nativeCPUMaxFreqKHz = SystemInfo.processorFrequency * 1024;
			}
			return _nativeCPUMaxFreqKHz;
		}
		set
		{
			_nativeCPUMaxFreqKHz = value;
		}
	}

	public override string deviceModel
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

	public override DeviceLevel deviceLevel
	{
		get
		{
			if (_deviceLevel == DeviceLevel.UnInitialized)
			{
				_deviceLevel = DeviceLevel.Unknown;
				if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2)
				{
					_deviceLevel = DeviceLevel.Low;
					Log.Info("[AndroidRuntimeInfo]: LowLevel cause OpenGLES2");
				}
				else
				{
					for (int i = 0; i < ScoreSeq.Length; i++)
					{
						if (deviceScore < ScoreSeq[i])
						{
							_deviceLevel = (DeviceLevel)i;
							break;
						}
					}
					if (_deviceLevel == DeviceLevel.Unknown)
					{
						if (deviceScore >= ScoreSeq[ScoreSeq.Length - 1])
						{
							_deviceLevel = DeviceLevel.UltraHigh;
						}
						else
						{
							_deviceLevel = DeviceLevel.Mid;
						}
					}
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
				if (PlayerPrefs.HasKey("DEVICE_SCORE_SAVE_KEY_UNITY"))
				{
					_deviceScore = PlayerPrefs.GetFloat("DEVICE_SCORE_SAVE_KEY_UNITY");
				}
				else
				{
					_deviceScore = GetDeviceScoreFromModelAndGpu(deviceModel, SystemInfo.graphicsDeviceName);
					if (_deviceScore >= 0f)
					{
						PlayerPrefs.SetFloat("DEVICE_SCORE_SAVE_KEY_UNITY", _deviceScore);
					}
					else
					{
						_deviceScore = GetDeviceScoreFromCPUAndMemory(nativeCPUMaxFreqKHz, nativeTotalMemory);
					}
				}
			}
			return _deviceScore;
		}
	}

	public AndroidRuntimeInfo(string name)
		: base(name)
	{
	}

	public override void Init()
	{
		base.Init();
		try
		{
			string dataFromNative = GameEntry.Sdk.GetDataFromNative("LW_GetStaticAndroidRuntimeInfo", "");
			if (!string.IsNullOrEmpty(dataFromNative) && JsonMapper.ToObject(dataFromNative) != null)
			{
				Log.Info("[AndroidRuntimeInfo]: LW_GetStaticAndroidRuntimeInfo:" + dataFromNative);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}

	public override void LogRunTimeInfo()
	{
		DeviceLevel deviceLevel = this.deviceLevel;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("[AndroidRuntimeInfo]:");
		stringBuilder.AppendLine("name:" + base.name);
		stringBuilder.AppendLine("model:" + deviceModel);
		stringBuilder.AppendLine("gpu:" + SystemInfo.graphicsDeviceName);
		stringBuilder.AppendLine($"benchmarkScore:{deviceScore}");
		stringBuilder.AppendLine($"TotalMemory:{nativeTotalMemory}");
		stringBuilder.AppendLine($"CPUMaxFreqKHz:{nativeCPUMaxFreqKHz}");
		stringBuilder.AppendLine($"level:{deviceLevel}");
		stringBuilder.AppendLine($"firstInstall:{ClientConfig.IsAppFirstInstalled}");
		Log.Info(stringBuilder.ToString());
		ClientConfig.IsAppFirstInstalled = false;
	}

	private int JudgeMemoryLevel(long ram)
	{
		if (ram <= 0)
		{
			return -1;
		}
		float num = (float)Math.Round((double)ram / 1048576.0);
		int num2 = -1;
		if (num <= 1500f)
		{
			return 0;
		}
		if (num <= 2500f)
		{
			return 1;
		}
		if (num <= 3500f)
		{
			return 2;
		}
		if (num <= 6500f)
		{
			return 3;
		}
		if (num <= 8500f)
		{
			return 4;
		}
		return 5;
	}

	private int JudgeCPULevel(int freqKHz)
	{
		if (freqKHz <= 0)
		{
			return -1;
		}
		float num = (float)freqKHz / 1000f;
		int num2 = -1;
		if (num <= 1600f)
		{
			return 0;
		}
		if (num <= 2000f)
		{
			return 2;
		}
		if (num <= 2500f)
		{
			return 3;
		}
		if (num <= 2800f)
		{
			return 4;
		}
		return 5;
	}

	private float GetDeviceScoreFromModelAndGpu(string model, string gpu)
	{
		try
		{
			float num = AndroidDeviceModelScore.QueryModel(GetCleanDeviceName(model));
			if (num < 0f)
			{
				num = AndroidDeviceGpuScore.QueryGpu(GetCleanGpuName(gpu));
			}
			return num / 100f;
		}
		catch (Exception ex)
		{
			Log.Error("[AndroidRuntimeInfo]: GetDeviceScoreFromModelAndGpu error:" + ex.ToString());
			return -1f;
		}
	}

	private float GetDeviceScoreFromCPUAndMemory(int freqKHz, long totalMemory)
	{
		try
		{
			int num = JudgeCPULevel(freqKHz);
			int num2 = JudgeMemoryLevel(totalMemory);
			if (num < 0 && num2 < 0)
			{
				return -1f;
			}
			if (num2 < 0)
			{
				return ScoreSeq[num] - 1f;
			}
			if (num < 0)
			{
				return ScoreSeq[num2] - 1f;
			}
			if (num < num2)
			{
				return ScoreSeq[num] - 1f;
			}
			return ScoreSeq[num2] - 1f;
		}
		catch (Exception ex)
		{
			Log.Error("[AndroidRuntimeInfo]: GetDeviceScoreFromCPUAndMemory error:" + ex.ToString());
			return -1f;
		}
	}

	private string GetCleanDeviceName(string modelName)
	{
		return Regex.Replace(modelName.ToLower(), "[^a-z0-9]", "");
	}

	private string GetCleanGpuName(string gpuName)
	{
		gpuName = gpuName.Replace("(TM)", "");
		return Regex.Replace(gpuName.ToLower(), "[^a-z0-9]", "");
	}
}
