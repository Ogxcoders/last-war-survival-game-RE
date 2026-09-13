namespace AppsFlyerSDK;

public interface IAppsFlyerConversionData
{
	void onConversionDataSuccess(string conversionData);

	void onConversionDataFail(string error);

	void onAppOpenAttribution(string attributionData);

	void onAppOpenAttributionFailure(string error);
}
