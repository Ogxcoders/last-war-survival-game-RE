using System;
using System.Runtime.CompilerServices;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Collision;

public static class CollisionUtils
{
	public struct EPAxis
	{
		public enum EPAxisType
		{
			Unknown,
			EdgeA,
			EdgeB
		}

		public FVector2 Normal;

		public EPAxisType Type;

		public int Index;

		public FP Separation;
	}

	public struct TempPolygon
	{
		public FixedArray8<FVector2> Vertices;

		public FixedArray8<FVector2> Normals;

		public int Count;
	}

	private struct ReferenceFace
	{
		public int I1;

		public int I2;

		public FVector2 Normal;

		public FVector2 SideNormal1;

		public FVector2 SideNormal2;

		public FP SideOffset1;

		public FP SideOffset2;

		public FVector2 V1;

		public FVector2 V2;
	}

	private static FP k_relativeTol = FP.FromRaw(4209067950L);

	private static FP k_absoluteTol = FP.FromRaw(4294967L);

	private static FP sinTol = FP.FromRaw(429496729L);

	private static readonly FP k_tol = (FP)0.1f * Settings.LinearSlop;

	public static void CollideCircles(ref Manifold manifold, CircleShape circleA, in Transform xfA, CircleShape circleB, in Transform xfB)
	{
		manifold.PointCount = 0;
		FVector2 fVector = MathUtils.Mul(in xfA, in circleA.Position);
		FVector2 fVector2 = MathUtils.Mul(in xfB, in circleB.Position) - fVector;
		FP fP = FVector2.Dot(fVector2, fVector2);
		FP x = circleA.Radius + circleB.Radius;
		if (!(fP > x * x))
		{
			manifold.Type = ManifoldType.Circles;
			manifold.LocalPoint = circleA.Position;
			manifold.LocalNormal.SetZero();
			manifold.PointCount = 1;
			manifold.Points.Value0.LocalPoint = circleB.Position;
			manifold.Points.Value0.Id.Key = 0u;
		}
	}

	public static void CollidePolygonAndCircle(ref Manifold manifold, PolygonShape polygonA, in Transform xfA, CircleShape circleB, in Transform xfB)
	{
		manifold.PointCount = 0;
		FVector2 fVector = MathUtils.MulT(in xfA, MathUtils.Mul(in xfB, in circleB.Position));
		int num = 0;
		FP fP = Settings.MinFloat;
		FP x = polygonA.Radius + circleB.Radius;
		int count = polygonA.Count;
		FVector2[] vertices = polygonA.Vertices;
		FVector2[] normals = polygonA.Normals;
		for (int i = 0; i < count; i++)
		{
			FP fP2 = FVector2.Dot(normals[i], fVector - vertices[i]);
			if (fP2 > x)
			{
				return;
			}
			if (fP2 > fP)
			{
				fP = fP2;
				num = i;
			}
		}
		int num2 = num;
		int num3 = ((num2 + 1 < count) ? (num2 + 1) : 0);
		FVector2 fVector2 = vertices[num2];
		FVector2 fVector3 = vertices[num3];
		if (fP < Settings.Epsilon)
		{
			manifold.PointCount = 1;
			manifold.Type = ManifoldType.FaceA;
			manifold.LocalNormal = normals[num];
			manifold.LocalPoint = 0.5f * (fVector2 + fVector3);
			manifold.Points.Value0.LocalPoint = circleB.Position;
			manifold.Points.Value0.Id.Key = 0u;
			return;
		}
		FP fP3 = FVector2.Dot(fVector - fVector2, fVector3 - fVector2);
		FP fP4 = FVector2.Dot(fVector - fVector3, fVector2 - fVector3);
		if (fP3 <= 0f)
		{
			if (!(FVector2.DistanceSquared(fVector, fVector2) > x * x))
			{
				manifold.PointCount = 1;
				manifold.Type = ManifoldType.FaceA;
				manifold.LocalNormal = fVector - fVector2;
				manifold.LocalNormal.Normalize();
				manifold.LocalPoint = fVector2;
				manifold.Points.Value0.LocalPoint = circleB.Position;
				manifold.Points.Value0.Id.Key = 0u;
			}
		}
		else if (fP4 <= 0f)
		{
			if (!(FVector2.DistanceSquared(fVector, fVector3) > x * x))
			{
				manifold.PointCount = 1;
				manifold.Type = ManifoldType.FaceA;
				manifold.LocalNormal = fVector - fVector3;
				manifold.LocalNormal.Normalize();
				manifold.LocalPoint = fVector3;
				manifold.Points.Value0.LocalPoint = circleB.Position;
				manifold.Points.Value0.Id.Key = 0u;
			}
		}
		else
		{
			FVector2 fVector4 = 0.5f * (fVector2 + fVector3);
			if (!(FVector2.Dot(fVector - fVector4, normals[num2]) > x))
			{
				manifold.PointCount = 1;
				manifold.Type = ManifoldType.FaceA;
				manifold.LocalNormal = normals[num2];
				manifold.LocalPoint = fVector4;
				manifold.Points.Value0.LocalPoint = circleB.Position;
				manifold.Points.Value0.Id.Key = 0u;
			}
		}
	}

