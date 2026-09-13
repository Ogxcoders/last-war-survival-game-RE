using System.Globalization;

namespace System.Numerics;

public struct Matrix4x4 : IEquatable<Matrix4x4>
{
	private struct CanonicalBasis
	{
		public Vector3 Row0;

		public Vector3 Row1;

		public Vector3 Row2;
	}

	private struct VectorBasis
	{
		public unsafe Vector3* Element0;

		public unsafe Vector3* Element1;

		public unsafe Vector3* Element2;
	}

	public float M11;

	public float M12;

	public float M13;

	public float M14;

	public float M21;

	public float M22;

	public float M23;

	public float M24;

	public float M31;

	public float M32;

	public float M33;

	public float M34;

	public float M41;

	public float M42;

	public float M43;

	public float M44;

	private static readonly Matrix4x4 _identity = new Matrix4x4(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);

	public static Matrix4x4 Identity => _identity;

	public bool IsIdentity
	{
		get
		{
			if (M11 == 1f && M22 == 1f && M33 == 1f && M44 == 1f && M12 == 0f && M13 == 0f && M14 == 0f && M21 == 0f && M23 == 0f && M24 == 0f && M31 == 0f && M32 == 0f && M34 == 0f && M41 == 0f && M42 == 0f)
			{
				return M43 == 0f;
			}
			return false;
		}
	}

	public Vector3 Translation
	{
		get
		{
			return new Vector3(M41, M42, M43);
		}
		set
		{
			M41 = value.X;
			M42 = value.Y;
			M43 = value.Z;
		}
	}

	public Matrix4x4(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44)
	{
		M11 = m11;
		M12 = m12;
		M13 = m13;
		M14 = m14;
		M21 = m21;
		M22 = m22;
		M23 = m23;
		M24 = m24;
		M31 = m31;
		M32 = m32;
		M33 = m33;
		M34 = m34;
		M41 = m41;
		M42 = m42;
		M43 = m43;
		M44 = m44;
	}

	public Matrix4x4(Matrix3x2 value)
	{
		M11 = value.M11;
		M12 = value.M12;
		M13 = 0f;
		M14 = 0f;
		M21 = value.M21;
		M22 = value.M22;
		M23 = 0f;
		M24 = 0f;
		M31 = 0f;
		M32 = 0f;
		M33 = 1f;
		M34 = 0f;
		M41 = value.M31;
		M42 = value.M32;
		M43 = 0f;
		M44 = 1f;
	}

