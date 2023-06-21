using MvvmFramework;

namespace WizBulbApi.WinUI;

public class BulbExtraViewModel : ViewModel
{
	private readonly IAudioListenerFactory _audioListenerFactory;
	private readonly LoopbackCaptureAudioListener _loopbackCaptureAudioListener;
	private readonly RecordingDeviceCaptureAudioListener _recordingDeviceCaptureAudioListener;

	public BulbExtraViewModel(IAudioListenerFactory audioListenerFactory)
	{
		_audioListenerFactory = audioListenerFactory;

		_loopbackCaptureAudioListener = _audioListenerFactory.Create<LoopbackCaptureAudioListener>();
		_recordingDeviceCaptureAudioListener = _audioListenerFactory.Create<RecordingDeviceCaptureAudioListener>();
	}

	public float LoopbackLevel { get; set; }
	public float MicrophoneLevel { get; set; }

	//public override async Task LoadAsync()
	//{
	//	//_loopbackCaptureAudioListener.StartLevelCapture(level =>
	//	//{
	//	//    App.Window.DispatcherQueue.TryEnqueue(() =>
	//	//    {
	//	//        LoopbackLevel = level * WizBulbApi.Brightness.Max;
	//	//    });

	//	//    var lerpedColor = Lerp(Colors.Green, Colors.Red, level);
	//	//    var color = Color;
	//	//    color.R = lerpedColor.R;
	//	//    color.G = lerpedColor.G;
	//	//    color.B = lerpedColor.B;
	//	//    Color = color;

	//	//}, smoothingFactor: 0.0f);

	//	//_recordingDeviceCaptureAudioListener.StartLevelCapture(level =>
	//	//{
	//	//    App.Window.DispatcherQueue.TryEnqueue(() =>
	//	//    {
	//	//        MicrophoneLevel = level * WizBulbApi.Brightness.Max;
	//	//    });
	//	//});
	//}

	//public static System.Drawing.Color Lerp(Color s, Color t, float k)
	//{
	//    var bk = (1 - k);
	//    var a = s.A * bk + t.A * k;
	//    var r = s.R * bk + t.R * k;
	//    var g = s.G * bk + t.G * k;
	//    var b = s.B * bk + t.B * k;

	//    return System.Drawing.Color.FromArgb((int)a, (int)r, (int)g, (int)b);
	//}
}
