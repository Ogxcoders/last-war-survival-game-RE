namespace Spine;

public class PathConstraintSpacingTimeline : CurveTimeline1
{
	private readonly int pathConstraintIndex;

	public int PathConstraintIndex => pathConstraintIndex;

	public PathConstraintSpacingTimeline(int frameCount, int bezierCount, int pathConstraintIndex)
		: base(frameCount, bezierCount, 17 + "|" + pathConstraintIndex)
	{
		this.pathConstraintIndex = pathConstraintIndex;
	}

	public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> events, float alpha, MixBlend blend, MixDirection direction)
	{
		PathConstraint pathConstraint = skeleton.pathConstraints.Items[pathConstraintIndex];
		if (!pathConstraint.active)
		{
			return;
		}
		float[] array = frames;
		if (time < array[0])
		{
			switch (blend)
			{
			case MixBlend.Setup:
				pathConstraint.spacing = pathConstraint.data.spacing;
				break;
			case MixBlend.First:
				pathConstraint.spacing += (pathConstraint.data.spacing - pathConstraint.spacing) * alpha;
				break;
			}
		}
		else
		{
			float curveValue = GetCurveValue(time);
			if (blend == MixBlend.Setup)
			{
				pathConstraint.spacing = pathConstraint.data.spacing + (curveValue - pathConstraint.data.spacing) * alpha;
			}
			else
			{
				pathConstraint.spacing += (curveValue - pathConstraint.spacing) * alpha;
			}
		}
	}
}
