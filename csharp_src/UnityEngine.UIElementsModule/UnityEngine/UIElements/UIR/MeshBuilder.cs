#define UNITY_ASSERTIONS
using System;
using Unity.Collections;
using Unity.Profiling;
using UnityEngine.TextCore;

namespace UnityEngine.UIElements.UIR;

internal static class MeshBuilder
{
	internal struct AllocMeshData
	{
		internal delegate MeshWriteData Allocator(uint vertexCount, uint indexCount, ref AllocMeshData allocatorData);

		internal Allocator alloc;

		internal Texture texture;

		internal Material material;

		internal MeshGenerationContext.MeshFlags flags;

		internal MeshWriteData Allocate(uint vertexCount, uint indexCount)
		{
			return alloc(vertexCount, indexCount, ref this);
		}
	}

	private struct ClipCounts
	{
		public int firstClippedIndex;

		public int firstDegenerateIndex;

		public int lastClippedIndex;

		public int clippedTriangles;

		public int addedTriangles;

		public int degenerateTriangles;
	}

	private static ProfilerMarker s_VectorGraphics9Slice = new ProfilerMarker("UIR.MakeVector9Slice");

	private static ProfilerMarker s_VectorGraphicsStretch = new ProfilerMarker("UIR.MakeVectorStretch");

	private static readonly ushort[] slicedQuadIndices = new ushort[54]
	{
		0, 4, 1, 4, 5, 1, 1, 5, 2, 5,
		6, 2, 2, 6, 3, 6, 7, 3, 4, 8,
		5, 8, 9, 5, 5, 9, 6, 9, 10, 6,
		6, 10, 7, 10, 11, 7, 8, 12, 9, 12,
		13, 9, 9, 13, 10, 13, 14, 10, 10, 14,
		11, 14, 15, 11
	};

	private static readonly float[] k_TexCoordSlicesX = new float[4];

	private static readonly float[] k_TexCoordSlicesY = new float[4];

	private static readonly float[] k_PositionSlicesX = new float[4];

	private static readonly float[] k_PositionSlicesY = new float[4];

	internal static void MakeBorder(MeshGenerationContextUtils.BorderParams borderParams, float posZ, AllocMeshData meshAlloc)
	{
		Tessellation.TessellateBorder(borderParams, posZ, meshAlloc);
	}

	internal static void MakeSolidRect(MeshGenerationContextUtils.RectangleParams rectParams, float posZ, AllocMeshData meshAlloc)
	{
		if (!rectParams.HasRadius(Tessellation.kEpsilon))
		{
			MakeQuad(rectParams.rect, Rect.zero, rectParams.color, posZ, meshAlloc);
		}
		else
		{
			Tessellation.TessellateRect(rectParams, posZ, meshAlloc, computeUVs: false);
		}
	}

	internal static void MakeTexturedRect(MeshGenerationContextUtils.RectangleParams rectParams, float posZ, AllocMeshData meshAlloc)
	{
		if ((float)rectParams.leftSlice <= Mathf.Epsilon && (float)rectParams.topSlice <= Mathf.Epsilon && (float)rectParams.rightSlice <= Mathf.Epsilon && (float)rectParams.bottomSlice <= Mathf.Epsilon)
		{
			if (!rectParams.HasRadius(Tessellation.kEpsilon))
			{
				MakeQuad(rectParams.rect, rectParams.uv, rectParams.color, posZ, meshAlloc);
			}
			else
			{
				Tessellation.TessellateRect(rectParams, posZ, meshAlloc, computeUVs: true);
			}
		}
		else if (rectParams.texture == null)
		{
			MakeQuad(rectParams.rect, rectParams.uv, rectParams.color, posZ, meshAlloc);
		}
		else
		{
			MakeSlicedQuad(ref rectParams, posZ, meshAlloc);
		}
	}

	private static Vertex ConvertTextVertexToUIRVertex(MeshInfo info, int index, Vector2 offset)
	{
		return new Vertex
		{
			position = new Vector3(info.vertices[index].x + offset.x, info.vertices[index].y + offset.y, 0.995f),
			uv = info.uvs0[index],
			tint = info.colors32[index],
			idsFlags = new Color32(0, 0, 0, 1)
		};
	}

	private static Vertex ConvertTextVertexToUIRVertex(TextVertex textVertex, Vector2 offset)
	{
		return new Vertex
		{
			position = new Vector3(textVertex.position.x + offset.x, textVertex.position.y + offset.y, 0.995f),
			uv = textVertex.uv0,
			tint = textVertex.color,
			idsFlags = new Color32(0, 0, 0, 1)
		};
	}

	private static int LimitTextVertices(int vertexCount, bool logTruncation = true)
	{
		if (vertexCount <= 49152)
		{
			return vertexCount;
		}
		if (logTruncation)
		{
			Debug.LogError($"Generated text will be truncated because it exceeds {49152} vertices.");
		}
		return 49152;
	}

