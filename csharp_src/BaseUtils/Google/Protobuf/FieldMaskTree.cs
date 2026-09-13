using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;

namespace Google.Protobuf;

internal sealed class FieldMaskTree
{
	internal sealed class Node
	{
		public Dictionary<string, Node> Children { get; } = new Dictionary<string, Node>();
	}

	private const char FIELD_PATH_SEPARATOR = '.';

	private readonly Node root = new Node();

	public FieldMaskTree()
	{
	}

	public FieldMaskTree(FieldMask mask)
	{
		MergeFromFieldMask(mask);
	}

	public override string ToString()
	{
		return ToFieldMask().ToString();
	}

	public FieldMaskTree AddFieldPath(string path)
	{
		string[] array = path.Split(new char[1] { '.' });
		if (array.Length == 0)
		{
			return this;
		}
		Node node = root;
		bool flag = false;
		string[] array2 = array;
		foreach (string key in array2)
		{
			if (!flag && node != root && node.Children.Count == 0)
			{
				return this;
			}
			if (!node.Children.TryGetValue(key, out var value))
			{
				flag = true;
				value = new Node();
				node.Children.Add(key, value);
			}
			node = value;
		}
		node.Children.Clear();
		return this;
	}

	public FieldMaskTree MergeFromFieldMask(FieldMask mask)
	{
		foreach (string path in mask.Paths)
		{
			AddFieldPath(path);
		}
		return this;
	}

	public FieldMask ToFieldMask()
	{
		FieldMask fieldMask = new FieldMask();
		if (root.Children.Count != 0)
		{
			List<string> list = new List<string>();
			GetFieldPaths(root, "", list);
			fieldMask.Paths.AddRange(list);
		}
		return fieldMask;
	}

	private void GetFieldPaths(Node node, string path, List<string> paths)
	{
		if (node.Children.Count == 0)
		{
			paths.Add(path);
			return;
		}
		foreach (KeyValuePair<string, Node> child in node.Children)
		{
			string path2 = ((path.Length == 0) ? child.Key : (path + "." + child.Key));
			GetFieldPaths(child.Value, path2, paths);
		}
	}

	public void IntersectFieldPath(string path, FieldMaskTree output)
	{
		if (root.Children.Count == 0)
		{
			return;
		}
		string[] array = path.Split(new char[1] { '.' });
		if (array.Length == 0)
		{
			return;
		}
		Node value = root;
		string[] array2 = array;
		foreach (string key in array2)
		{
			if (value != root && value.Children.Count == 0)
			{
				output.AddFieldPath(path);
				return;
			}
			if (!value.Children.TryGetValue(key, out value))
			{
				return;
			}
		}
		List<string> list = new List<string>();
		GetFieldPaths(value, path, list);
		foreach (string item in list)
		{
			output.AddFieldPath(item);
		}
	}

	public void Merge(IMessage source, IMessage destination, FieldMask.MergeOptions options)
	{
		if (source.Descriptor != destination.Descriptor)
		{
			throw new InvalidProtocolBufferException("Cannot merge messages of different types.");
		}
		if (root.Children.Count != 0)
		{
			Merge(root, "", source, destination, options);
		}
	}

	private void Merge(Node node, string path, IMessage source, IMessage destination, FieldMask.MergeOptions options)
	{
		if (source.Descriptor != destination.Descriptor)
		{
			throw new InvalidProtocolBufferException($"source ({source.Descriptor}) and destination ({destination.Descriptor}) descriptor must be equal");
		}
		MessageDescriptor descriptor = source.Descriptor;
		foreach (KeyValuePair<string, Node> child in node.Children)
		{
			FieldDescriptor fieldDescriptor = descriptor.FindFieldByName(child.Key);
			if (fieldDescriptor == null)
			{
				continue;
			}
			if (child.Value.Children.Count != 0)
			{
				if (fieldDescriptor.IsRepeated || fieldDescriptor.FieldType != FieldType.Message)
				{
					continue;
				}
				object value = fieldDescriptor.Accessor.GetValue(source);
				object obj = fieldDescriptor.Accessor.GetValue(destination);
				if (value != null || obj != null)
				{
					if (obj == null)
					{
						obj = fieldDescriptor.MessageType.Parser.CreateTemplate();
						fieldDescriptor.Accessor.SetValue(destination, obj);
					}
					string path2 = ((path.Length == 0) ? child.Key : (path + "." + child.Key));
					Merge(child.Value, path2, (IMessage)value, (IMessage)obj, options);
				}
				continue;
			}
			if (fieldDescriptor.IsRepeated)
			{
				if (options.ReplaceRepeatedFields)
				{
					fieldDescriptor.Accessor.Clear(destination);
				}
				IList obj2 = (IList)fieldDescriptor.Accessor.GetValue(source);
				IList list = (IList)fieldDescriptor.Accessor.GetValue(destination);
				foreach (object item in obj2)
				{
					list.Add(item);
				}
				continue;
			}
			object value2 = fieldDescriptor.Accessor.GetValue(source);
			if (fieldDescriptor.FieldType == FieldType.Message)
			{
				if (options.ReplaceMessageFields)
				{
					if (value2 == null)
					{
						fieldDescriptor.Accessor.Clear(destination);
					}
					else
					{
						fieldDescriptor.Accessor.SetValue(destination, value2);
					}
				}
				else if (value2 != null)
				{
					ByteString data = ((IMessage)value2).ToByteString();
					IMessage message = (IMessage)fieldDescriptor.Accessor.GetValue(destination);
					if (message != null)
					{
						message.MergeFrom(data);
					}
					else
					{
						fieldDescriptor.Accessor.SetValue(destination, fieldDescriptor.MessageType.Parser.ParseFrom(data));
					}
				}
			}
			else if (value2 != null || !options.ReplacePrimitiveFields)
			{
				fieldDescriptor.Accessor.SetValue(destination, value2);
			}
			else
			{
				fieldDescriptor.Accessor.Clear(destination);
			}
		}
	}
}
