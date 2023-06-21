using NAudio.Wave;

namespace WizBulbApi.WinUI;

public class RecordingDeviceCaptureAudioListener : AudioListener
{
    protected override IWaveIn CreateCapture()
    {
        return new WaveInEvent
        {
            DeviceNumber = 0,
            WaveFormat = new WaveFormat(rate: 4400, bits: 16, channels: 1),
            BufferMilliseconds = 50,
        };
    }
}
