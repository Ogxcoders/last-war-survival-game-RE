using System.Text;

namespace System.Data.Common;

internal class MultipartIdentifier
{
	private enum MPIState
	{
		MPI_Value,
		MPI_ParseNonQuote,
		MPI_LookForSeparator,
		MPI_LookForNextCharOrSeparator,
		MPI_ParseQuote,
		MPI_RightQuote
	}

	private const int MaxParts = 4;

	internal const int ServerIndex = 0;

	internal const int CatalogIndex = 1;

	internal const int SchemaIndex = 2;

	internal const int TableIndex = 3;

	internal static string[] ParseMultipartIdentifier(string name, string leftQuote, string rightQuote, string property, bool ThrowOnEmptyMultipartName)
	{
		return ParseMultipartIdentifier(name, leftQuote, rightQuote, '.', 4, removequotes: true, property, ThrowOnEmptyMultipartName);
	}

	private static void IncrementStringCount(string name, string[] ary, ref int position, string property)
	{
		position++;
		int num = ary.Length;
		if (position >= num)
		{
			throw ADP.InvalidMultipartNameToManyParts(property, name, num);
		}
		ary[position] = string.Empty;
	}

	private static bool IsWhitespace(char ch)
	{
		return char.IsWhiteSpace(ch);
	}

	internal static string[] ParseMultipartIdentifier(string name, string leftQuote, string rightQuote, char separator, int limit, bool removequotes, string property, bool ThrowOnEmptyMultipartName)
	{
		if (limit <= 0)
		{
			throw ADP.InvalidMultipartNameToManyParts(property, name, limit);
		}
		if (-1 != leftQuote.IndexOf(separator) || -1 != rightQuote.IndexOf(separator) || leftQuote.Length != rightQuote.Length)
		{
			throw ADP.InvalidMultipartNameIncorrectUsageOfQuotes(property, name);
		}
		string[] array = new string[limit];
		int position = 0;
		MPIState mPIState = MPIState.MPI_Value;
		StringBuilder stringBuilder = new StringBuilder(name.Length);
		StringBuilder stringBuilder2 = null;
		char c = ' ';
		foreach (char c2 in name)
		{
			switch (mPIState)
			{
			case MPIState.MPI_Value:
			{
				if (IsWhitespace(c2))
				{
					break;
				}
				int index;
				if (c2 == separator)
				{
					array[position] = string.Empty;
					IncrementStringCount(name, array, ref position, property);
				}
				else if (-1 != (index = leftQuote.IndexOf(c2)))
				{
					c = rightQuote[index];
					stringBuilder.Length = 0;
					if (!removequotes)
					{
						stringBuilder.Append(c2);
					}
					mPIState = MPIState.MPI_ParseQuote;
				}
				else
				{
					if (-1 != rightQuote.IndexOf(c2))
					{
						throw ADP.InvalidMultipartNameIncorrectUsageOfQuotes(property, name);
					}
					stringBuilder.Length = 0;
					stringBuilder.Append(c2);
					mPIState = MPIState.MPI_ParseNonQuote;
				}
				break;
			}
			case MPIState.MPI_ParseNonQuote:
				if (c2 == separator)
				{
					array[position] = stringBuilder.ToString();
					IncrementStringCount(name, array, ref position, property);
					mPIState = MPIState.MPI_Value;
					break;
				}
				if (-1 != rightQuote.IndexOf(c2))
				{
					throw ADP.InvalidMultipartNameIncorrectUsageOfQuotes(property, name);
				}
				if (-1 != leftQuote.IndexOf(c2))
				{
					throw ADP.InvalidMultipartNameIncorrectUsageOfQuotes(property, name);
				}
				if (IsWhitespace(c2))
				{
					array[position] = stringBuilder.ToString();
					if (stringBuilder2 == null)
					{
						stringBuilder2 = new StringBuilder();
					}
					stringBuilder2.Length = 0;
					stringBuilder2.Append(c2);
					mPIState = MPIState.MPI_LookForNextCharOrSeparator;
				}
				else
				{
					stringBuilder.Append(c2);
				}
				break;
			case MPIState.MPI_LookForNextCharOrSeparator:
				if (!IsWhitespace(c2))
				{
					if (c2 == separator)
					{
						IncrementStringCount(name, array, ref position, property);
						mPIState = MPIState.MPI_Value;
						break;
					}
					stringBuilder.Append(stringBuilder2);
					stringBuilder.Append(c2);
					array[position] = stringBuilder.ToString();
					mPIState = MPIState.MPI_ParseNonQuote;
				}
				else
				{
					stringBuilder2.Append(c2);
				}
				break;
			case MPIState.MPI_ParseQuote:
				if (c2 == c)
				{
					if (!removequotes)
					{
						stringBuilder.Append(c2);
					}
					mPIState = MPIState.MPI_RightQuote;
				}
				else
				{
					stringBuilder.Append(c2);
				}
				break;
			case MPIState.MPI_RightQuote:
				if (c2 == c)
				{
					stringBuilder.Append(c2);
					mPIState = MPIState.MPI_ParseQuote;
					break;
				}
				if (c2 == separator)
				{
					array[position] = stringBuilder.ToString();
					IncrementStringCount(name, array, ref position, property);
					mPIState = MPIState.MPI_Value;
					break;
				}
				if (!IsWhitespace(c2))
				{
					throw ADP.InvalidMultipartNameIncorrectUsageOfQuotes(property, name);
				}
				array[position] = stringBuilder.ToString();
				mPIState = MPIState.MPI_LookForSeparator;
				break;
			case MPIState.MPI_LookForSeparator:
				if (!IsWhitespace(c2))
				{
					if (c2 != separator)
					{
						throw ADP.InvalidMultipartNameIncorrectUsageOfQuotes(property, name);
					}
					IncrementStringCount(name, array, ref position, property);
					mPIState = MPIState.MPI_Value;
				}
				break;
			}
		}
		switch (mPIState)
		{
		case MPIState.MPI_ParseNonQuote:
		case MPIState.MPI_RightQuote:
			array[position] = stringBuilder.ToString();
			break;
		default:
			throw ADP.InvalidMultipartNameIncorrectUsageOfQuotes(property, name);
		case MPIState.MPI_Value:
		case MPIState.MPI_LookForSeparator:
		case MPIState.MPI_LookForNextCharOrSeparator:
			break;
		}
		if (array[0] == null)
		{
			if (ThrowOnEmptyMultipartName)
			{
				throw ADP.InvalidMultipartName(property, name);
			}
		}
		else
		{
			int num = limit - position - 1;
			if (num > 0)
			{
				for (int num2 = limit - 1; num2 >= num; num2--)
				{
					array[num2] = array[num2 - num];
					array[num2 - num] = null;
				}
			}
		}
		return array;
	}
}
