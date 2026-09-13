using System;

namespace Spine;

public class Bone : IUpdatable
{
	public static bool yDown;

	internal BoneData data;

	internal Skeleton skeleton;

	internal Bone parent;

	internal ExposedList<Bone> children = new ExposedList<Bone>();

	internal float x;

	internal float y;

	internal float rotation;

	internal float scaleX;

	internal float scaleY;

	internal float shearX;

	internal float shearY;

	internal float ax;

	internal float ay;

	internal float arotation;

	internal float ascaleX;

	internal float ascaleY;

	internal float ashearX;

	internal float ashearY;

	internal float a;

	internal float b;

	internal float worldX;

	internal float c;

	internal float d;

	internal float worldY;

	internal bool sorted;

	internal bool active;

	public BoneData Data => data;

	public Skeleton Skeleton => skeleton;

	public Bone Parent => parent;

	public ExposedList<Bone> Children => children;

	public bool Active => active;

	public float X
	{
		get
		{
			return x;
		}
		set
		{
			x = value;
		}
	}

	public float Y
	{
		get
		{
			return y;
		}
		set
		{
			y = value;
		}
	}

	public float Rotation
	{
		get
		{
			return rotation;
		}
		set
		{
			rotation = value;
		}
	}

	public float ScaleX
	{
		get
		{
			return scaleX;
		}
		set
		{
			scaleX = value;
		}
	}

	public float ScaleY
	{
		get
		{
			return scaleY;
		}
		set
		{
			scaleY = value;
		}
	}

	public float ShearX
	{
		get
		{
			return shearX;
		}
		set
		{
			shearX = value;
		}
	}

	public float ShearY
	{
		get
		{
			return shearY;
		}
		set
		{
			shearY = value;
		}
	}

	public float AppliedRotation
	{
		get
		{
			return arotation;
		}
		set
		{
			arotation = value;
		}
	}

	public float AX
	{
		get
		{
			return ax;
		}
		set
		{
			ax = value;
		}
	}

	public float AY
	{
		get
		{
			return ay;
		}
		set
		{
			ay = value;
		}
	}

	public float AScaleX
	{
		get
		{
			return ascaleX;
		}
		set
		{
			ascaleX = value;
		}
	}

	public float AScaleY
	{
		get
		{
			return ascaleY;
		}
		set
		{
			ascaleY = value;
		}
	}

	public float AShearX
	{
		get
		{
			return ashearX;
		}
		set
		{
			ashearX = value;
		}
	}

	public float AShearY
	{
		get
		{
			return ashearY;
		}
		set
		{
			ashearY = value;
		}
	}

	public float A
	{
		get
		{
			return a;
		}
		set
		{
			a = value;
		}
	}

	public float B
	{
		get
		{
			return b;
		}
		set
		{
			b = value;
		}
	}

	public float C
	{
		get
		{
			return c;
		}
		set
		{
			c = value;
		}
	}

	public float D
	{
		get
		{
			return d;
		}
		set
		{
			d = value;
		}
	}

	public float WorldX
	{
		get
		{
			return worldX;
		}
		set
		{
			worldX = value;
		}
	}

	public float WorldY
	{
		get
		{
			return worldY;
		}
		set
		{
			worldY = value;
		}
	}

	public float WorldRotationX => MathUtils.Atan2(c, a) * (180f / MathF.PI);

	public float WorldRotationY => MathUtils.Atan2(d, b) * (180f / MathF.PI);

	public float WorldScaleX => (float)Math.Sqrt(a * a + c * c);

	public float WorldScaleY => (float)Math.Sqrt(b * b + d * d);

	public float WorldToLocalRotationX
	{
		get
		{
			Bone bone = parent;
			if (bone == null)
			{
				return arotation;
			}
			float num = bone.a;
			float num2 = bone.b;
			float num3 = bone.c;
			float num4 = bone.d;
			float num5 = a;
			float num6 = c;
			return MathUtils.Atan2(num * num6 - num3 * num5, num4 * num5 - num2 * num6) * (180f / MathF.PI);
		}
	}

	public float WorldToLocalRotationY
	{
		get
		{
			Bone bone = parent;
			if (bone == null)
			{
				return arotation;
			}
			float num = bone.a;
			float num2 = bone.b;
			float num3 = bone.c;
			float num4 = bone.d;
			float num5 = b;
			float num6 = d;
			return MathUtils.Atan2(num * num6 - num3 * num5, num4 * num5 - num2 * num6) * (180f / MathF.PI);
		}
	}

