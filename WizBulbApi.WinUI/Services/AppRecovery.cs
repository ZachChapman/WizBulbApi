using MvvmFramework;
using System.Reflection;

namespace WizBulbApi.WinUI;

public class AppRecovery
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IAppStateDataAccess _appStateDataAccess;
    private readonly INavigationService _navigationService;

    public AppRecovery(IServiceProvider serviceProvider, IAppStateDataAccess appStateDataAccess, INavigationService navigationService)
    {
        _serviceProvider = serviceProvider;
        _appStateDataAccess = appStateDataAccess;
        _navigationService = navigationService;
    }

    public async Task StoreState(AppState appState)
    {
        await _appStateDataAccess.SaveAsync(appState);
    }

    public async Task<bool> TryRestoreLastState()
    {
        var appState = await _appStateDataAccess.LoadAsync();
        if(appState is null)
        {
            return false;
        }

        if(appState.ViewTypeName is null)
        {
            return false;
        }

        var pageType =
            Assembly
            .GetEntryAssembly()
            .GetTypes()
            .Where(t => t.Name == appState.ViewTypeName)
            .FirstOrDefault();

        if(pageType is null)
        {
            throw new Exception($"{nameof(AppState)}.{nameof(AppState.ViewTypeName)} set to an invalid value. '{appState.ViewTypeName}'");
        }

        if(appState.ViewModelTypeName is null)
        {
            _navigationService.Navigate(pageType);
        }

        var viewModelType =
            Assembly
            .GetEntryAssembly()
            .GetTypes()
            .Where(t => t.Name == appState.ViewModelTypeName)
            .FirstOrDefault();

        if(viewModelType is null)
        {
            throw new Exception($"{nameof(AppState)}.{nameof(AppState.ViewModelTypeName)} set to an invalid value. '{appState.ViewModelTypeName}'");
        }

        var viewModel =
            _serviceProvider.GetService(viewModelType) as ViewModel
            ?? Activator.CreateInstance(viewModelType) as ViewModel;

        if(viewModel is not null)
        {
            await viewModel.RestoreState(appState.ViewModelState);
        }

        _navigationService.Navigate(pageType, viewModel);

        ClearState();

        return true;
    }

    public void ClearState()
    {
        _appStateDataAccess.Clear();
    }
}
