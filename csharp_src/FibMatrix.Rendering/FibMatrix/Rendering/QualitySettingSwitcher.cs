using System.Collections.Generic;
using UnityEngine;

namespace FibMatrix.Rendering;

[HelpURL("https://rivergame.feishu.cn/wiki/wikcnIUhvM8w66J8lJBKgcy1uqf")]
[ExecuteAlways]
[DefaultExecutionOrder(9999)]
public class QualitySettingSwitcher : MonoBehaviour
{
	[SerializeReference]
	public List<QualitySettingGroup> qualitySettings;

	private static EnQualityLevel CurrentLevel
	{
		get
		{
			return RenderQualitySetting.CurrentLevel;
		}
		set
		{
			RenderQualitySetting.SwitchQualityLevel(value);
		}
	}

	private object QualitySettingSwitcheres => QualitySettingSwitcherCenter.QualitySettingSwitcheres;

	private object RenderStateRegisters => RenderStateRegisterCenter.RenderStateRegisters;

	private void Awake()
	{
		if (qualitySettings == null)
		{
			qualitySettings = new List<QualitySettingGroup>();
		}
	}

	private void OnEnable()
	{
		if (CurrentLevel == EnQualityLevel.Unknown)
		{
			RenderQualitySetting.SwitchQualityLevel((EnQualityLevel)QualitySettings.GetQualityLevel());
		}
		if (qualitySettings != null)
		{
			for (int i = 0; i < qualitySettings.Count; i++)
			{
				qualitySettings[i]?.OnEnable();
			}
		}
		QualitySettingSwitcherCenter.QualitySettingSwitcheres[this] = qualitySettings;
	}

	private void OnDisable()
	{
		if (qualitySettings != null)
		{
			for (int i = 0; i < qualitySettings.Count; i++)
			{
				qualitySettings[i]?.OnDisable();
			}
		}
		QualitySettingSwitcherCenter.QualitySettingSwitcheres.Remove(this);
	}

	internal void SwitchInternal(EnQualityLevel level)
	{
		if (qualitySettings != null)
		{
			for (int i = 0; i < qualitySettings.Count; i++)
			{
				qualitySettings[i]?.Switch(level);
			}
		}
	}
}
