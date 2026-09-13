using System;
using Google.Protobuf.Reflection;

namespace Protobuf;

public static class WorldLightDataReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static WorldLightDataReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChRXb3JsZExpZ2h0RGF0YS5wcm90bxIIcHJvdG9idWYiOwoJTGlnaHREYXRh" + "Eg8KB3BvaW50SWQYASABKAUSDgoGcmFkaXVzGAIgASgFEg0KBWxldmVsGAMg" + "ASgFIkQKD1B1c2hMaWdodENoYW5nZRIiCgVsaWdodBgBIAEoCzITLnByb3Rv" + "YnVmLkxpZ2h0RGF0YRINCgVzdGF0ZRgCIAEoCCI4Cg5SYW5nZUxpZ2h0RGF0" + "YRImCglsaWdodExpc3QYASADKAsyEy5wcm90b2J1Zi5MaWdodERhdGFCHQob" + "bmV0LmltMzAuYXBzLm1vZGVsLnByb3RvYnVmYgZwcm90bzM="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[3]
		{
			new GeneratedClrTypeInfo(typeof(LightData), LightData.Parser, new string[3] { "PointId", "Radius", "Level" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(PushLightChange), PushLightChange.Parser, new string[2] { "Light", "State" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(RangeLightData), RangeLightData.Parser, new string[1] { "LightList" }, null, null, null, null)
		}));
	}
}
