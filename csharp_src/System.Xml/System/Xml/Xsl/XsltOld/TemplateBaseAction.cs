namespace System.Xml.Xsl.XsltOld;

internal abstract class TemplateBaseAction : ContainerAction
{
	protected int variableCount;

	private int variableFreeSlot;

	public int AllocateVariableSlot()
	{
		int result = variableFreeSlot;
		variableFreeSlot++;
		if (variableCount < variableFreeSlot)
		{
			variableCount = variableFreeSlot;
		}
		return result;
	}

	public void ReleaseVariableSlots(int n)
	{
	}
}
