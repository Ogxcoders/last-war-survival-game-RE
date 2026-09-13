using System.Globalization;

namespace System.Numerics;

public struct Matrix3x2 : IEquatable<Matrix3x2>
{
	public float M11;

	public float M12;

	public float M21;

	public float M22;

	public float M31;

	public float M32;

	private static readonly Matrix3x2 _identity = new Matrix3x2(1f, 0f, 0f, 1f, 0f, 0f);

	public static Matrix3x2 Identity => _identity;

	public bool IsIdentity
	{
		get
		{
			if (M11 == 1f && M22 == 1f && M12 == 0f && M21 == 0f && M31 == 0f)
			{
				return M32 == 0f;
			}
			return false;
		}
	}

	public Vector2 Translation
	{
		get
		{
			return new Vector2(M31, M32);
		}
		set
		{
			M31 = value.X;
			M32 = value.Y;
		}
	}

	public Matrix3x2(float m11, float m12, float m21, float m22, float m31, float m32)
	{
		M11 = m11;
		M12 = m12;
		M21 = m21;
		M22 = m22;
		M31 = m31;
		M32 = m32;
	}

	public static Matrix3x2 CreateTranslation(Vector2 position)
	{
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = 1f;
		result.M12 = 0f;
		result.M21 = 0f;
		result.M22 = 1f;
		result.M31 = position.X;
		result.M32 = position.Y;
		return result;
	}

	public static Matrix3x2 CreateTranslation(float xPosition, float yPosition)
	{
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = 1f;
		result.M12 = 0f;
		result.M21 = 0f;
		result.M22 = 1f;
		result.M31 = xPosition;
		result.M32 = yPosition;
		return result;
	}

	public static Matrix3x2 CreateScale(float xScale, float yScale)
	{
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = xScale;
		result.M12 = 0f;
		result.M21 = 0f;
		result.M22 = yScale;
		result.M31 = 0f;
		result.M32 = 0f;
		return result;
	}

	public static Matrix3x2 CreateScale(float xScale, float yScale, Vector2 centerPoint)
	{
		float m = centerPoint.X * (1f - xScale);
		float m2 = centerPoint.Y * (1f - yScale);
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = xScale;
		result.M12 = 0f;
		result.M21 = 0f;
		result.M22 = yScale;
		result.M31 = m;
		result.M32 = m2;
		return result;
	}

	public static Matrix3x2 CreateScale(Vector2 scales)
	{
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = scales.X;
		result.M12 = 0f;
		result.M21 = 0f;
		result.M22 = scales.Y;
		result.M31 = 0f;
		result.M32 = 0f;
		return result;
	}

	public static Matrix3x2 CreateScale(Vector2 scales, Vector2 centerPoint)
	{
		float m = centerPoint.X * (1f - scales.X);
		float m2 = centerPoint.Y * (1f - scales.Y);
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = scales.X;
		result.M12 = 0f;
		result.M21 = 0f;
		result.M22 = scales.Y;
		result.M31 = m;
		result.M32 = m2;
		return result;
	}

	public static Matrix3x2 CreateScale(float scale)
	{
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = scale;
		result.M12 = 0f;
		result.M21 = 0f;
		result.M22 = scale;
		result.M31 = 0f;
		result.M32 = 0f;
		return result;
	}

	public static Matrix3x2 CreateScale(float scale, Vector2 centerPoint)
	{
		float m = centerPoint.X * (1f - scale);
		float m2 = centerPoint.Y * (1f - scale);
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = scale;
		result.M12 = 0f;
		result.M21 = 0f;
		result.M22 = scale;
		result.M31 = m;
		result.M32 = m2;
		return result;
	}

	public static Matrix3x2 CreateSkew(float radiansX, float radiansY)
	{
		float m = MathF.Tan(radiansX);
		float m2 = MathF.Tan(radiansY);
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = 1f;
		result.M12 = m2;
		result.M21 = m;
		result.M22 = 1f;
		result.M31 = 0f;
		result.M32 = 0f;
		return result;
	}

	public static Matrix3x2 CreateSkew(float radiansX, float radiansY, Vector2 centerPoint)
	{
		float num = MathF.Tan(radiansX);
		float num2 = MathF.Tan(radiansY);
		float m = (0f - centerPoint.Y) * num;
		float m2 = (0f - centerPoint.X) * num2;
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = 1f;
		result.M12 = num2;
		result.M21 = num;
		result.M22 = 1f;
		result.M31 = m;
		result.M32 = m2;
		return result;
	}

