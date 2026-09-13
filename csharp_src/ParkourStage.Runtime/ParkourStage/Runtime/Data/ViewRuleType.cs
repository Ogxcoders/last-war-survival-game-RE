using System.ComponentModel;

namespace ParkourStage.Runtime.Data;

public enum ViewRuleType
{
	[Description("刷新点出生即可见")]
	None,
	[Description("距离屏幕中心偏移值")]
	Offset
}
