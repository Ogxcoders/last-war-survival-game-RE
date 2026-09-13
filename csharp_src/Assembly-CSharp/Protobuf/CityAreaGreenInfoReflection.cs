using System;
using Google.Protobuf.Reflection;

namespace Protobuf;

public static class CityAreaGreenInfoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static CityAreaGreenInfoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChdDaXR5QXJlYUdyZWVuSW5mby5wcm90bxIIcHJvdG9idWYiMgoNQ2l0eUdy" + "ZWVuSW5mbxIOCgZjaXR5SWQYASABKAUSEQoJZ3JlZW5SYXRlGAIgASgBIkIK" + "FVdvcmxkQWxsQ2l0eUdyZWVuSW5mbxIpCghpbmZvTGlzdBgBIAMoCzIXLnBy" + "b3RvYnVmLkNpdHlHcmVlbkluZm8iKwoLR3JlZW5Qb2ludHMSDgoGcG9pbnRz" + "GAEgAygFEgwKBHR5cGUYAiABKAUiRAoQSW5kZXhHcmVlblBvaW50cxINCgVp" + "bmRleBgBIAEoBRIOCgZwb2ludHMYAiADKAUSEQoJdGltZVN0YW1wGAMgASgD" + "Qh0KG25ldC5pbTMwLmFwcy5tb2RlbC5wcm90b2J1ZmIGcHJvdG8z"), new FileDescriptor[0], new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[4]
		{
			new GeneratedClrTypeInfo(typeof(CityGreenInfo), CityGreenInfo.Parser, new string[2] { "CityId", "GreenRate" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(WorldAllCityGreenInfo), WorldAllCityGreenInfo.Parser, new string[1] { "InfoList" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(GreenPoints), GreenPoints.Parser, new string[2] { "Points", "Type" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(IndexGreenPoints), IndexGreenPoints.Parser, new string[3] { "Index", "Points", "TimeStamp" }, null, null, null, null)
		}));
	}
}
