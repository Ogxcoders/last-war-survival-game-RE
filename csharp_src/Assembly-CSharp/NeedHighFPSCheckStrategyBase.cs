public abstract class NeedHighFPSCheckStrategyBase
{
	public virtual bool pass { get; protected set; }

	public virtual float keep { get; protected set; }

	public abstract void Check(InputHelper.InputState last, InputHelper.InputState curr);
}
