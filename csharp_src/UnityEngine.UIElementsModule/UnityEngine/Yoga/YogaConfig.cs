using System;

namespace UnityEngine.Yoga;

internal class YogaConfig
{
	internal static readonly YogaConfig Default = new YogaConfig(Native.YGConfigGetDefault());

	private IntPtr _ygConfig;

	private Logger _logger;

	internal IntPtr Handle => _ygConfig;

	public Logger Logger
	{
		get
		{
			return _logger;
		}
		set
		{
			_logger = value;
		}
	}

	public bool UseWebDefaults
	{
		get
		{
			return Native.YGConfigGetUseWebDefaults(_ygConfig);
		}
		set
		{
			Native.YGConfigSetUseWebDefaults(_ygConfig, value);
		}
	}

	public float PointScaleFactor
	{
		set
		{
			Native.YGConfigSetPointScaleFactor(_ygConfig, value);
		}
	}

	private YogaConfig(IntPtr ygConfig)
	{
		_ygConfig = ygConfig;
		if (_ygConfig == IntPtr.Zero)
		{
			throw new InvalidOperationException("Failed to allocate native memory");
		}
	}

	public YogaConfig()
		: this(Native.YGConfigNew())
	{
	}

	~YogaConfig()
	{
		if (Handle != Default.Handle)
		{
			Native.YGConfigFree(Handle);
		}
	}

	public void SetExperimentalFeatureEnabled(YogaExperimentalFeature feature, bool enabled)
	{
		Native.YGConfigSetExperimentalFeatureEnabled(_ygConfig, feature, enabled);
	}

	public bool IsExperimentalFeatureEnabled(YogaExperimentalFeature feature)
	{
		return Native.YGConfigIsExperimentalFeatureEnabled(_ygConfig, feature);
	}

	public static int GetInstanceCount()
	{
		return Native.YGConfigGetInstanceCount();
	}

	public static void SetDefaultLogger(Logger logger)
	{
		Default.Logger = logger;
	}
}
