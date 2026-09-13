namespace System.Xml.Xsl.XsltOld;

internal interface RecordOutput
{
	Processor.OutputResult RecordDone(RecordBuilder record);

	void TheEnd();
}
