using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Data;

[Serializable]
public class TypedDataSetGeneratorException : DataException
{
	private ArrayList errorList;

	private string KEY_ARRAYCOUNT = "KEY_ARRAYCOUNT";

	private string KEY_ARRAYVALUES = "KEY_ARRAYVALUES";

	public ArrayList ErrorList => errorList;

	protected TypedDataSetGeneratorException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		int num = (int)info.GetValue(KEY_ARRAYCOUNT, typeof(int));
		if (num > 0)
		{
			errorList = new ArrayList();
			for (int i = 0; i < num; i++)
			{
				errorList.Add(info.GetValue(KEY_ARRAYVALUES + i, typeof(string)));
			}
		}
		else
		{
			errorList = null;
		}
	}

	public TypedDataSetGeneratorException()
	{
		errorList = null;
		base.HResult = -2146232021;
	}

	public TypedDataSetGeneratorException(string message)
		: base(message)
	{
		base.HResult = -2146232021;
	}

	public TypedDataSetGeneratorException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146232021;
	}

	public TypedDataSetGeneratorException(ArrayList list)
		: this()
	{
		errorList = list;
		base.HResult = -2146232021;
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		if (errorList != null)
		{
			info.AddValue(KEY_ARRAYCOUNT, errorList.Count);
			for (int i = 0; i < errorList.Count; i++)
			{
				info.AddValue(KEY_ARRAYVALUES + i, errorList[i].ToString());
			}
		}
		else
		{
			info.AddValue(KEY_ARRAYCOUNT, 0);
		}
	}
}
