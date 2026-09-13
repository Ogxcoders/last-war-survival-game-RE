using System.Security.Permissions;

namespace System.Diagnostics;

[SwitchLevel(typeof(TraceLevel))]
public class TraceSwitch : Switch
{
	public TraceLevel Level
	{
		get
		{
			return (TraceLevel)base.SwitchSetting;
		}
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		set
		{
			if (value < TraceLevel.Off || value > TraceLevel.Verbose)
			{
				throw new ArgumentException(global::SR.GetString("The Level must be set to a value in the enumeration TraceLevel."));
			}
			base.SwitchSetting = (int)value;
		}
	}

	public bool TraceError => Level >= TraceLevel.Error;

	public bool TraceWarning => Level >= TraceLevel.Warning;

	public bool TraceInfo => Level >= TraceLevel.Info;

	public bool TraceVerbose => Level == TraceLevel.Verbose;

	public TraceSwitch(string displayName, string description)
		: base(displayName, description)
	{
	}

	public TraceSwitch(string displayName, string description, string defaultSwitchValue)
		: base(displayName, description, defaultSwitchValue)
	{
	}

	protected override void OnSwitchSettingChanged()
	{
		int num = base.SwitchSetting;
		if (num < 0)
		{
			base.SwitchSetting = 0;
		}
		else if (num > 4)
		{
			base.SwitchSetting = 4;
		}
	}

	protected override void OnValueChanged()
	{
		base.SwitchSetting = (int)Enum.Parse(typeof(TraceLevel), base.Value, ignoreCase: true);
	}
}
