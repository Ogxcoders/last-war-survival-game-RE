namespace System.Xml;

internal class HWStack : ICloneable
{
	private object[] stack;

	private int growthRate;

	private int used;

	private int size;

	private int limit;

	internal object this[int index]
	{
		get
		{
			if (index >= 0 && index < used)
			{
				return stack[index];
			}
			throw new IndexOutOfRangeException();
		}
		set
		{
			if (index >= 0 && index < used)
			{
				stack[index] = value;
				return;
			}
			throw new IndexOutOfRangeException();
		}
	}

	internal int Length => used;

	internal HWStack(int GrowthRate)
		: this(GrowthRate, int.MaxValue)
	{
	}

	internal HWStack(int GrowthRate, int limit)
	{
		growthRate = GrowthRate;
		used = 0;
		stack = new object[GrowthRate];
		size = GrowthRate;
		this.limit = limit;
	}

	internal object Push()
	{
		if (used == size)
		{
			if (limit <= used)
			{
				throw new XmlException("Stack overflow.", string.Empty);
			}
			object[] destinationArray = new object[size + growthRate];
			if (used > 0)
			{
				Array.Copy(stack, 0, destinationArray, 0, used);
			}
			stack = destinationArray;
			size += growthRate;
		}
		return stack[used++];
	}

	internal object Pop()
	{
		if (0 < used)
		{
			used--;
			return stack[used];
		}
		return null;
	}

	internal object Peek()
	{
		if (used <= 0)
		{
			return null;
		}
		return stack[used - 1];
	}

	internal void AddToTop(object o)
	{
		if (used > 0)
		{
			stack[used - 1] = o;
		}
	}

	private HWStack(object[] stack, int growthRate, int used, int size)
	{
		this.stack = stack;
		this.growthRate = growthRate;
		this.used = used;
		this.size = size;
	}

	public object Clone()
	{
		return new HWStack((object[])stack.Clone(), growthRate, used, size);
	}
}
