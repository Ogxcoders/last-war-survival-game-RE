using System.Collections.Generic;

public class PayOrderDataManager
{
	private HashSet<string> _consumedOrderList = new HashSet<string>();

	private bool _isConsumedOrderDetectFunctionOpen;

	private string _nativeQueryPriceResult = "";

	private string _storefrontCode = string.Empty;

	public bool IsOrderConsumed(string orderId)
	{
		return _consumedOrderList.Contains(orderId);
	}

	public void AddOrderToConsumedList(string orderId)
	{
		_consumedOrderList.Add(orderId);
	}

	public void SetConsumedOrderDetectFunctionOpen(bool isOpen)
	{
		_isConsumedOrderDetectFunctionOpen = isOpen;
	}

	public bool IsConsumedOrderDetectFunctionOpen()
	{
		return _isConsumedOrderDetectFunctionOpen;
	}

	public void SaveNativeQueryPriceResult(string data)
	{
		_nativeQueryPriceResult = data;
	}

	public string GetNativeQueryPriceResult()
	{
		string nativeQueryPriceResult = _nativeQueryPriceResult;
		_nativeQueryPriceResult = "";
		return nativeQueryPriceResult;
	}

	public void SaveStorefrontCode(string code)
	{
		_storefrontCode = code ?? string.Empty;
	}

	public string GetStorefrontCode()
	{
		return _storefrontCode;
	}
}
