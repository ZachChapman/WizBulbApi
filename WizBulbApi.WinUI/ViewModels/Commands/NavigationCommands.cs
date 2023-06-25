using MvvmFramework;

namespace WizBulbApi.WinUI;

public class NavigationCommands
{
    private readonly INavigationService _navigationService;
    private readonly IViewModelProvider _viewModelProvider;

    public NavigationCommands(INavigationService navigationService, IViewModelProvider viewModelProvider)
    {
        _navigationService = navigationService;
        _viewModelProvider = viewModelProvider;
    }

    public async Task<BulbListViewModel> GoToBulbList(int homeId, INavigationOptions? navigationOptions = default)
    {
        var viewModel = _viewModelProvider.Create<BulbListViewModel>();

        _navigationService.Navigate(
            typeof(BulbListView),
            viewModel,
            navigationOptions);

        await viewModel.Initialise(homeId);

        return viewModel;
    }

    public async Task<BulbViewModel> GoToBulb(BulbHandle handle, INavigationOptions? navigationOptions = default)
    {
        var viewModel = _viewModelProvider.Create<BulbViewModel>();

        _navigationService.Navigate(
            typeof(BulbView),
            viewModel,
            navigationOptions);

        await viewModel.Initialise(handle);

        return viewModel;
    }

    public async Task<LoginViewModel> GoToLogin(INavigationOptions? navigationOptions = default)
    {
        var viewModel = _viewModelProvider.Create<LoginViewModel>();

        _navigationService.Navigate(
            typeof(LoginView),
            viewModel,
            navigationOptions);

        await viewModel.Initialise();

        return viewModel;
    }

    public async Task<SettingsViewModel> GoToSettings(INavigationOptions? navigationOptions = default)
    {
        var viewModel = _viewModelProvider.Create<SettingsViewModel>();

        _navigationService.Navigate(
            typeof(SettingsView),
            viewModel,
            navigationOptions);

        await viewModel.Initialise();

        return viewModel;
    }
}
