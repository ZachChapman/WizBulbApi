namespace WizBulbApi.WinUI;

public class AudioListenerFactory : IAudioListenerFactory
{
    private HashSet<IAudioListener> _audioListenerInstances = new();

    public TAudioListener Create<TAudioListener>() where TAudioListener : IAudioListener
    {
        // TODO: Dependency Injection
        if(_audioListenerInstances.Select(listener => listener.GetType()).Contains(typeof(TAudioListener)))
        {
            return (TAudioListener)_audioListenerInstances.First(listener => listener.GetType() == typeof(TAudioListener));
        }

        var audioListener = (TAudioListener)Activator.CreateInstance(typeof(TAudioListener));
        _audioListenerInstances.Add(audioListener);

        return audioListener;
    }
}
