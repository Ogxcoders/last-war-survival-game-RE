public sealed class ReferencePoolInfo
{
	private readonly string m_TypeName;

	private readonly int m_UnusedReferenceCount;

	private readonly int m_UsingReferenceCount;

	private readonly int m_AcquireReferenceCount;

	private readonly int m_ReleaseReferenceCount;

	private readonly int m_AddReferenceCount;

	private readonly int m_RemoveReferenceCount;

	public string TypeName => m_TypeName;

	public int UnusedReferenceCount => m_UnusedReferenceCount;

	public int UsingReferenceCount => m_UsingReferenceCount;

	public int AcquireReferenceCount => m_AcquireReferenceCount;

	public int ReleaseReferenceCount => m_ReleaseReferenceCount;

	public int AddReferenceCount => m_AddReferenceCount;

	public int RemoveReferenceCount => m_RemoveReferenceCount;

	public ReferencePoolInfo(string typeName, int unusedReferenceCount, int usingReferenceCount, int acquireReferenceCount, int releaseReferenceCount, int addReferenceCount, int removeReferenceCount)
	{
		m_TypeName = typeName;
		m_UnusedReferenceCount = unusedReferenceCount;
		m_UsingReferenceCount = usingReferenceCount;
		m_AcquireReferenceCount = acquireReferenceCount;
		m_ReleaseReferenceCount = releaseReferenceCount;
		m_AddReferenceCount = addReferenceCount;
		m_RemoveReferenceCount = removeReferenceCount;
	}
}
