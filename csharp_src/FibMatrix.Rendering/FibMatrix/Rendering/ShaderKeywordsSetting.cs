using System.Collections.Generic;
using UnityEngine;

namespace FibMatrix.Rendering;

public class ShaderKeywordsSetting : QualitySettingGroup
{
	[SerializeReference]
	public List<ShaderKeywordsSettingItem> keywords;

	public List<Renderer> renderers;

	public List<Material> materials;

	public override void OnBeforeSerialize()
	{
		if (keywords == null)
		{
			keywords = new List<ShaderKeywordsSettingItem>();
		}
		if (materials == null)
		{
			materials = new List<Material>();
		}
		base.OnBeforeSerialize();
	}

	public override void Switch(EnQualityLevel level)
	{
		if (keywords != null)
		{
			bool flag = base[level];
			for (int i = 0; i < keywords.Count; i++)
			{
				if (keywords[i] == null || keywords[i].keywords == null || string.IsNullOrEmpty(keywords[i].keywords.Trim()))
				{
					continue;
				}
				if (keywords[i].local)
				{
					if (renderers != null)
					{
						foreach (Renderer renderer in renderers)
						{
							if (renderer != null && renderer.sharedMaterial != null)
							{
								if (flag)
								{
									renderer.sharedMaterial.EnableKeyword(keywords[i].keywords);
								}
								else
								{
									renderer.sharedMaterial.DisableKeyword(keywords[i].keywords);
								}
							}
						}
					}
					if (materials == null)
					{
						continue;
					}
					foreach (Material material in materials)
					{
						if (material != null)
						{
							if (flag)
							{
								material.EnableKeyword(keywords[i].keywords);
							}
							else
							{
								material.DisableKeyword(keywords[i].keywords);
							}
						}
					}
				}
				else if (flag)
				{
					Shader.EnableKeyword(keywords[i].keywords);
				}
				else
				{
					Shader.DisableKeyword(keywords[i].keywords);
				}
			}
		}
		base.Switch(level);
	}
}
