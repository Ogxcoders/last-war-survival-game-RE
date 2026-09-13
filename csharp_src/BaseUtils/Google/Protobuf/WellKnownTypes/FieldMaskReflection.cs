using System;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes;

public static class FieldMaskReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static FieldMaskReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("CiBnb29nbGUvcHJvdG9idWYvZmllbGRfbWFzay5wcm90bxIPZ29vZ2xlLnBy" + "b3RvYnVmIhoKCUZpZWxkTWFzaxINCgVwYXRocxgBIAMoCUKMAQoTY29tLmdv" + "b2dsZS5wcm90b2J1ZkIORmllbGRNYXNrUHJvdG9QAVo5Z29vZ2xlLmdvbGFu" + "Zy5vcmcvZ2VucHJvdG8vcHJvdG9idWYvZmllbGRfbWFzaztmaWVsZF9tYXNr" + "+AEBogIDR1BCqgIeR29vZ2xlLlByb3RvYnVmLldlbGxLbm93blR5cGVzYgZw" + "cm90bzM="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[1]
		{
			new GeneratedClrTypeInfo(typeof(FieldMask), FieldMask.Parser, new string[1] { "Paths" }, null, null, null, null)
		}));
	}
}