	public static void CollideEdgeAndCircle(ref Manifold manifold, EdgeShape edgeA, in Transform xfA, CircleShape circleB, in Transform xfB)
	{
		manifold.PointCount = 0;
		FVector2 fVector = MathUtils.MulT(in xfA, MathUtils.Mul(in xfB, in circleB.Position));
		FVector2 vertex = edgeA.Vertex1;
		FVector2 vertex2 = edgeA.Vertex2;
		FVector2 fVector2 = vertex2 - vertex;
		FVector2 fVector3 = new FVector2(fVector2.Y, -fVector2.X);
		FP fP = FVector2.Dot(fVector3, fVector - vertex);
		if (edgeA.OneSided && fP < 0f)
		{
			return;
		}
		FP fP2 = FVector2.Dot(fVector2, vertex2 - fVector);
		FP fP3 = FVector2.Dot(fVector2, fVector - vertex);
		FP x = edgeA.Radius + circleB.Radius;
		ContactFeature contactFeature = new ContactFeature
		{
			IndexB = 0,
			TypeB = 0
		};
		if (fP3 <= 0f)
		{
			FVector2 fVector4 = vertex;
			FVector2 fVector5 = fVector - fVector4;
			if (FVector2.Dot(fVector5, fVector5) > x * x)
			{
				return;
			}
			if (edgeA.OneSided)
			{
				FVector2 vertex3 = edgeA.Vertex0;
				FVector2 fVector6 = vertex;
				if (FVector2.Dot(fVector6 - vertex3, fVector6 - fVector) > 0f)
				{
					return;
				}
			}
			contactFeature.IndexA = 0;
			contactFeature.TypeA = 0;
			manifold.PointCount = 1;
			manifold.Type = ManifoldType.Circles;
			manifold.LocalNormal.SetZero();
			manifold.LocalPoint = fVector4;
			ref ManifoldPoint value = ref manifold.Points.Value0;
			value.Id.Key = 0u;
			value.Id.ContactFeature = contactFeature;
			value.LocalPoint = circleB.Position;
			return;
		}
		if (fP2 <= 0f)
		{
			FVector2 fVector7 = vertex2;
			FVector2 fVector8 = fVector - fVector7;
			if (FVector2.Dot(fVector8, fVector8) > x * x)
			{
				return;
			}
			if (edgeA.OneSided)
			{
				FVector2 vertex4 = edgeA.Vertex3;
				FVector2 fVector9 = vertex2;
				if (FVector2.Dot(vertex4 - fVector9, fVector - fVector9) > 0f)
				{
					return;
				}
			}
			contactFeature.IndexA = 1;
			contactFeature.TypeA = 0;
			manifold.PointCount = 1;
			manifold.Type = ManifoldType.Circles;
			manifold.LocalNormal.SetZero();
			manifold.LocalPoint = fVector7;
			ref ManifoldPoint value2 = ref manifold.Points.Value0;
			value2.Id.Key = 0u;
			value2.Id.ContactFeature = contactFeature;
			value2.LocalPoint = circleB.Position;
			return;
		}
		FP fP4 = FVector2.Dot(fVector2, fVector2);
		FVector2 fVector10 = 1f / fP4 * (fP2 * vertex + fP3 * vertex2);
		FVector2 fVector11 = fVector - fVector10;
		if (!(FVector2.Dot(fVector11, fVector11) > x * x))
		{
			if (fP < 0f)
			{
				fVector3.Set(-fVector3.X, -fVector3.Y);
			}
			fVector3.Normalize();
			contactFeature.IndexA = 0;
			contactFeature.TypeA = 1;
			manifold.PointCount = 1;
			manifold.Type = ManifoldType.FaceA;
			manifold.LocalNormal = fVector3;
			manifold.LocalPoint = vertex;
			ref ManifoldPoint value3 = ref manifold.Points.Value0;
			value3.Id.Key = 0u;
			value3.Id.ContactFeature = contactFeature;
			value3.LocalPoint = circleB.Position;
		}
	}

