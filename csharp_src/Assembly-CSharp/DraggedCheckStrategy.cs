using UnityEngine;

public class DraggedCheckStrategy : NeedHighFPSCheckStrategyBase
{
	private class MobileDraggedCheckStrategy : NeedHighFPSCheckStrategyBase
	{
		public override void Check(InputHelper.InputState last, InputHelper.InputState curr)
		{
			pass = false;
			for (int i = 0; i < curr.touchPoints.Count; i++)
			{
				if (curr.touchPoints[i].phase == TouchPhase.Moved)
				{
					pass = true;
				}
			}
			keep = 0f;
		}
	}

	private class StandaloneDraggedCheckStrategy : NeedHighFPSCheckStrategyBase
	{
		private float m_SqrMagnitude;

		public StandaloneDraggedCheckStrategy(float magnitude)
		{
			m_SqrMagnitude = magnitude * magnitude;
		}

		public override void Check(InputHelper.InputState last, InputHelper.InputState curr)
		{
			pass = InputHelper.IsPressed(curr) && last != null && curr != null && (curr.mousePosition - last.mousePosition).sqrMagnitude > m_SqrMagnitude;
			keep = 0f;
		}
	}

	private NeedHighFPSCheckStrategyBase m_Strategy;

	public DraggedCheckStrategy(float keep, float magnitude = 10f)
	{
		if (Application.isMobilePlatform)
		{
			m_Strategy = new MobileDraggedCheckStrategy();
		}
		else
		{
			m_Strategy = new StandaloneDraggedCheckStrategy(magnitude);
		}
		this.keep = keep;
	}

	public override void Check(InputHelper.InputState last, InputHelper.InputState curr)
	{
		m_Strategy.Check(last, curr);
		pass = m_Strategy.pass;
	}
}
