namespace Spine;

public class TransformConstraintTimeline : CurveTimeline
{
	public const int ENTRIES = 7;

	private const int ROTATE = 1;

	private const int X = 2;

	private const int Y = 3;

	private const int SCALEX = 4;

	private const int SCALEY = 5;

	private const int SHEARY = 6;

	private readonly int transformConstraintIndex;

	public override int FrameEntries => 7;

	public int TransformConstraintIndex => transformConstraintIndex;

	public TransformConstraintTimeline(int frameCount, int bezierCount, int transformConstraintIndex)
		: base(frameCount, bezierCount, 15 + "|" + transformConstraintIndex)
	{
		this.transformConstraintIndex = transformConstraintIndex;
	}

	public void SetFrame(int frame, float time, float mixRotate, float mixX, float mixY, float mixScaleX, float mixScaleY, float mixShearY)
	{
		frame *= 7;
		frames[frame] = time;
		frames[frame + 1] = mixRotate;
		frames[frame + 2] = mixX;
		frames[frame + 3] = mixY;
		frames[frame + 4] = mixScaleX;
		frames[frame + 5] = mixScaleY;
		frames[frame + 6] = mixShearY;
	}

	public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction)
	{
		TransformConstraint transformConstraint = skeleton.transformConstraints.Items[transformConstraintIndex];
		if (!transformConstraint.active)
		{
			return;
		}
		float[] array = frames;
		if (time < array[0])
		{
			TransformConstraintData data = transformConstraint.data;
			switch (blend)
			{
			case MixBlend.Setup:
				transformConstraint.mixRotate = data.mixRotate;
				transformConstraint.mixX = data.mixX;
				transformConstraint.mixY = data.mixY;
				transformConstraint.mixScaleX = data.mixScaleX;
				transformConstraint.mixScaleY = data.mixScaleY;
				transformConstraint.mixShearY = data.mixShearY;
				break;
			case MixBlend.First:
				transformConstraint.mixRotate += (data.mixRotate - transformConstraint.mixRotate) * alpha;
				transformConstraint.mixX += (data.mixX - transformConstraint.mixX) * alpha;
				transformConstraint.mixY += (data.mixY - transformConstraint.mixY) * alpha;
				transformConstraint.mixScaleX += (data.mixScaleX - transformConstraint.mixScaleX) * alpha;
				transformConstraint.mixScaleY += (data.mixScaleY - transformConstraint.mixScaleY) * alpha;
				transformConstraint.mixShearY += (data.mixShearY - transformConstraint.mixShearY) * alpha;
				break;
			}
			return;
		}
		int num = Timeline.Search(array, time, 7);
		int num2 = (int)curves[num / 7];
		float num3;
		float num4;
		float num5;
		float num6;
		float num7;
		float num8;
		switch (num2)
		{
		case 0:
		{
			float num9 = array[num];
			num3 = array[num + 1];
			num4 = array[num + 2];
			num5 = array[num + 3];
			num6 = array[num + 4];
			num7 = array[num + 5];
			num8 = array[num + 6];
			float num10 = (time - num9) / (array[num + 7] - num9);
			num3 += (array[num + 7 + 1] - num3) * num10;
			num4 += (array[num + 7 + 2] - num4) * num10;
			num5 += (array[num + 7 + 3] - num5) * num10;
			num6 += (array[num + 7 + 4] - num6) * num10;
			num7 += (array[num + 7 + 5] - num7) * num10;
			num8 += (array[num + 7 + 6] - num8) * num10;
			break;
		}
		case 1:
			num3 = array[num + 1];
			num4 = array[num + 2];
			num5 = array[num + 3];
			num6 = array[num + 4];
			num7 = array[num + 5];
			num8 = array[num + 6];
			break;
		default:
			num3 = GetBezierValue(time, num, 1, num2 - 2);
			num4 = GetBezierValue(time, num, 2, num2 + 18 - 2);
			num5 = GetBezierValue(time, num, 3, num2 + 36 - 2);
			num6 = GetBezierValue(time, num, 4, num2 + 54 - 2);
			num7 = GetBezierValue(time, num, 5, num2 + 72 - 2);
			num8 = GetBezierValue(time, num, 6, num2 + 90 - 2);
			break;
		}
		if (blend == MixBlend.Setup)
		{
			TransformConstraintData data2 = transformConstraint.data;
			transformConstraint.mixRotate = data2.mixRotate + (num3 - data2.mixRotate) * alpha;
			transformConstraint.mixX = data2.mixX + (num4 - data2.mixX) * alpha;
			transformConstraint.mixY = data2.mixY + (num5 - data2.mixY) * alpha;
			transformConstraint.mixScaleX = data2.mixScaleX + (num6 - data2.mixScaleX) * alpha;
			transformConstraint.mixScaleY = data2.mixScaleY + (num7 - data2.mixScaleY) * alpha;
			transformConstraint.mixShearY = data2.mixShearY + (num8 - data2.mixShearY) * alpha;
		}
		else
		{
			transformConstraint.mixRotate += (num3 - transformConstraint.mixRotate) * alpha;
			transformConstraint.mixX += (num4 - transformConstraint.mixX) * alpha;
			transformConstraint.mixY += (num5 - transformConstraint.mixY) * alpha;
			transformConstraint.mixScaleX += (num6 - transformConstraint.mixScaleX) * alpha;
			transformConstraint.mixScaleY += (num7 - transformConstraint.mixScaleY) * alpha;
			transformConstraint.mixShearY += (num8 - transformConstraint.mixShearY) * alpha;
		}
	}
}
