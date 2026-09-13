using System.Globalization;
using System.Numerics.Hashing;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Numerics;

public struct Vector2 : IEquatable<Vector2>, IFormattable
{
	public float X;

	public float Y;

	public static Vector2 Zero => default(Vector2);

	public static Vector2 One => new Vector2(1f, 1f);

	public static Vector2 UnitX => new Vector2(1f, 0f);

	public static Vector2 UnitY => new Vector2(0f, 1f);

	public override int GetHashCode()
	{
		return System.Numerics.Hashing.HashHelpers.Combine(X.GetHashCode(), Y.GetHashCode());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object obj)
	{
		if (!(obj is Vector2))
		{
			return false;
		}
		return Equals((Vector2)obj);
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
		return MathF.Sqrt(X * X + Y * Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float LengthSquared()
	{
		if (Vector.IsHardwareAccelerated)
		{
			return Dot(this, this);
		}
		return X * X + Y * Y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Distance(Vector2 value1, Vector2 value2)
	{
		if (Vector.IsHardwareAccelerated)
		{
			Vector2 vector = value1 - value2;
			return MathF.Sqrt(Dot(vector, vector));
		}
		float num = value1.X - value2.X;
		float num2 = value1.Y - value2.Y;
		return MathF.Sqrt(num * num + num2 * num2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float DistanceSquared(Vector2 value1, Vector2 value2)
	{
		if (Vector.IsHardwareAccelerated)
		{
			Vector2 vector = value1 - value2;
			return Dot(vector, vector);
		}
		float num = value1.X - value2.X;
		float num2 = value1.Y - value2.Y;
		return num * num + num2 * num2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Normalize(Vector2 value)
	{
		if (Vector.IsHardwareAccelerated)
		{
			float num = value.Length();
			return value / num;
		}
		float x = value.X * value.X + value.Y * value.Y;
		float num2 = 1f / MathF.Sqrt(x);
		return new Vector2(value.X * num2, value.Y * num2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Reflect(Vector2 vector, Vector2 normal)
	{
		if (Vector.IsHardwareAccelerated)
		{
			float num = Dot(vector, normal);
			return vector - 2f * num * normal;
		}
		float num2 = vector.X * normal.X + vector.Y * normal.Y;
		return new Vector2(vector.X - 2f * num2 * normal.X, vector.Y - 2f * num2 * normal.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Clamp(Vector2 value1, Vector2 min, Vector2 max)
	{
		float x = value1.X;
		x = ((x > max.X) ? max.X : x);
		x = ((x < min.X) ? min.X : x);
		float y = value1.Y;
		y = ((y > max.Y) ? max.Y : y);
		y = ((y < min.Y) ? min.Y : y);
		return new Vector2(x, y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
	{
		return new Vector2(value1.X + (value2.X - value1.X) * amount, value1.Y + (value2.Y - value1.Y) * amount);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Transform(Vector2 position, Matrix3x2 matrix)
	{
		return new Vector2(position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M31, position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M32);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Transform(Vector2 position, Matrix4x4 matrix)
	{
		return new Vector2(position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M41, position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M42);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 TransformNormal(Vector2 normal, Matrix3x2 matrix)
	{
		return new Vector2(normal.X * matrix.M11 + normal.Y * matrix.M21, normal.X * matrix.M12 + normal.Y * matrix.M22);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 TransformNormal(Vector2 normal, Matrix4x4 matrix)
	{
		return new Vector2(normal.X * matrix.M11 + normal.Y * matrix.M21, normal.X * matrix.M12 + normal.Y * matrix.M22);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Transform(Vector2 value, Quaternion rotation)
	{
		float num = rotation.X + rotation.X;
		float num2 = rotation.Y + rotation.Y;
		float num3 = rotation.Z + rotation.Z;
		float num4 = rotation.W * num3;
		float num5 = rotation.X * num;
		float num6 = rotation.X * num2;
		float num7 = rotation.Y * num2;
		float num8 = rotation.Z * num3;
		return new Vector2(value.X * (1f - num7 - num8) + value.Y * (num6 - num4), value.X * (num6 + num4) + value.Y * (1f - num5 - num8));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Add(Vector2 left, Vector2 right)
	{
		return left + right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Subtract(Vector2 left, Vector2 right)
	{
		return left - right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Multiply(Vector2 left, Vector2 right)
	{
		return left * right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Multiply(Vector2 left, float right)
	{
		return left * right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Multiply(float left, Vector2 right)
	{
		return left * right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Divide(Vector2 left, Vector2 right)
	{
		return left / right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Divide(Vector2 left, float divisor)
	{
		return left / divisor;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Negate(Vector2 value)
	{
		return -value;
	}

	[System.Runtime.CompilerServices.Intrinsic]
	public Vector2(float value)
		: this(value, value)
	{
	}

	[System.Runtime.CompilerServices.Intrinsic]
	public Vector2(float x, float y)
	{
		X = x;
		Y = y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void CopyTo(float[] array)
	{
		CopyTo(array, 0);
	}

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
		if (array.Length - index < 2)
		{
			throw new ArgumentException(global::SR.Format("Number of elements in source vector is greater than the destination array", index));
		}
		array[index] = X;
		array[index + 1] = Y;
	}

	[System.Runtime.CompilerServices.Intrinsic]
	public bool Equals(Vector2 other)
	{
		if (X == other.X)
		{
			return Y == other.Y;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static float Dot(Vector2 value1, Vector2 value2)
	{
		return value1.X * value2.X + value1.Y * value2.Y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector2 Min(Vector2 value1, Vector2 value2)
	{
		return new Vector2((value1.X < value2.X) ? value1.X : value2.X, (value1.Y < value2.Y) ? value1.Y : value2.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector2 Max(Vector2 value1, Vector2 value2)
	{
		return new Vector2((value1.X > value2.X) ? value1.X : value2.X, (value1.Y > value2.Y) ? value1.Y : value2.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector2 Abs(Vector2 value)
	{
		return new Vector2(MathF.Abs(value.X), MathF.Abs(value.Y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector2 SquareRoot(Vector2 value)
	{
		return new Vector2(MathF.Sqrt(value.X), MathF.Sqrt(value.Y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector2 operator +(Vector2 left, Vector2 right)
	{
		return new Vector2(left.X + right.X, left.Y + right.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector2 operator -(Vector2 left, Vector2 right)
	{
		return new Vector2(left.X - right.X, left.Y - right.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector2 operator *(Vector2 left, Vector2 right)
	{
		return new Vector2(left.X * right.X, left.Y * right.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector2 operator *(float left, Vector2 right)
	{
		return new Vector2(left, left) * right;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector2 operator *(Vector2 left, float right)
	{
		return left * new Vector2(right, right);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Runtime.CompilerServices.Intrinsic]
	public static Vector2 operator /(Vector2 left, Vector2 right)
	{
		return new Vector2(left.X / right.X, left.Y / right.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 operator /(Vector2 value1, float value2)
	{
		return value1 / new Vector2(value2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 operator -(Vector2 value)
	{
		return Zero - value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(Vector2 left, Vector2 right)
	{
		return left.Equals(right);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(Vector2 left, Vector2 right)
	{
		return !(left == right);
	}
}