	private static EPAxis ComputeEdgeSeparation(in TempPolygon polygonB, in FVector2 v1, FVector2 normal1)
	{
		EPAxis result = new EPAxis
		{
			Type = EPAxis.EPAxisType.EdgeA,
			Index = -1,
			Separation = Settings.MinFloat,
			Normal = default(FVector2)
		};
		FVector2[] array = new FVector2[2]
		{
			normal1,
			-normal1
		};
		for (int i = 0; i < 2; i++)
		{
			FP fP = Settings.MaxFloat;
			for (int j = 0; j < polygonB.Count; j++)
			{
				FP fP2 = FVector2.Dot(array[i], polygonB.Vertices[j] - v1);
				if (fP2 < fP)
				{
					fP = fP2;
				}
			}
			if (fP > result.Separation)
			{
				result.Index = i;
				result.Separation = fP;
				result.Normal = array[i];
			}
		}
		return result;
	}

	private static EPAxis ComputePolygonSeparation(in TempPolygon polygonB, in FVector2 v1, in FVector2 v2)
	{
		EPAxis result = new EPAxis
		{
			Type = EPAxis.EPAxisType.Unknown,
			Index = -1,
			Separation = Settings.MinFloat,
			Normal = default(FVector2)
		};
		for (int i = 0; i < polygonB.Count; i++)
		{
			FVector2 fVector = -polygonB.Normals[i];
			FP left = FVector2.Dot(fVector, polygonB.Vertices[i] - v1);
			FP righ = FVector2.Dot(fVector, polygonB.Vertices[i] - v2);
			FP fP = FP.Min(left, righ);
			if (fP > result.Separation)
			{
				result.Type = EPAxis.EPAxisType.EdgeB;
				result.Index = i;
				result.Separation = fP;
				result.Normal = fVector;
			}
		}
		return result;
	}

