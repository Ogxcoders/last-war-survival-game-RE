namespace System.Runtime.Remoting.Messaging;

internal class CADObjRef
{
	internal ObjRef objref;

	internal int SourceDomain;

	internal byte[] TypeInfo;

	public string TypeName => objref.TypeInfo.TypeName;

	public string URI => objref.URI;

	public CADObjRef(ObjRef o, int sourceDomain)
	{
		objref = o;
		TypeInfo = o.SerializeType();
		SourceDomain = sourceDomain;
	}
}
