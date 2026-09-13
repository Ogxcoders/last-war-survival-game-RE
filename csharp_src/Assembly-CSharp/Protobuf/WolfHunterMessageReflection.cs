using System;
using Google.Protobuf.Reflection;

namespace Protobuf;

public static class WolfHunterMessageReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static WolfHunterMessageReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChdXb2xmSHVudGVyTWVzc2FnZS5wcm90bxIIcHJvdG9idWYiNgoPV29ybGRX" + "b2xmUG9pbnRzEg8KB3BvaW50SWQYASADKAUSEgoKYWxsaWFuY2VJZBgCIAEo" + "CSJFChpXb3JsZFZpZXcyV29sZlBvaW50SW5mb01zZxInCgRsaXN0GAEgAygL" + "MhkucHJvdG9idWYuV29ybGRXb2xmUG9pbnRzQh0KG25ldC5pbTMwLmFwcy5t" + "b2RlbC5wcm90b2J1ZmIGcHJvdG8z"), new FileDescriptor[0], new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[2]
		{
			new GeneratedClrTypeInfo(typeof(WorldWolfPoints), WorldWolfPoints.Parser, new string[2] { "PointId", "AllianceId" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(WorldView2WolfPointInfoMsg), WorldView2WolfPointInfoMsg.Parser, new string[1] { "List" }, null, null, null, null)
		}));
	}
}
