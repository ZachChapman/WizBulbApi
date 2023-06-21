using NAudio.Wave;

namespace WizBulbApi.WinUI;

public abstract class AudioListener : IAudioListener
{
    protected IWaveIn _capture;
    protected Action<float> _levelChangedCallback;
    protected float _smoothingFactor;
    protected int _maxPcmValue;
    protected float _levelFraction;

    public virtual void StartLevelCapture(Action<float> levelChangedCallback, float smoothingFactor = 0)
    {
        if(_capture is not null) return;

        _levelChangedCallback = levelChangedCallback;
        _smoothingFactor = smoothingFactor;

        _capture = CreateCapture();

        _capture.DataAvailable += DataAvailable;
        _capture.StartRecording();
    }

    public virtual void StopLevelCapture()
    {
        if(_capture is null) return;

        _capture.DataAvailable -= DataAvailable;
        _capture.StopRecording();
        _capture.Dispose();
        _capture = null;
    }

    protected virtual void DataAvailable(object sender, WaveInEventArgs args)
    {
        var peakValue = int.MinValue;
        var bytesPerSample = 2;

        for(int index = 0; index < args.BytesRecorded; index += bytesPerSample)
        {
            var value = BitConverter.ToInt16(args.Buffer, index);
            peakValue = Math.Max(peakValue, value);
        }

        // report maximum relative to the maximum value previously seen
        _maxPcmValue = Math.Max(_maxPcmValue, peakValue);
        var fraction = (float)peakValue / _maxPcmValue;

        if(_smoothingFactor > 0)
        {
            // basic smoothing so the level does not change too quickly
            _levelFraction += (fraction - _levelFraction) * _smoothingFactor;
            _levelFraction = Math.Max(_levelFraction, 0);



            _levelChangedCallback(_levelFraction);

            return;
        }

        _levelChangedCallback(fraction);
    }

    protected abstract IWaveIn CreateCapture();
}
