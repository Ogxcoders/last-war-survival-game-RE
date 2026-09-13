namespace Spine;

public class ShearYTimeline : CurveTimeline1, IBoneTimeline
{
	private readonly int boneIndex;

	public int BoneIndex => boneIndex;

	public ShearYTimeline(int frameCount, int bezierCount, int boneIndex)
		: base(frameCount, bezierCount, 6 + "|" + boneIndex)
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
				bone.shearY = bone.data.shearY;
				break;
			case MixBlend.First:
				bone.shearY += (bone.data.shearY - bone.shearY) * alpha;
				break;
			}
			return;
		}
		float curveValue = GetCurveValue(time);
		switch (blend)
		{
		case MixBlend.Setup:
			bone.shearY = bone.data.shearY + curveValue * alpha;
			break;
		case MixBlend.First:
		case MixBlend.Replace:
			bone.shearY += (bone.data.shearY + curveValue - bone.shearY) * alpha;
			break;
		case MixBlend.Add:
			bone.shearY += curveValue * alpha;
			break;
		}
	}
}
