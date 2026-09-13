using System.Text;

namespace System.Xml.Schema;

internal class KeySequence
{
	private TypedObject[] ks;

	private int dim;

	private int hashcode = -1;

	private int posline;

	private int poscol;

	public int PosLine => posline;

	public int PosCol => poscol;

	public object this[int index]
	{
		get
		{
			return ks[index];
		}
		set
		{
			ks[index] = (TypedObject)value;
		}
	}

	internal KeySequence(int dim, int line, int col)
	{
		this.dim = dim;
		ks = new TypedObject[dim];
		posline = line;
		poscol = col;
	}

	public KeySequence(TypedObject[] ks)
	{
		this.ks = ks;
		dim = ks.Length;
		posline = (poscol = 0);
	}

	internal bool IsQualified()
	{
		for (int i = 0; i < ks.Length; i++)
		{
			if (ks[i] == null || ks[i].Value == null)
			{
				return false;
			}
		}
		return true;
	}

	public override int GetHashCode()
	{
		if (hashcode != -1)
		{
			return hashcode;
		}
		hashcode = 0;
		for (int i = 0; i < ks.Length; i++)
		{
			ks[i].SetDecimal();
			if (ks[i].IsDecimal)
			{
				for (int j = 0; j < ks[i].Dim; j++)
				{
					hashcode += ks[i].Dvalue[j].GetHashCode();
				}
			}
			else if (ks[i].Value is Array array)
			{
				if (array is XmlAtomicValue[] array2)
				{
					for (int k = 0; k < array2.Length; k++)
					{
						hashcode += ((XmlAtomicValue)array2.GetValue(k)).TypedValue.GetHashCode();
					}
				}
				else
				{
					for (int l = 0; l < ((Array)ks[i].Value).Length; l++)
					{
						hashcode += ((Array)ks[i].Value).GetValue(l).GetHashCode();
					}
				}
			}
			else
			{
				hashcode += ks[i].Value.GetHashCode();
			}
		}
		return hashcode;
	}

	public override bool Equals(object other)
	{
		KeySequence keySequence = (KeySequence)other;
		for (int i = 0; i < ks.Length; i++)
		{
			if (!ks[i].Equals(keySequence.ks[i]))
			{
				return false;
			}
		}
		return true;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(ks[0].ToString());
		for (int i = 1; i < ks.Length; i++)
		{
			stringBuilder.Append(" ");
			stringBuilder.Append(ks[i].ToString());
		}
		return stringBuilder.ToString();
	}
}
