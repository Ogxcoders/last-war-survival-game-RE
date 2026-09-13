namespace System.Runtime.Serialization;

[Serializable]
internal class FixupHolder
{
	internal const int ArrayFixup = 1;

	internal const int MemberFixup = 2;

	internal const int DelayedFixup = 4;

	internal long m_id;

	internal object m_fixupInfo;

	internal int m_fixupType;

	internal FixupHolder(long id, object fixupInfo, int fixupType)
	{
		m_id = id;
		m_fixupInfo = fixupInfo;
		m_fixupType = fixupType;
	}
}
