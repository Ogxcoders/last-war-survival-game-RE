using System.Globalization;
using System.Numerics.Hashing;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Numerics;

public struct Vector4 : IEquatable<Vector4>, IFormattable
{
	public float X;

	public float Y;

	public float Z;

	public float W;

	public static Vector4 Zero => default(Vector4);

	public static Vector4 One => new Vector4(1f, 1f, 1f, 1f);

	public static Vector4 UnitX => new Vector4(1f, 0f, 0f, 0f);

	public static Vector4 UnitY => new Vector4(0f, 1f, 0f, 0f);

	public static Vector4 UnitZ => new Vector4(0f, 0f, 1f, 0f);

	public static Vector4 UnitW => new Vector4(0f, 0f, 0f, 1f);

	public override int GetHashCode()
	{
		return System.Numerics.Hashing.HashHelpers.Combine(System.Numerics.Hashing.HashHelpers.Combine(System.Numerics.Hashing.HashHelpers.Combine(X.GetHashCode(), Y.GetHashCode()), Z.GetHashCode()), W.GetHashCode());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object obj)
	{
		if (!(obj is Vector4))
		{
			return false;
		}
		return Equals((Vector4)obj);
	}

	public override string ToString()
	{
		return ToString("G", CultureInfo.CurrentCulture);
	}

