using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class BlurOptimized : MonoBehaviour
{
	public enum BlurType
	{
		StandardGauss,
		SgxGauss
	}

	[Range(0f, 10f)]
	public float blurSize = 3f;

	[Range(1f, 4f)]
	public int blurIterations = 2;

	public BlurType blurType;

	public Material blurMaterial;

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		int num = 0;
		float num2 = 1f / (1f * (float)(1 << num));
		blurMaterial.SetVector("_Parameter", new Vector4(blurSize * num2, (0f - blurSize) * num2, 0f, 0f));
		source.filterMode = FilterMode.Bilinear;
		int width = source.width >> num;
		int height = source.height >> num;
		RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
		renderTexture.filterMode = FilterMode.Bilinear;
		Graphics.Blit(source, renderTexture, blurMaterial, 0);
		int num3 = ((blurType != BlurType.StandardGauss) ? 2 : 0);
		for (int i = 0; i < blurIterations; i++)
		{
			float num4 = (float)i * 1f;
			blurMaterial.SetVector("_Parameter", new Vector4(blurSize * num2 + num4, (0f - blurSize) * num2 - num4, 0f, 0f));
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
			temporary.filterMode = FilterMode.Bilinear;
			Graphics.Blit(renderTexture, temporary, blurMaterial, 1 + num3);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
			temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
			temporary.filterMode = FilterMode.Bilinear;
			Graphics.Blit(renderTexture, temporary, blurMaterial, 2 + num3);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
		}
		Graphics.Blit(renderTexture, destination);
		RenderTexture.ReleaseTemporary(renderTexture);
	}
}
