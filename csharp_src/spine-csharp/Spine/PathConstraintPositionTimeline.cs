namespace Spine;

public class PathConstraintPositionTimeline : CurveTimeline1
{
	private readonly int pathConstraintIndex;

	public int PathConstraintIndex => pathConstraintIndex;

	public PathConstraintPositionTimeline(int frameCount, int bezierCount, int pathConstraintIndex)
		: base(frameCount, bezierCount, 16 + "|" + pathConstraintIndex)
	{
		this.pathConstraintIndex = pathConstraintIndex;
	}

	public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction)
	{
		PathConstraint pathConstraint = skeleton.pathConstraints.Items[pathConstraintIndex];
		if (!pathConstraint.active)
		{
			return;
		}
		if (time < frames[0])
		{
			switch (blend)
			{
			case MixBlend.Setup:
				pathConstraint.position = pathConstraint.data.position;
				break;
			case MixBlend.First:
				pathConstraint.position += (pathConstraint.data.position - pathConstraint.position) * alpha;
				break;
			}
		}
		else
		{
			float curveValue = GetCurveValue(time);
			if (blend == MixBlend.Setup)
			{
				pathConstraint.position = pathConstraint.data.position + (curveValue - pathConstraint.data.position) * alpha;
			}
			else
			{
				pathConstraint.position += (curveValue - pathConstraint.position) * alpha;
			}
		}
	}
}
