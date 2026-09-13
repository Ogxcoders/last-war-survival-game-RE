using System.Collections.Specialized;
using System.Globalization;
using System.Net.Mail;
using System.Text;

namespace System.Net.Mime;

internal class HeaderCollection : NameValueCollection
{
	private MimeBasePart _part;

	internal HeaderCollection()
		: base(StringComparer.OrdinalIgnoreCase)
	{
	}

	public override void Remove(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name == string.Empty)
		{
			throw new ArgumentException(global::SR.Format("The parameter '{0}' cannot be an empty string.", "name"), "name");
		}
		MailHeaderID iD = MailHeaderInfo.GetID(name);
		if (iD == MailHeaderID.ContentType && _part != null)
		{
			_part.ContentType = null;
		}
		else if (iD == MailHeaderID.ContentDisposition && _part is MimePart)
		{
			((MimePart)_part).ContentDisposition = null;
		}
		base.Remove(name);
	}

	public override string Get(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name == string.Empty)
		{
			throw new ArgumentException(global::SR.Format("The parameter '{0}' cannot be an empty string.", "name"), "name");
		}
		MailHeaderID iD = MailHeaderInfo.GetID(name);
		if (iD == MailHeaderID.ContentType && _part != null)
		{
			_part.ContentType.PersistIfNeeded(this, forcePersist: false);
		}
		else if (iD == MailHeaderID.ContentDisposition && _part is MimePart)
		{
			((MimePart)_part).ContentDisposition.PersistIfNeeded(this, forcePersist: false);
		}
		return base.Get(name);
	}

	public override string[] GetValues(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name == string.Empty)
		{
			throw new ArgumentException(global::SR.Format("The parameter '{0}' cannot be an empty string.", "name"), "name");
		}
		MailHeaderID iD = MailHeaderInfo.GetID(name);
		if (iD == MailHeaderID.ContentType && _part != null)
		{
			_part.ContentType.PersistIfNeeded(this, forcePersist: false);
		}
		else if (iD == MailHeaderID.ContentDisposition && _part is MimePart)
		{
			((MimePart)_part).ContentDisposition.PersistIfNeeded(this, forcePersist: false);
		}
		return base.GetValues(name);
	}

	internal void InternalRemove(string name)
	{
		base.Remove(name);
	}

	internal void InternalSet(string name, string value)
	{
		base.Set(name, value);
	}

	internal void InternalAdd(string name, string value)
	{
		if (MailHeaderInfo.IsSingleton(name))
		{
			base.Set(name, value);
		}
		else
		{
			base.Add(name, value);
		}
	}

	public override void Set(string name, string value)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (name == string.Empty)
		{
			throw new ArgumentException(global::SR.Format("The parameter '{0}' cannot be an empty string.", "name"), "name");
		}
		if (value == string.Empty)
		{
			throw new ArgumentException(global::SR.Format("The parameter '{0}' cannot be an empty string.", "value"), "value");
		}
		if (!MimeBasePart.IsAscii(name, permitCROrLF: false))
		{
			throw new FormatException(global::SR.Format("An invalid character was found in header name."));
		}
		name = MailHeaderInfo.NormalizeCase(name);
		MailHeaderID iD = MailHeaderInfo.GetID(name);
		value = value.Normalize(NormalizationForm.FormC);
		if (iD == MailHeaderID.ContentType && _part != null)
		{
			_part.ContentType.Set(value.ToLower(CultureInfo.InvariantCulture), this);
		}
		else if (iD == MailHeaderID.ContentDisposition && _part is MimePart)
		{
			((MimePart)_part).ContentDisposition.Set(value.ToLower(CultureInfo.InvariantCulture), this);
		}
		else
		{
			base.Set(name, value);
		}
	}

	public override void Add(string name, string value)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (name == string.Empty)
		{
			throw new ArgumentException(global::SR.Format("The parameter '{0}' cannot be an empty string.", "name"), "name");
		}
		if (value == string.Empty)
		{
			throw new ArgumentException(global::SR.Format("The parameter '{0}' cannot be an empty string.", "value"), "value");
		}
		MailBnfHelper.ValidateHeaderName(name);
		name = MailHeaderInfo.NormalizeCase(name);
		MailHeaderID iD = MailHeaderInfo.GetID(name);
		value = value.Normalize(NormalizationForm.FormC);
		if (iD == MailHeaderID.ContentType && _part != null)
		{
			_part.ContentType.Set(value.ToLower(CultureInfo.InvariantCulture), this);
		}
		else if (iD == MailHeaderID.ContentDisposition && _part is MimePart)
		{
			((MimePart)_part).ContentDisposition.Set(value.ToLower(CultureInfo.InvariantCulture), this);
		}
		else
		{
			InternalAdd(name, value);
		}
	}
}
