using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MoreMountains.NiceVibrations;

public class MMSwitch : MMTouchButton
{
	public enum SwitchStates
	{
		Off,
		On
	}

	[Header("Switch")]
	public Image SwitchKnob;

	[Header("Knob")]
	public SwitchStates InitialState;

	public Transform OffPosition;

	public Transform OnPosition;

	public AnimationCurve KnobMovementCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	public float KnobMovementDuration = 0.2f;

	[Header("Binding")]
	public UnityEvent SwitchOn;

	public UnityEvent SwitchOff;

	protected float _knobMovementStartedAt = -50f;

	public SwitchStates CurrentSwitchState { get; set; }

	protected override void Initialization()
	{
		base.Initialization();
		CurrentSwitchState = InitialState;
		InitializeState();
	}

	public virtual void InitializeState()
	{
		if (CurrentSwitchState == SwitchStates.Off)
		{
			if (_animator != null)
			{
				_animator.Play("RollLeft");
			}
			SwitchKnob.transform.position = OffPosition.transform.position;
		}
		else
		{
			if (_animator != null)
			{
				_animator.Play("RollRight");
			}
			SwitchKnob.transform.position = OnPosition.transform.position;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (Time.time - _knobMovementStartedAt < KnobMovementDuration)
		{
			float time = Remap(Time.time - _knobMovementStartedAt, 0f, KnobMovementDuration, 0f, 1f);
			float t = KnobMovementCurve.Evaluate(time);
			if (CurrentSwitchState == SwitchStates.Off)
			{
				SwitchKnob.transform.position = Vector3.Lerp(OnPosition.transform.position, OffPosition.transform.position, t);
			}
			else
			{
				SwitchKnob.transform.position = Vector3.Lerp(OffPosition.transform.position, OnPosition.transform.position, t);
			}
		}
	}

	public virtual void SwitchState()
	{
		_knobMovementStartedAt = Time.time;
		if (CurrentSwitchState == SwitchStates.Off)
		{
			CurrentSwitchState = SwitchStates.On;
			if (_animator != null)
			{
				_animator?.SetTrigger("Right");
			}
			if (SwitchOn != null)
			{
				SwitchOn.Invoke();
			}
		}
		else
		{
			CurrentSwitchState = SwitchStates.Off;
			if (_animator != null)
			{
				_animator?.SetTrigger("Left");
			}
			if (SwitchOff != null)
			{
				SwitchOff.Invoke();
			}
		}
	}
}
