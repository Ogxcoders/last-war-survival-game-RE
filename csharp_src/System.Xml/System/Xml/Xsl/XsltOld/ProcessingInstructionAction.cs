using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class ProcessingInstructionAction : ContainerAction
{
	private const int NameEvaluated = 2;

	private const int NameReady = 3;

	private Avt nameAvt;

	private string name;

	private const char CharX = 'X';

	private const char Charx = 'x';

	private const char CharM = 'M';

	private const char Charm = 'm';

	private const char CharL = 'L';

	private const char Charl = 'l';

	internal ProcessingInstructionAction()
	{
	}

	internal override void Compile(Compiler compiler)
	{
		CompileAttributes(compiler);
		CheckRequiredAttribute(compiler, nameAvt, "name");
		if (nameAvt.IsConstant)
		{
			name = nameAvt.Evaluate(null, null);
			nameAvt = null;
			if (!IsProcessingInstructionName(name))
			{
				name = null;
			}
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
		if (Ref.Equal(localName, compiler.Atoms.Name))
		{
			nameAvt = Avt.CompileAvt(compiler, value);
			return true;
		}
		return false;
	}

	internal override void Execute(Processor processor, ActionFrame frame)
	{
		switch (frame.State)
		{
		case 0:
			if (nameAvt == null)
			{
				frame.StoredOutput = name;
				if (name == null)
				{
					frame.Finished();
					break;
				}
			}
			else
			{
				frame.StoredOutput = nameAvt.Evaluate(processor, frame);
				if (!IsProcessingInstructionName(frame.StoredOutput))
				{
					frame.Finished();
					break;
				}
			}
			goto case 3;
		case 3:
			if (!processor.BeginEvent(XPathNodeType.ProcessingInstruction, string.Empty, frame.StoredOutput, string.Empty, empty: false))
			{
				frame.State = 3;
				break;
			}
			processor.PushActionFrame(frame);
			frame.State = 1;
			break;
		case 1:
			if (!processor.EndEvent(XPathNodeType.ProcessingInstruction))
			{
				frame.State = 1;
			}
			else
			{
				frame.Finished();
			}
			break;
		default:
			frame.Finished();
			break;
		}
	}

	internal static bool IsProcessingInstructionName(string name)
	{
		if (name == null)
		{
			return false;
		}
		int length = name.Length;
		int i = 0;
		XmlCharType instance;
		for (instance = XmlCharType.Instance; i < length && instance.IsWhiteSpace(name[i]); i++)
		{
		}
		if (i >= length)
		{
			return false;
		}
		int num = ValidateNames.ParseNCName(name, i);
		if (num == 0)
		{
			return false;
		}
		for (i += num; i < length && instance.IsWhiteSpace(name[i]); i++)
		{
		}
		if (i < length)
		{
			return false;
		}
		if (length == 3 && (name[0] == 'X' || name[0] == 'x') && (name[1] == 'M' || name[1] == 'm') && (name[2] == 'L' || name[2] == 'l'))
		{
			return false;
		}
		return true;
	}
}