	public static void CollideEdgeAndPolygon(ref Manifold manifold, EdgeShape edgeA, Transform xfA, PolygonShape polygonB, in Transform xfB)
	{
		manifold.PointCount = 0;
		Transform T = MathUtils.MulT(in xfA, in xfB);
		FVector2 fVector = MathUtils.Mul(in T, in polygonB.Centroid);
		FVector2 v = edgeA.Vertex1;
		FVector2 v2 = edgeA.Vertex2;
		FVector2 b = v2 - v;
		b.Normalize();
		FVector2 fVector2 = new FVector2(b.Y, -b.X);
		FP fP = FVector2.Dot(fVector2, fVector - v);
		bool oneSided = edgeA.OneSided;
		if (oneSided && fP < 0f)
		{
			return;
		}
		TempPolygon polygonB2 = new TempPolygon
		{
			Count = polygonB.Count
		};
		for (int i = 0; i < polygonB.Count; i++)
		{
			polygonB2.Vertices[i] = MathUtils.Mul(in T, in polygonB.Vertices[i]);
			polygonB2.Normals[i] = MathUtils.Mul(in T.Rotation, in polygonB.Normals[i]);
		}
		FP y = polygonB.Radius + edgeA.Radius;
		EPAxis ePAxis = ComputeEdgeSeparation(in polygonB2, in v, fVector2);
		if (ePAxis.Separation > y)
		{
			return;
		}
		EPAxis ePAxis2 = ComputePolygonSeparation(in polygonB2, in v, in v2);
		if (ePAxis2.Separation > y)
		{
			return;
		}
		EPAxis ePAxis3 = default(EPAxis);
		ePAxis3 = ((!(ePAxis3.Separation - y > k_relativeTol * (ePAxis.Separation - y) + k_absoluteTol)) ? ePAxis : ePAxis2);
		if (oneSided)
		{
			FVector2 a = v - edgeA.Vertex0;
			a.Normalize();
			FVector2 b2 = new FVector2(a.Y, -a.X);
			bool flag = MathUtils.Cross(in a, in b) >= 0f;
			FVector2 b3 = edgeA.Vertex3 - v2;
			b3.Normalize();
			FVector2 a2 = new FVector2(b3.Y, -b3.X);
			bool flag2 = MathUtils.Cross(in b, in b3) >= 0f;
			if (FVector2.Dot(ePAxis3.Normal, b) <= 0f)
			{
				if (flag)
				{
					if (MathUtils.Cross(in ePAxis3.Normal, in b2) > sinTol)
					{
						return;
					}
				}
				else
				{
					ePAxis3 = ePAxis;
				}
			}
			else if (flag2)
			{
				if (MathUtils.Cross(in a2, in ePAxis3.Normal) > sinTol)
				{
					return;
				}
			}
			else
			{
				ePAxis3 = ePAxis;
			}
		}
		ClipVertex[] array = new ClipVertex[2];
		ReferenceFace referenceFace = default(ReferenceFace);
		if (ePAxis3.Type == EPAxis.EPAxisType.EdgeA)
		{
			manifold.Type = ManifoldType.FaceA;
			int num = 0;
			FP fP2 = FVector2.Dot(ePAxis3.Normal, polygonB2.Normals[0]);
			for (int j = 1; j < polygonB2.Count; j++)
			{
				FP fP3 = FVector2.Dot(ePAxis3.Normal, polygonB2.Normals[j]);
				if (fP3 < fP2)
				{
					fP2 = fP3;
					num = j;
				}
			}
			int num2 = num;
			int num3 = ((num2 + 1 < polygonB2.Count) ? (num2 + 1) : 0);
			array[0].Vector = polygonB2.Vertices[num2];
			array[0].Id.ContactFeature.IndexA = 0;
			array[0].Id.ContactFeature.IndexB = (byte)num2;
			array[0].Id.ContactFeature.TypeA = 1;
			array[0].Id.ContactFeature.TypeB = 0;
			array[1].Vector = polygonB2.Vertices[num3];
			array[1].Id.ContactFeature.IndexA = 0;
			array[1].Id.ContactFeature.IndexB = (byte)num3;
			array[1].Id.ContactFeature.TypeA = 1;
			array[1].Id.ContactFeature.TypeB = 0;
			referenceFace.I1 = 0;
			referenceFace.I2 = 1;
			referenceFace.V1 = v;
			referenceFace.V2 = v2;
			referenceFace.Normal = ePAxis3.Normal;
			referenceFace.SideNormal1 = -b;
			referenceFace.SideNormal2 = b;
		}
		else
		{
			manifold.Type = ManifoldType.FaceB;
			array[0].Vector = v2;
			array[0].Id.ContactFeature.IndexA = 1;
			array[0].Id.ContactFeature.IndexB = (byte)ePAxis3.Index;
			array[0].Id.ContactFeature.TypeA = 0;
			array[0].Id.ContactFeature.TypeB = 1;
			array[1].Vector = v;
			array[1].Id.ContactFeature.IndexA = 0;
			array[1].Id.ContactFeature.IndexB = (byte)ePAxis3.Index;
			array[1].Id.ContactFeature.TypeA = 0;
			array[1].Id.ContactFeature.TypeB = 1;
			referenceFace.I1 = ePAxis3.Index;
			referenceFace.I2 = ((referenceFace.I1 + 1 < polygonB2.Count) ? (referenceFace.I1 + 1) : 0);
			referenceFace.V1 = polygonB2.Vertices[referenceFace.I1];
			referenceFace.V2 = polygonB2.Vertices[referenceFace.I2];
			referenceFace.Normal = polygonB2.Normals[referenceFace.I1];
			referenceFace.SideNormal1.Set(referenceFace.Normal.Y, -referenceFace.Normal.X);
			referenceFace.SideNormal2 = -referenceFace.SideNormal1;
		}
		referenceFace.SideOffset1 = FVector2.Dot(referenceFace.SideNormal1, referenceFace.V1);
		referenceFace.SideOffset2 = FVector2.Dot(referenceFace.SideNormal2, referenceFace.V2);
		Span<ClipVertex> vOut = stackalloc ClipVertex[2];
		Span<ClipVertex> vOut2 = stackalloc ClipVertex[2];
		if (ClipSegmentToLine(in vOut, (Span<ClipVertex>)array, in referenceFace.SideNormal1, referenceFace.SideOffset1, referenceFace.I1) < 2 || ClipSegmentToLine(in vOut2, in vOut, in referenceFace.SideNormal2, referenceFace.SideOffset2, referenceFace.I2) < 2)
		{
			return;
		}
		if (ePAxis3.Type == EPAxis.EPAxisType.EdgeA)
		{
			manifold.LocalNormal = referenceFace.Normal;
			manifold.LocalPoint = referenceFace.V1;
		}
		else
		{
			manifold.LocalNormal = polygonB.Normals[referenceFace.I1];
			manifold.LocalPoint = polygonB.Vertices[referenceFace.I1];
		}
		int num4 = 0;
		for (int k = 0; k < 2; k++)
		{
			if (FVector2.Dot(referenceFace.Normal, vOut2[k].Vector - referenceFace.V1) <= y)
			{
				ref ManifoldPoint reference = ref manifold.Points[num4];
				if (ePAxis3.Type == EPAxis.EPAxisType.EdgeA)
				{
					reference.LocalPoint = MathUtils.MulT(in T, in vOut2[k].Vector);
					reference.Id = vOut2[k].Id;
				}
				else
				{
					reference.LocalPoint = vOut2[k].Vector;
					reference.Id.ContactFeature.TypeA = vOut2[k].Id.ContactFeature.TypeB;
					reference.Id.ContactFeature.TypeB = vOut2[k].Id.ContactFeature.TypeA;
					reference.Id.ContactFeature.IndexA = vOut2[k].Id.ContactFeature.IndexB;
					reference.Id.ContactFeature.IndexB = vOut2[k].Id.ContactFeature.IndexA;
				}
				num4++;
			}
		}
		manifold.PointCount = num4;
	}

