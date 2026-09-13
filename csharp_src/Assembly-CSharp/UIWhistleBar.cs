using UnityEngine;

public class UIWhistleBar : MonoBehaviour
{
	[SerializeField]
	private GameObject waiting;

	[SerializeField]
	private GameObject whistling;

	[SerializeField]
	private GameObject following;

	[SerializeField]
	private SuperTextMesh descTxt;

	[SerializeField]
	private SuperTextMesh nameTxt;

	private WhistleMonsterState curState;

	public void RefreshView(WorldMarch marchInfo)
	{
		curState = marchInfo.darknessMonsterData.whistleMonsterData?.state ?? WhistleMonsterState.Normal;
		switch (curState)
		{
		case WhistleMonsterState.Whistled:
			waiting.SetActive(value: true);
			whistling.SetActive(value: false);
			following.SetActive(value: false);
			break;
		case WhistleMonsterState.Following:
			waiting.SetActive(value: false);
			whistling.SetActive(value: false);
			following.SetActive(value: true);
			descTxt.text = GameEntry.Localization.GetString("season_s4_activity_1200009_desc34");
			break;
		default:
			waiting.SetActive(value: false);
			whistling.SetActive(value: false);
			following.SetActive(value: false);
			break;
		}
		nameTxt.text = (string.IsNullOrEmpty(marchInfo.ownerName) ? "" : marchInfo.ownerName);
	}

	public void SetWhistling()
	{
		if (curState == WhistleMonsterState.Whistled)
		{
			waiting.SetActive(value: false);
			whistling.SetActive(value: true);
		}
	}

	public void Dispose()
	{
	}
}
