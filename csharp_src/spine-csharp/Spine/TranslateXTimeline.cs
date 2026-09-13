namespace Spine;

public class TranslateXTimeline : CurveTimeline1, IBoneTimeline
{
	private readonly int boneIndex;

	public int BoneIndex => boneIndex;

	public TranslateXTimeline(int frameCount, int bezierCount, int boneIndex)
		: base(frameCount, bezierCount, 1 + "|" + boneIndex)
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
		float[] array = frames;
		if (time < array[0])
		{
			switch (blend)
			{
			case MixBlend.Setup:
				bone.x = bone.data.x;
				break;
			case MixBlend.First:
				bone.x += (bone.data.x - bone.x) * alpha;
				break;
			}
			return;
		}
		float curveValue = GetCurveValue(time);
		switch (blend)
		{
		case MixBlend.Setup:
			bone.x = bone.data.x + curveValue * alpha;
			break;
		case MixBlend.First:
		case MixBlend.Replace:
			bone.x += (bone.data.x + curveValue - bone.x) * alpha;
			break;
		case MixBlend.Add:
			bone.x += curveValue * alpha;
			break;
		}
	}
}
