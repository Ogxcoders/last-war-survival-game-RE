using UnityEngine;

namespace MoreMountains.NiceVibrations;

public class SoundSwitch : MonoBehaviour
{
	public V2DemoManager DemoManager;

	protected MMSwitch _switch;

	protected virtual void Awake()
	{
		_switch = base.gameObject.GetComponent<MMSwitch>();
	}

	protected virtual void OnEnable()
	{
		if (DemoManager.SoundActive)
		{
			_switch.CurrentSwitchState = MMSwitch.SwitchStates.On;
			_switch.InitializeState();
		}
		else
		{
			_switch.CurrentSwitchState = MMSwitch.SwitchStates.Off;
			_switch.InitializeState();
		}
	}
}
