#define UNITY_ASSERTIONS
using System;
using Unity.Collections;
using Unity.Profiling;

namespace UnityEngine.UIElements.UIR;

internal static class Tessellation
{
	private enum TessellationType
	{
		EdgeHorizontal,
		EdgeVertical,
		EdgeCorner,
		Content
	}

	internal static float kEpsilon = 0.001f;

	internal static ushort kSubdivisions = 6;

	private static ProfilerMarker s_MarkerTessellateRect = new ProfilerMarker("TessellateRect");

	private static ProfilerMarker s_MarkerTessellateBorder = new ProfilerMarker("TessellateBorder");

	public static void TessellateRect(MeshGenerationContextUtils.RectangleParams rectParams, float posZ, MeshBuilder.AllocMeshData meshAlloc, bool computeUVs)
	{
		if (!(rectParams.rect.width < kEpsilon) && !(rectParams.rect.height < kEpsilon))
		{
			Vector2 rhs = new Vector2(rectParams.rect.width * 0.5f, rectParams.rect.height * 0.5f);
			rectParams.topLeftRadius = Vector2.Min(rectParams.topLeftRadius, rhs);
			rectParams.topRightRadius = Vector2.Min(rectParams.topRightRadius, rhs);
			rectParams.bottomRightRadius = Vector2.Min(rectParams.bottomRightRadius, rhs);
			rectParams.bottomLeftRadius = Vector2.Min(rectParams.bottomLeftRadius, rhs);
			ushort vertexCount = 0;
			ushort indexCount = 0;
			CountRectTriangles(ref rectParams, ref vertexCount, ref indexCount);
			MeshWriteData meshWriteData = meshAlloc.Allocate(vertexCount, indexCount);
			vertexCount = 0;
			indexCount = 0;
			TessellateRectInternal(ref rectParams, posZ, meshWriteData, ref vertexCount, ref indexCount);
			if (computeUVs)
			{
				ComputeUVs(rectParams.rect, rectParams.uv, meshWriteData.uvRegion, meshWriteData.m_Vertices);
			}
			Debug.Assert(vertexCount == meshWriteData.vertexCount);
			Debug.Assert(indexCount == meshWriteData.indexCount);
		}
	}

	public static void TessellateBorder(MeshGenerationContextUtils.BorderParams borderParams, float posZ, MeshBuilder.AllocMeshData meshAlloc)
	{
		if (!(borderParams.rect.width < kEpsilon) && !(borderParams.rect.height < kEpsilon))
		{
			Vector2 rhs = new Vector2(borderParams.rect.width * 0.5f, borderParams.rect.height * 0.5f);
			borderParams.topLeftRadius = Vector2.Min(borderParams.topLeftRadius, rhs);
			borderParams.topRightRadius = Vector2.Min(borderParams.topRightRadius, rhs);
			borderParams.bottomRightRadius = Vector2.Min(borderParams.bottomRightRadius, rhs);
			borderParams.bottomLeftRadius = Vector2.Min(borderParams.bottomLeftRadius, rhs);
			borderParams.leftWidth = Mathf.Min(borderParams.leftWidth, rhs.x);
			borderParams.topWidth = Mathf.Min(borderParams.topWidth, rhs.y);
			borderParams.rightWidth = Mathf.Min(borderParams.rightWidth, rhs.x);
			borderParams.bottomWidth = Mathf.Min(borderParams.bottomWidth, rhs.y);
			ushort vertexCount = 0;
			ushort indexCount = 0;
			CountBorderTriangles(ref borderParams, ref vertexCount, ref indexCount);
			MeshWriteData meshWriteData = meshAlloc.Allocate(vertexCount, indexCount);
			vertexCount = 0;
			indexCount = 0;
			TessellateBorderInternal(ref borderParams, posZ, meshWriteData, ref vertexCount, ref indexCount);
			Debug.Assert(vertexCount == meshWriteData.vertexCount);
			Debug.Assert(indexCount == meshWriteData.indexCount);
		}
	}

	private static void CountRectTriangles(ref MeshGenerationContextUtils.RectangleParams rectParams, ref ushort vertexCount, ref ushort indexCount)
	{
		TessellateRectInternal(ref rectParams, 0f, null, ref vertexCount, ref indexCount, countOnly: true);
	}

	private static void CountBorderTriangles(ref MeshGenerationContextUtils.BorderParams border, ref ushort vertexCount, ref ushort indexCount)
	{
		TessellateBorderInternal(ref border, 0f, null, ref vertexCount, ref indexCount, countOnly: true);
	}

	private static void TessellateRectInternal(ref MeshGenerationContextUtils.RectangleParams rectParams, float posZ, MeshWriteData mesh, ref ushort vertexCount, ref ushort indexCount, bool countOnly = false)
	{
		if (!rectParams.HasRadius(kEpsilon))
		{
			TessellateQuad(rectParams.rect, 0f, 0f, 0f, TessellationType.Content, rectParams.color, posZ, mesh, ref vertexCount, ref indexCount, countOnly);
		}
		else
		{
			TessellateRoundedCorners(ref rectParams, posZ, mesh, ref vertexCount, ref indexCount, countOnly);
		}
	}

	private static void TessellateBorderInternal(ref MeshGenerationContextUtils.BorderParams border, float posZ, MeshWriteData mesh, ref ushort vertexCount, ref ushort indexCount, bool countOnly = false)
	{
		TessellateRoundedBorders(ref border, posZ, mesh, ref vertexCount, ref indexCount, countOnly);
	}

