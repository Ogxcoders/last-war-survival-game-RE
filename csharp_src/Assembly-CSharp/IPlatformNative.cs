public interface IPlatformNative
{
	bool HasSignedIn { get; set; }

	string UID { get; set; }

	GamePlatform ID { get; }

	PaymentChannel PaymentChannel { get; }

	LoginPlatform LoginPlatform { get; set; }

	void InitPlatform(string proxyName);

	void SignIn(string json);

	void SignOut();

	void Pay(int channelId, string json);

	void QueryPurchaseOrder();

	void ConsumeProduct(string orderId, int status);

	void SendDataToNative(string funcName, string data);

	string GetDataFromNative(string funcName, string data);

	string GetPermissionByType(string data);

	string GetLaunchPushID();

	void Restart(string data);

	string GetRestartData();

	long GetRealtime();
}
