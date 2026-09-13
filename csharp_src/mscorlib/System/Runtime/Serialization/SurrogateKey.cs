namespace System.Runtime.Serialization;

[Serializable]
internal class SurrogateKey
{
	internal Type m_type;

	internal StreamingContext m_context;

	internal SurrogateKey(Type type, StreamingContext context)
	{
		m_type = type;
		m_context = context;
	}

	public override int GetHashCode()
	{
		return m_type.GetHashCode();
	}
}
