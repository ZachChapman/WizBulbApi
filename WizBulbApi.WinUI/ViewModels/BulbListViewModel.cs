using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Media.Animation;
using MvvmFramework;
using MvvmFramework.WinUI;
using System.Collections.ObjectModel;

namespace WizBulbApi.WinUI;

public class BulbListViewModel : ViewModel
{
	private readonly INavigationService _navigationService;
	private readonly IBulbDataAccess _bulbDataAccess;
	private readonly ISettingsDataAccess _settingsDataAccess;
	private int _homeId;

	public BulbListViewModel(
		INavigationService navigationService,
		IBulbDataAccess bulbDataAccess,
		ISettingsDataAccess settingsDataAccess)
	{
		_navigationService = navigationService;
		_bulbDataAccess = bulbDataAccess;
		_settingsDataAccess = settingsDataAccess;
	}

	public static BulbListViewModel Create(int homeId)
	{
		var viewModel = App.Host.Services.GetRequiredService<BulbListViewModel>();
		viewModel._homeId = homeId;

		return viewModel;
	}

	public ObservableCollection<BulbListItemViewModel> Bulbs { get; private set; } = new();
	public bool IsLoading { get; private set; }

	public AsyncRelayCommand ScanCommand => new(ScanAsync);
	public AsyncRelayCommand<BulbHandle> GoToBulbCommand => new(handle => _navigationService.GoToBulb(handle, new NavigationOptions(new DrillInNavigationTransitionInfo())));
	public AsyncRelayCommand GoToSettingsCommand => new(() => _navigationService.GoToSettings(new NavigationOptions(new DrillInNavigationTransitionInfo())));

	public override async Task Initialise()
	{
		try
		{
			IsLoading = true;

			var handles = await _bulbDataAccess.GetAllBulbHandlesAsync(_homeId);
			handles.ForEach(handle =>
			{
				var bulb = BulbListItemViewModel.Create(handle);
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

				var bulb = BulbListItemViewModel.Create(handle);
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
