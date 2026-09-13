using System.Collections.Generic;

namespace System.Xml.Schema;

internal sealed class ValidationState
{
	public bool IsNill;

	public bool IsDefault;

	public bool NeedValidateChildren;

	public bool CheckRequiredAttribute;

	public bool ValidationSkipped;

	public int Depth;

	public XmlSchemaContentProcessing ProcessContents;

	public XmlSchemaValidity Validity;

	public SchemaElementDecl ElementDecl;

	public SchemaElementDecl ElementDeclBeforeXsi;

	public string LocalName;

	public string Namespace;

	public ConstraintStruct[] Constr;

	public StateUnion CurrentState;

	public bool HasMatched;

	public BitSet[] CurPos = new BitSet[2];

	public BitSet AllElementsSet;

	public List<RangePositionInfo> RunningPositions;

	public bool TooComplex;
}
