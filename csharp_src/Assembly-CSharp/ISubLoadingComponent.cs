public interface ISubLoadingComponent
{
	TextMeshProUGUIEx LoadingText { get; }

	TextMeshProUGUIEx VersionText { get; }

	TextMeshProUGUIEx DownloadText { get; }

	void CSOpen();

	void SetProgressBar(float value);

	void SetBg();

	void SetIcon();

	void SetSlider();

	void CSClose();

	void SetVersionText();
}
