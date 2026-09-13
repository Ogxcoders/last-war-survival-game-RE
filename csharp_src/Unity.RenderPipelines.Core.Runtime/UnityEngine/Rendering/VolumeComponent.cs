using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace UnityEngine.Rendering;

[Serializable]
public class VolumeComponent : ScriptableObject
{
	public bool active = true;

	[SerializeField]
	private bool m_AdvancedMode;

	public string displayName { get; protected set; } = "";

	public ReadOnlyCollection<VolumeParameter> parameters { get; private set; }

	protected virtual void OnEnable()
	{
		parameters = (from t in GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			where t.FieldType.IsSubclassOf(typeof(VolumeParameter))
			orderby t.MetadataToken
			select (VolumeParameter)t.GetValue(this)).ToList().AsReadOnly();
		foreach (VolumeParameter parameter in parameters)
		{
			parameter.OnEnable();
		}
	}

	protected virtual void OnDisable()
	{
		if (parameters == null)
		{
			return;
		}
		foreach (VolumeParameter parameter in parameters)
		{
			parameter.OnDisable();
		}
	}

	public virtual void Override(VolumeComponent state, float interpFactor)
	{
		int count = parameters.Count;
		for (int i = 0; i < count; i++)
		{
			VolumeParameter volumeParameter = state.parameters[i];
			VolumeParameter volumeParameter2 = parameters[i];
			volumeParameter.overrideState = volumeParameter2.overrideState;
			if (volumeParameter2.overrideState)
			{
				volumeParameter.Interp(volumeParameter, volumeParameter2, interpFactor);
			}
		}
	}

	public void SetAllOverridesTo(bool state)
	{
		SetAllOverridesTo(parameters, state);
	}

	private void SetAllOverridesTo(IEnumerable<VolumeParameter> enumerable, bool state)
	{
		foreach (VolumeParameter item in enumerable)
		{
			item.overrideState = state;
			Type type = item.GetType();
			if (VolumeParameter.IsObjectParameter(type))
			{
				ReadOnlyCollection<VolumeParameter> readOnlyCollection = (ReadOnlyCollection<VolumeParameter>)type.GetProperty("parameters", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(item, null);
				if (readOnlyCollection != null)
				{
					SetAllOverridesTo(readOnlyCollection, state);
				}
			}
		}
	}

	public override int GetHashCode()
	{
		int num = 17;
		for (int i = 0; i < parameters.Count; i++)
		{
			num = num * 23 + parameters[i].GetHashCode();
		}
		return num;
	}

	protected virtual void OnDestroy()
	{
		Release();
	}

	public void Release()
	{
		for (int i = 0; i < parameters.Count; i++)
		{
			parameters[i].Release();
		}
	}
}
