using Microsoft.Extensions.Logging;
using Microsoft.UI;
using MvvmFramework;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Toolbox;
using Windows.UI;
using WizBulbApi.Commands;

namespace WizBulbApi.WinUI;

public class BulbViewModel : ViewModel, IDisposable
{
    private BulbClient _bulbClient;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly IBulbDataAccess _bulbDataAccess;
    private readonly ISettingsDataAccess _settingsDataAccess;
    private readonly NavigationCommands _navigationCommands;
    private readonly ILogger<BulbViewModel> _logger;
    private readonly Repeater _stateListenerRepeater;
    private readonly Repeater _sampleScreenRepeater;
    private readonly ActionQueue _commandProcessQueue = new();
    private readonly List<IBulbModule> _bulbModules = new();
    private bool _queueCommandsViaPropertyChange => _queueCommandLock == 0;
    private int _queueCommandLock;

    public BulbViewModel(
        INavigationService navigationService,
        IDialogService dialogService,
        IBulbDataAccess bulbDataAccess,
        ISettingsDataAccess settingsDataAccess,
        ISamplePointDataAccess samplePointDataAccess,
        NavigationCommands navigationCommands,
        ILogger<BulbViewModel> logger)
    {
        _navigationService = navigationService;
        _dialogService = dialogService;
        _bulbDataAccess = bulbDataAccess;
        _settingsDataAccess = settingsDataAccess;
        _navigationCommands = navigationCommands;
        _logger = logger;

        _stateListenerRepeater = new(async () =>
        {
            await ListenToStateAsync();
        }, TimeSpan.FromSeconds(2));

        // TODO: Inject/Initialise elsewhere.
        _bulbModules.Add(new ScreenSamplerBulbModule(samplePointDataAccess));
    }

    public Connection Connection { get; private set; }
    public bool IsSearching => Connection is Connection.Searching;
    public bool IsConnected => Connection is Connection.Connected;
    public bool IsNotFound => Connection is Connection.NotFound;
    public string Name { get; set; }
    public bool IsOn { get; set; }
    public Color Color { get; set; } = new();
    public int Temperature { get; set; }
    public int Brightness { get; set; }
    public int Speed { get; set; }
    public int SceneId { get; set; } = -1;

    public string ModuleName { get; private set; }
    public int HomeId { get; private set; }
    public int? GroupId { get; private set; }
    public int? RoomId { get; private set; }
    public string? FirmwareVersion { get; private set; }
    public string Ip { get; private set; }
    public string Mac { get; private set; }
    public int? Rssi { get; private set; }

    public bool ColorIsEnabled { get; set; }
    public bool TemperatureIsEnabled { get; set; }
    public bool BrightnessIsEnabled { get; set; }
    public bool SpeedIsEnabled { get; set; }
    public bool SceneIsEnabled { get; set; }

    public ObservableCollection<SavedScene> SavedScenes { get; private set; } = new();

    public AsyncRelayCommand NavigateBackCommand => new(NavigateBack);
    public AsyncRelayCommand OpenScenesCommand => new(OpenScenes);
    public AsyncRelayCommand AddSavedSceneCommand => new(AddSavedScene);
    public RelayCommand<SavedScene> RestoreSavedSceneCommand => new(RestoreSavedScene);
    public AsyncRelayCommand<SavedScene> RemoveSavedSceneCommand => new(RemoveSavedScene);

    public async Task Initialise(BulbHandle handle)
    {
        _bulbClient = new(handle);

        Connection = Connection.Searching;

        Name = handle.Name;

        ReflectState(await GetSystemConfigStateAsync());
        ReflectState(await GetPilotStateAsync());

        var settings = await _settingsDataAccess.LoadAsync();
        if(settings is not null)
        {
            SavedScenes.AddRange(settings.SavedScenes);
        }

        _stateListenerRepeater.Start();

        PropertyChanged += PropertyChangedHandler;
    }

    public override Task OnNavigatedFrom()
    {
        _sampleScreenRepeater?.Dispose();

        return base.OnNavigatedFrom();
    }

    public override async Task Suspend() => _stateListenerRepeater.Stop();
    public override async Task Resume() => _stateListenerRepeater.Start();

    private async Task NavigateBack()
    {
        var success = _navigationService.GoBack();
        if(!success)
        {
            await _navigationCommands.GoToBulbList(HomeId);
        }
    }

    private async Task OpenScenes()
    {
        await _dialogService.OpenDialog(new()
        {
            Content = new DialogViewContent
            {
                ViewType = ViewTypes.Scenes,
                Parameters = { { "ViewModel", this } },
            },
            Buttons =
            {
                new(0, "Close"),
            },
        });
    }

    private async Task AddSavedScene()
    {
        var item = new SavedScene
        {
            SceneId = SceneId,
            Color = Color,
            Temperature = Temperature,
            Brightness = Brightness,
            Speed = Speed,

            ColorIsEnabled = ColorIsEnabled,
            TemperatureIsEnabled = TemperatureIsEnabled,
            BrightnessIsEnabled = BrightnessIsEnabled,
            SpeedIsEnabled = SpeedIsEnabled,
        };

        if(SavedScenes.Contains(item)) return;

        SavedScenes.Insert(0, item);

        var settings = await _settingsDataAccess.LoadOrCreateAsync();
        settings.SavedScenes.Add(item);
        await _settingsDataAccess.SaveAsync(settings);
    }

    private void RestoreSavedScene(SavedScene savedScene)
    {
        SceneId = savedScene.SceneId;

        if(savedScene.ColorIsEnabled)
        {
            Color = savedScene.Color;
        }

        if(savedScene.TemperatureIsEnabled)
        {
            Temperature = savedScene.Temperature;
        }

        if(savedScene.BrightnessIsEnabled)
        {
            Brightness = savedScene.Brightness;
        }

        if(savedScene.SpeedIsEnabled)
        {
            Speed = savedScene.Speed;
        }
    }

