using PVEBattleLogic.Bullet;
using PVEBattleLogic.Effect;
using PVEBattleLogic.Unit;

namespace PVEBattleLogic;

public static class PVEBattleLogicManager
{
	public static void Dispose()
	{
		BulletViewFacade.Dispose();
		EffectViewFacade.Dispose();
		UnitViewFacade.Dispose();
	}
}
