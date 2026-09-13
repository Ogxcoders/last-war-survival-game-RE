using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization;

internal class CodeTypeReference
{
	private string baseType;

	[OptionalField]
	private bool isInterface;

	private int arrayRank;

	private CodeTypeReference arrayElementType;

	[OptionalField]
	private List<CodeTypeReference> typeArguments;

	[OptionalField]
	private CodeTypeReferenceOptions referenceOptions;

	[OptionalField]
	private bool needsFixup;

	public CodeTypeReference ArrayElementType
	{
		get
		{
			return arrayElementType;
		}
		set
		{
			arrayElementType = value;
		}
	}

	public int ArrayRank
	{
		get
		{
			return arrayRank;
		}
		set
		{
			arrayRank = value;
		}
	}

	internal int NestedArrayDepth
	{
		get
		{
			if (arrayElementType == null)
			{
				return 0;
			}
			return 1 + arrayElementType.NestedArrayDepth;
		}
	}

	public string BaseType
	{
		get
		{
			if (arrayRank > 0 && arrayElementType != null)
			{
				return arrayElementType.BaseType;
			}
			if (string.IsNullOrEmpty(baseType))
			{
				return string.Empty;
			}
			string text = baseType;
			if (needsFixup && TypeArguments.Count > 0)
			{
				text = text + "`" + TypeArguments.Count.ToString(CultureInfo.InvariantCulture);
			}
			return text;
		}
		set
		{
			baseType = value;
			Initialize(baseType);
		}
	}

	[ComVisible(false)]
	public CodeTypeReferenceOptions Options
	{
		get
		{
			return referenceOptions;
		}
		set
		{
			referenceOptions = value;
		}
	}

	[ComVisible(false)]
	public List<CodeTypeReference> TypeArguments
	{
		get
		{
			if (arrayRank > 0 && arrayElementType != null)
			{
				return arrayElementType.TypeArguments;
			}
			if (typeArguments == null)
			{
				typeArguments = new List<CodeTypeReference>();
			}
			return typeArguments;
		}
	}

	internal bool IsInterface => isInterface;

	public CodeTypeReference()
	{
		baseType = string.Empty;
		arrayRank = 0;
		arrayElementType = null;
	}