	internal static void MakeText(MeshInfo meshInfo, Vector2 offset, AllocMeshData meshAlloc)
	{
		int num = LimitTextVertices(meshInfo.vertexCount);
		int num2 = num / 4;
		MeshWriteData meshWriteData = meshAlloc.Allocate((uint)(num2 * 4), (uint)(num2 * 6));
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		while (num3 < num2)
		{
			meshWriteData.SetNextVertex(ConvertTextVertexToUIRVertex(meshInfo, num4, offset));
			meshWriteData.SetNextVertex(ConvertTextVertexToUIRVertex(meshInfo, num4 + 1, offset));
			meshWriteData.SetNextVertex(ConvertTextVertexToUIRVertex(meshInfo, num4 + 2, offset));
			meshWriteData.SetNextVertex(ConvertTextVertexToUIRVertex(meshInfo, num4 + 3, offset));
			meshWriteData.SetNextIndex((ushort)num4);
			meshWriteData.SetNextIndex((ushort)(num4 + 1));
			meshWriteData.SetNextIndex((ushort)(num4 + 2));
			meshWriteData.SetNextIndex((ushort)(num4 + 2));
			meshWriteData.SetNextIndex((ushort)(num4 + 3));
			meshWriteData.SetNextIndex((ushort)num4);
			num3++;
			num4 += 4;
			num5 += 6;
		}
	}

	internal static void MakeText(NativeArray<TextVertex> uiVertices, Vector2 offset, AllocMeshData meshAlloc)
	{
		int num = LimitTextVertices(uiVertices.Length);
		int num2 = num / 4;
		MeshWriteData meshWriteData = meshAlloc.Allocate((uint)(num2 * 4), (uint)(num2 * 6));
		int num3 = 0;
		int num4 = 0;
		while (num3 < num2)
		{
			meshWriteData.SetNextVertex(ConvertTextVertexToUIRVertex(uiVertices[num4], offset));
			meshWriteData.SetNextVertex(ConvertTextVertexToUIRVertex(uiVertices[num4 + 1], offset));
			meshWriteData.SetNextVertex(ConvertTextVertexToUIRVertex(uiVertices[num4 + 2], offset));
			meshWriteData.SetNextVertex(ConvertTextVertexToUIRVertex(uiVertices[num4 + 3], offset));
			meshWriteData.SetNextIndex((ushort)num4);
			meshWriteData.SetNextIndex((ushort)(num4 + 1));
			meshWriteData.SetNextIndex((ushort)(num4 + 2));
			meshWriteData.SetNextIndex((ushort)(num4 + 2));
			meshWriteData.SetNextIndex((ushort)(num4 + 3));
			meshWriteData.SetNextIndex((ushort)num4);
			num3++;
			num4 += 4;
		}
	}

	internal static void UpdateText(NativeArray<TextVertex> uiVertices, Vector2 offset, Matrix4x4 transform, Color32 xformClipPages, Color32 idsFlags, Color32 opacityPageSVGSettingIndex, NativeSlice<Vertex> vertices)
	{
		int num = LimitTextVertices(uiVertices.Length, logTruncation: false);
		Debug.Assert(num == vertices.Length);
		idsFlags.a = 1;
		for (int i = 0; i < num; i++)
		{
			TextVertex textVertex = uiVertices[i];
			vertices[i] = new Vertex
			{
				position = transform.MultiplyPoint3x4(new Vector3(textVertex.position.x + offset.x, textVertex.position.y + offset.y, 0.995f)),
				uv = textVertex.uv0,
				tint = textVertex.color,
				xformClipPages = xformClipPages,
				idsFlags = idsFlags,
				opacityPageSVGSettingIndex = opacityPageSVGSettingIndex
			};
		}
	}

	private static void MakeQuad(Rect rcPosition, Rect rcTexCoord, Color color, float posZ, AllocMeshData meshAlloc)
	{
		MeshWriteData meshWriteData = meshAlloc.Allocate(4u, 6u);
		float x = rcPosition.x;
		float xMax = rcPosition.xMax;
		float yMax = rcPosition.yMax;
		float y = rcPosition.y;
		Rect uvRegion = meshWriteData.uvRegion;
		float x2 = rcTexCoord.x * uvRegion.width + uvRegion.xMin;
		float x3 = rcTexCoord.xMax * uvRegion.width + uvRegion.xMin;
		float y2 = rcTexCoord.y * uvRegion.height + uvRegion.yMin;
		float y3 = rcTexCoord.yMax * uvRegion.height + uvRegion.yMin;
		meshWriteData.SetNextVertex(new Vertex
		{
			position = new Vector3(x, yMax, posZ),
			tint = color,
			uv = new Vector2(x2, y2)
		});
		meshWriteData.SetNextVertex(new Vertex
		{
			position = new Vector3(xMax, yMax, posZ),
			tint = color,
			uv = new Vector2(x3, y2)
		});
		meshWriteData.SetNextVertex(new Vertex
		{
			position = new Vector3(x, y, posZ),
			tint = color,
			uv = new Vector2(x2, y3)
		});
		meshWriteData.SetNextVertex(new Vertex
		{
			position = new Vector3(xMax, y, posZ),
			tint = color,
			uv = new Vector2(x3, y3)
		});
		meshWriteData.SetNextIndex(0);
		meshWriteData.SetNextIndex(2);
		meshWriteData.SetNextIndex(1);
		meshWriteData.SetNextIndex(1);
		meshWriteData.SetNextIndex(2);
		meshWriteData.SetNextIndex(3);
	}

