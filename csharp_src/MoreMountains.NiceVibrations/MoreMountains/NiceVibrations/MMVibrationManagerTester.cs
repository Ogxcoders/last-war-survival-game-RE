using System.Collections;
using UnityEngine;

namespace MoreMountains.NiceVibrations;

public class MMVibrationManagerTester : MonoBehaviour
{
	public enum HapticMethods
	{
		NativePreset,
		Transient,
		Continuous,
		AdvancedPattern,
		Stop
	}

	public enum Timescales
	{
		ScaledTime,
		UnscaledTime
	}

	[Header("Haptics")]
	public HapticMethods HapticMethod;

	[MMNVEnumCondition("HapticMethod", new int[] { 0 })]
	public HapticTypes HapticType = HapticTypes.None;

	[MMNVEnumCondition("HapticMethod", new int[] { 1 })]
	public float TransientIntensity = 1f;

	[MMNVEnumCondition("HapticMethod", new int[] { 1 })]
	public float TransientSharpness = 1f;

	[MMNVEnumCondition("HapticMethod", new int[] { 2 })]
	public float InitialContinuousIntensity = 1f;

	[MMNVEnumCondition("HapticMethod", new int[] { 2 })]
	public AnimationCurve ContinuousIntensityCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));

	[MMNVEnumCondition("HapticMethod", new int[] { 2 })]
	public float InitialContinuousSharpness = 1f;

	[MMNVEnumCondition("HapticMethod", new int[] { 2 })]
	public AnimationCurve ContinuousSharpnessCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));

	[MMNVEnumCondition("HapticMethod", new int[] { 2 })]
	public float ContinuousDuration = 1f;

	[MMNVEnumCondition("HapticMethod", new int[] { 3 })]
	public TextAsset AHAPFileForIOS;

	[MMNVEnumCondition("HapticMethod", new int[] { 3 })]
	public MMNVAndroidWaveFormAsset AndroidWaveFormFile;

	[MMNVEnumCondition("HapticMethod", new int[] { 3 })]
	public MMNVRumbleWaveFormAsset RumbleWaveFormFile;

	[MMNVEnumCondition("HapticMethod", new int[] { 3 })]
	public int AndroidRepeat = -1;

	[MMNVEnumCondition("HapticMethod", new int[] { 3 })]
	public int RumbleRepeat = -1;

	[MMNVEnumCondition("HapticMethod", new int[] { 3 })]
	public HapticTypes OldIOSFallback;

	[MMNVEnumCondition("HapticMethod", new int[] { 3 })]
	public Timescales Timescale = Timescales.UnscaledTime;

	[Header("Rumble")]
	public bool AllowRumble = true;

	[MMNVInspectorButton("TestVibration")]
	public bool TestVibrationButton;

	protected static bool _continuousPlaying;

	protected static float _continuousStartedAt;

	protected virtual void TestVibration()
	{
		_ = base.transform.position;
		switch (HapticMethod)
		{
		case HapticMethods.AdvancedPattern:
			MMVibrationManager.AdvancedHapticPattern((AHAPFileForIOS == null) ? "" : AHAPFileForIOS.text, (AndroidWaveFormFile == null) ? null : AndroidWaveFormFile.WaveForm.Pattern, (AndroidWaveFormFile == null) ? null : AndroidWaveFormFile.WaveForm.Amplitudes, rumblePattern: (RumbleWaveFormFile == null) ? null : RumbleWaveFormFile.WaveForm.Pattern, rumbleLowFreqAmplitudes: (RumbleWaveFormFile == null) ? null : RumbleWaveFormFile.WaveForm.LowFrequencyAmplitudes, rumbleHighFreqAmplitudes: (RumbleWaveFormFile == null) ? null : RumbleWaveFormFile.WaveForm.HighFrequencyAmplitudes, androidRepeat: AndroidRepeat, rumbleRepeat: RumbleRepeat, fallbackOldiOS: OldIOSFallback, coroutineSupport: this);
			break;
		case HapticMethods.Continuous:
			StartCoroutine(ContinuousHapticsCoroutine());
			break;
		case HapticMethods.NativePreset:
			MMVibrationManager.Haptic(HapticType, defaultToRegularVibrate: false, AllowRumble, this);
			break;
		case HapticMethods.Transient:
			MMVibrationManager.TransientHaptic(TransientIntensity, TransientSharpness, AllowRumble, this);
			break;
		case HapticMethods.Stop:
			if (_continuousPlaying)
			{
				MMVibrationManager.StopContinuousHaptic(AllowRumble);
				_continuousPlaying = false;
			}
			break;
		}
	}

	protected virtual IEnumerator ContinuousHapticsCoroutine()
	{
		_continuousStartedAt = ((Timescale == Timescales.ScaledTime) ? Time.time : Time.unscaledTime);
		_continuousPlaying = true;
		float elapsedTime = ComputeElapsedTime();
		MMVibrationManager.ContinuousHaptic(InitialContinuousIntensity, InitialContinuousSharpness, ContinuousDuration, HapticTypes.Success, this);
		while (_continuousPlaying && elapsedTime < ContinuousDuration)
		{
			elapsedTime = ComputeElapsedTime();
			float time = Remap(elapsedTime, 0f, ContinuousDuration, 0f, 1f);
			float intensity = ContinuousIntensityCurve.Evaluate(time);
			float sharpness = ContinuousSharpnessCurve.Evaluate(time);
			MMVibrationManager.UpdateContinuousHaptic(intensity, sharpness, alsoRumble: true);
			_ = AllowRumble;
			yield return null;
		}
		if (_continuousPlaying)
		{
			_continuousPlaying = false;
			MMVibrationManager.StopContinuousHaptic(AllowRumble);
		}
	}

	protected virtual float ComputeElapsedTime()
	{
		if (Timescale != Timescales.ScaledTime)
		{
			return Time.unscaledTime - _continuousStartedAt;
		}
		return Time.time - _continuousStartedAt;
	}

	public static float Remap(float x, float A, float B, float C, float D)
	{
		return C + (x - A) / (B - A) * (D - C);
	}
}
