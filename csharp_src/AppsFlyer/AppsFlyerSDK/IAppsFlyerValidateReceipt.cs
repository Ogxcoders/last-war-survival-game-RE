namespace AppsFlyerSDK;

public interface IAppsFlyerValidateReceipt
{
	void didFinishValidateReceipt(string result);

	void didFinishValidateReceiptWithError(string error);
}