	internal static void MakeSlicedQuad(ref MeshGenerationContextUtils.RectangleParams rectParams, float posZ, AllocMeshData meshAlloc)
	{
		MeshWriteData meshWriteData = meshAlloc.Allocate(16u, 54u);
		float num = 1f;
		float num2 = rectParams.texture.width;
		float num3 = rectParams.texture.height;
		float num4 = num / num2;
		float num5 = num / num3;
		float num6 = Mathf.Max(0f, rectParams.leftSlice);
		float num7 = Mathf.Max(0f, rectParams.rightSlice);
		float num8 = Mathf.Max(0f, rectParams.bottomSlice);
		float num9 = Mathf.Max(0f, rectParams.topSlice);
		float num10 = Mathf.Clamp(num6 * num4, 0f, 1f);
		float num11 = Mathf.Clamp(num7 * num4, 0f, 1f);
		float num12 = Mathf.Clamp(num8 * num5, 0f, 1f);
		float num13 = Mathf.Clamp(num9 * num5, 0f, 1f);
		k_TexCoordSlicesX[0] = rectParams.uv.min.x;
		k_TexCoordSlicesX[1] = rectParams.uv.min.x + num10;
		k_TexCoordSlicesX[2] = rectParams.uv.max.x - num11;
		k_TexCoordSlicesX[3] = rectParams.uv.max.x;
		k_TexCoordSlicesY[0] = rectParams.uv.max.y;
		k_TexCoordSlicesY[1] = rectParams.uv.max.y - num12;
		k_TexCoordSlicesY[2] = rectParams.uv.min.y + num13;
		k_TexCoordSlicesY[3] = rectParams.uv.min.y;
		Rect uvRegion = meshWriteData.uvRegion;
		for (int i = 0; i < 4; i++)
		{
			k_TexCoordSlicesX[i] = k_TexCoordSlicesX[i] * uvRegion.width + uvRegion.xMin;
			k_TexCoordSlicesY[i] = (rectParams.uv.min.y + rectParams.uv.max.y - k_TexCoordSlicesY[i]) * uvRegion.height + uvRegion.yMin;
		}
		float num14 = num6 + num7;
		if (num14 > rectParams.rect.width)
		{
			float num15 = rectParams.rect.width / num14;
			num6 *= num15;
			num7 *= num15;
		}
		float num16 = num8 + num9;
		if (num16 > rectParams.rect.height)
		{
			float num17 = rectParams.rect.height / num16;
			num8 *= num17;
			num9 *= num17;
		}
		k_PositionSlicesX[0] = rectParams.rect.x;
		k_PositionSlicesX[1] = rectParams.rect.x + num6;
		k_PositionSlicesX[2] = rectParams.rect.xMax - num7;
		k_PositionSlicesX[3] = rectParams.rect.xMax;
		k_PositionSlicesY[0] = rectParams.rect.yMax;
		k_PositionSlicesY[1] = rectParams.rect.yMax - num8;
		k_PositionSlicesY[2] = rectParams.rect.y + num9;
		k_PositionSlicesY[3] = rectParams.rect.y;
		for (int j = 0; j < 16; j++)
		{
			int num18 = j % 4;
			int num19 = j / 4;
			meshWriteData.SetNextVertex(new Vertex
			{
				position = new Vector3(k_PositionSlicesX[num18], k_PositionSlicesY[num19], posZ),
				uv = new Vector2(k_TexCoordSlicesX[num18], k_TexCoordSlicesY[num19]),
				tint = rectParams.color
			});
		}
		meshWriteData.SetAllIndices(slicedQuadIndices);
	}

	internal static void MakeVectorGraphics(MeshGenerationContextUtils.RectangleParams rectParams, int settingIndexOffset, AllocMeshData meshAlloc, out int finalVertexCount, out int finalIndexCount)
	{
		VectorImage vectorImage = rectParams.vectorImage;
		Debug.Assert(vectorImage != null);
		finalVertexCount = 0;
		finalIndexCount = 0;
		int num = vectorImage.vertices.Length;
		Vertex[] array = new Vertex[num];
		for (int i = 0; i < num; i++)
		{
			VectorImageVertex vectorImageVertex = vectorImage.vertices[i];
			array[i] = new Vertex
			{
				position = vectorImageVertex.position,
				tint = vectorImageVertex.tint,
				uv = vectorImageVertex.uv,
				opacityPageSVGSettingIndex = new Color32(0, 0, (byte)(vectorImageVertex.settingIndex >> 8), (byte)vectorImageVertex.settingIndex)
			};
		}
		if (!((float)rectParams.leftSlice <= Mathf.Epsilon) || !((float)rectParams.topSlice <= Mathf.Epsilon) || !((float)rectParams.rightSlice <= Mathf.Epsilon) || !((float)rectParams.bottomSlice <= Mathf.Epsilon))
		{
			MakeVectorGraphics9SliceBackground(sliceLTRB: new Vector4(rectParams.leftSlice, rectParams.topSlice, rectParams.rightSlice, rectParams.bottomSlice), svgVertices: array, svgIndices: vectorImage.indices, svgWidth: vectorImage.size.x, svgHeight: vectorImage.size.y, targetRect: rectParams.rect, stretch: true, tint: rectParams.color, settingIndexOffset: settingIndexOffset, meshAlloc: meshAlloc);
			return;
		}
		MakeVectorGraphicsStretchBackground(array, vectorImage.indices, vectorImage.size.x, vectorImage.size.y, rectParams.rect, rectParams.uv, rectParams.scaleMode, rectParams.color, settingIndexOffset, meshAlloc, out finalVertexCount, out finalIndexCount);
	}

