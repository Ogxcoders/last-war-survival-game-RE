using Box2DSharp.Common;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("设置数据", "数据")]
public struct SetDataAction : IAction
{
	public PropertyID PropertyID;

	public DataOp DataOp;

	public FP DeltaValue;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		SetDataAction setDataAction = ((action is SetDataAction) ? ((SetDataAction)(object)action) : default(SetDataAction));
		EcsPool<ComponentData> pool = world.GetPool<ComponentData>();
		if (pool.Has(entity))
		{
			FP x = pool.Get(entity).GetPropertyValue(setDataAction.PropertyID);
			if (setDataAction.DataOp == DataOp.Add)
			{
				x += setDataAction.DeltaValue;
			}
			else if (setDataAction.DataOp == DataOp.Reduce)
			{
				x -= setDataAction.DeltaValue;
			}
			else if (setDataAction.DataOp == DataOp.Negate)
			{
				x = ((x.AsInt != 1) ? 1 : 0);
			}
			FuncData.SetData(world, entity, setDataAction.PropertyID, x);
		}
	}
}
