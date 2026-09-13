namespace Joker;

public class MessageAttribute : ClassAttribute
{
	public int Opcode { get; }

	public MessageAttribute(int opcode = 0)
	{
		Opcode = opcode;
	}
}