	internal static void MakeVectorGraphicsStretchBackground(Vertex[] svgVertices, ushort[] svgIndices, float svgWidth, float svgHeight, Rect targetRect, Rect sourceUV, ScaleMode scaleMode, Color tint, int settingIndexOffset, AllocMeshData meshAlloc, out int finalVertexCount, out int finalIndexCount)
	{
		Vector2 size = new Vector2(svgWidth * sourceUV.width, svgHeight * sourceUV.height);
		Vector2 vector = new Vector2(sourceUV.xMin * svgWidth, sourceUV.yMin * svgHeight);
		Rect rect = new Rect(vector, size);
		bool flag = sourceUV.xMin != 0f || sourceUV.yMin != 0f || sourceUV.width != 1f || sourceUV.height != 1f;
		float num = size.x / size.y;
		float num2 = targetRect.width / targetRect.height;
		Vector2 vector3 = default(Vector2);
		Vector2 vector2 = default(Vector2);
		switch (scaleMode)
		{
		case ScaleMode.StretchToFill:
			vector3 = new Vector2(0f, 0f);
			vector2.x = targetRect.width / size.x;
			vector2.y = targetRect.height / size.y;
			break;
		case ScaleMode.ScaleAndCrop:
			vector3 = new Vector2(0f, 0f);
			if (num2 > num)
			{
				vector2.x = (vector2.y = targetRect.width / size.x);
				float num3 = targetRect.height / vector2.y;
				float num4 = rect.height / 2f - num3 / 2f;
				vector3.y -= num4 * vector2.y;
				rect.y += num4;
				rect.height = num3;
				flag = true;
			}
			else if (num2 < num)
			{
				vector2.x = (vector2.y = targetRect.height / size.y);
				float num5 = targetRect.width / vector2.x;
				float num6 = rect.width / 2f - num5 / 2f;
				vector3.x -= num6 * vector2.x;
				rect.x += num6;
				rect.width = num5;
				flag = true;
			}
			else
			{
				vector2.x = (vector2.y = targetRect.width / size.x);
			}
			break;
		case ScaleMode.ScaleToFit:
			if (num2 > num)
			{
				vector2.x = (vector2.y = targetRect.height / size.y);
				vector3.x = (targetRect.width - size.x * vector2.x) * 0.5f;
				vector3.y = 0f;
			}
			else
			{
				vector2.x = (vector2.y = targetRect.width / size.x);
				vector3.x = 0f;
				vector3.y = (targetRect.height - size.y * vector2.y) * 0.5f;
			}
			break;
		default:
			throw new NotImplementedException();
		}
		vector3 -= vector * vector2;
		int newVertexCount = svgVertices.Length;
		int num7 = svgIndices.Length;
		ClipCounts cc = default(ClipCounts);
		Vector4 clipRectMinMax = Vector4.zero;
		if (flag)
		{
			if (rect.width <= 0f || rect.height <= 0f)
			{
				finalVertexCount = (finalIndexCount = 0);
				return;
			}
			clipRectMinMax = new Vector4(rect.xMin, rect.yMin, rect.xMax, rect.yMax);
			cc = UpperBoundApproximateRectClippingResults(svgVertices, svgIndices, clipRectMinMax);
			newVertexCount += cc.clippedTriangles * 6;
			num7 += cc.addedTriangles * 3;
			num7 -= cc.degenerateTriangles * 3;
		}
		MeshWriteData meshWriteData = meshAlloc.alloc((uint)newVertexCount, (uint)num7, ref meshAlloc);
		if (flag)
		{
			RectClip(svgVertices, svgIndices, clipRectMinMax, meshWriteData, cc, ref newVertexCount);
		}
		else
		{
			meshWriteData.SetAllIndices(svgIndices);
		}
		Rect uvRegion = meshWriteData.uvRegion;
		int num8 = svgVertices.Length;
		for (int i = 0; i < num8; i++)
		{
			Vertex nextVertex = svgVertices[i];
			nextVertex.position.x = nextVertex.position.x * vector2.x + vector3.x;
			nextVertex.position.y = nextVertex.position.y * vector2.y + vector3.y;
			nextVertex.uv.x = nextVertex.uv.x * uvRegion.width + uvRegion.xMin;
			nextVertex.uv.y = nextVertex.uv.y * uvRegion.height + uvRegion.yMin;
			ref Color32 tint2 = ref nextVertex.tint;
			tint2 *= tint;
			uint num9 = (uint)(((nextVertex.opacityPageSVGSettingIndex.b << 8) | nextVertex.opacityPageSVGSettingIndex.a) + settingIndexOffset);
			nextVertex.opacityPageSVGSettingIndex.b = (byte)(num9 >> 8);
			nextVertex.opacityPageSVGSettingIndex.a = (byte)num9;
			meshWriteData.SetNextVertex(nextVertex);
		}
		for (int j = num8; j < newVertexCount; j++)
		{
			Vertex value = meshWriteData.m_Vertices[j];
			value.position.x = value.position.x * vector2.x + vector3.x;
			value.position.y = value.position.y * vector2.y + vector3.y;
			value.uv.x = value.uv.x * uvRegion.width + uvRegion.xMin;
			value.uv.y = value.uv.y * uvRegion.height + uvRegion.yMin;
			ref Color32 tint3 = ref value.tint;
			tint3 *= tint;
			uint num10 = (uint)(((value.opacityPageSVGSettingIndex.b << 8) | value.opacityPageSVGSettingIndex.a) + settingIndexOffset);
			value.opacityPageSVGSettingIndex.b = (byte)(num10 >> 8);
			value.opacityPageSVGSettingIndex.a = (byte)num10;
			meshWriteData.m_Vertices[j] = value;
		}
		finalVertexCount = meshWriteData.vertexCount;
		finalIndexCount = meshWriteData.indexCount;
	}

