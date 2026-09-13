using System;

namespace Spine;

public class ScaleXTimeline : CurveTimeline1, IBoneTimeline
{
	private readonly int boneIndex;

	public int BoneIndex => boneIndex;

	public ScaleXTimeline(int frameCount, int bezierCount, int boneIndex)
		: base(frameCount, bezierCount, 3 + "|" + boneIndex)
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
				bone.scaleX = bone.data.scaleX;
				break;
			case MixBlend.First:
				bone.scaleX += (bone.data.scaleX - bone.scaleX) * alpha;
				break;
			}
			return;
		}
		float num = GetCurveValue(time) * bone.data.scaleX;
		if (alpha == 1f)
		{
			if (blend == MixBlend.Add)
			{
				bone.scaleX += num - bone.data.scaleX;
			}
			else
			{
				bone.scaleX = num;
			}
		}
		else if (direction == MixDirection.Out)
		{
			switch (blend)
			{
			case MixBlend.Setup:
			{
				float scaleX = bone.data.scaleX;
				bone.scaleX = scaleX + (Math.Abs(num) * (float)Math.Sign(scaleX) - scaleX) * alpha;
				break;
			}
			case MixBlend.First:
			case MixBlend.Replace:
			{
				float scaleX = bone.scaleX;
				bone.scaleX = scaleX + (Math.Abs(num) * (float)Math.Sign(scaleX) - scaleX) * alpha;
				break;
			}
			case MixBlend.Add:
				bone.scaleX = (num - bone.data.scaleX) * alpha;
				break;
			}
		}
		else
		{
			switch (blend)
			{
			case MixBlend.Setup:
			{
				float scaleX = Math.Abs(bone.data.scaleX) * (float)Math.Sign(num);
				bone.scaleX = scaleX + (num - scaleX) * alpha;
				break;
			}
			case MixBlend.First:
			case MixBlend.Replace:
			{
				float scaleX = Math.Abs(bone.scaleX) * (float)Math.Sign(num);
				bone.scaleX = scaleX + (num - scaleX) * alpha;
				break;
			}
			case MixBlend.Add:
				bone.scaleX += (num - bone.data.scaleX) * alpha;
				break;
			}
		}
	}
}
