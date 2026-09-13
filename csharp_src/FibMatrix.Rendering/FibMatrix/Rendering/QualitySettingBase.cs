using System;
using UnityEngine;

namespace FibMatrix.Rendering;

[Serializable]
public abstract class QualitySettingBase : ISerializationCallbackReceiver
{
	public string description;

	public QualityLevelFlags flags;

	public virtual bool showDescription => false;

	public bool this[EnQualityLevel level] => flags.HasFlag((QualityLevelFlags)(1 << (int)level));

	public virtual void OnBeforeSerialize()
	{
	}

	public virtual void OnAfterDeserialize()
	{
	}

	public virtual void OnEnable()
	{
		OnSwitch();
	}

	public virtual void OnDisable()
	{
	}

	public virtual void OnSwitch()
	{
		Switch(RenderQualitySetting.CurrentLevel);
	}

	public virtual void Switch(EnQualityLevel level)
	{
	}
}