    private async Task RemoveSavedScene(SavedScene savedScene)
    {
        SavedScenes.Remove(savedScene);

        var settings = await _settingsDataAccess.LoadOrCreateAsync();
        settings.SavedScenes.Remove(savedScene);
        await _settingsDataAccess.SaveAsync(settings);
    }

    private async void PropertyChangedHandler(object? sender, PropertyChangedEventArgs args)
    {
        var propertyName = args.PropertyName;

        var value = GetType().GetProperty(propertyName)?.GetValue(this);
        _logger.LogDebug("Property changed: {PropertyName}. New value: {Value}", propertyName, value);

        if(propertyName is nameof(Name))
        {
            await _bulbDataAccess.UpdateBulbNameAsync(_bulbClient.Handle.MacAddress, Name);
            return;
        }

        if(propertyName is nameof(Connection))
        {
            ReflectState(await GetSystemConfigStateAsync(), logState: true);
        }

        if(!_queueCommandsViaPropertyChange) return;

        if(propertyName is nameof(IsOn))
        {
            QueueCommand(new PowerCommand(IsOn));
            return;
        }

        if(propertyName is nameof(Color))
        {
            QueueCommand(new ColourCommand(new((int)Color.R, Color.G, Color.B)));
            return;
        }

        if(propertyName is nameof(Temperature))
        {
            QueueCommand(new TemperatureCommand(Temperature));
            return;
        }

        if(propertyName is nameof(Brightness))
        {
            QueueCommand(new BrightnessCommand(Brightness));
            return;
        }

        if(propertyName is nameof(SceneId))
        {
            var scene = Scene.FromIndex(SceneId);
            if(scene is null)
            {
                if(_bulbModules.FirstOrDefault(m => m.SceneId == SceneId) is IBulbModule module)
                {
                    module.Execute(this);
                }

                return;
            }

            //if(scene == Scene.CustomWhite)
            //{
            //    QueueCommand(new TemperatureCommand(Temperature));
            //    return;
            //}

            // Custom isn't a valid scene command,
            // send a command that will set the bulb to custom.
            if(scene == Scene.Custom)
            {
                QueueCommand(new ColourCommand(new((int)Color.R, Color.G, Color.B)));
                return;
            }

            QueueCommand(new SceneCommand(scene, Speed));
            return;
        }

        if(propertyName is nameof(Speed))
        {
            QueueCommand(new SpeedCommand(Speed));
            return;
        }
    }

    private void QueueCommand(BulbCommand command)
    {
        _commandProcessQueue.Enqueue(
            tag: command.GetType().Name,
            action: async () =>
            {
                await SendStateAsync(command);

                _queueCommandLock++;
                ReflectState(await GetPilotStateAsync());
                _queueCommandLock--;
            });
    }

    private async Task<BulbState?> SendStateAsync(BulbState state)
    {
        try
        {
            _logger.LogDebug("Send: {State}", state.ToJson());

            var response = await _bulbClient.SendStateAsync(state);

            Connection = Connection.Connected;

            return response;
        }
        catch(Exception ex)
        {
            Connection = Connection.NotFound;

            _logger.LogError(ex, "Send_Error: {Message}", ex.Message);
        }

        return default;
    }

    private void ReflectState(BulbState? state, bool logState = false)
    {
        if(state is null) return;
        if(state.Result is null) return;

        var result = state.Result;

        if(result.State is not null)
        {
            IsOn = result.State.Value;
        }

        TemperatureIsEnabled = result.Temp is not null;
        if(TemperatureIsEnabled)
        {
            Temperature = result.Temp!.Value;
        }

        BrightnessIsEnabled = result.Dimming is not null;
        if(BrightnessIsEnabled)
        {
            Brightness = result.Dimming!.Value;
        }

        SpeedIsEnabled = result.Speed is not null;
        if(SpeedIsEnabled)
        {
            Speed = result.Speed!.Value;
        }

        SceneIsEnabled = result.SceneId is not null;
        if(SceneIsEnabled)
        {
            SceneId = (int)result.SceneId;
        }

        ColorIsEnabled = result.R is not null;
        if(ColorIsEnabled)
        {
            Color = new Color()
            {
                R = (byte)result.R!,
                G = (byte)result.G!,
                B = (byte)result.B!,
            };
        }
        else
        {
            Color = Colors.White;
        }

        // Configuration
        if(result.ModuleName is not null) ModuleName = result.ModuleName;
        if(result.HomeId is not null) HomeId = result.HomeId ?? 0;
        if(result.GroupId is not null) GroupId = result.GroupId;
        if(result.RoomId is not null) RoomId = result.RoomId;
        if(result.FwVersion is not null) FirmwareVersion = result.FwVersion;
        if(result.PhoneIp is not null) Ip = result.PhoneIp;
        if(result.Mac is not null) Mac = result.Mac;
        if(result.Rssi is not null) Rssi = result.Rssi;

        if(!logState) return;

        _logger.LogDebug("Response: {State}", state.ToJson());
    }

    private async Task ListenToStateAsync()
    {
        if(_commandProcessQueue.HasRequests || _commandProcessQueue.IsHandlingRequest)
        {
            return;
        }

        _queueCommandLock++;
        ReflectState(await GetPilotStateAsync(), logState: true);
        _queueCommandLock--;
    }

    private async Task<BulbState?> GetSystemConfigStateAsync() => await SendStateAsync(new GetSystemConfigCommand());

    private async Task<BulbState?> GetPilotStateAsync() => await SendStateAsync(new GetPilotCommand());

    public void Dispose()
    {
        _stateListenerRepeater.Dispose();
    }
}