	public static FP FindMaxSeparation(out int edgeIndex, PolygonShape poly1, in Transform xf1, PolygonShape poly2, in Transform xf2)
	{
		int count = poly1.Count;
		int count2 = poly2.Count;
		Span<FVector2> span = poly1.Normals;
		Span<FVector2> span2 = poly1.Vertices;
		Span<FVector2> span3 = poly2.Vertices;
		FP y = xf1.Position.X - xf2.Position.X;
		FP y2 = xf1.Position.Y - xf2.Position.Y;
		FP y3 = xf2.Rotation.Cos * y + xf2.Rotation.Sin * y2;
		FP y4 = -xf2.Rotation.Sin * y + xf2.Rotation.Cos * y2;
		FP x = xf2.Rotation.Cos * xf1.Rotation.Sin - xf2.Rotation.Sin * xf1.Rotation.Cos;
		FP x2 = xf2.Rotation.Cos * xf1.Rotation.Cos + xf2.Rotation.Sin * xf1.Rotation.Sin;
		int num = 0;
		FP fP = Settings.MinFloat;
		for (int i = 0; i < count; i++)
		{
			ref FVector2 reference = ref span[i];
			FP x3 = x2 * reference.X - x * reference.Y;
			FP x4 = x * reference.X + x2 * reference.Y;
			ref FVector2 reference2 = ref span2[i];
			FP y5 = x2 * reference2.X - x * reference2.Y + y3;
			FP y6 = x * reference2.X + x2 * reference2.Y + y4;
			FP fP2 = Settings.MaxFloat;
			for (int j = 0; j < count2; j++)
			{
				ref FVector2 reference3 = ref span3[j];
				FP fP3 = x3 * (reference3.X - y5) + x4 * (reference3.Y - y6);
				if (fP3 < fP2)
				{
					fP2 = fP3;
				}
			}
			if (fP2 > fP)
			{
				fP = fP2;
				num = i;
			}
		}
		edgeIndex = num;
		return fP;
	}

