using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.NiceVibrations;

public class PresetDemoManager : DemoManager
{
	[Header("Image")]
	public Image IconImage;

	public Animator IconImageAnimator;

	public List<PresetDemoItem> DemoItems;

	protected WaitForSeconds _ahapTurnDelay;

	protected int _idleAnimationParameter;

	protected virtual void Awake()
	{
		_ahapTurnDelay = new WaitForSeconds(0.02f);
		_idleAnimationParameter = Animator.StringToHash("Idle");
		IconImageAnimator.SetBool(_idleAnimationParameter, value: true);
	}

	public virtual void PlayAHAP(int index)
	{
		Logo.Shaking = true;
		if (index < 5)
		{
			MMVibrationManager.AdvancedHapticPattern(DemoItems[index].AHAPFile.text, DemoItems[index].WaveFormAsset.WaveForm.Pattern, DemoItems[index].WaveFormAsset.WaveForm.Amplitudes, -1, DemoItems[index].RumbleWaveFormAsset.WaveForm.Pattern, DemoItems[index].RumbleWaveFormAsset.WaveForm.LowFrequencyAmplitudes, DemoItems[index].RumbleWaveFormAsset.WaveForm.HighFrequencyAmplitudes, -1, HapticTypes.LightImpact, this);
			DemoItems[index].AssociatedSound.Play();
			StartCoroutine(ChangeIcon(DemoItems[index].AssociatedSprite));
		}
		else
		{
			MMVibrationManager.AdvancedHapticPattern(DemoItems[index].AHAPFile.text, DemoItems[index].WaveFormAsset.WaveForm.Pattern, DemoItems[index].WaveFormAsset.WaveForm.Amplitudes, -1, DemoItems[index].RumbleWaveFormAsset.WaveForm.Pattern, DemoItems[index].RumbleWaveFormAsset.WaveForm.LowFrequencyAmplitudes, DemoItems[index].RumbleWaveFormAsset.WaveForm.HighFrequencyAmplitudes, -1, HapticTypes.LightImpact, this, -1, threaded: true);
			DemoItems[index].AssociatedSound.Play();
			StartCoroutine(ChangeIcon(DemoItems[index].AssociatedSprite));
		}
	}

	protected virtual IEnumerator ChangeIcon(Sprite newSprite)
	{
		IconImageAnimator.SetBool(_idleAnimationParameter, value: false);
		yield return _ahapTurnDelay;
		IconImage.sprite = newSprite;
	}

	public virtual void Test()
	{
		StartCoroutine(BackToIdle());
	}

	protected virtual void OnHapticsStopped()
	{
		StartCoroutine(BackToIdle());
	}

	protected virtual IEnumerator BackToIdle()
	{
		Logo.Shaking = false;
		IconImageAnimator.SetBool(_idleAnimationParameter, value: true);
		yield return _ahapTurnDelay;
		IconImage.sprite = DemoItems[0].AssociatedSprite;
	}

	protected virtual void OnHapticsError()
	{
	}

	protected virtual void OnHapticsReset()
	{
	}

	protected virtual void OnEnable()
	{
		MMNViOSCoreHaptics.OnHapticPatternStopped += OnHapticsStopped;
		MMNViOSCoreHaptics.OnHapticPatternError += OnHapticsError;
		MMNViOSCoreHaptics.OnHapticPatternReset += OnHapticsReset;
	}

	protected virtual void OnDisable()
	{
		MMNViOSCoreHaptics.OnHapticPatternStopped -= OnHapticsStopped;
		MMNViOSCoreHaptics.OnHapticPatternError -= OnHapticsError;
		MMNViOSCoreHaptics.OnHapticPatternReset -= OnHapticsReset;
		MMNVAndroid.ClearThreads();
	}
}
