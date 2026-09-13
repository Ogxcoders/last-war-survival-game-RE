using System.Collections.Generic;

namespace System;

public static class AppContext
{
	[Flags]
	private enum SwitchValueState
	{
		HasFalseValue = 1,
		HasTrueValue = 2,
		HasLookedForOverride = 4,
		UnknownValue = 8
	}

	private static readonly Dictionary<string, SwitchValueState> s_switchMap = new Dictionary<string, SwitchValueState>();

	private static volatile bool s_defaultsInitialized = false;

	public static string BaseDirectory => ((string)AppDomain.CurrentDomain.GetData("APP_CONTEXT_BASE_DIRECTORY")) ?? AppDomain.CurrentDomain.BaseDirectory;

	public static string TargetFrameworkName => AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;

	public static object GetData(string name)
	{
		return AppDomain.CurrentDomain.GetData(name);
	}

	private static void InitializeDefaultSwitchValues()
	{
		lock (s_switchMap)
		{
			if (!s_defaultsInitialized)
			{
				AppContextDefaultValues.PopulateDefaultValues();
				s_defaultsInitialized = true;
			}
		}
	}

	public static bool TryGetSwitch(string switchName, out bool isEnabled)
	{
		if (switchName == null)
		{
			throw new ArgumentNullException("switchName");
		}
		if (switchName.Length == 0)
		{
			throw new ArgumentException(Environment.GetResourceString("Empty name is not legal."), "switchName");
		}
		if (!s_defaultsInitialized)
		{
			InitializeDefaultSwitchValues();
		}
		isEnabled = false;
		lock (s_switchMap)
		{
			if (s_switchMap.TryGetValue(switchName, out var value))
			{
				if (value == SwitchValueState.UnknownValue)
				{
					isEnabled = false;
					return false;
				}
				isEnabled = (value & SwitchValueState.HasTrueValue) == SwitchValueState.HasTrueValue;
				if ((value & SwitchValueState.HasLookedForOverride) == SwitchValueState.HasLookedForOverride)
				{
					return true;
				}
				if (AppContextDefaultValues.TryGetSwitchOverride(switchName, out var overrideValue))
				{
					isEnabled = overrideValue;
				}
				s_switchMap[switchName] = (SwitchValueState)(((!isEnabled) ? 1 : 2) | 4);
				return true;
			}
			if (AppContextDefaultValues.TryGetSwitchOverride(switchName, out var overrideValue2))
			{
				isEnabled = overrideValue2;
				s_switchMap[switchName] = (SwitchValueState)(((!isEnabled) ? 1 : 2) | 4);
				return true;
			}
			s_switchMap[switchName] = SwitchValueState.UnknownValue;
		}
		return false;
	}

	public static void SetSwitch(string switchName, bool isEnabled)
	{
		if (switchName == null)
		{
			throw new ArgumentNullException("switchName");
		}
		if (switchName.Length == 0)
		{
			throw new ArgumentException(Environment.GetResourceString("Empty name is not legal."), "switchName");
		}
		if (!s_defaultsInitialized)
		{
			InitializeDefaultSwitchValues();
		}
		SwitchValueState value = (SwitchValueState)(((!isEnabled) ? 1 : 2) | 4);
		lock (s_switchMap)
		{
			s_switchMap[switchName] = value;
		}
	}

	internal static void DefineSwitchDefault(string switchName, bool isEnabled)
	{
		s_switchMap[switchName] = ((!isEnabled) ? SwitchValueState.HasFalseValue : SwitchValueState.HasTrueValue);
	}

	internal static void DefineSwitchOverride(string switchName, bool isEnabled)
	{
		s_switchMap[switchName] = (SwitchValueState)(((!isEnabled) ? 1 : 2) | 4);
	}
}
