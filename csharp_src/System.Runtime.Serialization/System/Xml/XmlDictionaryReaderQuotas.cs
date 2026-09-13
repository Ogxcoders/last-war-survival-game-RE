using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Xml;

public sealed class XmlDictionaryReaderQuotas
{
	private bool readOnly;

	private int maxStringContentLength;

	private int maxArrayLength;

	private int maxDepth;

	private int maxNameTableCharCount;

	private int maxBytesPerRead;

	private XmlDictionaryReaderQuotaTypes modifiedQuotas;

	private const int DefaultMaxDepth = 32;

	private const int DefaultMaxStringContentLength = 8192;

	private const int DefaultMaxArrayLength = 16384;

	private const int DefaultMaxBytesPerRead = 4096;

	private const int DefaultMaxNameTableCharCount = 16384;

	private static XmlDictionaryReaderQuotas defaultQuota = new XmlDictionaryReaderQuotas(32, 8192, 16384, 4096, 16384, (XmlDictionaryReaderQuotaTypes)0);

	private static XmlDictionaryReaderQuotas maxQuota = new XmlDictionaryReaderQuotas(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, XmlDictionaryReaderQuotaTypes.MaxDepth | XmlDictionaryReaderQuotaTypes.MaxStringContentLength | XmlDictionaryReaderQuotaTypes.MaxArrayLength | XmlDictionaryReaderQuotaTypes.MaxBytesPerRead | XmlDictionaryReaderQuotaTypes.MaxNameTableCharCount);

	public static XmlDictionaryReaderQuotas Max => maxQuota;

	[DefaultValue(8192)]
	public int MaxStringContentLength
	{
		get
		{
			return maxStringContentLength;
		}
		set
		{
			if (readOnly)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("The '{0}' quota is readonly.", "MaxStringContentLength")));
			}
			if (value <= 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Quota must be a positive value."), "value"));
			}
			maxStringContentLength = value;
			modifiedQuotas |= XmlDictionaryReaderQuotaTypes.MaxStringContentLength;
		}
	}

	[DefaultValue(16384)]
	public int MaxArrayLength
	{
		get
		{
			return maxArrayLength;
		}
		set
		{
			if (readOnly)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("The '{0}' quota is readonly.", "MaxArrayLength")));
			}
			if (value <= 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Quota must be a positive value."), "value"));
			}
			maxArrayLength = value;
			modifiedQuotas |= XmlDictionaryReaderQuotaTypes.MaxArrayLength;
		}
	}

	[DefaultValue(4096)]
	public int MaxBytesPerRead
	{
		get
		{
			return maxBytesPerRead;
		}
		set
		{
			if (readOnly)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("The '{0}' quota is readonly.", "MaxBytesPerRead")));
			}
			if (value <= 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Quota must be a positive value."), "value"));
			}
			maxBytesPerRead = value;
			modifiedQuotas |= XmlDictionaryReaderQuotaTypes.MaxBytesPerRead;
		}
	}

	[DefaultValue(32)]
	public int MaxDepth
	{
		get
		{
			return maxDepth;
		}
		set
		{
			if (readOnly)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("The '{0}' quota is readonly.", "MaxDepth")));
			}
			if (value <= 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Quota must be a positive value."), "value"));
			}
			maxDepth = value;
			modifiedQuotas |= XmlDictionaryReaderQuotaTypes.MaxDepth;
		}
	}

	[DefaultValue(16384)]
	public int MaxNameTableCharCount
	{
		get
		{
			return maxNameTableCharCount;
		}
		set
		{
			if (readOnly)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("The '{0}' quota is readonly.", "MaxNameTableCharCount")));
			}
			if (value <= 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Quota must be a positive value."), "value"));
			}
			maxNameTableCharCount = value;
			modifiedQuotas |= XmlDictionaryReaderQuotaTypes.MaxNameTableCharCount;
		}
	}

	public XmlDictionaryReaderQuotaTypes ModifiedQuotas => modifiedQuotas;

	public XmlDictionaryReaderQuotas()
	{
		defaultQuota.CopyTo(this);
	}

	private XmlDictionaryReaderQuotas(int maxDepth, int maxStringContentLength, int maxArrayLength, int maxBytesPerRead, int maxNameTableCharCount, XmlDictionaryReaderQuotaTypes modifiedQuotas)
	{
		this.maxDepth = maxDepth;
		this.maxStringContentLength = maxStringContentLength;
		this.maxArrayLength = maxArrayLength;
		this.maxBytesPerRead = maxBytesPerRead;
		this.maxNameTableCharCount = maxNameTableCharCount;
		this.modifiedQuotas = modifiedQuotas;
		MakeReadOnly();
	}

	public void CopyTo(XmlDictionaryReaderQuotas quotas)
	{
		if (quotas == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("quotas"));
		}
		if (quotas.readOnly)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Cannot copy XmlDictionaryReaderQuotas. Target is readonly.")));
		}
		InternalCopyTo(quotas);
	}

	internal void InternalCopyTo(XmlDictionaryReaderQuotas quotas)
	{
		quotas.maxStringContentLength = maxStringContentLength;
		quotas.maxArrayLength = maxArrayLength;
		quotas.maxDepth = maxDepth;
		quotas.maxNameTableCharCount = maxNameTableCharCount;
		quotas.maxBytesPerRead = maxBytesPerRead;
		quotas.modifiedQuotas = modifiedQuotas;
	}

	internal void MakeReadOnly()
	{
		readOnly = true;
	}
}
