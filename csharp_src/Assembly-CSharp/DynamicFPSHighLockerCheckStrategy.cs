internal class DynamicFPSHighLockerCheckStrategy : NeedHighFPSCheckStrategyBase
{
	public override void Check(InputHelper.InputState last, InputHelper.InputState curr)
	{
		pass = DynamicFPSHighLocker.count > 0;
		keep = 0f;
	}
}