	private static void MakeVectorGraphics9SliceBackground(Vertex[] svgVertices, ushort[] svgIndices, float svgWidth, float svgHeight, Rect targetRect, Vector4 sliceLTRB, bool stretch, Color tint, int settingIndexOffset, AllocMeshData meshAlloc)
	{
		MeshWriteData meshWriteData = meshAlloc.alloc((uint)svgVertices.Length, (uint)svgIndices.Length, ref meshAlloc);
		meshWriteData.SetAllIndices(svgIndices);
		if (!stretch)
		{
			throw new NotImplementedException("Support for repeating 9-slices is not done yet");
		}
		Rect uvRegion = meshWriteData.uvRegion;
		int num = svgVertices.Length;
		Vector2 vector = new Vector2(1f / (svgWidth - sliceLTRB.z - sliceLTRB.x), 1f / (svgHeight - sliceLTRB.w - sliceLTRB.y));
		Vector2 vector2 = new Vector2(targetRect.width - svgWidth, targetRect.height - svgHeight);
		Vector2 vector3 = default(Vector2);
		for (int i = 0; i < num; i++)
		{
			Vertex nextVertex = svgVertices[i];
			vector3.x = Mathf.Clamp01((nextVertex.position.x - sliceLTRB.x) * vector.x);
			vector3.y = Mathf.Clamp01((nextVertex.position.y - sliceLTRB.y) * vector.y);
			nextVertex.position.x += vector3.x * vector2.x;
			nextVertex.position.y += vector3.y * vector2.y;
			nextVertex.uv.x = nextVertex.uv.x * uvRegion.width + uvRegion.xMin;
			nextVertex.uv.y = nextVertex.uv.y * uvRegion.height + uvRegion.yMin;
			ref Color32 tint2 = ref nextVertex.tint;
			tint2 *= tint;
			uint num2 = (uint)(((nextVertex.opacityPageSVGSettingIndex.b << 8) | nextVertex.opacityPageSVGSettingIndex.a) + settingIndexOffset);
			nextVertex.opacityPageSVGSettingIndex.b = (byte)(num2 >> 8);
			nextVertex.opacityPageSVGSettingIndex.a = (byte)num2;
			meshWriteData.SetNextVertex(nextVertex);
		}
	}

	private static ClipCounts UpperBoundApproximateRectClippingResults(Vertex[] vertices, ushort[] indices, Vector4 clipRectMinMax)
	{
		ClipCounts result = new ClipCounts
		{
			firstClippedIndex = int.MaxValue,
			firstDegenerateIndex = -1,
			lastClippedIndex = -1
		};
		int num = indices.Length;
		Vector4 vector = default(Vector4);
		for (int i = 0; i < num; i += 3)
		{
			Vector3 position = vertices[indices[i]].position;
			Vector3 position2 = vertices[indices[i + 1]].position;
			Vector3 position3 = vertices[indices[i + 2]].position;
			vector.x = ((position.x < position2.x) ? position.x : position2.x);
			vector.x = ((vector.x < position3.x) ? vector.x : position3.x);
			vector.y = ((position.y < position2.y) ? position.y : position2.y);
			vector.y = ((vector.y < position3.y) ? vector.y : position3.y);
			vector.z = ((position.x > position2.x) ? position.x : position2.x);
			vector.z = ((vector.z > position3.x) ? vector.z : position3.x);
			vector.w = ((position.y > position2.y) ? position.y : position2.y);
			vector.w = ((vector.w > position3.y) ? vector.w : position3.y);
			if (vector.x >= clipRectMinMax.x && vector.z <= clipRectMinMax.z && vector.y >= clipRectMinMax.y && vector.w <= clipRectMinMax.w)
			{
				result.firstDegenerateIndex = -1;
				continue;
			}
			result.firstClippedIndex = ((result.firstClippedIndex < i) ? result.firstClippedIndex : i);
			result.lastClippedIndex = i + 2;
			if (vector.x >= clipRectMinMax.z || vector.z <= clipRectMinMax.x || vector.y >= clipRectMinMax.w || vector.w <= clipRectMinMax.y)
			{
				result.firstDegenerateIndex = ((result.firstDegenerateIndex == -1) ? i : result.firstDegenerateIndex);
				result.degenerateTriangles++;
			}
			result.firstDegenerateIndex = -1;
			result.clippedTriangles++;
			result.addedTriangles += 4;
		}
		return result;
	}

