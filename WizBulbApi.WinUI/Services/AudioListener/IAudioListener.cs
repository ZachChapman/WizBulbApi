namespace WizBulbApi.WinUI;

public interface IAudioListener
{
    public void StartLevelCapture(Action<float> levelChangedCallback, float smoothingFactor = 0);
    public void StopLevelCapture();
}
