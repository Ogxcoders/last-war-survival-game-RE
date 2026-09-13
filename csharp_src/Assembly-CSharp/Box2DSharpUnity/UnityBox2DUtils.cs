using System;
using System.Collections.Generic;
using System.IO;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using UnityEngine;

namespace Box2DSharpUnity;

public static class UnityBox2DUtils
{
	public static void SaveTextureAsPNG(Texture2D texture, string filePath)
	{
		if (texture == null)
		{
			Debug.LogWarning("Texture is null, cannot save.");
			return;
		}
		byte[] array = texture.EncodeToPNG();
		if (array == null)
		{
			Debug.LogWarning("Failed to encode texture to PNG.");
			return;
		}
		try
		{
			File.WriteAllBytes(filePath, array);
			Debug.Log("Texture saved successfully to: " + filePath);
		}
		catch (Exception ex)
		{
			Debug.LogError("Failed to save texture to: " + filePath + ". Error: " + ex.Message);
		}
	}

	public static Texture2D CreateWallSDF(Body body, int width, int height, float worldRange = 10f, float sdfRange = 0.3f)
	{
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.Alpha8, mipChain: false);
		List<(Vector2, Vector2)> list = ExtractChainEdges(body);
		Vector2 worldMin = new Vector2(0f - worldRange, 0f - worldRange);
		Vector2 worldMax = new Vector2(worldRange, worldRange);
		float[] array = new float[width * height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = sdfRange;
		}
		HashSet<Vector2Int> hashSet = new HashSet<Vector2Int>();
		Queue<Vector2Int> queue = new Queue<Vector2Int>();
		foreach (var item5 in list)
		{
			Vector2 item = item5.Item1;
			Vector2 item2 = item5.Item2;
			int num = Mathf.CeilToInt(Vector2.Distance(item, item2) / Mathf.Min(worldRange * 2f / (float)width, worldRange * 2f / (float)height));
			for (int j = 0; j <= num; j++)
			{
				Vector2Int item3 = WorldToPixel(Vector2.Lerp(item, item2, (float)j / (float)num), width, height, worldMin, worldMax);
				if (!hashSet.Contains(item3))
				{
					hashSet.Add(item3);
					array[item3.x + item3.y * width] = 0f;
					queue.Enqueue(item3);
				}
			}
		}
		Vector2Int[] array2 = new Vector2Int[4]
		{
			new Vector2Int(1, 0),
			new Vector2Int(-1, 0),
			new Vector2Int(0, 1),
			new Vector2Int(0, -1)
		};
		float num2 = worldRange * 2f / (float)Mathf.Max(width, height);
		while (queue.Count > 0)
		{
			Vector2Int vector2Int = queue.Dequeue();
			float num3 = array[vector2Int.x + vector2Int.y * width];
			Vector2Int[] array3 = array2;
			foreach (Vector2Int vector2Int2 in array3)
			{
				Vector2Int item4 = vector2Int + vector2Int2;
				float num4 = num3 + num2;
				if (item4.x >= 0 && item4.x < width && item4.y >= 0 && item4.y < height && array[item4.x + item4.y * width] > num4)
				{
					array[item4.x + item4.y * width] = num4;
					queue.Enqueue(item4);
				}
			}
		}
		int num5 = 0;
		int num6 = 0;
		for (int l = 0; l < height; l++)
		{
			for (int m = 0; m < width; m++)
			{
				byte b = (byte)(Mathf.Clamp01(array[m + l * width] / sdfRange) * 255f);
				if (b > 128)
				{
					num6++;
				}
				else
				{
					num5++;
				}
				texture2D.SetPixel(m, l, new Color32(0, 0, 0, b));
			}
		}
		texture2D.Apply();
		return texture2D;
	}

	private static List<(Vector2, Vector2)> ExtractChainEdges(Body body)
	{
		List<(Vector2, Vector2)> list = new List<(Vector2, Vector2)>();
		foreach (Fixture fixture in body.FixtureList)
		{
			if (fixture.Shape is ChainShape chainShape)
			{
				for (int i = 0; i < chainShape.Count - 1; i++)
				{
					FVector2 fVector = chainShape.Vertices[i];
					FVector2 fVector2 = chainShape.Vertices[i + 1];
					list.Add((new Vector2(fVector.X.AsFloat, fVector.Y.AsFloat), new Vector2(fVector2.X.AsFloat, fVector2.Y.AsFloat)));
				}
			}
		}
		return list;
	}

	private static Vector2 PixelToWorld(int x, int y, int width, int height, Vector2 worldMin, Vector2 worldMax)
	{
		float x2 = Mathf.Lerp(worldMin.x, worldMax.x, (float)x / (float)(width - 1));
		float y2 = Mathf.Lerp(worldMin.y, worldMax.y, (float)y / (float)(height - 1));
		return new Vector2(x2, y2);
	}

	private static Vector2Int WorldToPixel(Vector2 worldPos, int texWidth, int texHeight, Vector2 worldMin, Vector2 worldMax)
	{
		int x = Mathf.RoundToInt((worldPos.x - worldMin.x) / (worldMax.x - worldMin.x) * (float)(texWidth - 1));
		int y = Mathf.RoundToInt((worldPos.y - worldMin.y) / (worldMax.y - worldMin.y) * (float)(texHeight - 1));
		return new Vector2Int(x, y);
	}

	private static float DistanceToSegment(Vector2 point, Vector2 start, Vector2 end, out bool inSegment)
	{
		Vector2 vector = end - start;
		float sqrMagnitude = vector.sqrMagnitude;
		if (Mathf.Approximately(sqrMagnitude, 0f))
		{
			inSegment = true;
			return Vector2.Distance(point, start);
		}
		float num = Vector2.Dot(point - start, vector) / sqrMagnitude;
		inSegment = num >= 0f && num <= 1f;
		float num2 = Mathf.Clamp01(num);
		Vector2 b = start + num2 * vector;
		return Vector2.Distance(point, b);
	}

	private static bool IsPointRightOfLine(Vector2 point, Vector2 start, Vector2 end)
	{
		return (point.x - start.x) * (end.y - start.y) - (point.y - start.y) * (end.x - start.x) < 0f;
	}
}
