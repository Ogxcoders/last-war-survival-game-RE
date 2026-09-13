using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

namespace FibMatrix.Rendering;

public class PostProcessSetting : QualitySettingGroup
{
	[SerializeReference]
	public List<VolumeComponentSettingBase> postProcessStates;

	private WeakReference<List<Volume>> m_Volumes = new WeakReference<List<Volume>>(null);

	public override void OnBeforeSerialize()
	{
		base.OnBeforeSerialize();
		if (postProcessStates == null)
		{
			postProcessStates = new List<VolumeComponentSettingBase>();
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();
		RegisterPostProcessState(base[RenderQualitySetting.CurrentLevel]);
	}

	public override void OnDisable()
	{
		RegisterPostProcessState(enable: false);
		base.OnDisable();
	}

	private void RegisterPostProcessState(bool enable)
	{
		if (enable)
		{
			RenderQualitySetting.PostProcessStateRegister.Register(this);
		}
		else
		{
			RenderQualitySetting.PostProcessStateRegister.Deregister(this);
		}
	}

	public override void Switch(EnQualityLevel level)
	{
		RegisterPostProcessState(base[level]);
		if (m_Volumes.TryGetTarget(out var target) || target == null)
		{
			target = typeof(VolumeManager).GetField("m_Volumes", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(VolumeManager.instance) as List<Volume>;
			m_Volumes.SetTarget(target);
		}
		if (postProcessStates != null)
		{
			foreach (VolumeComponentSettingBase postProcessState in postProcessStates)
			{
				if (postProcessState != null)
				{
					postProcessState.Volumes = m_Volumes;
					postProcessState.Switch(level);
				}
			}
		}
		base.Switch(level);
	}
}