	public Bone(BoneData data, Skeleton skeleton, Bone parent)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data", "data cannot be null.");
		}
		if (skeleton == null)
		{
			throw new ArgumentNullException("skeleton", "skeleton cannot be null.");
		}
		this.data = data;
		this.skeleton = skeleton;
		this.parent = parent;
		SetToSetupPose();
	}

	public void Update()
	{
		UpdateWorldTransform(ax, ay, arotation, ascaleX, ascaleY, ashearX, ashearY);
	}

	public void UpdateWorldTransform()
	{
		UpdateWorldTransform(x, y, rotation, scaleX, scaleY, shearX, shearY);
	}

	public void UpdateWorldTransform(float x, float y, float rotation, float scaleX, float scaleY, float shearX, float shearY)
	{
		ax = x;
		ay = y;
		arotation = rotation;
		ascaleX = scaleX;
		ascaleY = scaleY;
		ashearX = shearX;
		ashearY = shearY;
		Bone bone = parent;
		if (bone == null)
		{
			float degrees = rotation + 90f + shearY;
			float num = skeleton.ScaleX;
			float num2 = skeleton.ScaleY;
			a = MathUtils.CosDeg(rotation + shearX) * scaleX * num;
			b = MathUtils.CosDeg(degrees) * scaleY * num;
			c = MathUtils.SinDeg(rotation + shearX) * scaleX * num2;
			d = MathUtils.SinDeg(degrees) * scaleY * num2;
			worldX = x * num + skeleton.x;
			worldY = y * num2 + skeleton.y;
			return;
		}
		float num3 = bone.a;
		float num4 = bone.b;
		float num5 = bone.c;
		float num6 = bone.d;
		worldX = num3 * x + num4 * y + bone.worldX;
		worldY = num5 * x + num6 * y + bone.worldY;
		switch (data.transformMode)
		{
		case TransformMode.Normal:
		{
			float degrees4 = rotation + 90f + shearY;
			float num24 = MathUtils.CosDeg(rotation + shearX) * scaleX;
			float num25 = MathUtils.CosDeg(degrees4) * scaleY;
			float num26 = MathUtils.SinDeg(rotation + shearX) * scaleX;
			float num27 = MathUtils.SinDeg(degrees4) * scaleY;
			a = num3 * num24 + num4 * num26;
			b = num3 * num25 + num4 * num27;
			c = num5 * num24 + num6 * num26;
			d = num5 * num25 + num6 * num27;
			return;
		}
		case TransformMode.OnlyTranslation:
		{
			float degrees5 = rotation + 90f + shearY;
			a = MathUtils.CosDeg(rotation + shearX) * scaleX;
			b = MathUtils.CosDeg(degrees5) * scaleY;
			c = MathUtils.SinDeg(rotation + shearX) * scaleX;
			d = MathUtils.SinDeg(degrees5) * scaleY;
			break;
		}
		case TransformMode.NoRotationOrReflection:
		{
			float num18 = num3 * num3 + num5 * num5;
			float num19;
			if (num18 > 0.0001f)
			{
				num18 = Math.Abs(num3 * num6 - num4 * num5) / num18;
				num3 /= skeleton.ScaleX;
				num5 /= skeleton.ScaleY;
				num4 = num5 * num18;
				num6 = num3 * num18;
				num19 = MathUtils.Atan2(num5, num3) * (180f / MathF.PI);
			}
			else
			{
				num3 = 0f;
				num5 = 0f;
				num19 = 90f - MathUtils.Atan2(num6, num4) * (180f / MathF.PI);
			}
			float degrees2 = rotation + shearX - num19;
			float degrees3 = rotation + shearY - num19 + 90f;
			float num20 = MathUtils.CosDeg(degrees2) * scaleX;
			float num21 = MathUtils.CosDeg(degrees3) * scaleY;
			float num22 = MathUtils.SinDeg(degrees2) * scaleX;
			float num23 = MathUtils.SinDeg(degrees3) * scaleY;
			a = num3 * num20 - num4 * num22;
			b = num3 * num21 - num4 * num23;
			c = num5 * num20 + num6 * num22;
			d = num5 * num21 + num6 * num23;
			break;
		}
		case TransformMode.NoScale:
		case TransformMode.NoScaleOrReflection:
		{
			float num7 = MathUtils.CosDeg(rotation);
			float num8 = MathUtils.SinDeg(rotation);
			float num9 = (num3 * num7 + num4 * num8) / skeleton.ScaleX;
			float num10 = (num5 * num7 + num6 * num8) / skeleton.ScaleY;
			float num11 = (float)Math.Sqrt(num9 * num9 + num10 * num10);
			if (num11 > 1E-05f)
			{
				num11 = 1f / num11;
			}
			num9 *= num11;
			num10 *= num11;
			num11 = (float)Math.Sqrt(num9 * num9 + num10 * num10);
			if (data.transformMode == TransformMode.NoScale && num3 * num6 - num4 * num5 < 0f != (skeleton.ScaleX < 0f != skeleton.ScaleY < 0f))
			{
				num11 = 0f - num11;
			}
			float radians = MathF.PI / 2f + MathUtils.Atan2(num10, num9);
			float num12 = MathUtils.Cos(radians) * num11;
			float num13 = MathUtils.Sin(radians) * num11;
			float num14 = MathUtils.CosDeg(shearX) * scaleX;
			float num15 = MathUtils.CosDeg(90f + shearY) * scaleY;
			float num16 = MathUtils.SinDeg(shearX) * scaleX;
			float num17 = MathUtils.SinDeg(90f + shearY) * scaleY;
			a = num9 * num14 + num12 * num16;
			b = num9 * num15 + num12 * num17;
			c = num10 * num14 + num13 * num16;
			d = num10 * num15 + num13 * num17;
			break;
		}
		}
		a *= skeleton.ScaleX;
		b *= skeleton.ScaleX;
		c *= skeleton.ScaleY;
		d *= skeleton.ScaleY;
	}

	public void SetToSetupPose()
	{
		BoneData boneData = data;
		x = boneData.x;
		y = boneData.y;
		rotation = boneData.rotation;
		scaleX = boneData.scaleX;
		scaleY = boneData.scaleY;
		shearX = boneData.shearX;
		shearY = boneData.shearY;
	}

	public void UpdateAppliedTransform()
	{
		Bone bone = parent;
		if (bone == null)
		{
			ax = worldX - skeleton.x;
			ay = worldY - skeleton.y;
			arotation = MathUtils.Atan2(c, a) * (180f / MathF.PI);
			ascaleX = (float)Math.Sqrt(a * a + c * c);
			ascaleY = (float)Math.Sqrt(b * b + d * d);
			ashearX = 0f;
			ashearY = MathUtils.Atan2(a * b + c * d, a * d - b * c) * (180f / MathF.PI);
			return;
		}
		float num = bone.a;
		float num2 = bone.b;
		float num3 = bone.c;
		float num4 = bone.d;
		float num5 = 1f / (num * num4 - num2 * num3);
		float num6 = worldX - bone.worldX;
		float num7 = worldY - bone.worldY;
		ax = num6 * num4 * num5 - num7 * num2 * num5;
		ay = num7 * num * num5 - num6 * num3 * num5;
		float num8 = num5 * num4;
		float num9 = num5 * num;
		float num10 = num5 * num2;
		float num11 = num5 * num3;
		float num12 = num8 * a - num10 * c;
		float num13 = num8 * b - num10 * d;
		float num14 = num9 * c - num11 * a;
		float num15 = num9 * d - num11 * b;
		ashearX = 0f;
		ascaleX = (float)Math.Sqrt(num12 * num12 + num14 * num14);
		if (ascaleX > 0.0001f)
		{
			float num16 = num12 * num15 - num13 * num14;
			ascaleY = num16 / ascaleX;
			ashearY = MathUtils.Atan2(num12 * num13 + num14 * num15, num16) * (180f / MathF.PI);
			arotation = MathUtils.Atan2(num14, num12) * (180f / MathF.PI);
		}
		else
		{
			ascaleX = 0f;
			ascaleY = (float)Math.Sqrt(num13 * num13 + num15 * num15);
			ashearY = 0f;
			arotation = 90f - MathUtils.Atan2(num15, num13) * (180f / MathF.PI);
		}
	}

	public void WorldToLocal(float worldX, float worldY, out float localX, out float localY)
	{
		float num = a;
		float num2 = b;
		float num3 = c;
		float num4 = d;
		float num5 = num * num4 - num2 * num3;
		float num6 = worldX - this.worldX;
		float num7 = worldY - this.worldY;
		localX = (num6 * num4 - num7 * num2) / num5;
		localY = (num7 * num - num6 * num3) / num5;
	}

	public void LocalToWorld(float localX, float localY, out float worldX, out float worldY)
	{
		worldX = localX * a + localY * b + this.worldX;
		worldY = localX * c + localY * d + this.worldY;
	}

	public float WorldToLocalRotation(float worldRotation)
	{
		float num = MathUtils.SinDeg(worldRotation);
		float num2 = MathUtils.CosDeg(worldRotation);
		return MathUtils.Atan2(a * num - c * num2, d * num2 - b * num) * (180f / MathF.PI) + rotation - shearX;
	}

	public float LocalToWorldRotation(float localRotation)
	{
		localRotation -= rotation - shearX;
		float num = MathUtils.SinDeg(localRotation);
		float num2 = MathUtils.CosDeg(localRotation);
		return MathUtils.Atan2(num2 * c + num * d, num2 * a + num * b) * (180f / MathF.PI);
	}

	public void RotateWorld(float degrees)
	{
		float num = a;
		float num2 = b;
		float num3 = c;
		float num4 = d;
		float num5 = MathUtils.CosDeg(degrees);
		float num6 = MathUtils.SinDeg(degrees);
		a = num5 * num - num6 * num3;
		b = num5 * num2 - num6 * num4;
		c = num6 * num + num5 * num3;
		d = num6 * num2 + num5 * num4;
	}

	public override string ToString()
	{
		return data.name;
	}
}
