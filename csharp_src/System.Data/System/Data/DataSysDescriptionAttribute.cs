using System.ComponentModel;

namespace System.Data;

[AttributeUsage(AttributeTargets.All)]
[Obsolete("DataSysDescriptionAttribute has been deprecated.  https://go.microsoft.com/fwlink/?linkid=14202", false)]
public class DataSysDescriptionAttribute : DescriptionAttribute
{
	private bool _replaced;

	public override string Description
	{
		get
		{
			if (!_replaced)
			{
				_replaced = true;
				base.DescriptionValue = base.Description;
			}
			return base.Description;
		}
	}

	[Obsolete("DataSysDescriptionAttribute has been deprecated.  https://go.microsoft.com/fwlink/?linkid=14202", false)]
	public DataSysDescriptionAttribute(string description)
		: base(description)
	{
	}
}