	public static Matrix3x2 CreateRotation(float radians)
	{
		radians = MathF.IEEERemainder(radians, MathF.PI * 2f);
		float num;
		float num2;
		if (radians > -1.7453294E-05f && radians < 1.7453294E-05f)
		{
			num = 1f;
			num2 = 0f;
		}
		else if (radians > 1.570779f && radians < 1.5708138f)
		{
			num = 0f;
			num2 = 1f;
		}
		else if (radians < -3.1415753f || radians > 3.1415753f)
		{
			num = -1f;
			num2 = 0f;
		}
		else if (radians > -1.5708138f && radians < -1.570779f)
		{
			num = 0f;
			num2 = -1f;
		}
		else
		{
			num = MathF.Cos(radians);
			num2 = MathF.Sin(radians);
		}
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = num;
		result.M12 = num2;
		result.M21 = 0f - num2;
		result.M22 = num;
		result.M31 = 0f;
		result.M32 = 0f;
		return result;
	}

	public static Matrix3x2 CreateRotation(float radians, Vector2 centerPoint)
	{
		radians = MathF.IEEERemainder(radians, MathF.PI * 2f);
		float num;
		float num2;
		if (radians > -1.7453294E-05f && radians < 1.7453294E-05f)
		{
			num = 1f;
			num2 = 0f;
		}
		else if (radians > 1.570779f && radians < 1.5708138f)
		{
			num = 0f;
			num2 = 1f;
		}
		else if (radians < -3.1415753f || radians > 3.1415753f)
		{
			num = -1f;
			num2 = 0f;
		}
		else if (radians > -1.5708138f && radians < -1.570779f)
		{
			num = 0f;
			num2 = -1f;
		}
		else
		{
			num = MathF.Cos(radians);
			num2 = MathF.Sin(radians);
		}
		float m = centerPoint.X * (1f - num) + centerPoint.Y * num2;
		float m2 = centerPoint.Y * (1f - num) - centerPoint.X * num2;
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = num;
		result.M12 = num2;
		result.M21 = 0f - num2;
		result.M22 = num;
		result.M31 = m;
		result.M32 = m2;
		return result;
	}

	public float GetDeterminant()
	{
		return M11 * M22 - M21 * M12;
	}

	public static bool Invert(Matrix3x2 matrix, out Matrix3x2 result)
	{
		float num = matrix.M11 * matrix.M22 - matrix.M21 * matrix.M12;
		if (MathF.Abs(num) < float.Epsilon)
		{
			result = new Matrix3x2(float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN);
			return false;
		}
		float num2 = 1f / num;
		result.M11 = matrix.M22 * num2;
		result.M12 = (0f - matrix.M12) * num2;
		result.M21 = (0f - matrix.M21) * num2;
		result.M22 = matrix.M11 * num2;
		result.M31 = (matrix.M21 * matrix.M32 - matrix.M31 * matrix.M22) * num2;
		result.M32 = (matrix.M31 * matrix.M12 - matrix.M11 * matrix.M32) * num2;
		return true;
	}

	public static Matrix3x2 Lerp(Matrix3x2 matrix1, Matrix3x2 matrix2, float amount)
	{
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = matrix1.M11 + (matrix2.M11 - matrix1.M11) * amount;
		result.M12 = matrix1.M12 + (matrix2.M12 - matrix1.M12) * amount;
		result.M21 = matrix1.M21 + (matrix2.M21 - matrix1.M21) * amount;
		result.M22 = matrix1.M22 + (matrix2.M22 - matrix1.M22) * amount;
		result.M31 = matrix1.M31 + (matrix2.M31 - matrix1.M31) * amount;
		result.M32 = matrix1.M32 + (matrix2.M32 - matrix1.M32) * amount;
		return result;
	}

	public static Matrix3x2 Negate(Matrix3x2 value)
	{
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = 0f - value.M11;
		result.M12 = 0f - value.M12;
		result.M21 = 0f - value.M21;
		result.M22 = 0f - value.M22;
		result.M31 = 0f - value.M31;
		result.M32 = 0f - value.M32;
		return result;
	}

	public static Matrix3x2 Add(Matrix3x2 value1, Matrix3x2 value2)
	{
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = value1.M11 + value2.M11;
		result.M12 = value1.M12 + value2.M12;
		result.M21 = value1.M21 + value2.M21;
		result.M22 = value1.M22 + value2.M22;
		result.M31 = value1.M31 + value2.M31;
		result.M32 = value1.M32 + value2.M32;
		return result;
	}

	public static Matrix3x2 Subtract(Matrix3x2 value1, Matrix3x2 value2)
	{
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = value1.M11 - value2.M11;
		result.M12 = value1.M12 - value2.M12;
		result.M21 = value1.M21 - value2.M21;
		result.M22 = value1.M22 - value2.M22;
		result.M31 = value1.M31 - value2.M31;
		result.M32 = value1.M32 - value2.M32;
		return result;
	}

