using System.Collections.Generic;
using System.Security;
using System.Threading;
using System.Xml;

namespace System.Runtime.Serialization.Json;

internal class JsonClassDataContract : JsonDataContract
{
	private class JsonClassDataContractCriticalHelper : JsonDataContractCriticalHelper
	{
		private JsonFormatClassReaderDelegate jsonFormatReaderDelegate;

		private JsonFormatClassWriterDelegate jsonFormatWriterDelegate;

		private XmlDictionaryString[] memberNames;

		private ClassDataContract traditionalClassDataContract;

		private string typeName;

		internal JsonFormatClassReaderDelegate JsonFormatReaderDelegate
		{
			get
			{
				return jsonFormatReaderDelegate;
			}
			set
			{
				jsonFormatReaderDelegate = value;
			}
		}

		internal JsonFormatClassWriterDelegate JsonFormatWriterDelegate
		{
			get
			{
				return jsonFormatWriterDelegate;
			}
			set
			{
				jsonFormatWriterDelegate = value;
			}
		}

		internal XmlDictionaryString[] MemberNames => memberNames;

		internal ClassDataContract TraditionalClassDataContract => traditionalClassDataContract;

		public JsonClassDataContractCriticalHelper(ClassDataContract traditionalDataContract)
			: base(traditionalDataContract)
		{
			typeName = (string.IsNullOrEmpty(traditionalDataContract.Namespace.Value) ? traditionalDataContract.Name.Value : (traditionalDataContract.Name.Value + ":" + XmlObjectSerializerWriteContextComplexJson.TruncateDefaultDataContractNamespace(traditionalDataContract.Namespace.Value)));
			traditionalClassDataContract = traditionalDataContract;
			CopyMembersAndCheckDuplicateNames();
		}

		private void CopyMembersAndCheckDuplicateNames()
		{
			if (traditionalClassDataContract.MemberNames == null)
			{
				return;
			}
			int num = traditionalClassDataContract.MemberNames.Length;
			Dictionary<string, object> dictionary = new Dictionary<string, object>(num);
			XmlDictionaryString[] array = new XmlDictionaryString[num];
			for (int i = 0; i < num; i++)
			{
				if (dictionary.ContainsKey(traditionalClassDataContract.MemberNames[i].Value))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("Duplicate member, including '{1}', is found in JSON input, in type '{0}'.", DataContract.GetClrTypeFullName(traditionalClassDataContract.UnderlyingType), traditionalClassDataContract.MemberNames[i].Value)));
				}
				dictionary.Add(traditionalClassDataContract.MemberNames[i].Value, null);
				array[i] = DataContractJsonSerializer.ConvertXmlNameToJsonName(traditionalClassDataContract.MemberNames[i]);
			}
			memberNames = array;
		}
	}

	[SecurityCritical]
	private JsonClassDataContractCriticalHelper helper;

	internal JsonFormatClassReaderDelegate JsonFormatReaderDelegate
	{
		[SecuritySafeCritical]
		get
		{
			if (helper.JsonFormatReaderDelegate == null)
			{
				lock (this)
				{
					if (helper.JsonFormatReaderDelegate == null)
					{
						if (TraditionalClassDataContract.IsReadOnlyContract)
						{
							DataContract.ThrowInvalidDataContractException(TraditionalClassDataContract.DeserializationExceptionMessage, null);
						}
						JsonFormatClassReaderDelegate jsonFormatReaderDelegate = new JsonFormatReaderGenerator().GenerateClassReader(TraditionalClassDataContract);
						Thread.MemoryBarrier();
						helper.JsonFormatReaderDelegate = jsonFormatReaderDelegate;
					}
				}
			}
			return helper.JsonFormatReaderDelegate;
		}
	}

	internal JsonFormatClassWriterDelegate JsonFormatWriterDelegate
	{
		[SecuritySafeCritical]
		get
		{
			if (helper.JsonFormatWriterDelegate == null)
			{
				lock (this)
				{
					if (helper.JsonFormatWriterDelegate == null)
					{
						JsonFormatClassWriterDelegate jsonFormatWriterDelegate = new JsonFormatWriterGenerator().GenerateClassWriter(TraditionalClassDataContract);
						Thread.MemoryBarrier();
						helper.JsonFormatWriterDelegate = jsonFormatWriterDelegate;
					}
				}
			}
			return helper.JsonFormatWriterDelegate;
		}
	}

	internal XmlDictionaryString[] MemberNames
	{
		[SecuritySafeCritical]
		get
		{
			return helper.MemberNames;
		}
	}

	internal override string TypeName
	{
		[SecuritySafeCritical]
		get
		{
			return helper.TypeName;
		}
	}

	private ClassDataContract TraditionalClassDataContract
	{
		[SecuritySafeCritical]
		get
		{
			return helper.TraditionalClassDataContract;
		}
	}

	[SecuritySafeCritical]
	public JsonClassDataContract(ClassDataContract traditionalDataContract)
		: base(new JsonClassDataContractCriticalHelper(traditionalDataContract))
	{
		helper = base.Helper as JsonClassDataContractCriticalHelper;
	}

	public override object ReadJsonValueCore(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
	{
		jsonReader.Read();
		object result = JsonFormatReaderDelegate(jsonReader, context, XmlDictionaryString.Empty, MemberNames);
		jsonReader.ReadEndElement();
		return result;
	}

	public override void WriteJsonValueCore(XmlWriterDelegator jsonWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, RuntimeTypeHandle declaredTypeHandle)
	{
		jsonWriter.WriteAttributeString(null, "type", null, "object");
		JsonFormatWriterDelegate(jsonWriter, obj, context, TraditionalClassDataContract, MemberNames);
	}
}