	private unsafe static void RectClip(Vertex[] vertices, ushort[] indices, Vector4 clipRectMinMax, MeshWriteData mwd, ClipCounts cc, ref int newVertexCount)
	{
		int num = cc.lastClippedIndex;
		if (cc.firstDegenerateIndex != -1 && cc.firstDegenerateIndex < num)
		{
			num = cc.firstDegenerateIndex;
		}
		ushort nextNewVertex = (ushort)vertices.Length;
		for (int i = 0; i < cc.firstClippedIndex; i++)
		{
			mwd.SetNextIndex(indices[i]);
		}
		ushort* ptr = stackalloc ushort[3];
		Vertex* ptr2 = stackalloc Vertex[3];
		Vector4 vector = default(Vector4);
		for (int j = cc.firstClippedIndex; j < num; j += 3)
		{
			*ptr = indices[j];
			ptr[1] = indices[j + 1];
			ptr[2] = indices[j + 2];
			*ptr2 = vertices[*ptr];
			ptr2[1] = vertices[ptr[1]];
			ptr2[2] = vertices[ptr[2]];
			vector.x = ((ptr2->position.x < ptr2[1].position.x) ? ptr2->position.x : ptr2[1].position.x);
			vector.x = ((vector.x < ptr2[2].position.x) ? vector.x : ptr2[2].position.x);
			vector.y = ((ptr2->position.y < ptr2[1].position.y) ? ptr2->position.y : ptr2[1].position.y);
			vector.y = ((vector.y < ptr2[2].position.y) ? vector.y : ptr2[2].position.y);
			vector.z = ((ptr2->position.x > ptr2[1].position.x) ? ptr2->position.x : ptr2[1].position.x);
			vector.z = ((vector.z > ptr2[2].position.x) ? vector.z : ptr2[2].position.x);
			vector.w = ((ptr2->position.y > ptr2[1].position.y) ? ptr2->position.y : ptr2[1].position.y);
			vector.w = ((vector.w > ptr2[2].position.y) ? vector.w : ptr2[2].position.y);
			if (vector.x >= clipRectMinMax.x && vector.z <= clipRectMinMax.z && vector.y >= clipRectMinMax.y && vector.w <= clipRectMinMax.w)
			{
				mwd.SetNextIndex(*ptr);
				mwd.SetNextIndex(ptr[1]);
				mwd.SetNextIndex(ptr[2]);
			}
			else if (!(vector.x >= clipRectMinMax.z) && !(vector.z <= clipRectMinMax.x) && !(vector.y >= clipRectMinMax.w) && !(vector.w <= clipRectMinMax.y))
			{
				RectClipTriangle(ptr2, ptr, clipRectMinMax, mwd, ref nextNewVertex);
			}
		}
		int num2 = indices.Length;
		for (int k = cc.lastClippedIndex + 1; k < num2; k++)
		{
			mwd.SetNextIndex(indices[k]);
		}
		newVertexCount = nextNewVertex;
		mwd.m_Vertices = mwd.m_Vertices.Slice(0, newVertexCount);
		mwd.m_Indices = mwd.m_Indices.Slice(0, mwd.currentIndex);
	}

