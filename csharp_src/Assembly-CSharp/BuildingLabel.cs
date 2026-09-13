using UnityEngine;

public class BuildingLabel : MonoBehaviour
{
	[SerializeField]
	private SuperTextMesh m_level;

	[SerializeField]
	private GameObject m_Upgrade;

	[SerializeField]
	private SuperTextMesh m_name;

	[SerializeField]
	private GameObject m_nameBg;

	private CityBuilding building;

	private Vector3 localPosition;

	public CityBuilding Building
	{
		get
		{
			return building;
		}
		set
		{
			building = value;
		}
	}

	private void Awake()
	{
		localPosition = base.transform.localPosition;
	}

	public void SetData()
	{
		BuildPointInfo buildInfo = building.GetBuildInfo();
		if (buildInfo == null)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		PlayerType playerType = buildInfo.GetPlayerType();
		string ownerUid = buildInfo.ownerUid;
		m_name.text = ownerUid;
		switch (playerType)
		{
		case PlayerType.PlayerSelf:
			m_name.color32 = GameDefines.CityLabelTextColor.Green;
			break;
		case PlayerType.PlayerAlliance:
			m_name.color32 = GameDefines.CityLabelTextColor.Blue;
			break;
		case PlayerType.PlayerAllianceLeader:
			m_name.color32 = GameDefines.CityLabelTextColor.Purple;
			break;
		default:
			m_name.color32 = GameDefines.CityLabelTextColor.White;
			break;
		}
	}

	public void UpdateUpgradeLabel()
	{
	}

	public void OnBuildingPositionChanged()
	{
		base.transform.position = building.transform.TransformPoint(localPosition);
	}

	public void BuildingEnterMoveState(bool isEnter)
	{
	}
}
