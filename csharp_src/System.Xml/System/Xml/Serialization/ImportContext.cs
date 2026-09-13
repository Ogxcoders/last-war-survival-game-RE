using System.Collections;
using System.Collections.Specialized;

namespace System.Xml.Serialization;

public class ImportContext
{
	private bool _shareTypes;

	private CodeIdentifiers _typeIdentifiers;

	private StringCollection _warnings = new StringCollection();

	internal Hashtable MappedTypes;

	internal Hashtable DataMappedTypes;

	internal Hashtable SharedAnonymousTypes;

	public bool ShareTypes => _shareTypes;

	public CodeIdentifiers TypeIdentifiers => _typeIdentifiers;

	public StringCollection Warnings => _warnings;

	public ImportContext(CodeIdentifiers identifiers, bool shareTypes)
	{
		_typeIdentifiers = identifiers;
		_shareTypes = shareTypes;
		if (shareTypes)
		{
			MappedTypes = new Hashtable();
			DataMappedTypes = new Hashtable();
			SharedAnonymousTypes = new Hashtable();
		}
	}
}
