using System.Collections.Generic;
using Protobuf;

public class WorldSuppliesPoint : PointInfo
{
	private IceSuppliesPointInfo _pbInfo;

	private List<string> _uidList;

	public int userCount;

	public long createTime;

	public string discovererAllianceId;

	public string discovererUid;

	public long workEndTime;

	public int workState;

	public int state
	{
		get
		{
			if (_pbInfo != null)
			{
				return _pbInfo.State;
			}
			return -1;
		}
	}

	public int configId
	{
		get
		{
			if (_pbInfo != null)
			{
				return _pbInfo.ConfigId;
			}
			return -1;
		}
	}

	public List<string> uidList
	{
		get
		{
			if (_pbInfo != null && _uidList == null)
			{
				_uidList = new List<string>(_pbInfo.RewardUserList);
			}
			return _uidList;
		}
	}

	public WorldSuppliesPoint()
	{
	}

	public WorldSuppliesPoint(WorldPointInfo pi)
		: base(pi)
	{
		if (pi.IceSuppliesPointInfo.ThermalConductor != null)
		{
			thermalConductor = new ThermalConductor(pi.IceSuppliesPointInfo.ThermalConductor);
		}
		_pbInfo = pi.IceSuppliesPointInfo.Clone();
		userCount = pi.IceSuppliesPointInfo.RewardUserList.Count;
		createTime = pi.IceSuppliesPointInfo.CreateTime;
		discovererAllianceId = pi.IceSuppliesPointInfo.DiscovererAllianceId;
		discovererUid = pi.IceSuppliesPointInfo.DiscovererUid;
		workEndTime = pi.IceSuppliesPointInfo.ChargeEndTime;
		workState = pi.IceSuppliesPointInfo.ChargeState;
		string templateData = GameEntry.ConfigCache.GetTemplateData("ice_supplies", configId, "size");
		tileSize = (string.IsNullOrEmpty(templateData) ? 3 : int.Parse(templateData));
	}
}
