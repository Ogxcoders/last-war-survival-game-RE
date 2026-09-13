namespace System.Xml.Xsl.XsltOld;

internal class IfAction : ContainerAction
{
	internal enum ConditionType
	{
		ConditionIf,
		ConditionWhen,
		ConditionOtherwise
	}

	private ConditionType type;

	private int testKey = -1;

	internal IfAction(ConditionType type)
	{
		this.type = type;
	}

	internal override void Compile(Compiler compiler)
	{
		CompileAttributes(compiler);
		if (type != ConditionType.ConditionOtherwise)
		{
			CheckRequiredAttribute(compiler, testKey != -1, "test");
		}
		if (compiler.Recurse())
		{
			CompileTemplate(compiler);
			compiler.ToParent();
		}
	}

	internal override bool CompileAttribute(Compiler compiler)
	{
		string localName = compiler.Input.LocalName;
		string value = compiler.Input.Value;
		if (Ref.Equal(localName, compiler.Atoms.Test))
		{
			if (type == ConditionType.ConditionOtherwise)
			{
				return false;
			}
			testKey = compiler.AddBooleanQuery(value);
			return true;
		}
		return false;
	}

	internal override void Execute(Processor processor, ActionFrame frame)
	{
		switch (frame.State)
		{
		case 0:
			if ((type == ConditionType.ConditionIf || type == ConditionType.ConditionWhen) && !processor.EvaluateBoolean(frame, testKey))
			{
				frame.Finished();
				break;
			}
			processor.PushActionFrame(frame);
			frame.State = 1;
			break;
		case 1:
			if (type == ConditionType.ConditionWhen || type == ConditionType.ConditionOtherwise)
			{
				frame.Exit();
			}
			frame.Finished();
			break;
		}
	}
}
