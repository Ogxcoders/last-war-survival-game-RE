using System.Collections.Generic;
using System.Globalization;
using Microsoft.Internal;

namespace System.ComponentModel.Composition;

[Serializable]
public class ChangeRejectedException : CompositionException
{
	public override string Message => string.Format(CultureInfo.CurrentCulture, Strings.CompositionException_ChangesRejected, base.Message);

	public ChangeRejectedException()
		: this(null, null)
	{
	}

	public ChangeRejectedException(string message)
		: this(message, null)
	{
	}

	public ChangeRejectedException(string message, Exception innerException)
		: base(message, innerException, null)
	{
	}

	public ChangeRejectedException(IEnumerable<CompositionError> errors)
		: base(null, null, errors)
	{
	}
}
