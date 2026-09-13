using System;

namespace UnityEngine.Lumin;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class UsesLuminPlatformLevelAttribute : Attribute
{
	private readonly uint m_PlatformLevel;

	public uint platformLevel => m_PlatformLevel;

	public UsesLuminPlatformLevelAttribute(uint platformLevel)
	{
		m_PlatformLevel = platformLevel;
	}
}
