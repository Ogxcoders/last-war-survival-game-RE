namespace System.Xml.Schema;

internal class CompiledIdentityConstraint
{
	public enum ConstraintRole
	{
		Unique,
		Key,
		Keyref
	}

	internal XmlQualifiedName name = XmlQualifiedName.Empty;

	private ConstraintRole role;

	private Asttree selector;

	private Asttree[] fields;

	internal XmlQualifiedName refer = XmlQualifiedName.Empty;

	public static readonly CompiledIdentityConstraint Empty = new CompiledIdentityConstraint();

	public ConstraintRole Role => role;

	public Asttree Selector => selector;

	public Asttree[] Fields => fields;

	private CompiledIdentityConstraint()
	{
	}

	public CompiledIdentityConstraint(XmlSchemaIdentityConstraint constraint, XmlNamespaceManager nsmgr)
	{
		name = constraint.QualifiedName;
		try
		{
			selector = new Asttree(constraint.Selector.XPath, isField: false, nsmgr);
		}
		catch (XmlSchemaException ex)
		{
			ex.SetSource(constraint.Selector);
			throw ex;
		}
		XmlSchemaObjectCollection xmlSchemaObjectCollection = constraint.Fields;
		fields = new Asttree[xmlSchemaObjectCollection.Count];
		for (int i = 0; i < xmlSchemaObjectCollection.Count; i++)
		{
			try
			{
				fields[i] = new Asttree(((XmlSchemaXPath)xmlSchemaObjectCollection[i]).XPath, isField: true, nsmgr);
			}
			catch (XmlSchemaException ex2)
			{
				ex2.SetSource(constraint.Fields[i]);
				throw ex2;
			}
		}
		if (constraint is XmlSchemaUnique)
		{
			role = ConstraintRole.Unique;
			return;
		}
		if (constraint is XmlSchemaKey)
		{
			role = ConstraintRole.Key;
			return;
		}
		role = ConstraintRole.Keyref;
		refer = ((XmlSchemaKeyref)constraint).Refer;
	}
}
