using System.Collections.Generic;

namespace System.Security.AccessControl;

internal class SddlAccessRight
{
	private static readonly SddlAccessRight[] rights = new SddlAccessRight[28]
	{
		new SddlAccessRight
		{
			Name = "CC",
			Value = 1,
			ObjectType = 1
		},
		new SddlAccessRight
		{
			Name = "DC",
			Value = 2,
			ObjectType = 1
		},
		new SddlAccessRight
		{
			Name = "LC",
			Value = 4,
			ObjectType = 1
		},
		new SddlAccessRight
		{
			Name = "SW",
			Value = 8,
			ObjectType = 1
		},
		new SddlAccessRight
		{
			Name = "RP",
			Value = 16,
			ObjectType = 1
		},
		new SddlAccessRight
		{
			Name = "WP",
			Value = 32,
			ObjectType = 1
		},
		new SddlAccessRight
		{
			Name = "DT",
			Value = 64,
			ObjectType = 1
		},
		new SddlAccessRight
		{
			Name = "LO",
			Value = 128,
			ObjectType = 1
		},
		new SddlAccessRight
		{
			Name = "CR",
			Value = 256,
			ObjectType = 1
		},
		new SddlAccessRight
		{
			Name = "SD",
			Value = 65536
		},
		new SddlAccessRight
		{
			Name = "RC",
			Value = 131072
		},
		new SddlAccessRight
		{
			Name = "WD",
			Value = 262144
		},
		new SddlAccessRight
		{
			Name = "WO",
			Value = 524288
		},
		new SddlAccessRight
		{
			Name = "GA",
			Value = 268435456
		},
		new SddlAccessRight
		{
			Name = "GX",
			Value = 536870912
		},
		new SddlAccessRight
		{
			Name = "GW",
			Value = 1073741824
		},
		new SddlAccessRight
		{
			Name = "GR",
			Value = int.MinValue
		},
		new SddlAccessRight
		{
			Name = "FA",
			Value = 2032127,
			ObjectType = 2
		},
		new SddlAccessRight
		{
			Name = "FR",
			Value = 1179785,
			ObjectType = 2
		},
		new SddlAccessRight
		{
			Name = "FW",
			Value = 1179926,
			ObjectType = 2
		},
		new SddlAccessRight
		{
			Name = "FX",
			Value = 1179808,
			ObjectType = 2
		},
		new SddlAccessRight
		{
			Name = "KA",
			Value = 983103,
			ObjectType = 3
		},
		new SddlAccessRight
		{
			Name = "KR",
			Value = 131097,
			ObjectType = 3
		},
		new SddlAccessRight
		{
			Name = "KW",
			Value = 131078,
			ObjectType = 3
		},
		new SddlAccessRight
		{
			Name = "KX",
			Value = 131097,
			ObjectType = 3
		},
		new SddlAccessRight
		{
			Name = "NW",
			Value = 1
		},
		new SddlAccessRight
		{
			Name = "NR",
			Value = 2
		},
		new SddlAccessRight
		{
			Name = "NX",
			Value = 4
		}
	};

	public string Name { get; set; }

	public int Value { get; set; }

	public int ObjectType { get; set; }

	public static SddlAccessRight LookupByName(string s)
	{
		SddlAccessRight[] array = rights;
		foreach (SddlAccessRight sddlAccessRight in array)
		{
			if (sddlAccessRight.Name == s)
			{
				return sddlAccessRight;
			}
		}
		return null;
	}

	public static SddlAccessRight[] Decompose(int mask)
	{
		SddlAccessRight[] array = rights;
		foreach (SddlAccessRight sddlAccessRight in array)
		{
			if (mask == sddlAccessRight.Value)
			{
				return new SddlAccessRight[1] { sddlAccessRight };
			}
		}
		int num = 0;
		List<SddlAccessRight> list = new List<SddlAccessRight>();
		int num2 = 0;
		array = rights;
		foreach (SddlAccessRight sddlAccessRight2 in array)
		{
			if ((mask & sddlAccessRight2.Value) == sddlAccessRight2.Value && (num2 | sddlAccessRight2.Value) != num2)
			{
				if (num == 0)
				{
					num = sddlAccessRight2.ObjectType;
				}
				if (sddlAccessRight2.ObjectType != 0 && num != sddlAccessRight2.ObjectType)
				{
					return null;
				}
				list.Add(sddlAccessRight2);
				num2 |= sddlAccessRight2.Value;
			}
			if (num2 == mask)
			{
				return list.ToArray();
			}
		}
		return null;
	}
}
