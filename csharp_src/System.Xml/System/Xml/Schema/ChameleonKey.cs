namespace System.Xml.Schema;

internal class ChameleonKey
{
	internal string targetNS;

	internal Uri chameleonLocation;

	internal XmlSchema originalSchema;

	private int hashCode;

	public ChameleonKey(string ns, XmlSchema originalSchema)
	{
		targetNS = ns;
		chameleonLocation = originalSchema.BaseUri;
		if (chameleonLocation.OriginalString.Length == 0)
		{
			this.originalSchema = originalSchema;
		}
	}

	public override int GetHashCode()
	{
		if (hashCode == 0)
		{
			hashCode = targetNS.GetHashCode() + chameleonLocation.GetHashCode() + ((originalSchema != null) ? originalSchema.GetHashCode() : 0);
		}
		return hashCode;
	}

	public override bool Equals(object obj)
	{
		if (this == obj)
		{
			return true;
		}
		if (obj is ChameleonKey chameleonKey)
		{
			if (targetNS.Equals(chameleonKey.targetNS) && chameleonLocation.Equals(chameleonKey.chameleonLocation))
			{
				return originalSchema == chameleonKey.originalSchema;
			}
			return false;
		}
		return false;
	}
}