	public string ToString(string format)
	{
		return ToString(format, CultureInfo.CurrentCulture);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string numberGroupSeparator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
		stringBuilder.Append('<');
		stringBuilder.Append(X.ToString(format, formatProvider));
		stringBuilder.Append(numberGroupSeparator);
		stringBuilder.Append(' ');
		stringBuilder.Append(Y.ToString(format, formatProvider));
		stringBuilder.Append(numberGroupSeparator);
		stringBuilder.Append(' ');
		stringBuilder.Append(Z.ToString(format, formatProvider));
		stringBuilder.Append(numberGroupSeparator);
		stringBuilder.Append(' ');
		stringBuilder.Append(W.ToString(format, formatProvider));
		stringBuilder.Append('>');
		return stringBuilder.ToString();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float Length()
	{
		if (Vector.IsHardwareAccelerated)
		{
			return MathF.Sqrt(Dot(this, this));
		}
		return MathF.Sqrt(X * X + Y * Y + Z * Z + W * W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float LengthSquared()
	{
		if (Vector.IsHardwareAccelerated)
		{
			return Dot(this, this);
		}
		return X * X + Y * Y + Z * Z + W * W;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Distance(Vector4 value1, Vector4 value2)
	{
		if (Vector.IsHardwareAccelerated)
		{
			Vector4 vector = value1 - value2;
			return MathF.Sqrt(Dot(vector, vector));
		}
		float num = value1.X - value2.X;
		float num2 = value1.Y - value2.Y;
		float num3 = value1.Z - value2.Z;
		float num4 = value1.W - value2.W;
		return MathF.Sqrt(num * num + num2 * num2 + num3 * num3 + num4 * num4);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float DistanceSquared(Vector4 value1, Vector4 value2)
	{
		if (Vector.IsHardwareAccelerated)
		{
			Vector4 vector = value1 - value2;
			return Dot(vector, vector);
		}
		float num = value1.X - value2.X;
		float num2 = value1.Y - value2.Y;
		float num3 = value1.Z - value2.Z;
		float num4 = value1.W - value2.W;
		return num * num + num2 * num2 + num3 * num3 + num4 * num4;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Normalize(Vector4 vector)
	{
		if (Vector.IsHardwareAccelerated)
		{
			float num = vector.Length();
			return vector / num;
		}
		float x = vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z + vector.W * vector.W;
		float num2 = 1f / MathF.Sqrt(x);
		return new Vector4(vector.X * num2, vector.Y * num2, vector.Z * num2, vector.W * num2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Clamp(Vector4 value1, Vector4 min, Vector4 max)
	{
		float x = value1.X;
		x = ((x > max.X) ? max.X : x);
		x = ((x < min.X) ? min.X : x);
		float y = value1.Y;
		y = ((y > max.Y) ? max.Y : y);
		y = ((y < min.Y) ? min.Y : y);
		float z = value1.Z;
		z = ((z > max.Z) ? max.Z : z);
		z = ((z < min.Z) ? min.Z : z);
		float w = value1.W;
		w = ((w > max.W) ? max.W : w);
		w = ((w < min.W) ? min.W : w);
		return new Vector4(x, y, z, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Lerp(Vector4 value1, Vector4 value2, float amount)
	{
		return new Vector4(value1.X + (value2.X - value1.X) * amount, value1.Y + (value2.Y - value1.Y) * amount, value1.Z + (value2.Z - value1.Z) * amount, value1.W + (value2.W - value1.W) * amount);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Transform(Vector2 position, Matrix4x4 matrix)
	{
		return new Vector4(position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M41, position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M42, position.X * matrix.M13 + position.Y * matrix.M23 + matrix.M43, position.X * matrix.M14 + position.Y * matrix.M24 + matrix.M44);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Transform(Vector3 position, Matrix4x4 matrix)
	{
		return new Vector4(position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41, position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42, position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43, position.X * matrix.M14 + position.Y * matrix.M24 + position.Z * matrix.M34 + matrix.M44);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Transform(Vector4 vector, Matrix4x4 matrix)
	{
		return new Vector4(vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + vector.W * matrix.M41, vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + vector.W * matrix.M42, vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + vector.W * matrix.M43, vector.X * matrix.M14 + vector.Y * matrix.M24 + vector.Z * matrix.M34 + vector.W * matrix.M44);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Transform(Vector2 value, Quaternion rotation)
	{
		float num = rotation.X + rotation.X;
		float num2 = rotation.Y + rotation.Y;
		float num3 = rotation.Z + rotation.Z;
		float num4 = rotation.W * num;
		float num5 = rotation.W * num2;
		float num6 = rotation.W * num3;
		float num7 = rotation.X * num;
		float num8 = rotation.X * num2;
		float num9 = rotation.X * num3;
		float num10 = rotation.Y * num2;
		float num11 = rotation.Y * num3;
		float num12 = rotation.Z * num3;
		return new Vector4(value.X * (1f - num10 - num12) + value.Y * (num8 - num6), value.X * (num8 + num6) + value.Y * (1f - num7 - num12), value.X * (num9 - num5) + value.Y * (num11 + num4), 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Transform(Vector3 value, Quaternion rotation)
	{
		float num = rotation.X + rotation.X;
		float num2 = rotation.Y + rotation.Y;
		float num3 = rotation.Z + rotation.Z;
		float num4 = rotation.W * num;
		float num5 = rotation.W * num2;
		float num6 = rotation.W * num3;
		float num7 = rotation.X * num;
		float num8 = rotation.X * num2;
		float num9 = rotation.X * num3;
		float num10 = rotation.Y * num2;
		float num11 = rotation.Y * num3;
		float num12 = rotation.Z * num3;
		return new Vector4(value.X * (1f - num10 - num12) + value.Y * (num8 - num6) + value.Z * (num9 + num5), value.X * (num8 + num6) + value.Y * (1f - num7 - num12) + value.Z * (num11 - num4), value.X * (num9 - num5) + value.Y * (num11 + num4) + value.Z * (1f - num7 - num10), 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Transform(Vector4 value, Quaternion rotation)
	{
		float num = rotation.X + rotation.X;
		float num2 = rotation.Y + rotation.Y;
		float num3 = rotation.Z + rotation.Z;
		float num4 = rotation.W * num;
		float num5 = rotation.W * num2;
		float num6 = rotation.W * num3;
		float num7 = rotation.X * num;
		float num8 = rotation.X * num2;
		float num9 = rotation.X * num3;
		float num10 = rotation.Y * num2;
		float num11 = rotation.Y * num3;
		float num12 = rotation.Z * num3;
		return new Vector4(value.X * (1f - num10 - num12) + value.Y * (num8 - num6) + value.Z * (num9 + num5), value.X * (num8 + num6) + value.Y * (1f - num7 - num12) + value.Z * (num11 - num4), value.X * (num9 - num5) + value.Y * (num11 + num4) + value.Z * (1f - num7 - num10), value.W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Add(Vector4 left, Vector4 right)
	{
		return left + right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Subtract(Vector4 left, Vector4 right)
	{
		return left - right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Multiply(Vector4 left, Vector4 right)
	{
		return left * right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Multiply(Vector4 left, float right)
	{
		return left * new Vector4(right, right, right, right);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Multiply(float left, Vector4 right)
	{
		return new Vector4(left, left, left, left) * right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Divide(Vector4 left, Vector4 right)
	{
		return left / right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Divide(Vector4 left, float divisor)
	{
		return left / divisor;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 Negate(Vector4 value)
	{
		return -value;
	}

	[System.Runtime.CompilerServices.Intrinsic]
	public Vector4(float value)
		: this(value, value, value, value)
	{
	}

	[System.Runtime.CompilerServices.Intrinsic]
	public Vector4(float x, float y, float z, float w)
	{
		W = w;
		X = x;
		Y = y;
		Z = z;
	}

	public Vector4(Vector2 value, float z, float w)
	{
		X = value.X;
		Y = value.Y;
		Z = z;
		W = w;
	}

	public Vector4(Vector3 value, float w)
	{
		X = value.X;
		Y = value.Y;
		Z = value.Z;
		W = w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void CopyTo(float[] array)
	{
		CopyTo(array, 0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public void CopyTo(float[] array, int index)
	{
		if (array == null)
		{
			throw new NullReferenceException("The method was called with a null array argument.");
		}
		if (index < 0 || index >= array.Length)
		{
			throw new ArgumentOutOfRangeException("index", global::SR.Format("Index was out of bounds:", index));
		}
		if (array.Length - index < 4)
		{
			throw new ArgumentException(global::SR.Format("Number of elements in source vector is greater than the destination array", index));
		}
		array[index] = X;
		array[index + 1] = Y;
		array[index + 2] = Z;
		array[index + 3] = W;
	}

	[System.Runtime.CompilerServices.Intrinsic]
	public bool Equals(Vector4 other)
	{
		if (X == other.X && Y == other.Y && Z == other.Z)
		{
			return W == other.W;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static float Dot(Vector4 vector1, Vector4 vector2)
	{
		return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z + vector1.W * vector2.W;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector4 Min(Vector4 value1, Vector4 value2)
	{
		return new Vector4((value1.X < value2.X) ? value1.X : value2.X, (value1.Y < value2.Y) ? value1.Y : value2.Y, (value1.Z < value2.Z) ? value1.Z : value2.Z, (value1.W < value2.W) ? value1.W : value2.W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector4 Max(Vector4 value1, Vector4 value2)
	{
		return new Vector4((value1.X > value2.X) ? value1.X : value2.X, (value1.Y > value2.Y) ? value1.Y : value2.Y, (value1.Z > value2.Z) ? value1.Z : value2.Z, (value1.W > value2.W) ? value1.W : value2.W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector4 Abs(Vector4 value)
	{
		return new Vector4(MathF.Abs(value.X), MathF.Abs(value.Y), MathF.Abs(value.Z), MathF.Abs(value.W));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector4 SquareRoot(Vector4 value)
	{
		return new Vector4(MathF.Sqrt(value.X), MathF.Sqrt(value.Y), MathF.Sqrt(value.Z), MathF.Sqrt(value.W));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector4 operator +(Vector4 left, Vector4 right)
	{
		return new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector4 operator -(Vector4 left, Vector4 right)
	{
		return new Vector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector4 operator *(Vector4 left, Vector4 right)
	{
		return new Vector4(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector4 operator *(Vector4 left, float right)
	{
		return left * new Vector4(right);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector4 operator *(float left, Vector4 right)
	{
		return new Vector4(left) * right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector4 operator /(Vector4 left, Vector4 right)
	{
		return new Vector4(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 operator /(Vector4 value1, float value2)
	{
		return value1 / new Vector4(value2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector4 operator -(Vector4 value)
	{
		return Zero - value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static bool operator ==(Vector4 left, Vector4 right)
	{
		return left.Equals(right);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(Vector4 left, Vector4 right)
	{
		return !(left == right);
	}
}
