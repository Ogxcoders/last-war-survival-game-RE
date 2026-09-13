using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[NativeHeader("Modules/Animation/AnimatorInfo.h")]
[RequiredByNativeCode]
public struct AnimatorTransitionInfo
{
	[NativeName("fullPathHash")]
	private int m_FullPath;

	[NativeName("userNameHash")]
	private int m_UserName;

	[NativeName("nameHash")]
	private int m_Name;

	[NativeName("hasFixedDuration")]
	private bool m_HasFixedDuration;

	[NativeName("duration")]
	private float m_Duration;

	[NativeName("normalizedTime")]
	private float m_NormalizedTime;

	[NativeName("anyState")]
	private bool m_AnyState;

	[NativeName("transitionType")]
	private int m_TransitionType;

	public int fullPathHash => m_FullPath;

	public int nameHash => m_Name;

	public int userNameHash => m_UserName;

	public DurationUnit durationUnit => (!m_HasFixedDuration) ? DurationUnit.Normalized : DurationUnit.Fixed;

	public float duration => m_Duration;

	public float normalizedTime => m_NormalizedTime;

	public bool anyState => m_AnyState;

	internal bool entry => (m_TransitionType & 2) != 0;

	internal bool exit => (m_TransitionType & 4) != 0;

	public bool IsName(string name)
	{
		return Animator.StringToHash(name) == m_Name || Animator.StringToHash(name) == m_FullPath;
	}

	public bool IsUserName(string name)
	{
		return Animator.StringToHash(name) == m_UserName;
	}
}
