using System.Collections;

namespace System.Xml.Schema;

internal sealed class AllElementsContentValidator : ContentValidator
{
	private Hashtable elements;

	private object[] particles;

	private BitSet isRequired;

	private int countRequired;

	public override bool IsEmptiable
	{
		get
		{
			if (!base.IsEmptiable)
			{
				return countRequired == 0;
			}
			return true;
		}
	}

	public AllElementsContentValidator(XmlSchemaContentType contentType, int size, bool isEmptiable)
		: base(contentType, isOpen: false, isEmptiable)
	{
		elements = new Hashtable(size);
		particles = new object[size];
		isRequired = new BitSet(size);
	}

	public bool AddElement(XmlQualifiedName name, object particle, bool isEmptiable)
	{
		if (elements[name] != null)
		{
			return false;
		}
		int count = elements.Count;
		elements.Add(name, count);
		particles[count] = particle;
		if (!isEmptiable)
		{
			isRequired.Set(count);
			countRequired++;
		}
		return true;
	}

	public override void InitValidation(ValidationState context)
	{
		context.AllElementsSet = new BitSet(elements.Count);
		context.CurrentState.AllElementsRequired = -1;
	}

	public override object ValidateElement(XmlQualifiedName name, ValidationState context, out int errorCode)
	{
		object obj = elements[name];
		errorCode = 0;
		if (obj == null)
		{
			context.NeedValidateChildren = false;
			return null;
		}
		int num = (int)obj;
		if (context.AllElementsSet[num])
		{
			errorCode = -2;
			return null;
		}
		if (context.CurrentState.AllElementsRequired == -1)
		{
			context.CurrentState.AllElementsRequired = 0;
		}
		context.AllElementsSet.Set(num);
		if (isRequired[num])
		{
			context.CurrentState.AllElementsRequired++;
		}
		return particles[num];
	}

	public override bool CompleteValidation(ValidationState context)
	{
		if (context.CurrentState.AllElementsRequired == countRequired || (IsEmptiable && context.CurrentState.AllElementsRequired == -1))
		{
			return true;
		}
		return false;
	}

	public override ArrayList ExpectedElements(ValidationState context, bool isRequiredOnly)
	{
		ArrayList arrayList = null;
		foreach (DictionaryEntry element in elements)
		{
			if (!context.AllElementsSet[(int)element.Value] && (!isRequiredOnly || isRequired[(int)element.Value]))
			{
				if (arrayList == null)
				{
					arrayList = new ArrayList();
				}
				arrayList.Add(element.Key);
			}
		}
		return arrayList;
	}

	public override ArrayList ExpectedParticles(ValidationState context, bool isRequiredOnly, XmlSchemaSet schemaSet)
	{
		ArrayList result = new ArrayList();
		foreach (DictionaryEntry element in elements)
		{
			if (!context.AllElementsSet[(int)element.Value] && (!isRequiredOnly || isRequired[(int)element.Value]))
			{
				ContentValidator.AddParticleToExpected(particles[(int)element.Value] as XmlSchemaParticle, schemaSet, result);
			}
		}
		return result;
	}
}
