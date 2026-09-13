using System;

namespace MiniGame.Core.Server;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class MessageIDAttribute : Attribute
{
	public int Value { get; }

	public MessageIDAttribute(int value)
	{
		Value = value;
	}
}
