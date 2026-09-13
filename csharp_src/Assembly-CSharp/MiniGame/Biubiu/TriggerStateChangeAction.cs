using System.Runtime.InteropServices;
using Box2DSharp.Common;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[StructLayout(LayoutKind.Sequential, Size = 1)]
[TitleAndCategory("开关状态改变", "功能")]
public struct TriggerStateChangeAction : IAction
{
	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		int intData = FuncData.GetIntData(world, entity, PropertyID.ConfigID);
		ToggleConfig toggleConfig = world.GetShared<SharedRuntime>().ResourceLoader.LoadConfig<ToggleConfig>(intData);
		if (toggleConfig.Type == ToggleConfig.ToggleType.RemoteSensing)
		{
			ref ComponentRotation reference = ref world.GetPool<ComponentRotation>().Get(entity);
			FP x = FuncComposeEntity.GetAngleByComponentEntityRoot(world, entity);
			bool boolData = FuncData.GetBoolData(world, entity, PropertyID.ToggleState);
			reference.Rotation = x + (FP)(boolData ? (0f - toggleConfig.Angle) : toggleConfig.Angle);
			ref ComponentPhysics reference2 = ref world.GetPool<ComponentPhysics>().Get(entity);
			reference2.Body.SetTransform(reference2.Body.GetPosition(), reference.Rotation * FP.Deg2Rad);
		}
	}
}
