namespace System.Xml.Xsl;

public sealed class XsltSettings
{
	private bool enableDocumentFunction;

	private bool enableScript;

	private bool checkOnly;

	private bool includeDebugInformation;

	private int warningLevel = -1;

	private bool treatWarningsAsErrors;

	public static XsltSettings Default => new XsltSettings(enableDocumentFunction: false, enableScript: false);

	public static XsltSettings TrustedXslt => new XsltSettings(enableDocumentFunction: true, enableScript: true);

	public bool EnableDocumentFunction
	{
		get
		{
			return enableDocumentFunction;
		}
		set
		{
			enableDocumentFunction = value;
		}
	}

	public bool EnableScript
	{
		get
		{
			return enableScript;
		}
		set
		{
			enableScript = value;
		}
	}

	internal bool CheckOnly
	{
		get
		{
			return checkOnly;
		}
		set
		{
			checkOnly = value;
		}
	}

	internal bool IncludeDebugInformation
	{
		get
		{
			return includeDebugInformation;
		}
		set
		{
			includeDebugInformation = value;
		}
	}

	internal int WarningLevel
	{
		get
		{
			return warningLevel;
		}
		set
		{
			warningLevel = value;
		}
	}

	internal bool TreatWarningsAsErrors
	{
		get
		{
			return treatWarningsAsErrors;
		}
		set
		{
			treatWarningsAsErrors = value;
		}
	}

	public XsltSettings()
	{
	}

	public XsltSettings(bool enableDocumentFunction, bool enableScript)
	{
		this.enableDocumentFunction = enableDocumentFunction;
		this.enableScript = enableScript;
	}
}
