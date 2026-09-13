using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.TerrainAPI;

public static class TerrainPaintUtility
{
	public enum BuiltinPaintMaterialPasses
	{
		RaiseLowerHeight,
		StampHeight,
		SetHeights,
		SmoothHeights,
		PaintTexture,
		PaintHoles
	}

	private static Material s_BuiltinPaintMaterial = null;

	private static Material s_BlitMaterial = null;

	private static Material s_HeightBlitMaterial = null;

	private static Material s_CopyTerrainLayerMaterial = null;

	internal static bool paintTextureUsesCopyTexture => (SystemInfo.copyTextureSupport & (CopyTextureSupport.TextureToRT | CopyTextureSupport.RTToTexture)) == (CopyTextureSupport.TextureToRT | CopyTextureSupport.RTToTexture);

	public static Material GetBuiltinPaintMaterial()
	{
		if (s_BuiltinPaintMaterial == null)
		{
			s_BuiltinPaintMaterial = new Material(Shader.Find("Hidden/TerrainEngine/PaintHeight"));
		}
		return s_BuiltinPaintMaterial;
	}

	public static void GetBrushWorldSizeLimits(out float minBrushWorldSize, out float maxBrushWorldSize, float terrainTileWorldSize, int terrainTileTextureResolutionPixels, int minBrushResolutionPixels = 1, int maxBrushResolutionPixels = 8192)
	{
		if (terrainTileTextureResolutionPixels <= 0)
		{
			minBrushWorldSize = terrainTileWorldSize;
			maxBrushWorldSize = terrainTileWorldSize;
			return;
		}
		float num = terrainTileWorldSize / (float)terrainTileTextureResolutionPixels;
		minBrushWorldSize = (float)minBrushResolutionPixels * num;
		float num2 = Mathf.Min(maxBrushResolutionPixels, SystemInfo.maxTextureSize);
		maxBrushWorldSize = num2 * num;
	}

	public static BrushTransform CalculateBrushTransform(Terrain terrain, Vector2 brushCenterTerrainUV, float brushSize, float brushRotationDegrees)
	{
		float f = brushRotationDegrees * ((float)Math.PI / 180f);
		float num = Mathf.Cos(f);
		float num2 = Mathf.Sin(f);
		Vector2 vector = new Vector2(num, 0f - num2) * brushSize;
		Vector2 vector2 = new Vector2(num2, num) * brushSize;
		Vector3 size = terrain.terrainData.size;
		Vector2 vector3 = brushCenterTerrainUV * new Vector2(size.x, size.z);
		Vector2 brushOrigin = vector3 - 0.5f * vector - 0.5f * vector2;
		return new BrushTransform(brushOrigin, vector, vector2);
	}

	public static void BuildTransformPaintContextUVToPaintContextUV(PaintContext src, PaintContext dst, out Vector4 scaleOffset)
	{
		float num = ((float)src.pixelRect.xMin - 0.5f) * src.pixelSize.x;
		float num2 = ((float)src.pixelRect.yMin - 0.5f) * src.pixelSize.y;
		float num3 = (float)src.pixelRect.width * src.pixelSize.x;
		float num4 = (float)src.pixelRect.height * src.pixelSize.y;
		float num5 = ((float)dst.pixelRect.xMin - 0.5f) * dst.pixelSize.x;
		float num6 = ((float)dst.pixelRect.yMin - 0.5f) * dst.pixelSize.y;
		float num7 = (float)dst.pixelRect.width * dst.pixelSize.x;
		float num8 = (float)dst.pixelRect.height * dst.pixelSize.y;
		scaleOffset = new Vector4(num3 / num7, num4 / num8, (num - num5) / num7, (num2 - num6) / num8);
	}

	public static void SetupTerrainToolMaterialProperties(PaintContext paintContext, BrushTransform brushXform, Material material)
	{
		float num = ((float)paintContext.pixelRect.xMin - 0.5f) * paintContext.pixelSize.x;
		float num2 = ((float)paintContext.pixelRect.yMin - 0.5f) * paintContext.pixelSize.y;
		float num3 = (float)paintContext.pixelRect.width * paintContext.pixelSize.x;
		float num4 = (float)paintContext.pixelRect.height * paintContext.pixelSize.y;
		Vector2 vector = num3 * brushXform.targetX;
		Vector2 vector2 = num4 * brushXform.targetY;
		Vector2 vector3 = brushXform.targetOrigin + num * brushXform.targetX + num2 * brushXform.targetY;
		material.SetVector("_PCUVToBrushUVScales", new Vector4(vector.x, vector.y, vector2.x, vector2.y));
		material.SetVector("_PCUVToBrushUVOffset", new Vector4(vector3.x, vector3.y, 0f, 0f));
	}