	private static void TessellateRoundedCorners(ref MeshGenerationContextUtils.RectangleParams rectParams, float posZ, MeshWriteData mesh, ref ushort vertexCount, ref ushort indexCount, bool countOnly)
	{
		ushort num = 0;
		ushort num2 = 0;
		Vector2 vector = new Vector2(rectParams.rect.width * 0.5f, rectParams.rect.height * 0.5f);
		Rect rect = new Rect(rectParams.rect.x, rectParams.rect.y, vector.x, vector.y);
		TessellateRoundedCorner(rect, rectParams.color, posZ, rectParams.topLeftRadius, mesh, ref vertexCount, ref indexCount, countOnly);
		num = vertexCount;
		num2 = indexCount;
		TessellateRoundedCorner(rect, rectParams.color, posZ, rectParams.topRightRadius, mesh, ref vertexCount, ref indexCount, countOnly);
		if (!countOnly)
		{
			MirrorVertices(rect, mesh.m_Vertices, num, vertexCount - num, flipHorizontal: true);
			FlipWinding(mesh.m_Indices, num2, indexCount - num2);
		}
		num = vertexCount;
		num2 = indexCount;
		TessellateRoundedCorner(rect, rectParams.color, posZ, rectParams.bottomRightRadius, mesh, ref vertexCount, ref indexCount, countOnly);
		if (!countOnly)
		{
			MirrorVertices(rect, mesh.m_Vertices, num, vertexCount - num, flipHorizontal: true);
			MirrorVertices(rect, mesh.m_Vertices, num, vertexCount - num, flipHorizontal: false);
		}
		num = vertexCount;
		num2 = indexCount;
		TessellateRoundedCorner(rect, rectParams.color, posZ, rectParams.bottomLeftRadius, mesh, ref vertexCount, ref indexCount, countOnly);
		if (!countOnly)
		{
			MirrorVertices(rect, mesh.m_Vertices, num, vertexCount - num, flipHorizontal: false);
			FlipWinding(mesh.m_Indices, num2, indexCount - num2);
		}
	}

	private static void TessellateRoundedBorders(ref MeshGenerationContextUtils.BorderParams border, float posZ, MeshWriteData mesh, ref ushort vertexCount, ref ushort indexCount, bool countOnly)
	{
		ushort num = 0;
		ushort num2 = 0;
		Vector2 vector = new Vector2(border.rect.width * 0.5f, border.rect.height * 0.5f);
		Rect rect = new Rect(border.rect.x, border.rect.y, vector.x, vector.y);
		Color32 leftColor = border.leftColor;
		Color32 topColor = border.topColor;
		Color32 topColor2 = border.bottomColor;
		Color32 leftColor2 = border.rightColor;
		TessellateRoundedBorder(rect, leftColor, topColor, posZ, border.topLeftRadius, border.leftWidth, border.topWidth, mesh, ref vertexCount, ref indexCount, countOnly);
		num = vertexCount;
		num2 = indexCount;
		TessellateRoundedBorder(rect, leftColor2, topColor, posZ, border.topRightRadius, border.rightWidth, border.topWidth, mesh, ref vertexCount, ref indexCount, countOnly);
		if (!countOnly)
		{
			MirrorVertices(rect, mesh.m_Vertices, num, vertexCount - num, flipHorizontal: true);
			FlipWinding(mesh.m_Indices, num2, indexCount - num2);
		}
		num = vertexCount;
		num2 = indexCount;
		TessellateRoundedBorder(rect, leftColor2, topColor2, posZ, border.bottomRightRadius, border.rightWidth, border.bottomWidth, mesh, ref vertexCount, ref indexCount, countOnly);
		if (!countOnly)
		{
			MirrorVertices(rect, mesh.m_Vertices, num, vertexCount - num, flipHorizontal: true);
			MirrorVertices(rect, mesh.m_Vertices, num, vertexCount - num, flipHorizontal: false);
		}
		num = vertexCount;
		num2 = indexCount;
		TessellateRoundedBorder(rect, leftColor, topColor2, posZ, border.bottomLeftRadius, border.leftWidth, border.bottomWidth, mesh, ref vertexCount, ref indexCount, countOnly);
		if (!countOnly)
		{
			MirrorVertices(rect, mesh.m_Vertices, num, vertexCount - num, flipHorizontal: false);
			FlipWinding(mesh.m_Indices, num2, indexCount - num2);
		}
	}

	private static void TessellateRoundedCorner(Rect rect, Color32 color, float posZ, Vector2 radius, MeshWriteData mesh, ref ushort vertexCount, ref ushort indexCount, bool countOnly)
	{
		Vector2 center = rect.position + radius;
		Rect rect2 = Rect.zero;
		if (radius == Vector2.zero)
		{
			TessellateQuad(rect, 0f, 0f, 0f, TessellationType.Content, color, posZ, mesh, ref vertexCount, ref indexCount, countOnly);
			return;
		}
		TessellateFilledFan(TessellationType.Content, center, radius, 0f, 0f, color, posZ, mesh, ref vertexCount, ref indexCount, countOnly);
		if (radius.x < rect.width)
		{
			rect2 = new Rect(rect.x + radius.x, rect.y, rect.width - radius.x, rect.height);
			TessellateQuad(rect2, 0f, 0f, 0f, TessellationType.Content, color, posZ, mesh, ref vertexCount, ref indexCount, countOnly);
		}
		if (radius.y < rect.height)
		{
			rect2 = new Rect(rect.x, rect.y + radius.y, (radius.x < rect.width) ? radius.x : rect.width, rect.height - radius.y);
			TessellateQuad(rect2, 0f, 0f, 0f, TessellationType.Content, color, posZ, mesh, ref vertexCount, ref indexCount, countOnly);
		}
	}

