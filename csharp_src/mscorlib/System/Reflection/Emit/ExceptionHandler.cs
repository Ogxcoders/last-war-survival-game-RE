using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[ComVisible(false)]
public readonly struct ExceptionHandler : IEquatable<ExceptionHandler>
{
	internal readonly int m_exceptionClass;

	internal readonly int m_tryStartOffset;

	internal readonly int m_tryEndOffset;

	internal readonly int m_filterOffset;

	internal readonly int m_handlerStartOffset;

	internal readonly int m_handlerEndOffset;

	internal readonly ExceptionHandlingClauseOptions m_kind;

	public int ExceptionTypeToken => m_exceptionClass;

	public int TryOffset => m_tryStartOffset;

	public int TryLength => m_tryEndOffset - m_tryStartOffset;

	public int FilterOffset => m_filterOffset;

	public int HandlerOffset => m_handlerStartOffset;

	public int HandlerLength => m_handlerEndOffset - m_handlerStartOffset;

	public ExceptionHandlingClauseOptions Kind => m_kind;

	public ExceptionHandler(int tryOffset, int tryLength, int filterOffset, int handlerOffset, int handlerLength, ExceptionHandlingClauseOptions kind, int exceptionTypeToken)
	{
		if (tryOffset < 0)
		{
			throw new ArgumentOutOfRangeException("tryOffset", Environment.GetResourceString("Non-negative number required."));
		}
		if (tryLength < 0)
		{
			throw new ArgumentOutOfRangeException("tryLength", Environment.GetResourceString("Non-negative number required."));
		}
		if (filterOffset < 0)
		{
			throw new ArgumentOutOfRangeException("filterOffset", Environment.GetResourceString("Non-negative number required."));
		}
		if (handlerOffset < 0)
		{
			throw new ArgumentOutOfRangeException("handlerOffset", Environment.GetResourceString("Non-negative number required."));
		}
		if (handlerLength < 0)
		{
			throw new ArgumentOutOfRangeException("handlerLength", Environment.GetResourceString("Non-negative number required."));
		}
		if ((long)tryOffset + (long)tryLength > int.MaxValue)
		{
			throw new ArgumentOutOfRangeException("tryLength", Environment.GetResourceString("Valid values are between {0} and {1}, inclusive.", 0, int.MaxValue - tryOffset));
		}
		if ((long)handlerOffset + (long)handlerLength > int.MaxValue)
		{
			throw new ArgumentOutOfRangeException("handlerLength", Environment.GetResourceString("Valid values are between {0} and {1}, inclusive.", 0, int.MaxValue - handlerOffset));
		}
		if (kind == ExceptionHandlingClauseOptions.Clause && (exceptionTypeToken & 0xFFFFFF) == 0)
		{
			throw new ArgumentException(Environment.GetResourceString("Token {0:x} is not a valid Type token.", exceptionTypeToken), "exceptionTypeToken");
		}
		if (!IsValidKind(kind))
		{
			throw new ArgumentOutOfRangeException("kind", Environment.GetResourceString("Enum value was out of legal range."));
		}
		m_tryStartOffset = tryOffset;
		m_tryEndOffset = tryOffset + tryLength;
		m_filterOffset = filterOffset;
		m_handlerStartOffset = handlerOffset;
		m_handlerEndOffset = handlerOffset + handlerLength;
		m_kind = kind;
		m_exceptionClass = exceptionTypeToken;
	}

	internal ExceptionHandler(int tryStartOffset, int tryEndOffset, int filterOffset, int handlerStartOffset, int handlerEndOffset, int kind, int exceptionTypeToken)
	{
		m_tryStartOffset = tryStartOffset;
		m_tryEndOffset = tryEndOffset;
		m_filterOffset = filterOffset;
		m_handlerStartOffset = handlerStartOffset;
		m_handlerEndOffset = handlerEndOffset;
		m_kind = (ExceptionHandlingClauseOptions)kind;
		m_exceptionClass = exceptionTypeToken;
	}

	private static bool IsValidKind(ExceptionHandlingClauseOptions kind)
	{
		if ((uint)kind <= 2u || kind == ExceptionHandlingClauseOptions.Fault)
		{
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return m_exceptionClass ^ m_tryStartOffset ^ m_tryEndOffset ^ m_filterOffset ^ m_handlerStartOffset ^ m_handlerEndOffset ^ (int)m_kind;
	}

	public override bool Equals(object obj)
	{
		if (obj is ExceptionHandler)
		{
			return Equals((ExceptionHandler)obj);
		}
		return false;
	}

	public bool Equals(ExceptionHandler other)
	{
		if (other.m_exceptionClass == m_exceptionClass && other.m_tryStartOffset == m_tryStartOffset && other.m_tryEndOffset == m_tryEndOffset && other.m_filterOffset == m_filterOffset && other.m_handlerStartOffset == m_handlerStartOffset && other.m_handlerEndOffset == m_handlerEndOffset)
		{
			return other.m_kind == m_kind;
		}
		return false;
	}

	public static bool operator ==(ExceptionHandler left, ExceptionHandler right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(ExceptionHandler left, ExceptionHandler right)
	{
		return !left.Equals(right);
	}
}
