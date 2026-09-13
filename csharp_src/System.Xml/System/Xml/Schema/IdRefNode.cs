namespace System.Xml.Schema;

internal class IdRefNode
{
	internal string Id;

	internal int LineNo;

	internal int LinePos;

	internal IdRefNode Next;

	internal IdRefNode(IdRefNode next, string id, int lineNo, int linePos)
	{
		Id = id;
		LineNo = lineNo;
		LinePos = linePos;
		Next = next;
	}
}