	public static Matrix4x4 CreateBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 cameraUpVector, Vector3 cameraForwardVector)
	{
		Vector3 left = new Vector3(objectPosition.X - cameraPosition.X, objectPosition.Y - cameraPosition.Y, objectPosition.Z - cameraPosition.Z);
		float num = left.LengthSquared();
		left = ((!(num < 0.0001f)) ? Vector3.Multiply(left, 1f / MathF.Sqrt(num)) : (-cameraForwardVector));
		Vector3 vector = Vector3.Normalize(Vector3.Cross(cameraUpVector, left));
		Vector3 vector2 = Vector3.Cross(left, vector);
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = vector.X;
		result.M12 = vector.Y;
		result.M13 = vector.Z;
		result.M14 = 0f;
		result.M21 = vector2.X;
		result.M22 = vector2.Y;
		result.M23 = vector2.Z;
		result.M24 = 0f;
		result.M31 = left.X;
		result.M32 = left.Y;
		result.M33 = left.Z;
		result.M34 = 0f;
		result.M41 = objectPosition.X;
		result.M42 = objectPosition.Y;
		result.M43 = objectPosition.Z;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateConstrainedBillboard(Vector3 objectPosition, Vector3 cameraPosition, Vector3 rotateAxis, Vector3 cameraForwardVector, Vector3 objectForwardVector)
	{
		Vector3 left = new Vector3(objectPosition.X - cameraPosition.X, objectPosition.Y - cameraPosition.Y, objectPosition.Z - cameraPosition.Z);
		float num = left.LengthSquared();
		left = ((!(num < 0.0001f)) ? Vector3.Multiply(left, 1f / MathF.Sqrt(num)) : (-cameraForwardVector));
		Vector3 vector = rotateAxis;
		Vector3 vector3;
		Vector3 vector2;
		if (MathF.Abs(Vector3.Dot(rotateAxis, left)) > 0.99825466f)
		{
			vector2 = objectForwardVector;
			if (MathF.Abs(Vector3.Dot(rotateAxis, vector2)) > 0.99825466f)
			{
				vector2 = ((MathF.Abs(rotateAxis.Z) > 0.99825466f) ? new Vector3(1f, 0f, 0f) : new Vector3(0f, 0f, -1f));
			}
			vector3 = Vector3.Normalize(Vector3.Cross(rotateAxis, vector2));
			vector2 = Vector3.Normalize(Vector3.Cross(vector3, rotateAxis));
		}
		else
		{
			vector3 = Vector3.Normalize(Vector3.Cross(rotateAxis, left));
			vector2 = Vector3.Normalize(Vector3.Cross(vector3, vector));
		}
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = vector3.X;
		result.M12 = vector3.Y;
		result.M13 = vector3.Z;
		result.M14 = 0f;
		result.M21 = vector.X;
		result.M22 = vector.Y;
		result.M23 = vector.Z;
		result.M24 = 0f;
		result.M31 = vector2.X;
		result.M32 = vector2.Y;
		result.M33 = vector2.Z;
		result.M34 = 0f;
		result.M41 = objectPosition.X;
		result.M42 = objectPosition.Y;
		result.M43 = objectPosition.Z;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateTranslation(Vector3 position)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = 1f;
		result.M12 = 0f;
		result.M13 = 0f;
		result.M14 = 0f;
		result.M21 = 0f;
		result.M22 = 1f;
		result.M23 = 0f;
		result.M24 = 0f;
		result.M31 = 0f;
		result.M32 = 0f;
		result.M33 = 1f;
		result.M34 = 0f;
		result.M41 = position.X;
		result.M42 = position.Y;
		result.M43 = position.Z;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateTranslation(float xPosition, float yPosition, float zPosition)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = 1f;
		result.M12 = 0f;
		result.M13 = 0f;
		result.M14 = 0f;
		result.M21 = 0f;
		result.M22 = 1f;
		result.M23 = 0f;
		result.M24 = 0f;
		result.M31 = 0f;
		result.M32 = 0f;
		result.M33 = 1f;
		result.M34 = 0f;
		result.M41 = xPosition;
		result.M42 = yPosition;
		result.M43 = zPosition;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateScale(float xScale, float yScale, float zScale)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = xScale;
		result.M12 = 0f;
		result.M13 = 0f;
		result.M14 = 0f;
		result.M21 = 0f;
		result.M22 = yScale;
		result.M23 = 0f;
		result.M24 = 0f;
		result.M31 = 0f;
		result.M32 = 0f;
		result.M33 = zScale;
		result.M34 = 0f;
		result.M41 = 0f;
		result.M42 = 0f;
		result.M43 = 0f;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateScale(float xScale, float yScale, float zScale, Vector3 centerPoint)
	{
		float m = centerPoint.X * (1f - xScale);
		float m2 = centerPoint.Y * (1f - yScale);
		float m3 = centerPoint.Z * (1f - zScale);
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = xScale;
		result.M12 = 0f;
		result.M13 = 0f;
		result.M14 = 0f;
		result.M21 = 0f;
		result.M22 = yScale;
		result.M23 = 0f;
		result.M24 = 0f;
		result.M31 = 0f;
		result.M32 = 0f;
		result.M33 = zScale;
		result.M34 = 0f;
		result.M41 = m;
		result.M42 = m2;
		result.M43 = m3;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateScale(Vector3 scales)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = scales.X;
		result.M12 = 0f;
		result.M13 = 0f;
		result.M14 = 0f;
		result.M21 = 0f;
		result.M22 = scales.Y;
		result.M23 = 0f;
		result.M24 = 0f;
		result.M31 = 0f;
		result.M32 = 0f;
		result.M33 = scales.Z;
		result.M34 = 0f;
		result.M41 = 0f;
		result.M42 = 0f;
		result.M43 = 0f;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateScale(Vector3 scales, Vector3 centerPoint)
	{
		float m = centerPoint.X * (1f - scales.X);
		float m2 = centerPoint.Y * (1f - scales.Y);
		float m3 = centerPoint.Z * (1f - scales.Z);
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = scales.X;
		result.M12 = 0f;
		result.M13 = 0f;
		result.M14 = 0f;
		result.M21 = 0f;
		result.M22 = scales.Y;
		result.M23 = 0f;
		result.M24 = 0f;
		result.M31 = 0f;
		result.M32 = 0f;
		result.M33 = scales.Z;
		result.M34 = 0f;
		result.M41 = m;
		result.M42 = m2;
		result.M43 = m3;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateScale(float scale)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = scale;
		result.M12 = 0f;
		result.M13 = 0f;
		result.M14 = 0f;
		result.M21 = 0f;
		result.M22 = scale;
		result.M23 = 0f;
		result.M24 = 0f;
		result.M31 = 0f;
		result.M32 = 0f;
		result.M33 = scale;
		result.M34 = 0f;
		result.M41 = 0f;
		result.M42 = 0f;
		result.M43 = 0f;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateScale(float scale, Vector3 centerPoint)
	{
		float m = centerPoint.X * (1f - scale);
		float m2 = centerPoint.Y * (1f - scale);
		float m3 = centerPoint.Z * (1f - scale);
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = scale;
		result.M12 = 0f;
		result.M13 = 0f;
		result.M14 = 0f;
		result.M21 = 0f;
		result.M22 = scale;
		result.M23 = 0f;
		result.M24 = 0f;
		result.M31 = 0f;
		result.M32 = 0f;
		result.M33 = scale;
		result.M34 = 0f;
		result.M41 = m;
		result.M42 = m2;
		result.M43 = m3;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateRotationX(float radians)
	{
		float num = MathF.Cos(radians);
		float num2 = MathF.Sin(radians);
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = 1f;
		result.M12 = 0f;
		result.M13 = 0f;
		result.M14 = 0f;
		result.M21 = 0f;
		result.M22 = num;
		result.M23 = num2;
		result.M24 = 0f;
		result.M31 = 0f;
		result.M32 = 0f - num2;
		result.M33 = num;
		result.M34 = 0f;
		result.M41 = 0f;
		result.M42 = 0f;
		result.M43 = 0f;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateRotationX(float radians, Vector3 centerPoint)
	{
		float num = MathF.Cos(radians);
		float num2 = MathF.Sin(radians);
		float m = centerPoint.Y * (1f - num) + centerPoint.Z * num2;
		float m2 = centerPoint.Z * (1f - num) - centerPoint.Y * num2;
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = 1f;
		result.M12 = 0f;
		result.M13 = 0f;
		result.M14 = 0f;
		result.M21 = 0f;
		result.M22 = num;
		result.M23 = num2;
		result.M24 = 0f;
		result.M31 = 0f;
		result.M32 = 0f - num2;
		result.M33 = num;
		result.M34 = 0f;
		result.M41 = 0f;
		result.M42 = m;
		result.M43 = m2;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateRotationY(float radians)
	{
		float num = MathF.Cos(radians);
		float num2 = MathF.Sin(radians);
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = num;
		result.M12 = 0f;
		result.M13 = 0f - num2;
		result.M14 = 0f;
		result.M21 = 0f;
		result.M22 = 1f;
		result.M23 = 0f;
		result.M24 = 0f;
		result.M31 = num2;
		result.M32 = 0f;
		result.M33 = num;
		result.M34 = 0f;
		result.M41 = 0f;
		result.M42 = 0f;
		result.M43 = 0f;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateRotationY(float radians, Vector3 centerPoint)
	{
		float num = MathF.Cos(radians);
		float num2 = MathF.Sin(radians);
		float m = centerPoint.X * (1f - num) - centerPoint.Z * num2;
		float m2 = centerPoint.Z * (1f - num) + centerPoint.X * num2;
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = num;
		result.M12 = 0f;
		result.M13 = 0f - num2;
		result.M14 = 0f;
		result.M21 = 0f;
		result.M22 = 1f;
		result.M23 = 0f;
		result.M24 = 0f;
		result.M31 = num2;
		result.M32 = 0f;
		result.M33 = num;
		result.M34 = 0f;
		result.M41 = m;
		result.M42 = 0f;
		result.M43 = m2;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateRotationZ(float radians)
	{
		float num = MathF.Cos(radians);
		float num2 = MathF.Sin(radians);
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = num;
		result.M12 = num2;
		result.M13 = 0f;
		result.M14 = 0f;
		result.M21 = 0f - num2;
		result.M22 = num;
		result.M23 = 0f;
		result.M24 = 0f;
		result.M31 = 0f;
		result.M32 = 0f;
		result.M33 = 1f;
		result.M34 = 0f;
		result.M41 = 0f;
		result.M42 = 0f;
		result.M43 = 0f;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateRotationZ(float radians, Vector3 centerPoint)
	{
		float num = MathF.Cos(radians);
		float num2 = MathF.Sin(radians);
		float m = centerPoint.X * (1f - num) + centerPoint.Y * num2;
		float m2 = centerPoint.Y * (1f - num) - centerPoint.X * num2;
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = num;
		result.M12 = num2;
		result.M13 = 0f;
		result.M14 = 0f;
		result.M21 = 0f - num2;
		result.M22 = num;
		result.M23 = 0f;
		result.M24 = 0f;
		result.M31 = 0f;
		result.M32 = 0f;
		result.M33 = 1f;
		result.M34 = 0f;
		result.M41 = m;
		result.M42 = m2;
		result.M43 = 0f;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateFromAxisAngle(Vector3 axis, float angle)
	{
		float x = axis.X;
		float y = axis.Y;
		float z = axis.Z;
		float num = MathF.Sin(angle);
		float num2 = MathF.Cos(angle);
		float num3 = x * x;
		float num4 = y * y;
		float num5 = z * z;
		float num6 = x * y;
		float num7 = x * z;
		float num8 = y * z;
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = num3 + num2 * (1f - num3);
		result.M12 = num6 - num2 * num6 + num * z;
		result.M13 = num7 - num2 * num7 - num * y;
		result.M14 = 0f;
		result.M21 = num6 - num2 * num6 - num * z;
		result.M22 = num4 + num2 * (1f - num4);
		result.M23 = num8 - num2 * num8 + num * x;
		result.M24 = 0f;
		result.M31 = num7 - num2 * num7 + num * y;
		result.M32 = num8 - num2 * num8 - num * x;
		result.M33 = num5 + num2 * (1f - num5);
		result.M34 = 0f;
		result.M41 = 0f;
		result.M42 = 0f;
		result.M43 = 0f;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
	{
		if (fieldOfView <= 0f || fieldOfView >= MathF.PI)
		{
			throw new ArgumentOutOfRangeException("fieldOfView");
		}
		if (nearPlaneDistance <= 0f)
		{
			throw new ArgumentOutOfRangeException("nearPlaneDistance");
		}
		if (farPlaneDistance <= 0f)
		{
			throw new ArgumentOutOfRangeException("farPlaneDistance");
		}
		if (nearPlaneDistance >= farPlaneDistance)
		{
			throw new ArgumentOutOfRangeException("nearPlaneDistance");
		}
		float num = 1f / MathF.Tan(fieldOfView * 0.5f);
		float m = num / aspectRatio;
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = m;
		result.M12 = (result.M13 = (result.M14 = 0f));
		result.M22 = num;
		result.M21 = (result.M23 = (result.M24 = 0f));
		result.M31 = (result.M32 = 0f);
		float num2 = (result.M33 = (float.IsPositiveInfinity(farPlaneDistance) ? (-1f) : (farPlaneDistance / (nearPlaneDistance - farPlaneDistance))));
		result.M34 = -1f;
		result.M41 = (result.M42 = (result.M44 = 0f));
		result.M43 = nearPlaneDistance * num2;
		return result;
	}

	public static Matrix4x4 CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance)
	{
		if (nearPlaneDistance <= 0f)
		{
			throw new ArgumentOutOfRangeException("nearPlaneDistance");
		}
		if (farPlaneDistance <= 0f)
		{
			throw new ArgumentOutOfRangeException("farPlaneDistance");
		}
		if (nearPlaneDistance >= farPlaneDistance)
		{
			throw new ArgumentOutOfRangeException("nearPlaneDistance");
		}
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = 2f * nearPlaneDistance / width;
		result.M12 = (result.M13 = (result.M14 = 0f));
		result.M22 = 2f * nearPlaneDistance / height;
		result.M21 = (result.M23 = (result.M24 = 0f));
		float num = (result.M33 = (float.IsPositiveInfinity(farPlaneDistance) ? (-1f) : (farPlaneDistance / (nearPlaneDistance - farPlaneDistance))));
		result.M31 = (result.M32 = 0f);
		result.M34 = -1f;
		result.M41 = (result.M42 = (result.M44 = 0f));
		result.M43 = nearPlaneDistance * num;
		return result;
	}

	public static Matrix4x4 CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance)
	{
		if (nearPlaneDistance <= 0f)
		{
			throw new ArgumentOutOfRangeException("nearPlaneDistance");
		}
		if (farPlaneDistance <= 0f)
		{
			throw new ArgumentOutOfRangeException("farPlaneDistance");
		}
		if (nearPlaneDistance >= farPlaneDistance)
		{
			throw new ArgumentOutOfRangeException("nearPlaneDistance");
		}
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = 2f * nearPlaneDistance / (right - left);
		result.M12 = (result.M13 = (result.M14 = 0f));
		result.M22 = 2f * nearPlaneDistance / (top - bottom);
		result.M21 = (result.M23 = (result.M24 = 0f));
		result.M31 = (left + right) / (right - left);
		result.M32 = (top + bottom) / (top - bottom);
		float num = (result.M33 = (float.IsPositiveInfinity(farPlaneDistance) ? (-1f) : (farPlaneDistance / (nearPlaneDistance - farPlaneDistance))));
		result.M34 = -1f;
		result.M43 = nearPlaneDistance * num;
		result.M41 = (result.M42 = (result.M44 = 0f));
		return result;
	}

	public static Matrix4x4 CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = 2f / width;
		result.M12 = (result.M13 = (result.M14 = 0f));
		result.M22 = 2f / height;
		result.M21 = (result.M23 = (result.M24 = 0f));
		result.M33 = 1f / (zNearPlane - zFarPlane);
		result.M31 = (result.M32 = (result.M34 = 0f));
		result.M41 = (result.M42 = 0f);
		result.M43 = zNearPlane / (zNearPlane - zFarPlane);
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = 2f / (right - left);
		result.M12 = (result.M13 = (result.M14 = 0f));
		result.M22 = 2f / (top - bottom);
		result.M21 = (result.M23 = (result.M24 = 0f));
		result.M33 = 1f / (zNearPlane - zFarPlane);
		result.M31 = (result.M32 = (result.M34 = 0f));
		result.M41 = (left + right) / (left - right);
		result.M42 = (top + bottom) / (bottom - top);
		result.M43 = zNearPlane / (zNearPlane - zFarPlane);
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateLookAt(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
	{
		Vector3 vector = Vector3.Normalize(cameraPosition - cameraTarget);
		Vector3 vector2 = Vector3.Normalize(Vector3.Cross(cameraUpVector, vector));
		Vector3 vector3 = Vector3.Cross(vector, vector2);
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = vector2.X;
		result.M12 = vector3.X;
		result.M13 = vector.X;
		result.M14 = 0f;
		result.M21 = vector2.Y;
		result.M22 = vector3.Y;
		result.M23 = vector.Y;
		result.M24 = 0f;
		result.M31 = vector2.Z;
		result.M32 = vector3.Z;
		result.M33 = vector.Z;
		result.M34 = 0f;
		result.M41 = 0f - Vector3.Dot(vector2, cameraPosition);
		result.M42 = 0f - Vector3.Dot(vector3, cameraPosition);
		result.M43 = 0f - Vector3.Dot(vector, cameraPosition);
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateWorld(Vector3 position, Vector3 forward, Vector3 up)
	{
		Vector3 vector = Vector3.Normalize(-forward);
		Vector3 vector2 = Vector3.Normalize(Vector3.Cross(up, vector));
		Vector3 vector3 = Vector3.Cross(vector, vector2);
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = vector2.X;
		result.M12 = vector2.Y;
		result.M13 = vector2.Z;
		result.M14 = 0f;
		result.M21 = vector3.X;
		result.M22 = vector3.Y;
		result.M23 = vector3.Z;
		result.M24 = 0f;
		result.M31 = vector.X;
		result.M32 = vector.Y;
		result.M33 = vector.Z;
		result.M34 = 0f;
		result.M41 = position.X;
		result.M42 = position.Y;
		result.M43 = position.Z;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateFromQuaternion(Quaternion quaternion)
	{
		float num = quaternion.X * quaternion.X;
		float num2 = quaternion.Y * quaternion.Y;
		float num3 = quaternion.Z * quaternion.Z;
		float num4 = quaternion.X * quaternion.Y;
		float num5 = quaternion.Z * quaternion.W;
		float num6 = quaternion.Z * quaternion.X;
		float num7 = quaternion.Y * quaternion.W;
		float num8 = quaternion.Y * quaternion.Z;
		float num9 = quaternion.X * quaternion.W;
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = 1f - 2f * (num2 + num3);
		result.M12 = 2f * (num4 + num5);
		result.M13 = 2f * (num6 - num7);
		result.M14 = 0f;
		result.M21 = 2f * (num4 - num5);
		result.M22 = 1f - 2f * (num3 + num);
		result.M23 = 2f * (num8 + num9);
		result.M24 = 0f;
		result.M31 = 2f * (num6 + num7);
		result.M32 = 2f * (num8 - num9);
		result.M33 = 1f - 2f * (num2 + num);
		result.M34 = 0f;
		result.M41 = 0f;
		result.M42 = 0f;
		result.M43 = 0f;
		result.M44 = 1f;
		return result;
	}

	public static Matrix4x4 CreateFromYawPitchRoll(float yaw, float pitch, float roll)
	{
		return CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll));
	}

	public static Matrix4x4 CreateShadow(Vector3 lightDirection, Plane plane)
	{
		Plane plane2 = Plane.Normalize(plane);
		float num = plane2.Normal.X * lightDirection.X + plane2.Normal.Y * lightDirection.Y + plane2.Normal.Z * lightDirection.Z;
		float num2 = 0f - plane2.Normal.X;
		float num3 = 0f - plane2.Normal.Y;
		float num4 = 0f - plane2.Normal.Z;
		float num5 = 0f - plane2.D;
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = num2 * lightDirection.X + num;
		result.M21 = num3 * lightDirection.X;
		result.M31 = num4 * lightDirection.X;
		result.M41 = num5 * lightDirection.X;
		result.M12 = num2 * lightDirection.Y;
		result.M22 = num3 * lightDirection.Y + num;
		result.M32 = num4 * lightDirection.Y;
		result.M42 = num5 * lightDirection.Y;
		result.M13 = num2 * lightDirection.Z;
		result.M23 = num3 * lightDirection.Z;
		result.M33 = num4 * lightDirection.Z + num;
		result.M43 = num5 * lightDirection.Z;
		result.M14 = 0f;
		result.M24 = 0f;
		result.M34 = 0f;
		result.M44 = num;
		return result;
	}

	public static Matrix4x4 CreateReflection(Plane value)
	{
		value = Plane.Normalize(value);
		float x = value.Normal.X;
		float y = value.Normal.Y;
		float z = value.Normal.Z;
		float num = -2f * x;
		float num2 = -2f * y;
		float num3 = -2f * z;
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = num * x + 1f;
		result.M12 = num2 * x;
		result.M13 = num3 * x;
		result.M14 = 0f;
		result.M21 = num * y;
		result.M22 = num2 * y + 1f;
		result.M23 = num3 * y;
		result.M24 = 0f;
		result.M31 = num * z;
		result.M32 = num2 * z;
		result.M33 = num3 * z + 1f;
		result.M34 = 0f;
		result.M41 = num * value.D;
		result.M42 = num2 * value.D;
		result.M43 = num3 * value.D;
		result.M44 = 1f;
		return result;
	}

	public float GetDeterminant()
	{
		float m = M11;
		float m2 = M12;
		float m3 = M13;
		float m4 = M14;
		float m5 = M21;
		float m6 = M22;
		float m7 = M23;
		float m8 = M24;
		float m9 = M31;
		float m10 = M32;
		float m11 = M33;
		float m12 = M34;
		float m13 = M41;
		float m14 = M42;
		float m15 = M43;
		float m16 = M44;
		float num = m11 * m16 - m12 * m15;
		float num2 = m10 * m16 - m12 * m14;
		float num3 = m10 * m15 - m11 * m14;
		float num4 = m9 * m16 - m12 * m13;
		float num5 = m9 * m15 - m11 * m13;
		float num6 = m9 * m14 - m10 * m13;
		return m * (m6 * num - m7 * num2 + m8 * num3) - m2 * (m5 * num - m7 * num4 + m8 * num5) + m3 * (m5 * num2 - m6 * num4 + m8 * num6) - m4 * (m5 * num3 - m6 * num5 + m7 * num6);
	}

	public static bool Invert(Matrix4x4 matrix, out Matrix4x4 result)
	{
		float m = matrix.M11;
		float m2 = matrix.M12;
		float m3 = matrix.M13;
		float m4 = matrix.M14;
		float m5 = matrix.M21;
		float m6 = matrix.M22;
		float m7 = matrix.M23;
		float m8 = matrix.M24;
		float m9 = matrix.M31;
		float m10 = matrix.M32;
		float m11 = matrix.M33;
		float m12 = matrix.M34;
		float m13 = matrix.M41;
		float m14 = matrix.M42;
		float m15 = matrix.M43;
		float m16 = matrix.M44;
		float num = m11 * m16 - m12 * m15;
		float num2 = m10 * m16 - m12 * m14;
		float num3 = m10 * m15 - m11 * m14;
		float num4 = m9 * m16 - m12 * m13;
		float num5 = m9 * m15 - m11 * m13;
		float num6 = m9 * m14 - m10 * m13;
		float num7 = m6 * num - m7 * num2 + m8 * num3;
		float num8 = 0f - (m5 * num - m7 * num4 + m8 * num5);
		float num9 = m5 * num2 - m6 * num4 + m8 * num6;
		float num10 = 0f - (m5 * num3 - m6 * num5 + m7 * num6);
		float num11 = m * num7 + m2 * num8 + m3 * num9 + m4 * num10;
		if (MathF.Abs(num11) < float.Epsilon)
		{
			result = new Matrix4x4(float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN);
			return false;
		}
		float num12 = 1f / num11;
		result.M11 = num7 * num12;
		result.M21 = num8 * num12;
		result.M31 = num9 * num12;
		result.M41 = num10 * num12;
		result.M12 = (0f - (m2 * num - m3 * num2 + m4 * num3)) * num12;
		result.M22 = (m * num - m3 * num4 + m4 * num5) * num12;
		result.M32 = (0f - (m * num2 - m2 * num4 + m4 * num6)) * num12;
		result.M42 = (m * num3 - m2 * num5 + m3 * num6) * num12;
		float num13 = m7 * m16 - m8 * m15;
		float num14 = m6 * m16 - m8 * m14;
		float num15 = m6 * m15 - m7 * m14;
		float num16 = m5 * m16 - m8 * m13;
		float num17 = m5 * m15 - m7 * m13;
		float num18 = m5 * m14 - m6 * m13;
		result.M13 = (m2 * num13 - m3 * num14 + m4 * num15) * num12;
		result.M23 = (0f - (m * num13 - m3 * num16 + m4 * num17)) * num12;
		result.M33 = (m * num14 - m2 * num16 + m4 * num18) * num12;
		result.M43 = (0f - (m * num15 - m2 * num17 + m3 * num18)) * num12;
		float num19 = m7 * m12 - m8 * m11;
		float num20 = m6 * m12 - m8 * m10;
		float num21 = m6 * m11 - m7 * m10;
		float num22 = m5 * m12 - m8 * m9;
		float num23 = m5 * m11 - m7 * m9;
		float num24 = m5 * m10 - m6 * m9;
		result.M14 = (0f - (m2 * num19 - m3 * num20 + m4 * num21)) * num12;
		result.M24 = (m * num19 - m3 * num22 + m4 * num23) * num12;
		result.M34 = (0f - (m * num20 - m2 * num22 + m4 * num24)) * num12;
		result.M44 = (m * num21 - m2 * num23 + m3 * num24) * num12;
		return true;
	}

	public unsafe static bool Decompose(Matrix4x4 matrix, out Vector3 scale, out Quaternion rotation, out Vector3 translation)
	{
		bool result = true;
		fixed (Vector3* ptr = &scale)
		{
			float* ptr2 = (float*)ptr;
			VectorBasis vectorBasis = default(VectorBasis);
			Vector3** ptr3 = (Vector3**)(&vectorBasis);
			Matrix4x4 identity = Identity;
			CanonicalBasis canonicalBasis = default(CanonicalBasis);
			Vector3* ptr4 = &canonicalBasis.Row0;
			canonicalBasis.Row0 = new Vector3(1f, 0f, 0f);
			canonicalBasis.Row1 = new Vector3(0f, 1f, 0f);
			canonicalBasis.Row2 = new Vector3(0f, 0f, 1f);
			translation = new Vector3(matrix.M41, matrix.M42, matrix.M43);
			*ptr3 = (Vector3*)(&identity.M11);
			ptr3[1] = (Vector3*)(&identity.M21);
			ptr3[2] = (Vector3*)(&identity.M31);
			*(*ptr3) = new Vector3(matrix.M11, matrix.M12, matrix.M13);
			*ptr3[1] = new Vector3(matrix.M21, matrix.M22, matrix.M23);
			*ptr3[2] = new Vector3(matrix.M31, matrix.M32, matrix.M33);
			scale.X = (*ptr3)->Length();
			scale.Y = ptr3[1]->Length();
			scale.Z = ptr3[2]->Length();
			float num = *ptr2;
			float num2 = ptr2[1];
			float num3 = ptr2[2];
			uint num4;
			uint num5;
			uint num6;
			if (num < num2)
			{
				if (num2 < num3)
				{
					num4 = 2u;
					num5 = 1u;
					num6 = 0u;
				}
				else
				{
					num4 = 1u;
					if (num < num3)
					{
						num5 = 2u;
						num6 = 0u;
					}
					else
					{
						num5 = 0u;
						num6 = 2u;
					}
				}
			}
			else if (num < num3)
			{
				num4 = 2u;
				num5 = 0u;
				num6 = 1u;
			}
			else
			{
				num4 = 0u;
				if (num2 < num3)
				{
					num5 = 2u;
					num6 = 1u;
				}
				else
				{
					num5 = 1u;
					num6 = 2u;
				}
			}
			if (ptr2[num4] < 0.0001f)
			{
				*ptr3[num4] = ptr4[num4];
			}
			*ptr3[num4] = Vector3.Normalize(*ptr3[num4]);
			if (ptr2[num5] < 0.0001f)
			{
				float num7 = MathF.Abs(ptr3[num4]->X);
				float num8 = MathF.Abs(ptr3[num4]->Y);
				float num9 = MathF.Abs(ptr3[num4]->Z);
				uint num10 = ((num7 < num8) ? ((!(num8 < num9)) ? ((!(num7 < num9)) ? 2u : 0u) : 0u) : ((num7 < num9) ? 1u : ((num8 < num9) ? 1u : 2u)));
				*ptr3[num5] = Vector3.Cross(*ptr3[num4], ptr4[num10]);
			}
			*ptr3[num5] = Vector3.Normalize(*ptr3[num5]);
			if (ptr2[num6] < 0.0001f)
			{
				*ptr3[num6] = Vector3.Cross(*ptr3[num4], *ptr3[num5]);
			}
			*ptr3[num6] = Vector3.Normalize(*ptr3[num6]);
			float num11 = identity.GetDeterminant();
			if (num11 < 0f)
			{
				ptr2[num4] = 0f - ptr2[num4];
				*ptr3[num4] = -(*ptr3[num4]);
				num11 = 0f - num11;
			}
			num11 -= 1f;
			num11 *= num11;
			if (0.0001f < num11)
			{
				rotation = Quaternion.Identity;
				result = false;
			}
			else
			{
				rotation = Quaternion.CreateFromRotationMatrix(identity);
			}
		}
		return result;
	}

	public static Matrix4x4 Transform(Matrix4x4 value, Quaternion rotation)
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
		float num13 = 1f - num10 - num12;
		float num14 = num8 - num6;
		float num15 = num9 + num5;
		float num16 = num8 + num6;
		float num17 = 1f - num7 - num12;
		float num18 = num11 - num4;
		float num19 = num9 - num5;
		float num20 = num11 + num4;
		float num21 = 1f - num7 - num10;
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = value.M11 * num13 + value.M12 * num14 + value.M13 * num15;
		result.M12 = value.M11 * num16 + value.M12 * num17 + value.M13 * num18;
		result.M13 = value.M11 * num19 + value.M12 * num20 + value.M13 * num21;
		result.M14 = value.M14;
		result.M21 = value.M21 * num13 + value.M22 * num14 + value.M23 * num15;
		result.M22 = value.M21 * num16 + value.M22 * num17 + value.M23 * num18;
		result.M23 = value.M21 * num19 + value.M22 * num20 + value.M23 * num21;
		result.M24 = value.M24;
		result.M31 = value.M31 * num13 + value.M32 * num14 + value.M33 * num15;
		result.M32 = value.M31 * num16 + value.M32 * num17 + value.M33 * num18;
		result.M33 = value.M31 * num19 + value.M32 * num20 + value.M33 * num21;
		result.M34 = value.M34;
		result.M41 = value.M41 * num13 + value.M42 * num14 + value.M43 * num15;
		result.M42 = value.M41 * num16 + value.M42 * num17 + value.M43 * num18;
		result.M43 = value.M41 * num19 + value.M42 * num20 + value.M43 * num21;
		result.M44 = value.M44;
		return result;
	}

	public static Matrix4x4 Transpose(Matrix4x4 matrix)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = matrix.M11;
		result.M12 = matrix.M21;
		result.M13 = matrix.M31;
		result.M14 = matrix.M41;
		result.M21 = matrix.M12;
		result.M22 = matrix.M22;
		result.M23 = matrix.M32;
		result.M24 = matrix.M42;
		result.M31 = matrix.M13;
		result.M32 = matrix.M23;
		result.M33 = matrix.M33;
		result.M34 = matrix.M43;
		result.M41 = matrix.M14;
		result.M42 = matrix.M24;
		result.M43 = matrix.M34;
		result.M44 = matrix.M44;
		return result;
	}

	public static Matrix4x4 Lerp(Matrix4x4 matrix1, Matrix4x4 matrix2, float amount)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = matrix1.M11 + (matrix2.M11 - matrix1.M11) * amount;
		result.M12 = matrix1.M12 + (matrix2.M12 - matrix1.M12) * amount;
		result.M13 = matrix1.M13 + (matrix2.M13 - matrix1.M13) * amount;
		result.M14 = matrix1.M14 + (matrix2.M14 - matrix1.M14) * amount;
		result.M21 = matrix1.M21 + (matrix2.M21 - matrix1.M21) * amount;
		result.M22 = matrix1.M22 + (matrix2.M22 - matrix1.M22) * amount;
		result.M23 = matrix1.M23 + (matrix2.M23 - matrix1.M23) * amount;
		result.M24 = matrix1.M24 + (matrix2.M24 - matrix1.M24) * amount;
		result.M31 = matrix1.M31 + (matrix2.M31 - matrix1.M31) * amount;
		result.M32 = matrix1.M32 + (matrix2.M32 - matrix1.M32) * amount;
		result.M33 = matrix1.M33 + (matrix2.M33 - matrix1.M33) * amount;
		result.M34 = matrix1.M34 + (matrix2.M34 - matrix1.M34) * amount;
		result.M41 = matrix1.M41 + (matrix2.M41 - matrix1.M41) * amount;
		result.M42 = matrix1.M42 + (matrix2.M42 - matrix1.M42) * amount;
		result.M43 = matrix1.M43 + (matrix2.M43 - matrix1.M43) * amount;
		result.M44 = matrix1.M44 + (matrix2.M44 - matrix1.M44) * amount;
		return result;
	}

	public static Matrix4x4 Negate(Matrix4x4 value)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = 0f - value.M11;
		result.M12 = 0f - value.M12;
		result.M13 = 0f - value.M13;
		result.M14 = 0f - value.M14;
		result.M21 = 0f - value.M21;
		result.M22 = 0f - value.M22;
		result.M23 = 0f - value.M23;
		result.M24 = 0f - value.M24;
		result.M31 = 0f - value.M31;
		result.M32 = 0f - value.M32;
		result.M33 = 0f - value.M33;
		result.M34 = 0f - value.M34;
		result.M41 = 0f - value.M41;
		result.M42 = 0f - value.M42;
		result.M43 = 0f - value.M43;
		result.M44 = 0f - value.M44;
		return result;
	}

	public static Matrix4x4 Add(Matrix4x4 value1, Matrix4x4 value2)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = value1.M11 + value2.M11;
		result.M12 = value1.M12 + value2.M12;
		result.M13 = value1.M13 + value2.M13;
		result.M14 = value1.M14 + value2.M14;
		result.M21 = value1.M21 + value2.M21;
		result.M22 = value1.M22 + value2.M22;
		result.M23 = value1.M23 + value2.M23;
		result.M24 = value1.M24 + value2.M24;
		result.M31 = value1.M31 + value2.M31;
		result.M32 = value1.M32 + value2.M32;
		result.M33 = value1.M33 + value2.M33;
		result.M34 = value1.M34 + value2.M34;
		result.M41 = value1.M41 + value2.M41;
		result.M42 = value1.M42 + value2.M42;
		result.M43 = value1.M43 + value2.M43;
		result.M44 = value1.M44 + value2.M44;
		return result;
	}

	public static Matrix4x4 Subtract(Matrix4x4 value1, Matrix4x4 value2)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = value1.M11 - value2.M11;
		result.M12 = value1.M12 - value2.M12;
		result.M13 = value1.M13 - value2.M13;
		result.M14 = value1.M14 - value2.M14;
		result.M21 = value1.M21 - value2.M21;
		result.M22 = value1.M22 - value2.M22;
		result.M23 = value1.M23 - value2.M23;
		result.M24 = value1.M24 - value2.M24;
		result.M31 = value1.M31 - value2.M31;
		result.M32 = value1.M32 - value2.M32;
		result.M33 = value1.M33 - value2.M33;
		result.M34 = value1.M34 - value2.M34;
		result.M41 = value1.M41 - value2.M41;
		result.M42 = value1.M42 - value2.M42;
		result.M43 = value1.M43 - value2.M43;
		result.M44 = value1.M44 - value2.M44;
		return result;
	}

	public static Matrix4x4 Multiply(Matrix4x4 value1, Matrix4x4 value2)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = value1.M11 * value2.M11 + value1.M12 * value2.M21 + value1.M13 * value2.M31 + value1.M14 * value2.M41;
		result.M12 = value1.M11 * value2.M12 + value1.M12 * value2.M22 + value1.M13 * value2.M32 + value1.M14 * value2.M42;
		result.M13 = value1.M11 * value2.M13 + value1.M12 * value2.M23 + value1.M13 * value2.M33 + value1.M14 * value2.M43;
		result.M14 = value1.M11 * value2.M14 + value1.M12 * value2.M24 + value1.M13 * value2.M34 + value1.M14 * value2.M44;
		result.M21 = value1.M21 * value2.M11 + value1.M22 * value2.M21 + value1.M23 * value2.M31 + value1.M24 * value2.M41;
		result.M22 = value1.M21 * value2.M12 + value1.M22 * value2.M22 + value1.M23 * value2.M32 + value1.M24 * value2.M42;
		result.M23 = value1.M21 * value2.M13 + value1.M22 * value2.M23 + value1.M23 * value2.M33 + value1.M24 * value2.M43;
		result.M24 = value1.M21 * value2.M14 + value1.M22 * value2.M24 + value1.M23 * value2.M34 + value1.M24 * value2.M44;
		result.M31 = value1.M31 * value2.M11 + value1.M32 * value2.M21 + value1.M33 * value2.M31 + value1.M34 * value2.M41;
		result.M32 = value1.M31 * value2.M12 + value1.M32 * value2.M22 + value1.M33 * value2.M32 + value1.M34 * value2.M42;
		result.M33 = value1.M31 * value2.M13 + value1.M32 * value2.M23 + value1.M33 * value2.M33 + value1.M34 * value2.M43;
		result.M34 = value1.M31 * value2.M14 + value1.M32 * value2.M24 + value1.M33 * value2.M34 + value1.M34 * value2.M44;
		result.M41 = value1.M41 * value2.M11 + value1.M42 * value2.M21 + value1.M43 * value2.M31 + value1.M44 * value2.M41;
		result.M42 = value1.M41 * value2.M12 + value1.M42 * value2.M22 + value1.M43 * value2.M32 + value1.M44 * value2.M42;
		result.M43 = value1.M41 * value2.M13 + value1.M42 * value2.M23 + value1.M43 * value2.M33 + value1.M44 * value2.M43;
		result.M44 = value1.M41 * value2.M14 + value1.M42 * value2.M24 + value1.M43 * value2.M34 + value1.M44 * value2.M44;
		return result;
	}

	public static Matrix4x4 Multiply(Matrix4x4 value1, float value2)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = value1.M11 * value2;
		result.M12 = value1.M12 * value2;
		result.M13 = value1.M13 * value2;
		result.M14 = value1.M14 * value2;
		result.M21 = value1.M21 * value2;
		result.M22 = value1.M22 * value2;
		result.M23 = value1.M23 * value2;
		result.M24 = value1.M24 * value2;
		result.M31 = value1.M31 * value2;
		result.M32 = value1.M32 * value2;
		result.M33 = value1.M33 * value2;
		result.M34 = value1.M34 * value2;
		result.M41 = value1.M41 * value2;
		result.M42 = value1.M42 * value2;
		result.M43 = value1.M43 * value2;
		result.M44 = value1.M44 * value2;
		return result;
	}

	public static Matrix4x4 operator -(Matrix4x4 value)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = 0f - value.M11;
		result.M12 = 0f - value.M12;
		result.M13 = 0f - value.M13;
		result.M14 = 0f - value.M14;
		result.M21 = 0f - value.M21;
		result.M22 = 0f - value.M22;
		result.M23 = 0f - value.M23;
		result.M24 = 0f - value.M24;
		result.M31 = 0f - value.M31;
		result.M32 = 0f - value.M32;
		result.M33 = 0f - value.M33;
		result.M34 = 0f - value.M34;
		result.M41 = 0f - value.M41;
		result.M42 = 0f - value.M42;
		result.M43 = 0f - value.M43;
		result.M44 = 0f - value.M44;
		return result;
	}

	public static Matrix4x4 operator +(Matrix4x4 value1, Matrix4x4 value2)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = value1.M11 + value2.M11;
		result.M12 = value1.M12 + value2.M12;
		result.M13 = value1.M13 + value2.M13;
		result.M14 = value1.M14 + value2.M14;
		result.M21 = value1.M21 + value2.M21;
		result.M22 = value1.M22 + value2.M22;
		result.M23 = value1.M23 + value2.M23;
		result.M24 = value1.M24 + value2.M24;
		result.M31 = value1.M31 + value2.M31;
		result.M32 = value1.M32 + value2.M32;
		result.M33 = value1.M33 + value2.M33;
		result.M34 = value1.M34 + value2.M34;
		result.M41 = value1.M41 + value2.M41;
		result.M42 = value1.M42 + value2.M42;
		result.M43 = value1.M43 + value2.M43;
		result.M44 = value1.M44 + value2.M44;
		return result;
	}

	public static Matrix4x4 operator -(Matrix4x4 value1, Matrix4x4 value2)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = value1.M11 - value2.M11;
		result.M12 = value1.M12 - value2.M12;
		result.M13 = value1.M13 - value2.M13;
		result.M14 = value1.M14 - value2.M14;
		result.M21 = value1.M21 - value2.M21;
		result.M22 = value1.M22 - value2.M22;
		result.M23 = value1.M23 - value2.M23;
		result.M24 = value1.M24 - value2.M24;
		result.M31 = value1.M31 - value2.M31;
		result.M32 = value1.M32 - value2.M32;
		result.M33 = value1.M33 - value2.M33;
		result.M34 = value1.M34 - value2.M34;
		result.M41 = value1.M41 - value2.M41;
		result.M42 = value1.M42 - value2.M42;
		result.M43 = value1.M43 - value2.M43;
		result.M44 = value1.M44 - value2.M44;
		return result;
	}

	public static Matrix4x4 operator *(Matrix4x4 value1, Matrix4x4 value2)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = value1.M11 * value2.M11 + value1.M12 * value2.M21 + value1.M13 * value2.M31 + value1.M14 * value2.M41;
		result.M12 = value1.M11 * value2.M12 + value1.M12 * value2.M22 + value1.M13 * value2.M32 + value1.M14 * value2.M42;
		result.M13 = value1.M11 * value2.M13 + value1.M12 * value2.M23 + value1.M13 * value2.M33 + value1.M14 * value2.M43;
		result.M14 = value1.M11 * value2.M14 + value1.M12 * value2.M24 + value1.M13 * value2.M34 + value1.M14 * value2.M44;
		result.M21 = value1.M21 * value2.M11 + value1.M22 * value2.M21 + value1.M23 * value2.M31 + value1.M24 * value2.M41;
		result.M22 = value1.M21 * value2.M12 + value1.M22 * value2.M22 + value1.M23 * value2.M32 + value1.M24 * value2.M42;
		result.M23 = value1.M21 * value2.M13 + value1.M22 * value2.M23 + value1.M23 * value2.M33 + value1.M24 * value2.M43;
		result.M24 = value1.M21 * value2.M14 + value1.M22 * value2.M24 + value1.M23 * value2.M34 + value1.M24 * value2.M44;
		result.M31 = value1.M31 * value2.M11 + value1.M32 * value2.M21 + value1.M33 * value2.M31 + value1.M34 * value2.M41;
		result.M32 = value1.M31 * value2.M12 + value1.M32 * value2.M22 + value1.M33 * value2.M32 + value1.M34 * value2.M42;
		result.M33 = value1.M31 * value2.M13 + value1.M32 * value2.M23 + value1.M33 * value2.M33 + value1.M34 * value2.M43;
		result.M34 = value1.M31 * value2.M14 + value1.M32 * value2.M24 + value1.M33 * value2.M34 + value1.M34 * value2.M44;
		result.M41 = value1.M41 * value2.M11 + value1.M42 * value2.M21 + value1.M43 * value2.M31 + value1.M44 * value2.M41;
		result.M42 = value1.M41 * value2.M12 + value1.M42 * value2.M22 + value1.M43 * value2.M32 + value1.M44 * value2.M42;
		result.M43 = value1.M41 * value2.M13 + value1.M42 * value2.M23 + value1.M43 * value2.M33 + value1.M44 * value2.M43;
		result.M44 = value1.M41 * value2.M14 + value1.M42 * value2.M24 + value1.M43 * value2.M34 + value1.M44 * value2.M44;
		return result;
	}

	public static Matrix4x4 operator *(Matrix4x4 value1, float value2)
	{
		Matrix4x4 result = default(Matrix4x4);
		result.M11 = value1.M11 * value2;
		result.M12 = value1.M12 * value2;
		result.M13 = value1.M13 * value2;
		result.M14 = value1.M14 * value2;
		result.M21 = value1.M21 * value2;
		result.M22 = value1.M22 * value2;
		result.M23 = value1.M23 * value2;
		result.M24 = value1.M24 * value2;
		result.M31 = value1.M31 * value2;
		result.M32 = value1.M32 * value2;
		result.M33 = value1.M33 * value2;
		result.M34 = value1.M34 * value2;
		result.M41 = value1.M41 * value2;
		result.M42 = value1.M42 * value2;
		result.M43 = value1.M43 * value2;
		result.M44 = value1.M44 * value2;
		return result;
	}

	public static bool operator ==(Matrix4x4 value1, Matrix4x4 value2)
	{
		if (value1.M11 == value2.M11 && value1.M22 == value2.M22 && value1.M33 == value2.M33 && value1.M44 == value2.M44 && value1.M12 == value2.M12 && value1.M13 == value2.M13 && value1.M14 == value2.M14 && value1.M21 == value2.M21 && value1.M23 == value2.M23 && value1.M24 == value2.M24 && value1.M31 == value2.M31 && value1.M32 == value2.M32 && value1.M34 == value2.M34 && value1.M41 == value2.M41 && value1.M42 == value2.M42)
		{
			return value1.M43 == value2.M43;
		}
		return false;
	}

	public static bool operator !=(Matrix4x4 value1, Matrix4x4 value2)
	{
		if (value1.M11 == value2.M11 && value1.M12 == value2.M12 && value1.M13 == value2.M13 && value1.M14 == value2.M14 && value1.M21 == value2.M21 && value1.M22 == value2.M22 && value1.M23 == value2.M23 && value1.M24 == value2.M24 && value1.M31 == value2.M31 && value1.M32 == value2.M32 && value1.M33 == value2.M33 && value1.M34 == value2.M34 && value1.M41 == value2.M41 && value1.M42 == value2.M42 && value1.M43 == value2.M43)
		{
			return value1.M44 != value2.M44;
		}
		return true;
	}

	public bool Equals(Matrix4x4 other)
	{
		if (M11 == other.M11 && M22 == other.M22 && M33 == other.M33 && M44 == other.M44 && M12 == other.M12 && M13 == other.M13 && M14 == other.M14 && M21 == other.M21 && M23 == other.M23 && M24 == other.M24 && M31 == other.M31 && M32 == other.M32 && M34 == other.M34 && M41 == other.M41 && M42 == other.M42)
		{
			return M43 == other.M43;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is Matrix4x4)
		{
			return Equals((Matrix4x4)obj);
		}
		return false;
	}

	public override string ToString()
	{
		CultureInfo currentCulture = CultureInfo.CurrentCulture;
		return string.Format(currentCulture, "{{ {{M11:{0} M12:{1} M13:{2} M14:{3}}} {{M21:{4} M22:{5} M23:{6} M24:{7}}} {{M31:{8} M32:{9} M33:{10} M34:{11}}} {{M41:{12} M42:{13} M43:{14} M44:{15}}} }}", M11.ToString(currentCulture), M12.ToString(currentCulture), M13.ToString(currentCulture), M14.ToString(currentCulture), M21.ToString(currentCulture), M22.ToString(currentCulture), M23.ToString(currentCulture), M24.ToString(currentCulture), M31.ToString(currentCulture), M32.ToString(currentCulture), M33.ToString(currentCulture), M34.ToString(currentCulture), M41.ToString(currentCulture), M42.ToString(currentCulture), M43.ToString(currentCulture), M44.ToString(currentCulture));
	}

	public override int GetHashCode()
	{
		return M11.GetHashCode() + M12.GetHashCode() + M13.GetHashCode() + M14.GetHashCode() + M21.GetHashCode() + M22.GetHashCode() + M23.GetHashCode() + M24.GetHashCode() + M31.GetHashCode() + M32.GetHashCode() + M33.GetHashCode() + M34.GetHashCode() + M41.GetHashCode() + M42.GetHashCode() + M43.GetHashCode() + M44.GetHashCode();
	}
}
