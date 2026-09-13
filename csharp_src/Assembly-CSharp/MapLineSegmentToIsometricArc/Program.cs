using System;
using System.Numerics;
using UnityEngine;

namespace MapLineSegmentToIsometricArc;

internal class Program
{
	private static void Main(string[] args)
	{
		System.Numerics.Vector2 a = new System.Numerics.Vector2(2f, -4f);
		System.Numerics.Vector2 a2 = new System.Numerics.Vector2(2f, 4f);
		System.Numerics.Vector2 o = new System.Numerics.Vector2(0f, 0f);
		System.Numerics.Vector2 b = new System.Numerics.Vector2(Mathf.Sqrt(2f) / 2f, Mathf.Sqrt(2f) / 2f);
		System.Numerics.Vector2 b2 = new System.Numerics.Vector2(Mathf.Sqrt(2f) / 2f, (0f - Mathf.Sqrt(2f)) / 2f);
		Line2CirArcTransformator line2CirArcTransformator = new Line2CirArcTransformator(a, a2, b, b2, 1f);
		Console.WriteLine(line2CirArcTransformator.MapLinePoint(new System.Numerics.Vector2(2f, 4f)));
		Console.WriteLine(line2CirArcTransformator.MapLinePoint(new System.Numerics.Vector2(2f, 0f)));
		Console.WriteLine(line2CirArcTransformator.MapLinePoint(new System.Numerics.Vector2(2f, -4f)));
		Line2CirArcTransformator line2CirArcTransformator2 = new Line2CirArcTransformator(a, a2, o, new System.Numerics.Vector2(1f, 0f));
		Console.WriteLine(line2CirArcTransformator2.MapLinePoint(new System.Numerics.Vector2(2f, 4f)));
		Console.WriteLine(line2CirArcTransformator2.MapLinePoint(new System.Numerics.Vector2(2f, 0f)));
		Console.WriteLine(line2CirArcTransformator2.MapLinePoint(new System.Numerics.Vector2(2f, -4f)));
	}
}
