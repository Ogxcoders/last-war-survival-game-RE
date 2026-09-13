public class PressedCheckStrategy : NeedHighFPSCheckStrategyBase
{
	public PressedCheckStrategy(float keep)
	{
		this.keep = keep;
	}

	public override void Check(InputHelper.InputState last, InputHelper.InputState curr)
	{
		pass = InputHelper.IsPressed(curr);
	}
}
