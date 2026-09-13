using System;
using UnityEngine.Rendering.Universal;

namespace UnityEngine.Experimental.Rendering.Universal;

internal static class Light2DLookupTexture
{
	private static Texture2D s_PointLightLookupTexture;

	private static Texture2D s_FalloffLookupTexture;

	public static Texture2D CreatePointLightLookupTexture()
	{
		GraphicsFormat format = GraphicsFormat.R8G8B8A8_UNorm;
		if (RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.R16G16B16A16_SFloat, FormatUsage.SetPixels))
		{
			format = GraphicsFormat.R16G16B16A16_SFloat;
		}
		else if (RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.R32G32B32A32_SFloat, FormatUsage.SetPixels))
		{
			format = GraphicsFormat.R32G32B32A32_SFloat;
		}
		s_PointLightLookupTexture = new Texture2D(256, 256, format, TextureCreationFlags.None);
		s_PointLightLookupTexture.filterMode = FilterMode.Bilinear;
		s_PointLightLookupTexture.wrapMode = TextureWrapMode.Clamp;
		if (s_PointLightLookupTexture != null)
		{
			Vector2 vector = new Vector2(128f, 128f);
			for (int i = 0; (float)i < 256f; i++)
			{
				for (int j = 0; (float)j < 256f; j++)
				{
					Vector2 vector2 = new Vector2(j, i);
					float num = Vector2.Distance(vector2, vector);
					Vector2 vector3 = vector2 - vector;
					Vector2 vector4 = vector - vector2;
					vector4.Normalize();
					float r = (((float)j != 255f && (float)i != 255f) ? Mathf.Clamp(1f - 2f * num / 256f, 0f, 1f) : 0f);
					float num2 = Mathf.Acos(Vector2.Dot(Vector2.down, vector3.normalized)) / MathF.PI;
					float g = Mathf.Clamp(1f - num2, 0f, 1f);
					float x = vector4.x;
					float y = vector4.y;
					Color color = new Color(r, g, x, y);
					s_PointLightLookupTexture.SetPixel(j, i, color);
				}
			}
		}
		s_PointLightLookupTexture.Apply();
		return s_PointLightLookupTexture;
	}

	public static Texture2D CreateFalloffLookupTexture()
	{
		GraphicsFormat format = GraphicsFormat.R8G8B8A8_SRGB;
		s_FalloffLookupTexture = new Texture2D(2048, 128, format, TextureCreationFlags.None);
		s_FalloffLookupTexture.filterMode = FilterMode.Bilinear;
		s_FalloffLookupTexture.wrapMode = TextureWrapMode.Clamp;
		if (s_FalloffLookupTexture != null)
		{
			for (int i = 0; (float)i < 192f; i++)
			{
				float num = (float)(i + 32) / 256f;
				float p = Mathf.Log(0f - num + 1f) / Mathf.Log(num);
				for (int j = 0; (float)j < 2048f; j++)
				{
					float r = Mathf.Pow((float)j / 2048f, p);
					Color color = new Color(r, 0f, 0f, 1f);
					if (i >= 32 && i < 160)
					{
						s_FalloffLookupTexture.SetPixel(j, i - 32, color);
					}
				}
			}
		}
		s_FalloffLookupTexture.Apply();
		return s_FalloffLookupTexture;
	}
}
