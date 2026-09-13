using System;
using Google.Protobuf.Reflection;

namespace Protobuf;

public static class AllianceCityRecordProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static AllianceCityRecordProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("Ch1BbGxpYW5jZUNpdHlSZWNvcmRQcm90by5wcm90bxIIcHJvdG9idWYiNAoW" + "QWxsaWFuY2VDaXR5VXNlclJlY29yZBILCgN1aWQYASABKAkSDQoFcG9pbnQY" + "AiABKAMikgEKF0FsbGlhbmNlQ2l0eVJlY29yZFByb3RvEjkKD2tpbGxVc2Vy" + "UmVjb3JkcxgBIAMoCzIgLnByb3RvYnVmLkFsbGlhbmNlQ2l0eVVzZXJSZWNv" + "cmQSPAoSZGVzdHJveVVzZXJSZWNvcmRzGAIgAygLMiAucHJvdG9idWYuQWxs" + "aWFuY2VDaXR5VXNlclJlY29yZCKZAQoWQWxsaWFuY2VDaXR5T2NjdXB5SW5m" + "bxIOCgZjaXR5SWQYASABKAUSDAoEYWJichgCIAEoCRISCgphbGxhaW5jZUlk" + "GAMgASgJEg0KBWNvbG9yGAQgASgFEhAKCGNpdHlOYW1lGAUgASgJEhQKDGFs" + "bGlhbmNlTmFtZRgGIAEoCRIWCg5vY2N1cHlTZXJ2ZXJJZBgHIAEoBSJMChhX" + "b3JsZEFsbEFsbGlhbmNlQ2l0eUluZm8SMAoGaW5mb2VzGAEgAygLMiAucHJv" + "dG9idWYuQWxsaWFuY2VDaXR5T2NjdXB5SW5mb0IdChtuZXQuaW0zMC5hcHMu" + "bW9kZWwucHJvdG9idWZiBnByb3RvMw=="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[4]
		{
			new GeneratedClrTypeInfo(typeof(AllianceCityUserRecord), AllianceCityUserRecord.Parser, new string[2] { "Uid", "Point" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(AllianceCityRecordProto), AllianceCityRecordProto.Parser, new string[2] { "KillUserRecords", "DestroyUserRecords" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(AllianceCityOccupyInfo), AllianceCityOccupyInfo.Parser, new string[7] { "CityId", "Abbr", "AllainceId", "Color", "CityName", "AllianceName", "OccupyServerId" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(WorldAllAllianceCityInfo), WorldAllAllianceCityInfo.Parser, new string[1] { "Infoes" }, null, null, null, null)
		}));
	}
}
