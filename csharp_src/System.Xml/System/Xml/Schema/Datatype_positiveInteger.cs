namespace System.Xml.Schema;

internal class Datatype_positiveInteger : Datatype_nonNegativeInteger
{
	private static readonly FacetsChecker numeric10FacetsChecker = new Numeric10FacetsChecker(1m, decimal.MaxValue);

	internal override FacetsChecker FacetsChecker => numeric10FacetsChecker;

	public override XmlTypeCode TypeCode => XmlTypeCode.PositiveInteger;
}
