using System.Collections;
using System.Reflection;
using System.Text;

namespace System.Web.Services.Protocols;

public sealed class LogicalMethodInfo
{
	private MethodInfo method_info;

	private MethodInfo end_method_info;

	private ParameterInfo[] parameters;

	private ParameterInfo[] out_parameters;

	private ParameterInfo[] in_parameters;

	private WebMethodAttribute attribute;

	public ParameterInfo AsyncCallbackParameter => method_info.GetParameters()[^2];

	public ParameterInfo AsyncResultParameter => end_method_info.GetParameters()[^1];

	public ParameterInfo AsyncStateParameter => method_info.GetParameters()[^1];

	public MethodInfo BeginMethodInfo
	{
		get
		{
			if (IsBeginMethod(method_info))
			{
				return method_info;
			}
			return null;
		}
	}

	public ICustomAttributeProvider CustomAttributeProvider => method_info;

	public Type DeclaringType => method_info.DeclaringType;

	public MethodInfo EndMethodInfo => end_method_info;

	public ParameterInfo[] InParameters
	{
		get
		{
			if (parameters == null)
			{
				ComputeParameters();
			}
			return in_parameters;
		}
	}

	public bool IsAsync => end_method_info != null;

	public bool IsVoid => ReturnType == typeof(void);

	public MethodInfo MethodInfo
	{
		get
		{
			if (IsBeginMethod(method_info))
			{
				return null;
			}
			return method_info;
		}
	}

	public string Name => method_info.Name;

	internal MethodInfo ActualMethodInfo => method_info;

	public ParameterInfo[] OutParameters
	{
		get
		{
			if (parameters == null)
			{
				ComputeParameters();
			}
			return out_parameters;
		}
	}

	public ParameterInfo[] Parameters
	{
		get
		{
			if (parameters == null)
			{
				ComputeParameters();
			}
			return parameters;
		}
	}

	public Type ReturnType
	{
		get
		{
			if (IsAsync)
			{
				return end_method_info.ReturnType;
			}
			return method_info.ReturnType;
		}
	}

	public ICustomAttributeProvider ReturnTypeCustomAttributeProvider => method_info.ReturnTypeCustomAttributes;

	internal bool EnableSession
	{
		get
		{
			if (method_info == null)
			{
				return false;
			}
			if (this.attribute == null)
			{
				object[] customAttributes = method_info.GetCustomAttributes(inherit: false);
				for (int i = 0; i < customAttributes.Length; i++)
				{
					Attribute attribute = (Attribute)customAttributes[i];
					if (attribute is WebMethodAttribute)
					{
						this.attribute = (WebMethodAttribute)attribute;
						break;
					}
				}
			}
			if (this.attribute == null)
			{
				return false;
			}
			return this.attribute.EnableSession;
		}
	}

	internal int CacheDuration
	{
		get
		{
			if (method_info == null)
			{
				return -1;
			}
			if (this.attribute == null)
			{
				object[] customAttributes = method_info.GetCustomAttributes(inherit: false);
				for (int i = 0; i < customAttributes.Length; i++)
				{
					Attribute attribute = (Attribute)customAttributes[i];
					if (attribute is WebMethodAttribute)
					{
						this.attribute = (WebMethodAttribute)attribute;
						break;
					}
				}
			}
			if (this.attribute == null)
			{
				return -1;
			}
			return this.attribute.CacheDuration;
		}
	}

	public LogicalMethodInfo(MethodInfo method_info)
	{
		if (method_info == null)
		{
			throw new ArgumentNullException("method_info should be non-null");
		}
		if (method_info.IsStatic)
		{
			throw new InvalidOperationException("method is static");
		}
		this.method_info = method_info;
	}

	private LogicalMethodInfo(MethodInfo method_info, MethodInfo end_method_info)
	{
		if (method_info == null)
		{
			throw new ArgumentNullException("method_info should be non-null");
		}
		if (method_info.IsStatic)
		{
			throw new InvalidOperationException("method is static");
		}
		this.method_info = method_info;
		this.end_method_info = end_method_info;
	}

	private void ComputeParameters()
	{
		ParameterInfo[] array = method_info.GetParameters();
		if (IsAsync)
		{
			parameters = new ParameterInfo[array.Length - 2];
			Array.Copy(array, 0, parameters, 0, array.Length - 2);
			in_parameters = new ParameterInfo[parameters.Length];
			parameters.CopyTo(in_parameters, 0);
			ParameterInfo[] array2 = end_method_info.GetParameters();
			out_parameters = new ParameterInfo[array2.Length - 1];
			Array.Copy(array2, 0, out_parameters, 0, out_parameters.Length);
			return;
		}
		parameters = array;
		int num = 0;
		int num2 = 0;
		ParameterInfo[] array3 = parameters;
		foreach (ParameterInfo parameterInfo in array3)
		{
			if (parameterInfo.ParameterType.IsByRef)
			{
				num++;
				if (!parameterInfo.IsOut)
				{
					num2++;
				}
			}
			else
			{
				num2++;
			}
		}
		out_parameters = new ParameterInfo[num];
		int num3 = 0;
		for (int j = 0; j < parameters.Length; j++)
		{
			if (parameters[j].ParameterType.IsByRef)
			{
				out_parameters[num3++] = parameters[j];
			}
		}
		in_parameters = new ParameterInfo[num2];
		num3 = 0;
		for (int k = 0; k < parameters.Length; k++)
		{
			if (parameters[k].ParameterType.IsByRef)
			{
				if (!parameters[k].IsOut)
				{
					in_parameters[num3++] = parameters[k];
				}
			}
			else
			{
				in_parameters[num3++] = parameters[k];
			}
		}
	}

