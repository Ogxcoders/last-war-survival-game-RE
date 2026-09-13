using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace FibMatrix.Rendering;

[Serializable]
public class MaterialRenderQueueSettingItem : MaterialPropertySettingItemBase
{
	public enum BlendModePreset
	{
		Opaque,
		Cutout,
		Fade,
		Transparent
	}

	public string overrideTag;

	public BlendMode srcBlend;

	public BlendMode dstBlend;

	public CompareFunction depthTest;

	public bool depthWrite;

	public int queue;

	public int offset;

	public bool alphaTest;

	public bool alphaBlend;

	public bool alphaPremultiply;

	private string alphaTestKeywords = "_ALPHATEST_ON";

	private string alphaBlendKeywords = "_ALPHABLEND_ON";

	private string alphahaPremultiplyKeywords = "_ALPHAPREMULTIPLY_ON";

	public BlendModePreset preset;

	public override void Apply(Material material)
	{
		material.SetOverrideTag("RenderType", overrideTag);
		material.SetFloat("_SrcBlend", (float)srcBlend);
		material.SetFloat("_DstBlend", (float)dstBlend);
		material.SetFloat("_ZWrite", Convert.ToSingle(depthWrite));
		material.renderQueue = queue;
		SwithKeywords(material, alphaTestKeywords, alphaTest);
		SwithKeywords(material, alphaBlendKeywords, alphaBlend);
		SwithKeywords(material, alphahaPremultiplyKeywords, alphaPremultiply);
	}

	public void SwithKeywords(Material material, string keywords, bool state)
	{
		if (state)
		{
			material.EnableKeyword(keywords);
		}
		else
		{
			material.DisableKeyword(keywords);
		}
	}

	public void ApplyBlendModePreset()
	{
		int min = -1;
		int max = 5000;
		int num = -1;
		switch (preset)
		{
		case BlendModePreset.Opaque:
			overrideTag = string.Empty;
			srcBlend = BlendMode.One;
			dstBlend = BlendMode.Zero;
			depthTest = CompareFunction.LessEqual;
			depthWrite = true;
			alphaTest = false;
			alphaBlend = false;
			alphaPremultiply = false;
			min = -1;
			max = 2449;
			num = -1;
			break;
		case BlendModePreset.Cutout:
			overrideTag = "TransparentCutout";
			srcBlend = BlendMode.One;
			dstBlend = BlendMode.Zero;
			depthTest = CompareFunction.LessEqual;
			depthWrite = true;
			alphaTest = true;
			alphaBlend = false;
			alphaPremultiply = false;
			min = 2450;
			max = 2500;
			num = 2450;
			break;
		case BlendModePreset.Fade:
			overrideTag = "Transparent";
			srcBlend = BlendMode.SrcAlpha;
			dstBlend = BlendMode.OneMinusSrcAlpha;
			depthTest = CompareFunction.LessEqual;
			depthWrite = false;
			alphaTest = false;
			alphaBlend = true;
			alphaPremultiply = false;
			min = 2501;
			max = 3999;
			num = 3000;
			break;
		case BlendModePreset.Transparent:
			overrideTag = "Transparent";
			srcBlend = BlendMode.One;
			dstBlend = BlendMode.OneMinusSrcAlpha;
			depthTest = CompareFunction.LessEqual;
			depthWrite = false;
			alphaTest = false;
			alphaBlend = false;
			alphaPremultiply = true;
			min = 2501;
			max = 3999;
			num = 3000;
			break;
		}
		queue = Mathf.Clamp(num + offset, min, max);
	}
}
