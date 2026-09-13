using System;

namespace UnityEngine.Rendering.Universal;

public static class PostProcessUtils
{
	private static class ShaderConstants
	{
		public static readonly int _Grain_Texture = Shader.PropertyToID("_Grain_Texture");

		public static readonly int _Grain_Params = Shader.PropertyToID("_Grain_Params");

		public static readonly int _Grain_TilingParams = Shader.PropertyToID("_Grain_TilingParams");

		public static readonly int _BlueNoise_Texture = Shader.PropertyToID("_BlueNoise_Texture");

		public static readonly int _Dithering_Params = Shader.PropertyToID("_Dithering_Params");
	}

	[Obsolete("This method is obsolete. Use ConfigureDithering override that takes camera pixel width and height instead.")]
	public static int ConfigureDithering(PostProcessData data, int index, Camera camera, Material material)
	{
		return ConfigureDithering(data, index, camera.pixelWidth, camera.pixelHeight, material);
	}

	public static int ConfigureDithering(PostProcessData data, int index, int cameraPixelWidth, int cameraPixelHeight, Material material)
	{
		Texture2D[] blueNoise16LTex = data.textures.blueNoise16LTex;
		if (blueNoise16LTex == null || blueNoise16LTex.Length == 0)
		{
			return 0;
		}
		if (++index >= blueNoise16LTex.Length)
		{
			index = 0;
		}
		float value = Random.value;
		float value2 = Random.value;
		Texture2D texture2D = blueNoise16LTex[index];
		material.SetTexture(ShaderConstants._BlueNoise_Texture, texture2D);
		material.SetVector(ShaderConstants._Dithering_Params, new Vector4((float)cameraPixelWidth / (float)texture2D.width, (float)cameraPixelHeight / (float)texture2D.height, value, value2));
		return index;
	}

	[Obsolete("This method is obsolete. Use ConfigureFilmGrain override that takes camera pixel width and height instead.")]
	public static void ConfigureFilmGrain(PostProcessData data, FilmGrain settings, Camera camera, Material material)
	{
		ConfigureFilmGrain(data, settings, camera.pixelWidth, camera.pixelHeight, material);
	}

	public static void ConfigureFilmGrain(PostProcessData data, FilmGrain settings, int cameraPixelWidth, int cameraPixelHeight, Material material)
	{
		Texture texture = settings.texture.value;
		if (settings.type.value != FilmGrainLookup.Custom)
		{
			texture = data.textures.filmGrainTex[(int)settings.type.value];
		}
		float value = Random.value;
		float value2 = Random.value;
		Vector4 value3 = ((texture == null) ? Vector4.zero : new Vector4((float)cameraPixelWidth / (float)texture.width, (float)cameraPixelHeight / (float)texture.height, value, value2));
		material.SetTexture(ShaderConstants._Grain_Texture, texture);
		material.SetVector(ShaderConstants._Grain_Params, new Vector2(settings.intensity.value * 4f, settings.response.value));
		material.SetVector(ShaderConstants._Grain_TilingParams, value3);
	}

	public static float Exp2(float x)
	{
		return Mathf.Exp(x * 0.6931472f);
	}
}
