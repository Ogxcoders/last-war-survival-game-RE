using System.Collections;
using System.IO;
using System.Linq;
using Google.Protobuf.Reflection;

namespace Google.Protobuf;

public static class MessageExtensions
{
	public static void MergeFrom(this IMessage message, byte[] data)
	{
		message.MergeFrom(data, discardUnknownFields: false, null);
	}

	public static void MergeFrom(this IMessage message, byte[] data, int offset, int length)
	{
		message.MergeFrom(data, offset, length, discardUnknownFields: false, null);
	}

	public static void MergeFrom(this IMessage message, ByteString data)
	{
		message.MergeFrom(data, discardUnknownFields: false, null);
	}

	public static void MergeFrom(this IMessage message, Stream input)
	{
		message.MergeFrom(input, discardUnknownFields: false, null);
	}

	public static void MergeDelimitedFrom(this IMessage message, Stream input)
	{
		message.MergeDelimitedFrom(input, discardUnknownFields: false, null);
	}

	public static byte[] ToByteArray(this IMessage message)
	{
		ProtoPreconditions.CheckNotNull(message, "message");
		byte[] array = new byte[message.CalculateSize()];
		CodedOutputStream codedOutputStream = new CodedOutputStream(array);
		message.WriteTo(codedOutputStream);
		codedOutputStream.CheckNoSpaceLeft();
		return array;
	}

	public static void WriteTo(this IMessage message, Stream output)
	{
		ProtoPreconditions.CheckNotNull(message, "message");
		ProtoPreconditions.CheckNotNull(output, "output");
		CodedOutputStream codedOutputStream = new CodedOutputStream(output);
		message.WriteTo(codedOutputStream);
		codedOutputStream.Flush();
	}

	public static void WriteDelimitedTo(this IMessage message, Stream output)
	{
		ProtoPreconditions.CheckNotNull(message, "message");
		ProtoPreconditions.CheckNotNull(output, "output");
		CodedOutputStream codedOutputStream = new CodedOutputStream(output);
		codedOutputStream.WriteRawVarint32((uint)message.CalculateSize());
		message.WriteTo(codedOutputStream);
		codedOutputStream.Flush();
	}

	public static ByteString ToByteString(this IMessage message)
	{
		ProtoPreconditions.CheckNotNull(message, "message");
		return ByteString.AttachBytes(message.ToByteArray());
	}

	public static bool IsInitialized(this IMessage message)
	{
		if (message.Descriptor.File.Syntax == Syntax.Proto3)
		{
			return true;
		}
		if (!message.Descriptor.IsExtensionsInitialized(message))
		{
			return false;
		}
		return message.Descriptor.Fields.InDeclarationOrder().All(delegate(FieldDescriptor f)
		{
			if (f.IsMap)
			{
				if (f.MessageType.Fields[2].FieldType == FieldType.Message)
				{
					return ((IDictionary)f.Accessor.GetValue(message)).Values.Cast<IMessage>().All(IsInitialized);
				}
				return true;
			}
			if ((f.IsRepeated && f.FieldType == FieldType.Message) || f.FieldType == FieldType.Group)
			{
				return ((IEnumerable)f.Accessor.GetValue(message)).Cast<IMessage>().All(IsInitialized);
			}
			if (f.FieldType == FieldType.Message || f.FieldType == FieldType.Group)
			{
				if (f.Accessor.HasValue(message))
				{
					return ((IMessage)f.Accessor.GetValue(message)).IsInitialized();
				}
				return !f.IsRequired;
			}
			return !f.IsRequired || f.Accessor.HasValue(message);
		});
	}

	internal static void MergeFrom(this IMessage message, byte[] data, bool discardUnknownFields, ExtensionRegistry registry)
	{
		ProtoPreconditions.CheckNotNull(message, "message");
		ProtoPreconditions.CheckNotNull(data, "data");
		CodedInputStream codedInputStream = new CodedInputStream(data);
		codedInputStream.DiscardUnknownFields = discardUnknownFields;
		codedInputStream.ExtensionRegistry = registry;
		message.MergeFrom(codedInputStream);
		codedInputStream.CheckReadEndOfStreamTag();
	}

	internal static void MergeFrom(this IMessage message, byte[] data, int offset, int length, bool discardUnknownFields, ExtensionRegistry registry)
	{
		ProtoPreconditions.CheckNotNull(message, "message");
		ProtoPreconditions.CheckNotNull(data, "data");
		CodedInputStream codedInputStream = new CodedInputStream(data, offset, length);
		codedInputStream.DiscardUnknownFields = discardUnknownFields;
		codedInputStream.ExtensionRegistry = registry;
		message.MergeFrom(codedInputStream);
		codedInputStream.CheckReadEndOfStreamTag();
	}

	internal static void MergeFrom(this IMessage message, ByteString data, bool discardUnknownFields, ExtensionRegistry registry)
	{
		ProtoPreconditions.CheckNotNull(message, "message");
		ProtoPreconditions.CheckNotNull(data, "data");
		CodedInputStream codedInputStream = data.CreateCodedInput();
		codedInputStream.DiscardUnknownFields = discardUnknownFields;
		codedInputStream.ExtensionRegistry = registry;
		message.MergeFrom(codedInputStream);
		codedInputStream.CheckReadEndOfStreamTag();
	}

	internal static void MergeFrom(this IMessage message, Stream input, bool discardUnknownFields, ExtensionRegistry registry)
	{
		ProtoPreconditions.CheckNotNull(message, "message");
		ProtoPreconditions.CheckNotNull(input, "input");
		CodedInputStream codedInputStream = new CodedInputStream(input);
		codedInputStream.DiscardUnknownFields = discardUnknownFields;
		codedInputStream.ExtensionRegistry = registry;
		message.MergeFrom(codedInputStream);
		codedInputStream.CheckReadEndOfStreamTag();
	}

	internal static void MergeDelimitedFrom(this IMessage message, Stream input, bool discardUnknownFields, ExtensionRegistry registry)
	{
		ProtoPreconditions.CheckNotNull(message, "message");
		ProtoPreconditions.CheckNotNull(input, "input");
		int size = (int)CodedInputStream.ReadRawVarint32(input);
		Stream input2 = new LimitedInputStream(input, size);
		message.MergeFrom(input2, discardUnknownFields, registry);
	}
}
