namespace System.Xml.Xsl.XsltOld;

internal class UseAttributeSetsAction : CompiledAction
{
	private XmlQualifiedName[] useAttributeSets;

	private string useString;

	private const int ProcessingSets = 2;

	internal XmlQualifiedName[] UsedSets => useAttributeSets;

	internal override void Compile(Compiler compiler)
	{
		useString = compiler.Input.Value;
		if (useString.Length == 0)
		{
			useAttributeSets = new XmlQualifiedName[0];
			return;
		}
		string[] array = XmlConvert.SplitString(useString);
		try
		{
			useAttributeSets = new XmlQualifiedName[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				useAttributeSets[i] = compiler.CreateXPathQName(array[i]);
			}
		}
		catch (XsltException)
		{
			if (!compiler.ForwardCompatibility)
			{
				throw;
			}
			useAttributeSets = new XmlQualifiedName[0];
		}
	}

	internal override void Execute(Processor processor, ActionFrame frame)
	{
		switch (frame.State)
		{
		default:
			return;
		case 0:
			frame.Counter = 0;
			frame.State = 2;
			break;
		case 2:
			break;
		}
		if (frame.Counter < useAttributeSets.Length)
		{
			AttributeSetAction attributeSet = processor.RootAction.GetAttributeSet(useAttributeSets[frame.Counter]);
			frame.IncrementCounter();
			processor.PushActionFrame(attributeSet, frame.NodeSet);
		}
		else
		{
			frame.Finished();
		}
	}
}