	private static void TessellateRoundedBorder(Rect rect, Color32 leftColor, Color32 topColor, float posZ, Vector2 radius, float leftWidth, float topWidth, MeshWriteData mesh, ref ushort vertexCount, ref ushort indexCount, bool countOnly)
	{
		if (leftWidth < kEpsilon && topWidth < kEpsilon)
		{
			return;
		}
		leftWidth = Mathf.Max(0f, leftWidth);
		topWidth = Mathf.Max(0f, topWidth);
		radius.x = Mathf.Clamp(radius.x, 0f, rect.width);
		radius.y = Mathf.Clamp(radius.y, 0f, rect.height);
		Vector2 center = rect.position + radius;
		Rect zero = Rect.zero;
		if (radius.x < kEpsilon || radius.y < kEpsilon)
		{
			if (leftWidth > kEpsilon)
			{
				zero = new Rect(rect.x, rect.y, leftWidth, rect.height);
				TessellateQuad(zero, topWidth, leftWidth, topWidth, TessellationType.EdgeVertical, leftColor, posZ, mesh, ref vertexCount, ref indexCount, countOnly);
			}
			if (topWidth > kEpsilon)
			{
				zero = new Rect(rect.x, rect.y, rect.width, topWidth);
				TessellateQuad(zero, leftWidth, leftWidth, topWidth, TessellationType.EdgeHorizontal, topColor, posZ, mesh, ref vertexCount, ref indexCount, countOnly);
			}
			return;
		}
		if (LooseCompare(radius.x, leftWidth) == 0 && LooseCompare(radius.y, topWidth) == 0)
		{
			if (leftColor.InternalEquals(topColor))
			{
				TessellateFilledFan(TessellationType.EdgeCorner, center, radius, leftWidth, topWidth, leftColor, posZ, mesh, ref vertexCount, ref indexCount, countOnly);
			}
			else
			{
				TessellateFilledFan(center, radius, leftWidth, topWidth, leftColor, topColor, posZ, mesh, ref vertexCount, ref indexCount, countOnly);
			}
		}
		else if (LooseCompare(radius.x, leftWidth) > 0 && LooseCompare(radius.y, topWidth) > 0)
		{
			if (leftColor.InternalEquals(topColor))
			{
				TessellateBorderedFan(center, radius, leftWidth, topWidth, leftColor, posZ, mesh, ref vertexCount, ref indexCount, countOnly);
			}
			else
			{
				TessellateBorderedFan(center, radius, leftWidth, topWidth, leftColor, topColor, posZ, mesh, ref vertexCount, ref indexCount, countOnly);
			}
		}
		else
		{
			zero = new Rect(rect.x, rect.y, Mathf.Max(radius.x, leftWidth), Mathf.Max(radius.y, topWidth));
			if (leftColor.InternalEquals(topColor))
			{
				TessellateComplexBorderCorner(zero, radius, leftWidth, topWidth, leftColor, posZ, mesh, ref vertexCount, ref indexCount, countOnly);
			}
			else
			{
				TessellateComplexBorderCorner(zero, radius, leftWidth, topWidth, leftColor, topColor, posZ, mesh, ref vertexCount, ref indexCount, countOnly);
			}
		}
		float num = Mathf.Max(radius.y, topWidth);
		zero = new Rect(rect.x, rect.y + num, leftWidth, rect.height - num);
		TessellateQuad(zero, 0f, leftWidth, topWidth, TessellationType.EdgeVertical, leftColor, posZ, mesh, ref vertexCount, ref indexCount, countOnly);
		num = Mathf.Max(radius.x, leftWidth);
		zero = new Rect(rect.x + num, rect.y, rect.width - num, topWidth);
		TessellateQuad(zero, 0f, leftWidth, topWidth, TessellationType.EdgeHorizontal, topColor, posZ, mesh, ref vertexCount, ref indexCount, countOnly);
	}

	private static Vector2 IntersectEllipseWithLine(float a, float b, Vector2 dir)
	{
		Debug.Assert(dir.x > 0f || dir.y > 0f);
		if (a < Mathf.Epsilon || b < Mathf.Epsilon)
		{
			return new Vector2(0f, 0f);
		}
		if ((double)dir.y < 0.001 * (double)dir.x)
		{
			return new Vector2(a, 0f);
		}
		if ((double)dir.x < 0.001 * (double)dir.y)
		{
			return new Vector2(0f, b);
		}
		float num = dir.y / dir.x;
		float num2 = b / a;
		float num3 = b * (num2 + num - Mathf.Sqrt(2f * num * num2)) / (num * num + num2 * num2);
		float y = num * num3;
		return new Vector2(num3, y);
	}

	private static float GetCenteredEllipseLineIntersectionTheta(float a, float b, Vector2 dir)
	{
		return Mathf.Atan2(dir.y * a, dir.x * b);
	}