	public static void FindIncidentEdge(in Span<ClipVertex> c, PolygonShape poly1, in Transform xf1, int edge1, PolygonShape poly2, in Transform xf2)
	{
		FVector2[] normals = poly1.Normals;
		int count = poly2.Count;
		FVector2[] vertices = poly2.Vertices;
		FVector2[] normals2 = poly2.Normals;
		ref FVector2 reference = ref normals[edge1];
		FVector2 fVector = new FVector2(xf1.Rotation.Cos * reference.X - xf1.Rotation.Sin * reference.Y, xf1.Rotation.Sin * reference.X + xf1.Rotation.Cos * reference.Y);
		FVector2 value = new FVector2(xf2.Rotation.Cos * fVector.X + xf2.Rotation.Sin * fVector.Y, -xf2.Rotation.Sin * fVector.X + xf2.Rotation.Cos * fVector.Y);
		int num = 0;
		FP fP = Settings.MaxFloat;
		for (int i = 0; i < count; i++)
		{
			FP fP2 = FVector2.Dot(value, normals2[i]);
			if (fP2 < fP)
			{
				fP = fP2;
				num = i;
			}
		}
		int num2 = num;
		int num3 = ((num2 + 1 < count) ? (num2 + 1) : 0);
		ref ClipVertex reference2 = ref c[0];
		reference2.Vector = MathUtils.Mul(in xf2, in vertices[num2]);
		reference2.Id.ContactFeature.IndexA = (byte)edge1;
		reference2.Id.ContactFeature.IndexB = (byte)num2;
		reference2.Id.ContactFeature.TypeA = 1;
		reference2.Id.ContactFeature.TypeB = 0;
		ref ClipVertex reference3 = ref c[1];
		reference3.Vector = MathUtils.Mul(in xf2, in vertices[num3]);
		reference3.Id.ContactFeature.IndexA = (byte)edge1;
		reference3.Id.ContactFeature.IndexB = (byte)num3;
		reference3.Id.ContactFeature.TypeA = 1;
		reference3.Id.ContactFeature.TypeB = 0;
	}

	public static void CollidePolygons(ref Manifold manifold, PolygonShape polyA, in Transform xfA, PolygonShape polyB, in Transform xfB)
	{
		manifold.PointCount = 0;
		FP y = polyA.Radius + polyB.Radius;
		int edgeIndex;
		FP x = FindMaxSeparation(out edgeIndex, polyA, in xfA, polyB, in xfB);
		if (x > y)
		{
			return;
		}
		int edgeIndex2;
		FP fP = FindMaxSeparation(out edgeIndex2, polyB, in xfB, polyA, in xfA);
		if (fP > y)
		{
			return;
		}
		PolygonShape polygonShape;
		PolygonShape poly;
		Transform xf;
		Transform xf2;
		int num;
		byte b;
		if (fP > x + k_tol)
		{
			polygonShape = polyB;
			poly = polyA;
			xf = xfB;
			xf2 = xfA;
			num = edgeIndex2;
			manifold.Type = ManifoldType.FaceB;
			b = 1;
		}
		else
		{
			polygonShape = polyA;
			poly = polyB;
			xf = xfA;
			xf2 = xfB;
			num = edgeIndex;
			manifold.Type = ManifoldType.FaceA;
			b = 0;
		}
		Span<ClipVertex> c = stackalloc ClipVertex[2];
		FindIncidentEdge(in c, polygonShape, in xf, num, poly, in xf2);
		int count = polygonShape.Count;
		FVector2[] vertices = polygonShape.Vertices;
		int num2 = num;
		int num3 = ((num + 1 < count) ? (num + 1) : 0);
		FVector2 v = vertices[num2];
		FVector2 v2 = vertices[num3];
		FVector2 a = v2 - v;
		a.Normalize();
		FVector2 localNormal = MathUtils.Cross(in a, 1f);
		FVector2 localPoint = 0.5f * (v + v2);
		FVector2 a2 = MathUtils.Mul(in xf.Rotation, in a);
		FVector2 value = MathUtils.Cross(in a2, 1f);
		v = MathUtils.Mul(in xf, in v);
		v2 = MathUtils.Mul(in xf, in v2);
		FP y2 = FVector2.Dot(value, v);
		FP offset = -FVector2.Dot(a2, v) + y;
		FP offset2 = FVector2.Dot(a2, v2) + y;
		Span<ClipVertex> vOut = stackalloc ClipVertex[2];
		Span<ClipVertex> vOut2 = stackalloc ClipVertex[2];
		if (ClipSegmentToLine(in vOut, in c, -a2, offset, num2) < 2 || ClipSegmentToLine(in vOut2, in vOut, in a2, offset2, num3) < 2)
		{
			return;
		}
		manifold.LocalNormal = localNormal;
		manifold.LocalPoint = localPoint;
		int num4 = 0;
		for (int i = 0; i < 2; i++)
		{
			if (FVector2.Dot(value, vOut2[i].Vector) - y2 <= y)
			{
				ref ManifoldPoint reference = ref manifold.Points[num4];
				reference.LocalPoint = MathUtils.MulT(in xf2, in vOut2[i].Vector);
				reference.Id = vOut2[i].Id;
				if (b != 0)
				{
					ContactFeature contactFeature = reference.Id.ContactFeature;
					reference.Id.ContactFeature.IndexA = contactFeature.IndexB;
					reference.Id.ContactFeature.IndexB = contactFeature.IndexA;
					reference.Id.ContactFeature.TypeA = contactFeature.TypeB;
					reference.Id.ContactFeature.TypeB = contactFeature.TypeA;
				}
				num4++;
			}
		}
		manifold.PointCount = num4;
	}

