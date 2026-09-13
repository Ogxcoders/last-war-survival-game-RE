using UnityEngine;

public static class Raycast2DLineRect
{
	public struct LineRectResult
	{
		public float entry;

		public Sector entrySector;

		public float exit;

		public Sector exitSector;

		public bool HaveHit => !float.IsPositiveInfinity(entry);

		public override string ToString()
		{
			return $"t_entry={entry}, t_exit={exit}";
		}
	}

	public enum Sector
	{
		__,
		S0,
		S1,
		S2,
		S3,
		S4,
		S5,
		S6,
		S7,
		S8
	}

	private static readonly Sector[,,] RaycastLookup = new Sector[9, 9, 2]
	{
		{
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.S0,
				Sector.S4
			},
			{
				Sector.S0,
				Sector.S5
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.S0,
				Sector.S7
			},
			{
				Sector.S0,
				Sector.S8
			}
		},
		{
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.S1,
				Sector.S3
			},
			{
				Sector.S1,
				Sector.S4
			},
			{
				Sector.S1,
				Sector.S5
			},
			{
				Sector.S1,
				Sector.S6
			},
			{
				Sector.S1,
				Sector.S7
			},
			{
				Sector.S1,
				Sector.S8
			}
		},
		{
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.S2,
				Sector.S3
			},
			{
				Sector.S2,
				Sector.S4
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.S2,
				Sector.S6
			},
			{
				Sector.S2,
				Sector.S7
			},
			{
				Sector.__,
				Sector.__
			}
		},
		{
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.S3,
				Sector.S1
			},
			{
				Sector.S3,
				Sector.S2
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.S3,
				Sector.S4
			},
			{
				Sector.S3,
				Sector.S5
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.S3,
				Sector.S7
			},
			{
				Sector.S3,
				Sector.S8
			}
		},
		{
			{
				Sector.S4,
				Sector.S0
			},
			{
				Sector.S4,
				Sector.S1
			},
			{
				Sector.S4,
				Sector.S2
			},
			{
				Sector.S4,
				Sector.S3
			},
			{
				Sector.S4,
				Sector.S4
			},
			{
				Sector.S4,
				Sector.S5
			},
			{
				Sector.S4,
				Sector.S6
			},
			{
				Sector.S4,
				Sector.S7
			},
			{
				Sector.S4,
				Sector.S8
			}
		},
		{
			{
				Sector.S5,
				Sector.S0
			},
			{
				Sector.S5,
				Sector.S1
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.S5,
				Sector.S3
			},
			{
				Sector.S5,
				Sector.S4
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.S5,
				Sector.S6
			},
			{
				Sector.S5,
				Sector.S7
			},
			{
				Sector.__,
				Sector.__
			}
		},
		{
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.S6,
				Sector.S1
			},
			{
				Sector.S6,
				Sector.S2
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.S6,
				Sector.S4
			},
			{
				Sector.S6,
				Sector.S5
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.__,
				Sector.__
			}
		},
		{
			{
				Sector.S7,
				Sector.S0
			},
			{
				Sector.S7,
				Sector.S1
			},
			{
				Sector.S7,
				Sector.S2
			},
			{
				Sector.S7,
				Sector.S3
			},
			{
				Sector.S7,
				Sector.S4
			},
			{
				Sector.S7,
				Sector.S5
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.__,
				Sector.__
			}
		},
		{
			{
				Sector.S8,
				Sector.S0
			},
			{
				Sector.S8,
				Sector.S1
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.S8,
				Sector.S3
			},
			{
				Sector.S8,
				Sector.S4
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.__,
				Sector.__
			},
			{
				Sector.__,
				Sector.__
			}
		}
	};

	public static void RaycastLineRect(Vector2 begin, Vector2 end, Rect rect, ref LineRectResult res)
	{
		Vector3 vector = end - begin;
		int rectPointSector = GetRectPointSector(rect, begin);
		int rectPointSector2 = GetRectPointSector(rect, end);
		res.entry = GetRayToRectSide(begin, vector, RaycastLookup[rectPointSector, rectPointSector2, 0], rect, 0f, out var hitSide);
		res.entrySector = hitSide;
		res.exit = GetRayToRectSide(begin, vector, RaycastLookup[rectPointSector, rectPointSector2, 1], rect, 1f, out hitSide);
		res.exitSector = hitSide;
	}

	private static int GetRectPointSector(Rect rect, Vector2 point)
	{
		if (point.y > rect.yMax)
		{
			if (point.x < rect.xMin)
			{
				return 0;
			}
			if (point.x > rect.xMax)
			{
				return 2;
			}
			return 1;
		}
		if (point.y < rect.yMin)
		{
			if (point.x < rect.xMin)
			{
				return 6;
			}
			if (point.x > rect.xMax)
			{
				return 8;
			}
			return 7;
		}
		if (point.x < rect.xMin)
		{
			return 3;
		}
		if (point.x > rect.xMax)
		{
			return 5;
		}
		return 4;
	}

	private static float GetRayToRectSide(Vector2 begin, Vector2 dir, Sector side, Rect rect, float r4val, out Sector hitSide)
	{
		hitSide = Sector.__;
		switch (side)
		{
		case Sector.S0:
		{
			float rayToRectSide = GetRayToRectSide(begin, dir, Sector.S1, rect, r4val, out hitSide);
			if (!float.IsPositiveInfinity(rayToRectSide))
			{
				return rayToRectSide;
			}
			return GetRayToRectSide(begin, dir, Sector.S3, rect, r4val, out hitSide);
		}
		case Sector.S1:
		{
			float rayToRectSide = RayToHoriz(begin, dir, rect.xMin, rect.yMax, rect.width);
			if (!float.IsPositiveInfinity(rayToRectSide))
			{
				hitSide = Sector.S1;
			}
			return rayToRectSide;
		}
		case Sector.S2:
		{
			float rayToRectSide = GetRayToRectSide(begin, dir, Sector.S1, rect, r4val, out hitSide);
			if (!float.IsPositiveInfinity(rayToRectSide))
			{
				return rayToRectSide;
			}
			return GetRayToRectSide(begin, dir, Sector.S5, rect, r4val, out hitSide);
		}
		case Sector.S3:
		{
			float rayToRectSide = RayToVert(begin, dir, rect.xMin, rect.yMin, rect.height);
			if (!float.IsPositiveInfinity(rayToRectSide))
			{
				hitSide = Sector.S3;
			}
			return rayToRectSide;
		}
		case Sector.S4:
			return r4val;
		case Sector.S5:
		{
			float rayToRectSide = RayToVert(begin, dir, rect.xMax, rect.yMin, rect.height);
			if (!float.IsPositiveInfinity(rayToRectSide))
			{
				hitSide = Sector.S5;
			}
			return rayToRectSide;
		}
		case Sector.S6:
		{
			float rayToRectSide = GetRayToRectSide(begin, dir, Sector.S3, rect, r4val, out hitSide);
			if (!float.IsPositiveInfinity(rayToRectSide))
			{
				return rayToRectSide;
			}
			return GetRayToRectSide(begin, dir, Sector.S7, rect, r4val, out hitSide);
		}
		case Sector.S7:
		{
			float rayToRectSide = RayToHoriz(begin, dir, rect.xMin, rect.yMin, rect.width);
			if (!float.IsPositiveInfinity(rayToRectSide))
			{
				hitSide = Sector.S7;
			}
			return rayToRectSide;
		}
		case Sector.S8:
		{
			float rayToRectSide = GetRayToRectSide(begin, dir, Sector.S5, rect, r4val, out hitSide);
			if (!float.IsPositiveInfinity(rayToRectSide))
			{
				return rayToRectSide;
			}
			return GetRayToRectSide(begin, dir, Sector.S7, rect, r4val, out hitSide);
		}
		default:
			return float.PositiveInfinity;
		}
	}

	private static float RayToHoriz(Vector2 fromPoint, Vector2 fromDir, float x, float y, float width)
	{
		float num = (y - fromPoint.y) / fromDir.y;
		if (num < 0f || num > 1f)
		{
			return float.PositiveInfinity;
		}
		float num2 = (fromPoint.x + fromDir.x * num - x) / width;
		if (num2 < 0f || num2 > 1f)
		{
			return float.PositiveInfinity;
		}
		return num;
	}

	private static float RayToVert(Vector2 fromPoint, Vector2 fromDir, float x, float y, float height)
	{
		float num = (x - fromPoint.x) / fromDir.x;
		if (num < 0f || num > 1f)
		{
			return float.PositiveInfinity;
		}
		float num2 = (fromPoint.y + fromDir.y * num - y) / height;
		if (num2 < 0f || num2 > 1f)
		{
			return float.PositiveInfinity;
		}
		return num;
	}

	private static float RayToRay(Vector2 fromPoint, Vector2 fromDir, Vector2 toPoint, Vector2 toDir)
	{
		float num = (toDir.x * (fromPoint.y - toPoint.y) + toDir.y * (toPoint.x - fromPoint.x)) / (fromDir.x * toDir.y - fromDir.y * toDir.x);
		if (num < 0f || num > 1f)
		{
			return float.PositiveInfinity;
		}
		float num2 = (fromDir.x * (toPoint.y - fromPoint.y) + fromDir.y * (fromPoint.x - toPoint.x)) / (toDir.x * fromDir.y - toDir.y * fromDir.x);
		if (num2 < 0f || num2 > 1f)
		{
			return float.PositiveInfinity;
		}
		return num;
	}
}
