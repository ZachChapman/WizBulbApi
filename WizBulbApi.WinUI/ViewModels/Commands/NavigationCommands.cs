using MvvmFramework;

namespace WizBulbApi.WinUI;

public static class NavigationCommands
{
	public static async Task<BulbListViewModel> GoToBulbList(this INavigationService navigationService, int homeId, INavigationOptions? navigationOptions = default)
	{
		var viewModel = BulbListViewModel.Create(homeId);

		navigationService.Navigate(
			typeof(BulbListView),
			viewModel,
			navigationOptions);

		await viewModel.Initialise();

		return viewModel;
	}

	public static async Task<BulbViewModel> GoToBulb(this INavigationService navigationService, BulbHandle handle, INavigationOptions? navigationOptions = default)
	{
		var viewModel = BulbViewModel.Create(handle);

		navigationService.Navigate(
			typeof(BulbView),
			viewModel,
			navigationOptions);

		await viewModel.Initialise();

		return viewModel;
	}

	public static async Task<LoginViewModel> GoToLogin(this INavigationService navigationService, INavigationOptions? navigationOptions = default)
	{
		var viewModel = LoginViewModel.Create();

		navigationService.Navigate(
			typeof(LoginView),
			viewModel,
			navigationOptions);

		await viewModel.Initialise();

		return viewModel;
	}

	public static async Task<SettingsViewModel> GoToSettings(this INavigationService navigationService, INavigationOptions? navigationOptions = default)
	{
		var viewModel = SettingsViewModel.Create();

		navigationService.Navigate(
			typeof(SettingsView),
			viewModel,
			navigationOptions);

		await viewModel.Initialise();

		return viewModel;
	}
}
