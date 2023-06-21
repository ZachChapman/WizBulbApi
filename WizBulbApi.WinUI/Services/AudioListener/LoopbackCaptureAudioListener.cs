using NAudio.Wave;

namespace WizBulbApi.WinUI;

public class LoopbackCaptureAudioListener : AudioListener
{
    protected override IWaveIn CreateCapture()
    {
        return new WasapiLoopbackCapture()
        {
            WaveFormat = new WaveFormat(rate: 4400, bits: 16, channels: 1),
        };
    }
}
