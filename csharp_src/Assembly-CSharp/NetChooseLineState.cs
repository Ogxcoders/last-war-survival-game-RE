using System.Collections.Generic;
using GameKit.Base;
using Sfs2X.Core;
using XLua;

public class NetChooseLineState : NetStateBase
{
	private List<INetProxy> m_allSelectProxy = new List<INetProxy>();

	public NetChooseLineState(TcpNetManager tcpNetManager)
		: base(NetState.CHOOSELINE, tcpNetManager)
	{
	}

	public override void OnEnter(params object[] param)
	{
		clearAllLine();
		initLine();
		chooseLine();
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
		foreach (INetProxy item in m_allSelectProxy)
		{
			item.UpdateSmartFoxClient();
		}
	}

	private void clearAllLine()
	{
		foreach (INetProxy item in m_allSelectProxy)
		{
			item.Disconnect();
		}
		m_allSelectProxy.Clear();
	}

	private void initLine()
	{
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllProxy");
		if (luaTable == null)
		{
			return;
		}
		for (int i = 1; i <= luaTable.Length; i++)
		{
			LuaTable luaTable2 = (LuaTable)luaTable[i];
			if (luaTable2.ContainsKey("lineName") && luaTable2.ContainsKey("lineUrl") && luaTable2.ContainsKey("port"))
			{
				string name = luaTable2.Get<string>("lineName");
				string h = luaTable2.Get<string>("lineUrl");
				int p = luaTable2.Get<int>("port");
				INetProxy item = new NetRawProxy(name, h, p, 0, _netManager, 0);
				m_allSelectProxy.Add(item);
			}
		}
	}

	private void chooseLine()
	{
		initLine();
		foreach (INetProxy item in m_allSelectProxy)
		{
			item.Connect();
		}
	}

	public override bool OnConnection(INetProxy proxy, BaseEvent e)
	{
		if ((bool)e.Params["success"])
		{
			foreach (INetProxy item in m_allSelectProxy)
			{
				if (!item.proxyName.Equals(proxy.proxyName))
				{
					item.Disconnect();
				}
			}
			_netManager.changeState(NetState.CONNECTED, proxy);
			return true;
		}
		bool flag = true;
		foreach (INetProxy item2 in m_allSelectProxy)
		{
			if (item2.Status != ProxyStatus.connectError)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			_netManager.changeState(NetState.CONNECTERROR, e);
		}
		return false;
	}
}
