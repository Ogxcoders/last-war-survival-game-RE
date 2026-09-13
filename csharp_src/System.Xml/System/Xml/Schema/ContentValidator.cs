using System.Collections;

namespace System.Xml.Schema;

internal class ContentValidator
{
	private XmlSchemaContentType contentType;

	private bool isOpen;

	private bool isEmptiable;

	public static readonly ContentValidator Empty = new ContentValidator(XmlSchemaContentType.Empty);

	public static readonly ContentValidator TextOnly = new ContentValidator(XmlSchemaContentType.TextOnly, isOpen: false, isEmptiable: false);

	public static readonly ContentValidator Mixed = new ContentValidator(XmlSchemaContentType.Mixed);

	public static readonly ContentValidator Any = new ContentValidator(XmlSchemaContentType.Mixed, isOpen: true, isEmptiable: true);

	public XmlSchemaContentType ContentType => contentType;

	public bool PreserveWhitespace
	{
		get
		{
			if (contentType != XmlSchemaContentType.TextOnly)
			{
				return contentType == XmlSchemaContentType.Mixed;
			}
			return true;
		}
	}

	public virtual bool IsEmptiable => isEmptiable;

	public bool IsOpen
	{
		get
		{
			if (contentType == XmlSchemaContentType.TextOnly || contentType == XmlSchemaContentType.Empty)
			{
				return false;
			}
			return isOpen;
		}
		set
		{
			isOpen = value;
		}
	}

	public ContentValidator(XmlSchemaContentType contentType)
	{
		this.contentType = contentType;
		isEmptiable = true;
	}

	protected ContentValidator(XmlSchemaContentType contentType, bool isOpen, bool isEmptiable)
	{
		this.contentType = contentType;
		this.isOpen = isOpen;
		this.isEmptiable = isEmptiable;
	}

	public virtual void InitValidation(ValidationState context)
	{
	}

	public virtual object ValidateElement(XmlQualifiedName name, ValidationState context, out int errorCode)
	{
		if (contentType == XmlSchemaContentType.TextOnly || contentType == XmlSchemaContentType.Empty)
		{
			context.NeedValidateChildren = false;
		}
		errorCode = -1;
		return null;
	}

	public virtual bool CompleteValidation(ValidationState context)
	{
		return true;
	}

	public virtual ArrayList ExpectedElements(ValidationState context, bool isRequiredOnly)
	{
		return null;
	}

	public virtual ArrayList ExpectedParticles(ValidationState context, bool isRequiredOnly, XmlSchemaSet schemaSet)
	{
		return null;
	}

	public static void AddParticleToExpected(XmlSchemaParticle p, XmlSchemaSet schemaSet, ArrayList particles)
	{
		AddParticleToExpected(p, schemaSet, particles, global: false);
	}

	public static void AddParticleToExpected(XmlSchemaParticle p, XmlSchemaSet schemaSet, ArrayList particles, bool global)
	{
		if (!particles.Contains(p))
		{
			particles.Add(p);
		}
		if (!(p is XmlSchemaElement xmlSchemaElement) || (!global && xmlSchemaElement.RefName.IsEmpty))
		{
			return;
		}
		XmlSchemaSubstitutionGroup xmlSchemaSubstitutionGroup = (XmlSchemaSubstitutionGroup)schemaSet.SubstitutionGroups[xmlSchemaElement.QualifiedName];
		if (xmlSchemaSubstitutionGroup == null)
		{
			return;
		}
		for (int i = 0; i < xmlSchemaSubstitutionGroup.Members.Count; i++)
		{
			XmlSchemaElement xmlSchemaElement2 = (XmlSchemaElement)xmlSchemaSubstitutionGroup.Members[i];
			if (!xmlSchemaElement.QualifiedName.Equals(xmlSchemaElement2.QualifiedName) && !particles.Contains(xmlSchemaElement2))
			{
				particles.Add(xmlSchemaElement2);
			}
		}
	}
}
