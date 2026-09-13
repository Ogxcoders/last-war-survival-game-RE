using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine;

[Serializable]
[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
public struct AxisState
{
	[Serializable]
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	public struct Recentering
	{
		[Tooltip("If checked, will enable automatic recentering of the axis. If unchecked, recenting is disabled.")]
		public bool m_enabled;

		[Tooltip("If no user input has been detected on the axis, the axis will wait this long in seconds before recentering.")]
		public float m_WaitTime;

		[Tooltip("How long it takes to reach destination once recentering has started.")]
		public float m_RecenteringTime;

		private float mLastAxisInputTime;

		private float mRecenteringVelocity;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("m_HeadingDefinition")]
		private int m_LegacyHeadingDefinition;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("m_VelocityFilterStrength")]
		private int m_LegacyVelocityFilterStrength;

		public Recentering(bool enabled, float waitTime, float recenteringTime)
		{
			m_enabled = enabled;
			m_WaitTime = waitTime;
			m_RecenteringTime = recenteringTime;
			mLastAxisInputTime = 0f;
			mRecenteringVelocity = 0f;
			m_LegacyHeadingDefinition = (m_LegacyVelocityFilterStrength = -1);
		}

		public void Validate()
		{
			m_WaitTime = Mathf.Max(0f, m_WaitTime);
			m_RecenteringTime = Mathf.Max(0f, m_RecenteringTime);
		}

		public void CancelRecentering()
		{
			mLastAxisInputTime = Time.time;
			mRecenteringVelocity = 0f;
		}

		public void RecenterNow()
		{
			mLastAxisInputTime = 0f;
		}

		public void DoRecentering(ref AxisState axis, float deltaTime, float recenterTarget)
		{
			if (!m_enabled)
			{
				return;
			}
			if (deltaTime < 0f)
			{
				CancelRecentering();
				axis.Value = recenterTarget;
			}
			else
			{
				if (!(Time.time > mLastAxisInputTime + m_WaitTime))
				{
					return;
				}
				float num = m_RecenteringTime / 3f;
				if (num <= deltaTime)
				{
					axis.Value = recenterTarget;
					return;
				}
				float f = Mathf.DeltaAngle(axis.Value, recenterTarget);
				float num2 = Mathf.Abs(f);
				if (num2 < 0.0001f)
				{
					axis.Value = recenterTarget;
					mRecenteringVelocity = 0f;
					return;
				}
				float num3 = deltaTime / num;
				float num4 = Mathf.Sign(f) * Mathf.Min(num2, num2 * num3);
				float num5 = num4 - mRecenteringVelocity;
				if ((num4 < 0f && num5 < 0f) || (num4 > 0f && num5 > 0f))
				{
					num4 = mRecenteringVelocity + num4 * num3;
				}
				axis.Value += num4;
				mRecenteringVelocity = num4;
			}
		}

		internal bool LegacyUpgrade(ref int heading, ref int velocityFilter)
		{
			if (m_LegacyHeadingDefinition != -1 && m_LegacyVelocityFilterStrength != -1)
			{
				heading = m_LegacyHeadingDefinition;
				velocityFilter = m_LegacyVelocityFilterStrength;
				m_LegacyHeadingDefinition = (m_LegacyVelocityFilterStrength = -1);
				return true;
			}
			return false;
		}
	}

	[NoSaveDuringPlay]
	[Tooltip("The current value of the axis.")]
	public float Value;

	[Tooltip("The maximum speed of this axis in units/second")]
	public float m_MaxSpeed;

	[Tooltip("The amount of time in seconds it takes to accelerate to MaxSpeed with the supplied Axis at its maximum value")]
	public float m_AccelTime;

	[Tooltip("The amount of time in seconds it takes to decelerate the axis to zero if the supplied axis is in a neutral position")]
	public float m_DecelTime;

	[FormerlySerializedAs("m_AxisName")]
	[Tooltip("The name of this axis as specified in Unity Input manager. Setting to an empty string will disable the automatic updating of this axis")]
	public string m_InputAxisName;

	[NoSaveDuringPlay]
	[Tooltip("The value of the input axis.  A value of 0 means no input.  You can drive this directly from a custom input system, or you can set the Axis Name and have the value driven by the internal Input Manager")]
	public float m_InputAxisValue;

	[FormerlySerializedAs("m_InvertAxis")]
	[Tooltip("If checked, then the raw value of the input axis will be inverted before it is used")]
	public bool m_InvertInput;

	[Tooltip("The minimum value for the axis")]
	public float m_MinValue;

	[Tooltip("The maximum value for the axis")]
	public float m_MaxValue;

	[Tooltip("If checked, then the axis will wrap around at the min/max values, forming a loop")]
	public bool m_Wrap;

	[Tooltip("Automatic recentering to at-rest position")]
	public Recentering m_Recentering;

	private float mCurrentSpeed;

	private const float Epsilon = 0.0001f;

	public bool ValueRangeLocked { get; set; }

	public bool HasRecentering { get; set; }

	public AxisState(float minValue, float maxValue, bool wrap, bool rangeLocked, float maxSpeed, float accelTime, float decelTime, string name, bool invert)
	{
		m_MinValue = minValue;
		m_MaxValue = maxValue;
		m_Wrap = wrap;
		ValueRangeLocked = rangeLocked;
		HasRecentering = false;
		m_Recentering = new Recentering(enabled: false, 1f, 2f);
		m_MaxSpeed = maxSpeed;
		m_AccelTime = accelTime;
		m_DecelTime = decelTime;
		Value = (minValue + maxValue) / 2f;
		m_InputAxisName = name;
		m_InputAxisValue = 0f;
		m_InvertInput = invert;
		mCurrentSpeed = 0f;
	}

	public void Validate()
	{
		m_MaxSpeed = Mathf.Max(0f, m_MaxSpeed);
		m_AccelTime = Mathf.Max(0f, m_AccelTime);
		m_DecelTime = Mathf.Max(0f, m_DecelTime);
		m_MaxValue = Mathf.Clamp(m_MaxValue, m_MinValue, m_MaxValue);
	}

	public void Reset()
	{
		m_InputAxisValue = 0f;
		mCurrentSpeed = 0f;
	}

	public bool Update(float deltaTime)
	{
		if (!string.IsNullOrEmpty(m_InputAxisName))
		{
			try
			{
				m_InputAxisValue = CinemachineCore.GetInputAxis(m_InputAxisName);
			}
			catch (ArgumentException ex)
			{
				Debug.LogError(ex.ToString());
			}
		}
		float num = m_InputAxisValue;
		if (m_InvertInput)
		{
			num *= -1f;
		}
		if (m_MaxSpeed > 0.0001f)
		{
			float num2 = num * m_MaxSpeed;
			if (Mathf.Abs(num2) < 0.0001f || (Mathf.Sign(mCurrentSpeed) == Mathf.Sign(num2) && Mathf.Abs(num2) < Mathf.Abs(mCurrentSpeed)))
			{
				float num3 = Mathf.Min(Mathf.Abs(num2 - mCurrentSpeed) / Mathf.Max(0.0001f, m_DecelTime) * deltaTime, Mathf.Abs(mCurrentSpeed));
				mCurrentSpeed -= Mathf.Sign(mCurrentSpeed) * num3;
			}
			else
			{
				float num4 = Mathf.Abs(num2 - mCurrentSpeed) / Mathf.Max(0.0001f, m_AccelTime);
				mCurrentSpeed += Mathf.Sign(num2) * num4 * deltaTime;
				if (Mathf.Sign(mCurrentSpeed) == Mathf.Sign(num2) && Mathf.Abs(mCurrentSpeed) > Mathf.Abs(num2))
				{
					mCurrentSpeed = num2;
				}
			}
		}
		float maxSpeed = GetMaxSpeed();
		mCurrentSpeed = Mathf.Clamp(mCurrentSpeed, 0f - maxSpeed, maxSpeed);
		Value += mCurrentSpeed * deltaTime;
		if (Value > m_MaxValue || Value < m_MinValue)
		{
			if (m_Wrap)
			{
				if (Value > m_MaxValue)
				{
					Value = m_MinValue + (Value - m_MaxValue);
				}
				else
				{
					Value = m_MaxValue + (Value - m_MinValue);
				}
			}
			else
			{
				Value = Mathf.Clamp(Value, m_MinValue, m_MaxValue);
				mCurrentSpeed = 0f;
			}
		}
		return Mathf.Abs(num) > 0.0001f;
	}

	private float GetMaxSpeed()
	{
		float num = m_MaxValue - m_MinValue;
		if (!m_Wrap && num > 0f)
		{
			float num2 = num / 10f;
			if (mCurrentSpeed > 0f && m_MaxValue - Value < num2)
			{
				float t = (m_MaxValue - Value) / num2;
				return Mathf.Lerp(0f, m_MaxSpeed, t);
			}
			if (mCurrentSpeed < 0f && Value - m_MinValue < num2)
			{
				float t2 = (Value - m_MinValue) / num2;
				return Mathf.Lerp(0f, m_MaxSpeed, t2);
			}
		}
		return m_MaxSpeed;
	}
}
