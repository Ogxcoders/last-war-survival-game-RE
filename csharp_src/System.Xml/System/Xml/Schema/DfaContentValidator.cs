using System.Collections;

namespace System.Xml.Schema;

internal sealed class DfaContentValidator : ContentValidator
{
	private int[][] transitionTable;

	private SymbolsDictionary symbols;

	internal DfaContentValidator(int[][] transitionTable, SymbolsDictionary symbols, XmlSchemaContentType contentType, bool isOpen, bool isEmptiable)
		: base(contentType, isOpen, isEmptiable)
	{
		this.transitionTable = transitionTable;
		this.symbols = symbols;
	}

	public override void InitValidation(ValidationState context)
	{
		context.CurrentState.State = 0;
		context.HasMatched = transitionTable[0][symbols.Count] > 0;
	}

	public override object ValidateElement(XmlQualifiedName name, ValidationState context, out int errorCode)
	{
		int num = symbols[name];
		int num2 = transitionTable[context.CurrentState.State][num];
		errorCode = 0;
		if (num2 != -1)
		{
			context.CurrentState.State = num2;
			context.HasMatched = transitionTable[context.CurrentState.State][symbols.Count] > 0;
			return symbols.GetParticle(num);
		}
		if (base.IsOpen && context.HasMatched)
		{
			return null;
		}
		context.NeedValidateChildren = false;
		errorCode = -1;
		return null;
	}

	public override bool CompleteValidation(ValidationState context)
	{
		if (!context.HasMatched)
		{
			return false;
		}
		return true;
	}

	public override ArrayList ExpectedElements(ValidationState context, bool isRequiredOnly)
	{
		ArrayList arrayList = null;
		int[] array = transitionTable[context.CurrentState.State];
		if (array != null)
		{
			for (int i = 0; i < array.Length - 1; i++)
			{
				if (array[i] == -1)
				{
					continue;
				}
				if (arrayList == null)
				{
					arrayList = new ArrayList();
				}
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)symbols.GetParticle(i);
				if (xmlSchemaParticle == null)
				{
					string text = symbols.NameOf(i);
					if (text.Length != 0)
					{
						arrayList.Add(text);
					}
				}
				else
				{
					string nameString = xmlSchemaParticle.NameString;
					if (!arrayList.Contains(nameString))
					{
						arrayList.Add(nameString);
					}
				}
			}
		}
		return arrayList;
	}

	public override ArrayList ExpectedParticles(ValidationState context, bool isRequiredOnly, XmlSchemaSet schemaSet)
	{
		ArrayList arrayList = new ArrayList();
		int[] array = transitionTable[context.CurrentState.State];
		if (array != null)
		{
			for (int i = 0; i < array.Length - 1; i++)
			{
				if (array[i] != -1)
				{
					XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)symbols.GetParticle(i);
					if (xmlSchemaParticle != null)
					{
						ContentValidator.AddParticleToExpected(xmlSchemaParticle, schemaSet, arrayList);
					}
				}
			}
		}
		return arrayList;
	}
}
