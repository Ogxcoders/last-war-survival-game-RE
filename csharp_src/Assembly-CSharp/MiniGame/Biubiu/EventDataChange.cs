using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("数据变化", "事件")]
public struct EventDataChange : IEvent
{
	public EcsPackedEntity Sender { get; }

	public EcsPackedEntity Target { get; }

	public PropertyID PropertyID { get; }

	public EventDataChange(EcsPackedEntity sender, EcsPackedEntity target, PropertyID propertyID)
	{
		Sender = sender;
		Target = target;
		PropertyID = propertyID;
	}
}
