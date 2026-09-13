using System.ComponentModel;

namespace ParkourStage.Runtime.Data;

public enum MonsterBornType
{
	[Description("一个坐标一个怪；通过[monster]找怪(只能配静态怪物)")]
	SingleMonster = 1,
	[Description("触发线触发，在范围内生成若干怪物")]
	TriggerLineArea,
	[Description("触发线触发，在坐标点生成若干怪物")]
	TriggerLinePoint,
	[Description("一个坐标一个buff球；通过[buff_item]找buff球")]
	BuffBall,
	[Description("一个资源")]
	Resource,
	[Description("指定怪物死亡触发")]
	OnMonsterDeath,
	[Description("进入bonus环节后触发")]
	OnEnterBonus,
	[Description("空投")]
	AirDrop
}
