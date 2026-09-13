using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("时间触发", "事件")]
public struct EventTime : IEvent
{
	public EcsPackedEntity Sender { get; }

	public EcsPackedEntity Target { get; }

	public int TimerID { get; }

	public int TriggerCount { get; }

	public EventTime(EcsPackedEntity sender, int timer, int count, EcsPackedEntity target)
	{
		Sender = sender;
		Target = target;
		TimerID = timer;
		TriggerCount = count;
	}
}
