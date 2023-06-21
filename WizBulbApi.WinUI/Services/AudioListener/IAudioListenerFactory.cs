namespace WizBulbApi.WinUI;

public interface IAudioListenerFactory
{
    TAudioListener Create<TAudioListener>() where TAudioListener : IAudioListener;
}
