namespace Spine;

public abstract class CurveTimeline1 : CurveTimeline
{
	public const int ENTRIES = 2;

	internal const int VALUE = 1;

	public override int FrameEntries => 2;

	public CurveTimeline1(int frameCount, int bezierCount, string propertyId)
		: base(frameCount, bezierCount, propertyId)
	{
	}

	public void SetFrame(int frame, float time, float value)
	{
		frame <<= 1;
		frames[frame] = time;
		frames[frame + 1] = value;
	}

	public float GetCurveValue(float time)
	{
		float[] array = frames;
		int num = array.Length - 2;
		for (int i = 2; i <= num; i += 2)
		{
			if (array[i] > time)
			{
				num = i - 2;
				break;
			}
		}
		int num2 = (int)curves[num >> 1];
		switch (num2)
		{
		case 0:
		{
			float num3 = array[num];
			float num4 = array[num + 1];
			return num4 + (time - num3) / (array[num + 2] - num3) * (array[num + 2 + 1] - num4);
		}
		case 1:
			return array[num + 1];
		default:
			return GetBezierValue(time, num, 1, num2 - 2);
		}
	}
}
