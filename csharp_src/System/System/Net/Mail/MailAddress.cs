using System.Globalization;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail;

public class MailAddress
{
	private readonly Encoding _displayNameEncoding;

	private readonly string _displayName;

	private readonly string _userName;

	private readonly string _host;

	private static readonly EncodedStreamFactory s_encoderFactory = new EncodedStreamFactory();

	public string DisplayName => _displayName;

	public string User => _userName;

	public string Host => _host;

	public string Address => _userName + "@" + _host;

	private string SmtpAddress => "<" + Address + ">";

	internal MailAddress(string displayName, string userName, string domain)
	{
		_host = domain;
		_userName = userName;
		_displayName = displayName;
		_displayNameEncoding = Encoding.GetEncoding("utf-8");
	}

	public MailAddress(string address)
		: this(address, (string)null, (Encoding)null)
	{
	}

	public MailAddress(string address, string displayName)
		: this(address, displayName, (Encoding)null)
	{
	}

	public MailAddress(string address, string displayName, Encoding displayNameEncoding)
	{
		if (address == null)
		{
			throw new ArgumentNullException("address");
		}
		if (address == string.Empty)
		{
			throw new ArgumentException(global::SR.Format("The parameter '{0}' cannot be an empty string.", "address"), "address");
		}
		_displayNameEncoding = displayNameEncoding ?? Encoding.GetEncoding("utf-8");
		_displayName = displayName ?? string.Empty;
		if (!string.IsNullOrEmpty(_displayName))
		{
			_displayName = MailAddressParser.NormalizeOrThrow(_displayName);
			if (_displayName.Length >= 2 && _displayName[0] == '"' && _displayName[_displayName.Length - 1] == '"')
			{
				_displayName = _displayName.Substring(1, _displayName.Length - 2);
			}
		}
		MailAddress mailAddress = MailAddressParser.ParseAddress(address);
		_host = mailAddress._host;
		_userName = mailAddress._userName;
		if (string.IsNullOrEmpty(_displayName))
		{
			_displayName = mailAddress._displayName;
		}
	}

	private string GetUser(bool allowUnicode)
	{
		if (!allowUnicode && !MimeBasePart.IsAscii(_userName, permitCROrLF: true))
		{
			throw new SmtpException(global::SR.Format("The client or server is only configured for E-mail addresses with ASCII local-parts: {0}.", Address));
		}
		return _userName;
	}

	private string GetHost(bool allowUnicode)
	{
		string text = _host;
		if (!allowUnicode && !MimeBasePart.IsAscii(text, permitCROrLF: true))
		{
			IdnMapping idnMapping = new IdnMapping();
			try
			{
				text = idnMapping.GetAscii(text);
			}
			catch (ArgumentException innerException)
			{
				throw new SmtpException(global::SR.Format("The address has an invalid host name: {0}.", Address), innerException);
			}
		}
		return text;
	}

	private string GetAddress(bool allowUnicode)
	{
		return GetUser(allowUnicode) + "@" + GetHost(allowUnicode);
	}

	internal string GetSmtpAddress(bool allowUnicode)
	{
		return "<" + GetAddress(allowUnicode) + ">";
	}

	public override string ToString()
	{
		if (string.IsNullOrEmpty(DisplayName))
		{
			return Address;
		}
		return "\"" + DisplayName + "\" " + SmtpAddress;
	}

	public override bool Equals(object value)
	{
		if (value == null)
		{
			return false;
		}
		return ToString().Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase);
	}

	public override int GetHashCode()
	{
		return ToString().GetHashCode();
	}

	internal string Encode(int charsConsumed, bool allowUnicode)
	{
		string empty = string.Empty;
		if (!string.IsNullOrEmpty(_displayName))
		{
			if (MimeBasePart.IsAscii(_displayName, permitCROrLF: false) || allowUnicode)
			{
				empty = "\"" + _displayName + "\"";
			}
			else
			{
				IEncodableStream encoderForHeader = s_encoderFactory.GetEncoderForHeader(_displayNameEncoding, useBase64Encoding: false, charsConsumed);
				byte[] bytes = _displayNameEncoding.GetBytes(_displayName);
				encoderForHeader.EncodeBytes(bytes, 0, bytes.Length);
				empty = encoderForHeader.GetEncodedString();
			}
			return empty + " " + GetSmtpAddress(allowUnicode);
		}
		return GetAddress(allowUnicode);
	}
}
