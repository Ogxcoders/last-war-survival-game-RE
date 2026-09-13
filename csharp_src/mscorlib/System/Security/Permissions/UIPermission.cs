using System.Runtime.InteropServices;

namespace System.Security.Permissions;

[Serializable]
[ComVisible(true)]
public sealed class UIPermission : CodeAccessPermission, IUnrestrictedPermission, IBuiltInPermission
{
	private UIPermissionWindow _window;

	private UIPermissionClipboard _clipboard;

	private const int version = 1;

	public UIPermissionClipboard Clipboard
	{
		get
		{
			return _clipboard;
		}
		set
		{
			if (!Enum.IsDefined(typeof(UIPermissionClipboard), value))
			{
				throw new ArgumentException(string.Format(Locale.GetText("Invalid enum {0}"), value), "UIPermissionClipboard");
			}
			_clipboard = value;
		}
	}

	public UIPermissionWindow Window
	{
		get
		{
			return _window;
		}
		set
		{
			if (!Enum.IsDefined(typeof(UIPermissionWindow), value))
			{
				throw new ArgumentException(string.Format(Locale.GetText("Invalid enum {0}"), value), "UIPermissionWindow");
			}
			_window = value;
		}
	}

	public UIPermission(PermissionState state)
	{
		if (CodeAccessPermission.CheckPermissionState(state, allowUnrestricted: true) == PermissionState.Unrestricted)
		{
			_clipboard = UIPermissionClipboard.AllClipboard;
			_window = UIPermissionWindow.AllWindows;
		}
	}

	public UIPermission(UIPermissionClipboard clipboardFlag)
	{
		Clipboard = clipboardFlag;
	}

	public UIPermission(UIPermissionWindow windowFlag)
	{
		Window = windowFlag;
	}

	public UIPermission(UIPermissionWindow windowFlag, UIPermissionClipboard clipboardFlag)
	{
		Clipboard = clipboardFlag;
		Window = windowFlag;
	}

	public override IPermission Copy()
	{
		return new UIPermission(_window, _clipboard);
	}

	public override void FromXml(SecurityElement esd)
	{
		CodeAccessPermission.CheckSecurityElement(esd, "esd", 1, 1);
		if (CodeAccessPermission.IsUnrestricted(esd))
		{
			_window = UIPermissionWindow.AllWindows;
			_clipboard = UIPermissionClipboard.AllClipboard;
			return;
		}
		string text = esd.Attribute("Window");
		if (text == null)
		{
			_window = UIPermissionWindow.NoWindows;
		}
		else
		{
			_window = (UIPermissionWindow)Enum.Parse(typeof(UIPermissionWindow), text);
		}
		string text2 = esd.Attribute("Clipboard");
		if (text2 == null)
		{
			_clipboard = UIPermissionClipboard.NoClipboard;
		}
		else
		{
			_clipboard = (UIPermissionClipboard)Enum.Parse(typeof(UIPermissionClipboard), text2);
		}
	}

	public override IPermission Intersect(IPermission target)
	{
		UIPermission uIPermission = Cast(target);
		if (uIPermission == null)
		{
			return null;
		}
		UIPermissionWindow uIPermissionWindow = ((_window < uIPermission._window) ? _window : uIPermission._window);
		UIPermissionClipboard uIPermissionClipboard = ((_clipboard < uIPermission._clipboard) ? _clipboard : uIPermission._clipboard);
		if (IsEmpty(uIPermissionWindow, uIPermissionClipboard))
		{
			return null;
		}
		return new UIPermission(uIPermissionWindow, uIPermissionClipboard);
	}

	public override bool IsSubsetOf(IPermission target)
	{
		UIPermission uIPermission = Cast(target);
		if (uIPermission == null)
		{
			return IsEmpty(_window, _clipboard);
		}
		if (uIPermission.IsUnrestricted())
		{
			return true;
		}
		if (_window <= uIPermission._window)
		{
			return _clipboard <= uIPermission._clipboard;
		}
		return false;
	}

	public bool IsUnrestricted()
	{
		if (_window == UIPermissionWindow.AllWindows)
		{
			return _clipboard == UIPermissionClipboard.AllClipboard;
		}
		return false;
	}

	public override SecurityElement ToXml()
	{
		SecurityElement securityElement = Element(1);
		if (_window == UIPermissionWindow.AllWindows && _clipboard == UIPermissionClipboard.AllClipboard)
		{
			securityElement.AddAttribute("Unrestricted", "true");
		}
		else
		{
			if (_window != UIPermissionWindow.NoWindows)
			{
				securityElement.AddAttribute("Window", _window.ToString());
			}
			if (_clipboard != UIPermissionClipboard.NoClipboard)
			{
				securityElement.AddAttribute("Clipboard", _clipboard.ToString());
			}
		}
		return securityElement;
	}

	public override IPermission Union(IPermission target)
	{
		UIPermission uIPermission = Cast(target);
		if (uIPermission == null)
		{
			return Copy();
		}
		UIPermissionWindow uIPermissionWindow = ((_window > uIPermission._window) ? _window : uIPermission._window);
		UIPermissionClipboard uIPermissionClipboard = ((_clipboard > uIPermission._clipboard) ? _clipboard : uIPermission._clipboard);
		if (IsEmpty(uIPermissionWindow, uIPermissionClipboard))
		{
			return null;
		}
		return new UIPermission(uIPermissionWindow, uIPermissionClipboard);
	}

	int IBuiltInPermission.GetTokenIndex()
	{
		return 7;
	}

	private bool IsEmpty(UIPermissionWindow w, UIPermissionClipboard c)
	{
		if (w == UIPermissionWindow.NoWindows)
		{
			return c == UIPermissionClipboard.NoClipboard;
		}
		return false;
	}

	private UIPermission Cast(IPermission target)
	{
		if (target == null)
		{
			return null;
		}
		UIPermission obj = target as UIPermission;
		if (obj == null)
		{
			CodeAccessPermission.ThrowInvalidPermission(target, typeof(UIPermission));
		}
		return obj;
	}
}
