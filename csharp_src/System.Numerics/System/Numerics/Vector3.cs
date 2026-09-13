using System.Globalization;
using System.Numerics.Hashing;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Numerics;

public struct Vector3 : IEquatable<Vector3>, IFormattable
{
	public float X;

	public float Y;

	public float Z;

	public static Vector3 Zero => default(Vector3);

	public static Vector3 One => new Vector3(1f, 1f, 1f);

	public static Vector3 UnitX => new Vector3(1f, 0f, 0f);

	public static Vector3 UnitY => new Vector3(0f, 1f, 0f);

	public static Vector3 UnitZ => new Vector3(0f, 0f, 1f);

	public override int GetHashCode()
	{
		return System.Numerics.Hashing.HashHelpers.Combine(System.Numerics.Hashing.HashHelpers.Combine(X.GetHashCode(), Y.GetHashCode()), Z.GetHashCode());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object obj)
	{
		if (!(obj is Vector3))
		{
			return false;
		}
		return Equals((Vector3)obj);
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
		stringBuilder.Append(((IFormattable)X).ToString(format, formatProvider));
		stringBuilder.Append(numberGroupSeparator);
		stringBuilder.Append(' ');
		stringBuilder.Append(((IFormattable)Y).ToString(format, formatProvider));
		stringBuilder.Append(numberGroupSeparator);
		stringBuilder.Append(' ');
		stringBuilder.Append(((IFormattable)Z).ToString(format, formatProvider));
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
		return MathF.Sqrt(X * X + Y * Y + Z * Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float LengthSquared()
	{
		if (Vector.IsHardwareAccelerated)
		{
			return Dot(this, this);
		}
		return X * X + Y * Y + Z * Z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Distance(Vector3 value1, Vector3 value2)
	{
		if (Vector.IsHardwareAccelerated)
		{
			Vector3 vector = value1 - value2;
			return MathF.Sqrt(Dot(vector, vector));
		}
		float num = value1.X - value2.X;
		float num2 = value1.Y - value2.Y;
		float num3 = value1.Z - value2.Z;
		return MathF.Sqrt(num * num + num2 * num2 + num3 * num3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float DistanceSquared(Vector3 value1, Vector3 value2)
	{
		if (Vector.IsHardwareAccelerated)
		{
			Vector3 vector = value1 - value2;
			return Dot(vector, vector);
		}
		float num = value1.X - value2.X;
		float num2 = value1.Y - value2.Y;
		float num3 = value1.Z - value2.Z;
		return num * num + num2 * num2 + num3 * num3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Normalize(Vector3 value)
	{
		if (Vector.IsHardwareAccelerated)
		{
			float num = value.Length();
			return value / num;
		}
		float num2 = MathF.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z);
		return new Vector3(value.X / num2, value.Y / num2, value.Z / num2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
	{
		return new Vector3(vector1.Y * vector2.Z - vector1.Z * vector2.Y, vector1.Z * vector2.X - vector1.X * vector2.Z, vector1.X * vector2.Y - vector1.Y * vector2.X);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Reflect(Vector3 vector, Vector3 normal)
	{
		if (Vector.IsHardwareAccelerated)
		{
			float num = Dot(vector, normal);
			Vector3 vector2 = normal * num * 2f;
			return vector - vector2;
		}
		float num2 = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
		float num3 = normal.X * num2 * 2f;
		float num4 = normal.Y * num2 * 2f;
		float num5 = normal.Z * num2 * 2f;
		return new Vector3(vector.X - num3, vector.Y - num4, vector.Z - num5);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Clamp(Vector3 value1, Vector3 min, Vector3 max)
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
		return new Vector3(x, y, z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
	{
		if (Vector.IsHardwareAccelerated)
		{
			Vector3 vector = value1 * (1f - amount);
			Vector3 vector2 = value2 * amount;
			return vector + vector2;
		}
		return new Vector3(value1.X + (value2.X - value1.X) * amount, value1.Y + (value2.Y - value1.Y) * amount, value1.Z + (value2.Z - value1.Z) * amount);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Transform(Vector3 position, Matrix4x4 matrix)
	{
		return new Vector3(position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41, position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42, position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 TransformNormal(Vector3 normal, Matrix4x4 matrix)
	{
		return new Vector3(normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31, normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32, normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Transform(Vector3 value, Quaternion rotation)
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
		return new Vector3(value.X * (1f - num10 - num12) + value.Y * (num8 - num6) + value.Z * (num9 + num5), value.X * (num8 + num6) + value.Y * (1f - num7 - num12) + value.Z * (num11 - num4), value.X * (num9 - num5) + value.Y * (num11 + num4) + value.Z * (1f - num7 - num10));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Add(Vector3 left, Vector3 right)
	{
		return left + right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Subtract(Vector3 left, Vector3 right)
	{
		return left - right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Multiply(Vector3 left, Vector3 right)
	{
		return left * right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Multiply(Vector3 left, float right)
	{
		return left * right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Multiply(float left, Vector3 right)
	{
		return left * right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Divide(Vector3 left, Vector3 right)
	{
		return left / right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Divide(Vector3 left, float divisor)
	{
		return left / divisor;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Negate(Vector3 value)
	{
		return -value;
	}

	[System.Runtime.CompilerServices.Intrinsic]
	public Vector3(float value)
		: this(value, value, value)
	{
	}

	public Vector3(Vector2 value, float z)
		: this(value.X, value.Y, z)
	{
	}

	[System.Runtime.CompilerServices.Intrinsic]
	public Vector3(float x, float y, float z)
	{
		X = x;
		Y = y;
		Z = z;
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
		if (array.Length - index < 3)
		{
			throw new ArgumentException(global::SR.Format("Number of elements in source vector is greater than the destination array", index));
		}
		array[index] = X;
		array[index + 1] = Y;
		array[index + 2] = Z;
	}

	[System.Runtime.CompilerServices.Intrinsic]
	public bool Equals(Vector3 other)
	{
		if (X == other.X && Y == other.Y)
		{
			return Z == other.Z;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static float Dot(Vector3 vector1, Vector3 vector2)
	{
		return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
	}

	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector3 Min(Vector3 value1, Vector3 value2)
	{
		return new Vector3((value1.X < value2.X) ? value1.X : value2.X, (value1.Y < value2.Y) ? value1.Y : value2.Y, (value1.Z < value2.Z) ? value1.Z : value2.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector3 Max(Vector3 value1, Vector3 value2)
	{
		return new Vector3((value1.X > value2.X) ? value1.X : value2.X, (value1.Y > value2.Y) ? value1.Y : value2.Y, (value1.Z > value2.Z) ? value1.Z : value2.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector3 Abs(Vector3 value)
	{
		return new Vector3(MathF.Abs(value.X), MathF.Abs(value.Y), MathF.Abs(value.Z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector3 SquareRoot(Vector3 value)
	{
		return new Vector3(MathF.Sqrt(value.X), MathF.Sqrt(value.Y), MathF.Sqrt(value.Z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector3 operator +(Vector3 left, Vector3 right)
	{
		return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector3 operator -(Vector3 left, Vector3 right)
	{
		return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector3 operator *(Vector3 left, Vector3 right)
	{
		return new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector3 operator *(Vector3 left, float right)
	{
		return left * new Vector3(right);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector3 operator *(float left, Vector3 right)
	{
		return new Vector3(left) * right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector3 operator /(Vector3 left, Vector3 right)
	{
		return new Vector3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 operator /(Vector3 value1, float value2)
	{
		return value1 / new Vector3(value2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 operator -(Vector3 value)
	{
		return Zero - value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static bool operator ==(Vector3 left, Vector3 right)
	{
		if (left.X == right.X && left.Y == right.Y)
		{
			return left.Z == right.Z;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(Vector3 left, Vector3 right)
	{
		if (left.X == right.X && left.Y == right.Y)
		{
			return left.Z != right.Z;
		}
		return true;
	}
}