	internal static PaintContext InitializePaintContext(Terrain terrain, int targetWidth, int targetHeight, RenderTextureFormat pcFormat, Rect boundsInTerrainSpace, int extraBorderPixels = 0, bool texelPadding = true)
	{
		PaintContext paintContext = PaintContext.CreateFromBounds(terrain, boundsInTerrainSpace, targetWidth, targetHeight, extraBorderPixels, texelPadding);
		paintContext.CreateRenderTargets(pcFormat);
		return paintContext;
	}

	public static void ReleaseContextResources(PaintContext ctx)
	{
		ctx.Cleanup();
	}

	public static PaintContext BeginPaintHeightmap(Terrain terrain, Rect boundsInTerrainSpace, int extraBorderPixels = 0)
	{
		int heightmapResolution = terrain.terrainData.heightmapResolution;
		PaintContext paintContext = InitializePaintContext(terrain, heightmapResolution, heightmapResolution, Terrain.heightmapRenderTextureFormat, boundsInTerrainSpace, extraBorderPixels);
		paintContext.GatherHeightmap();
		return paintContext;
	}

	public static void EndPaintHeightmap(PaintContext ctx, string editorUndoName)
	{
		ctx.ScatterHeightmap(editorUndoName);
		ctx.Cleanup();
	}

	public static PaintContext BeginPaintHoles(Terrain terrain, Rect boundsInTerrainSpace, int extraBorderPixels = 0)
	{
		int holesResolution = terrain.terrainData.holesResolution;
		PaintContext paintContext = InitializePaintContext(terrain, holesResolution, holesResolution, Terrain.holesRenderTextureFormat, boundsInTerrainSpace, extraBorderPixels, texelPadding: false);
		paintContext.GatherHoles();
		return paintContext;
	}

	public static void EndPaintHoles(PaintContext ctx, string editorUndoName)
	{
		ctx.ScatterHoles(editorUndoName);
		ctx.Cleanup();
	}

	public static PaintContext CollectNormals(Terrain terrain, Rect boundsInTerrainSpace, int extraBorderPixels = 0)
	{
		int heightmapResolution = terrain.terrainData.heightmapResolution;
		PaintContext paintContext = InitializePaintContext(terrain, heightmapResolution, heightmapResolution, Terrain.normalmapRenderTextureFormat, boundsInTerrainSpace, extraBorderPixels);
		paintContext.GatherNormals();
		return paintContext;
	}

	public static PaintContext BeginPaintTexture(Terrain terrain, Rect boundsInTerrainSpace, TerrainLayer inputLayer, int extraBorderPixels = 0)
	{
		if (inputLayer == null)
		{
			return null;
		}
		int alphamapResolution = terrain.terrainData.alphamapResolution;
		PaintContext paintContext = InitializePaintContext(terrain, alphamapResolution, alphamapResolution, RenderTextureFormat.R8, boundsInTerrainSpace, extraBorderPixels);
		paintContext.GatherAlphamap(inputLayer);
		return paintContext;
	}

	public static void EndPaintTexture(PaintContext ctx, string editorUndoName)
	{
		ctx.ScatterAlphamap(editorUndoName);
		ctx.Cleanup();
	}

	public static Material GetBlitMaterial()
	{
		if (!s_BlitMaterial)
		{
			s_BlitMaterial = new Material(Shader.Find("Hidden/BlitCopy"));
		}
		return s_BlitMaterial;
	}

	public static Material GetHeightBlitMaterial()
	{
		if (!s_HeightBlitMaterial)
		{
			s_HeightBlitMaterial = new Material(Shader.Find("Hidden/TerrainEngine/HeightBlitCopy"));
		}
		return s_HeightBlitMaterial;
	}

	public static Material GetCopyTerrainLayerMaterial()
	{
		if (!s_CopyTerrainLayerMaterial)
		{
			s_CopyTerrainLayerMaterial = new Material(Shader.Find("Hidden/TerrainEngine/TerrainLayerUtils"));
		}
		return s_CopyTerrainLayerMaterial;
	}

