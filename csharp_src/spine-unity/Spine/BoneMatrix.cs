using System;

namespace Spine;

public struct BoneMatrix
{
	public float a;

	public float b;

	public float c;

	public float d;

	public float x;

	public float y;

	public static BoneMatrix CalculateSetupWorld(BoneData boneData)
	{
		if (boneData == null)
		{
			return default(BoneMatrix);
		}
		if (boneData.Parent == null)
		{
			return GetInheritedInternal(boneData, default(BoneMatrix));
		}
		BoneMatrix parentMatrix = CalculateSetupWorld(boneData.Parent);
		return GetInheritedInternal(boneData, parentMatrix);
	}

	private static BoneMatrix GetInheritedInternal(BoneData boneData, BoneMatrix parentMatrix)
	{
		if (boneData.Parent == null)
		{
			return new BoneMatrix(boneData);
		}
		float num = parentMatrix.a;
		float num2 = parentMatrix.b;
		float num3 = parentMatrix.c;
		float num4 = parentMatrix.d;
		BoneMatrix result = new BoneMatrix
		{
			x = num * boneData.X + num2 * boneData.Y + parentMatrix.x,
			y = num3 * boneData.X + num4 * boneData.Y + parentMatrix.y
		};
		switch (boneData.TransformMode)
		{
		case TransformMode.Normal:
		{
			float degrees3 = boneData.Rotation + 90f + boneData.ShearY;
			float num22 = MathUtils.CosDeg(boneData.Rotation + boneData.ShearX) * boneData.ScaleX;
			float num23 = MathUtils.CosDeg(degrees3) * boneData.ScaleY;
			float num24 = MathUtils.SinDeg(boneData.Rotation + boneData.ShearX) * boneData.ScaleX;
			float num25 = MathUtils.SinDeg(degrees3) * boneData.ScaleY;
			result.a = num * num22 + num2 * num24;
			result.b = num * num23 + num2 * num25;
			result.c = num3 * num22 + num4 * num24;
			result.d = num3 * num23 + num4 * num25;
			break;
		}
		case TransformMode.OnlyTranslation:
		{
			float degrees4 = boneData.Rotation + 90f + boneData.ShearY;
			result.a = MathUtils.CosDeg(boneData.Rotation + boneData.ShearX) * boneData.ScaleX;
			result.b = MathUtils.CosDeg(degrees4) * boneData.ScaleY;
			result.c = MathUtils.SinDeg(boneData.Rotation + boneData.ShearX) * boneData.ScaleX;
			result.d = MathUtils.SinDeg(degrees4) * boneData.ScaleY;
			break;
		}
		case TransformMode.NoRotationOrReflection:
		{
			float num16 = num * num + num3 * num3;
			float num17;
			if (num16 > 0.0001f)
			{
				num16 = Math.Abs(num * num4 - num2 * num3) / num16;
				num2 = num3 * num16;
				num4 = num * num16;
				num17 = MathUtils.Atan2(num3, num) * (180f / MathF.PI);
			}
			else
			{
				num = 0f;
				num3 = 0f;
				num17 = 90f - MathUtils.Atan2(num4, num2) * (180f / MathF.PI);
			}
			float degrees = boneData.Rotation + boneData.ShearX - num17;
			float degrees2 = boneData.Rotation + boneData.ShearY - num17 + 90f;
			float num18 = MathUtils.CosDeg(degrees) * boneData.ScaleX;
			float num19 = MathUtils.CosDeg(degrees2) * boneData.ScaleY;
			float num20 = MathUtils.SinDeg(degrees) * boneData.ScaleX;
			float num21 = MathUtils.SinDeg(degrees2) * boneData.ScaleY;
			result.a = num * num18 - num2 * num20;
			result.b = num * num19 - num2 * num21;
			result.c = num3 * num18 + num4 * num20;
			result.d = num3 * num19 + num4 * num21;
			break;
		}
		case TransformMode.NoScale:
		case TransformMode.NoScaleOrReflection:
		{
			float num5 = MathUtils.CosDeg(boneData.Rotation);
			float num6 = MathUtils.SinDeg(boneData.Rotation);
			float num7 = num * num5 + num2 * num6;
			float num8 = num3 * num5 + num4 * num6;
			float num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
			if (num9 > 1E-05f)
			{
				num9 = 1f / num9;
			}
			num7 *= num9;
			num8 *= num9;
			num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
			float radians = MathF.PI / 2f + MathUtils.Atan2(num8, num7);
			float num10 = MathUtils.Cos(radians) * num9;
			float num11 = MathUtils.Sin(radians) * num9;
			float num12 = MathUtils.CosDeg(boneData.ShearX) * boneData.ScaleX;
			float num13 = MathUtils.CosDeg(90f + boneData.ShearY) * boneData.ScaleY;
			float num14 = MathUtils.SinDeg(boneData.ShearX) * boneData.ScaleX;
			float num15 = MathUtils.SinDeg(90f + boneData.ShearY) * boneData.ScaleY;
			if (boneData.TransformMode != TransformMode.NoScaleOrReflection && num * num4 - num2 * num3 < 0f)
			{
				num10 = 0f - num10;
				num11 = 0f - num11;
			}
			result.a = num7 * num12 + num10 * num14;
			result.b = num7 * num13 + num10 * num15;
			result.c = num8 * num12 + num11 * num14;
			result.d = num8 * num13 + num11 * num15;
			break;
		}
		}
		return result;
	}

	public BoneMatrix(BoneData boneData)
	{
		float degrees = boneData.Rotation + 90f + boneData.ShearY;
		float degrees2 = boneData.Rotation + boneData.ShearX;
		a = MathUtils.CosDeg(degrees2) * boneData.ScaleX;
		c = MathUtils.SinDeg(degrees2) * boneData.ScaleX;
		b = MathUtils.CosDeg(degrees) * boneData.ScaleY;
		d = MathUtils.SinDeg(degrees) * boneData.ScaleY;
		x = boneData.X;
		y = boneData.Y;
	}

	public BoneMatrix(Bone bone)
	{
		float degrees = bone.Rotation + 90f + bone.ShearY;
		float degrees2 = bone.Rotation + bone.ShearX;
		a = MathUtils.CosDeg(degrees2) * bone.ScaleX;
		c = MathUtils.SinDeg(degrees2) * bone.ScaleX;
		b = MathUtils.CosDeg(degrees) * bone.ScaleY;
		d = MathUtils.SinDeg(degrees) * bone.ScaleY;
		x = bone.X;
		y = bone.Y;
	}

	public BoneMatrix TransformMatrix(BoneMatrix local)
	{
		return new BoneMatrix
		{
			a = a * local.a + b * local.c,
			b = a * local.b + b * local.d,
			c = c * local.a + d * local.c,
			d = c * local.b + d * local.d,
			x = a * local.x + b * local.y + x,
			y = c * local.x + d * local.y + y
		};
	}
}