	public IAsyncResult BeginInvoke(object target, object[] values, AsyncCallback callback, object asyncState)
	{
		int num = ((values != null) ? values.Length : 0);
		object[] array = new object[num + 2];
		if (num > 0)
		{
			values.CopyTo(array, 0);
		}
		array[num] = callback;
		array[num + 1] = asyncState;
		return (IAsyncResult)method_info.Invoke(target, array);
	}

	public static LogicalMethodInfo[] Create(MethodInfo[] method_infos)
	{
		return Create(method_infos, (LogicalMethodTypes)3);
	}

	public static LogicalMethodInfo[] Create(MethodInfo[] method_infos, LogicalMethodTypes types)
	{
		ArrayList arrayList = (((types & LogicalMethodTypes.Sync) != 0) ? new ArrayList() : null);
		ArrayList arrayList2;
		ArrayList arrayList3;
		if ((types & LogicalMethodTypes.Async) != 0)
		{
			arrayList2 = new ArrayList();
			arrayList3 = new ArrayList();
		}
		else
		{
			arrayList2 = (arrayList3 = null);
		}
		foreach (MethodInfo value in method_infos)
		{
			if (IsBeginMethod(value) && arrayList2 != null)
			{
				arrayList2.Add(value);
			}
			else if (IsEndMethod(value) && arrayList3 != null)
			{
				arrayList3.Add(value);
			}
			else
			{
				arrayList?.Add(value);
			}
		}
		int num = 0;
		int num2 = 0;
		if (arrayList2 != null)
		{
			num = (num2 = arrayList2.Count);
			if (num2 != arrayList3.Count)
			{
				throw new InvalidOperationException("Imbalance of begin/end methods");
			}
		}
		if (arrayList != null)
		{
			num2 += arrayList.Count;
		}
		LogicalMethodInfo[] array = new LogicalMethodInfo[num2];
		int num3 = 0;
		if (arrayList2 != null)
		{
			foreach (MethodInfo item in arrayList2)
			{
				string text = "End" + item.Name.Substring(5);
				int num4 = 0;
				if (num4 < num)
				{
					MethodInfo methodInfo2 = (MethodInfo)arrayList3[num4];
					if (!(methodInfo2.Name == text))
					{
						throw new InvalidOperationException("Imbalance of begin/end methods");
					}
					array[num3++] = new LogicalMethodInfo(item, methodInfo2);
				}
			}
		}
		if (arrayList != null)
		{
			foreach (MethodInfo item2 in arrayList)
			{
				array[num3++] = new LogicalMethodInfo(item2);
			}
		}
		return array;
	}

	public object[] EndInvoke(object target, IAsyncResult asyncResult)
	{
		if (parameters == null)
		{
			ComputeParameters();
		}
		object[] array = new object[out_parameters.Length + 1];
		array[^1] = asyncResult;
		object obj = end_method_info.Invoke(target, array);
		int num = ((!IsVoid) ? 1 : 0);
		object[] array2 = new object[num + out_parameters.Length];
		if (num == 1)
		{
			array2[0] = obj;
		}
		Array.Copy(array, 0, array2, num, out_parameters.Length);
		return array2;
	}

	public object GetCustomAttribute(Type type)
	{
		return Attribute.GetCustomAttribute(method_info, type, inherit: false);
	}

	public object[] GetCustomAttributes(Type type)
	{
		return method_info.GetCustomAttributes(type, inherit: false);
	}

	public object[] Invoke(object target, object[] values)
	{
		if (parameters == null)
		{
			ComputeParameters();
		}
		bool num = !IsVoid;
		object[] array = new object[(num ? 1 : 0) + out_parameters.Length];
		object obj = method_info.Invoke(target, values);
		if (num)
		{
			array[0] = obj;
		}
		int num2 = (num ? 1 : 0);
		for (int i = 0; i < parameters.Length; i++)
		{
			if (parameters[i].ParameterType.IsByRef)
			{
				array[num2++] = values[i];
			}
		}
		return array;
	}

	public static bool IsBeginMethod(MethodInfo method_info)
	{
		if (method_info == null)
		{
			throw new ArgumentNullException("method_info can not be null");
		}
		if (method_info.ReturnType != typeof(IAsyncResult))
		{
			return false;
		}
		if (method_info.Name.StartsWith("Begin"))
		{
			return true;
		}
		return false;
	}

	public static bool IsEndMethod(MethodInfo method_info)
	{
		if (method_info == null)
		{
			throw new ArgumentNullException("method_info can not be null");
		}
		ParameterInfo[] array = method_info.GetParameters();
		if (array.Length != 1)
		{
			return false;
		}
		if (array[0].ParameterType != typeof(IAsyncResult))
		{
			return false;
		}
		if (method_info.Name.StartsWith("End"))
		{
			return true;
		}
		return false;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (parameters == null)
		{
			ComputeParameters();
		}
		for (int i = 0; i < parameters.Length; i++)
		{
			stringBuilder.Append(parameters[i].ParameterType);
			if (parameters[i].ParameterType.IsByRef)
			{
				stringBuilder.Append(" ByRef");
			}
			if (i + 1 != parameters.Length)
			{
				stringBuilder.Append(", ");
			}
		}
		return $"{method_info.ReturnType} {method_info.Name} ({stringBuilder.ToString()})";
	}
}
