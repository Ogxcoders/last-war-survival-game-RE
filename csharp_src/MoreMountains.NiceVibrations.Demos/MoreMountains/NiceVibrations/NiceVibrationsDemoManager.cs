using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.NiceVibrations;

public class NiceVibrationsDemoManager : MonoBehaviour
{
	public Text DebugTextBox;

	protected string _debugString;

	protected string _platformString;

	protected const string _CURRENTVERSION = "1.7";

	protected virtual void Awake()
	{
		MMNViOS.iOSInitializeHaptics();
	}

	protected virtual void Start()
	{
		DisplayInformation();
	}

	protected virtual void DisplayInformation()
	{
		if (MMVibrationManager.Android())
		{
			_platformString = "API version " + MMNVAndroid.AndroidSDKVersion();
		}
		else if (MMVibrationManager.iOS())
		{
			_platformString = "iOS " + MMNViOS.iOSSDKVersion();
		}
		else
		{
			_platformString = string.Concat(Application.platform, ", not supported by Nice Vibrations for now.");
		}
		DebugTextBox.text = "Platform : " + _platformString + "\n Nice Vibrations v1.7";
	}

	protected virtual void OnDisable()
	{
		MMNViOS.iOSReleaseHaptics();
	}

	public virtual void TriggerDefault()
	{
		Handheld.Vibrate();
	}

	public virtual void TriggerVibrate()
	{
		MMVibrationManager.Vibrate();
	}

	public virtual void TriggerSelection()
	{
		MMVibrationManager.Haptic(HapticTypes.Selection);
	}

	public virtual void TriggerSuccess()
	{
		MMVibrationManager.Haptic(HapticTypes.Success);
	}

	public virtual void TriggerWarning()
	{
		MMVibrationManager.Haptic(HapticTypes.Warning);
	}

	public virtual void TriggerFailure()
	{
		MMVibrationManager.Haptic(HapticTypes.Failure);
	}

	public virtual void TriggerLightImpact()
	{
		MMVibrationManager.Haptic(HapticTypes.LightImpact);
	}

	public virtual void TriggerMediumImpact()
	{
		MMVibrationManager.Haptic(HapticTypes.MediumImpact);
	}

	public virtual void TriggerHeavyImpact()
	{
		MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
	}
}
