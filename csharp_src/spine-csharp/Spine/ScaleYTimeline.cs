using System;

namespace Spine;

public class ScaleYTimeline : CurveTimeline1, IBoneTimeline
{
	private readonly int boneIndex;

	public int BoneIndex => boneIndex;

	public ScaleYTimeline(int frameCount, int bezierCount, int boneIndex)
		: base(frameCount, bezierCount, 4 + "|" + boneIndex)
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
				bone.scaleY = bone.data.scaleY;
				break;
			case MixBlend.First:
				bone.scaleY += (bone.data.scaleY - bone.scaleY) * alpha;
				break;
			}
			return;
		}
		float num = GetCurveValue(time) * bone.data.scaleY;
		if (alpha == 1f)
		{
			if (blend == MixBlend.Add)
			{
				bone.scaleY += num - bone.data.scaleY;
			}
			else
			{
				bone.scaleY = num;
			}
		}
		else if (direction == MixDirection.Out)
		{
			switch (blend)
			{
			case MixBlend.Setup:
			{
				float scaleY = bone.data.scaleY;
				bone.scaleY = scaleY + (Math.Abs(num) * (float)Math.Sign(scaleY) - scaleY) * alpha;
				break;
			}
			case MixBlend.First:
			case MixBlend.Replace:
			{
				float scaleY = bone.scaleY;
				bone.scaleY = scaleY + (Math.Abs(num) * (float)Math.Sign(scaleY) - scaleY) * alpha;
				break;
			}
			case MixBlend.Add:
				bone.scaleY = (num - bone.data.scaleY) * alpha;
				break;
			}
		}
		else
		{
			switch (blend)
			{
			case MixBlend.Setup:
			{
				float scaleY = Math.Abs(bone.data.scaleY) * (float)Math.Sign(num);
				bone.scaleY = scaleY + (num - scaleY) * alpha;
				break;
			}
			case MixBlend.First:
			case MixBlend.Replace:
			{
				float scaleY = Math.Abs(bone.scaleY) * (float)Math.Sign(num);
				bone.scaleY = scaleY + (num - scaleY) * alpha;
				break;
			}
			case MixBlend.Add:
				bone.scaleY += (num - bone.data.scaleY) * alpha;
				break;
			}
		}
	}
}
