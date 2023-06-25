using Microsoft.Extensions.DependencyInjection;

namespace WizBulbApi.WinUI;

public class ViewModelProvider : IViewModelProvider
{
    private readonly IServiceProvider _serviceProvider;

    public ViewModelProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T Create<T>() where T : notnull
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}
