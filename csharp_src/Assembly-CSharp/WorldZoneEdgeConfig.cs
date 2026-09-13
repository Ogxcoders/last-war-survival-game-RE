using System.Collections.Generic;
using UnityEngine;

public class WorldZoneEdgeConfig : ScriptableObject
{
	public List<WorldZoneEdgeData> edgeList = new List<WorldZoneEdgeData>();

	public List<WorldZoneEdgeDataKeyInfo> zoneList = new List<WorldZoneEdgeDataKeyInfo>();
}
