using Microsoft.UI.Xaml.Media.Animation;
using MvvmFramework;
using MvvmFramework.WinUI;
using System.Collections.ObjectModel;

namespace WizBulbApi.WinUI;

public class BulbListViewModel : ViewModel
{
	private readonly IBulbDataAccess _bulbDataAccess;
	private readonly ISettingsDataAccess _settingsDataAccess;
    private readonly IViewModelProvider _viewModelProvider;
    private readonly NavigationCommands _navigationCommands;
    private int _homeId;

	public BulbListViewModel(
		IBulbDataAccess bulbDataAccess,
		ISettingsDataAccess settingsDataAccess,
		IViewModelProvider viewModelProvider,
		NavigationCommands navigationCommands)
	{
		_bulbDataAccess = bulbDataAccess;
		_settingsDataAccess = settingsDataAccess;
        _viewModelProvider = viewModelProvider;
        _navigationCommands = navigationCommands;
    }

	public ObservableCollection<BulbListItemViewModel> Bulbs { get; private set; } = new();
	public bool IsLoading { get; private set; }

	public AsyncRelayCommand ScanCommand => new(ScanAsync);
	public AsyncRelayCommand<BulbHandle> GoToBulbCommand => new(handle => _navigationCommands.GoToBulb(handle, new NavigationOptions(new DrillInNavigationTransitionInfo())));
	public AsyncRelayCommand GoToSettingsCommand => new(() => _navigationCommands.GoToSettings(new NavigationOptions(new DrillInNavigationTransitionInfo())));

	public async Task Initialise(int homeId)
	{
		_homeId = homeId;

		try
		{
			IsLoading = true;

			var handles = await _bulbDataAccess.GetAllBulbHandlesAsync(_homeId);
			handles.ForEach(async handle =>
			{
				var bulb = _viewModelProvider.Create<BulbListItemViewModel>();
				await bulb.Initialise(handle);
				Bulbs.Add(bulb);
			});
		}
		finally
		{
			IsLoading = false;
		}
	}

	private async Task ScanAsync()
	{
		if(IsLoading) return;

		try
		{
			IsLoading = true;

			using var scanner = await CreateBulbScanner();
			await scanner.FindBulbHandlesAsync(async handle =>
			{
				if(await _bulbDataAccess.ContainsAsync(handle.MacAddress))
				{
					return;
				}

				handle.Name = "New Light";
				await _bulbDataAccess.AddBulbHandleAsync(handle);

				var bulb = _viewModelProvider.Create<BulbListItemViewModel>();
				await bulb.Initialise(handle);
				Bulbs.Add(bulb);
			});
		}
		finally
		{
			IsLoading = false;
		}
	}

	private async Task<BulbScanner> CreateBulbScanner()
	{
		var settings = await _settingsDataAccess.LoadAsync();
		if(settings is null)
		{
			return new BulbScanner(_homeId);
		}
		else
		{
			var networkInfo =
				NetworkHelper.GetNetworkInfo().FirstOrDefault(info => info.Id == settings.NetworkInterfaceId)
				?? NetworkHelper.GetNetworkInfo().FirstOrDefault()
				?? throw new Exception("No networks detected");

			return new BulbScanner(_homeId, networkInfo.IpAddress, networkInfo.MacAddress);
		}
	}
}