	internal static void DrawQuad(RectInt destinationPixels, RectInt sourcePixels, Texture sourceTexture)
	{
		DrawQuad2(destinationPixels, sourcePixels, sourceTexture, sourcePixels, sourceTexture);
	}

	internal static void DrawQuad2(RectInt destinationPixels, RectInt sourcePixels, Texture sourceTexture, RectInt sourcePixels2, Texture sourceTexture2)
	{
		if (destinationPixels.width > 0 && destinationPixels.height > 0)
		{
			Rect rect = new Rect((float)sourcePixels.x / (float)sourceTexture.width, (float)sourcePixels.y / (float)sourceTexture.height, (float)sourcePixels.width / (float)sourceTexture.width, (float)sourcePixels.height / (float)sourceTexture.height);
			Rect rect2 = new Rect((float)sourcePixels2.x / (float)sourceTexture2.width, (float)sourcePixels2.y / (float)sourceTexture2.height, (float)sourcePixels2.width / (float)sourceTexture2.width, (float)sourcePixels2.height / (float)sourceTexture2.height);
			GL.Begin(7);
			GL.Color(new Color(1f, 1f, 1f, 1f));
			GL.MultiTexCoord2(0, rect.x, rect.y);
			GL.MultiTexCoord2(1, rect2.x, rect2.y);
			GL.Vertex3(destinationPixels.x, destinationPixels.y, 0f);
			GL.MultiTexCoord2(0, rect.x, rect.yMax);
			GL.MultiTexCoord2(1, rect2.x, rect2.yMax);
			GL.Vertex3(destinationPixels.x, destinationPixels.yMax, 0f);
			GL.MultiTexCoord2(0, rect.xMax, rect.yMax);
			GL.MultiTexCoord2(1, rect2.xMax, rect2.yMax);
			GL.Vertex3(destinationPixels.xMax, destinationPixels.yMax, 0f);
			GL.MultiTexCoord2(0, rect.xMax, rect.y);
			GL.MultiTexCoord2(1, rect2.xMax, rect2.y);
			GL.Vertex3(destinationPixels.xMax, destinationPixels.y, 0f);
			GL.End();
		}
	}

	internal static RectInt CalcPixelRectFromBounds(Terrain terrain, Rect boundsInTerrainSpace, int textureWidth, int textureHeight, int extraBorderPixels, bool texelPadding)
	{
		float num = ((float)textureWidth - (texelPadding ? 1f : 0f)) / terrain.terrainData.size.x;
		float num2 = ((float)textureHeight - (texelPadding ? 1f : 0f)) / terrain.terrainData.size.z;
		int num3 = Mathf.FloorToInt(boundsInTerrainSpace.xMin * num) - extraBorderPixels;
		int num4 = Mathf.FloorToInt(boundsInTerrainSpace.yMin * num2) - extraBorderPixels;
		int num5 = Mathf.CeilToInt(boundsInTerrainSpace.xMax * num) + extraBorderPixels;
		int num6 = Mathf.CeilToInt(boundsInTerrainSpace.yMax * num2) + extraBorderPixels;
		return new RectInt(num3, num4, num5 - num3 + 1, num6 - num4 + 1);
	}

	public static Texture2D GetTerrainAlphaMapChecked(Terrain terrain, int mapIndex)
	{
		if (mapIndex >= terrain.terrainData.alphamapTextureCount)
		{
			throw new ArgumentException("Trying to access out-of-bounds terrain alphamap information.");
		}
		return terrain.terrainData.GetAlphamapTexture(mapIndex);
	}

	public static int FindTerrainLayerIndex(Terrain terrain, TerrainLayer inputLayer)
	{
		TerrainLayer[] terrainLayers = terrain.terrainData.terrainLayers;
		for (int i = 0; i < terrainLayers.Length; i++)
		{
			if (terrainLayers[i] == inputLayer)
			{
				return i;
			}
		}
		return -1;
	}

	internal static int AddTerrainLayer(Terrain terrain, TerrainLayer inputLayer)
	{
		TerrainLayer[] terrainLayers = terrain.terrainData.terrainLayers;
		int num = terrainLayers.Length;
		TerrainLayer[] array = new TerrainLayer[num + 1];
		Array.Copy(terrainLayers, 0, array, 0, num);
		array[num] = inputLayer;
		terrain.terrainData.terrainLayers = array;
		return num;
	}
}
