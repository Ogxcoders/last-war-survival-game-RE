using System;
using Google.Protobuf.Reflection;

namespace Protobuf;

public static class WorldAllianceAssistanceInfoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static WorldAllianceAssistanceInfoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("CiFXb3JsZEFsbGlhbmNlQXNzaXN0YW5jZUluZm8ucHJvdG8SCHByb3RvYnVm" + "IkYKFkFsbGlhbmNlQXNzaXN0YW5jZUluZm8SDwoHcG9pbnRJZBgBIAEoBRIO" + "CgZudW1iZXIYAiABKAUSCwoDbWF4GAMgASgFIksKGVdvcmxkQW9pQXNzaXN0" + "YW5jZUluZm9Nc2cSLgoEbGlzdBgBIAMoCzIgLnByb3RvYnVmLkFsbGlhbmNl" + "QXNzaXN0YW5jZUluZm9CHQobbmV0LmltMzAuYXBzLm1vZGVsLnByb3RvYnVm" + "YgZwcm90bzM="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[2]
		{
			new GeneratedClrTypeInfo(typeof(AllianceAssistanceInfo), AllianceAssistanceInfo.Parser, new string[3] { "PointId", "Number", "Max" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(WorldAoiAssistanceInfoMsg), WorldAoiAssistanceInfoMsg.Parser, new string[1] { "List" }, null, null, null, null)
		}));
	}
}
