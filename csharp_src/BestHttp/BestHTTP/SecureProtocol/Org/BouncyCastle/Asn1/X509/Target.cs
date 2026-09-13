using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

public class Target : Asn1Encodable, IAsn1Choice
{
	public enum Choice
	{
		Name,
		Group
	}

	private readonly GeneralName targetName;

	private readonly GeneralName targetGroup;

	public virtual GeneralName TargetGroup => targetGroup;

	public virtual GeneralName TargetName => targetName;

	public static Target GetInstance(object obj)
	{
		if (obj is Target)
		{
			return (Target)obj;
		}
		if (obj is Asn1TaggedObject)
		{
			return new Target((Asn1TaggedObject)obj);
		}
		throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
	}

	private Target(Asn1TaggedObject tagObj)
	{
		switch ((Choice)tagObj.TagNo)
		{
		case Choice.Name:
			targetName = GeneralName.GetInstance(tagObj, explicitly: true);
			break;
		case Choice.Group:
			targetGroup = GeneralName.GetInstance(tagObj, explicitly: true);
			break;
		default:
			throw new ArgumentException("unknown tag: " + tagObj.TagNo);
		}
	}

	public Target(Choice type, GeneralName name)
		: this(new DerTaggedObject((int)type, name))
	{
	}

	public override Asn1Object ToAsn1Object()
	{
		if (targetName != null)
		{
			return new DerTaggedObject(explicitly: true, 0, targetName);
		}
		return new DerTaggedObject(explicitly: true, 1, targetGroup);
	}
}
