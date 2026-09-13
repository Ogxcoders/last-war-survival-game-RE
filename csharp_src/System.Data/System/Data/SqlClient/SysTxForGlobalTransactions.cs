using System.Reflection;
using System.Transactions;

namespace System.Data.SqlClient;

internal static class SysTxForGlobalTransactions
{
	private static readonly Lazy<MethodInfo> _enlistPromotableSinglePhase = new Lazy<MethodInfo>(() => typeof(Transaction).GetMethod("EnlistPromotableSinglePhase", new Type[2]
	{
		typeof(IPromotableSinglePhaseNotification),
		typeof(Guid)
	}));

	private static readonly Lazy<MethodInfo> _setDistributedTransactionIdentifier = new Lazy<MethodInfo>(() => typeof(Transaction).GetMethod("SetDistributedTransactionIdentifier", new Type[2]
	{
		typeof(IPromotableSinglePhaseNotification),
		typeof(Guid)
	}));

	private static readonly Lazy<MethodInfo> _getPromotedToken = new Lazy<MethodInfo>(() => typeof(Transaction).GetMethod("GetPromotedToken"));

	public static MethodInfo EnlistPromotableSinglePhase => _enlistPromotableSinglePhase.Value;

	public static MethodInfo SetDistributedTransactionIdentifier => _setDistributedTransactionIdentifier.Value;

	public static MethodInfo GetPromotedToken => _getPromotedToken.Value;
}
