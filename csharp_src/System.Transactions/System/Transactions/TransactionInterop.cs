namespace System.Transactions;

[System.MonoTODO]
public static class TransactionInterop
{
	public static readonly Guid PromoterTypeDtc = new Guid("14229753-FFE1-428D-82B7-DF73045CB8DA");

	[System.MonoTODO]
	public static IDtcTransaction GetDtcTransaction(Transaction transaction)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public static byte[] GetExportCookie(Transaction transaction, byte[] whereabouts)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public static Transaction GetTransactionFromDtcTransaction(IDtcTransaction transactionNative)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public static Transaction GetTransactionFromExportCookie(byte[] cookie)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public static Transaction GetTransactionFromTransmitterPropagationToken(byte[] propagationToken)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public static byte[] GetTransmitterPropagationToken(Transaction transaction)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public static byte[] GetWhereabouts()
	{
		throw new NotImplementedException();
	}
}
