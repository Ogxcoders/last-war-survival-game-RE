using Microsoft.Win32.SafeHandles;

namespace System.Net.Security;

internal sealed class SafeDeleteNegoContext : SafeDeleteContext
{
	private SafeGssNameHandle _targetName;

	private SafeGssContextHandle _context;

	private bool _isNtlmUsed;

	public SafeGssNameHandle TargetName => _targetName;

	public bool IsNtlmUsed => _isNtlmUsed;

	public SafeGssContextHandle GssContext => _context;

	public SafeDeleteNegoContext(SafeFreeNegoCredentials credential, string targetName)
		: base(credential)
	{
		try
		{
			_targetName = SafeGssNameHandle.CreatePrincipal(targetName);
		}
		catch
		{
			Dispose();
			throw;
		}
	}

	public void SetGssContext(SafeGssContextHandle context)
	{
		_context = context;
	}

	public void SetAuthenticationPackage(bool isNtlmUsed)
	{
		_isNtlmUsed = isNtlmUsed;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (_context != null)
			{
				_context.Dispose();
				_context = null;
			}
			if (_targetName != null)
			{
				_targetName.Dispose();
				_targetName = null;
			}
		}
		base.Dispose(disposing);
	}
}
