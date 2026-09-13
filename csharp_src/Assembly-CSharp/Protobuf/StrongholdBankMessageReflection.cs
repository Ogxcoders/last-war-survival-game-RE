using System;
using Google.Protobuf.Reflection;

namespace Protobuf;

public static class StrongholdBankMessageReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static StrongholdBankMessageReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChtTdHJvbmdob2xkQmFua01lc3NhZ2UucHJvdG8SCHByb3RvYnVmIkkKC0Jh" + "bmtEZXBvc2l0EhUKDWRlcG9zaXRBbW91bnQYASABKAMSEwoLZGVwb3NpdERh" + "eXMYAiABKAUSDgoGaXRlbUlkGAMgASgJQh0KG25ldC5pbTMwLmFwcy5tb2Rl" + "bC5wcm90b2J1ZmIGcHJvdG8z"), new FileDescriptor[0], new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[1]
		{
			new GeneratedClrTypeInfo(typeof(BankDeposit), BankDeposit.Parser, new string[3] { "DepositAmount", "DepositDays", "ItemId" }, null, null, null, null)
		}));
	}
}
