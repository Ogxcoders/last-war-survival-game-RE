using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine;

[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
[AddComponentMenu("")]
[SaveDuringPlay]
public class CinemachineBasicMultiChannelPerlin : CinemachineComponentBase
{
	[Tooltip("The asset containing the Noise Profile.  Define the frequencies and amplitudes there to make a characteristic noise profile.  Make your own or just use one of the many presets.")]
	[FormerlySerializedAs("m_Definition")]
	[NoiseSettingsProperty]
	public NoiseSettings m_NoiseProfile;

	[Tooltip("Gain to apply to the amplitudes defined in the NoiseSettings asset.  1 is normal.  Setting this to 0 completely mutes the noise.")]
	public float m_AmplitudeGain = 1f;

	[Tooltip("Scale factor to apply to the frequencies defined in the NoiseSettings asset.  1 is normal.  Larger magnitudes will make the noise shake more rapidly.")]
	public float m_FrequencyGain = 1f;

	private bool mInitialized;

	private float mNoiseTime;

	[SerializeField]
	[HideInInspector]
	private Vector3 mNoiseOffsets = Vector3.zero;

	public override bool IsValid
	{
		get
		{
			if (base.enabled)
			{
				return m_NoiseProfile != null;
			}
			return false;
		}
	}

	public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Noise;

	public override void MutateCameraState(ref CameraState curState, float deltaTime)
	{
		if (IsValid && !(deltaTime < 0f))
		{
			if (!mInitialized)
			{
				Initialize();
			}
			mNoiseTime += deltaTime * m_FrequencyGain;
			curState.PositionCorrection += curState.CorrectedOrientation * NoiseSettings.GetCombinedFilterResults(m_NoiseProfile.PositionNoise, mNoiseTime, mNoiseOffsets) * m_AmplitudeGain;
			Quaternion quaternion = Quaternion.Euler(NoiseSettings.GetCombinedFilterResults(m_NoiseProfile.OrientationNoise, mNoiseTime, mNoiseOffsets) * m_AmplitudeGain);
			curState.OrientationCorrection *= quaternion;
		}
	}

	public void ReSeed()
	{
		mNoiseOffsets = new Vector3(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f));
	}

	private void Initialize()
	{
		mInitialized = true;
		mNoiseTime = 0f;
		if (mNoiseOffsets == Vector3.zero)
		{
			ReSeed();
		}
	}
}
