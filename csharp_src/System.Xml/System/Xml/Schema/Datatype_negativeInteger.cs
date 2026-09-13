namespace System.Xml.Schema;

internal class Datatype_negativeInteger : Datatype_nonPositiveInteger
{
	private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(decimal.MinValue, -1m);

	internal override FacetsChecker FacetsChecker => numeric10FacetsChecker;

	public override XmlTypeCode TypeCode => XmlTypeCode.NegativeInteger;
}
