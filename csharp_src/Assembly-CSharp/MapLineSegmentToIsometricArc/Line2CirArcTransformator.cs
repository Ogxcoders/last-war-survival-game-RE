using System;
using System.Numerics;
using UnityEngine;

namespace MapLineSegmentToIsometricArc;

public class Line2CirArcTransformator
{
	public Complex A1 { get; private set; }

	public Complex A { get; private set; }

	public Complex LineMidPoint { get; private set; }

	public Complex B { get; private set; }

	public Complex B1 { get; private set; }

	public Complex BBMidPoint { get; private set; }

	public Complex O { get; private set; }

	public double Radius { get; private set; }

	private Complex N { get; set; }

	private double InversionCircleRadius { get; set; }

	private Complex FactorForP { get; set; }

	public Line2CirArcTransformator(System.Numerics.Vector2 A, System.Numerics.Vector2 A1, System.Numerics.Vector2 O, System.Numerics.Vector2 MapMidPoint)
	{
		Line2CirArcTransformator line2CirArcTransformator = this;
		float Radius = (O - MapMidPoint).Length();
		this.Radius = Radius;
		float num = (A - A1).Length();
		CalculateEndPoints(num);
		Initialize(A, A1, O, num > MathF.PI * Radius);
		Console.WriteLine("B: " + B);
		Console.WriteLine("B1:" + B1);
		void CalculateEndPoints(float length)
		{
			float lineHalfLength = length / 2f;
			B = VectorAsComplex(CalculateEndPoint(clockwise: true));
			B1 = VectorAsComplex(CalculateEndPoint(clockwise: false));
			System.Numerics.Vector2 CalculateEndPoint(bool clockwise)
			{
				float num2 = Mathf.Atan2(MapMidPoint.Y - O.Y, MapMidPoint.X - O.X);
				num2 = ((!clockwise) ? (num2 + lineHalfLength / Radius) : (num2 - lineHalfLength / Radius));
				return new System.Numerics.Vector2(O.X + Radius * Mathf.Cos(num2), O.Y + Radius * Mathf.Sin(num2));
			}
		}
	}

	public Line2CirArcTransformator(System.Numerics.Vector2 A, System.Numerics.Vector2 A1, System.Numerics.Vector2 B, System.Numerics.Vector2 B1, float Radius, bool isMajorArc = false)
	{
		this.Radius = Radius;
		this.B = VectorAsComplex(B);
		this.B1 = VectorAsComplex(B1);
		Initialize(A, A1, CalculateO(), isMajorArc);
		Console.WriteLine("B: " + B);
		Console.WriteLine("B1:" + B1);
		System.Numerics.Vector2 CalculateO()
		{
			float num = (B1.X - B.X) / 2f;
			float num2 = (B1.Y - B.Y) / 2f;
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			float num4 = Mathf.Sqrt(Radius * Radius - num3 * num3);
			return new System.Numerics.Vector2((B.X + B1.X) / 2f + num4 * num2 / num3, (B.Y + B1.Y) / 2f - num4 * num / num3);
		}
	}

	private void Initialize(System.Numerics.Vector2 A, System.Numerics.Vector2 A1, System.Numerics.Vector2 O, bool isMajorArc)
	{
		this.A1 = VectorAsComplex(A);
		this.A = VectorAsComplex(A1);
		this.O = VectorAsComplex(O);
		LineMidPoint = VectorAsComplex((A + A1) / 2f);
		BBMidPoint = (B + B1) / 2;
		N = CalculateN();
		InversionCircleRadius = (B - N).Magnitude;
		FactorForP = (B1 - B) / (this.A1 - this.A);
		Complex CalculateN()
		{
			return this.O + (double)((isMajorArc ? 1 : (-1)) * 2) * Radius / Math.Sqrt(4.0 * Radius * Radius - Math.Pow((B - B1).Magnitude, 2.0)) * (BBMidPoint - this.O);
		}
	}

	private Complex VectorAsComplex(System.Numerics.Vector2 vector)
	{
		return new Complex(vector.X, vector.Y);
	}

	public System.Numerics.Vector2 MapLinePoint(System.Numerics.Vector2 point)
	{
		Complex complex = VectorAsComplex(point);
		Complex value = BBMidPoint + FactorForP * (complex - LineMidPoint);
		Complex complex2 = N + InversionCircleRadius * InversionCircleRadius / (Complex.Conjugate(value) - Complex.Conjugate(N));
		return new System.Numerics.Vector2((float)complex2.Real, (float)complex2.Imaginary);
	}
}
