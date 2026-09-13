using Leopotam.EcsLite;

namespace MiniGame.Core;

public class SystemTemplateEditable : SystemTemplateInstantiate, IEcsRunSystem, IEcsSystem
{
	public void Run(IEcsSystems systems)
	{
		PreInit(systems);
	}
}
