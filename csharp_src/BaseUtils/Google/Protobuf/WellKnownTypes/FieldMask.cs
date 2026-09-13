using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes;

public sealed class FieldMask : IMessage<FieldMask>, IMessage, IEquatable<FieldMask>, IDeepCloneable<FieldMask>, ICustomDiagnosticMessage
{
	public sealed class MergeOptions
	{
		public bool ReplaceMessageFields { get; set; }

		public bool ReplaceRepeatedFields { get; set; }

		public bool ReplacePrimitiveFields { get; set; }
	}

	private static readonly MessageParser<FieldMask> _parser = new MessageParser<FieldMask>(() => new FieldMask());

	private UnknownFieldSet _unknownFields;

	public const int PathsFieldNumber = 1;

	private static readonly FieldCodec<string> _repeated_paths_codec = FieldCodec.ForString(10u);

	private readonly RepeatedField<string> paths_ = new RepeatedField<string>();

	private const char FIELD_PATH_SEPARATOR = ',';

	private const char FIELD_SEPARATOR_REGEX = '.';

	[DebuggerNonUserCode]
	public static MessageParser<FieldMask> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => FieldMaskReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<string> Paths => paths_;

	[DebuggerNonUserCode]
	public FieldMask()
	{
	}

	[DebuggerNonUserCode]
	public FieldMask(FieldMask other)
		: this()
	{
		paths_ = other.paths_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public FieldMask Clone()
	{
		return new FieldMask(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as FieldMask);
	}

	[DebuggerNonUserCode]
	public bool Equals(FieldMask other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!paths_.Equals(other.paths_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= paths_.GetHashCode();
		if (_unknownFields != null)
		{
			num ^= _unknownFields.GetHashCode();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public override string ToString()
	{
		return JsonFormatter.ToDiagnosticString(this);
	}

	[DebuggerNonUserCode]
	public void WriteTo(CodedOutputStream output)
	{
		paths_.WriteTo(output, _repeated_paths_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += paths_.CalculateSize(_repeated_paths_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(FieldMask other)
	{
		if (other != null)
		{
			paths_.Add(other.paths_);
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				paths_.AddEntriesFrom(input, _repeated_paths_codec);
			}
		}
	}

	internal static string ToJson(IList<string> paths, bool diagnosticOnly)
	{
		string text = paths.FirstOrDefault((string p) => !IsPathValid(p));
		if (text == null)
		{
			StringWriter stringWriter = new StringWriter();
			JsonFormatter.WriteString(stringWriter, string.Join(",", paths.Select(JsonFormatter.ToJsonName)));
			return stringWriter.ToString();
		}
		if (diagnosticOnly)
		{
			StringWriter stringWriter2 = new StringWriter();
			stringWriter2.Write("{ \"@warning\": \"Invalid FieldMask\", \"paths\": ");
			JsonFormatter.Default.WriteList(stringWriter2, (IList)paths);
			stringWriter2.Write(" }");
			return stringWriter2.ToString();
		}
		throw new InvalidOperationException("Invalid field mask to be converted to JSON: " + text);
	}

	public string ToDiagnosticString()
	{
		return ToJson(Paths, diagnosticOnly: true);
	}

	public static FieldMask FromString(string value)
	{
		return FromStringEnumerable<Empty>(new List<string>(value.Split(new char[1] { ',' })));
	}

	public static FieldMask FromString<T>(string value) where T : IMessage
	{
		return FromStringEnumerable<T>(new List<string>(value.Split(new char[1] { ',' })));
	}

	public static FieldMask FromStringEnumerable<T>(IEnumerable<string> paths) where T : IMessage
	{
		FieldMask fieldMask = new FieldMask();
		foreach (string path in paths)
		{
			if (path.Length != 0)
			{
				if (typeof(T) != typeof(Empty) && !IsValid<T>(path))
				{
					throw new InvalidProtocolBufferException(path + " is not a valid path for " + typeof(T));
				}
				fieldMask.Paths.Add(path);
			}
		}
		return fieldMask;
	}

	public static FieldMask FromFieldNumbers<T>(params int[] fieldNumbers) where T : IMessage
	{
		return FromFieldNumbers<T>((IEnumerable<int>)fieldNumbers);
	}

	public static FieldMask FromFieldNumbers<T>(IEnumerable<int> fieldNumbers) where T : IMessage
	{
		MessageDescriptor descriptor = Activator.CreateInstance<T>().Descriptor;
		FieldMask fieldMask = new FieldMask();
		foreach (int fieldNumber in fieldNumbers)
		{
			FieldDescriptor fieldDescriptor = descriptor.FindFieldByNumber(fieldNumber);
			if (fieldDescriptor == null)
			{
				throw new ArgumentNullException($"{fieldNumber} is not a valid field number for {descriptor.Name}");
			}
			fieldMask.Paths.Add(fieldDescriptor.Name);
		}
		return fieldMask;
	}

	private static bool IsPathValid(string input)
	{
		for (int i = 0; i < input.Length; i++)
		{
			char c = input[i];
			if (c >= 'A' && c <= 'Z')
			{
				return false;
			}
			if (c == '_' && i < input.Length - 1)
			{
				char c2 = input[i + 1];
				if (c2 < 'a' || c2 > 'z')
				{
					return false;
				}
			}
		}
		return true;
	}

	public static bool IsValid<T>(FieldMask fieldMask) where T : IMessage
	{
		return IsValid(Activator.CreateInstance<T>().Descriptor, fieldMask);
	}

	public static bool IsValid(MessageDescriptor descriptor, FieldMask fieldMask)
	{
		foreach (string path in fieldMask.Paths)
		{
			if (!IsValid(descriptor, path))
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsValid<T>(string path) where T : IMessage
	{
		return IsValid(Activator.CreateInstance<T>().Descriptor, path);
	}

	public static bool IsValid(MessageDescriptor descriptor, string path)
	{
		string[] array = path.Split(new char[1] { '.' });
		if (array.Length == 0)
		{
			return false;
		}
		string[] array2 = array;
		foreach (string name in array2)
		{
			FieldDescriptor fieldDescriptor = descriptor?.FindFieldByName(name);
			if (fieldDescriptor == null)
			{
				return false;
			}
			descriptor = ((fieldDescriptor.IsRepeated || fieldDescriptor.FieldType != FieldType.Message) ? null : fieldDescriptor.MessageType);
		}
		return true;
	}

	public FieldMask Normalize()
	{
		return new FieldMaskTree(this).ToFieldMask();
	}

	public FieldMask Union(params FieldMask[] otherMasks)
	{
		FieldMaskTree fieldMaskTree = new FieldMaskTree(this);
		foreach (FieldMask mask in otherMasks)
		{
			fieldMaskTree.MergeFromFieldMask(mask);
		}
		return fieldMaskTree.ToFieldMask();
	}

	public FieldMask Intersection(FieldMask additionalMask)
	{
		FieldMaskTree fieldMaskTree = new FieldMaskTree(this);
		FieldMaskTree fieldMaskTree2 = new FieldMaskTree();
		foreach (string path in additionalMask.Paths)
		{
			fieldMaskTree.IntersectFieldPath(path, fieldMaskTree2);
		}
		return fieldMaskTree2.ToFieldMask();
	}

	public void Merge(IMessage source, IMessage destination, MergeOptions options)
	{
		new FieldMaskTree(this).Merge(source, destination, options);
	}

	public void Merge(IMessage source, IMessage destination)
	{
		Merge(source, destination, new MergeOptions());
	}
}
