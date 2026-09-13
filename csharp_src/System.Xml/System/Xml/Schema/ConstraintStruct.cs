using System.Collections;

namespace System.Xml.Schema;

internal sealed class ConstraintStruct
{
	internal CompiledIdentityConstraint constraint;

	internal SelectorActiveAxis axisSelector;

	internal ArrayList axisFields;

	internal Hashtable qualifiedTable;

	internal Hashtable keyrefTable;

	private int tableDim;

	internal int TableDim => tableDim;

	internal ConstraintStruct(CompiledIdentityConstraint constraint)
	{
		this.constraint = constraint;
		tableDim = constraint.Fields.Length;
		axisFields = new ArrayList();
		axisSelector = new SelectorActiveAxis(constraint.Selector, this);
		if (this.constraint.Role != CompiledIdentityConstraint.ConstraintRole.Keyref)
		{
			qualifiedTable = new Hashtable();
		}
	}
}
