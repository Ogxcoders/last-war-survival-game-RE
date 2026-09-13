namespace System.Security.Authentication.ExtendedProtection;

public class TokenBinding
{
	private byte[] _rawTokenBindingId;

	public TokenBindingType BindingType { get; private set; }

	internal TokenBinding(TokenBindingType bindingType, byte[] rawData)
	{
		BindingType = bindingType;
		_rawTokenBindingId = rawData;
	}

	public byte[] GetRawTokenBindingId()
	{
		if (_rawTokenBindingId == null)
		{
			return null;
		}
		return (byte[])_rawTokenBindingId.Clone();
	}
}
