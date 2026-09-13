namespace System.Xml.Schema;

internal class UpaException : Exception
{
	private object particle1;

	private object particle2;

	public object Particle1 => particle1;

	public object Particle2 => particle2;

	public UpaException(object particle1, object particle2)
	{
		this.particle1 = particle1;
		this.particle2 = particle2;
	}
}
