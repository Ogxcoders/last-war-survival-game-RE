using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.NiceVibrations;

public class CarDemoManager : DemoManager
{
	[Header("Control")]
	public MMKnob Knob;

	public float MinimumKnobValue = 0.1f;

	public float MaximumPowerDuration = 10f;

	public float ChargingSpeed = 2f;

	public float CarSpeed;

	public float Power;

	public float StartClickDuration = 0.2f;

	public float DentDuration = 0.1f;

	public List<float> Dents;

	[Header("Car")]
	public AudioSource CarEngineAudioSource;

	public Transform LeftWheel;

	public Transform RightWheel;

	public RectTransform CarBody;

	public Vector3 WheelRotationSpeed = new Vector3(0f, 0f, 50f);

	[Header("UI")]
	public GameObject ReloadingPrompt;

	public AnimationCurve StartClickCurve;

	public MMProgressBar PowerBar;

	public List<PowerBarElement> SpeedBars;

	public Color ActiveColor;

	public Color InactiveColor;

	[Header("Debug")]
	public bool _carStarted;

	public float _carStartedAt;

	public float _lastStartClickAt;

	protected float _knobValueLastFrame;

	protected float _lastDentAt;

	protected float _knobValue;

	protected Vector3 _initialCarPosition;

	protected Vector3 _carPosition;

	protected virtual void Awake()
	{
		Power = MaximumPowerDuration;
		ReloadingPrompt.SetActive(value: false);
		_initialCarPosition = CarBody.localPosition;
	}

	protected virtual void Update()
	{
		HandlePower();
		UpdateCar();
		UpdateUI();
		_knobValueLastFrame = Knob.Value;
	}

	protected virtual void HandlePower()
	{
		_knobValue = (Knob.Active ? Knob.Value : 0f);
		if (!_carStarted)
		{
			if (_knobValue > MinimumKnobValue && Knob.Active)
			{
				_carStarted = true;
				_carStartedAt = Time.time;
				_lastStartClickAt = Time.time;
				MMVibrationManager.ContinuousHaptic(_knobValue, _knobValue, MaximumPowerDuration, HapticTypes.MediumImpact, this);
				CarEngineAudioSource.Play();
				return;
			}
			Power += Time.deltaTime * ChargingSpeed;
			Power = Mathf.Clamp(Power, 0f, MaximumPowerDuration);
			if (Power == MaximumPowerDuration)
			{
				Knob.SetActive(status: true);
				Knob._rectTransform.localScale = Vector3.one;
				ReloadingPrompt.SetActive(value: false);
			}
			else if (!Knob.Active)
			{
				Knob.SetValue(CarSpeed);
			}
		}
		else if (Time.time - _carStartedAt > MaximumPowerDuration)
		{
			_carStarted = false;
			Knob.SetActive(status: false);
			Knob._rectTransform.localScale = Vector3.one * 0.9f;
			ReloadingPrompt.SetActive(value: true);
		}
		else if (_knobValue > MinimumKnobValue)
		{
			Power -= Time.deltaTime;
			Power = Mathf.Clamp(Power, 0f, MaximumPowerDuration);
			MMVibrationManager.UpdateContinuousHaptic(_knobValue, _knobValue, alsoRumble: true);
			if (Power <= 0f)
			{
				_carStarted = false;
				Knob.SetActive(status: false);
				Knob._rectTransform.localScale = Vector3.one * 0.9f;
				ReloadingPrompt.SetActive(value: true);
			}
		}
		else
		{
			_carStarted = false;
			_lastStartClickAt = Time.time;
			MMVibrationManager.StopContinuousHaptic(alsoRumble: true);
		}
	}

	protected virtual void UpdateCar()
	{
		float b = (_carStarted ? NiceVibrationsDemoHelpers.Remap(Knob.Value, MinimumKnobValue, 1f, 0f, 1f) : 0f);
		CarSpeed = Mathf.Lerp(CarSpeed, b, Time.deltaTime * 1f);
		CarEngineAudioSource.volume = CarSpeed;
		CarEngineAudioSource.pitch = NiceVibrationsDemoHelpers.Remap(CarSpeed, 0f, 1f, 0.5f, 1.25f);
		LeftWheel.Rotate(CarSpeed * Time.deltaTime * WheelRotationSpeed, Space.Self);
		RightWheel.Rotate(CarSpeed * Time.deltaTime * WheelRotationSpeed, Space.Self);
		_carPosition.x = _initialCarPosition.x + 0f;
		_carPosition.y = _initialCarPosition.y + 10f * CarSpeed * Mathf.PerlinNoise(Time.time * 10f, CarSpeed * 10f);
		_carPosition.z = 0f;
		CarBody.localPosition = _carPosition;
	}

	protected virtual void UpdateUI()
	{
		if (Knob.Active)
		{
			if (Time.time - _lastStartClickAt < StartClickDuration)
			{
				float num = StartClickCurve.Evaluate((Time.time - _lastStartClickAt) * (1f / StartClickDuration));
				Knob._rectTransform.localScale = Vector3.one + Vector3.one * num * 0.05f;
				Knob._image.color = Color.Lerp(ActiveColor, Color.white, num);
			}
			foreach (float dent in Dents)
			{
				if ((_knobValue >= dent && _knobValueLastFrame < dent) || (_knobValue <= dent && _knobValueLastFrame > dent))
				{
					_lastDentAt = Time.time;
					break;
				}
			}
			if (Time.time - _lastDentAt < DentDuration)
			{
				float num2 = StartClickCurve.Evaluate((Time.time - _lastDentAt) * (1f / DentDuration));
				Knob._rectTransform.localScale = Vector3.one + Vector3.one * num2 * 0.02f;
				Knob._image.color = Color.Lerp(ActiveColor, Color.white, num2 * 0.05f);
				if (MMVibrationManager.iOS())
				{
					MMVibrationManager.TransientHaptic(0.4f, 1f);
				}
			}
		}
		PowerBar.UpdateBar(Power, 0f, MaximumPowerDuration);
		if (CarSpeed <= 0.1f)
		{
			for (int i = 0; i < SpeedBars.Count; i++)
			{
				SpeedBars[i].SetActive(status: false);
			}
			return;
		}
		int num3 = (int)(CarSpeed * 5f);
		for (int j = 0; j < SpeedBars.Count; j++)
		{
			if (j <= num3)
			{
				SpeedBars[j].SetActive(status: true);
			}
			else
			{
				SpeedBars[j].SetActive(status: false);
			}
		}
	}
}
