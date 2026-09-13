namespace System.Security.AccessControl;

public sealed class CustomAce : GenericAce
{
	private byte[] opaque;

	[MonoTODO]
	public static readonly int MaxOpaqueLength;

	[MonoTODO]
	public override int BinaryLength
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public int OpaqueLength => opaque.Length;

	public CustomAce(AceType type, AceFlags flags, byte[] opaque)
		: base(type, flags)
	{
		SetOpaque(opaque);
	}

	[MonoTODO]
	public override void GetBinaryForm(byte[] binaryForm, int offset)
	{
		throw new NotImplementedException();
	}

	public byte[] GetOpaque()
	{
		return (byte[])opaque.Clone();
	}

	public void SetOpaque(byte[] opaque)
	{
		if (opaque == null)
		{
			this.opaque = null;
		}
		else
		{
			this.opaque = (byte[])opaque.Clone();
		}
	}

	internal override string GetSddlForm()
	{
		throw new NotSupportedException();
	}
}
