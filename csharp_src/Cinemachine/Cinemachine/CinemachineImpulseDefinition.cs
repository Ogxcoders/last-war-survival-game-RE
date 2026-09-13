using System;
using UnityEngine;

namespace Cinemachine;

[Serializable]
[DocumentationSorting(DocumentationSortingAttribute.Level.API)]
public class CinemachineImpulseDefinition
{
	public enum RepeatMode
	{
		Stretch,
		Loop
	}

	private class SignalSource : ISignalSource6D
	{
		private CinemachineImpulseDefinition m_Def;

		private Vector3 m_Velocity;

		private float m_StartTimeOffset;

		public float SignalDuration => m_Def.m_RawSignal.SignalDuration;

		public SignalSource(CinemachineImpulseDefinition def, Vector3 velocity)
		{
			m_Def = def;
			m_Velocity = velocity;
			if (m_Def.m_Randomize && m_Def.m_RawSignal.SignalDuration <= 0f)
			{
				m_StartTimeOffset = UnityEngine.Random.Range(-1000f, 1000f);
			}
		}

		public void GetSignal(float timeSinceSignalStart, out Vector3 pos, out Quaternion rot)
		{
			float num = m_StartTimeOffset + timeSinceSignalStart * m_Def.m_FrequencyGain;
			float signalDuration = SignalDuration;
			if (signalDuration > 0f)
			{
				if (m_Def.m_RepeatMode == RepeatMode.Loop)
				{
					num %= signalDuration;
				}
				else if (m_Def.m_TimeEnvelope.Duration > 0.0001f)
				{
					num *= m_Def.m_TimeEnvelope.Duration / signalDuration;
				}
			}
			m_Def.m_RawSignal.GetSignal(num, out pos, out rot);
			float magnitude = m_Velocity.magnitude;
			_ = m_Velocity.normalized;
			magnitude *= m_Def.m_AmplitudeGain;
			pos *= magnitude;
			pos = Quaternion.FromToRotation(Vector3.down, m_Velocity) * pos;
			rot = Quaternion.SlerpUnclamped(Quaternion.identity, rot, magnitude);
		}
	}

	[CinemachineImpulseChannelProperty]
	[Tooltip("Impulse events generated here will appear on the channels included in the mask.")]
	public int m_ImpulseChannel = 1;

	[Header("Signal Shape")]
	[Tooltip("Defines the signal that will be generated.")]
	[CinemachineEmbeddedAssetProperty(true)]
	public SignalSourceAsset m_RawSignal;

	[Tooltip("Gain to apply to the amplitudes defined in the signal source.  1 is normal.  Setting this to 0 completely mutes the signal.")]
	public float m_AmplitudeGain = 1f;

	[Tooltip("Scale factor to apply to the time axis.  1 is normal.  Larger magnitudes will make the signal progress more rapidly.")]
	public float m_FrequencyGain = 1f;

	[Tooltip("How to fit the signal into the envelope time")]
	public RepeatMode m_RepeatMode;

	[Tooltip("Randomize the signal start time")]
	public bool m_Randomize = true;

	[Tooltip("This defines the time-envelope of the signal.  The raw signal will be time-scaled to fit in the envelope.")]
	[CinemachineImpulseEnvelopeProperty]
	public CinemachineImpulseManager.EnvelopeDefinition m_TimeEnvelope = CinemachineImpulseManager.EnvelopeDefinition.Default();

	[Header("Spatial Range")]
	[Tooltip("The signal will have full amplitude in this radius surrounding the impact point.  Beyond that it will dissipate with distance.")]
	public float m_ImpactRadius = 100f;

	[Tooltip("How the signal direction behaves as the listener moves away from the origin.")]
	public CinemachineImpulseManager.ImpulseEvent.DirectionMode m_DirectionMode;

	[Tooltip("This defines how the signal will dissipate with distance beyond the impact radius.")]
	public CinemachineImpulseManager.ImpulseEvent.DissipationMode m_DissipationMode = CinemachineImpulseManager.ImpulseEvent.DissipationMode.ExponentialDecay;

	[Tooltip("At this distance beyond the impact radius, the signal will have dissipated to zero.")]
	public float m_DissipationDistance = 1000f;

	public void OnValidate()
	{
		m_ImpactRadius = Mathf.Max(0f, m_ImpactRadius);
		m_DissipationDistance = Mathf.Max(0f, m_DissipationDistance);
		m_TimeEnvelope.Validate();
	}

	public void CreateEvent(Vector3 position, Vector3 velocity)
	{
		if (!(m_RawSignal == null) && !(Mathf.Abs(m_TimeEnvelope.Duration) < 0.0001f))
		{
			CinemachineImpulseManager.ImpulseEvent impulseEvent = CinemachineImpulseManager.Instance.NewImpulseEvent();
			impulseEvent.m_Envelope = m_TimeEnvelope;
			impulseEvent.m_Envelope = m_TimeEnvelope;
			if (m_TimeEnvelope.m_ScaleWithImpact)
			{
				impulseEvent.m_Envelope.m_DecayTime *= Mathf.Sqrt(velocity.magnitude);
			}
			impulseEvent.m_SignalSource = new SignalSource(this, velocity);
			impulseEvent.m_Position = position;
			impulseEvent.m_Radius = m_ImpactRadius;
			impulseEvent.m_Channel = m_ImpulseChannel;
			impulseEvent.m_DirectionMode = m_DirectionMode;
			impulseEvent.m_DissipationMode = m_DissipationMode;
			impulseEvent.m_DissipationDistance = m_DissipationDistance;
			CinemachineImpulseManager.Instance.AddImpulseEvent(impulseEvent);
		}
	}
}