	public static Matrix3x2 Multiply(Matrix3x2 value1, Matrix3x2 value2)
	{
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = value1.M11 * value2.M11 + value1.M12 * value2.M21;
		result.M12 = value1.M11 * value2.M12 + value1.M12 * value2.M22;
		result.M21 = value1.M21 * value2.M11 + value1.M22 * value2.M21;
		result.M22 = value1.M21 * value2.M12 + value1.M22 * value2.M22;
		result.M31 = value1.M31 * value2.M11 + value1.M32 * value2.M21 + value2.M31;
		result.M32 = value1.M31 * value2.M12 + value1.M32 * value2.M22 + value2.M32;
		return result;
	}

	public static Matrix3x2 Multiply(Matrix3x2 value1, float value2)
	{
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = value1.M11 * value2;
		result.M12 = value1.M12 * value2;
		result.M21 = value1.M21 * value2;
		result.M22 = value1.M22 * value2;
		result.M31 = value1.M31 * value2;
		result.M32 = value1.M32 * value2;
		return result;
	}

	public static Matrix3x2 operator -(Matrix3x2 value)
	{
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = 0f - value.M11;
		result.M12 = 0f - value.M12;
		result.M21 = 0f - value.M21;
		result.M22 = 0f - value.M22;
		result.M31 = 0f - value.M31;
		result.M32 = 0f - value.M32;
		return result;
	}

	public static Matrix3x2 operator +(Matrix3x2 value1, Matrix3x2 value2)
	{
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = value1.M11 + value2.M11;
		result.M12 = value1.M12 + value2.M12;
		result.M21 = value1.M21 + value2.M21;
		result.M22 = value1.M22 + value2.M22;
		result.M31 = value1.M31 + value2.M31;
		result.M32 = value1.M32 + value2.M32;
		return result;
	}

	public static Matrix3x2 operator -(Matrix3x2 value1, Matrix3x2 value2)
	{
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = value1.M11 - value2.M11;
		result.M12 = value1.M12 - value2.M12;
		result.M21 = value1.M21 - value2.M21;
		result.M22 = value1.M22 - value2.M22;
		result.M31 = value1.M31 - value2.M31;
		result.M32 = value1.M32 - value2.M32;
		return result;
	}

	public static Matrix3x2 operator *(Matrix3x2 value1, Matrix3x2 value2)
	{
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = value1.M11 * value2.M11 + value1.M12 * value2.M21;
		result.M12 = value1.M11 * value2.M12 + value1.M12 * value2.M22;
		result.M21 = value1.M21 * value2.M11 + value1.M22 * value2.M21;
		result.M22 = value1.M21 * value2.M12 + value1.M22 * value2.M22;
		result.M31 = value1.M31 * value2.M11 + value1.M32 * value2.M21 + value2.M31;
		result.M32 = value1.M31 * value2.M12 + value1.M32 * value2.M22 + value2.M32;
		return result;
	}

	public static Matrix3x2 operator *(Matrix3x2 value1, float value2)
	{
		Matrix3x2 result = default(Matrix3x2);
		result.M11 = value1.M11 * value2;
		result.M12 = value1.M12 * value2;
		result.M21 = value1.M21 * value2;
		result.M22 = value1.M22 * value2;
		result.M31 = value1.M31 * value2;
		result.M32 = value1.M32 * value2;
		return result;
	}

	public static bool operator ==(Matrix3x2 value1, Matrix3x2 value2)
	{
		if (value1.M11 == value2.M11 && value1.M22 == value2.M22 && value1.M12 == value2.M12 && value1.M21 == value2.M21 && value1.M31 == value2.M31)
		{
			return value1.M32 == value2.M32;
		}
		return false;
	}

	public static bool operator !=(Matrix3x2 value1, Matrix3x2 value2)
	{
		if (value1.M11 == value2.M11 && value1.M12 == value2.M12 && value1.M21 == value2.M21 && value1.M22 == value2.M22 && value1.M31 == value2.M31)
		{
			return value1.M32 != value2.M32;
		}
		return true;
	}

	public bool Equals(Matrix3x2 other)
	{
		if (M11 == other.M11 && M22 == other.M22 && M12 == other.M12 && M21 == other.M21 && M31 == other.M31)
		{
			return M32 == other.M32;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is Matrix3x2)
		{
			return Equals((Matrix3x2)obj);
		}
		return false;
	}

	public override string ToString()
	{
		CultureInfo currentCulture = CultureInfo.CurrentCulture;
		return string.Format(currentCulture, "{{ {{M11:{0} M12:{1}}} {{M21:{2} M22:{3}}} {{M31:{4} M32:{5}}} }}", M11.ToString(currentCulture), M12.ToString(currentCulture), M21.ToString(currentCulture), M22.ToString(currentCulture), M31.ToString(currentCulture), M32.ToString(currentCulture));
	}

	public override int GetHashCode()
	{
		return M11.GetHashCode() + M12.GetHashCode() + M21.GetHashCode() + M22.GetHashCode() + M31.GetHashCode() + M32.GetHashCode();
	}
}
