using System;
using System.Text;

namespace Mono.Security.Protocol.Ntlm;

public class Type3Message : MessageBase
{
	private NtlmAuthLevel _level;

	private byte[] _challenge;

	private string _host;

	private string _domain;

	private string _username;

	private string _password;

	private Type2Message _type2;

	private byte[] _lm;

	private byte[] _nt;

	internal const string LegacyAPIWarning = "Use of this API is highly discouraged, it selects legacy-mode LM/NTLM authentication, which sends your password in very weak encryption over the wire even if the server supports the more secure NTLMv2 / NTLMv2 Session. You need to use the new `Type3Message (Type2Message)' constructor to use the more secure NTLMv2 / NTLMv2 Session authentication modes. These require the Type 2 message from the server to compute the response.";

	[Obsolete("Use NtlmSettings.DefaultAuthLevel")]
	public static NtlmAuthLevel DefaultAuthLevel
	{
		get
		{
			return NtlmSettings.DefaultAuthLevel;
		}
		set
		{
			NtlmSettings.DefaultAuthLevel = value;
		}
	}

	public NtlmAuthLevel Level
	{
		get
		{
			return _level;
		}
		set
		{
			_level = value;
		}
	}

	[Obsolete("Use of this API is highly discouraged, it selects legacy-mode LM/NTLM authentication, which sends your password in very weak encryption over the wire even if the server supports the more secure NTLMv2 / NTLMv2 Session. You need to use the new `Type3Message (Type2Message)' constructor to use the more secure NTLMv2 / NTLMv2 Session authentication modes. These require the Type 2 message from the server to compute the response.")]
	public byte[] Challenge
	{
		get
		{
			if (_challenge == null)
			{
				return null;
			}
			return (byte[])_challenge.Clone();
		}
		set
		{
			if (_type2 != null || _level != NtlmAuthLevel.LM_and_NTLM)
			{
				throw new InvalidOperationException("Refusing to use legacy-mode LM/NTLM authentication unless explicitly enabled using DefaultAuthLevel.");
			}
			if (value == null)
			{
				throw new ArgumentNullException("Challenge");
			}
			if (value.Length != 8)
			{
				throw new ArgumentException(global::Locale.GetText("Invalid Challenge Length (should be 8 bytes)."), "Challenge");
			}
			_challenge = (byte[])value.Clone();
		}
	}

	public string Domain
	{
		get
		{
			return _domain;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (value == "")
			{
				base.Flags &= ~NtlmFlags.NegotiateDomainSupplied;
			}
			else
			{
				base.Flags |= NtlmFlags.NegotiateDomainSupplied;
			}
			_domain = value;
		}
	}

	public string Host
	{
		get
		{
			return _host;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (value == "")
			{
				base.Flags &= ~NtlmFlags.NegotiateWorkstationSupplied;
			}
			else
			{
				base.Flags |= NtlmFlags.NegotiateWorkstationSupplied;
			}
			_host = value;
		}
	}

	public string Password
	{
		get
		{
			return _password;
		}
		set
		{
			_password = value;
		}
	}

	public string Username
	{
		get
		{
			return _username;
		}
		set
		{
			_username = value;
		}
	}

	public byte[] LM => _lm;

	public byte[] NT
	{
		get
		{
			return _nt;
		}
		set
		{
			_nt = value;
		}
	}

	[Obsolete("Use of this API is highly discouraged, it selects legacy-mode LM/NTLM authentication, which sends your password in very weak encryption over the wire even if the server supports the more secure NTLMv2 / NTLMv2 Session. You need to use the new `Type3Message (Type2Message)' constructor to use the more secure NTLMv2 / NTLMv2 Session authentication modes. These require the Type 2 message from the server to compute the response.")]
	public Type3Message()
		: base(3)
	{
		if (DefaultAuthLevel != NtlmAuthLevel.LM_and_NTLM)
		{
			throw new InvalidOperationException("Refusing to use legacy-mode LM/NTLM authentication unless explicitly enabled using DefaultAuthLevel.");
		}
		_domain = Environment.UserDomainName;
		_host = Environment.MachineName;
		_username = Environment.UserName;
		_level = NtlmAuthLevel.LM_and_NTLM;
		base.Flags = NtlmFlags.NegotiateUnicode | NtlmFlags.NegotiateNtlm | NtlmFlags.NegotiateAlwaysSign;
	}

	public Type3Message(byte[] message)
		: base(3)
	{
		Decode(message);
	}

	public Type3Message(Type2Message type2)
		: base(3)
	{
		_type2 = type2;
		_level = NtlmSettings.DefaultAuthLevel;
		_challenge = (byte[])type2.Nonce.Clone();
		_domain = type2.TargetName;
		_host = Environment.MachineName;
		_username = Environment.UserName;
		base.Flags = NtlmFlags.NegotiateNtlm | NtlmFlags.NegotiateAlwaysSign;
		if ((type2.Flags & NtlmFlags.NegotiateUnicode) != 0)
		{
			base.Flags |= NtlmFlags.NegotiateUnicode;
		}
		else
		{
			base.Flags |= NtlmFlags.NegotiateOem;
		}
		if ((type2.Flags & NtlmFlags.NegotiateNtlm2Key) != 0)
		{
			base.Flags |= NtlmFlags.NegotiateNtlm2Key;
		}
	}

	~Type3Message()
	{
		if (_challenge != null)
		{
			Array.Clear(_challenge, 0, _challenge.Length);
		}
		if (_lm != null)
		{
			Array.Clear(_lm, 0, _lm.Length);
		}
		if (_nt != null)
		{
			Array.Clear(_nt, 0, _nt.Length);
		}
	}

