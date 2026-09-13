namespace Spine;

public class RotateTimeline : CurveTimeline1, IBoneTimeline
{
	private readonly int boneIndex;

	public int BoneIndex => boneIndex;

	public RotateTimeline(int frameCount, int bezierCount, int boneIndex)
		: base(frameCount, bezierCount, 0 + "|" + boneIndex)
	{
		this.boneIndex = boneIndex;
	}

	public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction)
	{
		Bone bone = skeleton.bones.Items[boneIndex];
		if (!bone.active)
		{
			return;
		}
		if (time < frames[0])
		{
			switch (blend)
			{
			case MixBlend.Setup:
				bone.rotation = bone.data.rotation;
				break;
			case MixBlend.First:
				bone.rotation += (bone.data.rotation - bone.rotation) * alpha;
				break;
			}
			return;
		}
		float num = GetCurveValue(time);
		switch (blend)
		{
		default:
			return;
		case MixBlend.Setup:
			bone.rotation = bone.data.rotation + num * alpha;
			return;
		case MixBlend.First:
		case MixBlend.Replace:
			num += bone.data.rotation - bone.rotation;
			break;
		case MixBlend.Add:
			break;
		}
		bone.rotation += num * alpha;
	}
}