	private unsafe static void RectClipTriangle(Vertex* vt, ushort* it, Vector4 clipRectMinMax, MeshWriteData mwd, ref ushort nextNewVertex)
	{
		Vertex* ptr = stackalloc Vertex[13];
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			if (vt[i].position.x >= clipRectMinMax.x && vt[i].position.y >= clipRectMinMax.y && vt[i].position.x <= clipRectMinMax.z && vt[i].position.y <= clipRectMinMax.w)
			{
				ptr[num++] = vt[i];
			}
		}
		if (num == 3)
		{
			mwd.SetNextIndex(*it);
			mwd.SetNextIndex(it[1]);
			mwd.SetNextIndex(it[2]);
			return;
		}
		Vector3 vertexBaryCentricCoordinates = GetVertexBaryCentricCoordinates(vt, clipRectMinMax.x, clipRectMinMax.y);
		Vector3 vertexBaryCentricCoordinates2 = GetVertexBaryCentricCoordinates(vt, clipRectMinMax.z, clipRectMinMax.y);
		Vector3 vertexBaryCentricCoordinates3 = GetVertexBaryCentricCoordinates(vt, clipRectMinMax.x, clipRectMinMax.w);
		Vector3 vertexBaryCentricCoordinates4 = GetVertexBaryCentricCoordinates(vt, clipRectMinMax.z, clipRectMinMax.w);
		if (vertexBaryCentricCoordinates.x >= -1E-07f && vertexBaryCentricCoordinates.x <= 1.0000001f && vertexBaryCentricCoordinates.y >= -1E-07f && vertexBaryCentricCoordinates.y <= 1.0000001f && vertexBaryCentricCoordinates.z >= -1E-07f && vertexBaryCentricCoordinates.z <= 1.0000001f)
		{
			ptr[num++] = InterpolateVertexInTriangle(vt, clipRectMinMax.x, clipRectMinMax.y, vertexBaryCentricCoordinates);
		}
		if (vertexBaryCentricCoordinates2.x >= -1E-07f && vertexBaryCentricCoordinates2.x <= 1.0000001f && vertexBaryCentricCoordinates2.y >= -1E-07f && vertexBaryCentricCoordinates2.y <= 1.0000001f && vertexBaryCentricCoordinates2.z >= -1E-07f && vertexBaryCentricCoordinates2.z <= 1.0000001f)
		{
			ptr[num++] = InterpolateVertexInTriangle(vt, clipRectMinMax.z, clipRectMinMax.y, vertexBaryCentricCoordinates2);
		}
		if (vertexBaryCentricCoordinates3.x >= -1E-07f && vertexBaryCentricCoordinates3.x <= 1.0000001f && vertexBaryCentricCoordinates3.y >= -1E-07f && vertexBaryCentricCoordinates3.y <= 1.0000001f && vertexBaryCentricCoordinates3.z >= -1E-07f && vertexBaryCentricCoordinates3.z <= 1.0000001f)
		{
			ptr[num++] = InterpolateVertexInTriangle(vt, clipRectMinMax.x, clipRectMinMax.w, vertexBaryCentricCoordinates3);
		}
		if (vertexBaryCentricCoordinates4.x >= -1E-07f && vertexBaryCentricCoordinates4.x <= 1.0000001f && vertexBaryCentricCoordinates4.y >= -1E-07f && vertexBaryCentricCoordinates4.y <= 1.0000001f && vertexBaryCentricCoordinates4.z >= -1E-07f && vertexBaryCentricCoordinates4.z <= 1.0000001f)
		{
			ptr[num++] = InterpolateVertexInTriangle(vt, clipRectMinMax.z, clipRectMinMax.w, vertexBaryCentricCoordinates4);
		}
		float num2 = IntersectSegments(vt->position.x, vt->position.y, vt[1].position.x, vt[1].position.y, clipRectMinMax.x, clipRectMinMax.y, clipRectMinMax.z, clipRectMinMax.y);
		if (num2 != float.MaxValue)
		{
			ptr[num++] = InterpolateVertexInTriangleEdge(vt, 0, 1, num2);
		}
		num2 = IntersectSegments(vt[1].position.x, vt[1].position.y, vt[2].position.x, vt[2].position.y, clipRectMinMax.x, clipRectMinMax.y, clipRectMinMax.z, clipRectMinMax.y);
		if (num2 != float.MaxValue)
		{
			ptr[num++] = InterpolateVertexInTriangleEdge(vt, 1, 2, num2);
		}
		num2 = IntersectSegments(vt[2].position.x, vt[2].position.y, vt->position.x, vt->position.y, clipRectMinMax.x, clipRectMinMax.y, clipRectMinMax.z, clipRectMinMax.y);
		if (num2 != float.MaxValue)
		{
			ptr[num++] = InterpolateVertexInTriangleEdge(vt, 2, 0, num2);
		}
		num2 = IntersectSegments(vt->position.x, vt->position.y, vt[1].position.x, vt[1].position.y, clipRectMinMax.z, clipRectMinMax.y, clipRectMinMax.z, clipRectMinMax.w);
		if (num2 != float.MaxValue)
		{
			ptr[num++] = InterpolateVertexInTriangleEdge(vt, 0, 1, num2);
		}
		num2 = IntersectSegments(vt[1].position.x, vt[1].position.y, vt[2].position.x, vt[2].position.y, clipRectMinMax.z, clipRectMinMax.y, clipRectMinMax.z, clipRectMinMax.w);
		if (num2 != float.MaxValue)
		{
			ptr[num++] = InterpolateVertexInTriangleEdge(vt, 1, 2, num2);
		}
		num2 = IntersectSegments(vt[2].position.x, vt[2].position.y, vt->position.x, vt->position.y, clipRectMinMax.z, clipRectMinMax.y, clipRectMinMax.z, clipRectMinMax.w);
		if (num2 != float.MaxValue)
		{
			ptr[num++] = InterpolateVertexInTriangleEdge(vt, 2, 0, num2);
		}
		num2 = IntersectSegments(vt->position.x, vt->position.y, vt[1].position.x, vt[1].position.y, clipRectMinMax.x, clipRectMinMax.w, clipRectMinMax.z, clipRectMinMax.w);
		if (num2 != float.MaxValue)
		{
			ptr[num++] = InterpolateVertexInTriangleEdge(vt, 0, 1, num2);
		}
		num2 = IntersectSegments(vt[1].position.x, vt[1].position.y, vt[2].position.x, vt[2].position.y, clipRectMinMax.x, clipRectMinMax.w, clipRectMinMax.z, clipRectMinMax.w);
		if (num2 != float.MaxValue)
		{
			ptr[num++] = InterpolateVertexInTriangleEdge(vt, 1, 2, num2);
		}
		num2 = IntersectSegments(vt[2].position.x, vt[2].position.y, vt->position.x, vt->position.y, clipRectMinMax.x, clipRectMinMax.w, clipRectMinMax.z, clipRectMinMax.w);
		if (num2 != float.MaxValue)
		{
			ptr[num++] = InterpolateVertexInTriangleEdge(vt, 2, 0, num2);
		}
		num2 = IntersectSegments(vt->position.x, vt->position.y, vt[1].position.x, vt[1].position.y, clipRectMinMax.x, clipRectMinMax.y, clipRectMinMax.x, clipRectMinMax.w);
		if (num2 != float.MaxValue)
		{
			ptr[num++] = InterpolateVertexInTriangleEdge(vt, 0, 1, num2);
		}
		num2 = IntersectSegments(vt[1].position.x, vt[1].position.y, vt[2].position.x, vt[2].position.y, clipRectMinMax.x, clipRectMinMax.y, clipRectMinMax.x, clipRectMinMax.w);
		if (num2 != float.MaxValue)
		{
			ptr[num++] = InterpolateVertexInTriangleEdge(vt, 1, 2, num2);
		}
		num2 = IntersectSegments(vt[2].position.x, vt[2].position.y, vt->position.x, vt->position.y, clipRectMinMax.x, clipRectMinMax.y, clipRectMinMax.x, clipRectMinMax.w);
		if (num2 != float.MaxValue)
		{
			ptr[num++] = InterpolateVertexInTriangleEdge(vt, 2, 0, num2);
		}
		if (num == 0)
		{
			return;
		}
		float* ptr2 = stackalloc float[num];
		*ptr2 = 0f;
		for (int j = 1; j < num; j++)
		{
			ptr2[j] = Mathf.Atan2(ptr[j].position.y - ptr->position.y, ptr[j].position.x - ptr->position.x);
			if (ptr2[j] < 0f)
			{
				ptr2[j] += (float)Math.PI * 2f;
			}
		}
		int* ptr3 = stackalloc int[num];
		*ptr3 = 0;
		uint num3 = 0u;
		for (int k = 1; k < num; k++)
		{
			int num4 = -1;
			float num5 = float.MaxValue;
			for (int l = 1; l < num; l++)
			{
				if ((num3 & (1 << l)) == 0L && ptr2[l] < num5)
				{
					num5 = ptr2[l];
					num4 = l;
				}
			}
			ptr3[k] = num4;
			num3 |= (uint)(1 << num4);
		}
		ushort num6 = nextNewVertex;
		for (int m = 0; m < num; m++)
		{
			mwd.m_Vertices[num6 + m] = ptr[ptr3[m]];
		}
		nextNewVertex += (ushort)num;
		int num7 = num - 2;
		bool flag = false;
		Vector3 position = mwd.m_Vertices[num6].position;
		for (int n = 0; n < num7; n++)
		{
			int num8 = num6 + n + 1;
			int num9 = num6 + n + 2;
			if (!flag)
			{
				float num10 = ptr2[ptr3[n + 1]];
				float num11 = ptr2[ptr3[n + 2]];
				if (num11 - num10 >= (float)Math.PI)
				{
					num8 = num6 + 1;
					num9 = num6 + num - 1;
					flag = true;
				}
			}
			Vector3 position2 = mwd.m_Vertices[num8].position;
			Vector3 position3 = mwd.m_Vertices[num9].position;
			Vector3 vector = Vector3.Cross(position2 - position, position3 - position);
			mwd.SetNextIndex(num6);
			if (vector.z < 0f)
			{
				mwd.SetNextIndex((ushort)num9);
				mwd.SetNextIndex((ushort)num8);
			}
			else
			{
				mwd.SetNextIndex((ushort)num8);
				mwd.SetNextIndex((ushort)num9);
			}
		}
	}

	private unsafe static Vector3 GetVertexBaryCentricCoordinates(Vertex* vt, float x, float y)
	{
		float num = vt[1].position.x - vt->position.x;
		float num2 = vt[1].position.y - vt->position.y;
		float num3 = vt[2].position.x - vt->position.x;
		float num4 = vt[2].position.y - vt->position.y;
		float num5 = x - vt->position.x;
		float num6 = y - vt->position.y;
		float num7 = num * num + num2 * num2;
		float num8 = num * num3 + num2 * num4;
		float num9 = num3 * num3 + num4 * num4;
		float num10 = num5 * num + num6 * num2;
		float num11 = num5 * num3 + num6 * num4;
		float num12 = num7 * num9 - num8 * num8;
		Vector3 result = default(Vector3);
		result.y = (num9 * num10 - num8 * num11) / num12;
		result.z = (num7 * num11 - num8 * num10) / num12;
		result.x = 1f - result.y - result.z;
		return result;
	}

	private unsafe static Vertex InterpolateVertexInTriangle(Vertex* vt, float x, float y, Vector3 uvw)
	{
		Vertex result = *vt;
		result.position.x = x;
		result.position.y = y;
		result.tint = (Color)vt->tint * uvw.x + (Color)vt[1].tint * uvw.y + (Color)vt[2].tint * uvw.z;
		result.uv = vt->uv * uvw.x + vt[1].uv * uvw.y + vt[2].uv * uvw.z;
		return result;
	}

	private unsafe static Vertex InterpolateVertexInTriangleEdge(Vertex* vt, int e0, int e1, float t)
	{
		Vertex result = *vt;
		result.position.x = vt[e0].position.x + t * (vt[e1].position.x - vt[e0].position.x);
		result.position.y = vt[e0].position.y + t * (vt[e1].position.y - vt[e0].position.y);
		result.tint = Color.LerpUnclamped(vt[e0].tint, vt[e1].tint, t);
		result.uv = Vector2.LerpUnclamped(vt[e0].uv, vt[e1].uv, t);
		return result;
	}

	private static float IntersectSegments(float ax, float ay, float bx, float by, float cx, float cy, float dx, float dy)
	{
		float num = (ax - dx) * (by - dy) - (ay - dy) * (bx - dx);
		float num2 = (ax - cx) * (by - cy) - (ay - cy) * (bx - cx);
		if (num * num2 >= 0f)
		{
			return float.MaxValue;
		}
		float num3 = (cx - ax) * (dy - ay) - (cy - ay) * (dx - ax);
		float num4 = num3 + num2 - num;
		if (num3 * num4 >= 0f)
		{
			return float.MaxValue;
		}
		return num3 / (num3 - num4);
	}
}
