namespace System.Xml;

internal interface IRemovableWriter
{
	OnRemoveWriter OnRemoveWriterEvent { get; set; }
}
