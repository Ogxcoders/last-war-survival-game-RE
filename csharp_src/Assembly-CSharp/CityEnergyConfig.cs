using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class CityEnergyConfig : MonoBehaviour
{
	private static int FOG_TILE_COUNT = 100;

	private static float FOG_TILE_SIZE = 2f;

	[HideInInspector]
	public int objId;

	public string triggerId;

	public string pos;

	public string triggerType;

	public string unlockPara1;

	public string unlockPara2;

	public string unlockPara3;

	public string unlockPara4;

	public string unlockPara5;

	public string triggerType2;

	public string itemName;

	public string description;

	public GameObject modelNameGameObject;

	public string modelName;

	public string timeline;

	public string anim;

	public string energyCost;

	public string rewardId;

	public string expReward;

	public string followNpc;

	public string trans;

	public string jsRotation;

	public string needArmy;

	public string dis;

	public bool sideQuest;

	public string order;

	public string showPos;

	public CityEnergyConfig conditionIdConfig;

	public string conditionId;

	public GameObject fogGameObject;

	public string unlockFog;

	private HashSet<int> unlockFogIdSet = new HashSet<int>();

	public bool showFogInEditor;

	public string specialTag;

	public string noviceboot;

	public string bubbleDisplay;

	public GameObject effectAreaGameObject;

	public string effectArea;

	private HashSet<Vector2Int> effectAreaSet = new HashSet<Vector2Int>();

	public bool showEffectAreaInEditor;

	public string forceFinish;

	public int editorFieldFontSize = 12;

	public string editorField = "rewardId";

	private static readonly ValueDropdownList<string> editorFieldList = new ValueDropdownList<string>
	{
		{ "无", "" },
		{ "id", "triggerId" },
		{ "name", "itemName" },
		{ "description", "description" },
		{ "Timeline", "timeline" },
		{ "energy_cost", "energyCost" },
		{ "rewardId", "rewardId" },
		{ "Noviceboot", "noviceboot" },
		{ "ForceFinish", "forceFinish" }
	};
}
