using System.IO;
using UnityEngine;

namespace FibMatrix.Rendering;

public class TextureUtilities
{
	public static void LoadTexture(string path, ref Texture2D tex)
	{
		if (!string.IsNullOrEmpty(path))
		{
			byte[] data = File.ReadAllBytes(path);
			tex = new Texture2D(1, 1);
			tex.hideFlags = HideFlags.HideAndDontSave;
			tex.LoadImage(data);
			tex.Apply();
		}
	}

	public static void SaveTexture(string path, Texture2D tex)
	{
		if (!string.IsNullOrEmpty(path))
		{
			byte[] bytes = tex.EncodeToPNG();
			File.WriteAllBytes(path, bytes);
		}
	}

	public static Texture2D RenderTextureToTexture2D(RenderTexture renderTexture, TextureFormat textureFormat)
	{
		Texture2D obj = new Texture2D(renderTexture.width, renderTexture.height, textureFormat, 0, linear: true)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = renderTexture;
		obj.ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0);
		obj.Apply();
		RenderTexture.active = active;
		obj.wrapMode = TextureWrapMode.Clamp;
		return obj;
	}

	public static Texture2D GradientToTexture2D(Gradient gradient, ref Texture2D texture, Vector2Int textureSize, TextureFormat format)
	{
		if (texture == null || texture.width != textureSize.x || texture.height != textureSize.y || texture.format != format)
		{
			texture = new Texture2D(textureSize.x, textureSize.y, format, mipChain: false);
		}
		for (int i = 0; i < textureSize.x; i++)
		{
			Color color = gradient.Evaluate((float)i / (float)textureSize.x);
			for (int j = 0; j < textureSize.y; j++)
			{
				texture.SetPixel(i, j, color);
			}
		}
		texture.Apply();
		return texture;
	}

	public static TextureFormat GetSupportsTextureFormat()
	{
		if (SystemInfo.SupportsTextureFormat(TextureFormat.RHalf))
		{
			return TextureFormat.RHalf;
		}
		if (SystemInfo.SupportsTextureFormat(TextureFormat.R8))
		{
			return TextureFormat.R8;
		}
		return TextureFormat.ARGB32;
	}

	public static RenderTextureFormat GetSupportsRenderTextureFormat()
	{
		if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RHalf))
		{
			return RenderTextureFormat.RHalf;
		}
		if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8))
		{
			return RenderTextureFormat.R8;
		}
		return RenderTextureFormat.ARGB32;
	}
}
