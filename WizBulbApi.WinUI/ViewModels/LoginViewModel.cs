using Microsoft.Extensions.DependencyInjection;
using MvvmFramework;

namespace WizBulbApi.WinUI;

public class LoginViewModel : ViewModel
{
	private readonly INavigationService _navigationService;
	private readonly ISettingsDataAccess _settingsDataAccess;

	public LoginViewModel(INavigationService navigationService, ISettingsDataAccess settingsDataAccess)
	{
		_navigationService = navigationService;
		_settingsDataAccess = settingsDataAccess;
	}

	public static LoginViewModel Create()
	{
		var viewModel = App.Host.Services.GetRequiredService<LoginViewModel>();

		return viewModel;
	}

	public List<NetworkInfo> NetworkInterfaces { get; private set; } = new();
	public NetworkInfo NetworkInterface { get; set; }
	public int? HomeId { get; set; }

	public AsyncRelayCommand SubmitCommand => new(Submit);

	public override async Task Initialise()
	{
		NetworkInterfaces = NetworkHelper.GetNetworkInfo().ToList();

		var settings = await _settingsDataAccess.LoadAsync();
		if(settings is not null)
		{
			HomeId = settings.HomeId;
			NetworkInterface = NetworkInterfaces.FirstOrDefault(info => info.Id == settings.NetworkInterfaceId);
		}
	}

	public override async Task Validate()
	{
		ClearErrors();

		if(NetworkInterface is null)
		{
			AddError(NetworkInterface, "Please select a network interface.");
		}

		if(HomeId is null or 0 or int.MinValue)
		{
			AddError(HomeId, "Please enter a valid WiZ Home ID.");
		}
	}

	public async Task Submit()
	{
		await Validate();

		if(HasErrors)
		{
			return;
		}

		var homeId = HomeId!.Value;

		await _settingsDataAccess.SaveAsync(new()
		{
			HomeId = homeId,
			NetworkInterfaceId = NetworkInterface.Id
		});

		await _navigationService.GoToBulbList(homeId);
	}
}