	public static void GetPointStates(in PointState[] state1, in PointState[] state2, in Manifold manifold1, in Manifold manifold2)
	{
		for (int i = 0; i < 2; i++)
		{
			state1[i] = PointState.NullState;
			state2[i] = PointState.NullState;
		}
		for (int j = 0; j < manifold1.PointCount; j++)
		{
			ContactId id = manifold1.Points[j].Id;
			state1[j] = PointState.RemoveState;
			for (int k = 0; k < manifold2.PointCount; k++)
			{
				if (manifold2.Points[k].Id.Key == id.Key)
				{
					state1[j] = PointState.PersistState;
					break;
				}
			}
		}
		for (int l = 0; l < manifold2.PointCount; l++)
		{
			ContactId id2 = manifold2.Points[l].Id;
			state2[l] = PointState.AddState;
			for (int m = 0; m < manifold1.PointCount; m++)
			{
				if (manifold1.Points[m].Id.Key == id2.Key)
				{
					state2[l] = PointState.PersistState;
					break;
				}
			}
		}
	}

	public static int ClipSegmentToLine(in Span<ClipVertex> vOut, in Span<ClipVertex> vIn, in FVector2 normal, FP offset, int vertexIndexA)
	{
		int num = 0;
		FP x = FVector2.Dot(normal, vIn[0].Vector) - offset;
		FP y = FVector2.Dot(normal, vIn[1].Vector) - offset;
		if (x <= 0f)
		{
			vOut[num++] = vIn[0];
		}
		if (y <= 0f)
		{
			vOut[num++] = vIn[1];
		}
		if (x * y < 0f)
		{
			FP fP = x / (x - y);
			vOut[num].Vector = vIn[0].Vector + fP * (vIn[1].Vector - vIn[0].Vector);
			vOut[num].Id.ContactFeature.IndexA = (byte)vertexIndexA;
			vOut[num].Id.ContactFeature.IndexB = vIn[0].Id.ContactFeature.IndexB;
			vOut[num].Id.ContactFeature.TypeA = 0;
			vOut[num].Id.ContactFeature.TypeB = 1;
			num++;
		}
		return num;
	}

	public static bool TestOverlap(Shape shapeA, int indexA, Shape shapeB, int indexB, in Transform xfA, in Transform xfB, GJkProfile gJkProfile)
	{
		DistanceInput input = default(DistanceInput);
		input.ProxyA.Set(shapeA, indexA);
		input.ProxyB.Set(shapeB, indexB);
		input.TransformA = xfA;
		input.TransformB = xfB;
		input.UseRadii = true;
		SimplexCache cache = default(SimplexCache);
		DistanceAlgorithm.Distance(out var output, ref cache, in input, in gJkProfile);
		return output.Distance < (FP)10f * Settings.Epsilon;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool TestOverlap(in AABB a, AABB b)
	{
		if (b.LowerBound.X - a.UpperBound.X > 0f || b.LowerBound.Y - a.UpperBound.Y > 0f)
		{
			return false;
		}
		if (a.LowerBound.X - b.UpperBound.X > 0f || a.LowerBound.Y - b.UpperBound.Y > 0f)
		{
			return false;
		}
		return true;
	}
}
