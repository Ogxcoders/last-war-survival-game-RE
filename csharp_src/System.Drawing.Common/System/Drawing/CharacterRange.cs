namespace System.Drawing;

public struct CharacterRange
{
	private int _dummy;

	public int First
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int Length
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public CharacterRange(int First, int Length)
	{
		throw new PlatformNotSupportedException();
	}

	public override bool Equals(object obj)
	{
		throw new PlatformNotSupportedException();
	}

	public override int GetHashCode()
	{
		throw new PlatformNotSupportedException();
	}

	public static bool operator ==(CharacterRange cr1, CharacterRange cr2)
	{
		throw new PlatformNotSupportedException();
	}

	public static bool operator !=(CharacterRange cr1, CharacterRange cr2)
	{
		throw new PlatformNotSupportedException();
	}
}
