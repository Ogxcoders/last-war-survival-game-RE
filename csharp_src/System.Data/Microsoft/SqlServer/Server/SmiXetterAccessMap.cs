namespace Microsoft.SqlServer.Server;

internal class SmiXetterAccessMap
{
	private const bool X = true;

	private const bool _ = false;

	private static bool[,] s_isSetterAccessValid = new bool[35, 17]
	{
		{
			false, false, false, false, false, false, false, true, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			true, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, true, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			true, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, true,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, true, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, true, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, true, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, true, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, true, false, false, false, false, false
		},
		{
			false, false, false, false, false, true, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, true, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, true, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, true, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true, true, true, false, true, true
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, true, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, true, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, true, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, true, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, true, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, true, false, false, false, false, false
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, true
		}
	};

	internal static bool IsSetterAccessValid(SmiMetaData metaData, SmiXetterTypeCode xetterType)
	{
		return s_isSetterAccessValid[(int)metaData.SqlDbType, (int)xetterType];
	}
}