	protected override void Decode(byte[] message)
	{
		base.Decode(message);
		_password = null;
		if (message.Length >= 64)
		{
			base.Flags = (NtlmFlags)Mono.Security.BitConverterLE.ToUInt32(message, 60);
		}
		else
		{
			base.Flags = NtlmFlags.NegotiateUnicode | NtlmFlags.NegotiateNtlm | NtlmFlags.NegotiateAlwaysSign;
		}
		int num = Mono.Security.BitConverterLE.ToUInt16(message, 12);
		int srcOffset = Mono.Security.BitConverterLE.ToUInt16(message, 16);
		_lm = new byte[num];
		Buffer.BlockCopy(message, srcOffset, _lm, 0, num);
		int num2 = Mono.Security.BitConverterLE.ToUInt16(message, 20);
		int srcOffset2 = Mono.Security.BitConverterLE.ToUInt16(message, 24);
		_nt = new byte[num2];
		Buffer.BlockCopy(message, srcOffset2, _nt, 0, num2);
		int len = Mono.Security.BitConverterLE.ToUInt16(message, 28);
		int offset = Mono.Security.BitConverterLE.ToUInt16(message, 32);
		_domain = DecodeString(message, offset, len);
		int len2 = Mono.Security.BitConverterLE.ToUInt16(message, 36);
		int offset2 = Mono.Security.BitConverterLE.ToUInt16(message, 40);
		_username = DecodeString(message, offset2, len2);
		int len3 = Mono.Security.BitConverterLE.ToUInt16(message, 44);
		int offset3 = Mono.Security.BitConverterLE.ToUInt16(message, 48);
		_host = DecodeString(message, offset3, len3);
	}

	private string DecodeString(byte[] buffer, int offset, int len)
	{
		if ((base.Flags & NtlmFlags.NegotiateUnicode) != 0)
		{
			return Encoding.Unicode.GetString(buffer, offset, len);
		}
		return Encoding.ASCII.GetString(buffer, offset, len);
	}

	private byte[] EncodeString(string text)
	{
		if (text == null)
		{
			return new byte[0];
		}
		if ((base.Flags & NtlmFlags.NegotiateUnicode) != 0)
		{
			return Encoding.Unicode.GetBytes(text);
		}
		return Encoding.ASCII.GetBytes(text);
	}

	public override byte[] GetBytes()
	{
		byte[] array = EncodeString(_domain);
		byte[] array2 = EncodeString(_username);
		byte[] array3 = EncodeString(_host);
		byte[] lm;
		byte[] ntlm;
		if (_type2 == null)
		{
			if (_level != NtlmAuthLevel.LM_and_NTLM)
			{
				throw new InvalidOperationException("Refusing to use legacy-mode LM/NTLM authentication unless explicitly enabled using DefaultAuthLevel.");
			}
			using ChallengeResponse challengeResponse = new ChallengeResponse(_password, _challenge);
			lm = challengeResponse.LM;
			ntlm = challengeResponse.NT;
		}
		else
		{
			ChallengeResponse2.Compute(_type2, _level, _username, _password, _domain, out lm, out ntlm);
		}
		int num = ((lm != null) ? lm.Length : 0);
		int num2 = ((ntlm != null) ? ntlm.Length : 0);
		byte[] array4 = PrepareMessage(64 + array.Length + array2.Length + array3.Length + num + num2);
		short num3 = (short)(64 + array.Length + array2.Length + array3.Length);
		array4[12] = (byte)num;
		array4[13] = 0;
		array4[14] = (byte)num;
		array4[15] = 0;
		array4[16] = (byte)num3;
		array4[17] = (byte)(num3 >> 8);
		short num4 = (short)(num3 + num);
		array4[20] = (byte)num2;
		array4[21] = (byte)(num2 >> 8);
		array4[22] = (byte)num2;
		array4[23] = (byte)(num2 >> 8);
		array4[24] = (byte)num4;
		array4[25] = (byte)(num4 >> 8);
		short num5 = (short)array.Length;
		short num6 = 64;
		array4[28] = (byte)num5;
		array4[29] = (byte)(num5 >> 8);
		array4[30] = array4[28];
		array4[31] = array4[29];
		array4[32] = (byte)num6;
		array4[33] = (byte)(num6 >> 8);
		short num7 = (short)array2.Length;
		short num8 = (short)(num6 + num5);
		array4[36] = (byte)num7;
		array4[37] = (byte)(num7 >> 8);
		array4[38] = array4[36];
		array4[39] = array4[37];
		array4[40] = (byte)num8;
		array4[41] = (byte)(num8 >> 8);
		short num9 = (short)array3.Length;
		short num10 = (short)(num8 + num7);
		array4[44] = (byte)num9;
		array4[45] = (byte)(num9 >> 8);
		array4[46] = array4[44];
		array4[47] = array4[45];
		array4[48] = (byte)num10;
		array4[49] = (byte)(num10 >> 8);
		short num11 = (short)array4.Length;
		array4[56] = (byte)num11;
		array4[57] = (byte)(num11 >> 8);
		int flags = (int)base.Flags;
		array4[60] = (byte)flags;
		array4[61] = (byte)((uint)flags >> 8);
		array4[62] = (byte)((uint)flags >> 16);
		array4[63] = (byte)((uint)flags >> 24);
		Buffer.BlockCopy(array, 0, array4, num6, array.Length);
		Buffer.BlockCopy(array2, 0, array4, num8, array2.Length);
		Buffer.BlockCopy(array3, 0, array4, num10, array3.Length);
		if (lm != null)
		{
			Buffer.BlockCopy(lm, 0, array4, num3, lm.Length);
			Array.Clear(lm, 0, lm.Length);
		}
		Buffer.BlockCopy(ntlm, 0, array4, num4, ntlm.Length);
		Array.Clear(ntlm, 0, ntlm.Length);
		return array4;
	}
}
