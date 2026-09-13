using System.ComponentModel;

namespace ParkourStage.Runtime.Data;

public enum MoveRuleType
{
	[Description("无运动")]
	None,
	[Description("圆形")]
	Circle,
	[Description("类椭圆形")]
	EllipseLike,
	[Description("左右往复")]
	PingPong
}
