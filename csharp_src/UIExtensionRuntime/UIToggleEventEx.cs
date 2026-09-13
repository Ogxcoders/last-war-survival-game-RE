using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ExecuteAlways]
public class UIToggleEventEx : MonoBehaviour
{
	[Serializable]
	public class UnityEventBool : UnityEvent<bool>
	{
	}

	public UnityEventBool onValueChangedInvert;

	public UnityEventBool onValueChangedToOn;

	public UnityEventBool onValueChangedToOff;

	private void Start()
	{
		Toggle component = base.transform.GetComponent<Toggle>();
		if (component != null)
		{
			component.onValueChanged.AddListener(OnValueChange);
		}
	}

	private void OnValueChange(bool on)
	{
		onValueChangedInvert.Invoke(!on);
		if (on)
		{
			onValueChangedToOn.Invoke(arg0: true);
		}
		else
		{
			onValueChangedToOff.Invoke(arg0: false);
		}
	}
}