	public CodeTypeReference(Type type)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		if (type.IsArray)
		{
			arrayRank = type.GetArrayRank();
			arrayElementType = new CodeTypeReference(type.GetElementType());
			baseType = null;
		}
		else
		{
			InitializeFromType(type);
			arrayRank = 0;
			arrayElementType = null;
		}
		isInterface = type.IsInterface;
	}

	public CodeTypeReference(Type type, CodeTypeReferenceOptions codeTypeReferenceOption)
		: this(type)
	{
		referenceOptions = codeTypeReferenceOption;
	}

	public CodeTypeReference(string typeName, CodeTypeReferenceOptions codeTypeReferenceOption)
	{
		Initialize(typeName, codeTypeReferenceOption);
	}

	public CodeTypeReference(string typeName)
	{
		Initialize(typeName);
	}

	private void InitializeFromType(Type type)
	{
		baseType = type.Name;
		if (!type.IsGenericParameter)
		{
			Type type2 = type;
			while (type2.IsNested)
			{
				type2 = type2.DeclaringType;
				baseType = type2.Name + "+" + baseType;
			}
			if (!string.IsNullOrEmpty(type.Namespace))
			{
				baseType = type.Namespace + "." + baseType;
			}
		}
		if (type.IsGenericType && !type.ContainsGenericParameters)
		{
			Type[] genericArguments = type.GetGenericArguments();
			for (int i = 0; i < genericArguments.Length; i++)
			{
				TypeArguments.Add(new CodeTypeReference(genericArguments[i]));
			}
		}
		else if (!type.IsGenericTypeDefinition)
		{
			needsFixup = true;
		}
	}

	private void Initialize(string typeName)
	{
		Initialize(typeName, referenceOptions);
	}

	private void Initialize(string typeName, CodeTypeReferenceOptions options)
	{
		Options = options;
		if (typeName == null || typeName.Length == 0)
		{
			typeName = typeof(void).FullName;
			baseType = typeName;
			arrayRank = 0;
			arrayElementType = null;
			return;
		}
		typeName = RipOffAssemblyInformationFromTypeName(typeName);
		int num = typeName.Length - 1;
		int num2 = num;
		needsFixup = true;
		Queue queue = new Queue();
		while (num2 >= 0)
		{
			int num3 = 1;
			if (typeName[num2--] != ']')
			{
				break;
			}
			while (num2 >= 0 && typeName[num2] == ',')
			{
				num3++;
				num2--;
			}
			if (num2 < 0 || typeName[num2] != '[')
			{
				break;
			}
			queue.Enqueue(num3);
			num2--;
			num = num2;
		}
		num2 = num;
		ArrayList arrayList = new ArrayList();
		Stack stack = new Stack();
		if (num2 > 0 && typeName[num2--] == ']')
		{
			needsFixup = false;
			int num4 = 1;
			int num5 = num;
			while (num2 >= 0)
			{
				if (typeName[num2] == '[')
				{
					if (--num4 == 0)
					{
						break;
					}
				}
				else if (typeName[num2] == ']')
				{
					num4++;
				}
				else if (typeName[num2] == ',' && num4 == 1)
				{
					if (num2 + 1 < num5)
					{
						stack.Push(typeName.Substring(num2 + 1, num5 - num2 - 1));
					}
					num5 = num2;
				}
				num2--;
			}
			if (num2 > 0 && num - num2 - 1 > 0)
			{
				if (num2 + 1 < num5)
				{
					stack.Push(typeName.Substring(num2 + 1, num5 - num2 - 1));
				}
				while (stack.Count > 0)
				{
					string typeName2 = RipOffAssemblyInformationFromTypeName((string)stack.Pop());
					arrayList.Add(new CodeTypeReference(typeName2));
				}
				num = num2 - 1;
			}
		}
		if (num < 0)
		{
			baseType = typeName;
			return;
		}
		if (queue.Count > 0)
		{
			CodeTypeReference codeTypeReference = new CodeTypeReference(typeName.Substring(0, num + 1), Options);
			for (int i = 0; i < arrayList.Count; i++)
			{
				codeTypeReference.TypeArguments.Add((CodeTypeReference)arrayList[i]);
			}
			while (queue.Count > 1)
			{
				codeTypeReference = new CodeTypeReference(codeTypeReference, (int)queue.Dequeue());
			}
			baseType = null;
			arrayRank = (int)queue.Dequeue();
			arrayElementType = codeTypeReference;
		}
		else if (arrayList.Count > 0)
		{
			for (int j = 0; j < arrayList.Count; j++)
			{
				TypeArguments.Add((CodeTypeReference)arrayList[j]);
			}
			baseType = typeName.Substring(0, num + 1);
		}
		else
		{
			baseType = typeName;
		}
		if (baseType != null && baseType.IndexOf('`') != -1)
		{
			needsFixup = false;
		}
	}

	public CodeTypeReference(string typeName, params CodeTypeReference[] typeArguments)
		: this(typeName)
	{
		if (typeArguments != null && typeArguments.Length != 0)
		{
			TypeArguments.AddRange(typeArguments);
		}
	}

	public CodeTypeReference(string baseType, int rank)
	{
		this.baseType = null;
		arrayRank = rank;
		arrayElementType = new CodeTypeReference(baseType);
	}

	public CodeTypeReference(CodeTypeReference arrayType, int rank)
	{
		baseType = null;
		arrayRank = rank;
		arrayElementType = arrayType;
	}

	private string RipOffAssemblyInformationFromTypeName(string typeName)
	{
		int i = 0;
		int num = typeName.Length - 1;
		string result = typeName;
		for (; i < typeName.Length && char.IsWhiteSpace(typeName[i]); i++)
		{
		}
		while (num >= 0 && char.IsWhiteSpace(typeName[num]))
		{
			num--;
		}
		if (i < num)
		{
			if (typeName[i] == '[' && typeName[num] == ']')
			{
				i++;
				num--;
			}
			if (typeName[num] != ']')
			{
				int num2 = 0;
				for (int num3 = num; num3 >= i; num3--)
				{
					if (typeName[num3] == ',')
					{
						num2++;
						if (num2 == 4)
						{
							result = typeName.Substring(i, num3 - i);
							break;
						}
					}
				}
			}
		}
		return result;
	}
}
