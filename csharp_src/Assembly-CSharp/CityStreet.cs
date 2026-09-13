using System.Collections.Generic;
using UnityEngine;

public class CityStreet
{
	public class CityStreetExit
	{
		public Vector2Int Pos;

		public Vector2Int OutPos;

		public int OutStreetId = -1;

		public CityStreetExit(Vector2Int pos, Vector2Int outPos)
		{
			Pos = pos;
			OutPos = outPos;
		}
	}

	public int StreetId;

	public List<Vector2Int> RoadList;

	public bool isSingle;

	public List<CityStreetExit> Exits;

	public CityStreet(int streetId)
	{
		StreetId = streetId;
		RoadList = new List<Vector2Int>();
		Exits = new List<CityStreetExit>();
		isSingle = false;
	}
}
