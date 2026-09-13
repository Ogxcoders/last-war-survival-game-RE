using System.Collections;
using System.Collections.Specialized;

namespace System.ComponentModel.Design;

public class MenuCommand
{
	private EventHandler _execHandler;

	private int _status;

	private IDictionary _properties;

	private const int ENABLED = 2;

	private const int INVISIBLE = 16;

	private const int CHECKED = 4;

	private const int SUPPORTED = 1;

	public virtual bool Checked
	{
		get
		{
			return (_status & 4) != 0;
		}
		set
		{
			SetStatus(4, value);
		}
	}

	public virtual bool Enabled
	{
		get
		{
			return (_status & 2) != 0;
		}
		set
		{
			SetStatus(2, value);
		}
	}

	public virtual IDictionary Properties => _properties ?? (_properties = new HybridDictionary());

	public virtual bool Supported
	{
		get
		{
			return (_status & 1) != 0;
		}
		set
		{
			SetStatus(1, value);
		}
	}

	public virtual bool Visible
	{
		get
		{
			return (_status & 0x10) == 0;
		}
		set
		{
			SetStatus(16, !value);
		}
	}

	public virtual CommandID CommandID { get; }

	public virtual int OleStatus => _status;

	public event EventHandler CommandChanged;

	public MenuCommand(EventHandler handler, CommandID command)
	{
		_execHandler = handler;
		CommandID = command;
		_status = 3;
	}

	private void SetStatus(int mask, bool value)
	{
		int status = _status;
		status = ((!value) ? (status & ~mask) : (status | mask));
		if (status != _status)
		{
			_status = status;
			OnCommandChanged(EventArgs.Empty);
		}
	}

	public virtual void Invoke()
	{
		if (_execHandler == null)
		{
			return;
		}
		try
		{
			_execHandler(this, EventArgs.Empty);
		}
		catch (CheckoutException ex)
		{
			if (ex == CheckoutException.Canceled)
			{
				return;
			}
			throw;
		}
	}

	public virtual void Invoke(object arg)
	{
		Invoke();
	}

	protected virtual void OnCommandChanged(EventArgs e)
	{
		this.CommandChanged?.Invoke(this, e);
	}

	public override string ToString()
	{
		string text = CommandID.ToString() + " : ";
		if ((_status & 1) != 0)
		{
			text += "Supported";
		}
		if ((_status & 2) != 0)
		{
			text += "|Enabled";
		}
		if ((_status & 0x10) == 0)
		{
			text += "|Visible";
		}
		if ((_status & 4) != 0)
		{
			text += "|Checked";
		}
		return text;
	}
}