	private static Vector2 IntersectLines(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
	{
		Vector2 vector = p3 - p2;
		Vector2 vector2 = p2 - p0;
		Vector2 vector3 = p1 - p0;
		float num = vector.x * vector3.y - vector3.x * vector.y;
		if (Mathf.Approximately(num, 0f))
		{
			return new Vector2(float.NaN, float.NaN);
		}
		float num2 = vector.x * vector2.y - vector2.x * vector.y;
		float num3 = num2 / num;
		return p0 + vector3 * num3;
	}

	private static int LooseCompare(float a, float b)
	{
		if (a < b - kEpsilon)
		{
			return -1;
		}
		if (a > b + kEpsilon)
		{
			return 1;
		}
		return 0;
	}

	private static void TessellateComplexBorderCorner(Rect rect, Vector2 radius, float leftWidth, float topWidth, Color32 color, float posZ, MeshWriteData mesh, ref ushort refVertexCount, ref ushort refIndexCount, bool countOnly)
	{
		if (rect.width < kEpsilon || rect.height < kEpsilon)
		{
			return;
		}
		int num = LooseCompare(leftWidth, radius.x);
		int num2 = LooseCompare(topWidth, radius.y);
		Debug.Assert(num != num2 || (num > 0 && num2 > 0));
		ushort num3 = refVertexCount;
		ushort num4 = refIndexCount;
		int num5 = kSubdivisions - 1;
		if (countOnly)
		{
			int num6 = num5;
			if (num2 != 0)
			{
				num6++;
			}
			if (num != 0)
			{
				num6++;
			}
			num3 += (ushort)(num6 + 3);
			num4 += (ushort)(num6 * 3);
			refIndexCount = num4;
			refVertexCount = num3;
			return;
		}
		Color32 idsFlags = new Color32(0, 0, 0, 5);
		Color32 idsFlags2 = new Color32(0, 0, 0, 0);
		Vector2 uv = new Vector2(leftWidth, topWidth);
		ushort nextIndex = num3;
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(leftWidth, topWidth, posZ),
			tint = color,
			uv = uv,
			idsFlags = idsFlags
		});
		num3++;
		ushort nextIndex2 = num3;
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(leftWidth, topWidth, posZ),
			tint = color,
			uv = uv,
			idsFlags = idsFlags
		});
		num3++;
		if (num2 < 0)
		{
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(rect.xMax, rect.yMax, posZ),
				tint = color,
				uv = uv,
				idsFlags = idsFlags
			});
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(0f, rect.yMax, posZ),
				tint = color,
				idsFlags = idsFlags2
			});
			num3 += 2;
			mesh.SetNextIndex(nextIndex2);
			mesh.SetNextIndex((ushort)(num3 - 2));
			mesh.SetNextIndex((ushort)(num3 - 1));
			num4 += 3;
		}
		else
		{
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(0f, rect.yMax, posZ),
				tint = color,
				idsFlags = idsFlags2
			});
			num3++;
		}
		if (num2 > 0)
		{
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(0f, radius.y, posZ),
				tint = color,
				idsFlags = idsFlags2
			});
			num3++;
			mesh.SetNextIndex(nextIndex2);
			mesh.SetNextIndex((ushort)(num3 - 2));
			mesh.SetNextIndex((ushort)(num3 - 1));
			num4 += 3;
		}
		float num7 = (float)Math.PI / 2f / (float)num5;
		for (int i = 1; i < num5; i++)
		{
			float f = (float)i * num7;
			Vector2 vector = new Vector2(radius.x - Mathf.Cos(f) * radius.x, radius.y - Mathf.Sin(f) * radius.y);
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(vector.x, vector.y, posZ),
				tint = color,
				idsFlags = idsFlags2
			});
			num3++;
			mesh.SetNextIndex(nextIndex2);
			mesh.SetNextIndex((ushort)(num3 - 2));
			mesh.SetNextIndex((ushort)(num3 - 1));
			num4 += 3;
		}
		if (num > 0)
		{
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(radius.x, 0f, posZ),
				tint = color,
				idsFlags = idsFlags2
			});
			num3++;
			mesh.SetNextIndex(nextIndex2);
			mesh.SetNextIndex((ushort)(num3 - 2));
			mesh.SetNextIndex((ushort)(num3 - 1));
			num4 += 3;
		}
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(rect.xMax, 0f, posZ),
			tint = color,
			idsFlags = idsFlags2
		});
		num3++;
		mesh.SetNextIndex(nextIndex);
		mesh.SetNextIndex((ushort)(num3 - 2));
		mesh.SetNextIndex((ushort)(num3 - 1));
		num4 += 3;
		if (num < 0)
		{
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(rect.xMax, rect.yMax, posZ),
				tint = color,
				uv = uv,
				idsFlags = idsFlags
			});
			num3++;
			mesh.SetNextIndex(nextIndex);
			mesh.SetNextIndex((ushort)(num3 - 2));
			mesh.SetNextIndex((ushort)(num3 - 1));
			num4 += 3;
		}
		refIndexCount = num4;
		refVertexCount = num3;
	}

	private static void TessellateComplexBorderCorner(Rect rect, Vector2 radius, float leftWidth, float topWidth, Color32 leftColor, Color32 topColor, float posZ, MeshWriteData mesh, ref ushort vertexCount, ref ushort indexCount, bool countOnly)
	{
		if (rect.width < kEpsilon || rect.height < kEpsilon)
		{
			return;
		}
		int num = LooseCompare(leftWidth, radius.x);
		int num2 = LooseCompare(topWidth, radius.y);
		Debug.Assert(num != num2 || (num > 0 && num2 > 0));
		if (countOnly)
		{
			vertexCount += kSubdivisions;
			vertexCount += 2;
			vertexCount += 3;
			int num3 = 2;
			num3 += kSubdivisions - 1;
			if (num != 0)
			{
				vertexCount++;
				num3++;
			}
			if (num2 != 0)
			{
				vertexCount++;
				num3++;
			}
			indexCount += (ushort)(num3 * 3);
			return;
		}
		Vector2 vector = new Vector2(rect.x + leftWidth, rect.y + topWidth);
		Vector2 vector2 = new Vector2(rect.x, rect.y);
		Vector2 p = new Vector2(rect.x, rect.y + radius.y);
		Vector2 p2 = new Vector2(rect.x + radius.x, rect.y);
		Vector2 vector3 = new Vector2(p2.x, p.y);
		Vector2 vector4 = IntersectLines(p, p2, vector, vector2);
		Vector2 vector5 = IntersectEllipseWithLine(radius.x, radius.y, vector - vector2);
		Vector2 vector6 = new Vector2(rect.xMax, rect.y);
		Vector2 vector7 = new Vector2(rect.x, rect.yMax);
		Vector2 vector8 = new Vector2(rect.xMax, rect.yMax);
		float centeredEllipseLineIntersectionTheta = GetCenteredEllipseLineIntersectionTheta(radius.x, radius.y, radius - vector5);
		vector5.x += rect.x;
		vector5.y += rect.y;
		int num4 = kSubdivisions - 1;
		int num5 = Mathf.Clamp(Mathf.RoundToInt(centeredEllipseLineIntersectionTheta / ((float)Math.PI / 2f) * (float)num4), 1, num4 - 1);
		int num6 = num4 - num5;
		Color32 idsFlags = new Color32(0, 0, 0, 5);
		Color32 idsFlags2 = new Color32(0, 0, 0, 0);
		Vector2 uv = new Vector2(leftWidth, topWidth);
		ushort nextIndex = vertexCount;
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(vector4.x, vector4.y, posZ),
			tint = leftColor,
			idsFlags = idsFlags2
		});
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(vector.x, vector.y, posZ),
			tint = leftColor,
			uv = uv,
			idsFlags = idsFlags
		});
		vertexCount += 2;
		if (num2 < 0)
		{
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(vector8.x, vector8.y, posZ),
				tint = leftColor,
				uv = uv,
				idsFlags = idsFlags
			});
			vertexCount++;
			mesh.SetNextIndex(nextIndex);
			mesh.SetNextIndex((ushort)(vertexCount - 2));
			mesh.SetNextIndex((ushort)(vertexCount - 1));
			indexCount += 3;
		}
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(vector7.x, vector7.y, posZ),
			tint = leftColor,
			idsFlags = idsFlags2
		});
		vertexCount++;
		mesh.SetNextIndex(nextIndex);
		mesh.SetNextIndex((ushort)(vertexCount - 2));
		mesh.SetNextIndex((ushort)(vertexCount - 1));
		indexCount += 3;
		if (num2 > 0)
		{
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(p.x, p.y, posZ),
				tint = leftColor,
				idsFlags = idsFlags2
			});
			vertexCount++;
			mesh.SetNextIndex(nextIndex);
			mesh.SetNextIndex((ushort)(vertexCount - 2));
			mesh.SetNextIndex((ushort)(vertexCount - 1));
			indexCount += 3;
		}
		float num7 = centeredEllipseLineIntersectionTheta / (float)num5;
		for (int i = 1; i < num5; i++)
		{
			float f = (float)i * num7;
			Vector2 vector9 = vector3 - new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * radius;
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(vector9.x, vector9.y, posZ),
				tint = leftColor,
				idsFlags = idsFlags2
			});
			vertexCount++;
			mesh.SetNextIndex(nextIndex);
			mesh.SetNextIndex((ushort)(vertexCount - 2));
			mesh.SetNextIndex((ushort)(vertexCount - 1));
			indexCount += 3;
		}
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(vector5.x, vector5.y, posZ),
			tint = leftColor,
			idsFlags = idsFlags2
		});
		vertexCount++;
		mesh.SetNextIndex(nextIndex);
		mesh.SetNextIndex((ushort)(vertexCount - 2));
		mesh.SetNextIndex((ushort)(vertexCount - 1));
		indexCount += 3;
		ushort nextIndex2 = vertexCount;
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(vector4.x, vector4.y, posZ),
			tint = topColor,
			idsFlags = idsFlags2
		});
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(vector5.x, vector5.y, posZ),
			tint = topColor,
			idsFlags = idsFlags2
		});
		vertexCount += 2;
		float num8 = ((float)Math.PI / 2f - centeredEllipseLineIntersectionTheta) / (float)num6;
		for (int j = 1; j < num6; j++)
		{
			float f2 = centeredEllipseLineIntersectionTheta + (float)j * num8;
			Vector2 vector10 = vector3 - new Vector2(Mathf.Cos(f2), Mathf.Sin(f2)) * radius;
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(vector10.x, vector10.y, posZ),
				tint = topColor,
				idsFlags = idsFlags2
			});
			vertexCount++;
			mesh.SetNextIndex(nextIndex2);
			mesh.SetNextIndex((ushort)(vertexCount - 2));
			mesh.SetNextIndex((ushort)(vertexCount - 1));
			indexCount += 3;
		}
		if (num > 0)
		{
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(p2.x, p2.y, posZ),
				tint = topColor,
				idsFlags = idsFlags2
			});
			vertexCount++;
			mesh.SetNextIndex(nextIndex2);
			mesh.SetNextIndex((ushort)(vertexCount - 2));
			mesh.SetNextIndex((ushort)(vertexCount - 1));
			indexCount += 3;
		}
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(vector6.x, vector6.y, posZ),
			tint = topColor,
			idsFlags = idsFlags2
		});
		vertexCount++;
		mesh.SetNextIndex(nextIndex2);
		mesh.SetNextIndex((ushort)(vertexCount - 2));
		mesh.SetNextIndex((ushort)(vertexCount - 1));
		indexCount += 3;
		if (num < 0)
		{
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(vector8.x, vector8.y, posZ),
				tint = topColor,
				uv = uv,
				idsFlags = idsFlags
			});
			vertexCount++;
			mesh.SetNextIndex(nextIndex2);
			mesh.SetNextIndex((ushort)(vertexCount - 2));
			mesh.SetNextIndex((ushort)(vertexCount - 1));
			indexCount += 3;
		}
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(vector.x, vector.y, posZ),
			tint = topColor,
			uv = uv,
			idsFlags = idsFlags
		});
		vertexCount++;
		mesh.SetNextIndex(nextIndex2);
		mesh.SetNextIndex((ushort)(vertexCount - 2));
		mesh.SetNextIndex((ushort)(vertexCount - 1));
		indexCount += 3;
	}

	private static void TessellateQuad(Rect rect, float miterOffset, float leftWidth, float topWidth, TessellationType tessellationType, Color32 color, float posZ, MeshWriteData mesh, ref ushort vertexCount, ref ushort indexCount, bool countOnly)
	{
		if ((rect.width < kEpsilon || rect.height < kEpsilon) && tessellationType != TessellationType.EdgeHorizontal && tessellationType != TessellationType.EdgeVertical)
		{
			return;
		}
		if (countOnly)
		{
			vertexCount += 4;
			indexCount += 6;
			return;
		}
		Vector3 position = new Vector3(rect.x, rect.y, posZ);
		Vector3 position2 = new Vector3(rect.xMax, rect.y, posZ);
		Vector3 position3 = new Vector3(rect.x, rect.yMax, posZ);
		Vector3 position4 = new Vector3(rect.xMax, rect.yMax, posZ);
		Vector2 vector = new Vector2(leftWidth, topWidth);
		Vector2 uv;
		Vector2 uv2;
		Vector2 uv3;
		Vector2 uv4;
		Color32 color3;
		Color32 idsFlags;
		Color32 color4;
		Color32 color2;
		switch (tessellationType)
		{
		case TessellationType.EdgeHorizontal:
			position3.x += miterOffset;
			uv = (uv2 = Vector2.zero);
			uv3 = (uv4 = vector);
			color3 = new Color32(0, 0, 0, 0);
			idsFlags = color3;
			color4 = new Color32(0, 0, 0, 5);
			color2 = new Color32(0, 0, 0, 6);
			break;
		case TessellationType.EdgeVertical:
			position2.y += miterOffset;
			uv = (uv3 = Vector2.zero);
			uv2 = (uv4 = vector);
			color4 = new Color32(0, 0, 0, 0);
			idsFlags = color4;
			color3 = new Color32(0, 0, 0, 5);
			color2 = new Color32(0, 0, 0, 7);
			break;
		case TessellationType.EdgeCorner:
			uv = (uv2 = (uv3 = (uv4 = Vector2.zero)));
			color2 = new Color32(0, 0, 0, 0);
			idsFlags = (color3 = (color4 = color2));
			break;
		case TessellationType.Content:
			uv = (uv2 = (uv3 = (uv4 = Vector2.zero)));
			color2 = new Color32(0, 0, 0, 0);
			idsFlags = (color3 = (color4 = color2));
			break;
		default:
			throw new NotImplementedException();
		}
		mesh.SetNextVertex(new Vertex
		{
			position = position,
			uv = uv,
			tint = color,
			idsFlags = idsFlags
		});
		mesh.SetNextVertex(new Vertex
		{
			position = position2,
			uv = uv2,
			tint = color,
			idsFlags = color3
		});
		mesh.SetNextVertex(new Vertex
		{
			position = position3,
			uv = uv3,
			tint = color,
			idsFlags = color4
		});
		mesh.SetNextVertex(new Vertex
		{
			position = position4,
			uv = uv4,
			tint = color,
			idsFlags = color2
		});
		mesh.SetNextIndex(vertexCount);
		mesh.SetNextIndex((ushort)(vertexCount + 1));
		mesh.SetNextIndex((ushort)(vertexCount + 2));
		mesh.SetNextIndex((ushort)(vertexCount + 3));
		mesh.SetNextIndex((ushort)(vertexCount + 2));
		mesh.SetNextIndex((ushort)(vertexCount + 1));
		vertexCount += 4;
		indexCount += 6;
	}

	private static void TessellateFilledFan(Vector2 center, Vector2 radius, float leftWidth, float topWidth, Color32 leftColor, Color32 topColor, float posZ, MeshWriteData mesh, ref ushort vertexCount, ref ushort indexCount, bool countOnly)
	{
		if (countOnly)
		{
			vertexCount += (ushort)(kSubdivisions + 3);
			indexCount += (ushort)((kSubdivisions - 1) * 3);
			return;
		}
		Color32 idsFlags = new Color32(0, 0, 0, 5);
		Color32 idsFlags2 = new Color32(0, 0, 0, 0);
		Vector2 uv = new Vector2(leftWidth, topWidth);
		float centeredEllipseLineIntersectionTheta = GetCenteredEllipseLineIntersectionTheta(radius.x, radius.y, radius);
		int num = kSubdivisions - 1;
		int num2 = Mathf.Clamp(Mathf.RoundToInt(centeredEllipseLineIntersectionTheta / ((float)Math.PI / 2f) * (float)num), 1, num - 1);
		int num3 = num - num2;
		ushort nextIndex = vertexCount;
		Vector2 vector = new Vector2(center.x - radius.x, center.y);
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(center.x, center.y, posZ),
			tint = leftColor,
			idsFlags = idsFlags,
			uv = uv
		});
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(vector.x, vector.y, posZ),
			tint = leftColor,
			idsFlags = idsFlags2
		});
		vertexCount += 2;
		float num4 = centeredEllipseLineIntersectionTheta / (float)num2;
		for (int i = 1; i <= num2; i++)
		{
			float f = num4 * (float)i;
			vector = center - new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * radius;
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(vector.x, vector.y, posZ),
				tint = leftColor,
				idsFlags = idsFlags2
			});
			vertexCount++;
			mesh.SetNextIndex(nextIndex);
			mesh.SetNextIndex((ushort)(vertexCount - 2));
			mesh.SetNextIndex((ushort)(vertexCount - 1));
			indexCount += 3;
		}
		ushort nextIndex2 = vertexCount;
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(center.x, center.y, posZ),
			tint = topColor,
			idsFlags = idsFlags,
			uv = uv
		});
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(vector.x, vector.y, posZ),
			tint = topColor,
			idsFlags = idsFlags2
		});
		vertexCount += 2;
		float num5 = ((float)Math.PI / 2f - centeredEllipseLineIntersectionTheta) / (float)num3;
		for (int j = 1; j <= num3; j++)
		{
			float f2 = centeredEllipseLineIntersectionTheta + num5 * (float)j;
			vector = center - new Vector2(Mathf.Cos(f2), Mathf.Sin(f2)) * radius;
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(vector.x, vector.y, posZ),
				tint = topColor,
				idsFlags = idsFlags2
			});
			vertexCount++;
			mesh.SetNextIndex(nextIndex2);
			mesh.SetNextIndex((ushort)(vertexCount - 2));
			mesh.SetNextIndex((ushort)(vertexCount - 1));
			indexCount += 3;
		}
	}

	private static void TessellateFilledFan(TessellationType tessellationType, Vector2 center, Vector2 radius, float leftWidth, float topWidth, Color32 color, float posZ, MeshWriteData mesh, ref ushort vertexCount, ref ushort indexCount, bool countOnly)
	{
		if (countOnly)
		{
			vertexCount += (ushort)(kSubdivisions + 1);
			indexCount += (ushort)((kSubdivisions - 1) * 3);
			return;
		}
		Color32 color2;
		Color32 idsFlags;
		if (tessellationType == TessellationType.EdgeCorner)
		{
			color2 = new Color32(0, 0, 0, 5);
			idsFlags = new Color32(0, 0, 0, 0);
		}
		else
		{
			color2 = new Color32(0, 0, 0, 0);
			idsFlags = color2;
		}
		Vector2 uv = new Vector2(leftWidth, topWidth);
		Vector2 vector = new Vector2(center.x - radius.x, center.y);
		ushort num = vertexCount;
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(center.x, center.y, posZ),
			tint = color,
			idsFlags = color2,
			uv = uv
		});
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(vector.x, vector.y, posZ),
			tint = color,
			idsFlags = idsFlags
		});
		vertexCount += 2;
		for (int i = 1; i < kSubdivisions; i++)
		{
			float f = (float)Math.PI / 2f * (float)i / (float)(kSubdivisions - 1);
			vector = center + new Vector2(0f - Mathf.Cos(f), 0f - Mathf.Sin(f)) * radius;
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(vector.x, vector.y, posZ),
				tint = color,
				idsFlags = idsFlags
			});
			vertexCount++;
			mesh.SetNextIndex(num);
			mesh.SetNextIndex((ushort)(num + i));
			mesh.SetNextIndex((ushort)(num + i + 1));
			indexCount += 3;
		}
		num += (ushort)(kSubdivisions + 1);
	}

	private static void TessellateBorderedFan(Vector2 center, Vector2 outerRadius, float leftWidth, float topWidth, Color32 leftColor, Color32 topColor, float posZ, MeshWriteData mesh, ref ushort vertexCount, ref ushort indexCount, bool countOnly)
	{
		if (countOnly)
		{
			vertexCount += (ushort)(kSubdivisions * 2 + 2);
			indexCount += (ushort)((kSubdivisions - 1) * 6);
			return;
		}
		Color32 idsFlags = new Color32(0, 0, 0, 5);
		Color32 idsFlags2 = new Color32(0, 0, 0, 0);
		Vector2 vector = new Vector2(outerRadius.x - leftWidth, outerRadius.y - topWidth);
		Vector2 uv = new Vector2(leftWidth, topWidth);
		Vector2 dir = new Vector2(leftWidth, topWidth);
		Vector2 vector2 = IntersectEllipseWithLine(outerRadius.x, outerRadius.y, dir);
		Vector2 vector3 = IntersectEllipseWithLine(vector.x, vector.y, dir);
		float centeredEllipseLineIntersectionTheta = GetCenteredEllipseLineIntersectionTheta(outerRadius.x, outerRadius.y, outerRadius - vector2);
		float centeredEllipseLineIntersectionTheta2 = GetCenteredEllipseLineIntersectionTheta(vector.x, vector.y, vector - vector3);
		float num = 0.5f * (centeredEllipseLineIntersectionTheta + centeredEllipseLineIntersectionTheta2);
		int num2 = kSubdivisions - 1;
		int num3 = Mathf.Clamp(Mathf.RoundToInt(num * (2f / (float)Math.PI) * (float)num2), 1, num2 - 1);
		int num4 = num2 - num3;
		float num5 = centeredEllipseLineIntersectionTheta / (float)num3;
		float num6 = centeredEllipseLineIntersectionTheta2 / (float)num3;
		Vector2 vector4 = new Vector2(center.x - outerRadius.x, center.y);
		Vector2 vector5 = new Vector2(center.x - vector.x, center.y);
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(vector5.x, vector5.y, posZ),
			tint = leftColor,
			idsFlags = idsFlags,
			uv = uv
		});
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(vector4.x, vector4.y, posZ),
			tint = leftColor,
			idsFlags = idsFlags2
		});
		vertexCount += 2;
		for (int i = 1; i <= num3; i++)
		{
			float f = (float)i * num5;
			float f2 = (float)i * num6;
			vector4 = center - new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * outerRadius;
			vector5 = center - new Vector2(Mathf.Cos(f2), Mathf.Sin(f2)) * vector;
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(vector5.x, vector5.y, posZ),
				tint = leftColor,
				idsFlags = idsFlags,
				uv = uv
			});
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(vector4.x, vector4.y, posZ),
				tint = leftColor,
				idsFlags = idsFlags2
			});
			vertexCount += 2;
			mesh.SetNextIndex((ushort)(vertexCount - 4));
			mesh.SetNextIndex((ushort)(vertexCount - 3));
			mesh.SetNextIndex((ushort)(vertexCount - 2));
			mesh.SetNextIndex((ushort)(vertexCount - 3));
			mesh.SetNextIndex((ushort)(vertexCount - 1));
			mesh.SetNextIndex((ushort)(vertexCount - 2));
			indexCount += 6;
		}
		float num7 = ((float)Math.PI / 2f - centeredEllipseLineIntersectionTheta) / (float)num4;
		float num8 = ((float)Math.PI / 2f - centeredEllipseLineIntersectionTheta2) / (float)num4;
		idsFlags2 = new Color32(0, 0, 0, 0);
		idsFlags = idsFlags2;
		Vector2 vector6 = center - new Vector2(Mathf.Cos(centeredEllipseLineIntersectionTheta), Mathf.Sin(centeredEllipseLineIntersectionTheta)) * outerRadius;
		Vector2 vector7 = center - new Vector2(Mathf.Cos(centeredEllipseLineIntersectionTheta2), Mathf.Sin(centeredEllipseLineIntersectionTheta2)) * vector;
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(vector7.x, vector7.y, posZ),
			tint = topColor,
			idsFlags = idsFlags,
			uv = uv
		});
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(vector6.x, vector6.y, posZ),
			tint = topColor,
			idsFlags = idsFlags2
		});
		vertexCount += 2;
		for (int j = 1; j <= num4; j++)
		{
			float f3 = centeredEllipseLineIntersectionTheta + (float)j * num7;
			float f4 = centeredEllipseLineIntersectionTheta2 + (float)j * num8;
			vector6 = center - new Vector2(Mathf.Cos(f3), Mathf.Sin(f3)) * outerRadius;
			vector7 = center - new Vector2(Mathf.Cos(f4), Mathf.Sin(f4)) * vector;
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(vector7.x, vector7.y, posZ),
				tint = topColor,
				idsFlags = idsFlags,
				uv = uv
			});
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(vector6.x, vector6.y, posZ),
				tint = topColor,
				idsFlags = idsFlags2
			});
			vertexCount += 2;
			mesh.SetNextIndex((ushort)(vertexCount - 4));
			mesh.SetNextIndex((ushort)(vertexCount - 3));
			mesh.SetNextIndex((ushort)(vertexCount - 2));
			mesh.SetNextIndex((ushort)(vertexCount - 3));
			mesh.SetNextIndex((ushort)(vertexCount - 1));
			mesh.SetNextIndex((ushort)(vertexCount - 2));
			indexCount += 6;
		}
	}

	private static void TessellateBorderedFan(Vector2 center, Vector2 radius, float leftWidth, float topWidth, Color32 color, float posZ, MeshWriteData mesh, ref ushort vertexCount, ref ushort indexCount, bool countOnly)
	{
		if (countOnly)
		{
			vertexCount += (ushort)(kSubdivisions * 2);
			indexCount += (ushort)((kSubdivisions - 1) * 6);
			return;
		}
		Color32 idsFlags = new Color32(0, 0, 0, 5);
		Color32 idsFlags2 = new Color32(0, 0, 0, 0);
		Vector2 uv = new Vector2(leftWidth, topWidth);
		float num = radius.x - leftWidth;
		float num2 = radius.y - topWidth;
		Vector2 vector = new Vector2(center.x - radius.x, center.y);
		Vector2 vector2 = new Vector2(center.x - num, center.y);
		ushort num3 = vertexCount;
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(vector2.x, vector2.y, posZ),
			tint = color,
			idsFlags = idsFlags,
			uv = uv
		});
		mesh.SetNextVertex(new Vertex
		{
			position = new Vector3(vector.x, vector.y, posZ),
			tint = color,
			idsFlags = idsFlags2
		});
		vertexCount += 2;
		for (int i = 1; i < kSubdivisions; i++)
		{
			float num4 = (float)i / (float)(kSubdivisions - 1);
			float f = (float)Math.PI / 2f * num4;
			vector = center + new Vector2(0f - Mathf.Cos(f), 0f - Mathf.Sin(f)) * radius;
			vector2 = center + new Vector2((0f - num) * Mathf.Cos(f), (0f - num2) * Mathf.Sin(f));
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(vector2.x, vector2.y, posZ),
				tint = color,
				idsFlags = idsFlags,
				uv = uv
			});
			mesh.SetNextVertex(new Vertex
			{
				position = new Vector3(vector.x, vector.y, posZ),
				tint = color,
				idsFlags = idsFlags2
			});
			vertexCount += 2;
			int num5 = i * 2;
			mesh.SetNextIndex((ushort)(num3 + (num5 - 2)));
			mesh.SetNextIndex((ushort)(num3 + (num5 - 1)));
			mesh.SetNextIndex((ushort)(num3 + num5));
			mesh.SetNextIndex((ushort)(num3 + (num5 - 1)));
			mesh.SetNextIndex((ushort)(num3 + (num5 + 1)));
			mesh.SetNextIndex((ushort)(num3 + num5));
			indexCount += 6;
		}
		num3 += (ushort)(kSubdivisions * 2);
	}

	private static void MirrorVertices(Rect rect, NativeSlice<Vertex> vertices, int vertexStart, int vertexCount, bool flipHorizontal)
	{
		if (flipHorizontal)
		{
			for (int i = 0; i < vertexCount; i++)
			{
				Vertex value = vertices[vertexStart + i];
				value.position.x = rect.xMax - (value.position.x - rect.xMax);
				value.uv.x = 0f - value.uv.x;
				vertices[vertexStart + i] = value;
			}
		}
		else
		{
			for (int j = 0; j < vertexCount; j++)
			{
				Vertex value2 = vertices[vertexStart + j];
				value2.position.y = rect.yMax - (value2.position.y - rect.yMax);
				value2.uv.y = 0f - value2.uv.y;
				vertices[vertexStart + j] = value2;
			}
		}
	}

	private static void FlipWinding(NativeSlice<ushort> indices, int indexStart, int indexCount)
	{
		for (int i = 0; i < indexCount; i += 3)
		{
			ushort value = indices[indexStart + i];
			indices[indexStart + i] = indices[indexStart + i + 1];
			indices[indexStart + i + 1] = value;
		}
	}

	private static void ComputeUVs(Rect tessellatedRect, Rect textureRect, Rect uvRegion, NativeSlice<Vertex> vertices)
	{
		Vector2 position = tessellatedRect.position;
		Vector2 vector = new Vector2(1f / tessellatedRect.width, 1f / tessellatedRect.height);
		for (int i = 0; i < vertices.Length; i++)
		{
			Vertex value = vertices[i];
			Vector2 vector2 = value.position;
			vector2 -= position;
			vector2 *= vector;
			value.uv.x = (vector2.x * textureRect.width + textureRect.xMin) * uvRegion.width + uvRegion.xMin;
			value.uv.y = ((1f - vector2.y) * textureRect.height + textureRect.yMin) * uvRegion.height + uvRegion.yMin;
			vertices[i] = value;
		}
	}
}
